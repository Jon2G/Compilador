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
        private readonly List<Advertencias.WarningError> ResultadosCompilacion;
        private readonly PropiedadesPrograma Propiedades;
        private static string[] PalabrasReservadas =
            new string[]
            {
                "Si", "Sino", "Byte", "//",".", "Word", "Cadena", "Consola", "Inicia", "Termina",
                "Def","No"
            };
        public ReconoceTokens(List<Advertencias.WarningError> ResultadosCompilacion, PropiedadesPrograma Propiedades)
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
            Regex regex = new Regex(@"(Def\s*Cadena)+\s*(([a-zA-Z]+[0-9a-zA-Z]|_*)*)\s*=\s*(""(([0-9a-zA-Z]|\s|\\n)*)"");");
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
                ErrorCompilacion error = new ErrorCompilacion(false, "El identificador de la cadena no puede ser una palabra reservada...", Linea);
                this.ResultadosCompilacion.Add(new WarningError(error));
                throw error;
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
                ErrorCompilacion error = new ErrorCompilacion(false, "El identificador de la palabra no puede ser una palabra reservada...", Linea);
                this.ResultadosCompilacion.Add(new WarningError(error));
                throw error;
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
                ErrorCompilacion error = new ErrorCompilacion(false, "El identificador de la palabra no puede ser una palabra reservada...", Linea);
                this.ResultadosCompilacion.Add(new WarningError(error));
                throw error;
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
                ErrorCompilacion error = new ErrorCompilacion(false,
                    "La función principal esta definida dos veces.", linea);
                this.ResultadosCompilacion.Add(new WarningError(error));
                throw error;
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
                ErrorCompilacion error = new ErrorCompilacion(false,
                    "La etiqueta de cierre para la función principal difiere de la etiqueta de apertura.", linea);
                this.ResultadosCompilacion.Add(new WarningError(error));
                throw error;
            }
            else if(this.Propiedades.Cerrado)
            {
                ErrorCompilacion error = new ErrorCompilacion(false,
                    "La etiqueta de cierre para la función principal ya ha sido cerrada antes.", linea);
                this.ResultadosCompilacion.Add(new WarningError(error));
                throw error;
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
            return false;
        }

        private bool EsImpresionDeConsola(string texto, int Linea)
        {
            Regex regex = new Regex(@"Consola.(Imprimir\(((""([0-9a-zA-Z]|\s)*"")|(([a-zA-Z]+[0-9a-zA-Z]+)*))\));");
            Match match = regex.Match(texto);
            if (!match.Success)
            {
                return false;
            }

            if (match.Groups.Count != 7)
            {
                return false;
            }
            string argumento = match.Groups[2].Value;
            if (string.IsNullOrEmpty(argumento))
            {
                ErrorCompilacion error = new ErrorCompilacion(false,
                    "Hace falta un argumento para la función Imprimir de Consola.", Linea);
                this.ResultadosCompilacion.Add(new WarningError(error));
                throw error;
            }

            if (this.Propiedades.Variables.FirstOrDefault(x => x.Identificador == argumento) is Variable var)
            {
                this.Propiedades.Acciones.Add(new Accion(Accion.Acciones.ImprimirPorConsola, var));
            }
            else
            {
                if (argumento.StartsWith("\""))
                {
                    this.Propiedades.Acciones.Add(new Accion(Accion.Acciones.ImprimirPorConsola, argumento));
                }
                else
                {
                    ErrorCompilacion error = new ErrorCompilacion(false,
                        $"Uso de variable no declarada en Consola.Imprimir({argumento});", Linea);
                    this.ResultadosCompilacion.Add(new WarningError(error));
                    throw error;
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
