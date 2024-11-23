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
    /// Lógica de interacción para Talla.xaml
    /// </summary>
    public partial class Talla : Page
    {
        public Talla()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                entTalla ca = new entTalla();
                ca.Talla = txtnombre.Text.Trim();
                logTalla.Instancia.Insertartalla(ca);
                System.Windows.MessageBox.Show("agregado correctamente.");

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
