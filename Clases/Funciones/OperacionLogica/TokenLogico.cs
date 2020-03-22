using My8086.Clases.Fases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My8086.Clases.Funciones.OperacionLogica
{
    internal enum TipoToken 
    { NumeroPositivo, NumeroNegativo, VariableNumerica, 
      Cadena, OperadorPrioritario, OperadorSecundario,
      Temporal,OperadorLogico }
    internal class TokenLogico
    {
        public bool EsNumero => (TipoToken == TipoToken.NumeroPositivo || TipoToken == TipoToken.NumeroNegativo || TipoToken == TipoToken.VariableNumerica);

        public Token Token { get; private set; }
        public TipoToken TipoToken { get; private set; }
        public TokenLogico()
        {

        }
        public TokenLogico(Token Token, TipoToken TipoToken)
        {
            this.Token = Token;
            this.TipoToken = TipoToken;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<");
            if (this.TipoToken == TipoToken.NumeroNegativo)
            {
                sb.Append("-");
            }
            sb.Append(this.Token.Lexema);
            sb.Append("> [");
            sb.Append(this.TipoToken);
            sb.Append(" ]");
            return sb.ToString();
        }
    }
}
