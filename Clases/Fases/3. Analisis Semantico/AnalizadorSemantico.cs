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
            this.Programa.Funciones.ForEach(Analizar);
        }
        private void Analizar(Funcion Funcion)
        {
            if (!Funcion.Cerrado)
            {
                this.Errores.ResultadoCompilacion(
                    $"Se esperaba el cierre de la función '{Funcion.Titulo}' y no se econtro", Funcion.InicioBloque);
            }
            foreach (Accion Accion in Funcion.Acciones)
            {
                if (!Accion.RevisarSemantica(this.Errores)&&this.EsValido)
                {
                    this.EsValido = false;
                }
            }
        }
    }
}
