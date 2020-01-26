using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My8086.Clases.Advertencias
{
    public class ErrorCompilacion : Exception
    {
        public bool EsAdvertencia { get; internal set; }
        public string Texto { get; internal set; }
        public int Linea { get; internal set; }

        public ErrorCompilacion(bool EsAdvertencia, string Texto, int Linea):base(Texto)
        {
            this.EsAdvertencia = EsAdvertencia;
            this.Texto = Texto;
            this.Linea = Linea;

        }
    }
}
