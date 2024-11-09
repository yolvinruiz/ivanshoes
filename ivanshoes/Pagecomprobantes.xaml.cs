using CapaEntidad;
using CapaLogica;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.IO;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para Pagecomprobantes.xaml
    /// </summary>
    public partial class Pagecomprobantes : Page
    {
        public Pagecomprobantes()
        {
            InitializeComponent();
            InicializarControles();
        }
        private void InicializarControles()
        {
            // Establecer fechas por defecto
            dpFechaInicio.SelectedDate = DateTime.Now.AddDays(-30);
            dpFechaFin.SelectedDate = DateTime.Now;
            cmbTipo.SelectedIndex = 0;

            // Cargar datos iniciales
            CargarComprobantes();
        }
        private void CargarComprobantes()
        {
            try
            {
                var fechaInicio = dpFechaInicio.SelectedDate ?? DateTime.Now.AddDays(-30);
                var fechaFin = dpFechaFin.SelectedDate ?? DateTime.Now;
                string tipoSeleccionado = ((ComboBoxItem)cmbTipo.SelectedItem)?.Content.ToString();

                var comprobantes = logBoleta.Instancia.ListarComprobantes(fechaInicio, fechaFin);

                // Filtrar por tipo si es necesario
                if (tipoSeleccionado != "Todos")
                {
                    string serie = tipoSeleccionado == "Boletas" ? "BB01" : "FF03";
                    comprobantes = comprobantes.Where(c => c.Serie.StartsWith(serie)).ToList();
                }

                // Asignar tipo de comprobante basado en la serie
                foreach (var comp in comprobantes)
                {
                    comp.TipoComprobante = comp.Serie.StartsWith("BB01") ? "Boleta" : "Factura";
                }

                dgComprobantes.ItemsSource = comprobantes;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar comprobantes: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BuscarComprobantes_Click(object sender, RoutedEventArgs e)
        {
            CargarComprobantes();
        }

        private void CmbTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
                CargarComprobantes();
        }


        private void VerTodasBoletas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Deshabilitar filtros
                dpFechaInicio.SelectedDate = null;
                dpFechaFin.SelectedDate = null;
                cmbTipo.SelectedIndex = 0;

                // Obtener todas las boletas
                var boletas = logBoleta.Instancia.ListarTodasBoletas();

                // Asignar tipo de comprobante basado en la serie
                foreach (var boleta in boletas)
                {
                    boleta.TipoComprobante = boleta.Serie?.StartsWith("BB01") == true ? "Boleta" :
                                            boleta.Serie?.StartsWith("FF03") == true ? "Factura" :
                                            "Desconocido";
                }

                // Mostrar en el DataGrid
                dgComprobantes.ItemsSource = boletas;

                // Mostrar conteo
                System.Windows.MessageBox.Show($"Se encontraron {boletas.Count} comprobantes",
                              "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar las boletas: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
