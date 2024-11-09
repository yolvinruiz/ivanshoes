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
using System.IO;

namespace ivanshoes
{
    /// <summary>
    /// Lógica de interacción para ClientesVista.xaml
    /// </summary>
    public partial class ClientesVista : Window
    {
        public ClientesVista()
        {
            InitializeComponent();
            listarClientes();
            string fondoGuardado = Properties.Settings.Default.FondoPantalla;
            if (!string.IsNullOrEmpty(fondoGuardado) && File.Exists(fondoGuardado))
            {
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(fondoGuardado)));
                    brush.Stretch = Stretch.UniformToFill;
                    this.Background = brush;
                }
                catch { }
            }
        }
        public void listarClientes()
        {
            dgvcliente.ItemsSource = logCliente.Instancia.Listarcliente();
        }
        private void btnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            Clientes clientes = new Clientes();
            clientes.Show(); 
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            listarClientes();
        }
    }
}
