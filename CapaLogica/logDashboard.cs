using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using ClosedXML.Excel;

namespace CapaLogica
{
    public class logDashboard
    {
        #region singleton
        private static readonly logDashboard _instancia = new logDashboard();
        public static logDashboard Instancia
        {
            get { return logDashboard._instancia; }
        }
        #endregion

        public EstadisticasVenta ObtenerEstadisticas(DateTime fechaInicio, DateTime fechaFin)
        {
            return datDashboard.Instancia.ObtenerEstadisticas(fechaInicio, fechaFin);
        }

        public List<VentaDiaria> ObtenerVentasDiarias(DateTime fechaInicio, DateTime fechaFin)
        {
            return datDashboard.Instancia.ObtenerVentasDiarias(fechaInicio, fechaFin);
        }

        public List<TopProducto> ObtenerTopProductos(DateTime fechaInicio, DateTime fechaFin, int cantidad = 10)
        {
            return datDashboard.Instancia.ObtenerTopProductos(fechaInicio, fechaFin, cantidad);
        }

        public (DateTime inicio, DateTime fin, int meses) ObtenerPeriodo(string tipoPeriodo)
        {
            DateTime fin = DateTime.Now;
            DateTime inicio;
            int meses;

            switch (tipoPeriodo)
            {
                case "Este Mes":
                    inicio = new DateTime(fin.Year, fin.Month, 1);
                    meses = 1;
                    break;
                case "Último Trimestre":
                    inicio = fin.AddMonths(-3);
                    meses = 3;
                    break;
                case "Este Año":
                    inicio = new DateTime(fin.Year, 1, 1);
                    meses = 12;
                    break;
                default:
                    inicio = fin.AddDays(-30);
                    meses = 1;
                    break;
            }

            return (inicio, fin, meses);
        }
        public List<VentaCategoria> ObtenerVentasCategoria()
        {
            return datDashboard.Instancia.ObtenerVentasCategoria();
        }
        public List<EstadisticaProducto> ObtenerEstadisticasProductos(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                // Validaciones básicas
                if (fechaInicio > fechaFin)
                {
                    throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha fin");
                }

                return datDashboard.Instancia.ObtenerEstadisticasProductos(fechaInicio, fechaFin);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public (decimal TotalVendido, decimal GananciaTotal) ObtenerResumenTotal(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                if (fechaInicio > fechaFin)
                {
                    throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha fin");
                }

                return datDashboard.Instancia.ObtenerResumenTotal(fechaInicio, fechaFin);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Método auxiliar para calcular totales de una lista de estadísticas
        public (decimal TotalVendido, decimal GananciaTotal) CalcularTotales(List<EstadisticaProducto> estadisticas)
        {
            decimal totalVendido = estadisticas.Sum(e => e.MontoVendido);
            decimal gananciaTotal = estadisticas.Sum(e => e.Ganancia);
            return (totalVendido, gananciaTotal);
        }

        // Método para exportar a Excel (opcional, si quieres mover la lógica de exportación aquí)
        public void ExportarAExcel(List<EstadisticaProducto> estadisticas, string rutaArchivo)
        {
            try
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
                    int row = 2;
                    foreach (var item in estadisticas)
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

                    // Totales
                    worksheet.Cell(row, 1).Value = "TOTALES";
                    worksheet.Cell(row, 5).FormulaA1 = $"SUM(E2:E{row - 1})";
                    worksheet.Cell(row, 6).FormulaA1 = $"SUM(F2:F{row - 1})";

                    workbook.SaveAs(rutaArchivo);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
