using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    class AsignaCadena : Accion
    {
        private readonly Token ValorCadena;
        private readonly Variable Variable;
        public AsignaCadena(Programa Programa, LineaLexica Linea, Variable Variable, Token ValorCadena) : base(Programa, Linea, 0)
        {
            this.Programa.OperacionesConCadenas = true;
            this.ValorCadena = ValorCadena;
            this.Variable = Variable;
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            Variable.HacerReferencia();
            return true;
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(";=================[ASIGNAR VALOR A LA CADENA]=================");
            if (ValorCadena.TipoDato == TipoDato.Cadena)
            {
                int TamanioCadena = 0;

                //Revisar si la cadena contiene \n para escaparlo en ensambaldor '10,13'
                string cadena = ValorCadena.Lexema;

                string[] partes =
                    Programa.Expresiones.SaltoLinea
                        .Split(cadena)
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();
                foreach (string parte in partes)
                {
                    switch (parte)
                    {
                        case "\\n":
                            sb.AppendLine($"MOV DL,10");
                            sb.AppendLine("MOV [SI],DL");
                            sb.AppendLine($"INC SI");
                            sb.AppendLine($"MOV DL,13");
                            sb.AppendLine("MOV [SI],DL");
                            sb.AppendLine($"INC SI");
                            TamanioCadena += 2;
                            continue;

                    }
                    //COPIAR VALOR ESTATICO
                    foreach (char caracter in parte)
                    {
                        sb.AppendLine($"MOV DL,'{caracter}'");
                        sb.AppendLine("MOV [SI],DL");
                        sb.AppendLine("INC SI");
                        TamanioCadena++;
                    }
                }

                sb.AppendLine("MOV DL,FIN_CADENA");
                sb.AppendLine("MOV [SI],DL");

                sb.AppendLine($"MOV DX,CADENA");
                sb.AppendLine($"MOV {Variable.Nombre},DX");

                //Inicio
                StringBuilder inicio = new StringBuilder();
                inicio.Append(";================[-ALOGAR CADENA-]================\n");
                inicio.Append($@"MOV BLOQUE_ACTUAL,{Math.Ceiling(TamanioCadena / 14.0) + 10}");
                inicio.AppendLine("D");

                inicio.AppendLine("CALL ALOGAR_CADENA");
                inicio.AppendLine("MOV SI,CADENA");

                sb.Insert(0, inicio);
            }


            return sb;
        }
    }
}
