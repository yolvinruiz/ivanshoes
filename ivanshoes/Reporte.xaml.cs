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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using CapaDatos;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;
using wpf = System.Windows.Controls;

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

        }


        private List<DatosVenta> ObtenerDatosVentasPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            List<DatosVenta> datos = new List<DatosVenta>();

            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(@"
                SELECT p.nombre, 
                       p.precio AS PrecioUnitario,
                       p.CostoUnitario,
                       SUM(dv.Cantidad) AS UnidadesVendidas,
                       SUM(dv.Subtotal) AS MontoVendido,
                       SUM(dv.Subtotal - (p.CostoUnitario * dv.Cantidad)) AS Ganancia
                FROM Producto p
                INNER JOIN Detalle_venta dv ON p.id_producto = dv.id_Producto
                INNER JOIN Venta v ON dv.id_Venta = v.ID_venta
                WHERE v.fecha BETWEEN @FechaInicio AND @FechaFin
                GROUP BY p.nombre, p.precio, p.CostoUnitario", cn))
                    {
                        cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                datos.Add(new DatosVenta
                                {
                                    Producto = dr["nombre"].ToString(),
                                    PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                                    CostoUnitario = Convert.ToDecimal(dr["CostoUnitario"]),
                                    UnidadesVendidas = Convert.ToInt32(dr["UnidadesVendidas"]),
                                    MontoVendido = Convert.ToDecimal(dr["MontoVendido"]),
                                    Ganancia = Convert.ToDecimal(dr["Ganancia"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener datos de ventas: " + ex.Message);
            }

            return datos;
        }
        public class DatosVenta
        {
            public string Producto { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal CostoUnitario { get; set; }
            public int UnidadesVendidas { get; set; }
            public decimal MontoVendido { get; set; }
            public decimal Ganancia { get; set; }
        }

        private void ExportarExcel_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (dgVentas.Items.Count == 0)
                {
                    System.Windows.MessageBox.Show("No hay datos para exportar",
                                  "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Archivo Excel|*.xlsx",
                    Title = "Guardar Reporte de Ventas",
                    FileName = $"ReporteVentas_{DateTime.Now:yyyyMMdd_HHmmss}"
                };

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(new FileInfo(saveDialog.FileName)))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Reporte de Ventas");

                        // Agregar título del reporte
                        worksheet.Cells[1, 1].Value = "REPORTE DE VENTAS";
                        worksheet.Cells[1, 1, 1, 6].Merge = true;
                        worksheet.Cells[1, 1].Style.Font.Size = 16;
                        worksheet.Cells[1, 1].Style.Font.Bold = true;
                        worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Agregar fechas del reporte
                        worksheet.Cells[2, 1].Value = $"Desde: {dpFechaInicio.SelectedDate:dd/MM/yyyy} - Hasta: {dpFechaFin.SelectedDate:dd/MM/yyyy}";
                        worksheet.Cells[2, 1, 2, 6].Merge = true;
                        worksheet.Cells[2, 1].Style.Font.Size = 12;
                        worksheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Encabezados de columnas
                        string[] headers = new string[]
                        {
                   "Producto",
                   "Precio Unitario",
                   "Costo Unitario",
                   "Unidades Vendidas",
                   "Monto Vendido",
                   "Ganancia"
                        };

                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cells[4, i + 1].Value = headers[i];
                            worksheet.Cells[4, i + 1].Style.Font.Bold = true;
                        }

                        // Estilo de encabezados
                        var headerRange = worksheet.Cells[4, 1, 4, headers.Length];
                        headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        headerRange.Style.Font.Bold = true;

                        // Agregar datos
                        var datos = dgVentas.ItemsSource as List<DatosVenta>;
                        int row = 5;

                        foreach (var item in datos)
                        {
                            worksheet.Cells[row, 1].Value = item.Producto;
                            worksheet.Cells[row, 2].Value = item.PrecioUnitario;
                            worksheet.Cells[row, 3].Value = item.CostoUnitario;
                            worksheet.Cells[row, 4].Value = item.UnidadesVendidas;
                            worksheet.Cells[row, 5].Value = item.MontoVendido;
                            worksheet.Cells[row, 6].Value = item.Ganancia;

                            // Formato de moneda
                            worksheet.Cells[row, 2].Style.Numberformat.Format = "₡#,##0.00";
                            worksheet.Cells[row, 3].Style.Numberformat.Format = "₡#,##0.00";
                            worksheet.Cells[row, 5].Style.Numberformat.Format = "₡#,##0.00";
                            worksheet.Cells[row, 6].Style.Numberformat.Format = "₡#,##0.00";

                            row++;
                        }

                        // Agregar totales
                        row++;
                        worksheet.Cells[row, 1].Value = "TOTALES";
                        worksheet.Cells[row, 1].Style.Font.Bold = true;

                        // Fórmulas para totales
                        worksheet.Cells[row, 4].Formula = $"SUM(D5:D{row - 1})";
                        worksheet.Cells[row, 5].Formula = $"SUM(E5:E{row - 1})";
                        worksheet.Cells[row, 6].Formula = $"SUM(F5:F{row - 1})";

                        // Formato de totales
                        var totalRange = worksheet.Cells[row, 1, row, 6];
                        totalRange.Style.Font.Bold = true;
                        totalRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        totalRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                        worksheet.Cells[row, 5].Style.Numberformat.Format = "₡#,##0.00";
                        worksheet.Cells[row, 6].Style.Numberformat.Format = "₡#,##0.00";

                        // Ajustar ancho de columnas
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        // Agregar bordes a todas las celdas
                        var tableRange = worksheet.Cells[4, 1, row, headers.Length];
                        tableRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        tableRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        tableRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        tableRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        // Guardar el archivo
                        package.Save();
                    }

                    System.Windows.MessageBox.Show("Reporte exportado exitosamente",
                                  "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Opcional: Abrir el archivo automáticamente
                    if (System.Windows.MessageBox.Show("¿Desea abrir el archivo?", "Confirmación",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = saveDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al exportar a Excel: {ex.Message}",
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BuscarVentas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dpFechaInicio.SelectedDate == null || dpFechaFin.SelectedDate == null)
                {
                    System.Windows.MessageBox.Show("Por favor seleccione ambas fechas",
                                  "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime fechaInicio = dpFechaInicio.SelectedDate.Value;
                DateTime fechaFin = dpFechaFin.SelectedDate.Value.AddDays(1).AddSeconds(-1);

                if (fechaInicio > fechaFin)
                {
                    System.Windows.MessageBox.Show("La fecha de inicio debe ser menor a la fecha final",
                                  "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Obtener y mostrar los datos
                var datos = ObtenerDatosVentasPorFecha(fechaInicio, fechaFin);
                dgVentas.ItemsSource = datos;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al buscar ventas: {ex.Message}",
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgVentas.Items.Count == 0)
                {
                    System.Windows.MessageBox.Show("No hay datos para imprimir",
                                  "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Crear el documento para imprimir
                FlowDocument doc = new FlowDocument();
                doc.PagePadding = new Thickness(50);

                // Título
                Paragraph titulo = new Paragraph(new Run("REPORTE DE VENTAS"));
                titulo.TextAlignment = TextAlignment.Center;
                titulo.FontSize = 20;
                titulo.FontWeight = FontWeights.Bold;
                titulo.Margin = new Thickness(0, 0, 0, 10);
                doc.Blocks.Add(titulo);

                // Fechas
                Paragraph fechas = new Paragraph(new Run(
                    $"Desde: {dpFechaInicio.SelectedDate:dd/MM/yyyy} - Hasta: {dpFechaFin.SelectedDate:dd/MM/yyyy}"));
                fechas.TextAlignment = TextAlignment.Center;
                fechas.FontSize = 12;
                fechas.Margin = new Thickness(0, 0, 0, 20);
                doc.Blocks.Add(fechas);

                // Crear tabla
                Table table = new Table();
                table.CellSpacing = 0;
                table.BorderBrush = System.Windows.Media.Brushes.Black; // Especificar explícitamente System.Windows.Media.Brushes
                table.BorderThickness = new Thickness(1);

                // Definir columnas
                for (int i = 0; i < 6; i++)
                {
                    table.Columns.Add(new TableColumn());
                }

                // Estilo para las celdas de encabezado
                Style headerStyle = new Style(typeof(TableCell));
                headerStyle.Setters.Add(new Setter(TableCell.BackgroundProperty, System.Windows.Media.Brushes.LightGray));
                headerStyle.Setters.Add(new Setter(TableCell.FontWeightProperty, FontWeights.Bold));
                headerStyle.Setters.Add(new Setter(TableCell.PaddingProperty, new Thickness(5)));
                headerStyle.Setters.Add(new Setter(TableCell.BorderThicknessProperty, new Thickness(1)));
                headerStyle.Setters.Add(new Setter(TableCell.BorderBrushProperty, System.Windows.Media.Brushes.Black));


                // Estilo para las celdas de datos
                Style cellStyle = new Style(typeof(TableCell));
                cellStyle.Setters.Add(new Setter(TableCell.PaddingProperty, new Thickness(5)));
                cellStyle.Setters.Add(new Setter(TableCell.BorderThicknessProperty, new Thickness(1)));
                cellStyle.Setters.Add(new Setter(TableCell.BorderBrushProperty, System.Windows.Media.Brushes.Black));

                // Agregar encabezados
                TableRow headerRow = new TableRow();
                string[] headers = new[] { "Producto", "Precio Unit.", "Costo Unit.",
                                 "Unid. Vendidas", "Monto Vendido", "Ganancia" };

                foreach (string header in headers)
                {
                    TableCell cell = new TableCell(new Paragraph(new Run(header)));
                    cell.Style = headerStyle;
                    headerRow.Cells.Add(cell);
                }
                table.RowGroups.Add(new TableRowGroup());
                table.RowGroups[0].Rows.Add(headerRow);

                // Agregar datos
                decimal totalMontoVendido = 0;
                decimal totalGanancia = 0;
                int totalUnidades = 0;

                foreach (DatosVenta item in dgVentas.Items)
                {
                    TableRow row = new TableRow();

                    // Agregar celdas
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Producto))) { Style = cellStyle });
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"S/{item.PrecioUnitario:N2}"))) { Style = cellStyle });
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"S/{item.CostoUnitario:N2}"))) { Style = cellStyle });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.UnidadesVendidas.ToString()))) { Style = cellStyle });
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"S/{item.MontoVendido:N2}"))) { Style = cellStyle });
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"S/{item.Ganancia:N2}"))) { Style = cellStyle });

                    table.RowGroups[0].Rows.Add(row);

                    // Actualizar totales
                    totalUnidades += item.UnidadesVendidas;
                    totalMontoVendido += item.MontoVendido;
                    totalGanancia += item.Ganancia;
                }

                // Agregar fila de totales
                TableRow totalRow = new TableRow();
                totalRow.Cells.Add(new TableCell(new Paragraph(new Run("TOTALES"))) { Style = headerStyle });
                totalRow.Cells.Add(new TableCell(new Paragraph(new Run(""))) { Style = headerStyle });
                totalRow.Cells.Add(new TableCell(new Paragraph(new Run(""))) { Style = headerStyle });
                totalRow.Cells.Add(new TableCell(new Paragraph(new Run(totalUnidades.ToString()))) { Style = headerStyle });
                totalRow.Cells.Add(new TableCell(new Paragraph(new Run($"S/{totalMontoVendido:N2}"))) { Style = headerStyle });
                totalRow.Cells.Add(new TableCell(new Paragraph(new Run($"S/{totalGanancia:N2}"))) { Style = headerStyle });
                table.RowGroups[0].Rows.Add(totalRow);

                doc.Blocks.Add(table);

                // Configurar la impresión
                System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    IDocumentPaginatorSource idpSource = doc;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Reporte de Ventas");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al imprimir: {ex.Message}",
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}

