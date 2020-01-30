using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace My8086.Clases
{
    class ExpresionesRegulares
    {
        Regex regex;
        ExpresionesRegulares()
        {

        }
        public Regex Cadena => new Regex(@"(\w\s[^$,""])*");
        public Regex Identificador => new Regex(@"[^\d](\w\d)+");
        public Regex Enteros => new Regex(@"(\d)+");
        public Regex Decimales => new Regex(@"(\d).(\d)+");
        public Regex Signos => new Regex(@"(+|-)");
    }
}
