using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;

namespace My8086.Clases.Compilador
{
    public class NetCompiler
    {
        private FileInfo RutaEjecutable;
        public string ResultadosCompilacion;
        private readonly CodigoIntermedio Intermedio;
        private readonly CSharpCodeProvider Provider;
        private CompilerResults CompilerResults;
        private readonly string Ruta;
        public NetCompiler(CodigoIntermedio Intermedio)
        {
            this.Intermedio = Intermedio;
            this.Provider = new CSharpCodeProvider();
            this.Ruta = Path.Combine(Path.GetTempPath(), "CompilacionesTemporales_", Guid.NewGuid().ToString());
            Directory.CreateDirectory(Ruta);
        }
        ~NetCompiler()
        {
            try
            {
                DirectoryInfo rutaPrincipal = new DirectoryInfo(this.Ruta).Parent;
                rutaPrincipal.Delete(true);
            }
            catch (Exception ex)
            {
                Log.LogMe(ex, "Al eliminar temporales de compilacion");
            }
        }
        public bool GeneraEjecutable()
        {
            string NombreArchivo = Path.Combine(this.Ruta, Intermedio.Programa.Titulo + ".exe");
            this.RutaEjecutable = new FileInfo(NombreArchivo);
            CompilerParameters CompilerParams = new CompilerParameters
            {
                GenerateInMemory = false,
                TreatWarningsAsErrors = false,
                GenerateExecutable = true,
                CompilerOptions = "/optimize",
                OutputAssembly = NombreArchivo
            };
            string[] references =
            {
                "System.dll",
                "mscorlib.dll",
                "System.Linq.dll",
                "System.Threading.Tasks.dll",
                "System.IO.dll",
                "System.Data.dll",
                "System.XML.dll"
            };
            CompilerParams.ReferencedAssemblies.AddRange(references);
            this.CompilerResults = this.Provider.CompileAssemblyFromSource(CompilerParams,
                this.Intermedio.Codigo.ToString());
            if (CompilerResults.Errors.HasErrors)
            {
                this.RutaEjecutable.Refresh();
                return false;
            }
            this.RutaEjecutable.Refresh();
            return true;
        }

        internal void Ejecutar()
        {
            try
            {
                this.RutaEjecutable.Refresh();
                if (!this.RutaEjecutable.Exists)
                {
                    return;
                }
                this.ResultadosCompilacion = $"{DateTime.Now.ToLongDateString()}-{DateTime.Now.ToLongTimeString()}\n";
                this.ResultadosCompilacion += "===========Ejecutando modulo===========\n";
                Thread th = new Thread(() =>
                {
                    try
                    {
                        Process pr = Process.Start(this.RutaEjecutable.FullName);
                        pr.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                        }
                    }
                });
                th.Start();
            }
            catch (Exception e)
            {
                throw e;
            }
            this.ResultadosCompilacion += "===========Ejecución finalizada===========\n";
        }

        internal string GenerarIdalsm()
        {
            if (RutaEjecutable.Exists)
            {
                FileInfo ildasm = new FileInfo(Path.Combine(AppData.Directorio, "NET_48", "ildasm.exe"));
                if (!ildasm.Exists)
                {
                    return "No se econtro el archivo ildasm";
                }

                FileInfo salida = new FileInfo(Path.Combine(this.Ruta, this.RutaEjecutable.Name + ".il"));
                ProcessStartInfo info = new ProcessStartInfo(ildasm.FullName)
                {
                    Arguments =
                        $"\"{this.RutaEjecutable.FullName}\" /output:\"{salida.FullName}\" /all"
                };
                Process pr = Process.Start(info);
                pr.WaitForExit();
                salida.Refresh();
                if (!salida.Exists)
                {
                    return "No se genero el archivo .il";
                }

                return File.ReadAllText(salida.FullName);
            }
            return "No se genero el archivo .il";
        }
    }
}
