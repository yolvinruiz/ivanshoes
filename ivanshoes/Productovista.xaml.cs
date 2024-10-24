using CapaDatos;
using CapaEntidad;
using CapaLogica;
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
    /// Lógica de interacción para Productovista.xaml
    /// </summary>
    public partial class Productovista : Window
    {
        public Productovista()
        {
            InitializeComponent();
            listarproducto();
        }
        public void listarproducto()
        {
            string termino = "";
            List<entProducto> productos = logProducto.Instancia.BuscarProductoConNombres(termino);
            dgvProductos.ItemsSource = productos;
            foreach (var column in dgvProductos.Columns)
            {
                if (column.Header.ToString() == "id_tipo_producto" ||
                    column.Header.ToString() == "id_marca" ||
                    column.Header.ToString() == "id_color" ||
                    column.Header.ToString() == "id_categoria" ||
                    column.Header.ToString() == "id_talla")
                {
                    column.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            listarproducto();
        }

        private void btnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnatributosproduc_Click(object sender, RoutedEventArgs e)
        {
            AtributosProducto atributosProducto = new AtributosProducto();
            atributosProducto.Show();
        }
    }
}
