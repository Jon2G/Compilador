using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    class OperacionArtimetica:Accion
    {
        private readonly Variable Variable;
        public OperacionArtimetica(Funcion Fx, Variable Variable, LineaLexica Linea) : base(Fx, Linea, 1)
        {
            this.Variable = Variable;
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            this.Variable.HacerReferencia();
            if (this.Argumentos[0].Lexema == "++")
            {
                return true;
            }

            return false;
        }

        public override StringBuilder Traduccion(Funcion Fx)
        {
            StringBuilder sb=new StringBuilder();
            sb.Append(Variable.Nombre);
            sb.Append(this.Argumentos[0].Lexema);
            sb.Append(";");
            return sb;
        }
    }
}
