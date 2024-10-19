using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ivanshoes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnempleado_Click(object sender, RoutedEventArgs e)
        {
            Empleado emp = new Empleado();
            this.Hide();
            emp.ShowDialog();
            this.Show();
        }

        private void btnempleado_Copiar_Click(object sender, RoutedEventArgs e)
        {
            Login emp = new Login();
            this.Hide();
            emp.ShowDialog();
            this.Show();
        }
    }
}