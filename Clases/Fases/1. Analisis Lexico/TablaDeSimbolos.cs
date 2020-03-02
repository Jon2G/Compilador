using System.Collections.Generic;

namespace My8086.Clases.Fases._1._Analisis_Lexico
{
    internal class TablaDeSimbolos
    {
        private readonly List<Token> Simbolos;
        public Token this[int posicion] => this.Simbolos[posicion];
        public int Elementos => Simbolos.Count;

        public TablaDeSimbolos()
        {
            this.Simbolos = new List<Token>();
        }
        public void Agregar(Token token)
        {
            this.Simbolos.Add(token);
        }


    }
}
