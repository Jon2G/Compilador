using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;
using My8086.Clases.Fases._2._Analisis_Sintactico;
using My8086.Clases.Fases.Base;
using My8086.Clases.UI;

namespace My8086.Clases.AutoCompletar
{
    public class AutoCompletado
    {
        private readonly TextDocument Documento;
        private readonly TextArea Texto;
        public CompletionWindow CompletionWindow { get; private set; }
        private readonly ResultadosCompilacion Errores;
        private Analizador Analizador;
        public bool Analizando { get; private set; }
        private Programa Programa;
        ITextMarkerService TextMarkerService;
        public readonly List<string> TiposDatos;
        public readonly List<string> Funciones;
        public AutoCompletado(TextArea Texto, ResultadosCompilacion Errores)
        {
            this.TiposDatos = new List<string>() { "Entero", "Cadena", "Decimal" };
            this.Funciones = new List<string>() { "Imprime", "Lee", "LimpiarPantalla" };
            this.Texto = Texto;
            this.Documento = this.Texto.Document;
            this.Errores = Errores;
            IniciarMarcadoDeErrores();
        }
        private void IniciarMarcadoDeErrores()
        {
            var textMarkerService = new TextMarkerService(Texto.Document);
            Texto.TextView.BackgroundRenderers.Add(textMarkerService);
            Texto.TextView.LineTransformers.Add(textMarkerService);
            IServiceContainer services = (IServiceContainer)Texto.Document.ServiceProvider.GetService(typeof(IServiceContainer));
            if (services != null)
                services.AddService(typeof(ITextMarkerService), textMarkerService);
            this.TextMarkerService = textMarkerService;
        }
        public void Analizar()
        {
            this.Analizando = true;
            if (this.Documento.TextLength > 0)
            {
                var lineas = this.Documento.Lines.Select(x => new Tuple<string, DocumentLine>(this.Documento.GetText(x), x)).ToList();
                Thread th = new Thread(() =>
             {
                 this.Errores.Resultados = new List<ErrorCompilacion>();
                 try
                 {
                     this.Analizador =
                         new Fases._1._Analisis_Lexico.AnalizadorLexico(this.Documento, this.Errores);
                     this.Analizador.Analizar(lineas);
                     if (this.Analizador.EsValido)
                     {

                         this.Analizador =
                             new AnalizadorSintactico((AnalizadorLexico)this.Analizador, this.Documento, this.Errores);
                         this.Analizador.Analizar();
                         Programa = ((AnalizadorSintactico)this.Analizador).Programa;
                     }

                     Application.Current?.Dispatcher?.Invoke(() =>
                     {
                         TextMarkerService.RemoveAll(m => true);
                         foreach (var ex in this.Errores.Resultados.Where(x => x.Linea != null && !x.Linea.IsDeleted && x.Linea.LineNumber >= 0))
                         {
                             if (ex.EsAdvertencia)
                             {
                                 ResaltarError(ex.Linea, System.Windows.Media.Colors.Olive);
                             }
                             else
                             {
                                 ResaltarError(ex.Linea, System.Windows.Media.Colors.Red);
                             }
                         }
                     });
                 }
                 catch (Exception) { }
                 this.Analizando = false;
             });
                th.SetApartmentState(ApartmentState.STA);
                th.Priority = ThreadPriority.Lowest;
                th.Start();

            }
            else { this.Analizando = false; }

        }
        private void ResaltarError(DocumentLine Linea, System.Windows.Media.Color Color)
        {
            ITextMarker marker = this.TextMarkerService.Create(Linea.Offset, Linea.Length);
            marker.MarkerTypes = TextMarkerTypes.SquigglyUnderline;
            marker.MarkerColor = Color;
        }
        private CompletionWindow InicializarVentana()
        {
            // open code completion after the user has pressed dot:
            this.CompletionWindow = new CompletionWindow(this.Texto)
            {
                MinWidth = 200
            };
            this.CompletionWindow.Closed += delegate
            {
                this.CompletionWindow = null;
            };
            return this.CompletionWindow;
        }
        public void AutoCompletar()
        {
            DocumentLine linea = this.Documento.GetLineByNumber(this.Texto.Caret.Line);
            string contexto = this.Documento.GetText(linea);
            // open code completion after the user has pressed dot:
            InicializarVentana();
            AutoCompletar(contexto, this.CompletionWindow.CompletionList.CompletionData);
            if (CompletionWindow.CompletionList.CompletionData.Any())
            {
                this.CompletionWindow.Show();
            }
        }
        public void AutoCompletarPalabrasReservadas()
        {
            DocumentLine linea = this.Documento.GetLineByNumber(this.Texto.Caret.Line);
            string contexto = this.Documento.GetText(linea).Trim();
            if (string.IsNullOrEmpty(contexto))
            {
                return;
            }
            InicializarVentana();
            // provide AvalonEdit with the data:
            AutoCompletarTiposDeDatos(contexto, this.CompletionWindow.CompletionList.CompletionData);
            if (!CompletionWindow.CompletionList.CompletionData.Any())
            {
                AutoCompletarFunciones(contexto, this.CompletionWindow.CompletionList.CompletionData);
            }
            if (CompletionWindow.CompletionList.CompletionData.Any())
            {
                this.CompletionWindow.Show();
            }
        }
        private void AutoCompletar(string contexto, IList<ICompletionData> data)
        {
            if (this.Programa is null) { return; }
            if (
                contexto.EndsWith("*") ||
                contexto.EndsWith("/") ||
                contexto.EndsWith("+") ||
                contexto.EndsWith("-") ||
                contexto.EndsWith(":=")
                )
            {
                foreach (Funciones.Variable variable in this.Programa.SegmentoDeDatos.Variables.Where(x => !x.EsAutomatica && x.TipoDato == TipoDato.Decimal || x.TipoDato == TipoDato.Entero))
                {
                    data.Add(new MyCompletionData(variable.Nombre, variable.Nombre, $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png", this));
                }
                return;
            }
            if (contexto.Length > 2)
            {
                contexto = contexto.Trim();
                contexto = contexto.Substring(0, contexto.Length - 1);
                contexto = contexto.Trim();
            }
            if (contexto.EndsWith("Imprime") || contexto.EndsWith("Lee"))
            {
                foreach (Funciones.Variable variable in this.Programa.SegmentoDeDatos.Variables.Where(x => !x.EsAutomatica))
                {
                    data.Add(new MyCompletionData(variable.Nombre, variable.Nombre + ");", $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png", this));
                }
            }
            if (contexto.EndsWith("Si"))
            {
                foreach (Funciones.Variable variable in this.Programa.SegmentoDeDatos.Variables.Where(x => !x.EsAutomatica))
                {
                    if (variable.TipoDato == TipoDato.Entero || variable.TipoDato == TipoDato.Decimal)
                    {
                        foreach (string comparacion in new string[] { ":=", ">", "<", ">=", "<=" })
                        {
                            data.Add(new MyCompletionData(variable.Nombre + comparacion, variable.Nombre + comparacion, $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png", this));
                        }
                    }
                    else
                    {
                        if (variable.TipoDato == TipoDato.Cadena)
                        {
                            data.Add(new MyCompletionData(variable.Nombre + "=", variable.Nombre + "=", $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png", this));
                        }
                    }

                }
            }
        }
        private void AutoCompletarTiposDeDatos(string contexto, IList<ICompletionData> data)
        {
            contexto = contexto.ToLower();
            string tipo = this.TiposDatos.FirstOrDefault(x => x.StartsWith(contexto, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(tipo))
            {
                Regex regex = new Regex(Regex.Escape(contexto), RegexOptions.IgnoreCase);
                data.Add(new MyCompletionData(tipo, regex.Replace(tipo, "", 1) + " ", $"Tipo de dato:{tipo}", 0, "Imgs\\AddVariable_16x.png", this));
            }

        }
        private void AutoCompletarFunciones(string contexto, IList<ICompletionData> data)
        {
            contexto = contexto.ToLower();
            foreach (string tipo in this.Funciones.Where(x => x.StartsWith(contexto, StringComparison.OrdinalIgnoreCase)))
            {
                string complemento = tipo == "LimpiarPantalla" ? "();" : "(";
                Regex regex = new Regex(Regex.Escape(contexto), RegexOptions.IgnoreCase);
                data.Add(new MyCompletionData(tipo, regex.Replace(tipo, "", 1) + complemento, $"Función :{tipo}", 0, "Imgs\\Method_left_16x.png", this));
            }
        }
    }
}
