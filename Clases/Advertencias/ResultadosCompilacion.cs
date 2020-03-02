using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Document;

namespace My8086.Clases.Advertencias
{
    public class ResultadosCompilacion
    {
        public readonly List<Advertencias.WarningError> Resultados;
        private EventHandler VerLinea;
        public bool SinErrores { get; private set; }

        public ResultadosCompilacion(EventHandler VerLinea)
        {
            this.Resultados = new List<WarningError>();
            this.VerLinea = VerLinea;
        }
        public void ResultadoCompilacion(string Texto, DocumentLine Linea, bool EsAdvertencia = false)
        {
            if (!EsAdvertencia)
            {
                this.SinErrores = false;
            }
            this.Resultados.Add(new WarningError(new ErrorCompilacion(EsAdvertencia, Texto, Linea),
                this.VerLinea));
        }

        public void Clear()
        {
            this.SinErrores = true;
            this.Resultados.Clear();
        }

        internal void SetVerLinea(EventHandler VerLinea)
        {
            this.VerLinea = VerLinea;
        }

        public void VariableNoDeclarada(string argumento, DocumentLine linea)
        {
            StringBuilder sb = new StringBuilder("Uso de variable no declarada '");
            sb.Append(argumento);
            sb.Append("'");
            this.ResultadoCompilacion(sb.ToString(), linea);
        }
    }
}
