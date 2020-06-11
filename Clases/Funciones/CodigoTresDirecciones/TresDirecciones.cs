using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My8086.Clases.Fases;

namespace My8086.Clases.Funciones.CodigoTresDirecciones
{
    internal class TresDirecciones
    {
        private readonly StringBuilder Asm;

        public TokenAritmetico ArbolDeDirecciones { get; set; }
        public Variable Variable { get; set; }
        public TresDirecciones()
        {
            this.Asm = new StringBuilder();
        }
        public TresDirecciones(List<TokenAritmetico> Tokens)
        {
            this.ArbolDeDirecciones = GenerarNodos(Tokens);
            this.Asm = new StringBuilder();
        }

        private Nodo GenerarNodos(List<TokenAritmetico> Tokens)
        {
            int NTemporal = 0;
            do
            {
                int IndexprimerOrden = Tokens.FindIndex(x => x.TipoToken == TipoToken.OperadorPrioritario);
                if (IndexprimerOrden <= 0)
                {
                    IndexprimerOrden = Tokens.FindIndex(x => x.TipoToken == TipoToken.OperadorSecundario);
                }

                TokenAritmetico n1 = Tokens[IndexprimerOrden - 1];
                TokenAritmetico operador = Tokens[IndexprimerOrden];
                TokenAritmetico n2 = Tokens[IndexprimerOrden + 1];
                //NodosT.Add(new Nodo(NodosT.Count+1, n1, operador, n2));
                Tokens.Insert(IndexprimerOrden - 1, new Nodo(++NTemporal, n1, operador, n2));

                Tokens.Remove(n1);
                Tokens.Remove(n2);
                Tokens.Remove(operador);

                if (Tokens.Count == 1 && Tokens.First().TipoToken == TipoToken.Temporal)
                {
                    break;
                }

            } while (Tokens.Any());

            return (Nodo)Tokens.First();
        }
        public void DeclararTemporales(Programa programa)
        {
            DeclararTemporales(programa, this.ArbolDeDirecciones);
        }
        private void DeclararTemporales(Programa programa, TokenAritmetico tk)
        {
            programa.OperacionesConNumeros = true;
            if (tk is Nodo nd)
            {
                string[] variables = new[]{
                    $"SIGNOT{nd.NOrden}",
                    $"ENTEROST{nd.NOrden}",
                    $"DECIMALEST{nd.NOrden}"
                };
                foreach (string variable in variables)
                {
                    if (!programa.SegmentoDeDatos.YaExisteVariable(variable))
                    {
                        programa.SegmentoDeDatos.Nueva(new Variable(programa, variable, TipoDato.Decimal))
                            .HacerReferencia();
                    }
                }
                //temporales.AppendLine($"SIGNOT{nd.NOrden}     db 0");
                //temporales.AppendLine($"ENTEROST{nd.NOrden}   db 0,0,0,0");
                //temporales.AppendLine($"DECIMALEST{nd.NOrden}   db 0,0,0,0");

                DeclararTemporales(programa, nd.Numero1);
                DeclararTemporales(programa, nd.Numero2);
            }
        }
        public StringBuilder GenerarAsm()
        {

            Asm.Append(";================[-CODIGO TRES DIRECCIONES-]================\n");
            /////////
            List<StringBuilder> operaciones = new List<StringBuilder>();
            if (this.ArbolDeDirecciones is Nodo)
            {
                Traducir(operaciones, this.ArbolDeDirecciones);
            }
            else
            {
                Asm.Append(EscribeNumero(this.ArbolDeDirecciones, this.Variable.Nombre));
            }

            foreach (var op in operaciones)
            {
                Asm.Append(op);
            }
            if (this.ArbolDeDirecciones.TipoToken == TipoToken.Temporal)
            {
                Asm.AppendLine($"LEA DX,SGN_{this.Variable.Nombre}");
                Asm.AppendLine("CALL SALVAR_TEMPORAL");
            }
            return Asm;
        }
        private List<StringBuilder> Traducir(List<StringBuilder> operaciones, TokenAritmetico Token)
        {
            if (Token is Nodo nd)
            {
                StringBuilder sb = new StringBuilder();
                if (nd.Numero1 is TokenAritmetico tk1)
                {
                    sb.Append(";=========[");
                    sb.Append(nd);
                    sb.AppendLine("]=========\n");
                    //
                    sb.Append(EscribeNumero(nd.Numero1, 1));
                    sb.Append(EscribeNumero(nd.Numero2, 2));

                    Traducir(operaciones, nd.Numero1);
                    Traducir(operaciones, nd.Numero2);

                    //sb.AppendLine(nd.NemonicoOperacion(nd.NOrden));
                    if (nd.TipoToken == TipoToken.Temporal)
                    {
                        sb.AppendLine(nd.NemonicoOperacion($"SIGNOT{nd.NOrden}"));
                    }
                    else
                    {
                        sb.AppendLine(nd.NemonicoOperacion(this.Variable.Nombre));
                    }
                    //

                    //
                    operaciones.Add(sb);
                }
            }

            return operaciones;
        }
        private string EscribeNumero(TokenAritmetico tk, int n)
        {
            StringBuilder sb = new StringBuilder();
            if (tk is Nodo nodoN1)
            {
                sb.AppendLine($"LEA DX,SGN_SIGNOT{nodoN1.NOrden}");
                sb.AppendLine($"CALL USAR_N{n}  ;vamos a utilizar T{nodoN1.NOrden}");
            }
            else if (tk.TipoToken == TipoToken.Temporal)
            {
                sb.AppendLine($"LEA DX,SGN_{tk.Token.Lexema}");
                sb.AppendLine($"CALL USAR_N{n}  ;vamos a utilizar T_{tk.Token.Lexema}");
            }
            else //Ahi le revisa
            {
                sb.AppendLine("\n;Parte Entera");
                sb.AppendLine($"\n MOV SGN{n},{(tk.TipoToken == TipoToken.NumeroNegativo ? "0FF" : "1")}H");

                for (int i = 0; i <= 8; i++)
                {
                    sb.Append($"MOV NUM{n}[{8 - i}h],");
                    sb.Append(NumeroLexema(tk, i, false).ToString());
                    sb.Append("\n");
                }

                sb.AppendLine("\n;Parte Decimal");
                for (int i = 0; i <= 8; i++)
                {
                    sb.Append($"MOV DEC{n}[{i}h],");
                    sb.Append(NumeroLexema(tk, i, true).ToString());
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }
        private string EscribeNumero(TokenAritmetico tk, string nombreVariable)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\n;Parte Entera");
            sb.AppendLine($"\n MOV SGN_{nombreVariable},{(tk.TipoToken == TipoToken.NumeroNegativo ? "0FF" : "1")}H");
            for (int i = 0; i <= 8; i++)
            {
                sb.Append($"MOV NUM_{nombreVariable}[{8 - i}h],");
                sb.Append(NumeroLexema(tk, i, false).ToString());
                sb.Append("\n");
            }

            sb.AppendLine("\n;Parte Decimal");
            for (int i = 0; i <= 8; i++)
            {
                sb.Append($"MOV DEC_{nombreVariable}[{i}h],");
                sb.Append(NumeroLexema(tk, i, true).ToString());
                sb.Append("\n");
            }


            return sb.ToString();
        }
        private char NumeroLexema(TokenAritmetico tk, int index, bool decimales)
        {
            char n = '0';
            string temp = tk.Token.Lexema;
            temp = string.Join("", temp.Reverse());
            int dot = temp.IndexOf('.');
            if (!decimales)
            {
                if (dot >= 0)
                {
                    temp = temp.Substring(dot + 1);
                }
                if (temp.Length > index)
                {
                    n = temp[index];
                }
            }
            else
            {
                if (dot >= 0)
                {
                    temp = temp.Substring(0, dot);
                    temp = string.Join("", temp.Reverse());
                }
                else
                {
                    return n;
                }
                if (temp.Length > index)
                {
                    n = temp[index];
                }
            }
            return n;
        }
    }
}
