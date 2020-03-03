﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Compilador;
using My8086.Clases.Fases._3._Analisis_Semantico;
using My8086.Clases.Funciones;

namespace My8086.Clases.Fases._4._Sintetizador
{
    internal class Sintesis : Base.Analizador
    {
        public readonly Programa Programa;
        public Sintesis(AnalizadorSemantico Semantico, TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.Programa = Semantico.Programa;
        }
        public override void Analizar()
        {
            this.EsValido = true;
            this.Programa.Funciones.ForEach(LeerFuncion);
        }

        private void LeerFuncion(Funciones.Funcion Funcion)
        {
            for (var index = 0; index < Funcion.Variables.Count; index++)
            {
                Variable variable = Funcion.Variables[index];
                if (variable.Referencias <= 0)
                {
                    Errores.ResultadoCompilacion($"La variable '{variable.Nombre}' se declara pero nunca se utiliza.",
                        variable.LineaDocumento, true);
                    Funcion.Variables.Remove(variable);
                }
            }

            //?
        }
    }
}
