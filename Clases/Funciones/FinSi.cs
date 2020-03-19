using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    class FinSi:Accion
    {
        public FinSi(Programa Programa, LineaLexica Linea, int InicioArgumentos) : base(Programa, Linea, InicioArgumentos)
        {
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            throw new NotImplementedException();
        }

        public override StringBuilder Traduccion()
        {
            throw new NotImplementedException();
        }
    }
}
