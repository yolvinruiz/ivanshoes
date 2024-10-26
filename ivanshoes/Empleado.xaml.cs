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
using System.Windows.Shapes;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para Empleado.xaml
    /// </summary>
    public partial class Empleado : Window
    {
        public Empleado()
        {
            InitializeComponent();
        }
        public void LimpiarDataGridEnPaginaObjetivo()
        {
            // Asegúrate de que el Frame existe y contiene la página objetivo
            if (MainFrame.Content is Paginaventa paginaObjetivo)
            {
                // Si ya está en PaginaObjetivo, llama al método de limpieza
                paginaObjetivo.LimpiarDataGrid();
            }
            else
            {
                // Si no está en PaginaObjetivo, navega a ella y luego limpia el DataGrid
                paginaObjetivo = new Paginaventa();
                MainFrame.Navigate(paginaObjetivo);
                paginaObjetivo.LimpiarDataGrid();
            }
        }

        private void btnempleado_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnVenta_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Paginaventa());
        }

        private void btnPedidos_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new paginaverpedidos());
        }
    }
}
