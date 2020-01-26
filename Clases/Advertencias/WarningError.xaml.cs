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

namespace My8086.Clases.Advertencias
{
    /// <summary>
    /// Lógica de interacción para WarningError.xaml
    /// </summary>
    public partial class WarningError : UserControl
    {
        public WarningError()
        {
            InitializeComponent();
        }
        public EventHandler VerLinea;
        public WarningError(ErrorCompilacion ce)
        {
            InitializeComponent();
            this.Img.MySource = ce.EsAdvertencia ? @"\Imgs\warning (2).png" : @"\Imgs\bug.png";
            this.Error.Text = ce.Texto;
            this.Linea.Text = ce.Linea.ToString();
            this.MouseDoubleClick += (o, i) => { this.VerLinea?.Invoke(ce.Linea, i); };
        }
    }
}
