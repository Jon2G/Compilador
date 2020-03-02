using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Compilador;
using My8086.Clases.Fases._1._Analisis_Lexico;
using My8086.Clases.Funciones;

namespace My8086.Clases.Fases._2._Analisis_Sintactico
{
    class AnalizadorSintactico : Base.Analizador
    {
        private Funcion FuncionActual;
        private readonly AnalizadorLexico Lexica;
        internal readonly Programa Programa;
        private LineaLexica[] LineasLexicas => Lexica.LineasLexicas.ToArray();
        public AnalizadorSintactico(AnalizadorLexico Lexica, TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.Lexica = Lexica;
            this.Programa = new Programa();
        }

        public override void Analizar()
        {
            foreach (LineaLexica linea in this.LineasLexicas)
            {
                if (IniciaPrograma(linea))
                    continue;
                if (TerminaPrograma(linea))
                    continue;
                if (IniciaFuncion(linea))
                    continue;
                if (Ejecutar(linea))
                    continue;
                if (Imprime(linea))
                    continue;
                if (Desde(linea))
                    continue;
                if (FinDesde(linea))
                    continue;
                if (DeclaraVariable(linea))
                    continue;
                this.Errores.ResultadoCompilacion("Sentencia no reconocida", linea.LineaDocumento);
            }

            RevisarCierres();
            this.EsValido = this.Errores.SinErrores;
        }

        private void RevisarCierres()
        {
            foreach (Funcion fx in this.Programa.Funciones.Where(x => !x.Cerrado))
            {
                this.Errores.ResultadoCompilacion($"No se cerro la función '{fx.Titulo}'", null);
            }
        }
        private bool IniciaPrograma(LineaLexica linea)
        {
            if (linea[0].Lexema == "Inicia")
            {
                if (linea[1].TipoToken == TipoToken.Identificador)
                {
                    if (this.Programa.FuncionPrincipal != null)
                    {
                        this.Errores.ResultadoCompilacion("Se declaro la función principal mas de una vez.",
                            linea.LineaDocumento);
                        return true;
                    }

                    if ((!this.FuncionActual?.Cerrado) ?? false)
                    {
                        this.Errores.ResultadoCompilacion("No puede iniciar una nueva función de usuario si no ha cerrado el bloque anterior.",
                            linea.LineaDocumento);
                        return true;
                    }
                    this.FuncionActual =
                    this.Programa.FuncionPrincipal = new Funcion(linea[1].Linea, linea[1].Lexema);
                    return true;
                }
            }
            return false;
        }
        private bool TerminaPrograma(LineaLexica linea)
        {
            if (linea[0].Lexema == "Termina")
            {
                if (linea[1].TipoToken == TipoToken.Identificador)
                {
                    if (this.FuncionActual is null)
                    {
                        this.Errores.ResultadoCompilacion("No se encontro ninguna función a la que le corresponda el cierre.",
                            linea.LineaDocumento);
                        return true;
                    }

                    if (this.FuncionActual.Titulo == linea[1].Lexema)
                    {
                        if (this.FuncionActual.Cerrado)
                        {
                            this.Errores.ResultadoCompilacion("La función principal ya habia sido cerrada anteriormente.",
                                linea.LineaDocumento);
                        }
                        else
                        {
                            this.FuncionActual.Cerrar(linea.LineaDocumento);
                        }
                    }
                    else
                    {
                        this.Errores.ResultadoCompilacion("La etiqueta de apertura y cierre de la función son diferentes!.",
                            linea.LineaDocumento);
                    }
                    return true;
                }
            }
            return false;
        }
        private bool IniciaFuncion(LineaLexica linea)
        {
            if (linea[0].Lexema == "Funcion")
            {
                if (linea[1].TipoToken == TipoToken.Identificador)
                {
                    if (this.Programa.Titulo == linea[1].Lexema)
                    {
                        this.Errores.ResultadoCompilacion("Una función de usuario no debe llevar el mismo nombre que la función principal.",
                            linea.LineaDocumento);
                        return true;
                    }
                    if ((!this.FuncionActual?.Cerrado) ?? false)
                    {
                        this.Errores.ResultadoCompilacion("No puede iniciar una nueva función de usuario si no ha cerrado el bloque anterior.",
                            linea.LineaDocumento);
                        return true;
                    }
                    if (this.Programa.Funciones.Any(x => x.Titulo.ToUpper() == linea[1].Lexema))
                    {
                        this.Errores.ResultadoCompilacion($"Ya existe una función de usuario con el nombre '{linea[1].Lexema}'",
                            linea.LineaDocumento);
                        return true;
                    }

                    this.FuncionActual = new Funcion(linea.LineaDocumento, linea[1].Lexema);
                    this.Programa.Funciones.Add(this.FuncionActual);
                    return true;
                }
            }
            return false;
        }
        private bool Imprime(LineaLexica linea)
        {
            if (linea[0].Lexema != "Imprimir")
            {
                return false;
            }
            this.FuncionActual.AgregarAccion(new Imprimir(linea, 1));
            return true;
        }

        private bool Ejecutar(LineaLexica linea)
        {
            if (linea[0].Lexema != "Ejecutar")
            {
                return false;
            }
            this.FuncionActual.AgregarAccion(new Ejecutar(linea, 1));
            return true;
        }
        private bool Desde(LineaLexica linea)
        {
            if (linea[0].Lexema == "Desde"
                && linea[2].Lexema == "hasta"
                && linea[4].Lexema == "hacer")
            {
                Token desde = linea[1];
                Token hasta = linea[3];
                this.FuncionActual.AgregarAccion(new Desde(linea));
                return true;
            }
            return false;
        }
        private bool FinDesde(LineaLexica linea)
        {
            if (linea[0].Lexema == "FinDesde")
            {
                Desde desde = this.FuncionActual.Acciones.OfType<Desde>().LastOrDefault();
                if (desde != null)
                {
                    if (!desde.Cerrado)
                    {
                        desde.Cerrar(linea.LineaDocumento);
                        return true;
                    }
                    else
                    {
                        this.Errores.ResultadoCompilacion("El ciclo Desde ya habia sido cerrado previamente", linea.LineaDocumento);
                    }
                }
                else
                {
                    this.Errores.ResultadoCompilacion("No se encontro ningun cliclo para cerra en este contexto", linea.LineaDocumento);
                }

            }
            return false;
        }
        private bool DeclaraVariable(LineaLexica linea)
        {
            if (linea.Elementos != 4)
            {
                return false;
            }
            if (linea[0].TipoToken == TipoToken.PalabraReservada &&
                linea[0].Lexema=="Entero" ||
                linea[0].Lexema == "Cadena"||
                linea[0].Lexema == "Byte"||
                linea[0].Lexema == "Caracter"
                )
            {
                Variable variableNueva = new Variable(linea);
                if (this.FuncionActual.Variables.Any(x => x.Nombre == variableNueva.Nombre))
                {
                    this.Errores.ResultadoCompilacion($"La variable '{variableNueva.Nombre}' ya fué declarada en este contexto.", linea.LineaDocumento);
                }
                else
                {
                    this.FuncionActual.Acciones.Add(variableNueva);
                    this.FuncionActual.Variables.Add(variableNueva);
                }

                return true;
            }
            return false;
        }
    }
}
