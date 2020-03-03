using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Compilador;
using My8086.Clases.Fases;

namespace My8086.Clases.Funciones
{
    public class Funcion
    {
        internal string Titulo { get; private set; }
        public bool Cerrado { get; set; }
        public DocumentLine FinBloque { get; set; }
        public DocumentLine InicioBloque { get; set; }
        public List<Variable> Variables => this.Acciones.OfType<Variable>().ToList();
        public readonly List<Accion> Acciones;
        public Funcion(DocumentLine InicioBloque, string Titulo)
        {
            this.Acciones = new List<Accion>();
            this.InicioBloque = InicioBloque;
            this.Titulo = Titulo;
        }

        public void AgregarAccion(Accion accion)
        {
            this.Acciones.Add(accion);
        }

        public void Cerrar(DocumentLine lineaLineaDocumento)
        {
            this.FinBloque = lineaLineaDocumento;
            this.Cerrado = true;
        }
    }
}
