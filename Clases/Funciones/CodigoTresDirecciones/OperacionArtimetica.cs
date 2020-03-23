using System.Collections.Generic;
using System.Text;
using My8086.Clases.Advertencias;
using My8086.Clases.Fases;
using My8086.Clases.Fases._1._Analisis_Lexico;

namespace My8086.Clases.Funciones.CodigoTresDirecciones
{
    class OperacionArtimetica : Accion
    {
        private readonly Variable Variable;
        private TresDirecciones TresDirecciones;
        public OperacionArtimetica(Programa Programa, Variable Variable, LineaLexica Linea, int  InicioArgumentos=1,int FinArgumentos=-1) : base(Programa, Linea, InicioArgumentos,FinArgumentos)
        {
            this.Variable = Variable;
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            this.Variable.HacerReferencia();
            if (this.Argumentos.Count == 3)
            {
                if (!this.Programa.Expresiones.Evaluar(this.Programa.Expresiones.OperadoresAritmeticos, this.Argumentos[0].Lexema))
                {
                    return false;
                }

                if (this.Argumentos[2].TipoToken == Fases.TipoToken.FinInstruccion
                    || this.Argumentos[2].TipoToken == Fases.TipoToken.ParentesCerrado)
                {
                    this.TresDirecciones = new TresDirecciones
                    {
                        Variable = this.Variable,
                        ArbolDeDirecciones = new TokenAritmetico(this.Argumentos[1], TipoToken.NumeroPositivo)
                    };
                    this.TresDirecciones.DeclararTemporales(this.Programa);
                    return true;
                }
            }

            if (this.Argumentos[0].Lexema == ":=")
            {
                this.TresDirecciones = new TresDirecciones(TokenAritmetico.Analizar(this.Argumentos.GetRange(1, this.Argumentos.Count - 1),this.Programa))
                {
                    Variable = this.Variable
                };
                this.TresDirecciones.DeclararTemporales(this.Programa);
                return true;
            }
            return false;
        }

        public override StringBuilder Traduccion()
        {
            return this.TresDirecciones.GenerarAsm();
        }
    }
}
