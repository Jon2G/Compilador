using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;
using My8086.Clases.Fases._2._Analisis_Sintactico;
using My8086.Clases.Fases.Base;

namespace My8086.Clases.AutoCompletar
{
    public class AutoCompletado
    {
        private TextDocument Documento;
        private readonly TextArea Texto;
        public CompletionWindow CompletionWindow { get; private set; }
        private ResultadosCompilacion Errores;
        private Analizador Analizador;
        public bool Analizando { get; private set; }
        private Programa Programa;
        public AutoCompletado(TextArea Texto, ResultadosCompilacion Errores)
        {
            this.Texto = Texto;
            this.Documento = this.Texto.Document;
            this.Errores = Errores;
            //this.Sugerencias = new List<Sugerencia>()
            //{
            //    new Sugerencia("Consola")
            //        .Agregar("Imprimir();", "Imprime un elemento en pantalla.", TipoSugerencia.Metodo)
            //        .Agregar("LimpiarPantalla();","Limpia el contenido de la pantalla",TipoSugerencia.Metodo)
            //};
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
        public void AutoCompletar()
        {
            DocumentLine linea = this.Documento.GetLineByNumber(this.Texto.Caret.Line);
            string contexto = this.Documento.GetText(linea);
            // open code completion after the user has pressed dot:
            this.CompletionWindow = new CompletionWindow(this.Texto);
            // provide AvalonEdit with the data:
            AutoCompletar(contexto, this.CompletionWindow.CompletionList.CompletionData);
            this.CompletionWindow.Show();
            this.CompletionWindow.Closed += delegate
            {
                this.CompletionWindow = null;
            };
        }

        private void AutoCompletar(string contexto, IList<ICompletionData> data)
        {
            if (this.Programa is null) { return; }
            contexto = contexto.Substring(0, contexto.Length - 1).Trim();

            if (contexto.EndsWith("Imprime") || contexto.EndsWith("Lee"))
            {
                foreach (Funciones.Variable variable in this.Programa.SegmentoDeDatos.Variables.Where(x => !x.EsAutomatica))
                {
                    data.Add(new MyCompletionData(variable.Nombre, variable.Nombre + ");", $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png"));
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
                            data.Add(new MyCompletionData(variable.Nombre +comparacion, variable.Nombre + comparacion, $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png"));
                        }
                    }
                    else
                    {
                        if (variable.TipoDato == TipoDato.Cadena)
                        {
                            data.Add(new MyCompletionData(variable.Nombre + "=", variable.Nombre + "=", $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png"));
                        }
                    }

                }
            }
        }
    }
}
