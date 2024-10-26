using CapaDatos;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para paginaverpedidos.xaml
    /// </summary>
    public partial class paginaverpedidos : Page
    {
        public int idP;
        public paginaverpedidos()
        {
            InitializeComponent();
            CargarPedidosDeHoy();
        }

        private void OcultarColumnas()
        {
        }
        private async void CargarPedidosDeHoy()
        {
            await Task.Delay(500);
            try
            {
                List<entPedido> pedidosHoy = logPedido.Instancia.ListarPedidosHoy();
                dgvpedidos.ItemsSource = pedidosHoy;
                foreach (var column in dgvpedidos.Columns)
                {
                    if (column.Header.ToString() == "NombreCliente" ||
                        column.Header.ToString() == "DNI" ||
                        column.Header.ToString() == "Destino")
                    {
                        column.Visibility = Visibility.Collapsed;
                    }
                }
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al cargar los pedidos de hoy: " + ex.Message);
            }
        }
        private void btnRealizarpedido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtcliente.Text = idP.ToString();
                logPedido.Instancia.ActualizarNumeroSeguimiento(idP, txtnumeroseguimiento.Text);
                System.Windows.MessageBox.Show("registro exitoso");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:" + ex.Message);
            }
        }

            

        private void dgvpedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvpedidos.SelectedItem is entPedido pedidoSeleccionado)
            {
                // Obtiene el ID del pedido seleccionado
                int idPedido = pedidoSeleccionado.Id;
                idP = pedidoSeleccionado.Id;
                // Llama a la función CargarInformacionPedido con el ID seleccionado
                CargarInformacionPedido(idPedido);

            }
        }
        private void CargarInformacionPedido(int idPedido)
        {
            try
            {
                // Llama al método de la capa lógica para obtener la información del pedido
                entPedido pedidoCliente = logPedido.Instancia.ObtenerInformacionPedidoCliente(idPedido);

                if (pedidoCliente != null)
                {
                    // Asigna los datos a los TextBlocks
                    txtcliente.Text = $"Cliente: {pedidoCliente.NombreCliente}";
                    txtDni.Text = $"DNI: {pedidoCliente.DNI}";
                    txtDestino.Text = $"Destino: {pedidoCliente.Destino}";
                    txtFechaenvio.Text = $"Fecha de Envío: {pedidoCliente.FechaEnvio:dd/MM/yyyy}";
                    

                }
                else
                {
                    System.Windows.MessageBox.Show("No se encontraron datos para el pedido especificado.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al cargar la información del pedido: " + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CargarPedidosDeHoy();
        }
    }
}
