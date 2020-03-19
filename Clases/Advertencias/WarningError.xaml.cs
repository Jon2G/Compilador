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
        private EventHandler VerLinea;
        public ErrorCompilacion Excepcion { get; private set; }
        public WarningError(ErrorCompilacion ce, EventHandler VerLinea)
        {
            InitializeComponent();
            this.Excepcion = ce;
            this.Img.MySource = this.Excepcion.EsAdvertencia ? @"\Imgs\StatusWarning_16x.png" : @"\Imgs\bug.png";
            this.Error.Text = this.Excepcion.Texto;
            this.Linea.Text = (this.Excepcion?.Linea?.LineNumber ?? -1).ToString();
            this.VerLinea = VerLinea;
            this.MouseDoubleClick += (o, i) =>
            {
                this.VerLinea?.Invoke(this.Excepcion.Linea, i);
            };
        }
    }
}
