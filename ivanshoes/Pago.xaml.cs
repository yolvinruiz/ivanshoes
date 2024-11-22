using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        public string pg, dniclie, nombreclie;
        public string idv;

        public Pago(string idventa,string dnicliente, string nombrecliente, string montototal)
        {
            InitializeComponent();
            pg = montototal;
            idv= idventa ;
            dniclie = dnicliente;
            nombreclie = nombrecliente;
            cambio.Navigate(new PagePagoEfectivo(idv, dniclie, nombreclie, pg));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cambio.Navigate(new PagePagoEfectivo(idv, dniclie, nombreclie, pg));
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            cambio.Navigate(new pagotarjeta(idv, dniclie, nombreclie, pg));
        }
    }
}
