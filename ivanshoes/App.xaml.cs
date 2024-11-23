using CapaLogica;
using MercadoPago.Config;
using System.Configuration;
using System.Data;
using System.Windows;
using CapaEntidad;

namespace ivanshoes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Manejar excepciones no controladas
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show($"Ha ocurrido un error: {e.Exception.Message}",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Limpiar recursos si es necesario
            Properties.Clear();
            base.OnExit(e);
            try
            {
                // Asegúrate de guardar el ID del empleado activo en una variable global o de sesión
                int idEmpleadoActivo = SesionActual.IDEmpleado;

                // Llamar al método de la capa de negocio para cerrar la sesión
                logLogin.Instancia.CerrarSesion(idEmpleadoActivo);
            }
            catch (Exception ex)
            {
                // Registrar o manejar el error
                Console.WriteLine("Error al cerrar la sesión del usuario activo: " + ex.Message);
            }
        }
    }

}
