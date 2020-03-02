using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Fases;
using My8086.Clases.Funciones;

namespace My8086.Clases.Compilador
{
    public class CodigoIntermedio
    {
        internal readonly Programa Programa;
        internal readonly StringBuilder Codigo;
        private List<string> Librerias;
        private Stack<IBloque> BloquesPorCerrar;
        internal CodigoIntermedio(Fases._4._Sintetizador.Sintesis sintetizado)
        {
            this.Codigo = new StringBuilder();
            this.Programa = sintetizado.Programa;
            this.BloquesPorCerrar = new Stack<IBloque>();
            this.Librerias = new List<string>
            {
                "System","System.Collections.Generic",
                "System.Linq","System.Text",
                "System.Threading.Tasks"
            };
        }
        public void Generar()
        {
            this.Codigo.Append($"namespace {this.Programa.FuncionPrincipal.Titulo} {{"); //namespace
            this.Codigo.Append($"class {this.Programa.FuncionPrincipal.Titulo} {{"); //clase principal
            GenerarFuncionPrincipal(this.Programa.FuncionPrincipal);
            foreach (Funcion Fx in
                this.Programa.Funciones.Where(x => x != this.Programa.FuncionPrincipal))
            {
                GenerarFuncion(Fx);
            }

            while (this.BloquesPorCerrar.Any())
            {
                this.Codigo.Append(this.BloquesPorCerrar.Pop().CerrarBloque());
            }
            this.Codigo.Append("}}"); //namespace,clase principal
            AgregarLibrerias();
        }
        private void GenerarFuncionPrincipal(Funcion Fx)
        {
            this.Codigo.AppendLine("static void Main(string[] args){");
            this.Codigo.AppendLine("try {");
            foreach (Accion accion in Fx.Acciones)
            {
                AgregarAccion(accion,Fx);
            }
            this.Codigo.AppendLine("}catch(Exception ex){Console.Clear();Console.WriteLine(\"Excepción no controlada X.x:\"); Console.WriteLine(ex.Message);}");
            this.Codigo.AppendLine("Console.ReadKey();}");
        }
        private void GenerarFuncion(Funcion Fx)
        {
            this.Codigo.AppendLine($"static void {Fx.Titulo}(){{");
            foreach (Accion accion in Fx.Acciones)
            {
                AgregarAccion(accion, Fx);
            }
            this.Codigo.AppendLine("}");
        }

        private void AgregarAccion(Accion accion, Funcion Fx)
        {
            if (this.BloquesPorCerrar.Any())
            {
                if (accion.LineaDocumento.LineNumber>=this.BloquesPorCerrar.Peek().FinBloque.LineNumber)
                {
                    this.Codigo.Append(this.BloquesPorCerrar.Pop().CerrarBloque());
                }
            }
            this.Codigo.Append(accion.Traduccion(Fx));
            if (accion is IBloque bloque)
            {
                BloquesPorCerrar.Push(bloque);
            }
        }

        private void AgregarLibrerias()
        {
            StringBuilder libs = new StringBuilder();
            foreach (string lib in this.Librerias)
            {
                libs.Append("using ");
                libs.Append(lib);
                libs.AppendLine(";");
            }
            this.Codigo.Insert(0, libs);
        }
    }
}
