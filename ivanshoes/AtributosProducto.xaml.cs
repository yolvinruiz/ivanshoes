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
using producatributos;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para AtributosProducto.xaml
    /// </summary>
    public partial class AtributosProducto : Window
    {
        public AtributosProducto()
        {
            InitializeComponent();
        }

        private void btntipoproducto_Click(object sender, RoutedEventArgs e)
        {
           atripro.Navigate(new producatributos.categoria());
        }

        private void btnmarca_Click(object sender, RoutedEventArgs e)
        {
            atripro.Navigate(new producatributos.Marca());
        }

        private void btntalla_Click(object sender, RoutedEventArgs e)
        {
            atripro.Navigate(new producatributos.Talla());
        }

        private void btncolores_Click(object sender, RoutedEventArgs e)
        {
            atripro.Navigate(new producatributos.Color());
        }

        private void btncategoria_Click(object sender, RoutedEventArgs e)
        {
            atripro.Navigate(new producatributos.categoria());
        }
    }
}
