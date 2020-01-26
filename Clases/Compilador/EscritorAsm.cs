using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My8086.Clases.Compilador
{
    public class TraductorAsm
    {
        private readonly StringBuilder Asm;
        private readonly PropiedadesPrograma Propiedades;
        public TraductorAsm(PropiedadesPrograma Propiedades)
        {
            this.Asm = new StringBuilder();
            this.Propiedades = Propiedades;
        }
        public string ObtenerAsm()
        {
            this.Asm.AppendLine(";--------------[INICIO]--------------");

            this.Asm.AppendLine(".model small ;Directiva de inicio simplificada para el modelo de datos 'SMALL'");
            this.Asm.AppendLine(".stack ;Directiva de inicializacion de la pila simplificada");
            this.Asm.AppendLine(".data ;Directiva de inicializacion de el segmento de datos simplificado");
            this.Asm.AppendLine(";-----[INICIA SEGMENTO DE DATOS]-----");
            this.Propiedades.Variables.ForEach(EscribeVariable);
            this.Asm.AppendLine(";----[TERMINA SEGMENTO DE DATOS]-----");
            this.Asm.AppendLine(".code");
            this.Asm.AppendLine(";-----[INICIA PROCEDIMIENTO PRINCIPAL]-----");
            //Procedimiento principal
            this.Asm.Append(this.Propiedades.Titulo);
            this.Asm.AppendLine(" proc FAR");
            this.Propiedades.Acciones.ForEach(EscribeAccion);
            this.Asm.AppendLine("");
            this.Asm.Append("end ");
            this.Asm.AppendLine(";-----[TERMINA PROCEDIMIENTO PRINCIPAL]-----");
            this.Asm.AppendLine(this.Propiedades.Titulo);
            this.Asm.AppendLine(";----------------[FIN]---------------");
            return this.Asm.ToString();
        }

        private void EscribeVariable(Variable variable)
        {
            string tipo = null;
            this.Asm.Append(variable.Identificador);
            this.Asm.Append("\t");
            switch (variable.Tipo)
            {
                case Variable.TipoDato.Cadena:
                case Variable.TipoDato.Byte:
                    tipo = "db";
                    break;
                case Variable.TipoDato.Word:
                    tipo = "dw";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.Asm.Append(tipo);
            this.Asm.Append("\t");
            switch (variable.Tipo)
            {
                case Variable.TipoDato.Word:
                case Variable.TipoDato.Byte:
                    this.Asm.Append(variable.Valor.ToString());
                    this.Asm.AppendLine("d");
                    break;
                case Variable.TipoDato.Cadena:
                    this.Asm.Append("\"");
                    int i = 0;
                    foreach (var caracter in variable.Valor.ToString())
                    {
                        if (i >= 20)
                        {
                            this.Asm.Append(caracter);
                            this.Asm.AppendLine("\"");
                            this.Asm.Append("\tdb \"");
                            i = 0;
                        }
                        else
                        {
                            this.Asm.Append(caracter);
                            i++;
                        }
                    }

                    this.Asm.AppendLine("\"");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EscribeAccion(Accion accion)
        {

        }
    }
}
