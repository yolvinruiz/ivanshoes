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
    /// Lógica de interacción para Empleado.xaml
    /// </summary>
    public partial class Empleado : Window
    {
        public Empleado(string nombre,string apellidos,int dnie)
        {
            InitializeComponent();
            MainFrame.Navigate(new PageVentas(dnie.ToString()));
            txtnombreempleado.Text = "Usuario: "+ nombre +" "+ apellidos;
            txtdniempleadoo.Text = "DNI: " + dnie.ToString();
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
        public void LimpiarDataGridEnPaginaObjetivo()
        {
            // Asegúrate de que el Frame existe y contiene la página objetivo

        }


    }
}
