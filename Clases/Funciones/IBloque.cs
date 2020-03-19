using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;

namespace My8086.Clases.Funciones
{
    interface IBloque
    {
        DocumentLine FinBloque { get; set; }
        DocumentLine InicioBloque { get; set; }
        StringBuilder CerrarBloque();
    }
}
