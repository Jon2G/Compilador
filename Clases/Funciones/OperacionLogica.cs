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
    class OperacionLogica : Accion
    {
        private readonly Token Valor1;
        private readonly Token Valor2;
        private readonly Token OperadorLogico;
        public OperacionLogica(Token Valor1, Token OperadorLogico,Token Valor2, LineaLexica Linea) : base(null, Linea, 1)
        {
            this.Valor1 = Valor1;
            this.OperadorLogico = OperadorLogico;
            this.Valor2 = Valor2;
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            //this.Variable.HacerReferencia();
            //if (this.Argumentos[0].Lexema == "++")
            //{
            //    return true;
            //}

            return false;
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb=new StringBuilder();
            //sb.Append(Variable.Nombre);
            //sb.Append(this.Argumentos[0].Lexema);
            //sb.Append(";");
            return sb;
        }
    }
}
