using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;

namespace My8086.Clases.Fases.Base
{
    internal abstract class Analizador
    {
        protected readonly TextDocument Documento;//reporta errores regresa verdadero o falso si fue exitosos o no
        public bool EsValido { get; protected set; }//ver si fue exitoso
        protected readonly ResultadosCompilacion Errores;//guardar errores
        protected Analizador(TextDocument Documento, ResultadosCompilacion Errores)
        {
            this.Documento = Documento;
            this.EsValido = false;
            this.Errores = Errores;
        }
        public abstract void Analizar();//se define si es valido o no
        public virtual void Analizar(List<Tuple<string, DocumentLine>> Lineas)
        {

        }
    }
}
