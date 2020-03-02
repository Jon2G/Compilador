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
    internal class Ejecutar:Accion
    {
        public Ejecutar(LineaLexica Linea, int InicioArgumentos) : base(Linea, InicioArgumentos)
        {

        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            if (this.Argumentos[0].TipoToken != TipoToken.Identificador)
            {
                return false;
            }

            return true;
        }

        public override StringBuilder Traduccion(Funcion Fx)
        {
            StringBuilder sb=new StringBuilder();
            sb.Append(this.Argumentos[0].Lexema);
            sb.AppendLine("();");
            if (this.Argumentos.Count > 1)
            {

            }

            return sb;
        }
    }
}
