using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My8086.Ejemplos
{
    public class Ejemplo
    {
        private string NombreRecurso { get; set; }
        public string Nombre { get;private set; }
        public Ejemplo(string NombreRecurso)
        {
            this.NombreRecurso = NombreRecurso;
            this.Nombre = this.NombreRecurso
                .Replace("My8086.Ejemplos.", "")
                .Replace(".my86", "");
        }
        public override string ToString()
        {
            return this.Nombre;
        }
        public static ObservableCollection<Ejemplo> ListarEjemplos()
        {
            ObservableCollection<Ejemplo> Ejemplos = new ObservableCollection<Ejemplo>();
            string[] ejemplos = typeof(Ejemplo).Assembly.GetManifestResourceNames();
            ejemplos = ejemplos.Where(x => x.EndsWith(".my86")).ToArray();
            foreach (string e in ejemplos)
            {
                Ejemplos.Add(new Ejemplo(e));
            }
            return Ejemplos;
        }

        internal string GetDocumento()
        {
            string result = string.Empty;
            using (Stream stream = typeof(Ejemplo).Assembly.GetManifestResourceStream(this.NombreRecurso))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
