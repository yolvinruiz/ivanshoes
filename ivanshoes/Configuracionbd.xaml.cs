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
        }

        private void ProbarConexion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = $"Data Source={txtServidor.Text}; " +
                                        $"Initial Catalog={txtBaseDatos.Text}; " +
                                        "Integrated Security=true";

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
                    string.IsNullOrWhiteSpace(txtBaseDatos.Text))
                {
                    System.Windows.MessageBox.Show("Debe ingresar todos los campos",
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Guardar en el archivo de configuración
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings.Remove("Servidor");
                config.AppSettings.Settings.Add("Servidor", txtServidor.Text);

                config.AppSettings.Settings.Remove("BaseDatos");
                config.AppSettings.Settings.Add("BaseDatos", txtBaseDatos.Text);

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
