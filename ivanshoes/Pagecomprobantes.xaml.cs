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
using CapaDatos;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para Pagecomprobantes.xaml
    /// </summary>
    public partial class Pagecomprobantes : Page
    {
        public ObservableCollection<entBoleta> Boletas { get; set; } = new ObservableCollection<entBoleta>();
        public Pagecomprobantes()
        {
            InitializeComponent();
            dataGridBoletas.ItemsSource = Boletas;
        }


        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string dniTexto = txtDni.Text.Trim();

                if (int.TryParse(dniTexto, out int dni))
                {
                    List<entBoleta> boletas = logBoleta.Instancia.BuscarBoletasPorDni(dni);
                    dataGridBoletas.ItemsSource = boletas;
                }
                else
                {
                    System.Windows.MessageBox.Show("Ingrese un DNI válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al buscar las boletas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnlistar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<entBoleta> boletas = logBoleta.Instancia.ListarTodasLasBoletas();
                dataGridBoletas.ItemsSource = boletas;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al listar las boletas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
