using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using My8086.Clases.Advertencias;
using My8086.Clases.Compilador;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;
using My8086.Clases.Funciones.CodigoTresDirecciones;
using TipoToken = My8086.Clases.Fases.TipoToken;

namespace My8086.Clases.Funciones
{
    internal class Imprimir : Accion
    {
        private readonly Variable VariableImpresion;

        public Imprimir(Programa Programa, Token VariableImpresion, LineaLexica Linea) : base(Programa, Linea, 1)
        {
            this.VariableImpresion = this.Programa.SegmentoDeDatos.ObtenerVariable(VariableImpresion.Lexema);
            //this.VariableImpresion = VariableImpresion;
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            if (this.VariableImpresion is null)
            {
                Errores.VariableNoDeclarada("Variable no identificada", this.LineaDocumento);
                return false;
            }
            return true;
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            switch (this.VariableImpresion.TipoDato)
            {
                case TipoDato.Decimal:
                case TipoDato.Entero:
                    sb.AppendLine($"LEA DI,SGN_{this.VariableImpresion.Nombre}");
                    sb.AppendLine($"CALL MOSTRAR_RESULTADO");
                    break;
                default:
                    sb.AppendLine($"MOV DX,{this.VariableImpresion.Nombre}");
                    sb.AppendLine("MOV AH,09H");
                    sb.AppendLine("INT 21H");
                    break;
            }
            return sb;
        }
    }
}
