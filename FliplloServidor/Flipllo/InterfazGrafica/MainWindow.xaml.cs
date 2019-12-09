using ServiciosDeComunicacion.Clases;
using ServiciosDeComunicacion.Interfaces;
using ServiciosDeComunicacion.Interfaces.Controladores;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            AdministradorDeHostDeServicios = new AdministradorDeHostDeServicios(this);
        }

        private void ButtonIniciarServidor_Click(object sender, RoutedEventArgs e)
        {
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

        public void EstadoDelServidorActualizado(string nombre, EstadoDelServidor estadoDelServidor, string mensaje = null)
        {
            TextBlockEstadoDelServidor.Text += "Nombre: " + nombre + Environment.NewLine + 
                "Estado: " + estadoDelServidor.ToString() + Environment.NewLine;
            if (mensaje != null)
            {
                TextBlockEstadoDelServidor.Text += mensaje + Environment.NewLine + Environment.NewLine;
            }
        }


        public void ListaDeSesionesActualizado(List<Sesion> sesiones)
        {
            Dispatcher.Invoke(
                () => {
                    ObservableCollection<Sesion> listaSesiones = new ObservableCollection<Sesion>();
                    foreach (Sesion sesion in sesiones)
                    {
                        listaSesiones.Add(sesion);
                    }
                    DataGridUsuariosConectados.ItemsSource = null;
                    DataGridUsuariosConectados.ItemsSource = listaSesiones;
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
