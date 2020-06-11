using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace My8086.Clases.Compilador
{
    public class BorlandC
    {
        private FileInfo RutaEjecutable;
        private FileInfo RutaTemporal;
        private readonly FileInfo RutaBorlandC;
        private readonly DirectoryInfo RutaTemporales;
        public string ResultadosCompilacion { get; private set; }
        public BorlandC()
        {
            this.RutaBorlandC = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), @"TurboAsm\BIN\gui64.exe"));
            this.RutaTemporales = new DirectoryInfo($"{RutaBorlandC.Directory.FullName}\\Temporales");
        }

        public bool GeneraEjecutable(string documento)
        {
            Random rand = new Random();
            //string NombreArchivo = "try"; //Guid.NewGuid().ToString().Replace("-", "_");
            string NombreArchivo = string.Join("", Enumerable.Repeat(0, 7).Select(n => (char)rand.Next(97, 122))).ToUpper();
            this.RutaTemporal = new FileInfo(RutaBorlandC.Directory.FullName + "\\" + NombreArchivo + ".asm");
            this.RutaEjecutable = new FileInfo(this.RutaTemporal.FullName.Replace("asm", "exe"));


            if (this.RutaTemporal.Exists)
            {
                this.RutaTemporal.Delete();
            }

            if (this.RutaEjecutable.Exists)
            {
                this.RutaEjecutable.Delete();
            }



            if (!RutaBorlandC.Exists)
            {
                MessageBox.Show("No se encontro el compilador de Turbo C", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            File.WriteAllText(RutaTemporal.FullName, documento);

            File.WriteAllText($@"{RutaBorlandC.Directory.FullName}\\Run.bat",
     $@"mount C: ""C:\Program Files (x86)\GUI Turbo Assembler\BIN""
            C:
            tasm.exe {NombreArchivo}.asm >gtasm.log
            tlink.exe /3 /x /v {NombreArchivo} >gtlink.log
            exit");
            string argumentos = $"\"{this.RutaBorlandC.Directory.FullName}\\Run.bat\"";
            Process borlandc = new Process()
            {
                StartInfo = new ProcessStartInfo(RutaBorlandC.FullName, argumentos)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }

            };
            borlandc.Start();
            while (!borlandc.StandardOutput.EndOfStream)
            {
                this.ResultadosCompilacion += borlandc.StandardOutput.ReadLine() + "\n";
            }
            borlandc.WaitForExit();
            FileInfo log = new FileInfo($"{this.RutaBorlandC.Directory.FullName}\\gtlink.log");
            if (log.Exists)
            {
                this.ResultadosCompilacion += File.ReadAllText(log.FullName);
                log.Delete();
            }
            log = new FileInfo($"{this.RutaBorlandC.Directory.FullName}\\gtasm.log");
            if (log.Exists)
            {
                this.ResultadosCompilacion += File.ReadAllText(log.FullName);
                log.Delete();
            }
            //////
            this.RutaEjecutable.Refresh();

            this.RutaTemporal.MoveTo($"{this.RutaTemporales.FullName}\\{RutaTemporal.Name}");
            if (this.RutaEjecutable.Exists)
            {
                this.RutaEjecutable.MoveTo(this.RutaTemporal.DirectoryName + $"\\{RutaEjecutable.Name}");
                File.Move(this.RutaBorlandC.DirectoryName + $"\\{RutaEjecutable.Name.Replace(".exe", ".OBJ")}",
                    this.RutaTemporal.DirectoryName + $"\\{RutaEjecutable.Name.Replace(".exe", ".OBJ")}");
            }
            ////// 

            this.ResultadosCompilacion += (
                (this.RutaEjecutable.Exists ? "\nCompilación exitosa :D" : "\nLa compilación falló :C"));


            return this.RutaEjecutable.Exists;
        }
        public void Ejecutar()
        {
            try
            {
                Thread th = new Thread(() =>
                  {
                      this.ResultadosCompilacion = $"{DateTime.Now.ToLongDateString()}-{DateTime.Now.ToLongTimeString()}\n";
                      this.ResultadosCompilacion += "===========Ejecutando modulo===========\n";
                      File.WriteAllText($@"{this.RutaEjecutable.Directory.FullName}\\Run.bat",
                          $@"mount C: ""C:\Program Files (x86)\GUI Turbo Assembler\BIN""
            C:
            cls
            {this.RutaEjecutable.Name}
            cls
            exit");
                      string argumentos = $"\"{RutaEjecutable.Directory.FullName}\\Run.bat\"";
                      Process borlandc = new Process()
                      {
                          StartInfo = new ProcessStartInfo($"{this.RutaBorlandC.Directory.FullName}\\gui64.exe", argumentos)
                          {
                              UseShellExecute = false,
                              RedirectStandardOutput = true,
                              CreateNoWindow = true
                          }

                      };
                      borlandc.Start();
                      while (!borlandc.StandardOutput.EndOfStream)
                      {
                          this.ResultadosCompilacion += borlandc.StandardOutput.ReadLine() + "\n";
                      }
                      borlandc.WaitForExit();
                  });
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
                //th.Join();
            }
            catch (Exception e)
            {
                Log.LogMe(e);
                throw e;
            }
            this.ResultadosCompilacion += "===========Ejecución finalizada===========\n";
        }

        public void Limpiar()
        {
            foreach (FileInfo temporal in this.RutaTemporales.EnumerateFiles())
            {
                string extension = temporal.Extension.ToLower();
                if (extension == ".asm" || extension == ".exe" || extension == ".obj")
                {
                    try
                    {
                        temporal.Delete();
                    }
                    catch (Exception ex)
                    {
                        Log.LogMe(ex, "Al eliminar un temporal");
                    }
                }
            }
        }
    }
}
