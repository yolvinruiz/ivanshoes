using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para Pago.xaml
    /// </summary>
    public partial class Pago : Window
    {
        public Pago()
        {
            InitializeComponent();
        }

        private void btnefectivo_Click(object sender, RoutedEventArgs e)
        {
            cambio.Navigate(new VentanaPagoEfectivo());
        }
    }
}
