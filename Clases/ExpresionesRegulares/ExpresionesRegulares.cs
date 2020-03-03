using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace My8086.Clases.ExpresionesRegulares
{
    public class ExpresionesRegulares
    {
        private readonly Dictionary<string, Regex> Diccionario = new Dictionary<string, Regex>();
        private GroupCollection _Grupos;
        public GroupCollection Grupos => _Grupos;
        public string this[string grupo] => this._Grupos[grupo].Value;

        internal ExpresionesRegulares()
        {
            this.Diccionario.Add("DeclaracionCadena", new Regex(
                @"(Def\\s*Cadena)+\\s*((?<Lexema>(([a-zA-Z]+[0-9a-zA-Z]|_*)*)))\\s*=\\s*""((?<Cadena>(\w||\s||\\x||\\)+))"";"
                , RegexOptions.Compiled, TimeSpan.FromSeconds(10)));

            this.Diccionario.Add("EsImpresionDeConsola", new Regex(
                @"Consola.Imprimir\((""(?<Cadena>(\w||\s||\\x||\\)+)""||(?<Lexema>(([a-zA-Z]+[0-9a-zA-Z]||\w)*)))\);"
                , RegexOptions.Compiled, TimeSpan.FromSeconds(10)));

                        this.Diccionario.Add("EsImpresionDeConsolaXY", new Regex(
                @"Consola.Imprimir\((((""(?<Cadena>(\w||\s||\\x||\\)+)"")||((?<Lexema>(([a-zA-Z]+[0-9a-zA-Z]||\w)*))))+(,(?<x>(\d)+),(?<y>(\d)+)))\);"
                , RegexOptions.Compiled, TimeSpan.FromSeconds(10)));
            //
            //

        }
        public bool Evaluar(string Id, string texto)
        {
            Match match = this.Diccionario[Id].Match(texto);
            if (match.Success)
            {
                this._Grupos = match.Groups;
                return true;
            }
            this._Grupos = null;
            return false;
        }

        //public static Regex Construir(string contexto, params Regex[] expresiones)
        //{

        //    //StringBuilder sb = new StringBuilder(contexto);
        //    //for (int i = 0; i < expresiones.Length; i++)
        //    //{
        //    //    sb = sb.Replace($"{{{i}}}", expresiones[i].ToString());
        //    //}
        //    //Regex regex=new Regex(sb.ToString());
        //    Regex regex = new Regex(string.Format(contexto, expresiones), RegexOptions.ECMAScript);
        //    Clipboard.SetText(regex.ToString());
        //    return regex;
        //}
        public static Regex Cadena => new Regex(@"^("")(?<Cadena>(.)+)("")$", RegexOptions.Compiled);
        public static Regex Identificador => new Regex(@"^(?<Lexema>(([a-zA-Z]+[0-9a-zA-Z]*|\\w)+))$", RegexOptions.Compiled);
        public static Regex Enteros => new Regex(@"^(\+|\-)?((\d)+)$", RegexOptions.Compiled);
        public static Regex Decimales => new Regex(@"^((\d).(\d)+)$", RegexOptions.Compiled);
        public static Regex Signos => new Regex(@"^(\+||\-)$", RegexOptions.Compiled);
        public static Regex Comentarios => new Regex(@"\/\/[\s\S]*$", RegexOptions.Compiled);
        public static Regex Documento => new Regex(@"(,|\s|\n)(?=(?:[^\""]*\""[^\""]*\"")*[^\""]*$)", RegexOptions.Compiled);
        public static bool Evaluar(Regex Expresion, string Texto)
        {
            return Expresion.IsMatch(Texto);
        }
    }
}
