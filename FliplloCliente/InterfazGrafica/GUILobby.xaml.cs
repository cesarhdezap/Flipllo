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
		CallBackDeFlipllo CanalDeFlipllo;
		public GUILobby(Servidor servidor, Sesion sesion, CallBackDeFlipllo callBackDeFlipllo, Sala sala)
		{
			InitializeComponent();
			SesionLocal = sesion;
			Servidor = servidor;
			Sala = sala;
			CanalDeFlipllo = callBackDeFlipllo;
			CanalDeFlipllo.ActualizarSalaEvent += ActualizarSala;
			AsignarSalaAinterfaz();
			VentanaDeChat.AsignarDatos(SesionLocal, servidor, CanalDeFlipllo);

		}

		private void AsignarSalaAinterfaz()
		{
			DataGridJugadores.ItemsSource = Sala.Jugadores;
			LabelNombreDeLobby.Content = Sala.Nombre;
			TextBoxNivelMaximo.Text = Sala.NivelMaximo.ToString();
			TextBoxNivelMinimo.Text = Sala.NivelMinimo.ToString();
		}

		private void ActualizarSala(Sala sala)
		{
			Sala = sala;
		}

		private void ButtonListo_Click(object sender, RoutedEventArgs e)
		{
			Servidor.CanalDelServidor.AlternarListoParaJugar(SesionLocal);
		}

		private void IniciarJuego()
		{
			ObjetoDeInicializacionDeJuego inicializadorDeJuego = new ObjetoDeInicializacionDeJuego(TipoDeJuego.EnRed);
			GUIJuegoLocal juegoLocal = new GUIJuegoLocal(inicializadorDeJuego);
		}
	}
}
