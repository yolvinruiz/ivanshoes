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
using System.Windows.Threading;
using System.Windows.Threading;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para PageVentas.xaml
    /// </summary>
    public partial class PageVentas : Page
    {

        public string idventap;
        public string dniempleado = "54962548";
        private DispatcherTimer searchTimer;
        private const int SEARCH_DELAY = 300; // milisegundos
        public string pago;
        public string cantidadv = "1";
        public PageVentas()
        {
            InitializeComponent();
            CargarProductos();
            searchTimer = new DispatcherTimer();
            searchTimer.Interval = TimeSpan.FromMilliseconds(SEARCH_DELAY);
            searchTimer.Tick += SearchTimer_Tick;
            
        }
        private void ActualizarVistaCarrito()
        {
            try
            {
                var detalles = logDetalleVenta.Instancia.MostrarDetallesVenta(Convert.ToInt32(idventap));

                // Convertir los detalles al formato que espera tu ItemsControl
                var detallesVista = detalles.Select(d => new
                {
                    ID_Detalle_venta = d.ID_Detalle_venta,
                    id_Venta = d.id_Venta,
                    id_Producto = d.id_Producto,
                    nombre = d.NombreProducto,
                    NombreTalla = d.NombreTalla,
                    Imagen = d.Imagen,
                    precio = d.Preciounitario,
                    Cantidad = d.Cantidad,
                    Subtotal = d.Subtotal
                }).ToList();

                itemsControlCarrito.ItemsSource = detallesVista;

                // Actualizar el total
                double total = detalles.Sum(d => d.Subtotal);
                txtTotal.Text = total.ToString("N2");
                pago = txtTotal.Text;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al actualizar el carrito: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CargarProductos()
        {
            try
            {
                List<entProducto> productos = logProducto.Instancia.BuscarProductoConNombres("");
                itemsControlProductos.ItemsSource = productos;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar productos: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void habilitarControlesProductos()
        {
            // Habilitar controles para agregar productos
            // Por ejemplo:
            itemsControlProductos.IsEnabled = true;
            txtBuscarProducto.IsEnabled = true;
            // ... otros controles que necesites habilitar
        }

        private void RealizarCompra_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AgregarAlCarrito_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(idventap))
                {
                    System.Windows.MessageBox.Show("Primero debe generar una orden de venta",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Especificar System.Windows.Controls.Button
                System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
                if (button == null) return;

                // Obtener el DataContext del contenedor del botón
                FrameworkElement parent = button.Parent as FrameworkElement;
                if (parent == null) return;

                entProducto producto = parent.DataContext as entProducto;
                if (producto == null)
                {
                    System.Windows.MessageBox.Show("No se pudo obtener la información del producto",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrEmpty(cantidadv))
                {
                    System.Windows.MessageBox.Show("Ingrese una cantidad",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int cantidad = Convert.ToInt32(cantidadv);
                double precioUnitario = producto.precio;
                double subtotal = precioUnitario * cantidad;

                entDetalleVenta nuevoDetalle = new entDetalleVenta
                {
                    id_Venta = Convert.ToInt32(idventap),
                    id_Producto = producto.id_producto,
                    Cantidad = cantidad,
                    Preciounitario = precioUnitario,
                    Subtotal = subtotal
                };

                logDetalleVenta.Instancia.InsertarDetalleVenta(nuevoDetalle);

                ActualizarVistaCarrito();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al agregar producto: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidarNumeros(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private async void VerificarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDniCliente.Text) || txtDniCliente.Text.Length != 8)
            {
                System.Windows.MessageBox.Show("Por favor ingrese un DNI válido de 8 dígitos",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int dni = Convert.ToInt32(txtDniCliente.Text);

                // Verificar si el empleado existe
                if (string.IsNullOrEmpty(dniempleado) ||
                    !logEmpleado.Instancia.VerificarEmpleadoPorDNI(Convert.ToInt32(dniempleado)))
                {
                    System.Windows.MessageBox.Show("Por favor ingrese un DNI de empleado válido",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Verificar si el cliente existe
                if (!logCliente.Instancia.VerificarClientePorDNI(dni))
                {
                    // Cliente no existe, obtener datos de la API
                    api apiCliente = new api();
                    api cli = await apiCliente.ObtenerDatosClienteAsync(dni.ToString());

                    entCliente cliente = new entCliente
                    {
                        Nombre = cli.Nombre,
                        Apellido = cli.Apellidos,
                        DNI = dni,
                        Telefono = 0,
                        Estado = true
                    };

                    // Insertar nuevo cliente
                    logCliente.Instancia.InsertarCliente(cliente);
                    System.Windows.MessageBox.Show("Cliente agregado correctamente.",
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Obtener datos del cliente (sea nuevo o existente)
                var clienteInfo = logCliente.Instancia.BuscarClientePorDNI(dni);
                if (clienteInfo != null)
                {
                    // Mostrar nombre del cliente
                    txtNombreCliente.Text = $"{clienteInfo.Nombre} {clienteInfo.Apellido}";
                    txtNombreCliente.IsEnabled = false;

                    // Generar orden de venta
                    int idEmpleado = logEmpleado.Instancia.BuscarIdempleadoPorDNI(
                        Convert.ToInt32(dniempleado));

                    entVenta venta = new entVenta
                    {
                        fecha = DateTime.Now,
                        id_cliente = clienteInfo.id_Cliente,
                        id_empleado = idEmpleado,
                        estado = false
                    };

                    int idVenta = logVenta.Instancia.InsertarVenta(venta);
                    idventap = idVenta.ToString(); // Asegúrate de tener esta variable definida en la clase

                    System.Windows.MessageBox.Show("Orden de venta generada correctamente.",
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Habilitar controles para agregar productos
                    // Aquí puedes habilitar los controles necesarios para agregar productos
                    habilitarControlesProductos(); // Deberás implementar este método
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al obtener datos del cliente.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisminuirCantidad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AumentarCantidad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EliminarProducto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtBuscarProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Reiniciar el timer
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            try
            {
                string termino = txtBuscarProducto.Text.Trim();
                List<entProducto> productos = logProducto.Instancia.BuscarProductoConNombres(termino);
                itemsControlProductos.ItemsSource = productos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en búsqueda: {ex.Message}");
            }
        }

    }
}
