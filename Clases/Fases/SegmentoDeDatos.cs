using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using My8086.Clases.Funciones;

namespace My8086.Clases.Fases
{
    public class SegmentoDeDatos
    {
        public List<Variable> Variables;
        public int ConsecutivoTemporal => _ConsecutivoTemporal++;
        private int _ConsecutivoTemporal = 1;

        public SegmentoDeDatos()
        {
            this.Variables = new List<Variable>();
        }
        public bool YaExisteVariable(string VariableNueva)
        {
            return this.Variables.Any(x => x.Nombre == VariableNueva);
        }
        public bool YaExisteVariable(Variable VariableNueva)
        {
            return YaExisteVariable(VariableNueva.Nombre);
        }
        public Variable ObtenerVariable(string Variable)
        {
            return this.Variables.FirstOrDefault(x => x.Nombre == Variable);
        }

        internal Variable Nueva(Variable VariableNueva)
        {
            this.Variables.Add(VariableNueva);
            return VariableNueva;
        }

        public StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Variable variable in this.Variables)
            {
                sb.Append(variable.Traduccion());
            }
            return sb;
        }

        public StringBuilder Free()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Variable variable in this.Variables.Where(x => x.TipoDato == TipoDato.Cadena))
            {
                sb.AppendLine($"MOV BX,{variable.Nombre}");
                sb.AppendLine($"CALL LIBERAR_CADENA");
            }
            return sb;
        }
    }
}
