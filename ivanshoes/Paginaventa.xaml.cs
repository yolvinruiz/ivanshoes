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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para Paginaventa.xaml
    /// </summary>
    public partial class Paginaventa : Page
    {
        public Paginaventa()
        {
            InitializeComponent();
        }


        private void btnmostrarproducto_Click(object sender, RoutedEventArgs e)
        {
            string termino = "";
            List<entProducto> productos = logProducto.Instancia.BuscarProductoConNombres(termino);
            dgvproducto.ItemsSource = productos;
            foreach (var column in dgvproducto.Columns)
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

        private void numericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (numericUpDown != null && numericUpDown.Value != null)
            {
                double precio = 0;
                double.TryParse(txtprecpro.Text.Trim(), out precio);
                // Obtener la cantidad seleccionada en el IntegerUpDown
                int cantidad = numericUpDown.Value ?? 1; // Si el valor es nulo, usa 1 como valor predeterminado

                // Calcular el subtotal
                double subtotal = cantidad * precio;

                // Mostrar el subtotal en el TextBlock con dos decimales
                txtsubtotal.Text = $"Subtotal: {subtotal:F2}";
            }
        }

        private void dgvgenerarorden_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgvproducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Asegurarse de que hay un elemento seleccionado
            if (dgvproducto.SelectedItem != null)
            {
                // Obtener la fila seleccionada como un objeto dinámico
                dynamic row = dgvproducto.SelectedItem;

                // Asignar los valores a los TextBox
                txtnompro.Text = row.NombreTipoProducto.ToString();
                txtprecpro.Text = Convert.ToDouble(row.precio).ToString("F2");
            }
        }
    }
}
