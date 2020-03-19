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
    internal class Lee : Accion
    {

        public Lee(Programa Programa, LineaLexica Linea) : base(Programa, Linea, 1)
        {

        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            if (!Argumentos.Any())
            {
                Errores.ResultadoCompilacion("Hace falta un argumento para la función imprimir",
                    LineaDocumento);
            }

            if (this.Argumentos[0].TipoToken == TipoToken.ParentesisAbierto)
            {
                if (this.Argumentos[1].TipoToken == TipoToken.Identificador)
                {
                    if (this.Argumentos[2].TipoToken == TipoToken.ParentesCerrado)
                    {
                        if (this.Programa.SegmentoDeDatos.ObtenerVariable(this.Argumentos[1].Lexema) is { } variable)
                        {
                            variable.HacerReferencia();
                            return true;
                        }
                        else
                        {
                            Errores.ResultadoCompilacion("Variable destino de leectura invalida",
                                LineaDocumento);
                        }
                    }
                    else
                    {
                        Errores.ResultadoCompilacion("Se esperaba ')'",
                            LineaDocumento);
                    }
                }
                else
                {
                    Errores.ResultadoCompilacion("Se esperaba una variable",
                        LineaDocumento);
                }
            }
            else
            {
                Errores.ResultadoCompilacion("Se esperaba '('",
                    LineaDocumento);
            }

            return false;
        }

        public override StringBuilder Traduccion()
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
