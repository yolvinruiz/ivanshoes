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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Text;
using CapaEntidad;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para VentanaPagoEfectivo.xaml
    /// </summary>
    public partial class VentanaPagoEfectivo : Page
    {
        public string pg;
        public static string idv;
        private readonly string API_URL = "https://facturaciondirecta.com/API_SUNAT/post.php";
        private readonly HttpClient client;
        private List<entDetalleVenta> detallesVenta;
        public VentanaPagoEfectivo(string pago, string idventa)
        {
            InitializeComponent();
            cmbTipoIGV.SelectedIndex = 0;
            cmbtipocomprovante.SelectedIndex = 0;
            client = new HttpClient();
            pg = pago;
            idv = idventa;
            detallesVenta = logDetalleVenta.Instancia.ListarDetallesPorVenta(Convert.ToInt32(idv));
            txtpago.Text = pg;
        }


        private void LimpiarDataGridEnOtrapagina()
        {
            // Obtén la referencia a VentanaObjetivo si está abierta
            var ventanaObjetivo = System.Windows.Application.Current.Windows.OfType<Empleado>().FirstOrDefault();

            if (ventanaObjetivo != null)
            {
                // Llama al método para limpiar el DataGrid en VentanaObjetivo
                ventanaObjetivo.LimpiarDataGridEnPaginaObjetivo();
            }
            else
            {
                System.Windows.MessageBox.Show("La ventana objetivo no está abierta.");
            }

        }

        private void txtmontorecibido_TextChanged(object sender, TextChangedEventArgs e)
        {
            try { 
            double cambio = Convert.ToDouble(txtmontorecibido.Text) - Convert.ToDouble(txtpago.Text);
            txtcambio.Text = cambio.ToString();
                }
            catch (Exception ex){ System.Windows.MessageBox.Show("Ocurrió un error: " + ex.Message); }
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

        private async void Realiza_boleta_Click(object sender, RoutedEventArgs e)
        {
            double factorIGV = 0;
            if (cmbTipoIGV.SelectedIndex == 0) {
                factorIGV = 1.18;
            }
            else if (cmbTipoIGV.SelectedIndex == 1)
            {
                factorIGV = 1.00;
            }
            string serie1="";
            string Tipentcli=""; 
            string document1 = "";
            string tdc = "";

            try
            {
                entCliente cliente = logCliente.Instancia.BuscarClientePorIdVenta(Convert.ToInt32(idv));
                if(cmbtipocomprovante.SelectedIndex == 0) {
                    serie1 = "BB01";
                    Tipentcli = "1";
                    document1 = cliente.DNI.ToString();
                    tdc = "03";
                }
                else if (cmbtipocomprovante.SelectedIndex == 1) {
                    serie1 = "FF03";
                    Tipentcli = "6";
                    document1 = txtRuc.Text;
                    tdc = "01";
                }
                System.Windows.MessageBox.Show(serie1 + "/ "+ Tipentcli + "/ " + document1 +"/ "+ tdc );
                // Calcular totales
                double totalVenta = 0;
                double totalGravada = 0;
                List<object> items = new List<object>();
                foreach (var detalle in detallesVenta)
                {
                    double precioUnitarioOriginal = detalle.Preciounitario;
                    double subtotalOriginal = detalle.Subtotal;

                    if (cmbTipoIGV.SelectedIndex == 0) // Precio incluye IGV
                    {
                        // Calcular precio base (sin IGV) cuando el precio incluye IGV
                        double precioBaseUnitario = Math.Round(precioUnitarioOriginal / 1.18, 2);
                        double subtotalBase = Math.Round(subtotalOriginal / 1.18, 2);
                        totalGravada += subtotalBase;
                        totalVenta += subtotalOriginal;

                        items.Add(new
                        {
                            producto = detalle.NombreProducto,
                            cantidad = detalle.Cantidad.ToString(),
                            precio_base = precioBaseUnitario.ToString("0.00"),
                            codigo_sunat = "-",
                            codigo_producto = detalle.id_Producto.ToString(),
                            codigo_unidad = "NIU",
                            tipo_igv_codigo = "10"
                        });
                    }
                    else // Precio sin IGV
                    {
                        totalGravada += subtotalOriginal;
                        totalVenta += (subtotalOriginal * 1.18); // Aumentar IGV

                        items.Add(new
                        {
                            producto = detalle.NombreProducto,
                            cantidad = detalle.Cantidad.ToString(),
                            precio_base = precioUnitarioOriginal.ToString("0.00"),
                            codigo_sunat = "-",
                            codigo_producto = detalle.id_Producto.ToString(),
                            codigo_unidad = "NIU",
                            tipo_igv_codigo = "10"
                        });
                    }
                }

                double igv = totalGravada * 0.18; // 18% de IGV

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
                        razon_social_nombres = $"{cliente.Nombre} {cliente.Apellido}",
                        numero_documento = document1,
                        codigo_tipo_entidad = Tipentcli,
                        cliente_direccion = "-"
                    },
                    venta = new
                    {
                        serie = serie1,
                        numero = idv,
                        fecha_emision = DateTime.Now.ToString("yyyy-MM-dd"),
                        hora_emision = DateTime.Now.ToString("HH:mm:ss"),
                        fecha_vencimiento = "",
                        moneda_id = "1",
                        forma_pago_id = "1",
                        total_gravada = totalGravada.ToString("0.00"),
                        total_igv = igv.ToString("0.00"),
                        total_exonerada = "",
                        total_inafecta = "",
                        tipo_documento_codigo = tdc,
                        nota = $"Venta realizada"
                    },
                    items = items
                };


                txtStatus.Text = "Enviando datos a SUNAT...";

                var content = new StringContent(JsonConvert.SerializeObject(jsonData), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(API_URL, content);
                var responseString = await response.Content.ReadAsStringAsync();

                var cleanJson = CleanJsonResponse(responseString);
                var responseData = JsonConvert.DeserializeObject<RootResponse>(cleanJson);

                if (responseData?.data != null)
                {
                    entBoleta nuevaBoleta = new entBoleta
                    {
                        id_Venta = Convert.ToInt32(idv),
                        Serie = jsonData.venta.serie,
                        DigestValueon = responseData.data.codigo_hash?.ToString() ?? "",
                        Estado_sunat = responseData.data.respuesta_sunat_codigo,
                        Mensaje_sunat = responseData.data.respuesta_sunat_descripcion,
                        Xml_filename = responseData.data.xml_base_64,
                        Pdf_filename = responseData.data.ruta_pdf,
                        Cdr_filename = responseData.data.cdr_base_64
                    };

                    logBoleta.Instancia.InsertarBoleta(nuevaBoleta);

                    if (!string.IsNullOrEmpty(responseData.data.ruta_pdf))
                    {
                        var pdfResponse = await client.GetAsync(responseData.data.ruta_pdf);
                        if (pdfResponse.IsSuccessStatusCode)
                        {
                            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                            {
                                Filter = "PDF files (*.pdf)|*.pdf",
                                FileName = $"boleta-{jsonData.venta.serie}-{jsonData.venta.numero}.pdf"
                            };

                            if (saveFileDialog.ShowDialog() == true)
                            {
                                var pdfBytes = await pdfResponse.Content.ReadAsByteArrayAsync();
                                File.WriteAllBytes(saveFileDialog.FileName, pdfBytes);
                                System.Windows.MessageBox.Show("Boleta emitida y guardada exitosamente!", "Éxito",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("No se recibió respuesta válida de SUNAT");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al emitir la boleta: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                txtStatus.Text = "Listo";
                btnGenerarFactura.IsEnabled = false;
            }

            try
            {
                int idventa = Convert.ToInt32(idv);
                int idFormaPago = 1;
                logPago.Instancia.RegistrarPagoYActualizarVenta(idventa, idFormaPago);
                System.Windows.MessageBox.Show("PAGO REGISTRADO CON EXITO");
                LimpiarDataGridEnOtrapagina();


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void cmbtipocomprovante_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    public class ResponseData
    {
        public string respuesta_sunat_codigo { get; set; }
        public string respuesta_sunat_descripcion { get; set; }
        public string ruta_xml { get; set; }
        public string ruta_cdr { get; set; }
        public string ruta_pdf { get; set; }
        public object codigo_hash { get; set; }
        public string xml_base_64 { get; set; }
        public string cdr_base_64 { get; set; }
    }

    public class RootResponse
    {
        public ResponseData data { get; set; }
    }
}
