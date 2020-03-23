using My8086.Clases.Fases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My8086.Clases.Funciones.OperacionLogica
{
    internal class Nodo : TokenLogico
    {
        public TokenLogico Numero1 { get; private set; }
        public TokenLogico Numero2 { get; private set; }
        public TokenLogico Operador { get; private set; }
        public int NOrden { get; private set; }
        public TipoDato TipoDato { get; private set; }
        public Nodo() : base() { }
        public Nodo(int NOrden, TokenLogico Numero1, TokenLogico Operador, TokenLogico Numero2, TipoDato TipoDato) : base(new Token($"T{NOrden}", Fases.TipoToken.Identificador, TipoDato.NoAplica, Numero1.Token.Linea), TipoToken.Temporal)
        {
            this.Numero1 = Numero1;
            this.Operador = Operador;
            this.Numero2 = Numero2;
            this.NOrden = NOrden;
            this.TipoDato = TipoDato;
        }
        public string NemonicoOperacion(string destino,TipoDato TipoDato)
        {
            StringBuilder sb = new StringBuilder();
            switch (this.Operador.Token.Lexema)
            {
                case "<=":
                    sb.AppendLine("CALL MENOR_QUE_IGUAL");
                    break;
                case ">=":
                    sb.AppendLine("CALL MAYOR_QUE_IGUAL");
                    break;
                case "=":
                    if (TipoDato == TipoDato.Cadena)
                    {
                        sb.AppendLine("CALL IGUAL_CADENA");
                    }
                    else
                    {
                        sb.AppendLine("CALL IGUAL");
                    }
                    break;
                case "<":
                    sb.AppendLine("CALL MENOR_QUE");
                    break;
                case ">":
                    sb.AppendLine("CALL MAYOR_QUE");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //sb.AppendLine($"LEA DX,SIGNOT{orden}");
            sb.AppendLine("MOV AL,R_COMPARADOR");
            sb.AppendLine($"MOV {destino},AL");
            return sb.ToString();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("T");
            sb.Append(NOrden);
            sb.Append(" = ");
            if (Numero1.TipoToken == TipoToken.NumeroNegativo)
            {
                sb.Append("-");
            }
            sb.Append(Numero1.Token.Lexema);
            sb.Append(this.Operador.Token.Lexema);
            if (Numero2.TipoToken == TipoToken.NumeroNegativo)
            {
                sb.Append("-");
            }
            sb.Append(Numero2.Token.Lexema);
            return sb.ToString();
        }
    }
}
