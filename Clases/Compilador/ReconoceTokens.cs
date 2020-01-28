using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using My8086.Clases.Advertencias;

namespace My8086.Clases.Compilador
{
    public class ReconoceTokens
    {
        private readonly ResultadosCompilacion ResultadosCompilacion;
        private readonly PropiedadesPrograma Propiedades;
        private static string[] PalabrasReservadas =
            new string[]
            {
                "Si", "Sino", "Byte", "//",".", "Word", "Cadena", "Consola", "Inicia", "Termina",
                "Def","No"
            };
        public ReconoceTokens(ResultadosCompilacion ResultadosCompilacion, PropiedadesPrograma Propiedades)
        {
            this.ResultadosCompilacion = ResultadosCompilacion;
            this.Propiedades = Propiedades;
        }
        internal bool EsDeclaracionDeVariable(string texto, int Linea)
        {
            texto = Normalizar(texto);
            //Si define una cadena
            if (DeclaracionCadena(texto, Linea))
            {
                return true;
            }
            else if (DeclaracionByte(texto, Linea))
            {
                return true;
            }
            else if (DeclaracionPalabra(texto, Linea))
            {
                return true;
            }
            return false;
        }
        private bool DeclaracionCadena(string texto, int Linea)
        {
            Regex regex = new Regex(@"(Def\s*Cadena)+\s*(([a-zA-Z]+[0-9a-zA-Z]|_*)*)\s*=\s*(""(([0-9a-zA-Z]|\s|\\n|.)*)"");");
            Match match = regex.Match(texto);
            if (!match.Success)
            {
                return false;
            }
            if (match.Groups.Count != 7)
            {
                return false;
            }

            string identificador = match.Groups[2].Value;
            if (EsPalabraReservada(identificador))
            {
                this.ResultadosCompilacion.ResultadoCompilacion("El identificador de la cadena no puede ser una palabra reservada...", Linea);
                return false;
            }
            string valor = match.Groups[5].Value;
            this.Propiedades.Variables.Add(new Variable(identificador, valor, Variable.TipoDato.Cadena));
            return true;
        }
        private bool DeclaracionByte(string texto, int Linea)
        {
            Regex regex = new Regex("(Def\\s*Byte)+\\s*(([a-zA-Z]+[0-9a-zA-Z]||_+)*)\\s*=\\s*([0-9]*h);");
            Match match = regex.Match(texto);
            if (!match.Success)
            {
                return false;
            }
            if (match.Groups.Count != 5)
            {
                return false;
            }
            string valor = match.Groups[4].Value;
            int val = 0;
            if (valor.EndsWith("h"))
            {
                valor = valor.Replace("h", "");
                val = Convert.ToInt32(valor, 16);
            }
            else
            {
                val = Convert.ToInt32(valor);
            }

            string identificador = match.Groups[2].Value;
            if (EsPalabraReservada(identificador))
            {
                this.ResultadosCompilacion.ResultadoCompilacion("El identificador de la palabra no puede ser una palabra reservada...", Linea);
                return false;
            }
            this.Propiedades.Variables.Add(new Variable(identificador, val, Variable.TipoDato.Byte));
            return true;
        }
        private bool DeclaracionPalabra(string texto, int Linea)
        {
            Regex regex = new Regex("(Def\\s*Word)+\\s*(([a-zA-Z]+[0-9a-zA-Z]||_+)*)\\s*=\\s*([0-9]*h);");
            Match match = regex.Match(texto);
            if (!match.Success)
            {
                return false;
            }
            if (match.Groups.Count != 5)
            {
                return false;
            }
            string valor = match.Groups[4].Value;
            int val = 0;
            if (valor.EndsWith("h"))
            {
                valor = valor.Replace("h", "");
                val = Convert.ToInt32(valor, 16);
            }
            else
            {
                val = Convert.ToInt32(valor);
            }

            string identificador = match.Groups[2].Value;
            if (EsPalabraReservada(identificador))
            {
                this.ResultadosCompilacion.ResultadoCompilacion("El identificador de la palabra no puede ser una palabra reservada...", Linea);
                return false;
            }

            this.Propiedades.Variables.Add(new Variable(identificador, val, Variable.TipoDato.Word));
            return true;
        }
        public bool EsIncio(string texto, int linea)
        {
            texto = Normalizar(texto);
            Regex regex = new Regex(@"Inicia\s*([a-zA-Z]+[0-9a-zA-Z]+)");
            Match matcher = regex.Match(texto);
            if (!matcher.Success)
            {
                return false;
            }

            if (matcher.Groups.Count != 2)
            {
                return false;
            }

            string Titulo = matcher.Groups[1].Value?.Trim();
            if (string.IsNullOrEmpty(Titulo))
            {
                return false;
            }

            if (EsPalabraReservada(Titulo))
            {
                return false;
            }

            if (this.Propiedades.Titulo != null)
            {
                this.ResultadosCompilacion.ResultadoCompilacion("La función principal esta definida dos veces.", linea);
            }
            else
            {
                this.Propiedades.Titulo = Titulo;
            }
            return true;
        }
        public bool EsFin(string texto, int linea)
        {
            texto = Normalizar(texto);
            Regex regex = new Regex(@"Termina\s*([a-zA-Z]+[0-9a-zA-Z]+)");
            Match matcher = regex.Match(texto);
            if (!matcher.Success)
            {
                return false;
            }

            if (matcher.Groups.Count != 2)
            {
                return false;
            }

            string Titulo = matcher.Groups[1].Value?.Trim();
            if (string.IsNullOrEmpty(Titulo))
            {
                return false;
            }

            if (EsPalabraReservada(Titulo))
            {
                return false;
            }

            if (this.Propiedades.Titulo != Titulo)
            {
                this.ResultadosCompilacion.ResultadoCompilacion("La etiqueta de cierre para la función principal difiere de la etiqueta de apertura.", linea);
                return false;
            }
            else if (this.Propiedades.Cerrado)
            {
                this.ResultadosCompilacion.ResultadoCompilacion("La etiqueta de cierre para la función principal ya ha sido cerrada antes.", linea);
                return false;
            }
            else
            {
                this.Propiedades.Cerrado = true;
            }
            return true;
        }
        internal bool EsUnaFuncionDeConsola(string texto, int Linea)
        {
            texto = Normalizar(texto);
            if (EsImpresionDeConsola(texto, Linea))
            {
                return true;
            }

            if (EsLimpiarConsola(texto, Linea))
            {
                return true;
            }
            return false;
        }
        private bool EsLimpiarConsola(string texto, int Linea)
        {
            Regex regex = new Regex(@"Consola.LimpiarPantalla\(\);");
            Match match = regex.Match(texto);
            if (!match.Success)
            {
                return false;
            }
            this.Propiedades.Acciones.Add(new Accion(Accion.Acciones.LimpiarConsola));
            return true;
        }
        private bool EsImpresionDeConsola(string texto, int Linea)
        {
            Regex regex = new Regex(
                @"Consola.Imprimir\(((""([0-9a-zA-Z]|\s|\\n|\.)+"")|(([a-zA-Z]+[0-9a-zA-Z]*)))\);");
            Match match = regex.Match(texto);
            if (!match.Success)
            {
                return EsImpresionDeConsolaXY(texto, Linea);
            }

            if (match.Groups.Count != 6)
            {
                return false;
            }
            string argumento = match.Groups[1].Value;
            if (string.IsNullOrEmpty(argumento))
            {
                this.ResultadosCompilacion.ResultadoCompilacion("Hace falta un argumento para la función Imprimir de Consola.", Linea);
            }

            if (this.Propiedades.Variables.FirstOrDefault(x => x.Identificador == argumento) is Variable var)
            {
                this.Propiedades.Acciones.Add(new Accion(Accion.Acciones.ImprimirPorConsola, var));
            }
            else
            {
                if (argumento.StartsWith("\""))
                {
                    argumento = argumento.Replace("\"", "").Trim();
                    Variable nuevaVariable = new Variable($"AutoVariable{this.Propiedades.Variables.Count}", argumento,
                        Variable.TipoDato.Cadena);
                    this.Propiedades.Variables.Add(nuevaVariable);
                    this.Propiedades.Acciones.Add(new Accion(Accion.Acciones.ImprimirPorConsola, nuevaVariable));
                }
                else
                {
                    this.ResultadosCompilacion.ResultadoCompilacion($"Uso de variable no declarada en Consola.Imprimir({argumento});", Linea);
                    return false;
                }

            }
            return true;
        }
        private bool EsImpresionDeConsolaXY(string texto, int Linea)
        {
            Regex regex = new Regex(
                @"Consola.Imprimir\(((""([0-9a-zA-Z]|\s|\\n|\.)+"")|(([a-zA-Z]+[0-9a-zA-Z]*)))((,([0-9]+),([0-9]+)))+\);");
            Match match = regex.Match(texto);
            if (!match.Success)
            {
                return false;
            }

            if (match.Groups.Count != 10)
            {
                return false;
            }
            string argumento = match.Groups[1].Value;
            byte x = Convert.ToByte(match.Groups[8].Value);
            byte y = Convert.ToByte(match.Groups[9].Value);
            if (string.IsNullOrEmpty(argumento))
            {
                this.ResultadosCompilacion.ResultadoCompilacion("Hace falta un argumento para la función Imprimir de Consola.", Linea);
                return false;
            }

            if (this.Propiedades.Variables.FirstOrDefault(variable => variable.Identificador == argumento) is Variable var)
            {
                this.Propiedades.Acciones.Add(new Accion(Accion.Acciones.ImprimirPorConsolaXY, var, x, y));
            }
            else
            {
                if (argumento.StartsWith("\""))
                {
                    argumento = argumento.Replace("\"", "").Trim();
                    Variable nuevaVariable = new Variable($"AutoVariable{this.Propiedades.Variables.Count}", argumento,
                        Variable.TipoDato.Cadena);
                    this.Propiedades.Variables.Add(nuevaVariable);
                    this.Propiedades.Acciones.Add(new Accion(Accion.Acciones.ImprimirPorConsolaXY, nuevaVariable, x, y));
                }
                else
                {
                    this.ResultadosCompilacion.ResultadoCompilacion($"Uso de variable no declarada en Consola.Imprimir({argumento});", Linea);
                    return false;
                }

            }
            return true;
        }
        private bool EsPalabraReservada(string titulo)
        {
            foreach (string token in ReconoceTokens.PalabrasReservadas)
            {
                if (titulo.Trim().ToUpper() == token)
                {
                    return true;
                }
            }

            return false;
        }
        private string Normalizar(string texto)
        {
            //texto = Regex.Replace(texto, "[^0-9a-zA-Z]+", "");
            texto = texto.Trim();
            return texto;
        }
    }
}
