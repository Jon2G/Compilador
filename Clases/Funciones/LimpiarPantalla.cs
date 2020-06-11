﻿using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My8086.Clases.Funciones
{
    class LimpiarPantalla : Accion
    {
        public LimpiarPantalla(Programa programa,LineaLexica linea):base(programa,linea,1)
        {

        }
        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            if (this.Argumentos.Count < 3) 
            {
                return false;
            }
            if (this.Argumentos[0].TipoToken == TipoToken.ParentesisAbierto )
            {
                if(this.Argumentos[1].TipoToken == TipoToken.ParentesCerrado)
                {
                    if(this.Argumentos[2].TipoToken == TipoToken.FinInstruccion)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();

            sb
                .AppendLine(";================>[LIMPIAR PANTALLLA]<================")
                .AppendLine("mov ax,0600h  ")
                .AppendLine(" mov bh,01Fh ")
                .AppendLine(" mov cx,0000h ")
                .AppendLine(" mov dx,184Fh ")
                .AppendLine("int 10h ")
                .AppendLine(";================>[MOVER CURSOR A 0]<================")
                .AppendLine("XOR DX,DX; dl Columna,dh fila")
                .AppendLine("MOV bh,0 ;PAGINA")
                .AppendLine(" mov ah, 02h ;mover cursor")
                .AppendLine("int  10h; ;servicio de video");

            return sb;
        }
    }
}