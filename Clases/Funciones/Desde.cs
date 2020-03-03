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
    internal class Desde : Accion, IBloque
    {
        public DocumentLine FinBloque { get; set; }
        public DocumentLine InicioBloque { get; set; }
        public string CerrarBloque()
        {
            return "}";
        }

        public bool Cerrado { get; set; }
        public Desde(Funcion Fx,LineaLexica Linea, int InicioArgumentos = 1) : base(Fx,Linea, InicioArgumentos)
        {
            this.InicioBloque = Linea.LineaDocumento;
        }
        public void Cerrar(DocumentLine lineaLineaDocumento)
        {
            this.FinBloque = lineaLineaDocumento;
            this.Cerrado = true;
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            if (Argumentos.Count < 3)
            {
                Errores.ResultadoCompilacion("Hacen falta argumentos para la instrucción desde", LineaDocumento);
                return false;
            }

            if (
                ((Argumentos[0].TipoDato == TipoDato.Entero) ||
                (Argumentos[0].TipoDato == TipoDato.Cadena && Argumentos[0].TipoToken == TipoToken.Identificador))
                &&
                ((Argumentos[2].TipoDato == TipoDato.Entero) ||
                 (Argumentos[2].TipoDato == TipoDato.Cadena && Argumentos[2].TipoToken == TipoToken.Identificador)))
            {
                if (Argumentos[0].TipoToken == TipoToken.Identificador)
                {
                    HacerReferencia(Argumentos[0]);
                }
                if (Argumentos[2].TipoToken == TipoToken.Identificador)
                {
                    HacerReferencia(Argumentos[2]);
                }
                return true;
            }
            return false;
        }

        public override StringBuilder Traduccion(Funcion Fx)
        {
            string contador = "AutoVar_" + Guid.NewGuid().ToString().Replace("-", "_");
            StringBuilder sb = new StringBuilder();
            sb.Append($"for(");
            sb.Append(this.Argumentos[0].ObtenerTipoDato(Fx));
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
    }
}
