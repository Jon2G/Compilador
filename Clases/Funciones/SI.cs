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
    class Si : Accion, IBloque
    {
        public DocumentLine InicioBloque { get; set; }
        public DocumentLine FinBloque { get; set; }
        public readonly OperacionLogica[] Argumentos;
        public Sino Sino { get; set; }
        public Si(Programa Programa, LineaLexica Linea,params OperacionLogica[] Argumentos) : base(Programa, Linea, 0)
        {
            this.Argumentos = Argumentos;
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }

        public override StringBuilder Traduccion()
        {
            throw new NotImplementedException();
        }

        StringBuilder IBloque.CerrarBloque()
        {
            throw new NotImplementedException();
        }
    }
}
