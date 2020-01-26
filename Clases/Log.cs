using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace My8086.Clases
{
    public static class Log
    {
        public static void LogMe(string error)
        {
            try
            {
                string mensaje = string.Concat(Environment.NewLine,
                    DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString() + "---->", error);
                if (Application.Current.Properties["DEBUG"] is bool b && b)
                {
                    Console.Write(mensaje);
                    return;
                }
                File.AppendAllText($"{AppData.Directorio}\\AppData.log", mensaje);
            }
            catch (Exception)
            {
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    MessageBox.Show(error,
                //        "Error del sistema", MessageBoxButton.OK, MessageBoxImage.Error);
                //});
            }
        }
        public static void LogMe(Exception error)
        {
            try
            {
                string mensaje = string.Concat(Environment.NewLine,
                    DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString() + "----> ", error.Message);
                if (Application.Current.Properties["DEBUG"] is bool b && b)
                {
                    Console.Write(mensaje);
                    return;
                }
                File.AppendAllText(AppData.Directorio + "\\AppData.log", mensaje);
            }
            catch (Exception)
            {
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    CustomMessageBox.Show(error.StackTrace,
                //        "Error del sistema", MessageBoxButton.OK, MessageBoxImage.Error);
                //});
            }
        }
        public static void LogMe(Exception error, string descripcion)
        {
            try
            {
                string mensaje = string.Concat(Environment.NewLine,
                    DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString(), descripcion, "----> ", error.Message);
                if (Application.Current.Properties["DEBUG"] is bool b && b)
                {
                    Console.Write(mensaje);
                    return;
                }
                File.AppendAllText($"{AppData.Directorio}\\AppData.log", mensaje);
            }
            catch (Exception)
            {
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    CustomMessageBox.Show(error.StackTrace + " <-> " + descripcion,
                //        "Error del sistema", MessageBoxButton.OK, MessageBoxImage.Error);
                //});
            }
        }
        public static void RenovarLogs()
        {
            try
            {
                foreach (string l in new string[] { "AppData.log", "AppDataDemonio.log", "SQL_T.sql" })
                {
                    FileInfo log = new FileInfo($"{AppData.Directorio}\\{l}");
                    if (log.Exists && DateTime.Now.Subtract(log.CreationTime).Days > 15) //Cada 15 dias renueva el log
                    {
                        DirectoryInfo dir = new DirectoryInfo($"{AppData.Directorio}\\DisposedLogs");
                        if (!dir.Exists)
                        {
                            dir.Create();
                        }
                        dir.Refresh();
                        if (!dir.Exists) { return; }
                        if (File.Exists($"{AppData.Directorio}\\DisposedLogs\\{DateTime.Today.ToString("dd_MM_yyyy") + l}"))
                        {
                            File.Delete(
                                $"{AppData.Directorio}\\DisposedLogs\\{DateTime.Today.ToString("dd_MM_yyyy") + l}");
                        }
                        log.MoveTo($"{AppData.Directorio}\\DisposedLogs\\{DateTime.Today.ToString("dd_MM_yyyy") + l}");
                        if (File.Exists($"{AppData.Directorio}\\{l}"))
                            File.Delete($"{AppData.Directorio}\\{l}");
                        //Borrar logs de hace 2 meses
                        EliminarDisposedLogs();
                    }
                }
            }
            catch (Exception ex)
            {
                LogMe(ex);
            }
        }
        private static void EliminarDisposedLogs()
        {
            DirectoryInfo dir = new DirectoryInfo($"{AppData.Directorio}\\DisposedLogs");
            if (dir.Exists)
            {
                try
                {
                    dir.GetFiles().Where(o => DateTime.Now.Subtract(o.CreationTime).Days > 62).ToList().ForEach(f => f.Delete());
                }
                catch (Exception ex)
                {
                    LogMe(ex, "Eliminando logs viejos");
                }
            }
        }
    }
}
