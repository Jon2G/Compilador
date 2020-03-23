using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;

namespace My8086.Clases.Fases.Base
{
    internal abstract class Analizador
    {
        protected readonly TextDocument Documento;
        public bool EsValido { get; protected set; }
        protected readonly ResultadosCompilacion Errores;
        protected Analizador(TextDocument Documento, ResultadosCompilacion Errores)
        {
            this.Documento = Documento;
            this.EsValido = false;
            this.Errores = Errores;
        }
        public abstract void Analizar();
        public virtual void Analizar(List<Tuple<string, DocumentLine>> Lineas)
        {

        }
    }
}
