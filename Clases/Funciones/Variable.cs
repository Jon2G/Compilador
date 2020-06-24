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
        public readonly TipoDato TipoDato;
        public int Referencias { get; private set; }
        public bool EsAutomatica { get;  set; }
        internal Variable(Programa Programa,string Nombre,TipoDato TipoDato) : base(Programa)
        {
            this.TipoDato = TipoDato;
            this.Nombre = Nombre;
        }
        internal Variable(Programa Programa, LineaLexica Linea, TipoDato TipoDato) : base(Programa, Linea, 0)
        {
            this.Nombre = this.Argumentos[1].Lexema;
            this.TipoDato = TipoDato;
            if (this.Argumentos[2].TipoToken == TipoToken.FinInstruccion)
            {
                return;
            }
            else
            {
                this.Valor = this.Argumentos[1];
            }
        }
        internal Variable(Programa Programa, Token Valor, LineaLexica Linea, TipoDato TipoDato) : base(Programa, Linea, 0)
        {
            this.Nombre = "AUTOVAR" + Guid.NewGuid().ToString("N");
            this.Valor = Valor;
            this.TipoDato = TipoDato;
            this.EsAutomatica = true;
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            switch (this.TipoDato)
            {
                case TipoDato.Decimal:
                case TipoDato.Entero:
                    sb.AppendLine($"\n;=======>[{Nombre}-{this.TipoDato}]");
                    sb.AppendLine($"SGN_{Nombre} db 0");
                    sb.AppendLine($"NUM_{Nombre} db 0,0,0,0,0,0,0,0,0");
                    sb.AppendLine($"DEC_{Nombre} db 0,0,0,0,0,0,0,0,0 ");
                    break;
                case TipoDato.Cadena:
                    sb.Append($"\n;=======>[{Nombre}-{this.TipoDato}]\n");
                    sb.Append(this.Nombre);
                    sb.AppendLine(" DW 00");
                    sb.AppendLine($"LONGUITUD_{Nombre} DW 0");
                    break;
                case TipoDato.BitLogico:
                    sb.Append($"\n;=======>[{Nombre}-{this.TipoDato}]\n");
                    sb.Append(this.Nombre);
                    sb.AppendLine(" DB 0");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return sb;
        }
        public void HacerReferencia() => this.Referencias++;
    }
}
