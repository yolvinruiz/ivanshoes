﻿using System;
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
    /// Lógica de interacción para Administrador.xaml
    /// </summary>
    public partial class Administrador : Window
    {
        public Administrador()
        {
            InitializeComponent();
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

        private void btnInventario_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            framecambio.Navigate(new Paginaconfiguracion());
        }

        private void btndashboar_Click(object sender, RoutedEventArgs e)
        {
            framecambio.Navigate(new Dashboard());
        }

        private void btnReportes_Click(object sender, RoutedEventArgs e)
        {
            framecambio.Navigate(new Reporte());
        }

        private void btnIconprobantes_Click(object sender, RoutedEventArgs e)
        {
            framecambio.Navigate(new Pagecomprobantes()); 
        }
    }
}
