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
using System.IO;
namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Clientes : Window
    {
        public string idcliente;
        public Clientes()
        {
            InitializeComponent();
            string fondoGuardado = Properties.Settings.Default.FondoPantalla;
            if (!string.IsNullOrEmpty(fondoGuardado) && File.Exists(fondoGuardado))
            {
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(fondoGuardado)));
                    brush.Stretch = Stretch.UniformToFill;
                    this.Background = brush;
                }
                catch { }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string dni = txtdnicliente.Text.Trim();
            api apiCliente = new api();

            try
            {
                api cliente = await apiCliente.ObtenerDatosClienteAsync(dni);
                txtnombrecliente.Text = cliente.Nombre;
                txtapellidoscliente.Text = $"{cliente.Apellidos}";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                int dni = Convert.ToInt32(txtdnicliente.Text.Trim());
                if (!logCliente.Instancia.VerificarClientePorDNI(dni))
                {
                    entCliente cliente = new entCliente();
                    cliente.Nombre = txtnombrecliente.Text.Trim();
                    cliente.Apellido = txtapellidoscliente.Text.Trim();
                    cliente.DNI = dni;
                    cliente.Telefono = Convert.ToInt32(txttelefonocliente.Text.Trim());
                    cliente.Estado = cbtestadocliente.IsChecked == true;

                    logCliente.Instancia.InsertarCliente(cliente);
                    System.Windows.MessageBox.Show("Cliente agregado correctamente.");
                }
                else
                {
                    System.Windows.MessageBox.Show("El cliente con el DNI especificado ya existe.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int dni = Convert.ToInt32(txtdnibuscar.Text.Trim());
                entCliente cliente = logCliente.Instancia.BuscarClientePorDNI(dni);

                if (cliente != null)
                {
                    idcliente = cliente.id_Cliente.ToString();
                    txtdnicliente.Text = cliente.DNI.ToString();
                    txtnombrecliente.Text = cliente.Nombre;
                    txtapellidoscliente.Text = cliente.Apellido;
                    txttelefonocliente.Text = cliente.Telefono.ToString();
                    cbtestadocliente.IsChecked = cliente.Estado;
                }
                else
                {
                    System.Windows.MessageBox.Show("Cliente no encontrado.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                entCliente c = new entCliente();
                c.id_Cliente = int.Parse(idcliente);
                c.Nombre = txtnombrecliente.Text.Trim();
                c.Apellido = txtapellidoscliente.Text.Trim();
                c.DNI = Convert.ToInt32(txtdnicliente.Text);
                c.Telefono = Convert.ToInt32(txttelefonocliente.Text);          
                c.Estado = Convert.ToBoolean(cbtestadocliente.IsChecked == true);
                logCliente.Instancia.EditarCliente(c);
                System.Windows.MessageBox.Show("Modificacion exitosa");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error.." + ex);
            }
        }
        private bool EsNumero(string texto)
        {
            return int.TryParse(texto, out _);
        }
        private void txtdnicliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsNumero(e.Text);
        }
        private bool EsLetra(string texto)
        {
            return texto.All(char.IsLetter);  // Devuelve true si todos los caracteres son letras
        }
        private void txtnombrecliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsLetra(e.Text);
        }

        private void txtapellidoscliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsLetra(e.Text);
        }

        private void txttelefonocliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsNumero(e.Text);
        }

        private void txtdnibuscar_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsNumero(e.Text);
        }
    }
}
