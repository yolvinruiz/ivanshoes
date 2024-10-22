using CapaDatos;
using CapaEntidad;
using CapaLogica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Xceed.Wpf.Toolkit;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para Paginaventa.xaml
    /// </summary>
    public partial class Paginaventa : Page
    {
        public string idventap;
        public string idproductop;
        public Paginaventa()
        {
            InitializeComponent();
        }
        private async void verificarcliente(int dnia)
        {
            api apiCliente = new api();
            try
            {
                int dni = dnia;
                if (!logCliente.Instancia.VerificarClientePorDNI(dni))
                {
                    api cli = await apiCliente.ObtenerDatosClienteAsync(Convert.ToString(dni));
                    entCliente cliente = new entCliente();
                    cliente.Nombre = cli.Nombre;
                    cliente.Apellido = $"{cli.Apellidos}";
                    cliente.DNI = dni;
                    cliente.Telefono = 0;
                    cliente.Estado = true;
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

        private void btnmostrarproducto_Click(object sender, RoutedEventArgs e)
        {
            string termino = "";
            List<entProducto> productos = logProducto.Instancia.BuscarProductoConNombres(termino);
            dgvproducto.ItemsSource = productos;
            foreach (var column in dgvproducto.Columns)
            {
                if (column.Header.ToString() == "id_tipo_producto" ||
                    column.Header.ToString() == "id_marca" ||
                    column.Header.ToString() == "id_color" ||
                    column.Header.ToString() == "id_categoria" ||
                    column.Header.ToString() == "id_talla")
                {
                    column.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void numericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (numericUpDown != null && numericUpDown.Value != null)
            {
                double precio = 0;
                double.TryParse(txtprecpro.Text.Trim(), out precio);
                int cantidad = numericUpDown.Value ?? 1;
                double subtotal = cantidad * precio;
                txtsubtotal.Text = $"{subtotal:F2}";
            }
        }

        private void dgvgenerarorden_Click(object sender, RoutedEventArgs e)
        {
            int dni = Convert.ToInt32(txtdniempleado.Text.Trim());
            if (!logEmpleado.Instancia.VerificarEmpleadoPorDNI(dni))
            {
                System.Windows.MessageBox.Show("Empleado no existe");
                txtdniempleado.Text = "";
            }
            else
            {

                int idCliente = logCliente.Instancia.BuscarIdClientePorDNI(Convert.ToInt32(txtdnicliente.Text.Trim()));
                int idEmpleado = logEmpleado.Instancia.BuscarIdempleadoPorDNI(Convert.ToInt32(txtdniempleado.Text.Trim()));
                if (idCliente > 0)
                {
                    entVenta venta = new entVenta
                    {
                        fecha = DateTime.Now,
                        id_cliente = idCliente,
                        id_empleado = idEmpleado,
                        estado = false
                    };

                    int idVenta = logVenta.Instancia.InsertarVenta(venta);
                    idventap = idVenta.ToString();
                    System.Windows.MessageBox.Show("Venta registrada correctamente.");
                }
                else
                {
                    System.Windows.MessageBox.Show("No se encontró el cliente para registrar la venta.");
                }
                txtdnicliente.Text = "";
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                verificarcliente(Convert.ToInt32(txtdnicliente.Text));
            }
            catch (Exception ex){
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvproducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Asegurarse de que hay un elemento seleccionado
            if (dgvproducto.SelectedItem != null)
            {
                // Obtener la fila seleccionada como un objeto dinámico
                dynamic row = dgvproducto.SelectedItem;

                // Asignar los valores a los TextBox
                txtnompro.Text = row.NombreTipoProducto.ToString();
                txtprecpro.Text = Convert.ToDouble(row.precio).ToString("F2");
                idproductop = Convert.ToString(row.id_producto).ToString();
            }
        }

        private void txtdnicliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtdniempleado_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text); 
        }

        private void btnbuscarproducto_Click(object sender, RoutedEventArgs e)
        {
            string termino = txtbuscar.Text.Trim();
            List<entProducto> productos = logProducto.Instancia.BuscarProductoConNombres(termino);
            dgvproducto.ItemsSource = productos;
            foreach (var column in dgvproducto.Columns)
            {
                if (column.Header.ToString() == "id_tipo_producto" ||
                    column.Header.ToString() == "id_marca" ||
                    column.Header.ToString() == "id_color" ||
                    column.Header.ToString() == "id_categoria" ||
                    column.Header.ToString() == "id_talla")
                {
                    column.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void btnagregarproducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<entDetalleVenta> detallesVenta = logDetalleVenta.Instancia.InsertarDetalleVenta(new entDetalleVenta
                {
                    id_Venta = Convert.ToInt32(idventap),
                    id_Producto = Convert.ToInt32(idproductop),
                    Cantidad = Convert.ToInt32(numericUpDown.Value),
                    Preciounitario = Convert.ToDouble(txtprecpro.Text),
                    Subtotal = Convert.ToDouble(txtsubtotal.Text)
                });
                dgvdetalleventa.ItemsSource = detallesVenta;
                foreach (var column in dgvdetalleventa.Columns)
                {
                    if (column.Header.ToString() == "ID_Detalle_venta" ||
                        column.Header.ToString() == "id_Venta" ||
                        column.Header.ToString() == "id_Producto")
                    {
                        column.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ocurrió un error al cargar los detalles de la venta: " + ex.Message);
            }
        }
    }
}
