using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Compilador;
using My8086.Clases.Fases._1._Analisis_Lexico;
using My8086.Clases.Fases._2._Analisis_Sintactico;
using My8086.Clases.Funciones;

namespace My8086.Clases.Fases._3._Analisis_Semantico
{
    class AnalizadorSemantico : Base.Analizador
    {
        internal readonly Programa Programa;
        public AnalizadorSemantico(AnalizadorSintactico Sintactico, TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.Programa = Sintactico.Programa;
        }

        public override void Analizar()
        {
            this.EsValido = true;

            for (int i = 0; i < Programa.Acciones.Count; i++)
            {
                Accion Accion = Programa.Acciones[i];
                if (!Accion.RevisarSemantica(this.Errores))
                {
                    this.EsValido= false;
                    break;
                }
            }
        }
    }
}
