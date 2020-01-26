using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My8086.Clases
{
    /// <summary>
    /// Lógica de interacción para ElementoAutoCompletado.xaml
    /// </summary>
    public partial class ElementoAutoCompletado : UserControl
    {
        public ElementoAutoCompletado(string Texto)
        {
            InitializeComponent();
            this.TxtName.Text = Texto;
            this.InvalidateVisual();

            this.Loaded += ElementoAutoCompletado_Loaded;
        }

        private void ElementoAutoCompletado_Loaded(object sender, RoutedEventArgs e)
        {
            MyCompletionData data = (MyCompletionData)this.DataContext;
            this.Imagen.Source = Extensiones.CreateImage($"{Environment.CurrentDirectory}\\{data.ImgSource}");
        }
    }
}
