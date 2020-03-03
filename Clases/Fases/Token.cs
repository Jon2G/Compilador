using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using My8086.Clases.Advertencias;
using My8086.Clases.BaseDeDatos;
using My8086.Clases.Funciones;
using static My8086.Clases.ExpresionesRegulares.ExpresionesRegulares;
namespace My8086.Clases.Fases
{
    public enum TipoToken
    {
        OperadorLogico, OperadorAritmetico,
        Identificador, Relacional, SeparadorParametros,
        PalabraReservada, Invalido
    }
    public enum TipoDato
    {
        Entero, Decimal, Cadena, 
        Exponencial, Invalido
    }
    public class Token
    {
        public TipoToken TipoToken { get; private set; }
        public TipoDato TipoDato { get; private set; }
        public Guid Id { get; private set; }
        public DocumentLine Linea { get; private set; }
        public List<DocumentLine> LineasDeReferencia { get; private set; }
        public int Referencias { get => LineasDeReferencia?.Count ?? 0; }
        public string Lexema { get; private set; }
        public bool EsValido => (TipoToken != TipoToken.Invalido && TipoDato != TipoDato.Invalido);
        public Token(string lexema, TipoToken TipoToken, TipoDato TipoDato, DocumentLine Linea)
        {
            this.TipoDato = TipoDato;
            this.TipoToken = TipoToken;
            this.Id = Guid.NewGuid();
            this.Linea = Linea;
            this.LineasDeReferencia = new List<DocumentLine>();
            this.Lexema = lexema;
        }
        internal string ObtenerTipoDato(Funcion Fx)
        {
            switch (this.TipoDato)
            {
                case TipoDato.Entero:
                    return "int";
            }

            if (this.TipoDato == TipoDato.Cadena && this.TipoToken == TipoToken.Identificador)
            {
                Variable var = Fx.Variables.First(x => x.Nombre == this.Lexema);
                return var.Valor.ObtenerTipoDato(Fx);
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(this.Lexema);
            sb.Append("] - ");
            sb.Append(this.TipoToken);
            sb.Append(" - ");
            sb.Append(this.TipoDato);
            return sb.ToString();
        }

        public static Token Identificar(string Token, DocumentLine Linea, ResultadosCompilacion Errores)
        {
            Fases.Token token = new Token(Token, TipoToken.Invalido,TipoDato.Invalido, Linea);
            //Especiales
            if (Token == "++")
            {
                token.TipoToken = TipoToken.OperadorAritmetico;
                token.TipoDato = TipoDato.Entero;
                return token;
            }
            //
            if (EsPalabraReservada(Token))
            {
                token.TipoToken = TipoToken.PalabraReservada;
                token.TipoDato = TipoDato.Cadena;
                return token;
            }
            //
            if (Evaluar(Enteros, Token))
            {
                token.TipoToken = TipoToken.Identificador;
                token.TipoDato = TipoDato.Entero;
                return token;
            }
            if (Evaluar(Decimales, Token))
            {
                token.TipoToken = TipoToken.Identificador;
                token.TipoDato = TipoDato.Decimal;
                return token;
            }
            if (Evaluar(Cadena, Token))
            {
                token.TipoToken = TipoToken.Identificador;
                token.TipoDato = TipoDato.Cadena;
                return token;
            }
            if (Evaluar(ExpresionesRegulares.ExpresionesRegulares.Identificador, Token))
            {
                token.TipoToken = TipoToken.Identificador;
                token.TipoDato = TipoDato.Cadena;
                return token;
            }
            if (Token == ",")
            {
                token.TipoToken = TipoToken.SeparadorParametros;
                token.TipoDato = TipoDato.Cadena;
                return token;
            }

            if (Token == "=")
            {
                token.TipoToken = TipoToken.Relacional;
                token.TipoDato = TipoDato.Cadena;
                return token;
            }

            if (Token == "==")
            {
                token.TipoToken = TipoToken.OperadorLogico;
                token.TipoDato = TipoDato.Cadena;
                return token;
            }
            if (Token == "+")
            {
                token.TipoToken = TipoToken.OperadorAritmetico;
                token.TipoDato = TipoDato.Cadena;
                return token;
            }

            return token;
        }

        private static bool EsPalabraReservada(string Token)
        {
            return AppData.SQLH.Exists("SELECT *FROM PALABRAS_RESERVADAS WHERE TOKEN=@TOKEN",
                new SQLiteParameter("TOKEN", Token));
        }
    }
}
