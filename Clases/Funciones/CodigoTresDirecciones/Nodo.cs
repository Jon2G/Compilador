using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My8086.Clases.Fases;

namespace My8086.Clases.Funciones.CodigoTresDirecciones
{
    internal class Nodo : TokenAritmetico
    {
        public TokenAritmetico Numero1 { get; private set; }
        public TokenAritmetico Numero2 { get; private set; }
        public TokenAritmetico Operador { get; private set; }
        public int NOrden { get; private set; }

        public Nodo() : base() { }
        public Nodo(int NOrden, TokenAritmetico Numero1, TokenAritmetico Operador, TokenAritmetico Numero2) : base(new Token($"T{NOrden}", Fases.TipoToken.Identificador, TipoDato.NoAplica, Numero1.Token.Linea), TipoToken.Temporal)
        {
            this.Numero1 = Numero1;
            this.Operador = Operador;
            this.Numero2 = Numero2;
            this.NOrden = NOrden;
        }
        public string NemonicoOperacion(string destino)
        {
            StringBuilder sb = new StringBuilder();
            switch (this.Operador.Token.Lexema)
            {
                case "+":
                    sb.AppendLine("CALL SUMA");
                    break;
                case "-":
                    sb.AppendLine("CALL RESTA");
                    break;
                case "/":
                    sb.AppendLine("CALL DIVIDE");
                    break;
                case "*":
                    sb.AppendLine("CALL MULTIPLICA");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //sb.AppendLine($"LEA DX,SIGNOT{orden}");
            sb.AppendLine($"LEA DX,SGN_{destino}");

            sb.AppendLine("CALL SALVAR_TEMPORAL");
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
