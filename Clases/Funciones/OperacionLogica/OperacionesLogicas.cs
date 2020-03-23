using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;
using My8086.Clases.Funciones.OperacionLogica;
using TipoToken = My8086.Clases.Funciones.OperacionLogica.TipoToken;

namespace My8086.Clases.Funciones
{
    public class OperacionesLogicas
    {
        private Nodo ArbolDeDirecciones { get; set; }
        private readonly StringBuilder Asm;
        private readonly Programa Programa;
        internal OperacionesLogicas(List<TokenLogico> Tokens, Programa Programa, ResultadosCompilacion Errores)
        {
            this.Programa = Programa;
            this.Programa.OperacionesLogicas = true;
            this.ArbolDeDirecciones = GenerarNodos(Tokens, Errores);
            this.Asm = new StringBuilder();
        }
        private Nodo GenerarNodos(List<TokenLogico> Tokens, ResultadosCompilacion Errores)
        {
            int NTemporal = 0;
            do
            {
                int IndexLogico = Tokens.FindIndex(x => x.TipoToken == TipoToken.OperadorLogico);
                if (IndexLogico <= 0)
                {
                    IndexLogico = Tokens.FindIndex(x => x.TipoToken == TipoToken.OperadorPrioritario);
                    if (IndexLogico <= 0)
                    {
                        IndexLogico = Tokens.FindIndex(x => x.TipoToken == TipoToken.OperadorSecundario);
                    }
                }

                TokenLogico n1 = Tokens[IndexLogico - 1];
                TokenLogico operador = Tokens[IndexLogico];
                TokenLogico n2 = Tokens[IndexLogico + 1];
                //NodosT.Add(new Nodo(NodosT.Count+1, n1, operador, n2));
                if (n1.Token.TipoDato != n2.Token.TipoDato && !(n1.EsNumero && n2.EsNumero))
                {
                    Errores.ResultadoCompilacion("Comparación incorrecta, se especificarón tipos de datos diferentes", null);
                    return null;
                }
                Tokens.Insert(IndexLogico - 1, new Nodo(++NTemporal, n1, operador, n2, n1.Token.TipoDato));

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
        public void DeclararTemporales()
        {
            DeclararTemporales(this.Programa, this.ArbolDeDirecciones);
        }
        private void DeclararTemporales(Programa programa, TokenLogico tk)
        {
            if (tk is Nodo nd)
            {
                string variable = $"R_COMPARADOR_{nd.NOrden}";
                if (!programa.SegmentoDeDatos.YaExisteVariable(variable))
                {
                    var NuevaVariable = new Variable(programa, variable, TipoDato.BitLogico) { EsAutomatica = true };
                    programa.SegmentoDeDatos.Nueva(NuevaVariable)
                        .HacerReferencia();
                }
                ///////////
                string[] variables = new[]{
                    $"SIGNOT{nd.NOrden}",
                    $"ENTEROST{nd.NOrden}",
                    $"DECIMALEST{nd.NOrden}"
                };
                foreach (string vv in variables)
                {
                    if (!programa.SegmentoDeDatos.YaExisteVariable(vv))
                    {
                        programa.SegmentoDeDatos.Nueva(new Variable(programa, vv, TipoDato.Decimal) {EsAutomatica=true })
                            .HacerReferencia();
                    }
                }
                //////////////
                DeclararTemporales(programa, nd.Numero1);
                DeclararTemporales(programa, nd.Numero2);
            }
            if (tk.TipoToken == TipoToken.Cadena && tk.Token.TipoToken == Fases.TipoToken.Cadena)
            {
                //Asm.Append(EscribeNumero(this.ArbolDeDirecciones, this.Variable.Nombre));
                //Es una cadena que no es una variable Ex. Imprime('Hola mundo');
                //Crearemos una variable con un identificador unico que guarde esta cadena
                LineaLexica lexica = new LineaLexica(tk.Token.Linea);
                Variable var = this.Programa.SegmentoDeDatos.Nueva(new Variable(this.Programa, tk.Token, lexica, tk.Token.TipoDato));
                var VariableComparacion = new Token(var.Nombre, My8086.Clases.Fases.TipoToken.Identificador, TipoDato.Cadena, tk.Token.Linea);
                this.Programa.Acciones.Add((new AsignaCadena(Programa, lexica, var, tk.Token)));
                tk.Token = VariableComparacion;
                //sb.AppendLine($"MOV DI,{var.Nombre}");
            }
        }
        internal static OperacionesLogicas Analizar(List<Token> Argumentos, Programa Programa, ResultadosCompilacion Errores)
        {
            OperacionesLogicas operaciones = null;
            List<TokenLogico> tokens = new List<TokenLogico>();

            for (int i = 0; i < Argumentos.Count; i++)
            {
                Token tk = Argumentos[i];
                if (double.TryParse(tk.Lexema, out double n))
                {
                    if (tokens.LastOrDefault()?.EsNumero ?? false)
                    {
                        tokens.Add(new TokenLogico(
                            new Token("+", Fases.TipoToken.OperadorAritmetico, TipoDato.NoAplica, tk.Linea), My8086.Clases.Funciones.OperacionLogica.TipoToken.OperadorSecundario));
                    }
                    tokens.Add(new TokenLogico(new Token(tk.Lexema.Replace("-", ""), Fases.TipoToken.Identificador, TipoDato.Decimal, tk.Linea),
                        n >= 0 ? My8086.Clases.Funciones.OperacionLogica.TipoToken.NumeroPositivo :
                           My8086.Clases.Funciones.OperacionLogica.TipoToken.NumeroNegativo));
                    continue;
                }
                if (tk.Lexema == "and")
                {
                    tokens.Add(new TokenLogico(tk, My8086.Clases.Funciones.OperacionLogica.TipoToken.OperadorPrioritario));
                    continue;
                }
                if (tk.Lexema == "or")
                {
                    tokens.Add(new TokenLogico(tk, My8086.Clases.Funciones.OperacionLogica.TipoToken.OperadorSecundario));
                    continue;
                }
                if (tk.Lexema.Length == 1)
                {
                    if (tk.Lexema == "<" || tk.Lexema == ">")
                    {
                        if (i < Argumentos.Count)
                        {
                            if (Argumentos[i + 1].Lexema == "=")
                            {
                                Argumentos.RemoveAt(i + 1);
                                tk.Lexema = tk.Lexema + "=";
                                tokens.Add(new TokenLogico(tk, My8086.Clases.Funciones.OperacionLogica.TipoToken.OperadorLogico));
                                continue;
                            }
                        }

                        tokens.Add(new TokenLogico(tk, My8086.Clases.Funciones.OperacionLogica.TipoToken.OperadorLogico));
                        continue;
                    }
                    else if (tk.Lexema == "=")
                    {
                        tokens.Add(new TokenLogico(tk, My8086.Clases.Funciones.OperacionLogica.TipoToken.OperadorLogico));
                        continue;
                    }
                }

                if (Programa.SegmentoDeDatos.ObtenerVariable(tk.Lexema) is Variable var)
                {
                    tk.TipoDato = var.TipoDato;
                    switch (var.TipoDato)
                    {
                        case TipoDato.Entero:
                        case TipoDato.Decimal:
                            tokens.Add(new TokenLogico(tk, My8086.Clases.Funciones.OperacionLogica.TipoToken.VariableNumerica));
                            continue;
                        case TipoDato.Cadena:
                            tokens.Add(new TokenLogico(tk, My8086.Clases.Funciones.OperacionLogica.TipoToken.Cadena));
                            continue;
                    }
                }
                else
                {
                    if (tk.TipoDato == TipoDato.Cadena && tk.TipoToken == Fases.TipoToken.Cadena)
                    {
                        tokens.Add(new TokenLogico(tk, My8086.Clases.Funciones.OperacionLogica.TipoToken.Cadena));
                        continue;
                    }
                    Errores.ResultadoCompilacion($"Uso de variable no declarada: {tk.Lexema}", null, false);
                    return null;
                }
            }
            operaciones = new OperacionesLogicas(tokens, Programa, Errores);
            return operaciones;
        }
        public StringBuilder GenerarAsm()
        {
            Asm.Append(";================[-OPERACION LOGICA-]================\n");
            Asm.AppendLine(";" + this.ToString());
            /////////
            List<StringBuilder> operaciones = new List<StringBuilder>();
            if (this.ArbolDeDirecciones is Nodo)
            {
                Traducir(operaciones, this.ArbolDeDirecciones);
            }

            foreach (var op in operaciones)
            {
                Asm.Append(op);
            }
            if (this.ArbolDeDirecciones.TipoToken == TipoToken.Temporal)
            {
                //Asm.AppendLine($"LEA DX,SGN_{this.Variable.Nombre}");
                //Asm.AppendLine("CALL SALVAR_TEMPORAL");
            }
            return Asm;
        }
        private List<StringBuilder> Traducir(List<StringBuilder> operaciones, TokenLogico Token)
        {
            if (Token is Nodo nd)
            {
                StringBuilder sb = new StringBuilder();
                if (nd.Numero1 is TokenLogico tk1)
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
                        if (nd.Operador.Token.Lexema == "and" || nd.Operador.Token.Lexema == "or")
                        {
                            sb.AppendLine($"MOV AL,R_COMPARADOR_{((Nodo)nd.Numero1).NOrden}");
                            sb.AppendLine($"MOV AH,R_COMPARADOR_{((Nodo)nd.Numero2).NOrden}");
                            if (nd.Operador.Token.Lexema == "and")
                            {
                                sb.AppendLine("MUL AH");

                            }
                            else
                            {
                                sb.AppendLine("ADD AH,AL");
                            }
                            sb.AppendLine($"MOV R_COMPARADOR_{nd.NOrden},AH");
                            sb.AppendLine($"MOV R_COMPARADOR,AH");
                            operaciones.Add(sb);
                            return operaciones;
                        }
                        sb.AppendLine(nd.NemonicoOperacion($"R_COMPARADOR_{nd.NOrden}", nd.TipoDato));
                    }
                    else
                    {
                        //sb.AppendLine(nd.NemonicoOperacion(this.Variable.Nombre, nd.NOrden));
                    }
                    //

                    //
                    operaciones.Add(sb);
                }
            }

            return operaciones;
        }
        private string EscribeNumero(TokenLogico tk, int n)
        {
            StringBuilder sb = new StringBuilder();
            if (tk is Nodo nodoN1)
            {
                switch (nodoN1.TipoDato)
                {
                    case TipoDato.Cadena:

                        break;

                    case TipoDato.Decimal:
                    case TipoDato.Entero:
                        sb.AppendLine($"LEA DX,SGN_SIGNOT{nodoN1.NOrden}");
                        sb.AppendLine($"CALL USAR_N{n}  ;vamos a utilizar T{nodoN1.NOrden}");
                        break;
                }
            }
            else if (tk.TipoToken == TipoToken.Temporal || tk.TipoToken == TipoToken.VariableNumerica)
            {
                sb.AppendLine($"LEA DX,SGN_{tk.Token.Lexema}");
                sb.AppendLine($"CALL USAR_N{n}  ;vamos a utilizar T_{tk.Token.Lexema}");
            }
            else if (tk.TipoToken == TipoToken.Cadena && tk.Token.TipoDato == TipoDato.Cadena)
            {
                sb.AppendLine($"MOV DI,{tk.Token.Lexema}");
                sb.AppendLine($"MOV CAD_TEMP{n},DI");
            }
            else if (tk.TipoToken == TipoToken.Cadena)
            {
                sb.AppendLine($"MOV DI,{tk.Token.Lexema}");
            }
            else
            {
                sb.AppendLine("\n;Parte Entera");
                sb.AppendLine($"\n MOV SGN{n},{(tk.TipoToken == TipoToken.NumeroNegativo ? "0FF" : "1")}H");
                for (int i = 0; i <= 3; i++)
                {
                    sb.Append($"MOV NUM{n}[{3 - i}h],");
                    sb.Append(NumeroLexema(tk, i, false).ToString());
                    sb.Append("\n");
                }

                sb.AppendLine("\n;Parte Decimal");
                for (int i = 0; i <= 3; i++)
                {
                    sb.Append($"MOV DEC{n}[{i}h],");
                    sb.Append(NumeroLexema(tk, i, true).ToString());
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }
        private string EscribeNumero(TokenLogico tk, string nombreVariable)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\n;Parte Entera");
            sb.AppendLine($"\n MOV SGN_{nombreVariable},{(tk.TipoToken == TipoToken.NumeroNegativo ? "0FF" : "1")}H");
            for (int i = 0; i <= 3; i++)
            {
                sb.Append($"MOV NUM_{nombreVariable}[{3 - i}h],");
                sb.Append(NumeroLexema(tk, i, false).ToString());
                sb.Append("\n");
            }

            sb.AppendLine("\n;Parte Decimal");
            for (int i = 0; i <= 3; i++)
            {
                sb.Append($"MOV DEC_{nombreVariable}[{i}h],");
                sb.Append(NumeroLexema(tk, i, true).ToString());
                sb.Append("\n");
            }


            return sb.ToString();
        }
        private char NumeroLexema(TokenLogico tk, int index, bool decimales)
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
        public override string ToString()
        {
            return this.ArbolDeDirecciones.ToString();
        }
    }
}
