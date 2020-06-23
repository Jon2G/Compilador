using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Compilador;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;
using My8086.Clases.Funciones.CodigoTresDirecciones;

namespace My8086.Clases.Funciones
{
    internal class For : Accion, IBloque
    {
        private static int ConsecutivoSalto = 0;
        private readonly string IdentifiacadorSalto;
        public DocumentLine InicioBloque { get; set; }
        public DocumentLine FinBloque { get; set; }
        //private Variable Contador;
        private OperacionArtimetica ContadorOp { get; set; }
        private OperacionArtimetica Incremento { get; set; }
        private readonly OperacionesLogicas OperacionLogica;
        public For(Programa Programa, LineaLexica Linea, Variable Contador, OperacionesLogicas OperacionLogica) : base(Programa, Linea, 1)
        {
            //this.Contador = Contador;
            this.OperacionLogica = OperacionLogica;
            //this.Contador = new Variable(Programa, $"CONTADOR_FOR{++For.ConsecutivoSalto}", TipoDato.Entero) { EsAutomatica = true };
            
            int FinContador=Linea.Tokens.FindIndex(x => x.Lexema == ";");
            this.ContadorOp = new OperacionArtimetica(Programa, Contador, Linea, 3, FinContador+1);

            int inicioIncremento = Linea.Tokens.FindLastIndex(x => x.Lexema == ";");
            this.Incremento = new OperacionArtimetica(Programa, Contador, Linea, inicioIncremento+2, Linea.Elementos - 1);

            this.OperacionLogica = OperacionLogica;
            this.IdentifiacadorSalto = (++For.ConsecutivoSalto).ToString();
            OperacionLogica.DeclararTemporales();

        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return this.ContadorOp.RevisarSemantica(Errores) && this.Incremento.RevisarSemantica(Errores);
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"INICIO_FOR_{IdentifiacadorSalto}:");//las mismas funciones del sino
            sb.Append(this.ContadorOp.Traduccion());
            sb.AppendLine($"INICIO_CICLO_{IdentifiacadorSalto}:");
            return sb;
        }

        public StringBuilder CerrarBloque()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Incremento.Traduccion())
                .Append(this.OperacionLogica.GenerarAsm())
                .AppendLine("MOV AL,1H")
                .AppendLine("CMP R_COMPARADOR,AL")
                .AppendLine($"JE INICIO_CICLO_{this.IdentifiacadorSalto}");
            return sb;
        }
    }
}
