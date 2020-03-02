using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using My8086.Clases.BaseDeDatos;

namespace My8086.Clases
{
    public static class AppData
    {
        public static string Directorio => Environment.CurrentDirectory;
        private static SQLH _SQLH;

        public static SQLH SQLH => _SQLH??=new SQLH();
    }
}
