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
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ivanshoes
{
    public partial class EmpleadoRegistro : Window
    {
        public EmpleadoRegistro()
        {
            InitializeComponent();
            cargarcargos();
        }
            public string idempleado = "";
        

        public void cargarcargos()
        {
            try
            {
                txtcargoempleado.ItemsSource = logCargo.Instancia.ListarCargo();
                txtcargoempleado.DisplayMemberPath = "Nombre";
                //txtcargoempleado.ValueMember = "id_Cargo";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al cargar los cargos: " + ex.Message);
            }
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string dni = txtdniempleado.Text.Trim();
            api apiCliente = new api();

            try
            {
                api cliente = await apiCliente.ObtenerDatosClienteAsync(dni);
                txtnombreempleado.Text = cliente.Nombre;
                txtapellidoempleado.Text = $"{cliente.Apellidos}";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                entEmpleado emp = new entEmpleado();
                emp.Nombre = txtnombreempleado.Text.Trim();
                emp.Apellidos = txtapellidoempleado.Text.Trim();
                emp.DNI = Convert.ToInt32(txtdniempleado.Text.Trim());
                emp.Telefono = Convert.ToInt32(txttelefonoempleado.Text.Trim());
                emp.correo = txtcorreoempleado.Text.Trim();
                emp.NombreCargo = txtcargoempleado.Text.Trim();
                emp.Estado = cbestadoempleado.IsChecked == true;

                logEmpleado.Instancia.InsertarEmpleado(emp);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al agregar el empleado: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                entEmpleado emp = new entEmpleado();
                emp.ID_Empleado = Convert.ToInt32(idempleado);
                emp.Nombre = txtnombreempleado.Text.Trim();
                emp.Apellidos = txtapellidoempleado.Text.Trim();
                emp.DNI = Convert.ToInt32(txtdniempleado.Text.Trim());
                emp.Telefono = Convert.ToInt32(txttelefonoempleado.Text.Trim());
                emp.NombreCargo = (string)txtcargoempleado.SelectedItem;
                emp.Estado = cbestadoempleado.IsChecked == true;

                logEmpleado.Instancia.ModificarEmpleado(emp);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al modificar el empleado: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

