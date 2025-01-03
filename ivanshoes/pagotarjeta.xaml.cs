﻿using System;
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
using CapaEntidad;
using CapaLogica;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Diagnostics;
using System.Threading;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Client.Payment;
using MercadoPago.Resource.Preference;
using MercadoPago.Resource.Payment;
using MercadoPago.Client.Common;


namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para pagotarjeta.xaml
    /// </summary>
    public partial class pagotarjeta : Page
    {
        public string pg;
        public string idv;
        private readonly string API_URL = "https://facturaciondirecta.com/API_SUNAT/post.php";
        private readonly HttpClient client;
        private List<entDetalleVenta> detallesVenta;
        public int tipodeigv = 1;
        public string dniclie;
        public double monototot;
        public pagotarjeta(string idventa, string dnicliente, string nombrecliente, string montototal)
        {
            InitializeComponent();
            txtNumeroOrden.Text = idventa;
            idv = idventa;
            client = new HttpClient();
            detallesVenta = logDetalleVenta.Instancia.ListarDetallesPorVenta(Convert.ToInt32(idv));
            txtDocumentoCliente.Text = dnicliente;
            txtNombreCliente.Text = nombrecliente;
            txtMontoTotal.Text = "S/" + montototal;
            monototot = Convert.ToDouble(montototal);
            dniclie = dnicliente;
            // Configuración de MercadoPago Producción
        }
  
    
        private string CleanJsonResponse(string response)
        {
            // Buscar el primer { y el último }
            int startIndex = response.IndexOf('{');
            int endIndex = response.LastIndexOf('}');

            if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
            {
                return response.Substring(startIndex, endIndex - startIndex + 1);
            }

            throw new Exception("No se pudo encontrar un JSON válido en la respuesta");
        }

        private async void EmitirComprobante_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Variables para el documento
                string serie, tipoEntidadCliente, documento, tipoDocumentoCodigo;

                // Determinar tipo de documento según RadioButton seleccionado
                if (rbBoleta.IsChecked == true)
                {
                    serie = "BB03";
                    tipoEntidadCliente = "1"; // DNI
                    documento = txtDocumentoCliente.Text;
                    tipoDocumentoCodigo = "03"; // Código para Boleta
                    btnEmitir.Content = "Emitir Boleta";
                }
                else
                {
                    serie = "FF03";
                    tipoEntidadCliente = "6"; // RUC
                    documento = txtDocumentoCliente.Text;
                    tipoDocumentoCodigo = "01"; // Código para Factura
                    btnEmitir.Content = "Emitir Factura";
                }

                // Calcular totales
                double totalGravada = 0;
                List<object> items = new List<object>();

                foreach (var detalle in detallesVenta)
                {
                    double precioUnitarioOriginal = detalle.precio;
                    double subtotalOriginal = detalle.Subtotal;

                    // Calcular precio base (sin IGV)
                    double precioBaseUnitario = Math.Round(precioUnitarioOriginal / 1.18, 2);
                    double subtotalBase = Math.Round(subtotalOriginal / 1.18, 2);
                    totalGravada += subtotalBase;

                    items.Add(new
                    {
                        producto = detalle.nombre,
                        cantidad = detalle.Cantidad.ToString(),
                        precio_base = precioBaseUnitario.ToString("0.00"),
                        codigo_sunat = "-",
                        codigo_producto = detalle.id_Producto.ToString(),
                        codigo_unidad = "NIU",
                        tipo_igv_codigo = "10"
                    });
                }

                double igv = Math.Round(totalGravada * 0.18, 2);

                var jsonData = new
                {
                    empresa = new
                    {
                        ruc = "20480674414",
                        razon_social = "Tienda de ropa IVANSHOES",
                        nombre_comercial = "IVAN SHOES",
                        domicilio_fiscal = "MZ 121 Lt 10 viru",
                        ubigeo = "150114",
                        urbanizacion = "VIRU",
                        distrito = "VIRU",
                        provincia = "LIMA",
                        departamento = "LIMA",
                        modo = "0",
                        usu_secundario_produccion_user = "MODDATOS",
                        usu_secundario_produccion_password = "MODDATOS"
                    },
                    cliente = new
                    {
                        razon_social_nombres = txtNombreCliente.Text,
                        numero_documento = documento,
                        codigo_tipo_entidad = tipoEntidadCliente,
                        cliente_direccion = "-"
                    },
                    venta = new
                    {
                        serie = serie,
                        numero = idv,
                        fecha_emision = DateTime.Now.ToString("yyyy-MM-dd"),
                        hora_emision = DateTime.Now.ToString("HH:mm:ss"),
                        fecha_vencimiento = "",
                        moneda_id = "1",
                        forma_pago_id = "2",
                        total_gravada = totalGravada.ToString("0.00"),
                        total_igv = igv.ToString("0.00"),
                        total_exonerada = "",
                        total_inafecta = "",
                        tipo_documento_codigo = tipoDocumentoCodigo,
                        nota = $"Venta realizada"
                    },
                    items = items
                };

                // Enviar a SUNAT y procesar respuesta
                var content = new StringContent(JsonConvert.SerializeObject(jsonData), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(API_URL, content);
                var responseString = await response.Content.ReadAsStringAsync();

                var cleanJson = CleanJsonResponse(responseString);
                var responseData = JsonConvert.DeserializeObject<RootResponse>(cleanJson);

                if (responseData?.data != null)
                {
                    entBoleta nuevoDocumento = new entBoleta
                    {
                        id_Venta = Convert.ToInt32(idv),
                        Serie = serie,
                        DigestValueon = responseData.data.codigo_hash?.ToString() ?? "",
                        Estado_sunat = responseData.data.respuesta_sunat_codigo,
                        Mensaje_sunat = responseData.data.respuesta_sunat_descripcion,
                        Xml_filename = responseData.data.ruta_xml, // Cambiado de xml_base_64
                        Pdf_filename = responseData.data.ruta_pdf,
                        Cdr_filename = responseData.data.ruta_cdr  // Cambiado de cdr_base_64
                    };
                    logBoleta.Instancia.InsertarBoleta(nuevoDocumento);

                    // Descargar PDF
                    if (!string.IsNullOrEmpty(responseData.data.ruta_pdf))
                    {
                        var pdfResponse = await client.GetAsync(responseData.data.ruta_pdf);
                        if (pdfResponse.IsSuccessStatusCode)
                        {
                            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                            {
                                Filter = "PDF files (*.pdf)|*.pdf",
                                FileName = $"documento-{serie}-{idv}.pdf"
                            };

                            if (saveFileDialog.ShowDialog() == true)
                            {
                                var pdfBytes = await pdfResponse.Content.ReadAsByteArrayAsync();
                                File.WriteAllBytes(saveFileDialog.FileName, pdfBytes);
                                System.Windows.MessageBox.Show("Documento emitido y guardado exitosamente!",
                                    "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                        }
                    }

                    // Registrar el pago
                    logPago.Instancia.RegistrarPagoYActualizarVenta(Convert.ToInt32(idv), 1);
                    System.Windows.MessageBox.Show("Pago registrado con éxito");

                    // Limpiar y cerrar
                    btnEmitir.IsEnabled = false;
                    ServicioActualizacion.Instancia.NotificarActualizacion();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al procesar el documento: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void rbBoleta_Checked(object sender, RoutedEventArgs e)
        {
            if (btnEmitir != null && lblDocumento != null)  // Verificar que los controles existan
            {
                btnEmitir.Content = "Emitir Boleta";
                lblDocumento.Text = "DNI del Cliente";
                txtDocumentoCliente.Text = dniclie;
            }
        }

        private void rbFactura_Checked(object sender, RoutedEventArgs e)
        {
            if (btnEmitir != null && lblDocumento != null)  // Verificar que los controles existan
            {
                btnEmitir.Content = "Emitir Factura";
                lblDocumento.Text = "RUC del Cliente";
                txtDocumentoCliente.Text = "";
            }
        }
        private MercadoPagoService _mpService;
        private string _currentPaymentId;

        private async void ProcesarPago_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_mpService == null)
                {
                    _mpService = new MercadoPagoService("APP_USR-8501224929440754-112215-f45dc2c0ff74146fa80e63e261a40019-2100641684"); // Reemplazar con tu access token
                }

                btnpagar.IsEnabled = false; // Deshabilitar el botón mientras se procesa

                // Obtener el monto total del TextBox
                decimal montoTotal = Convert.ToDecimal(monototot);

                // Generar un ID único para esta transacción
                _currentPaymentId = Guid.NewGuid().ToString();

                // Crear la preferencia de pago
                string paymentUrl = await _mpService.CrearPreferencia(
                    montoTotal,
                    $"pago - Orden #{idv}",
                    _currentPaymentId
                );

                // Abrir el navegador con la URL de pago
                Process.Start(new ProcessStartInfo
                {
                    FileName = paymentUrl,
                    UseShellExecute = true
                });

                // Iniciar verificación periódica del pago
                await VerificarPagoPeriodicamenteAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al procesar el pago: {ex.Message}");
            }
            finally
            {
                btnpagar.IsEnabled = true; // Rehabilitar el botón
            }

        }
        private async Task VerificarPagoPeriodicamenteAsync()
        {
            int intentos = 0;
            const int maximoIntentos = 20; // 5 minutos máximo (15 segundos * 20 intentos)

            while (intentos < maximoIntentos)
            {
                try
                {
                    bool pagoCorrecto = await _mpService.VerificarPago(_currentPaymentId);

                    if (pagoCorrecto)
                    {
                        System.Windows.MessageBox.Show("¡Pago realizado con éxito!", "Éxito");

                        // Aquí puedes actualizar el estado de la venta en tu base de datos
                        // y continuar con el proceso de emisión del comprobante

                        return;
                    }

                    await Task.Delay(15000); // Esperar 15 segundos entre cada verificación
                    intentos++;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error al verificar el pago: {ex.Message}");
                    return;
                }
            }

            System.Windows.MessageBox.Show("El tiempo de espera para la verificación del pago ha expirado. " +
                "Por favor, verifique manualmente el estado del pago.");
        }
    }

}
