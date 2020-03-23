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
            this.IdentifiacadorSalto = (++ConsecutivoSalto).ToString();
            // this.IdentifiacadorSalto = Guid.NewGuid().ToString().Replace("-", "");
            this.OperacionLogica = OperacionLogica;
            OperacionLogica.DeclararTemporales();
        }
        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }

        public override StringBuilder Traduccion()
        {
            if (this.Sino != null)
            {
                this.Sino.IdentifiacadorSalto = this.IdentifiacadorSalto;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(";");
            sb.Append(this.OperacionLogica.GenerarAsm());
            sb.AppendLine("MOV AL,1H");
            sb.AppendLine("CMP R_COMPARADOR,AL");
            sb.AppendLine($"JE IF_VERDADERO_{this.IdentifiacadorSalto}");
            if (this.Sino != null)
            {
                sb.AppendLine($"JMP INICIO_SINO_{this.IdentifiacadorSalto}");
            }
            else
            {
                sb.AppendLine($"JMP IF_FALSO_{this.IdentifiacadorSalto}");
            }
            sb.AppendLine($"IF_VERDADERO_{this.IdentifiacadorSalto}:");
            sb.AppendLine(";ACCIONES VERDADERAS :)");
            return sb;
        }

        StringBuilder IBloque.CerrarBloque()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"IF_FALSO_{this.IdentifiacadorSalto}:");
            sb.AppendLine(";ACCIONES FALSAS :(");
            return sb;
        }
    }
}
