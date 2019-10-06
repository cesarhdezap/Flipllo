using LogicaDeNegocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
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

namespace InterfazGrafica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiciosDeHost Servidor;
        public MainWindow()
        {
            InitializeComponent();
            
        }
        
        private void ButtonIniciarServidor_Click(object sender, RoutedEventArgs e)
        {
            Servidor = new ServiciosDeHost();
            Servidor.IniciarServidor();
            Thread.Sleep(1000);
            if (Servidor.ServidorActivo)
            {
                LabelEstadoDeServidor.Content = "Servidor corriendo";
            }
            else
            {
                LabelEstadoDeServidor.Content = "Error";
            }
        }

        private void ButtonPausarServidor_Click(object sender, RoutedEventArgs e)
        {
            Servidor.PararHost();
            LabelEstadoDeServidor.Content = "Servidor abortado";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Servidor.PararHost();
        }
    }
}
