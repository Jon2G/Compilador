using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;

namespace My8086.Clases
{
    public static class Extensiones
    {
        public static BitmapImage CreateImage(string path)
        {
            if (File.Exists(path))
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(path);
                myBitmapImage.EndInit();
                return myBitmapImage;
            }
            Log.LogMe($"Imagen no encontrada:{path}");
            return new BitmapImage();
        }
        //private static void _Add(this IList<PendienteParaCocina> pendientes, PendienteParaCocina nuevo)
        //{
        //    for (int i = 0; i < pendientes.Count; i++)
        //    {
        //        if (pendientes[i].Id != nuevo.Id || pendientes[i].Mesa != nuevo.Mesa ||
        //            pendientes[i].OpcionesSeleccionadas.Count != nuevo.OpcionesSeleccionadas.Count) continue;
        //        if (pendientes[i].OpcionesSeleccionadas.Where((t, j) => t.Id_Platillo !=
        //                                                                nuevo.OpcionesSeleccionadas[j].Id_Platillo).Any())
        //        {
        //            pendientes.Add(nuevo); return;
        //        }
        //        //son iguales
        //        pendientes[i].Cantidad += nuevo.Cantidad;
        //        return;
        //    }
        //    pendientes.Add(nuevo);
        //}
        public static byte[] ReadAllBytes(this FileInfo file)
        {
            return File.ReadAllBytes(file.FullName);
        }
        public static string ToImageString(this byte[] bytes)
        {
            return string.Concat("data:image/png;base64,", Convert.ToBase64String(bytes, 0, bytes.Length));
        }
        public static BitmapSource ToImageSource(this System.Drawing.Bitmap bitmap)
        {
            BitmapSource bitmapSource = null;
            try
            {
                System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

                bitmapSource = BitmapSource.Create(
                    bitmapData.Width, bitmapData.Height,
                    bitmap.HorizontalResolution, bitmap.VerticalResolution,
                    PixelFormats.Bgr24, null,
                    bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

                bitmap.UnlockBits(bitmapData);
            }
            catch (Exception) { }
            return bitmapSource;
        }
        public static byte[] ImageToByte(this System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        public static void SetPercent(this System.Windows.Controls.ProgressBar progressBar, double percentage)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Send,
                new Action(() =>
                    {
                        progressBar.IsIndeterminate = false;
                        DoubleAnimation animation = new DoubleAnimation(percentage, TimeSpan.FromSeconds(2)) { };
                        progressBar.BeginAnimation(RangeBase.ValueProperty, animation);
                    }
                ));
        }
        public static string GetOsName(this OperatingSystem os_info)
        {
            RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productName = (string)reg.GetValue("ProductName");
            if (productName.StartsWith("Windows 10"))
            {
                return productName;
            }
            string version =
                os_info.Version.Major.ToString() + "." +
                os_info.Version.Minor.ToString();
            switch (version)
            {
                case "10.0": return "10/Server 2016";
                case "6.3": return "8.1/Server 2012 R2";
                case "6.2": return "8/Server 2012";
                case "6.1": return "7/Server 2008 R2";
                case "6.0": return "Server 2008/Vista";
                case "5.2": return "Server 2003 R2/Server 2003/XP 64-Bit Edition";
                case "5.1": return "XP";
                case "5.0": return "2000";
            }
            return "Unknown";
        }
        public static void Append(this StringBuilder s, params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
                s.Append(parametros[i]);
        }
        /// <summary>
        /// Mueve el foco a los controles establecidos por el tab index y expande los combos donde establece el foco
        /// </summary>
        /// <param name="window"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        public static DataTable ToTable<T>(this IEnumerable<T> lista)
        {
            return ToTable(lista.ToList());
        }
        /// <summary>
        /// Convierte cualquier tabla a una Lista de objectos siempre que tengan campos publicos en común y un constructor publico sin parametros
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<T>[] Divide<T>(this List<T> lista, int dividir)
        {
            if (dividir <= 0)
            {
                throw new ArgumentOutOfRangeException("No puede dividir una lista entre:" + dividir);
            }
            List<T>[] resultado = new List<T>[dividir];
            if (lista?.Count < 0)
            {
                return resultado;
            }

            int xlista = lista.Count / dividir;

            int rango = 0;
            for (int i = 0; i < dividir; i++)
            {
                if (rango > lista.Count)
                {

                }
                resultado[i] = lista.GetRange(rango, xlista);
                rango += xlista;
            }
            return resultado;

        }
    }
}
