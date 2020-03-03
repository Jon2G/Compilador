using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    class SI : Accion
    {
        public SI(Funcion Fx, LineaLexica Linea, int InicioArgumentos) : base(Fx, Linea, InicioArgumentos)
        {
            
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            throw new NotImplementedException();
        }

        public override StringBuilder Traduccion(Funcion Fx)
        {
            throw new NotImplementedException();
        }
    }
}
