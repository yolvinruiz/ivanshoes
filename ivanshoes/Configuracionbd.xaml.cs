using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
    /// <summary>
    /// Lógica de interacción para Configuracionbd.xaml
    /// </summary>
    public partial class Configuracionbd : Window
    {
        public Configuracionbd()
        {
            InitializeComponent();
            CargarConfiguracion();
        }

        private void CargarConfiguracion()
        {
            txtServidor.Text = ConfigurationManager.AppSettings["Servidor"] ?? "";
            txtBaseDatos.Text = ConfigurationManager.AppSettings["BaseDatos"] ?? "";
            txtUsuario.Text = ConfigurationManager.AppSettings["Usuario"] ?? ""; // Para Azure
            txtContraseña.Password = ConfigurationManager.AppSettings["Contraseña"] ?? ""; // Para Azure
        }

        private void ProbarConexion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = $"Server=tcp:{txtServidor.Text},1433; " +
                                          $"Database={txtBaseDatos.Text}; " +
                                          $"User ID={txtUsuario.Text}; " +
                                          $"Password={txtContraseña.Password}; " +
                                          "Encrypt=True; TrustServerCertificate=False; Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    System.Windows.MessageBox.Show("Conexión exitosa",
                                  "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al conectar: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GuardarConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtServidor.Text) ||
                    string.IsNullOrWhiteSpace(txtBaseDatos.Text) ||
                    string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                    string.IsNullOrWhiteSpace(txtContraseña.Password))
                {
                    System.Windows.MessageBox.Show("Debe ingresar todos los campos",
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Abrir el archivo de configuración
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Actualizar los valores
                config.AppSettings.Settings["Servidor"].Value = txtServidor.Text;
                config.AppSettings.Settings["BaseDatos"].Value = txtBaseDatos.Text;
                config.AppSettings.Settings["Usuario"].Value = txtUsuario.Text;
                config.AppSettings.Settings["Contraseña"].Value = txtContraseña.Password;

                // Guardar los cambios
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                System.Windows.MessageBox.Show("Configuración guardada exitosamente",
                              "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al guardar la configuración: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
