using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using My8086.Clases.Fases;

namespace My8086.Clases.Funciones.CodigoTresDirecciones
{
    public enum TipoToken { NumeroPositivo, NumeroNegativo, OperadorPrioritario, OperadorSecundario, Temporal }
    internal class TokenAritmetico
    {
        public bool EsNumero => (TipoToken == TipoToken.NumeroPositivo || TipoToken == TipoToken.NumeroNegativo);
        public TipoToken TipoToken { get; private set; }
        public Token Token { get; private set; }
        protected TokenAritmetico() { }
        public TokenAritmetico(Token Token, TipoToken TipoToken)
        {
            this.Token = Token;
            this.TipoToken = TipoToken;
        }
        public static List<TokenAritmetico> Analizar(List<Token> Tks,Programa programa)
        {
            List<TokenAritmetico> tokens = new List<TokenAritmetico>();

            foreach (Token tk in Tks)
            {
                if (tk.TipoToken == Fases.TipoToken.FinInstruccion)
                {
                    continue;
                }
                if (double.TryParse(tk.Lexema, out double n))
                {
                    if (tokens.LastOrDefault()?.EsNumero ?? false)
                    {
                        tokens.Add(new TokenAritmetico(
                            new Token("+", Fases.TipoToken.OperadorAritmetico, TipoDato.NoAplica, tk.Linea), TipoToken.OperadorSecundario));
                    }
                    tokens.Add(new TokenAritmetico(new Token(tk.Lexema.Replace("-", ""), Fases.TipoToken.Identificador, TipoDato.Decimal, tk.Linea),
                        n >= 0 ? CodigoTresDirecciones.TipoToken.NumeroPositivo :
                            CodigoTresDirecciones.TipoToken.NumeroNegativo));
                    continue;
                }
                if (tk.Lexema.Length == 1)
                {
                    char simbolo = tk.Lexema.First();
                    if (simbolo == '+' || simbolo == '-')
                    {
                        if (simbolo == '+')
                        {
                            programa.UsarSuma = true;
                        }
                        if (simbolo == '-')
                        {
                            programa.UsarResta = true;
                        }
                        tokens.Add(new TokenAritmetico(tk, TipoToken.OperadorSecundario));
                    }
                    if (simbolo == '*' || simbolo == '/')
                    {
                        if (simbolo == '*')
                        {
                            programa.UsarMultiplicacion = true;
                        }
                        if (simbolo == '/')
                        {
                            programa.UsarDivision = true;
                        }
                        tokens.Add(new TokenAritmetico(tk, TipoToken.OperadorPrioritario));
                    }
                    continue;
                }
                tokens.Add(new TokenAritmetico(tk, TipoToken.Temporal));
            }

            return tokens;
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
