using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using My8086.Clases.Advertencias;

namespace My8086.Clases.Compilador
{
    public class Compilador
    {
        private ResultadosCompilacion ResultadosCompilacion;
        public List<Advertencias.WarningError> Resultados => ResultadosCompilacion.Resultados;
        private readonly TextDocument document;
        private readonly PropiedadesPrograma PropiedadesPrograma;
        public EventHandler OnProgreso;
        private readonly ReconoceTokens ReconceTokens;
        private EventHandler _VerLinea;

        public bool Compilado { get; set; }

        private readonly BorlandC Borland;
        public EventHandler VerLinea
        {
            get => _VerLinea;
            set
            {
                _VerLinea = value;
                this.ResultadosCompilacion = new ResultadosCompilacion(_VerLinea);
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

        public Compilador(TextDocument document)
        {
            this.document = document;
            this.PropiedadesPrograma = new PropiedadesPrograma();
            this.ResultadosCompilacion = new ResultadosCompilacion(this.VerLinea);
            this.ReconceTokens = new ReconoceTokens(this.ResultadosCompilacion, this.PropiedadesPrograma);
            this.Borland = new BorlandC();
        }



        public string Compilar()
        {

            this.Progreso = 0;
            this.ResultadosCompilacion.Clear();
            try
            {
                Convertir();
                if (this.ResultadosCompilacion.SinErrores)
                {
                    this.Compilado = false;
                    return "Se encontrarón errores previos a la compilación\n";
                }

                TraductorAsm traductor = new TraductorAsm(this.PropiedadesPrograma);
                string documento = traductor.ObtenerAsm();
                Clipboard.SetText(documento);
                if (this.Borland.GeneraEjecutable(documento))
                {
                    this.Compilado = true;
                }

                return this.Borland.ResultadosCompilacion;
            }
            catch (Exception e)
            {
                this.Compilado = false;
            }

            return "Error desconocido,la compilación no se realizo\n";

        }

        public string Ejecutar()
        {
            if (this.Compilado)
            {
                this.Borland.Ejecutar();
                return this.Borland.ResultadosCompilacion;
            }

            return "No se ha compilado el archivo";
        }
        private void Convertir()
        {
            int lineas = this.document.LineCount;
            foreach (DocumentLine linea in this.document.Lines)
            {
                this.Progreso = (linea.LineNumber / (double)lineas) * 100;
                string texto = document.GetText(linea)?.Trim();
                if (string.IsNullOrEmpty(texto))
                {
                    continue;
                }

                if (texto.StartsWith("//"))
                {
                    continue;
                }
                if (!texto.EndsWith(";"))
                {
                    if (this.ReconceTokens.EsIncio(texto, linea.LineNumber))
                    {
                        continue;
                    }
                    if (this.ReconceTokens.EsFin(texto, linea.LineNumber))
                    {
                        continue;
                    }

                    this.ResultadosCompilacion.ResultadoCompilacion("Se esperaba ';' al final de la instrucción.", linea.LineNumber);
                    continue;
                }

                if (this.PropiedadesPrograma.Titulo is null)
                {
                    this.ResultadosCompilacion.ResultadoCompilacion("FATAL No se definio un punto de entrada para la aplicación", linea.LineNumber);
                    break;
                }
                if (this.ReconceTokens.EsDeclaracionDeVariable(texto, linea.LineNumber))
                {
                    continue;
                }
                if (this.ReconceTokens.EsUnaFuncionDeConsola(texto, linea.LineNumber))
                {
                    continue;
                }
                this.ResultadosCompilacion.ResultadoCompilacion("Sentencia no recononocida...", linea.LineNumber);
                break;
            }
            if (!this.PropiedadesPrograma.Cerrado)
            {
                this.ResultadosCompilacion.ResultadoCompilacion($"Se esperaba el cierre de la función principal '{this.PropiedadesPrograma.Titulo}' y no se encontro", -1);
            }
        }
    }
}
