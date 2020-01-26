using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using My8086.Clases.Advertencias;

namespace My8086.Clases.Compilador
{
    public class Compilador
    {
        private readonly TextDocument document;
        public readonly List<Advertencias.WarningError> ResultadosCompilacion;
        private readonly PropiedadesPrograma PropiedadesPrograma;
        public EventHandler OnProgreso;
        private readonly ReconoceTokens ReconceTokens;
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
            this.ResultadosCompilacion = new List<WarningError>();

            this.PropiedadesPrograma = new PropiedadesPrograma();
            this.ReconceTokens = new ReconoceTokens(this.ResultadosCompilacion, this.PropiedadesPrograma);
        }

        public bool Compilar()
        {
            bool resultado = false;
            this.Progreso = 0;
            this.ResultadosCompilacion.Clear();

            try
            {
                Convertir();
                TraductorAsm traductor = new TraductorAsm(this.PropiedadesPrograma);
                string documento = traductor.ObtenerAsm();
            }
            catch (Exception)
            {
                return false;
            }

            resultado = true;
            return resultado;
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
                if (this.ReconceTokens.EsDeclaracionDeVariable(texto, linea.LineNumber))
                {
                    continue;
                }
                if (this.ReconceTokens.EsUnaFuncionDeConsola(texto, linea.LineNumber))
                {
                    continue;
                }
                if (this.ReconceTokens.EsIncio(texto, linea.LineNumber))
                {
                    continue;
                }
                if (this.ReconceTokens.EsFin(texto, linea.LineNumber))
                {
                    continue;
                }

                ErrorCompilacion error = new ErrorCompilacion(false,
                    "Sentencia no recononocida...", linea.LineNumber);
                this.ResultadosCompilacion.Add(new WarningError(error));
                throw error;
            }
        }

        /*


;
;-----------------------------------------------------------------------------------------------------------

resultado_str      db 44h dup(0),"$"                                        ;
posicion_pila      dw 0000h                                                 ;
;VARIABLES PARA LEER LA ENTRADA
posicion_entrada   dw 0000h                                                 ;
;-----------------------------------------------------------------------------------------------------------   
renglones_pila     db 00h                                                   ;cuenta los renglones de la pila de operaciones
aux                db 00h                                                   ;variable auxiliar
registros_tbl      dw 0000h,0000h,0000h,0000h 
 */
    }
}
