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
using CapaDatos;
using CapaLogica;
using CapaEntidad;
namespace producatributos
{
    /// <summary>
    /// Lógica de interacción para categoria.xaml
    /// </summary>
    public partial class categoria : Page
    {
        public categoria()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            { 

                    entCategoria ca = new entCategoria();
                    ca.Nombre = txtnombre.Text.Trim();
                    logCategoria.Instancia.Insertarcategoria(ca);
                    System.Windows.MessageBox.Show("agregado correctamente.");

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
