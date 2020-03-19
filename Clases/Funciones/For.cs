using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Compilador;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    internal class For : Accion, IBloque
    {

        public DocumentLine InicioBloque { get; set; }
        public DocumentLine FinBloque { get; set; }
        private double Incremento { get; set; } = 0;
        private double Inicio { get; set; } = 0;
        private Variable Contador { get; set; }
        private readonly OperacionLogica[] OperacionLogica;
        public For(Programa Programa, LineaLexica Linea, OperacionLogica[] OperacionLogica, double Inicio, double Incremento) : base(Programa, Linea, 1)
        {
            this.OperacionLogica = OperacionLogica;
            this.Inicio = Inicio;
            this.Incremento = Incremento;
           
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {


            return true;
        }

        public override StringBuilder Traduccion()
        {
            string contador = "AUTOCONTADOR_" + Guid.NewGuid().ToString().Replace("-", "");
            StringBuilder sb = new StringBuilder();
            sb.Append($"for(");
            //sb.Append(this.Argumentos[0].ObtenerTipoDato());
            sb.Append(" ");
            sb.Append(contador);
            sb.Append(" = ");
            sb.Append(this.Argumentos[0].Lexema);
            sb.Append(";");
            sb.Append(contador);
            sb.Append("<=");
            sb.Append(this.Argumentos[2].Lexema);
            sb.Append(";");
            sb.Append(contador);
            sb.Append("++){");
            return sb;
        }

        public StringBuilder CerrarBloque()
        {
            throw new NotImplementedException();
        }
    }
}
