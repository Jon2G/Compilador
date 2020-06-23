using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;

namespace My8086.Clases.Fases._1._Analisis_Lexico//divide el documento en lineas de tokens 
{
    internal class AnalizadorLexico : Base.Analizador//tiene como base el anterior
    {
        public readonly List<LineaLexica> LineasLexicas;//lista de lineas ya clasificadas
        internal ExpresionesRegulares.ExpresionesRegulares Expresiones;//uso de las expresiones 
        public readonly List<Token> Simbolos;//tabla de simnolos en forma de lista 
        public AnalizadorLexico(TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {

            this.LineasLexicas = new List<LineaLexica>();
            this.Simbolos = new List<Token>();
            this.Expresiones = new ExpresionesRegulares.ExpresionesRegulares();
        }
        public override void Analizar()//la primera implementacion de analizar
        {
            Token token = null;
            this.EsValido = true;//es correcto de entrada

            foreach (DocumentLine linea in this.Documento.Lines)// por cada line empieza a hacer esto
            {
                LineaLexica LLex = new LineaLexica(linea);//construye una linea de texto plano a una lexiaca
                string texto = this.Documento.GetText(linea).Trim();
                //Remover comentarios
                texto = Expresiones.Comentarios.Replace(texto, "").Trim();
                if (string.IsNullOrEmpty(texto))//si esta vacio era una linea de comentarios 
                {
                    continue;
                }
                string[] palabras =//separa las palabras
                    Expresiones.Documento
                        .Split(texto)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();
                foreach (string palabra in palabras)
                {
                    token = Token.Identificar(palabra, linea, Expresiones);
                    if (token != null)//revisa que sea valido y agrega a la tabla 
                    {
                        if (token.EsValido)
                        {
                            this.Simbolos.Add(token);
                            LLex.Agregar(token);
                        }
                        else
                        {
                            this.Errores.ResultadoCompilacion(
                                $"El nombre '{token.Lexema}' no existe en el contexto actual", linea);//mara el error y pone el analisis como invalido 
                            this.EsValido = false;
                        }
                    }
                }
                this.LineasLexicas.Add(LLex);//agraga la line a la lista 
            }



        }

        public override void Analizar(List<Tuple<string, DocumentLine>> Lineas)
        {
            bool Codigo8086 = false;
            Token token = null;
            this.EsValido = true;

            foreach (Tuple<string, DocumentLine> linea in Lineas)
            {
                LineaLexica LLex = new LineaLexica(linea.Item2);
                string texto = linea.Item1;
                //Remover comentarios
                texto = Expresiones.Comentarios.Replace(texto, "").Trim();//obtiene el texto de la linea
                if (string.IsNullOrEmpty(texto))//elimina comentarios
                {
                    continue;
                }
                string[] palabras =//divide en palabras
                    Expresiones.Documento
                        .Split(texto)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();
                foreach (string palabra in palabras)
                {
                    token = Token.Identificar(palabra, linea.Item2, Expresiones);
                    if (token != null)
                    {
                        if (token.EsValido)
                        {
                            if (Codigo8086 && token.Lexema != "begin" && token.Lexema != "end")
                            {
                                token.TipoToken= TipoToken.Ensamblador;
                                token.TipoDato = TipoDato.Ensamblador;
                            }
                            if (Codigo8086 && token.Lexema != "end")
                            {
                                Codigo8086 = false;
                            }

                            this.Simbolos.Add(token);
                            if (token.TipoToken == TipoToken.PalabraReservada && token.Lexema == "_8086")
                            {
                                if (Codigo8086)
                                {
                                    this.Errores.ResultadoCompilacion(
                                        $"No se cerro el bloque de _8086 previo", linea.Item2);
                                }
                                else
                                {
                                    Codigo8086 = true;
                                }
                            }
                            LLex.Agregar(token);//agrega los tokens 
                        }
                        else
                        {
                            if (Codigo8086)
                            {
                                return;
                            }
                            this.Errores.ResultadoCompilacion(
                                $"El nombre '{token.Lexema}' no existe en el contexto actual", linea.Item2);
                            this.EsValido = false;
                        }
                    }
                }
                this.LineasLexicas.Add(LLex);
            }



        }
    }
}
