using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.UI;

namespace My8086.Clases.Advertencias
{
    public class ResultadosCompilacion 
    {
        private List<ErrorCompilacion> _Resultados;
        public List<ErrorCompilacion> Resultados
        {
            get => _Resultados;
            set
            {
                _Resultados = value;
            }
        }

        public bool SinErrores { get; private set; }

        public ResultadosCompilacion()
        {
            this.Resultados = new List<ErrorCompilacion>();
        }
        public void ResultadoCompilacion(string Texto, DocumentLine Linea, bool EsAdvertencia = false)
        {
            if (!EsAdvertencia)
            {
                this.SinErrores = false;
            }
            this.Resultados.Add(new ErrorCompilacion(EsAdvertencia, Texto, Linea));
        }

        public void Clear()
        {
            this.SinErrores = true;
            this.Resultados = new List<ErrorCompilacion>();
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
