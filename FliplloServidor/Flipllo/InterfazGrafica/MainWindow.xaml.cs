using ServiciosDeComunicacion.Clases;
using ServiciosDeComunicacion.Interfaces.Controladores;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace InterfazGrafica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IControladorDeActualizacionDePantalla
    {
        private AdministradorDeHostDeServicios AdministradorDeHostDeServicios;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void ButtonIniciarServidor_Click(object sender, RoutedEventArgs e)
        {
            AdministradorDeHostDeServicios = new AdministradorDeHostDeServicios(this);
            AdministradorDeHostDeServicios.IniciarServicios();
            
        }

        private void ButtonPausarServidor_Click(object sender, RoutedEventArgs e)
        {
            AdministradorDeHostDeServicios.PararServicios();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (AdministradorDeHostDeServicios != null)
            {
                AdministradorDeHostDeServicios.PararServicios();
            }
        }

        public void EstadoDelServidorActualizado(EstadoDelServidor estadoDelServidor, string mensaje = null)
        {
            LabelEstadoDeServidor.Content = estadoDelServidor.ToString() + System.Environment.NewLine;
            if (mensaje != null)
            {
                LabelEstadoDeServidor.Content += mensaje;
            }
        }

        public void ListaDeSesionesActualizado(List<Sesion> sesiones)
        {
            Dispatcher.Invoke(
                () => {
                    DataGridUsuariosConectados.ItemsSource = null;
                    DataGridUsuariosConectados.ItemsSource = sesiones;
                    DataGridUsuariosConectados.Items.Refresh();
                });
        }

        public void ListaDeSalasActualizado(List<Sala> salas)
        {
            Dispatcher.BeginInvoke(new ThreadStart(
                () => DataGridSalasConectadas.ItemsSource = null
                )
            );

            Dispatcher.BeginInvoke(new ThreadStart(
                () => DataGridSalasConectadas.ItemsSource = salas
                )
            );
        }

        private void ButtonLimpiarSesiones_Click(object sender, RoutedEventArgs e)
        {
            AdministradorDeHostDeServicios.LimpiarSesiones();
        }

        private void ButtonLimpiarSalas_Click(object sender, RoutedEventArgs e)
        {
            AdministradorDeHostDeServicios.LimpiarSalas();
        }
    }
}
