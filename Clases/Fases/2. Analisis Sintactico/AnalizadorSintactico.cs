﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Advertencias;
using My8086.Clases.Compilador;
using My8086.Clases.Fases._1._Analisis_Lexico;
using My8086.Clases.Funciones;
using My8086.Clases.Funciones.CodigoTresDirecciones;

namespace My8086.Clases.Fases._2._Analisis_Sintactico
{
    class AnalizadorSintactico : Base.Analizador
    {
        private readonly AnalizadorLexico Lexica;
        public Programa Programa { get; protected set; }
        private LineaLexica[] LineasLexicas => Lexica.LineasLexicas.ToArray();
        internal ExpresionesRegulares.ExpresionesRegulares Expresiones;
        public AnalizadorSintactico(AnalizadorLexico Lexica, TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.Lexica = Lexica;
            this.Expresiones = Lexica.Expresiones;
        }

        public override void Analizar()// lo hace por cada lista lexica
        {
            foreach (LineaLexica linea in this.LineasLexicas)//regrese una lista de acciones 
            {
                if (NombrePrograma(linea))
                {
                    continue;
                }

                if (
                    DeclaraVariable(linea)
                    || IniciaPrograma(linea)
                    || TerminaPrograma(linea)
                    || Imprime(linea)
                    || Lee(linea)
                    || Si(linea)
                    || Sino(linea)
                    || Begin(linea)
                    || End(linea)
                    || For(linea)
                    || AsignacionCadena(linea)
                    || UsoVariableNumerica(linea)
                    || LimpiarPantalla(linea)
                    || Inicia8086(linea)
                    || Ensamblador(linea)
                    )
                {
                    if (this.Programa is null)
                    {
                        this.Errores.ResultadoCompilacion($"No se inicializo el nombre del programa!",
                            linea.LineaDocumento);
                        break;
                    }
                    continue;
                }
                if (
                    NombrePrograma(linea)

                    )
                    continue;

                this.Errores.ResultadoCompilacion("Sentencia no reconocida", linea.LineaDocumento);
            }

            if (this.Programa is null)
            {
                this.Errores.ResultadoCompilacion($"No se inicializo el nombre del programa!", null);
                this.EsValido = false;
                return;
            }

            RevisarBloques();
            this.EsValido = this.Errores.SinErrores;
        }
        private void RevisarBloques()
        {
            foreach (Accion fx in this.Programa.Acciones.OfType<IBloque>()
                .Where(x => x.InicioBloque is null || x.FinBloque is null).OfType<Accion>())
            {
                this.Errores.ResultadoCompilacion($"No se cerro la función '{fx.GetType().Name}'", fx.LineaDocumento);
            }
        }
        private bool NombrePrograma(LineaLexica linea)
        {
            if (linea[0].Lexema == "Program")//espera que la primera linea sea program
            {
                if (linea[1].TipoToken == TipoToken.PalabraReservada)// debe empezar con Nom.
                {
                    if (linea[1].Lexema.StartsWith("Nom."))
                    {
                        //si se inicializo el nombre
                        if (this.Programa != null)// daria error si se escribe dos veces
                        {
                            this.Errores.ResultadoCompilacion($"El nombre del programa ya ha sido declarado como '{this.Programa.Titulo}'",
                                linea.LineaDocumento);
                            return true;
                        }

                        string nombre = linea[1].Lexema.Replace("Nom.", "").Trim();//remplasa NOM. con una cadena vacia
                        this.Programa = new Programa(nombre, this.Expresiones);
                        if (string.IsNullOrEmpty(nombre))
                        {
                            this.Errores.ResultadoCompilacion("El nombre del programa no puede estar vacio.",
                                linea.LineaDocumento);
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        private bool IniciaPrograma(LineaLexica linea)// debe tener la palabra reservada begin 
        {
            if (linea[0].Lexema == "Begin")
            {
                if (this.Programa.Inicia != null)
                {
                    this.Errores.ResultadoCompilacion("Se declaro la función principal mas de una vez.",
                        linea.LineaDocumento);
                    return true;
                }

                this.Programa.Inicia = linea.LineaDocumento;// asigna la linea de inicio de programa 
                return true;
            }
            return false;
        }
        private bool TerminaPrograma(LineaLexica linea)
        {
            if (linea[0].Lexema == "End")
            {
                if (linea[1].TipoToken == TipoToken.FinInstruccion)
                {
                    if (this.Programa is null)
                    {
                        this.Errores.ResultadoCompilacion("No se encontro ninguna función a la que le corresponda el cierre.",
                            linea.LineaDocumento);
                        return true;
                    }
                    if (this.Programa.FinBloque != null)
                    {
                        this.Errores.ResultadoCompilacion("La función principal ya habia sido cerrada anteriormente.",
                            linea.LineaDocumento);
                    }
                    else
                    {
                        this.Programa.FinBloque = linea.LineaDocumento;
                    }
                    return true;
                }
            }
            return false;
        }
        private bool Imprime(LineaLexica linea)
        {
            if (linea[0].Lexema != "Imprime")
            {
                return false;
            }

            Token VariableImpresion;
            bool sumando = false;
            for (int i = 2; i < linea.Elementos; i++)
            {
                Token tk = linea[i];
                if (tk.TipoToken == TipoToken.FinInstruccion)
                {
                    continue;
                }

                if (tk.Lexema == "+")
                {
                    if (sumando)
                    {
                        Errores.ResultadoCompilacion($"Se esperaba una cadena o identificador en la función 'Imprime'",
                            linea.LineaDocumento);
                        return true;
                    }
                    sumando = true;
                    continue;
                }

                if (tk.TipoToken == TipoToken.ParentesCerrado)
                {
                    continue;
                }
                if (tk.TipoToken == TipoToken.Identificador && tk.TipoDato == TipoDato.Cadena)
                {
                    sumando = false;
                    continue;
                }
                else if (tk.TipoDato == TipoDato.Cadena && tk.TipoToken == TipoToken.Cadena)
                {
                    if (this.Programa?.SegmentoDeDatos is null)
                    {
                        this.Programa.SegmentoDeDatos = new SegmentoDeDatos();
                    }

                    //Es una cadena que no es una variable Ex. Imprime('Hola mundo');
                    //Crearemos una variable con un identificador unico que guarde esta cadena
                    Variable var = this.Programa.SegmentoDeDatos.Nueva(new Variable(this.Programa, tk, linea, tk.TipoDato));
                    VariableImpresion = new Token(var.Nombre, TipoToken.Identificador, TipoDato.Cadena, linea.LineaDocumento);

                    this.Programa.Acciones.Add(new AsignaCadena(Programa, linea, var, tk));
                    this.Programa.AgregarAccion(new Imprimir(this.Programa, VariableImpresion, linea));
                    return true;
                }
                else
                {
                    Errores.ResultadoCompilacion($"Sintaxis incorrecta cerca de '{tk.Lexema}' en la función 'Imprime'",
                        linea.LineaDocumento);
                    return true;
                }

            }

            VariableImpresion = linea[2];
            if (this.Programa?.SegmentoDeDatos is null)
            {
                this.Errores.ResultadoCompilacion($"No se aperturo el segmento de datos.", linea.LineaDocumento);
                return true;
            }
            this.Programa.AgregarAccion(new Imprimir(this.Programa, VariableImpresion, linea));
            return true;
        }
        private bool Begin(LineaLexica linea)
        {
            if (linea[0].Lexema != "begin")
            {
                return false;
            }

            if (this.Programa.Acciones.LastOrDefault() is IBloque bloque)
            {
                if (bloque.InicioBloque != null)
                {
                    this.Errores.ResultadoCompilacion($"Ya se habia declarado el inicio de este bloque en la linea '{bloque.InicioBloque.LineNumber}'.",
                        linea.LineaDocumento);
                    return true;
                }
                bloque.InicioBloque = linea.LineaDocumento;
            }
            else
            {
                this.Errores.ResultadoCompilacion($"Inicio de bloque incorrecto.",
                    linea.LineaDocumento);
            }
            return true;
        }
        private bool End(LineaLexica linea)
        {
            if (linea[0].Lexema != "end")
            {
                return false;
            }
            if (this.Programa.Acciones.OfType<IBloque>().Where(x => x.FinBloque is null).FirstOrDefault() is { } bloque)
            {
                if (bloque.FinBloque != null)
                {
                    this.Errores.ResultadoCompilacion($"Ya se habia declarado el inicio de este ciclo for en la linea '{bloque.InicioBloque.LineNumber}'.",
                        linea.LineaDocumento);
                    return true;
                }
                bloque.FinBloque = linea.LineaDocumento;
            }
            else
            {
                this.Errores.ResultadoCompilacion($"Fin de bloque incorrecto.",
                    linea.LineaDocumento);
            }
            return true;
        }
        private bool Lee(LineaLexica linea)
        {
            if (linea[0].Lexema != "Lee")
            {
                return false;
            }
            this.Programa.AgregarAccion(new Lee(Programa, linea));
            return true;
        }
        private bool LimpiarPantalla(LineaLexica linea)
        {
            if (linea[0].Lexema != "LimpiarPantalla")
            {
                return false;
            }
            this.Programa.AgregarAccion(new LimpiarPantalla(Programa, linea));
            return true;
        }
        private bool For(LineaLexica linea)
        {
            if (linea[0].Lexema == "For")
            {
                List<Token> Argumentos = new List<Token>();
                if (linea[1].TipoToken != TipoToken.ParentesisAbierto)
                {
                    this.Errores.ResultadoCompilacion($"Se esperaba un parentesis abierto", linea.LineaDocumento);
                    return true;
                }
                ////////
                Variable variableCiclo = Programa.SegmentoDeDatos.ObtenerVariable(linea[2].Lexema);
                if (variableCiclo != null)
                {
                    variableCiclo.HacerReferencia();
                }
                else
                {
                    this.Errores.ResultadoCompilacion($"Variable del cliclo invalida", linea.LineaDocumento);
                    return true;
                }
                int i = 6;
                while (linea[i].TipoToken != TipoToken.FinInstruccion)
                {
                    Argumentos.Add(linea[i]);
                    i++;
                }
                OperacionesLogicas operaciones = OperacionesLogicas.Analizar(Argumentos, this.Programa, this.Errores);
                if (operaciones is null)
                {
                    this.Errores.ResultadoCompilacion($"Comparación lógica incorrecta", linea.LineaDocumento);
                    return true;
                }

                For fur = new For(Programa, linea, variableCiclo, operaciones);
                this.Programa.AgregarAccion(fur);
                return true;

            }
            return false;
        }
        private bool DeclaraVariable(LineaLexica linea)
        {
            if (linea[0].Lexema == "Vars")//verificar que solo haya una var y no dos 
            {
                if (this.Programa.SegmentoDeDatos != null)//representacion de segmento de datos que se declara en el ensamblador 
                {
                    this.Errores.ResultadoCompilacion($"El inicio del segmento de datos ya habia sido declarado anteriormente.",
                        linea.LineaDocumento);
                    return true;
                }
                this.Programa.SegmentoDeDatos = new SegmentoDeDatos();
                return true;
            }
            if (linea[0].TipoToken == TipoToken.PalabraReservada &&//revisa si es una cadena reservada
                linea[0].Lexema == "Entero" ||
                linea[0].Lexema == "Cadena" ||
                linea[0].Lexema == "Decimal"
                )
            {
                TipoDato TipoDato = TipoDato.Invalido;
                switch (linea[0].Lexema) //ve que tipo de dato es 
                {
                    case "Entero":
                        TipoDato = TipoDato.Entero;
                        break;
                    case "Cadena":
                        TipoDato = TipoDato.Cadena;
                        break;
                    case "Decimal":
                        TipoDato = TipoDato.Decimal;
                        break;
                }
                if (linea.Elementos == 1)//verifica si no hay identificador
                {
                    this.Errores.ResultadoCompilacion($"Se esperaba un indentificador para la variable '{TipoDato}'.", linea.LineaDocumento);
                    return true;
                }
                if (linea.Elementos < 3)// si tiene menos de tres elementos tipo identificador y punto y coma 
                {
                    return false;
                }
                if (this.Programa?.SegmentoDeDatos is null)
                {
                    this.Errores.ResultadoCompilacion($"No se aperturo el segmento de datos.", linea.LineaDocumento);
                    return true;
                }
                Variable variableNueva = new Variable(Programa, linea, TipoDato);//crea una nueva variable 
                if (this.Programa.SegmentoDeDatos.YaExisteVariable(variableNueva))
                {
                    this.Errores.ResultadoCompilacion($"La variable '{variableNueva.Nombre}' ya fué declarada en este contexto.", linea.LineaDocumento);
                }
                else
                {
                    this.Programa.SegmentoDeDatos.Nueva(variableNueva);//agrega la primera variable 
                    if (linea.Elementos == 5) //verifica los 5 elementos en caso que tenga valor 
                    {
                        LineaLexica l = new LineaLexica(linea.LineaDocumento);
                        for (int i = 1; i < linea.Elementos; i++)
                        {
                            l.Agregar(linea[i]);
                        }
                        if (variableNueva.TipoDato == TipoDato.Cadena)
                        {
                            if (!AsignacionCadena(l))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!UsoVariableNumerica(l))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            return false;
        }
        private bool Si(LineaLexica linea)
        {
            if (linea[0].Lexema == "Si")
            {
                List<Token> Argumentos = new List<Token>();


                if (linea[1].TipoToken != TipoToken.ParentesisAbierto)
                {
                    this.Errores.ResultadoCompilacion($"Se esperaba un parentesis abierto", linea.LineaDocumento);
                    return true;
                }
                int i = 2;
                while (linea[i].TipoToken != TipoToken.ParentesCerrado)
                {
                    Argumentos.Add(linea[i]);
                    i++;
                    if (linea.Elementos > i)
                    {
                        if (linea[1].TipoToken != TipoToken.ParentesisAbierto)
                        {
                            this.Errores.ResultadoCompilacion($"Se esperaba un parentesis cerrado", linea.LineaDocumento);
                            return true;
                        }
                    }
                }
                OperacionesLogicas operaciones = OperacionesLogicas.Analizar(Argumentos, this.Programa, this.Errores);
                if (operaciones is null)
                {
                    this.Errores.ResultadoCompilacion($"Comparación lógica incorrecta", linea.LineaDocumento);
                    return true;
                }
                Si si = new Si(Programa, linea, operaciones);
                //if (this.Programa.Acciones.Last() is Sino sino)
                //{
                //    sino.Si = si;
                //}
                this.Programa.AgregarAccion(si);
                return true;
            }

            return false;
        }
        private bool Sino(LineaLexica linea)
        {
            if (linea[0].Lexema == "Sino")
            {
                if (this.Programa.Acciones.OfType<Si>().LastOrDefault() is { } si)
                {
                    if (si.Sino != null)
                    {
                        this.Errores.ResultadoCompilacion("Sino mal colocado", linea.LineaDocumento);
                        return true;
                    }
                    si.Sino = new Sino(si, Programa, linea);
                    this.Programa.Acciones.Add(si.Sino);
                    return true;
                }
            }

            return false;
        }
        private bool UsoVariableNumerica(LineaLexica linea)//detectar cualquier uso de una variable numerica 
        {
            if (linea[0].TipoToken == TipoToken.Identificador)// identificador verifica que lo tnega 
            {
                Variable var = this.Programa.SegmentoDeDatos.ObtenerVariable(linea[0].Lexema);//pide informacion de la variable 
                if (var != null)//ve si exixte la vribable 
                {
                    if (linea[1].TipoToken == TipoToken.OperadorAritmetico || linea[1].TipoToken == TipoToken.Relacional)//revisa el operador si es aritmetico
                    {
                        this.Programa.AgregarAccion(new OperacionArtimetica(Programa, var, linea));
                        return true;//genera codigo de tres direcciones como la practica 5
                    }
                    else if (linea[1].TipoToken == TipoToken.OperadorLogico)// si no es una operacion logica 
                    {
                        return false;
                    }
                }
                else
                {
                    this.Errores.ResultadoCompilacion($"Variable '{linea[0].Lexema}' no declarada", linea.LineaDocumento);
                    return true;
                }
            }

            return false;
        }
        private bool AsignacionCadena(LineaLexica linea)
        {
            if (linea[0].TipoToken == TipoToken.Identificador)
            {
                Variable var = this.Programa?.SegmentoDeDatos?.ObtenerVariable(linea[0].Lexema);
                if (var != null)
                {
                    if (linea[1].TipoToken == TipoToken.Relacional)
                    {
                        if (linea[2].TipoToken == TipoToken.Cadena)
                        {
                            this.Programa.AgregarAccion(new AsignaCadena(this.Programa, linea, var, linea[2]));
                            return true;
                        }
                    }
                }
                else
                {
                    this.Errores.ResultadoCompilacion($"Variable '{linea[0].Lexema}' no declarada", linea.LineaDocumento);
                    return true;
                }
            }

            return false;
        }
        private bool Inicia8086(LineaLexica linea)
        {
            if (linea.Tokens[0].Lexema == "_8086")
            {
                this.Programa.AgregarAccion(new _8086(Programa));
                this.Errores.ResultadoCompilacion("El uso de ensamblador directo se considera inseguro", linea.LineaDocumento, true);
                return true;
            }
            return false;
        }
        private bool Ensamblador(LineaLexica linea)
        {
            if (linea.Tokens.All(x => x.TipoDato == TipoDato.Ensamblador))
            {
                return true;
            }
            return false;
        }
    }
}
