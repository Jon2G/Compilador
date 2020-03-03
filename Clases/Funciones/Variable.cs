using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    public class Variable : Accion
    {
        public readonly string Nombre;
        public readonly Token Valor;
        public int Referencias { get; private set; }

        internal Variable(Funcion Fx, LineaLexica Linea) : base(Fx, Linea, 1)
        {
            this.Nombre = this.Argumentos[0].Lexema;
            this.Valor = this.Argumentos[2];
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }

        public override StringBuilder Traduccion(Funcion Fx)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Valor.ObtenerTipoDato(Fx));
            sb.Append(" ");
            sb.Append(Nombre);
            sb.Append(" = ");
            sb.Append(Valor.Lexema);
            sb.AppendLine(";");
            return sb;
        }
        public void HacerReferencia() => this.Referencias++;
    }
}
