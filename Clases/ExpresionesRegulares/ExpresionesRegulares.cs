using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace My8086.Clases.ExpresionesRegulares
{
    public class ExpresionesRegulares
    {
        public GroupCollection Grupos { get; private set; }
        public string this[string grupo] => this.Grupos[grupo].Value;

        internal ExpresionesRegulares()
        {


        }
        public Regex Cadena => new Regex(@"^(')(?<Cadena>(.)+)(')$", RegexOptions.Compiled);
        public Regex Identificador => new Regex(@"^(?<Lexema>(([a-zA-Z]+[0-9a-zA-Z]*|\w)+))$", RegexOptions.Compiled);
        public Regex PalabrasReservadas => new Regex(@"^(Nom\.(([a-zA-Z]+[0-9a-zA-Z]*|\w)+)|Program|Vars|Entero|Decimal|Cadena|Si|begin|end|Begin|End|LimpiarPantalla|_8086)$", RegexOptions.Compiled);
        public Regex Enteros => new Regex(@"^(\+|\-)?((\d)+)$", RegexOptions.Compiled);
        public Regex Decimales => new Regex(@"^(\+|\-)?(\d+.\d+)$", RegexOptions.Compiled);
        public Regex OperadoresAritmeticos => new Regex(@"^(\+|\-|:=)$", RegexOptions.Compiled);
        public Regex SaltoLinea => new Regex(@"(\\n)", RegexOptions.Compiled);
        public Regex OperadoresLogicos => new Regex(@"^(<>|<|>|=|and|or)$", RegexOptions.Compiled);
        public Regex Comentarios => new Regex(@"\/\/[\s\S]*$", RegexOptions.Compiled);
        public Regex Documento => new Regex(@"(,|\s|\n|;|:=|=|\+\s|\-\s|\*|\/|<>|<|>|\)|\()(?=(?:[^\']*\'[^\']*\')*[^\']*$)", RegexOptions.Compiled);
        public bool Evaluar(Regex Expresion, string Texto)
        {
            var match = Expresion.Match(Texto);
            this.Grupos = match.Groups;
            return match.Success;
        }
    }
}
