using System.Collections.Generic;
using System.Drawing;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using static My8086.Clases.AutoCompletar.Sugerencia;

namespace My8086.Clases.AutoCompletar
{
    public class AutoCompletado
    {
        private readonly TextDocument Documento;
        private readonly TextArea Texto;
        private readonly IList<Sugerencia> Sugerencias;
        public CompletionWindow CompletionWindow { get; private set; }
        public AutoCompletado(TextArea Texto)
        {
            this.Texto = Texto;
            this.Documento = this.Texto.Document;
            this.Sugerencias = new List<Sugerencia>()
            {
                new Sugerencia("Consola")
                    .Agregar("Imprimir();", "Imprime un elemento en pantalla.", TipoSugerencia.Metodo)
                    .Agregar("LimpiarPantalla();","Limpia el contenido de la pantalla",TipoSugerencia.Metodo)
            };
        }

        public void AutoCompletar()
        {
            DocumentLine linea = this.Documento.GetLineByNumber(this.Texto.Caret.Line);
            string contexto = this.Documento.GetText(linea);
            // open code completion after the user has pressed dot:
            this.CompletionWindow = new CompletionWindow(this.Texto);
            // provide AvalonEdit with the data:
            AutoCompletar(contexto, this.CompletionWindow.CompletionList.CompletionData);
            this.CompletionWindow.Show();
            this.CompletionWindow.Closed += delegate
            {
                this.CompletionWindow = null;
            };
        }

        private void AutoCompletar(string contexto, IList<ICompletionData> data)
        {
            contexto = contexto.Substring(0, contexto.Length - 1);
            foreach (Sugerencia sug in this.Sugerencias)
            {
                if (contexto.EndsWith(sug.Token))
                {
                    foreach (MyCompletionData autocompletado in sug.Sugerencias)
                    {
                        data.Add(autocompletado);
                    }
                }
            }
            //data.Add(new MyCompletionData("Item1"));
            //data.Add(new MyCompletionData("Item2"));
            //data.Add(new MyCompletionData("Item3"));
            //data.Add(new MyCompletionData("Another item"));
        }
    }
}
