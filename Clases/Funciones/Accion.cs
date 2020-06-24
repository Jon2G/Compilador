using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Compilador;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    public abstract class Accion
    {
        protected List<Token> Argumentos { get; private set; }
        public readonly DocumentLine LineaDocumento;
        protected readonly Programa Programa;
        protected Accion(Programa Programa)
        {
            this.Programa = Programa;
        }
        protected Accion(Programa Programa,LineaLexica Linea, int InicioArgumentos, int FinArgumentos=-1)
        {
            this.Programa = Programa;
            this.Argumentos = new List<Token>();
            this.LineaDocumento = Linea.LineaDocumento;
            if (FinArgumentos == -1)
            {
                FinArgumentos = Linea.Elementos;
            }

            for (int i = InicioArgumentos; i < FinArgumentos; i++)
            {
                Token tk = Linea[i];
                //if (tk.TipoToken != TipoToken.PalabraReservada&& tk.TipoToken != TipoToken.SeparadorParametros)
                //{
                this.Argumentos.Add(tk);
                //}
            }
        }
        protected void HacerReferencia(Token tk)
        {
            Variable variable = this.Programa.SegmentoDeDatos.ObtenerVariable(tk.Lexema);
            variable?.HacerReferencia();
        }
        public abstract bool RevisarSemantica(ResultadosCompilacion Errores);
        public abstract StringBuilder Traduccion();
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.GetType().Name);
            this.Argumentos.ForEach(arg =>
            {
                sb.Append("<");
                sb.Append(arg.Lexema);
                sb.Append("<");
                sb.Append(arg.TipoToken);
                sb.Append(">");
                sb.Append(">");
            });
            return sb.ToString();
        }
    }
}
