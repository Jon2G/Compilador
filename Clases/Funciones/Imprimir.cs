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

        public Imprimir(LineaLexica Linea, int InicioArgumentos = 1) : base(Linea, InicioArgumentos)
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

                    if (Argumentos[1].TipoDato == TipoDato.Entero && Argumentos[2].TipoDato == TipoDato.Entero)
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
            if (this.Argumentos.Count == 3)
            {
                sb.AppendLine($"Console.SetCursorPosition({this.Argumentos[1].Lexema}, {this.Argumentos[2].Lexema});");
            }
            sb.AppendLine($@"Console.Write({this.Argumentos[0].Lexema});");
            return sb;
        }
    }
}
