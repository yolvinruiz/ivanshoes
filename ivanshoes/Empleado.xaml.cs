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
