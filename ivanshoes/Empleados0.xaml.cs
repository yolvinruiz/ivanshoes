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
    /// Lógica de interacción para Empleados0.xaml
    /// </summary>
    public partial class Empleados0 : Window
    {
        public Empleados0()
        {
            InitializeComponent();
        }
        public void listarEmpleados()
        {
            dgvempleados.ItemsSource = logEmpleado.Instancia.ListarEmpleados();
        }
        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            listarEmpleados();
        }

        private void btnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            EmpleadoRegistro empleadoRegistro = new EmpleadoRegistro();
            empleadoRegistro.Show();
        }
    }
}
