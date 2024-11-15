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
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para pagotarjeta.xaml
    /// </summary>
    public partial class pagotarjeta : Page
    {
        private decimal montoTotal;
        private string numeroOrden;
        public pagotarjeta(decimal monto, string orden)
        {
            InitializeComponent();
            montoTotal = monto;
            numeroOrden = orden;

            txtMontoTotal.Text = $"S/{monto:N2}";
            txtNumeroOrden.Text = orden;

            // Configurar MercadoPago
            MercadoPagoConfig.AccessToken = mercadoPagoConfig.AccessToken;
        }
        private async void ProcesarPago_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(txtNumeroTarjeta.Text) ||
                    string.IsNullOrWhiteSpace(txtFechaVencimiento.Text) ||
                    string.IsNullOrWhiteSpace(txtCVV.Password) ||
                    string.IsNullOrWhiteSpace(txtTitular.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    System.Windows.MessageBox.Show("Por favor complete todos los campos",
                                  "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Crear preferencia de pago
                var client = new PreferenceClient();

                var preferenceRequest = new PreferenceRequest
                {
                    Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = $"Orden #{numeroOrden}",
                        Quantity = 1,
                        CurrencyId = "PEN",
                        UnitPrice = montoTotal
                    }
                },
                    Payer = new PreferencePayerRequest
                    {
                        Email = txtEmail.Text
                    },
                    ExternalReference = numeroOrden
                };

                var preference = await client.CreateAsync(preferenceRequest);

                // Aquí normalmente redirigirías al usuario a la URL de pago
                // Para pruebas, mostraremos un mensaje de éxito
                System.Windows.MessageBox.Show("Pago procesado exitosamente",
                              "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al procesar el pago: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    public static class mercadoPagoConfig
    {
        public const string PublicKey = "TEST-c43f074d-2e8a-4554-b215-55bf8756ffba";
        public const string AccessToken = "TEST-1879635287556006-111518-2da16b77a0d0cc1a1803d481370a0409-2100641684";
    }
}
