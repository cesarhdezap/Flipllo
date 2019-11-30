using LogicaDeNegocios.ClasesDeDominio;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion;
using ServiciosDeComunicacion.Proxy;
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

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for GUILobby.xaml
	/// </summary>
	public partial class GUILobby : Window
	{
		private Servidor Servidor;
		private Sesion SesionLocal;
		private Sala Sala;
		public GUILobby(Servidor servidor, Sesion sesion, Sala sala)
		{
			InitializeComponent();
			SesionLocal = sesion;
			Servidor = servidor;
			Sala = sala;
			AsignarSalaAinterfaz();

		}

		private void AsignarSalaAinterfaz()
		{
			DataGridJugadores.ItemsSource = Sala.Jugadores;
			LabelNombreDeLobby.Content = Sala.Nombre;
			TextBoxNivelMaximo.Text = Sala.NivelMaximo.ToString();
			TextBoxNivelMinimo.Text = Sala.NivelMinimo.ToString();
		}

		private void ButtonListo_Click(object sender, RoutedEventArgs e)
		{
			//Servidor.CanalDelServidor.Alternar(SesionLocal);
		}

		private void IniciarJuego()
		{
			GUIJuegoLocal juegoLocal = new GUIJuegoLocal(TipoDeJuego.EnRed);

		}
	}
}
