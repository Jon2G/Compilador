using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My8086.Clases.Advertencias
{
    public class ResultadosCompilacion
    {
        public readonly List<Advertencias.WarningError> Resultados;
        private readonly EventHandler VerLinea;
        public bool SinErrores => this.Resultados?.Any(x => !x.Excepcion.EsAdvertencia) ?? true;

        public ResultadosCompilacion(EventHandler VerLinea)
        {
            this.Resultados = new List<WarningError>();
            this.VerLinea = VerLinea;
        }
        public void ResultadoCompilacion(string Texto, int Linea = -1, bool EsAdvertencia = false)
        {
            this.Resultados.Add(new WarningError(new ErrorCompilacion(EsAdvertencia, Texto, Linea),
                this.VerLinea));
        }

        public void Clear()
        {
            this.Resultados.Clear();
        }
    }
}
