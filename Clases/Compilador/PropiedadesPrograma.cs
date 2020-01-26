using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My8086.Clases.Compilador
{
    public class PropiedadesPrograma
    {

        public string Titulo { get; set; }
        public List<Variable> Variables { get; set; }
        public List<Accion> Acciones { get; set; }
        public bool Cerrado { get; set; }

        public PropiedadesPrograma()
        {
            this.Titulo = null;
            this.Variables = new List<Variable>();
            this.Acciones = new List<Accion>();
        }
    }
}
