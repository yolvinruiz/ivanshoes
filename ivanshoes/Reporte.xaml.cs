using CapaLogica;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para Reporte.xaml
    /// </summary>
    public partial class Reporte : Page
    {
        public Reporte()
        {
            InitializeComponent();
            InicializarFechas();
            CargarDatos();
        }
        private void InicializarFechas()
        {
            dpFechaInicio.SelectedDate = DateTime.Today;
            dpFechaFin.SelectedDate = DateTime.Today;
        }
        private void CargarDatos()
        {
            try
            {
                var fechaInicio = dpFechaInicio.SelectedDate ?? DateTime.Today;
                var fechaFin = dpFechaFin.SelectedDate ?? DateTime.Today;

                var estadisticas = logDashboard.Instancia.ObtenerEstadisticasProductos(
                    fechaInicio, fechaFin);

                dgEstadisticas.ItemsSource = estadisticas;

                // Calcular totales
                decimal totalVendido = estadisticas.Sum(e => e.MontoVendido);
                decimal gananciaTotal = estadisticas.Sum(e => e.Ganancia);

                txtTotalVendido.Text = totalVendido.ToString("C2");
                txtGananciaTotal.Text = gananciaTotal.ToString("C2");
                txtGananciaTotal.Foreground = gananciaTotal >= 0 ?
                    System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar los datos: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }
        private void btnExportar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    FileName = $"Estadisticas_Productos_{DateTime.Now:yyyyMMdd}"
                };

                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var estadisticas = dgEstadisticas.ItemsSource as IEnumerable<EstadisticaProducto>;
                    ExportarAExcel(estadisticas, saveFileDialog.FileName);
                    System.Windows.MessageBox.Show("Archivo exportado exitosamente",
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al exportar: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExportarAExcel(IEnumerable<EstadisticaProducto> datos, string rutaArchivo)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Estadísticas");

                // Encabezados
                worksheet.Cell(1, 1).Value = "Producto";
                worksheet.Cell(1, 2).Value = "Precio Unitario";
                worksheet.Cell(1, 3).Value = "Costo Unitario";
                worksheet.Cell(1, 4).Value = "Unidades Vendidas";
                worksheet.Cell(1, 5).Value = "Monto Vendido";
                worksheet.Cell(1, 6).Value = "Ganancia";

                // Datos
                var row = 2;
                foreach (var item in datos)
                {
                    worksheet.Cell(row, 1).Value = item.NombreProducto;
                    worksheet.Cell(row, 2).Value = item.PrecioUnitario;
                    worksheet.Cell(row, 3).Value = item.CostoUnitario;
                    worksheet.Cell(row, 4).Value = item.UnidadesVendidas;
                    worksheet.Cell(row, 5).Value = item.MontoVendido;
                    worksheet.Cell(row, 6).Value = item.Ganancia;
                    row++;
                }

                // Formato
                var rango = worksheet.Range(1, 1, row - 1, 6);
                rango.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rango.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(rutaArchivo);
            }
        }

    }

}

