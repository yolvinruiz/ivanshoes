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
    /// Lógica de interacción para tipoproducto.xaml
    /// </summary>
    public partial class tipoproducto : Page
    {
        public tipoproducto()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                entTipo_producto ca = new entTipo_producto();
                ca.Nombre = txtnombre.Text.Trim();
                ca.Descripcion = txtdescripcion.Text.Trim(); 
                logTipo_producto.Instancia.Insertartipoproducto(ca);
                System.Windows.MessageBox.Show("agregado correctamente.");

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
