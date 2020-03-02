using System.Collections.Generic;
using System.Linq;
using My8086.Clases.Funciones;

namespace My8086.Clases.Fases
{
    internal class Programa
    {
        public string Titulo => this.FuncionPrincipal.Titulo;

        public Funcion FuncionPrincipal
        {
            get => this.Funciones.FirstOrDefault();
            set => this.Funciones.Insert(0, value);
        }
        public List<Funcion> Funciones { get; set; }
        public Programa()
        {
            this.Funciones = new List<Funcion>();
        }
    }
}
