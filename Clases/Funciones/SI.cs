using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    class Si : Accion, IBloque
    {
        private static int ConsecutivoSalto = 0;
        public string IdentifiacadorSalto { get; private set; }
        public DocumentLine InicioBloque { get; set; }
        public DocumentLine FinBloque { get; set; }
        public readonly OperacionesLogicas OperacionLogica;
        public Sino Sino { get; set; }
        public Si(Programa Programa, LineaLexica Linea, OperacionesLogicas OperacionLogica) : base(Programa, Linea, 0)
        {
<<<<<<< HEAD
            this.IdentifiacadorSalto = (++ConsecutivoSalto).ToString();//berifica sino hay un identificador igual y aumentar uno
            // this.IdentifiacadorSalto = Guid.NewGuid().ToString().Replace("-", "");
=======
            this.IdentifiacadorSalto = (++ConsecutivoSalto).ToString();
>>>>>>> e53720558099cb75543900398773c99c73bc02fb
            this.OperacionLogica = OperacionLogica;
            OperacionLogica.DeclararTemporales();
        }
        public override bool RevisarSemantica(ResultadosCompilacion Errores)//ya n berifica la semantica porque ya paso por varios filtros
        {
            return true;
        }

        public override StringBuilder Traduccion()
        {
            if (this.Sino != null)
            {
<<<<<<< HEAD
                this.Sino.IdentifiacadorSalto = this.IdentifiacadorSalto;//asigna el mismo identificador al sino en caso que lo haya
=======
                //En caso de que haya un sino se relacionan con el mismo identificador
                this.Sino.IdentifiacadorSalto = this.IdentifiacadorSalto;
>>>>>>> e53720558099cb75543900398773c99c73bc02fb
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(";");
            sb.Append(this.OperacionLogica.GenerarAsm());
<<<<<<< HEAD
            sb.AppendLine("MOV AL,1H");//asigna 1 a al
            sb.AppendLine("CMP R_COMPARADOR,AL"); //compara al con r_comparador
            sb.AppendLine($"JE IF_VERDADERO_{this.IdentifiacadorSalto}");//si es verdadera la condicon entonces salta a la etiqueta 
            if (this.Sino != null)//berifica si hay un sino
=======
            sb.AppendLine("MOV AL,1H");
            sb.AppendLine("CMP R_COMPARADOR,AL");
            //R_COMPARADOR SIEMPRE ES EL RESULTADO DE LA UTLIMA COMPARACION
            sb.AppendLine($"JE IF_VERDADERO_{this.IdentifiacadorSalto}");
            //SI ES IGUAL A AL(1) SALTA A IF_VERDARERO_N
            if (this.Sino != null)
>>>>>>> e53720558099cb75543900398773c99c73bc02fb
            {
                sb.AppendLine($"JMP INICIO_SINO_{this.IdentifiacadorSalto}");//si lo hay salta a la etiqueta
            }
            else
            {
                sb.AppendLine($"JMP IF_FALSO_{this.IdentifiacadorSalto}");// sino es asi pasa al else 
            }
            sb.AppendLine($"IF_VERDADERO_{this.IdentifiacadorSalto}:");//ejecuta las acciones se es verdadero
            sb.AppendLine(";ACCIONES VERDADERAS :)");
            return sb;
        }

        StringBuilder IBloque.CerrarBloque()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"IF_FALSO_{this.IdentifiacadorSalto}:");//cierra el bloque y ejecuta las acciones del else
            sb.AppendLine(";ACCIONES FALSAS :(");
            return sb;
        }
    }
}
