﻿using CapaEntidad;
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
    /// Lógica de interacción para Color.xaml
    /// </summary>
    public partial class Color : Page
    {
        public Color()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                entColor ca = new entColor();
                ca.Nombre = txtnombre1.Text.Trim();
                logColor.Instancia.Insertarcolor(ca);
                System.Windows.MessageBox.Show("agregado correctamente.");

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
