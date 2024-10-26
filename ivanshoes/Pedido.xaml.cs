using CapaEntidad;
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
using System.Windows.Shapes;

namespace ivanshoes
{

    public partial class Pedido : Window
    {

        private int IDventa;
        public Pedido(int idVenta)
        {
            InitializeComponent();
            IDventa = idVenta;
            dtfechaenvio.SelectedDate = DateTime.Now.AddDays(3);
            dtfechamaxentrega.SelectedDate = DateTime.Now.AddDays(7);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                entPedido nuevoPedido = new entPedido
                {
                    Fecha = DateTime.Now,
                    FechaEnvio = dtfechaenvio.SelectedDate.Value,
                    FechaEntrega = dtfechamaxentrega.SelectedDate.Value,
                    Estado = false,
                    Direccion = txtDireccion.Text,
                    Ciudad = txtCiudad.Text,
                    NumeroSeguimiento = "0",
                    ID_venta = IDventa
                };
                logPedido.Instancia.InsertarPedido(nuevoPedido);
                System.Windows.MessageBox.Show("PEDIDO PROGRAMADO CON EXITO");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
