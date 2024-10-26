using CapaLogica;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para VentanaPagoEfectivo.xaml
    /// </summary>
    public partial class VentanaPagoEfectivo : Page
    {
        public string pg;
        public string idv;
        public VentanaPagoEfectivo(string pago, string idventa)
        {
            InitializeComponent();
            pg = pago;
            idv = idventa;
            txtpago.Text = pg;
            btnConfirmarpago.IsEnabled = true;
        }

        private void btnConfirmarpago_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idventa = Convert.ToInt32(idv);
                int idFormaPago = 1;
                logPago.Instancia.RegistrarPagoYActualizarVenta(idventa, idFormaPago);
                System.Windows.MessageBox.Show("PAGO REGISTRADO CON EXITO");
                btnConfirmarpago.IsEnabled = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void txtmontorecibido_TextChanged(object sender, TextChangedEventArgs e)
        {
            try { 
            double cambio = Convert.ToDouble(txtmontorecibido.Text) - Convert.ToDouble(txtpago.Text);
            txtcambio.Text = cambio.ToString();
                }
            catch (Exception ex){ System.Windows.MessageBox.Show("Ocurrió un error: " + ex.Message); }
        }
    }
}
