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
        private readonly Funcion FuncionActual;
        protected Accion(Funcion FuncionActual, LineaLexica Linea, int InicioArgumentos)
        {
            this.Argumentos = new List<Token>();
            this.FuncionActual = FuncionActual;
            this.LineaDocumento = Linea.LineaDocumento;
            for (int i = InicioArgumentos; i < Linea.Elementos; i++)
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
            Variable variable = this.FuncionActual.Variables.FirstOrDefault(x => x.Nombre == tk.Lexema);
            variable?.HacerReferencia();
        }
        public abstract bool RevisarSemantica(ResultadosCompilacion Errores);
        public abstract StringBuilder Traduccion(Funcion Fx);
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
