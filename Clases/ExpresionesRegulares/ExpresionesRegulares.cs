using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace My8086.Clases.ExpresionesRegulares
{
    public class ExpresionesRegulares
    {
        private GroupCollection _Grupos;
        public GroupCollection Grupos => _Grupos;
        public string this[string grupo] => this._Grupos[grupo].Value;

        internal ExpresionesRegulares()
        {


        }
        public Regex Cadena => new Regex(@"^(')(?<Cadena>(.)+)(')$", RegexOptions.Compiled);
        public Regex Identificador => new Regex(@"^(?<Lexema>(([a-zA-Z]+[0-9a-zA-Z]*|\\w)+))$", RegexOptions.Compiled);
        public Regex PalabrasReservadas => new Regex(@"^(Nom.programa|Program|Vars|Entero|Decimal|Cadena|Si|begin|end|Begin|End)$", RegexOptions.Compiled);
        public Regex Enteros => new Regex(@"^(\+|\-)?((\d)+)$", RegexOptions.Compiled);
        public Regex Decimales => new Regex(@"^((\d).(\d)+)$", RegexOptions.Compiled);
        public Regex OperadoresAritmeticos => new Regex(@"^(\+|\-|:=)$", RegexOptions.Compiled);
        public Regex SaltoLinea => new Regex(@"(\\n)", RegexOptions.Compiled);
        public Regex OperadoresLogicos => new Regex(@"^(<>|<|>|=|and|or)$", RegexOptions.Compiled);
        public Regex Comentarios => new Regex(@"\/\/[\s\S]*$", RegexOptions.Compiled);
        // public  Regex Documento => new Regex(@"(,|\s|\n|;|:=|=|\+|<>|<|>)(?=(?:[^\""]*\""[^\""]*\"")*[^\""]*$)", RegexOptions.Compiled);
        public Regex Documento => new Regex(@"(,|\s|\n|;|:=|=|\+|\-|\*|\/|<>|<|>|\)|\()(?=(?:[^\']*\'[^\']*\')*[^\']*$)", RegexOptions.Compiled);
        public bool Evaluar(Regex Expresion, string Texto)
        {
            var match = Expresion.Match(Texto);
            this._Grupos = match.Groups;
            return match.Success;
        }
    }
}
