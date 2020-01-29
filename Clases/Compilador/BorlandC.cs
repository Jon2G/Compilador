using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace My8086.Clases.Compilador
{
    public class BorlandC
    {
        private FileInfo RutaEjecutable;
        public string ResultadosCompilacion;
        public BorlandC()
        {

        }
        public bool GeneraEjecutable(string documento)
        {
            string NombreArchivo = "MyCode";
            FileInfo temporal = new FileInfo($@"{AppData.Directorio}\TurboAsm\BIN\{NombreArchivo}.asm");
            if (!temporal.Directory.Exists)
            {
                MessageBox.Show("No se encontro el compilador de Turbo C", "Atención",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            File.WriteAllText(temporal.FullName, documento);
            //
            File.WriteAllText($@"{temporal.Directory.FullName}\\Run.bat",
     $@"mount C: ""C:\Program Files (x86)\GUI Turbo Assembler\BIN""
            C:
            tasm.exe {NombreArchivo}.asm >gtasm.log
            tlink.exe /3 /x /v {NombreArchivo} >gtlink.log
            exit");
            Process borlandc = new Process()
            {
                StartInfo = new ProcessStartInfo($"{temporal.Directory.FullName}\\gui64.exe", $"{temporal.Directory.FullName}\\Run.bat")
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
            FileInfo log = new FileInfo($"{temporal.Directory.FullName}\\gtlink.log");
            if (log.Exists)
            {
                this.ResultadosCompilacion += File.ReadAllText(log.FullName);
                log.Delete();
            }
            log = new FileInfo($"{temporal.Directory.FullName}\\gtasm.log");
            if (log.Exists)
            {
                this.ResultadosCompilacion += File.ReadAllText(log.FullName);
                log.Delete();
            }

            FileInfo ejecutable = new FileInfo(temporal.FullName.Replace("asm", "exe"));
            this.RutaEjecutable = ejecutable.Exists ? ejecutable : null;
            //
            return this.RutaEjecutable != null;
        }

        internal void Ejecutar()
        {
            try
            {
                this.ResultadosCompilacion = $"{DateTime.Now.ToLongDateString()}-{DateTime.Now.ToLongTimeString()}\n";
                this.ResultadosCompilacion += "===========Ejecutando modulo===========\n";
                File.WriteAllText($@"{this.RutaEjecutable.Directory.FullName}\\Run.bat",
                    $@"mount C: ""C:\Program Files (x86)\GUI Turbo Assembler\BIN""
            C:
            cls
            {this.RutaEjecutable.Name}
            exit");
                Process borlandc = new Process()
                {
                    StartInfo = new ProcessStartInfo($"{this.RutaEjecutable.Directory.FullName}\\gui64.exe", $"{RutaEjecutable.Directory.FullName}\\Run.bat")
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
            }
            catch (Exception e)
            {
                throw e;
            }
            this.ResultadosCompilacion += "===========Ejecución finalizada===========\n";
        }
    }
}
