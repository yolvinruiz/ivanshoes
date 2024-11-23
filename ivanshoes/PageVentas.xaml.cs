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
using System.Windows.Media;
using System.Net.Http;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para PageVentas.xaml
    /// </summary>
    public class ServicioActualizacion
    {
        private static readonly ServicioActualizacion _instancia = new ServicioActualizacion();
        public static ServicioActualizacion Instancia => _instancia;

        public event Action ActualizarProductos;

        public void NotificarActualizacion()
        {
            ActualizarProductos?.Invoke();
        }
    }
    public partial class PageVentas : Page
    {

        public string idventap;
        public string dniempleado;
        private DispatcherTimer searchTimer;
        private const int SEARCH_DELAY = 300; // milisegundos
        public string pago;
        public string cantidadv="1";
        public string pg;
        public static string idv;
        private readonly string API_URL = "https://facturaciondirecta.com/API_SUNAT/post.php";
        private readonly HttpClient client;
        private List<entDetalleVenta> detallesVenta;
        public PageVentas(string dniemp)
        {
            InitializeComponent();
            dniempleado = dniemp;
            CargarProductos();
            searchTimer = new DispatcherTimer();
            searchTimer.Interval = TimeSpan.FromMilliseconds(SEARCH_DELAY);
            searchTimer.Tick += SearchTimer_Tick;
            ServicioActualizacion.Instancia.ActualizarProductos += CargarProductos;

        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ServicioActualizacion.Instancia.ActualizarProductos -= CargarProductos;
        }
        private void ActualizarVistaCarrito()
        {
            try
            {
                var detalles = logDetalleVenta.Instancia.MostrarDetallesVenta(Convert.ToInt32(idventap));
                itemsControlCarrito.ItemsSource = detalles;

                // Actualizar el total
                decimal total = detalles.Sum(d => Convert.ToDecimal(d.Subtotal));
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
            Pago formPago = new Pago(idventap, txtDniCliente.Text, txtNombreCliente.Text, pago);
            if (formPago.ShowDialog() == true)  // Si el pago fue exitoso
            {
                CargarProductos();

            }
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
                    precio = precioUnitario,
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
                    ActualizarVistaCarrito();
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
            try
            {
                var button = sender as System.Windows.Controls.Button;
                if (button == null) return;

                var parent = VisualTreeHelper.GetParent(button);
                while (parent != null && !(parent is FrameworkElement element && element.DataContext is entDetalleVenta))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (parent is FrameworkElement parentElement)
                {
                    var detalle = parentElement.DataContext as entDetalleVenta;
                    if (detalle != null && detalle.Cantidad > 1)
                    {
                        logDetalleVenta.Instancia.ModificarCantidadDetalle(
                            detalle.ID_Detalle_venta,
                            detalle.Cantidad - 1,
                            detalle.precio
                        );
                        ActualizarVistaCarrito();
                    }
                    else if (detalle.Cantidad == 1)
                    {
                        // Opcional: preguntar si desea eliminar el producto
                        var result = System.Windows.MessageBox.Show(
                            "La cantidad llegará a 0. ¿Desea eliminar el producto del carrito?",
                            "Confirmar eliminación",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question
                        );

                        if (result == MessageBoxResult.Yes)
                        {
                            logDetalleVenta.Instancia.EliminarDetalleVenta(detalle.ID_Detalle_venta);
                            ActualizarVistaCarrito();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al disminuir cantidad: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AumentarCantidad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                if (button == null) return;

                var parent = VisualTreeHelper.GetParent(button);
                while (parent != null && !(parent is FrameworkElement element && element.DataContext is entDetalleVenta))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (parent is FrameworkElement parentElement)
                {
                    var detalle = parentElement.DataContext as entDetalleVenta;
                    if (detalle != null)
                    {
                        // Validar stock antes de aumentar
                        var producto = logProducto.Instancia.BuscarProductoPorId(detalle.id_Producto);
                        if (producto != null && detalle.Cantidad < producto.stock)
                        {
                            logDetalleVenta.Instancia.ModificarCantidadDetalle(
                                detalle.ID_Detalle_venta,
                                detalle.Cantidad + 1,
                                detalle.precio
                            );
                            ActualizarVistaCarrito();
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("No hay suficiente stock disponible",
                                "Stock insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al aumentar cantidad: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EliminarProducto_Click(object sender, RoutedEventArgs e)

        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                if (button == null) return;

                var detalle = button.DataContext as entDetalleVenta;
                if (detalle == null)
                {
                    System.Windows.MessageBox.Show("No se pudo obtener la información del producto",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = System.Windows.MessageBox.Show("¿Está seguro que desea eliminar este producto?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    logDetalleVenta.Instancia.EliminarDetalleVenta(detalle.ID_Detalle_venta);
                    ActualizarVistaCarrito(); // Usar el método que ya tienes
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al eliminar el producto: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void btnlimpiar_Click(object sender, RoutedEventArgs e)
        {
            itemsControlCarrito.ItemsSource = null;
            txtDniCliente.Text = "";
            txtNombreCliente.Text = "";
        }
    }

}
