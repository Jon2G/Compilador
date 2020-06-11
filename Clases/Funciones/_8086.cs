using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My8086.Clases.Funciones
{
    public class _8086 : Accion, IBloque
    {
        public DocumentLine InicioBloque { get; set; }
        public DocumentLine FinBloque { get; set; }
        public _8086(Programa Programa) : base(Programa)
        {

        }

        public StringBuilder CerrarBloque()
        {
            throw new NotImplementedException();
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }

        public override StringBuilder Traduccion()
        {
            throw new NotImplementedException();
        }
    }
}
