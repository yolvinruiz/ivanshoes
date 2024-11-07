using CapaLogica;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindow loginWindow;
        public MainWindow()
        {
            InitializeComponent();
        
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mostrar el login cuando se cierre esta ventana
            loginWindow = new MainWindow();
            loginWindow.Show();
        }

        private void txtDNI_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDNI.Text) || string.IsNullOrEmpty(txtPassword.Password))
                {
                    System.Windows.MessageBox.Show("Por favor ingrese DNI y contraseña",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int dni = Convert.ToInt32(txtDNI.Text);
                string contraseña = txtPassword.Password;

                var usuario = logUsuario.Instancia.ValidarUsuario(dni, contraseña);

                if (usuario != null)
                {
                    App.Current.Properties["UsuarioActual"] = usuario;

                    Window ventanaNueva;
                    if (usuario.Cargo.ToUpper() == "VENDEDOR")
                    {
                        var empleado = logEmpleado.Instancia.bucarempleadopordni(dni);
                        ventanaNueva = new Empleado(empleado.Nombre,empleado.Apellidos,empleado.DNI);
                    }
                    else if (usuario.Cargo.ToUpper() == "ADMINISTRADOR")
                    {
                        ventanaNueva = new Administrador();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Cargo no reconocido",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Configurar la nueva ventana
                    ventanaNueva.Closing += (s, args) =>
                    {
                        // Crear y mostrar una nueva instancia de login
                        var newLogin = new MainWindow();
                        newLogin.Show();
                    };

                    // Mostrar la nueva ventana y cerrar el login actual
                    ventanaNueva.Show();
                    this.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("DNI o contraseña incorrectos",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void txtDNI_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Solo permitir números
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrEmpty(txtPassword.Password) ?
                Visibility.Visible : Visibility.Hidden;
        }
    }
}
