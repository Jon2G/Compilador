using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases._1._Analisis_Lexico;
using My8086.Clases.Fases._2._Analisis_Sintactico;
using My8086.Clases.Fases._3._Analisis_Semantico;
using My8086.Clases.Fases._4._Sintetizador;

namespace My8086.Clases.Compilador
{
    public class Compilador
    {
        private readonly ResultadosCompilacion ResultadosCompilacion;
        private readonly TextDocument Document;
        public EventHandler OnProgreso;
        private readonly BorlandC TurboC;
        public StringBuilder ASM { get; private set; }
        public bool Compilado { get; set; }

        private double IProgreso { get; set; }

        public double Progreso
        {
            get => IProgreso;
            set
            {
                IProgreso = value;
                OnProgreso?.Invoke(this, null);
            }
        }

        public Compilador(TextDocument Document, ResultadosCompilacion ResultadosCompilacion)
        {
            this.Document = Document;

            this.ResultadosCompilacion = ResultadosCompilacion;
            this.TurboC = new BorlandC();
            //this.ReconceTokens = new ReconoceTokens(this.ResultadosCompilacion, this.PropiedadesPrograma);
        }
        public string Compilar()
        {

            this.Progreso = 0;
            this.ResultadosCompilacion.Clear();
            //try
            //{
            //Fase 1 Analisis Lexico
            Fases.Base.Analizador analizador
                 = new AnalizadorLexico(this.Document, this.ResultadosCompilacion);
            analizador.Analizar();
            if (analizador.EsValido)
            {
                //Fase 2 Analisis Sintactico
                analizador =
                    new AnalizadorSintactico((AnalizadorLexico)analizador, this.Document, this.ResultadosCompilacion);
                analizador.Analizar();
                if (analizador.EsValido)
                {
                    //Fase 3 Analisis Semantico
                    analizador =
                        new AnalizadorSemantico((AnalizadorSintactico)analizador, this.Document, this.ResultadosCompilacion);
                    analizador.Analizar();
                    if (analizador.EsValido)
                    {
                        //Fase 4 Sintetizador
                        analizador =
                            new Sintesis((AnalizadorSemantico)analizador, this.Document, this.ResultadosCompilacion);
                        analizador.Analizar();
                        if (analizador.EsValido)
                        {
                            CodigoIntermedio intermedio = new CodigoIntermedio((Sintesis)analizador);
                            intermedio.Generar();
                            this.TurboC.Limpiar();
                            this.Compilado = this.TurboC.GeneraEjecutable(intermedio.Codigo.ToString());
                            this.ASM = intermedio.Codigo;

                            return this.TurboC.ResultadosCompilacion;
                        }
                    }
                }
            }
            this.Compilado = false;
            return "Se encontrarón errores previos a la compilación\n";
        }

        public string Ejecutar()
        {
            if (this.Compilado)
            {
               this.TurboC.Ejecutar();
               return this.TurboC.ResultadosCompilacion;
            }

            return "No se ha compilado el archivo";
        }
    }
}
