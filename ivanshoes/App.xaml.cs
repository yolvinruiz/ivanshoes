using MercadoPago.Config;
using System.Configuration;
using System.Data;
using System.Windows;

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
            MercadoPagoConfig.AccessToken = "APP_USR-7906504568293219-112019-9528c0c622645a0d91b35dabbb091f29-2107940985";
        }
    }

}
