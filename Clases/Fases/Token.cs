using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using My8086.Clases.Advertencias;
using My8086.Clases.Funciones;
using My8086.Clases.ExpresionesRegulares;
namespace My8086.Clases.Fases
{
    public enum TipoToken
    {
        OperadorLogico, OperadorAritmetico,
        Identificador, Relacional, SeparadorParametros,
        PalabraReservada, Invalido, FinInstruccion,
        ParentesisAbierto, ParentesCerrado, Cadena, NumeroEntero, NumeroDecimal,
        Ensamblador
    }
    public enum TipoDato
    {
        Entero, Decimal, Cadena,BitLogico, Invalido, NoAplica, Ensamblador
    }
    public class Token
    {
        public TipoToken TipoToken { get;  set; }//el tipo de token
        public TipoDato TipoDato { get; set; }//el tipo de dato
        public DocumentLine Linea { get; private set; }//a que linea pertenece
        public List<DocumentLine> LineasDeReferencia { get; private set; }//referencia de la linea 
        public int Referencias { get => LineasDeReferencia?.Count ?? 0; }// conteo de referencias que tiene
        public string Lexema { get; internal set; }// la palabra
        public bool EsValido => (TipoToken != TipoToken.Invalido && TipoDato != TipoDato.Invalido);//verificar si es valido
        public Token(string lexema, TipoToken TipoToken, TipoDato TipoDato, DocumentLine Linea)//token,dato,referencia
        {
            this.TipoDato = TipoDato;//el tipo de dato
            this.TipoToken = TipoToken;//el tipo de token
            this.Linea = Linea;//a que linea pertenece
            this.LineasDeReferencia = new List<DocumentLine>();//referencia de la linea 
            this.Lexema = lexema;//texto
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

        public static Token Identificar(string Token, DocumentLine Linea, ExpresionesRegulares.ExpresionesRegulares Patrones)//
        {
            Fases.Token token = new Token(Token, TipoToken.Invalido, TipoDato.Invalido, Linea);///secrea el token con estado invalido
            if (Patrones.Evaluar(Patrones.PalabrasReservadas, Token))//copara con los patrones de la expreciones 
            {
                token.TipoToken = TipoToken.PalabraReservada;
                token.TipoDato = TipoDato.NoAplica;
                return token;
            }
            //
            if (Patrones.Evaluar(Patrones.Enteros, Token))//copara con enteros
            {
                token.TipoToken = TipoToken.NumeroEntero;
                token.TipoDato = TipoDato.Entero;
                return token;
            }
            if (Patrones.Evaluar(Patrones.Decimales, Token))//copara con decimales 
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
            }//comparaciones directas 
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
            if (Token == "+"|| Token == "*"||Token == "-"||Token == "/")// expresion regular
            {
                token.TipoToken = TipoToken.OperadorAritmetico;
                token.TipoDato = TipoDato.NoAplica;
                return token;
            }

            return token;
        }
    }
}
