﻿using System;
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
        private Variable VariableDestino;
        public Lee(Programa Programa, LineaLexica Linea) : base(Programa, Linea, 1)
        {

        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            if (!Argumentos.Any())
            {
                Errores.ResultadoCompilacion("Se esperaba (Variable); el la función leer.",
                    LineaDocumento);
            }

            if (this.Argumentos[0].TipoToken == TipoToken.ParentesisAbierto)
            {
                if (this.Argumentos[1].TipoToken == TipoToken.Identificador)
                {
                    if (this.Argumentos[2].TipoToken == TipoToken.ParentesCerrado)
                    {
                        Variable variable = this.Programa.SegmentoDeDatos.ObtenerVariable(this.Argumentos[1].Lexema);
                        if (variable != null)
                        {
                            this.VariableDestino = variable;
                            this.VariableDestino.HacerReferencia();
                            switch (VariableDestino.TipoDato)
                            {
                                case TipoDato.Cadena:
                                    this.Programa.LeecturaCadenas = true;
                                    break;
                                case TipoDato.Decimal:
                                    this.Programa.LeecturaNumerosDecimales = true;
                                    break;
                                case TipoDato.Entero:
                                    this.Programa.LeecturaNumerosEnteros = true;
                                    break;
                            }
                            return true;
                        }
                        else
                        {
                            Errores.ResultadoCompilacion("Variable no declarada",
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
            switch (this.VariableDestino.TipoDato)
            {
                case TipoDato.Cadena:
                    sb.AppendLine("CALL LEER_CADENA");
                    sb.AppendLine($"MOV SI,CADENA"); //CADENA variable auxiliar
                    sb.AppendLine($"MOV {VariableDestino.Nombre},SI");
                    break;
                case TipoDato.Entero:
                    sb.AppendLine($"LEA DI,SGN_{VariableDestino.Nombre}");
                    sb.AppendLine("CALL LEER_ENTERO");
                    break;
                case TipoDato.Decimal:
                    sb.AppendLine($"LEA DI,SGN_{VariableDestino.Nombre}");
                    sb.AppendLine("CALL LEER_DECIMAL");
                    break;
            }
            return sb;
        }
    }
}
