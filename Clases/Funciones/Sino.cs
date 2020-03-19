using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    class Sino : Accion, IBloque
    {
        public DocumentLine InicioBloque { get; set; }
        public DocumentLine FinBloque { get; set; }


        public Sino(Programa Programa, LineaLexica Linea) : base(Programa, Linea, 0)
        {

        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }

        public override StringBuilder Traduccion()
        {
            throw new NotImplementedException();
        }

        public StringBuilder CerrarBloque()
        {
            throw new NotImplementedException();
        }
    }
}
