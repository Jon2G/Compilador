using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My8086.Clases.Compilador
{
    public class Variable
    {
        public enum TipoDato
        {
            Byte, Word, Cadena
        }

        public TipoDato Tipo { get; private set; }
        public object Valor { get; private set; }
        public string Identificador { get; private set; }
        public Variable(string Identificador, object Valor, TipoDato Tipo)
        {
            this.Valor = Valor;
            this.Tipo = Tipo;
            this.Identificador = Identificador;
        }
    }
}
