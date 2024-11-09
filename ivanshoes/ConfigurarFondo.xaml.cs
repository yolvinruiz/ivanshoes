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
    /// Lógica de interacción para ConfigurarFondo.xaml
    /// </summary>
    public partial class ConfigurarFondo : Window
    {
        private string imagePath;
        public ConfigurarFondo()
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
        private void CargarFondoActual()
        {
            // Cargar el fondo actual si existe
            string fondoActual = Properties.Settings.Default.FondoPantalla;
            if (!string.IsNullOrEmpty(fondoActual) && File.Exists(fondoActual))
            {
                CargarImagen(fondoActual);
            }
        }

        private void SeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Seleccionar imagen de fondo"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                CargarImagen(openFileDialog.FileName);
            }
        }

        private void CargarImagen(string path)
        {
            try
            {
                var bitmap = new BitmapImage(new Uri(path));
                imgPreview.Source = bitmap;
                txtNoImage.Visibility = Visibility.Collapsed;
                imagePath = path;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar la imagen: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AplicarFondo_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                System.Windows.MessageBox.Show("Por favor seleccione una imagen primero.",
                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Guardar la ruta de la imagen en la configuración
                Properties.Settings.Default.FondoPantalla = imagePath;
                Properties.Settings.Default.Save();

                // Actualizar el fondo de todas las ventanas abiertas
                foreach (Window window in System.Windows.Application.Current.Windows)
                {
                    if (window is MainWindow ||
                      window is Administrador || window is Clientes || window is Empleado || window is ClientesVista) // Ajusta estos nombres según tus ventanas
                    {
                        try
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(imagePath)));
                            brush.Stretch = Stretch.UniformToFill;
                            window.Background = brush;
                        }
                        catch { }
                    }
                }

                System.Windows.MessageBox.Show("Fondo de pantalla actualizado exitosamente.",
                    "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al aplicar el fondo: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RestaurarDefault_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Restaurar al fondo predeterminado
                Properties.Settings.Default.FondoPantalla = "";
                Properties.Settings.Default.Save();

                // Restaurar el fondo del contenedor principal
                if (System.Windows.Application.Current.MainWindow != null)
                {
                    var mainGrid = System.Windows.Application.Current.MainWindow.Content as Grid;
                    if (mainGrid != null)
                    {
                        mainGrid.Background = new SolidColorBrush(Colors.White); // O el color que prefieras
                    }
                }

                System.Windows.MessageBox.Show("Fondo de pantalla restaurado al predeterminado.",
                    "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al restaurar el fondo: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
