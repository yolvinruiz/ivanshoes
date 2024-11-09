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
using CapaEntidad;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para AgregarProducto.xaml
    /// </summary>
    public partial class AgregarProducto : Window
    {
        private string rutaImagenSeleccionada;
        public AgregarProducto()
        {
            InitializeComponent();
            CargarCombos();
        }
        public static BitmapImage CargarImagen(string rutaCompleta)
        {
            try
            {
                if (File.Exists(rutaCompleta))
                {
                    return new BitmapImage(new Uri(rutaCompleta));
                }
            }
            catch { }

            // Retornar imagen por defecto si hay error
            return new BitmapImage(new Uri("/Recursos/no-image.png", UriKind.Relative));
        }
        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            cmbTipoProducto.SelectedIndex = -1;
            cmbMarca.SelectedIndex = -1;
            cmbTalla.SelectedIndex = -1;
            cmbColor.SelectedIndex = -1;
            cmbCategoria.SelectedIndex = -1;
            imgPreview.Source = CargarImagen("/Recursos/no-image.png");
            rutaImagenSeleccionada = null;
        }

        private void CargarCombos()
        {
            try
            {
                // Cargar tipos de producto
                var tiposProducto = logProducto.Instancia.ListarTiposProducto()
                    .Select(x => x.Value)
                    .ToList();
                cmbTipoProducto.ItemsSource = tiposProducto;

                // Cargar marcas
                var marcas = logProducto.Instancia.ListarMarcas()
                    .Select(x => x.Value)
                    .ToList();
                cmbMarca.ItemsSource = marcas;

                // Cargar tallas
                var tallas = logProducto.Instancia.ListarTallas()
                    .Select(x => x.Value)
                    .ToList();
                cmbTalla.ItemsSource = tallas;

                // Cargar colores
                var colores = logProducto.Instancia.ListarColores()
                    .Select(x => x.Value)
                    .ToList();
                cmbColor.ItemsSource = colores;

                // Cargar categorías
                var categorias = logProducto.Instancia.ListarCategorias()
                    .Select(x => x.Value)
                    .ToList();
                cmbCategoria.ItemsSource = categorias;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar los datos: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BuscarProducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBuscarId.Text))
                {
                    System.Windows.MessageBox.Show("Ingrese un ID para buscar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtBuscarId.Text, out int idProducto))
                {
                    System.Windows.MessageBox.Show("El ID debe ser un número", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                entProducto producto = logProducto.Instancia.BuscarProductoPorId(idProducto);

                if (producto != null)
                {
                    txtNombre.Text = producto.nombre;
                    txtPrecio.Text = producto.precio.ToString();
                    txtStock.Text = producto.stock.ToString();

                    // Seleccionar los valores en los ComboBox
                    cmbTipoProducto.SelectedItem = producto.NombreTipoProducto;
                    cmbMarca.SelectedItem = producto.NombreMarca;
                    cmbTalla.SelectedItem = producto.NombreTalla;
                    cmbColor.SelectedItem = producto.NombreColor;
                    cmbCategoria.SelectedItem = producto.NombreCategoria;

                    // Cargar imagen
                    if (!string.IsNullOrEmpty(producto.Imagen))
                    {
                        rutaImagenSeleccionada = producto.Imagen;
                        imgPreview.Source = CargarImagen(producto.Imagen);
                    }
                    else
                    {
                        imgPreview.Source = CargarImagen("/Recursos/no-image.png");
                        rutaImagenSeleccionada = null;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("No se encontró el producto", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al buscar el producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ModificarProducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar que se haya buscado un producto
                if (string.IsNullOrEmpty(txtBuscarId.Text))
                {
                    System.Windows.MessageBox.Show("Primero debe buscar un producto", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validaciones
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    System.Windows.MessageBox.Show("Debe ingresar el nombre del producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtNombre.Focus();
                    return;
                }

                if (!double.TryParse(txtPrecio.Text, out double precio) || precio <= 0)
                {
                    System.Windows.MessageBox.Show("El precio debe ser un número mayor a 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtPrecio.Focus();
                    return;
                }

                if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    System.Windows.MessageBox.Show("El stock debe ser un número entero no negativo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtStock.Focus();
                    return;
                }

                // Validar selección de ComboBoxes
                if (cmbTipoProducto.SelectedItem == null || cmbMarca.SelectedItem == null ||
                    cmbTalla.SelectedItem == null || cmbColor.SelectedItem == null ||
                    cmbCategoria.SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("Debe seleccionar todos los campos (Tipo, Marca, Talla, Color y Categoría)",
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Crear entidad Producto para modificar
                entProducto producto = new entProducto
                {
                    id_producto = int.Parse(txtBuscarId.Text),
                    nombre = txtNombre.Text.Trim(),
                    precio = precio,
                    stock = stock,
                    NombreTipoProducto = cmbTipoProducto.SelectedItem.ToString(),
                    NombreMarca = cmbMarca.SelectedItem.ToString(),
                    NombreTalla = cmbTalla.SelectedItem.ToString(),
                    NombreColor = cmbColor.SelectedItem.ToString(),
                    NombreCategoria = cmbCategoria.SelectedItem.ToString(),
                    Imagen = rutaImagenSeleccionada ?? ""
                };

                // Modificar producto
                logProducto.Instancia.ModificarProducto(producto);

                System.Windows.MessageBox.Show("Producto modificado exitosamente", "Éxito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpiar formulario
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al modificar el producto: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LimpiarFormulario_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }

        private void Agregarproductoab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    System.Windows.MessageBox.Show("Debe ingresar el nombre del producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtNombre.Focus();
                    return;
                }

                if (!double.TryParse(txtPrecio.Text, out double precio) || precio <= 0)
                {
                    System.Windows.MessageBox.Show("El precio debe ser un número mayor a 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtPrecio.Focus();
                    return;
                }

                if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    System.Windows.MessageBox.Show("El stock debe ser un número entero no negativo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtStock.Focus();
                    return;
                }

                // Validar selección de ComboBoxes
                if (cmbTipoProducto.SelectedItem == null || cmbMarca.SelectedItem == null ||
                    cmbTalla.SelectedItem == null || cmbColor.SelectedItem == null ||
                    cmbCategoria.SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("Debe seleccionar todos los campos (Tipo, Marca, Talla, Color y Categoría)",
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Crear entidad Producto
                entProducto producto = new entProducto
                {
                    nombre = txtNombre.Text.Trim(),
                    precio = precio,
                    stock = stock,
                    NombreTipoProducto = cmbTipoProducto.SelectedItem.ToString(),
                    NombreMarca = cmbMarca.SelectedItem.ToString(),
                    NombreTalla = cmbTalla.SelectedItem.ToString(),
                    NombreColor = cmbColor.SelectedItem.ToString(),
                    NombreCategoria = cmbCategoria.SelectedItem.ToString(),
                    Imagen = rutaImagenSeleccionada ?? ""
                };

                // Guardar producto
                logProducto.Instancia.InsertarProducto(producto);

                System.Windows.MessageBox.Show("Producto guardado exitosamente", "Éxito",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpiar formulario
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al guardar el producto: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SeleccionarImagenp_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.gif|Todos los archivos|*.*",
                Title = "Seleccionar imagen del producto"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Crear directorio si no existe
                    string directorioImagenes = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes");
                    if (!Directory.Exists(directorioImagenes))
                    {
                        Directory.CreateDirectory(directorioImagenes);
                    }

                    // Generar nombre único para la imagen
                    string nombreArchivo = $"producto_{DateTime.Now.Ticks}{System.IO.Path.GetExtension(openFileDialog.FileName)}";
                    string rutaDestino = System.IO.Path.Combine(directorioImagenes, nombreArchivo);

                    // Copiar imagen
                    File.Copy(openFileDialog.FileName, rutaDestino, true);

                    // Guardar ruta completa
                    rutaImagenSeleccionada = rutaDestino; // Guardamos la ruta completa

                    // Mostrar vista previa
                    imgPreview.Source = new BitmapImage(new Uri(rutaDestino));
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error al procesar la imagen: {ex.Message}");
                }
            }
        }


    }
}
