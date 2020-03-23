using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;

namespace My8086.Clases.Fases._1._Analisis_Lexico
{
    internal class AnalizadorLexico : Base.Analizador
    {
        public readonly List<LineaLexica> LineasLexicas;
        public bool FinLineas { get; private set; }
        internal ExpresionesRegulares.ExpresionesRegulares Expresiones;
        public readonly List<Token> Simbolos;
        public AnalizadorLexico(TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.FinLineas = false;
            this.LineasLexicas = new List<LineaLexica>();
            this.Simbolos = new List<Token>();
            this.Expresiones = new ExpresionesRegulares.ExpresionesRegulares();
        }
        public override void Analizar()
        {
            Token token = null;
            this.EsValido = true;

            foreach (DocumentLine linea in this.Documento.Lines)
            {
                LineaLexica LLex = new LineaLexica(linea);
                string texto = this.Documento.GetText(linea).Trim();
                //Remover comentarios
                texto = Expresiones.Comentarios.Replace(texto, "").Trim();
                if (string.IsNullOrEmpty(texto))
                {
                    continue;
                }
                string[] palabras =
                    Expresiones.Documento
                        .Split(texto)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();
                foreach (string palabra in palabras)
                {
                    token = Token.Identificar(palabra, linea, this.Errores, Expresiones);
                    if (token != null)
                    {
                        if (token.EsValido)
                        {
                            this.Simbolos.Add(token);
                            LLex.Agregar(token);
                        }
                        else
                        {
                            this.Errores.ResultadoCompilacion(
                                $"El nombre '{token.Lexema}' no existe en el contexto actual", linea);
                            this.EsValido = false;
                        }
                    }
                }
                this.LineasLexicas.Add(LLex);
            }



        }

        public override void Analizar(List<Tuple<string,DocumentLine>> Lineas)
        {
            Token token = null;
            this.EsValido = true;

            foreach (Tuple<string, DocumentLine> linea in Lineas)
            {
                LineaLexica LLex = new LineaLexica(linea.Item2);
                string texto = linea.Item1;
                //Remover comentarios
                texto = Expresiones.Comentarios.Replace(texto, "").Trim();
                if (string.IsNullOrEmpty(texto))
                {
                    continue;
                }
                string[] palabras =
                    Expresiones.Documento
                        .Split(texto)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();
                foreach (string palabra in palabras)
                {
                    token = Token.Identificar(palabra, linea.Item2, this.Errores, Expresiones);
                    if (token != null)
                    {
                        if (token.EsValido)
                        {
                            this.Simbolos.Add(token);
                            LLex.Agregar(token);
                        }
                        else
                        {
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
