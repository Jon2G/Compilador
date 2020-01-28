using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace My8086.Clases.Compilador
{
    public class BorlandC
    {
        private readonly FileInfo RutaBorland;
        private readonly FileInfo RutaEjecutable;

        public BorlandC()
        {

        }
        public bool GeneraEjecutable(string documento)
        {
            string nombre = Guid.NewGuid().ToString();
            return true;
        }

        internal void Ejecutar()
        {
            try
            {
                Process.Start(this.RutaEjecutable.FullName);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
