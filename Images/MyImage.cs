using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using My8086.Clases;

namespace My8086.Images
{
    public class MyImage : System.Windows.Controls.Image
    {
        public static readonly DependencyProperty MySourceProperty =
            DependencyProperty.Register(
                "MySource", typeof(string), typeof(MyImage),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    (o, e) => ((MyImage)o).ImagenCambio((string)e.NewValue)));

        private object ImagenCambio(string value)
        {
            if (value is null)
            {
                return value;
            }
            Source = Extensiones.CreateImage($"{AppData.Directorio}{value}");
            return value;
        }
        public string MySource
        {
            get => (string)GetValue(MySourceProperty);
            set => SetValue(MySourceProperty, value);
        }

        public void CambiarFuente(string Fuente)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.MySource = Fuente;

            }));
        }
        static MyImage()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MyImage),
            new FrameworkPropertyMetadata(typeof(MyImage)));
    }

}
}
