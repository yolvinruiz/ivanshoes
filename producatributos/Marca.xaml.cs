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

namespace producatributos
{
    /// <summary>
    /// Lógica de interacción para Marca.xaml
    /// </summary>
    public partial class Marca : Page
    {
        public Marca()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                entMarca ca = new entMarca();
                ca.Nombre = txtnombre.Text.Trim();
                logMarca.Instancia.Insertarmarca(ca);
                System.Windows.MessageBox.Show("agregado correctamente.");

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
