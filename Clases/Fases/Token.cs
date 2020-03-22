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
using My8086.Clases.ExpresionesRegulares;
namespace My8086.Clases.Fases
{
    public enum TipoToken
    {
        OperadorLogico, OperadorAritmetico,
        Identificador, Relacional, SeparadorParametros,
        PalabraReservada, Invalido, FinInstruccion,
        ParentesisAbierto, ParentesCerrado, Cadena, NumeroEntero, NumeroDecimal
    }
    public enum TipoDato
    {
        Entero, Decimal, Cadena,BitLogico, Invalido, NoAplica
    }
    public class Token
    {
        public TipoToken TipoToken { get; private set; }
        public TipoDato TipoDato { get; set; }
        public Guid Id { get; private set; }
        public DocumentLine Linea { get; private set; }
        public List<DocumentLine> LineasDeReferencia { get; private set; }
        public int Referencias { get => LineasDeReferencia?.Count ?? 0; }
        public string Lexema { get; internal set; }
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
        protected Token() { }
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

        public static Token Identificar(string Token, DocumentLine Linea, ResultadosCompilacion Errores, ExpresionesRegulares.ExpresionesRegulares Patrones)
        {
            Fases.Token token = new Token(Token, TipoToken.Invalido, TipoDato.Invalido, Linea);
            if (Patrones.Evaluar(Patrones.PalabrasReservadas, Token))
            {
                token.TipoToken = TipoToken.PalabraReservada;
                token.TipoDato = TipoDato.NoAplica;
                return token;
            }
            //
            if (Patrones.Evaluar(Patrones.Enteros, Token))
            {
                token.TipoToken = TipoToken.NumeroEntero;
                token.TipoDato = TipoDato.Entero;
                return token;
            }
            if (Patrones.Evaluar(Patrones.Decimales, Token))
            {
                token.TipoToken = TipoToken.NumeroDecimal;
                token.TipoDato = TipoDato.Decimal;
                return token;
            }
            if (Patrones.Evaluar(Patrones.Cadena, Token))
            {
                token.TipoToken = TipoToken.Cadena;
                token.TipoDato = TipoDato.Cadena;
                token.Lexema = Patrones["Cadena"];
                return token;
            }
            if (Patrones.Evaluar(Patrones.Identificador, Token))
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
            if (Token == ";")
            {
                token.TipoToken = TipoToken.FinInstruccion;
                token.TipoDato = TipoDato.NoAplica;
                return token;
            }

            if (Token == ":=")
            {
                token.TipoToken = TipoToken.Relacional;
                token.TipoDato = TipoDato.NoAplica;
                return token;
            }

            if (Patrones.Evaluar(Patrones.OperadoresLogicos, Token))
            {
                token.TipoToken = TipoToken.OperadorLogico;
                token.TipoDato = TipoDato.NoAplica;
                return token;
            }
            if (Token == "(")
            {
                token.TipoToken = TipoToken.ParentesisAbierto;
                token.TipoDato = TipoDato.NoAplica;
                return token;
            }
            if (Token == ")")
            {
                token.TipoToken = TipoToken.ParentesCerrado;
                token.TipoDato = TipoDato.NoAplica;
                return token;
            }
            if (Token == "+"|| Token == "*"||Token == "-"||Token == "/")
            {
                token.TipoToken = TipoToken.OperadorAritmetico;
                token.TipoDato = TipoDato.NoAplica;
                return token;
            }

            return token;
        }
    }
}
