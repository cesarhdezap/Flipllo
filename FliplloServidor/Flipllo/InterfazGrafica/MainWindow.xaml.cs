using ServiciosDeComunicacion.InterfacesDeServicios;
using ServiciosDeComunicacion.ServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Windows;

namespace InterfazGrafica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IControladorDeServiciosDeHost, IControladorDeServiciosDeFlipllo
    {
        private ServiciosDeHost Servidor;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void ButtonIniciarServidor_Click(object sender, RoutedEventArgs e)
        {
            Servidor = new ServiciosDeHost(this,this);
            Servidor.IniciarServidor();
        }

        private void ButtonPausarServidor_Click(object sender, RoutedEventArgs e)
        {
            Servidor.PararServidor();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Servidor.PararServidor();
        }

        public void EstadoDelServidorActualizado(EstadoDelServidor estadoDelServidor)
        {
            if (estadoDelServidor == EstadoDelServidor.Activo)
            {
                LabelEstadoDeServidor.Content = "Servidor corriendo";
            }
            else if (estadoDelServidor == EstadoDelServidor.Inactivo)
            {
                LabelEstadoDeServidor.Content = "Error al iniciar el host";
            }
            else if (estadoDelServidor == EstadoDelServidor.Detenido)
            {
                LabelEstadoDeServidor.Content = "Servidor abortado";
            }
            else
            {
                LabelEstadoDeServidor.Content = "Error en la configuración de la dirección del servidor.";
                Servidor.PararServidor();
            }
        }

        public void ListaDeSesionesActualizado(List<Sesion> sesiones)
        {
            Dispatcher.BeginInvoke(new ThreadStart(
                () => DataGridUsuariosConectados.ItemsSource = sesiones
                )
            );
        }
    }
}
