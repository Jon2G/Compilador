using System.Collections.Generic;
using System.Linq;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;

namespace My8086.Clases.Fases._1._Analisis_Lexico
{
    internal class AnalizadorLexico : Base.Analizador
    {
        private readonly TablaDeSimbolos Tabla;
        public readonly List<LineaLexica> LineasLexicas;
        public bool FinLineas { get; private set; }
        private int FilaTabla;
        public AnalizadorLexico(TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.FinLineas = false;
            this.Tabla = new TablaDeSimbolos();
            this.LineasLexicas=new List<LineaLexica>();
            this.FilaTabla = -1;
        }
        public override void Analizar()
        {
            Token token = null;
            this.EsValido = true;
            foreach (DocumentLine linea in this.Documento.Lines)
            {
                LineaLexica LLex = new LineaLexica(linea);
                string texto = this.Documento.GetText(linea).Trim();
                if (string.IsNullOrEmpty(texto))
                {
                    continue;
                }
                string[] palabras =
                    ExpresionesRegulares.ExpresionesRegulares.Documento
                        .Split(texto)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();
                foreach (string palabra in palabras)
                {
                    token = Token.Identificar(palabra, linea, this.Errores);
                    if (token.EsValido)
                    {
                        this.Tabla.Agregar(token);
                        LLex.Agregar(token);
                    }
                    else
                    {
                        this.Errores.ResultadoCompilacion(
                            $"El nombre '{token.Lexema}' no existe en el contexto actual", linea);
                        this.EsValido = false;
                    }
                }
                this.LineasLexicas.Add(LLex);
            }
        }
    }
}
