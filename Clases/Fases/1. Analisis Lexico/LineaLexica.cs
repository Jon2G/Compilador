using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Funciones.OperacionLogica;

namespace My8086.Clases.Fases._1._Analisis_Lexico
{
    public class LineaLexica
    {
        public List<Token> Tokens { get; private set; }
        public int Elementos => Tokens.Count;
        public Token this[int index] => Tokens[index];
        public DocumentLine LineaDocumento { get; private set; }
        public LineaLexica(DocumentLine LineaDocumento)//inicializa con una lista vacia
        {
            this.Tokens = new List<Token>();
            this.LineaDocumento = LineaDocumento;
        }
        internal void Agregar(Token token)
        {
            this.Tokens.Add(token);
        }
        internal bool Remover(Token token)
        {
            return this.Tokens.Remove(token);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            this.Tokens.ForEach(tk =>
            {
                sb.Append(tk.Lexema);
                sb.Append(" ");
            });
            return sb.ToString();
        }
    }
}
