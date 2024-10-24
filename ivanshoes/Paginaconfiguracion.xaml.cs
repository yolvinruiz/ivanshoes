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

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para Paginaconfiguracion.xaml
    /// </summary>
    public partial class Paginaconfiguracion : Page
    {
        public Paginaconfiguracion()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClientesVista clientes = new ClientesVista();
            clientes.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Empleados0 empleados0 = new Empleados0();
            empleados0.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Productovista productovista = new Productovista();
            productovista.Show();
        }
    }
}
