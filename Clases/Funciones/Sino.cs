﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones
{
    class Sino : Accion, IBloque
    {
        public string IdentifiacadorSalto;
        public DocumentLine InicioBloque { get; set; }
        public DocumentLine FinBloque { get; set; }
        public Si Si { get;private set; }

        public Sino(Si si,Programa Programa, LineaLexica Linea) : base(Programa, Linea, 0)
        {
            this.Si = si;
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            if (this.Si != null)//berifica si hay un si
            {
                IBloque bloque = this.Si;
                while (bloque!=null)
                {
                    if (bloque is Si si)
                    {
                        if (si.Sino != null)
                        {
                            bloque = si.Sino;
                            continue;
                        }
                        bloque = si;
                    }
                    else { break; }
                }
                if (bloque!=null)
                {
                    if(bloque is Si s)
                    {
                        sb.AppendLine($"JMP IF_FALSO_{s.IdentifiacadorSalto}");// si la condicion es false salta al las acciones del falso 

                    }
                    else if(bloque is Sino ss)
                    {
                        sb.AppendLine($"JMP FIN_SINO{ss.IdentifiacadorSalto}");// si no hay un sino salta al final 
                    }
                }
            }
            sb.AppendLine($"INICIO_SINO_{this.IdentifiacadorSalto}:");//si hay un sino salta al inicio del sino 
            return sb;
        }

        public StringBuilder CerrarBloque()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"FIN_SINO{this.IdentifiacadorSalto}:");//salta al final del no
            return sb;
        }
    }
}
