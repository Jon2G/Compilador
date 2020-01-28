using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My8086.Clases.Compilador
{
    public class Accion
    {
        public enum Acciones
        {
            ImprimirPorConsola,
            LimpiarConsola,
            ImprimirPorConsolaXY
        }

        public Acciones EjecutarAccion { get; private set; }
        public object[] Argumentos { get; private set; }
        public Accion(Acciones EjecutarAccion, params object[] Argumentos)
        {
            this.EjecutarAccion = EjecutarAccion;
            this.Argumentos = Argumentos;
        }
    }
}
