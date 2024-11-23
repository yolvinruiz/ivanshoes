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
using System.IO;
using CapaEntidad;

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
            // Cargar el fondo guardado si existe
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
            int idEmpleado;
            try
            {
                // Validar ID ingresado
                if (txtDNI.Text == null)
                {
                    System.Windows.MessageBox.Show("Por favor, ingresa un dni válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (txtPassword.Password == null)
                {
                    System.Windows.MessageBox.Show("Por favor, ingrese una contraseña válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                idEmpleado = logEmpleado.Instancia.BuscarIdempleadoPorDNI(Convert.ToInt32(txtDNI.Text));
                string contraseña = txtPassword.Password;

                // Llamar al método de la capa de negocio
                string mensaje = logLogin.Instancia.VerificarLogin(idEmpleado, contraseña);

                // Mostrar el mensaje en un MessageBox
                System.Windows.MessageBox.Show(mensaje, "Resultado del Login", MessageBoxButton.OK, MessageBoxImage.Information);

                // Si el mensaje contiene "Bienvenido", redirigir
                if (mensaje.StartsWith("Bienvenido Administrador"))
                {
                    Administrador mainWindow = new Administrador();
                    mainWindow.Show();
                    this.Close();
                    SesionActual.IDEmpleado = idEmpleado;
                }
                if (mensaje.StartsWith("Bienvenido Vendedor"))
                {
                    var empleado = logEmpleado.Instancia.bucarempleadopordni(Convert.ToInt32(txtDNI.Text));
                    Empleado mainWindow = new Empleado(empleado.Nombre,empleado.Apellidos,empleado.DNI);
                    mainWindow.Show();
                    this.Close();
                    SesionActual.IDEmpleado = idEmpleado;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
        public void ActualizarFondo(string imagePath)
        {
            try
            {
                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(imagePath)));
                brush.Stretch = Stretch.UniformToFill;
                this.Background = brush;
            }
            catch { }
        }

        public void RestaurarFondoDefault()
        {
            // Establecer el fondo predeterminado
            this.Background = new SolidColorBrush(Colors.White); // O el color que prefieras
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Empleado empleado = new Empleado("","",1);
            empleado.Show();
        }
    }
}
