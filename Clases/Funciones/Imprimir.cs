using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using My8086.Clases.Advertencias;
using My8086.Clases.Compilador;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    internal class Imprimir : Accion
    {

        public Imprimir(Funcion Fx, LineaLexica Linea, int InicioArgumentos = 1) : base(Fx, Linea, InicioArgumentos)
        {

        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            if (!Argumentos.Any())
            {
                Errores.ResultadoCompilacion("Hace falta un argumento para la función imprimir",
                    LineaDocumento);
            }

            if (this.Argumentos[0].TipoDato == TipoDato.Cadena || this.Argumentos[0].TipoToken == TipoToken.Identificador)
            {
                if (Argumentos.Count > 1)
                {
                    if (Argumentos.Count < 3)
                    {
                        Errores.ResultadoCompilacion("Hacen falta los argumentos de coordenada x,y",
                            LineaDocumento);
                        return false;
                    }

                    if (Argumentos[1].TipoToken == TipoToken.OperadorAritmetico)
                    {
                        if (Argumentos[1].Lexema != "+")
                        {
                            Errores.ResultadoCompilacion($"No se puede utilizar el operador '{Argumentos[1].Lexema}' en conjunto con la cadena",
                                LineaDocumento);
                        }
                    }
                    if (Argumentos[2].TipoDato == TipoDato.Entero || Argumentos[2].TipoDato == TipoDato.Cadena)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public override StringBuilder Traduccion(Funcion Fx)
        {
            StringBuilder sb = new StringBuilder();
            if (this.Argumentos.Count >= 4)
            {
                if (this.Argumentos[this.Argumentos.Count - 4].TipoToken == TipoToken.SeparadorParametros)
                {
                    var x = this.Argumentos[this.Argumentos.Count - 3];
                    var y = this.Argumentos[this.Argumentos.Count - 1];
                    if ((x.TipoDato == TipoDato.Entero || x.TipoToken == TipoToken.Identificador) &&
                        (y.TipoDato == TipoDato.Entero || y.TipoToken == TipoToken.Identificador))
                    {
                        sb.AppendLine($"Console.SetCursorPosition({x.Lexema}, {y.Lexema});");
                    }
                }
            }

            sb.AppendLine($@"Console.Write(");
            for (int i = 0; i < this.Argumentos.Count; i++)
            {
                sb.Append(this.Argumentos[i].Lexema);
            }
            sb.AppendLine(");");
            return sb;
        }
    }
}
