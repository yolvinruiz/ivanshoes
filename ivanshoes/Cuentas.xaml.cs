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
    /// Lógica de interacción para Cuentas.xaml
    /// </summary>
    public partial class Cuentas : Window
    {
        public Cuentas()
        {
            InitializeComponent();
            CargarEmpleados();
        }
        private void CargarEmpleados()
        {
            try
            {
                // Asumiendo que tienes un método para obtener empleados
                var empleados = logEmpleado.Instancia.ListarEmpleados()
                    .Where(e => e.Estado == true) // Solo empleados activos
                    .Select(e => $"{e.Nombre} {e.Apellidos}")
                    .ToList();
                cmbEmpleado.ItemsSource = empleados;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar empleados: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CrearCuenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbEmpleado.SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("Debe seleccionar un empleado",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtContraseña.Password))
                {
                    System.Windows.MessageBox.Show("Debe ingresar una contraseña",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtContraseña.Focus();
                    return;
                }

                if (txtContraseña.Password != txtConfirmarContraseña.Password)
                {
                    System.Windows.MessageBox.Show("Las contraseñas no coinciden",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtConfirmarContraseña.Focus();
                    return;
                }

                // Crear la cuenta
                var cuenta = new entCuentas
                {
                    IDempleado = ObtenerIdEmpleado(cmbEmpleado.SelectedItem.ToString()),
                    Contraseña = txtContraseña.Password
                };

                logCuentas.Instancia.CrearCuenta(cuenta);

                System.Windows.MessageBox.Show("Cuenta creada exitosamente",
                    "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al crear la cuenta: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private int ObtenerIdEmpleado(string nombreCompleto)
        {
            // Implementa la lógica para obtener el ID del empleado basado en su nombre completo
            // Esto dependerá de cómo estés manejando los datos en tu aplicación
            return logEmpleado.Instancia.ObtenerIdPorNombre(nombreCompleto);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ModificarContraseña_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbEmpleado.SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("Debe seleccionar un empleado",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtContraseña.Password))
                {
                    System.Windows.MessageBox.Show("Debe ingresar la contraseña actual",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtContraseña.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtConfirmarContraseña.Password))
                {
                    System.Windows.MessageBox.Show("Debe ingresar la nueva contraseña",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtConfirmarContraseña.Focus();
                    return;
                }

                if (txtContraseña.Password == txtConfirmarContraseña.Password)
                {
                    System.Windows.MessageBox.Show("La nueva contraseña debe ser diferente a la actual",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtConfirmarContraseña.Focus();
                    return;
                }

                if (txtConfirmarContraseña.Password.Length < 6)
                {
                    System.Windows.MessageBox.Show("La nueva contraseña debe tener al menos 6 caracteres",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtConfirmarContraseña.Focus();
                    return;
                }

                int idEmpleado = logEmpleado.Instancia.ObtenerIdPorNombre(cmbEmpleado.SelectedItem.ToString());

                logCuentas.Instancia.ModificarContraseña(
                    idEmpleado,
                    txtContraseña.Password,
                    txtConfirmarContraseña.Password
                );

                System.Windows.MessageBox.Show("Contraseña modificada exitosamente",
                    "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al modificar la contraseña: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
