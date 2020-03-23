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
        public event EventHandler VerLinea;

        public static readonly DependencyProperty ExcepcionProperty =
            DependencyProperty.Register(
                "Excepcion", typeof(ErrorCompilacion), typeof(WarningError),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    (o, e) => ((WarningError)o).AuxCambio(e.NewValue as ErrorCompilacion)));
        public ErrorCompilacion Excepcion
    {
            get => GetValue(ExcepcionProperty) as ErrorCompilacion;
            set => SetValue(ExcepcionProperty, value);
        }
        private ErrorCompilacion AuxCambio(ErrorCompilacion value)
        {
            ErrorCompilacion old = this.Excepcion;
            this.Excepcion = value;
            OnPropertyChanged(new DependencyPropertyChangedEventArgs(ExcepcionProperty,
                old, this.Excepcion));
            Cargar();
            return value;
        }
        private void Cargar()
        {
            if (this.Excepcion.Linea?.IsDeleted??false)
            {
                return;
            }
            this.Img.MySource = this.Excepcion.EsAdvertencia ? @"\Imgs\StatusWarning_16x.png" : @"\Imgs\bug.png";
            this.Error.Text = this.Excepcion.Texto;
            this.Linea.Text = (this.Excepcion?.Linea?.LineNumber ?? -1).ToString();
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.VerLinea?.Invoke(this.Excepcion.Linea, e);
        }
    }
}
