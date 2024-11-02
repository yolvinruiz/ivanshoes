using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Timers;
using CapaLogica;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace ivanshoes
{
    public partial class Dashboard : Page
    {
        private System.Timers.Timer actualizacionTimer;
        private readonly SolidColorBrush colorPositivo = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#4CAF50"));
        private readonly SolidColorBrush colorNegativo = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#F44336"));

        public Dashboard()
        {
            InitializeComponent();
            InicializarDashboard();
        }

        private void InicializarDashboard()
        {
            CargarCombos();
            ActualizarDashboard();
        }

        private void CargarCombos()
        {
            var añoActual = DateTime.Now.Year;
            var años = Enumerable.Range(añoActual - 4, 5).Reverse();
            cmbAño.ItemsSource = años;
            cmbAño.SelectedItem = añoActual;

            cmbPeriodo.SelectedIndex = 0;
        }
        private void ActualizarDashboard()
        {
            try
            {
                var periodoSeleccionado = ((ComboBoxItem)cmbPeriodo.SelectedItem)?.Content.ToString() ?? "Este Mes";
                var periodo = logDashboard.Instancia.ObtenerPeriodo(periodoSeleccionado);
                ActualizarKPIs(periodo.inicio, periodo.fin);
                ActualizarGraficos(periodo.inicio, periodo.fin);
                ActualizarTopProductos(periodo.inicio, periodo.fin);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al actualizar dashboard:" + ex);
            }
        }

        private void ActualizarKPIs(DateTime fechaInicio, DateTime fechaFin)
        {
            var estadisticas = logDashboard.Instancia.ObtenerEstadisticas(fechaInicio, fechaFin);
            var estadisticasAnteriores = logDashboard.Instancia.ObtenerEstadisticas(
                fechaInicio.AddMonths(-1),
                fechaFin.AddMonths(-1));

            ActualizarKPI(txtVentasTotales, txtPorcentajeVentas,
                estadisticas.VentasTotales, estadisticasAnteriores.VentasTotales, "C");

            ActualizarKPI(txtNumeroVentas, txtPorcentajeNumVentas,
                estadisticas.NumeroVentas, estadisticasAnteriores.NumeroVentas);

            ActualizarKPI(txtTicketPromedio, txtPorcentajeTicket,
                estadisticas.TicketPromedio, estadisticasAnteriores.TicketPromedio, "C");

            ActualizarKPI(txtProductosVendidos, txtPorcentajeProductos,
                estadisticas.ProductosVendidos, estadisticasAnteriores.ProductosVendidos);
        }

        private void ActualizarKPI(TextBlock valorTextBlock, TextBlock porcentajeTextBlock,
            decimal valorActual, decimal valorAnterior, string formato = "N0")
        {
            valorTextBlock.Text = valorActual.ToString(formato);

            if (valorAnterior != 0)
            {
                var porcentajeCambio = ((valorActual - valorAnterior) / valorAnterior) * 100;
                var signo = porcentajeCambio >= 0 ? "+" : "";
                porcentajeTextBlock.Text = $"{signo}{porcentajeCambio:N1}%";
                porcentajeTextBlock.Foreground = porcentajeCambio >= 0 ? colorPositivo : colorNegativo;
            }
            else
            {
                porcentajeTextBlock.Text = "N/A";
                porcentajeTextBlock.Foreground = colorPositivo;
            }
        }

        private void ActualizarGraficos(DateTime fechaInicio, DateTime fechaFin)
        {
            ActualizarGraficoVentasMensuales();
            ActualizarGraficoComparativo();
            ActualizarGraficoCategoriasVentas();
        }
        private void ActualizarGraficoVentasMensuales()
        {
            var año = (int)cmbAño.SelectedItem;
            var ventasMensuales = logDashboard.Instancia.ObtenerVentasDiarias(
                new DateTime(año, 1, 1),
                new DateTime(año, 12, 31));

            // Crear una lista con todos los meses del año
            var todosLosMeses = Enumerable.Range(1, 12).Select(mes => new
            {
                Mes = mes,
                Total = 0m
            }).ToDictionary(x => x.Mes, x => x.Total);

            // Actualizar con los datos reales
            var ventasPorMes = ventasMensuales
                .GroupBy(v => v.Fecha.Month)
                .ToDictionary(g => g.Key, g => g.Sum(v => v.MontoTotal));

            // Combinar los datos reales con los meses vacíos
            foreach (var venta in ventasPorMes)
            {
                todosLosMeses[venta.Key] = venta.Value;
            }

            var valores = new ChartValues<decimal>();
            var etiquetas = new List<string>();

            // Usar los meses ordenados
            foreach (var mes in todosLosMeses.OrderBy(x => x.Key))
            {
                valores.Add(mes.Value);
                etiquetas.Add(System.Globalization.CultureInfo.CurrentCulture
                    .DateTimeFormat.GetMonthName(mes.Key));
            }

            chartVentasMensuales.Series = new SeriesCollection
    {
        new ColumnSeries
        {
            Title = "Ventas",
            Values = valores,
            Fill = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2196F3"))
        }
    };

            chartVentasMensuales.AxisX[0].Labels = etiquetas;
        }

        private void ActualizarGraficoComparativo()
        {
            var año = (int)cmbAño.SelectedItem;
            var ventasActuales = logDashboard.Instancia.ObtenerVentasDiarias(
                new DateTime(año, 1, 1),
                new DateTime(año, 12, 31));

            var ventasAnteriores = logDashboard.Instancia.ObtenerVentasDiarias(
                new DateTime(año - 1, 1, 1),
                new DateTime(año - 1, 12, 31));

            chartComparativaVentas.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = año.ToString(),
                    Values = new ChartValues<decimal>(ventasActuales.Select(v => v.MontoTotal)),
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent
                },
                new LineSeries
                {
                    Title = (año - 1).ToString(),
                    Values = new ChartValues<decimal>(ventasAnteriores.Select(v => v.MontoTotal)),
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent
                }
            };
        }

        private void ActualizarGraficoCategoriasVentas()
        {
            var ventasPorCategoria = logDashboard.Instancia.ObtenerVentasCategoria();
            var series = new SeriesCollection();

            foreach (var venta in ventasPorCategoria)
            {
                series.Add(new PieSeries
                {
                    Title = venta.Categoria,
                    Values = new ChartValues<decimal> { venta.MontoTotal },
                    DataLabels = true,
                    LabelPoint = point => $"{venta.Categoria}: {point.Y:C}"
                });
            }

            chartCategoriasVentas.Series = series;
        }

        private void ActualizarTopProductos(DateTime fechaInicio, DateTime fechaFin)
        {
            var topProductos = logDashboard.Instancia.ObtenerTopProductos(fechaInicio, fechaFin, 10);
            dgTopProductos.ItemsSource = topProductos;
        }

        private void cmbAño_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded && cmbAño.SelectedItem != null)
            {
                ActualizarDashboard();
            }
        }

        private void cmbPeriodo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded && cmbPeriodo.SelectedItem != null)
            {
                ActualizarDashboard();
            }
        }

        public void Dispose()
        {
            if (actualizacionTimer != null)
            {
                actualizacionTimer.Stop();
                actualizacionTimer.Dispose();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InicializarDashboard();
        }
    }
}