using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases._1._Analisis_Lexico;
using My8086.Clases.Fases._2._Analisis_Sintactico;
using My8086.Clases.Fases._3._Analisis_Semantico;
using My8086.Clases.Fases._4._Sintetizador;

namespace My8086.Clases.Compilador
{
    public class Compilador
    {
        private readonly ResultadosCompilacion ResultadosCompilacion;
        public List<Advertencias.WarningError> Resultados => ResultadosCompilacion.Resultados;
        private readonly TextDocument Document;
        public EventHandler OnProgreso;
        private EventHandler _VerLinea;
        private BorlandC TurboC;
        public bool Compilado { get; set; }

        public EventHandler VerLinea
        {
            get => _VerLinea;
            set
            {
                _VerLinea = value;
                this.ResultadosCompilacion.SetVerLinea(_VerLinea);
            }
        }
        private double _Progreso { get; set; }

        public double Progreso
        {
            get => _Progreso;
            set
            {
                _Progreso = value;
                OnProgreso?.Invoke(this, null);
            }
        }

        public Compilador(TextDocument Document)
        {
            this.Document = Document;

            this.ResultadosCompilacion = new ResultadosCompilacion(this.VerLinea);
            this.TurboC = new BorlandC();
            //this.ReconceTokens = new ReconoceTokens(this.ResultadosCompilacion, this.PropiedadesPrograma);
        }



        public string _Compilar()
        {
            this.Progreso = 0;
            this.ResultadosCompilacion.Clear();
            try
            {
                Convertir();
                if (!this.ResultadosCompilacion.SinErrores)
                {
                    this.Compilado = false;
                    return "Se encontrarón errores previos a la compilación\n";
                }

                //TraductorAsm traductor = new TraductorAsm(this.PropiedadesPrograma);
                //string documento = traductor.ObtenerAsm();
                //Clipboard.SetText(documento);
                //if (this.Borland.GeneraEjecutable(documento))
                //{
                //    this.Compilado = true;
                //}

                //return this.Borland.ResultadosCompilacion;
            }
            catch (RegexMatchTimeoutException Rex)
            {
                this.ResultadosCompilacion.ResultadoCompilacion(Rex.Pattern + Environment.NewLine + "->" + Rex.Input, null);
                this.Compilado = false;
            }
            catch (Exception e)
            {
                this.Compilado = false;
            }
            return "Error desconocido,la compilación no se realizo\n";
        }
        public string Compilar()
        {

            this.Progreso = 0;
            this.ResultadosCompilacion.Clear();
            //try
            //{
            //Fase 1 Analisis Lexico
            Fases.Base.Analizador analizador
                 = new AnalizadorLexico(this.Document, this.ResultadosCompilacion);
            analizador.Analizar();
            if (analizador.EsValido)
            {
                //Fase 2 Analisis Sintactico
                analizador =
                    new AnalizadorSintactico((AnalizadorLexico)analizador, this.Document, this.ResultadosCompilacion);
                analizador.Analizar();
                if (analizador.EsValido)
                {
                    //Fase 3 Analisis Semantico
                    analizador =
                        new AnalizadorSemantico((AnalizadorSintactico)analizador, this.Document, this.ResultadosCompilacion);
                    analizador.Analizar();
                    if (analizador.EsValido)
                    {
                        //Fase 4 Sintetizador
                        analizador =
                            new Sintesis((AnalizadorSemantico)analizador, this.Document, this.ResultadosCompilacion);
                        analizador.Analizar();
                        if (analizador.EsValido)
                        {
                            CodigoIntermedio intermedio = new CodigoIntermedio((Sintesis)analizador);
                            intermedio.Generar();
                            this.TurboC.Limpiar();
                            this.Compilado = this.TurboC.GeneraEjecutable(intermedio.Codigo.ToString());
                            string ASM = intermedio.Codigo.ToString();

                            return this.TurboC.ResultadosCompilacion;
                        }
                    }
                }
            }
            this.Compilado = false;
            return "Se encontrarón errores previos a la compilación\n";
        }

        public string Ejecutar()
        {
            if (this.Compilado)
            {
               this.TurboC.Ejecutar();
               return this.TurboC.ResultadosCompilacion;
            }

            return "No se ha compilado el archivo";
        }
        private void Convertir()
        {
            //int lineas = this.Document.LineCount;
            //foreach (DocumentLine linea in this.Document.Lines)
            //{
            //    this.Progreso = (linea.LineNumber / (double)lineas) * 100;
            //    string texto = Document.GetText(linea)?.Trim();
            //    if (string.IsNullOrEmpty(texto))
            //    {
            //        continue;
            //    }

            //    if (texto.StartsWith("//"))
            //    {
            //        continue;
            //    }
            //    if (!texto.EndsWith(";"))
            //    {
            //        if (this.ReconceTokens.EsIncio(texto, linea.LineNumber))
            //        {
            //            continue;
            //        }
            //        if (this.ReconceTokens.EsFin(texto, linea.LineNumber))
            //        {
            //            continue;
            //        }

            //        this.ResultadosCompilacion.ResultadoCompilacion("Se esperaba ';' al final de la instrucción.", linea.LineNumber);
            //        continue;
            //    }

            //    if (this.PropiedadesPrograma.Titulo is null)
            //    {
            //        this.ResultadosCompilacion.ResultadoCompilacion("FATAL No se definio un punto de entrada para la aplicación", linea.LineNumber);
            //        break;
            //    }
            //    if (this.ReconceTokens.EsDeclaracionDeVariable(texto, linea.LineNumber))
            //    {
            //        continue;
            //    }
            //    if (this.ReconceTokens.EsUnaFuncionDeConsola(texto, linea.LineNumber))
            //    {
            //        continue;
            //    }
            //    this.ResultadosCompilacion.ResultadoCompilacion("Sentencia no recononocida...", linea.LineNumber);
            //    break;
            //}
            //if (!this.PropiedadesPrograma.Cerrado)
            //{
            //    this.ResultadosCompilacion.ResultadoCompilacion($"Se esperaba el cierre de la función principal '{this.PropiedadesPrograma.Titulo}' y no se encontro", -1);
            //}
        }
    }
}
