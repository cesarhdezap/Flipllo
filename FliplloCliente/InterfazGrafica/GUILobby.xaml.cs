using LogicaDeNegocios.ClasesDeDominio;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion;
using ServiciosDeComunicacion.Proxy;
using static LogicaDeNegocios.ServiciosDeRecursos;
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
using LogicaDeNegocios;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

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
		private int IndiceDeSkinSeleccionada = 0;
		private List<string> SkinsDisponibles;
		private ObservableCollection<Ficha> FichasDeVistaPrevia = new ObservableCollection<Ficha>();
		public GUILobby(Servidor servidor, Sesion sesion, CallBackDeFlipllo callBackDeFlipllo, Sala sala)
		{
			InitializeComponent();
			SesionLocal = sesion;
			Servidor = servidor;
			Sala = sala;
			CanalDeFlipllo = callBackDeFlipllo;
			CanalDeFlipllo.ActualizarSalaEvent += ActualizarSala;
			CanalDeFlipllo.JuegoIniciadoEvent += IniciarJuego;
			TableroBlanco.ItemsSource = FichasDeVistaPrevia;
			
			SkinsDisponibles = ListarSkins();
			AñadirFichasDeTableroInicial();
			AsignarSalaAinterfaz();
			VentanaDeChat.AsignarDatos(SesionLocal, servidor, CanalDeFlipllo);

		}

		public GUILobby()
		{
			InitializeComponent();
			SkinsDisponibles = ListarSkins();
			AñadirFichasDeTableroInicial();
			TableroBlanco.ItemsSource = FichasDeVistaPrevia;
			TableroNegro.ItemsSource = FichasDeVistaPrevia;
		}

		private void AñadirFichasDeTableroInicial()
		{
			FichasDeVistaPrevia = new ObservableCollection<Ficha>
			{
				new Ficha
				{
					ColorActual = LogicaDeNegocios.ColorDeFicha.Blanco,
					Posicion = new Point(4,3)
				},

				new Ficha
				{
					ColorActual = LogicaDeNegocios.ColorDeFicha.Negro,
					Posicion = new Point(4,4)
				},

				new Ficha
				{
					ColorActual = LogicaDeNegocios.ColorDeFicha.Negro,
					Posicion = new Point(3,3)
				},

				new Ficha
				{
					ColorActual = LogicaDeNegocios.ColorDeFicha.Blanco,
					Posicion = new Point(3,4)
				}
			};
		}

		private void AsignarSalaAinterfaz()
		{
			DataGridJugadores.ItemsSource = Sala.Jugadores;
			LabelNombreDeLobby.Content = Sala.Nombre;
			TextBoxNivelMaximo.Text = Sala.NivelMaximo.ToString();
			TextBoxNivelMinimo.Text = Sala.NivelMinimo.ToString();

			if(Sala.NombreDeUsuarioCreador == SesionLocal.Usuario.NombreDeUsuario)
			{
				TextBoxNivelMaximo.IsEnabled = true;
				TextBoxNivelMinimo.IsEnabled = true;
			}
			else
			{
				TextBoxNivelMaximo.IsEnabled = false;
				TextBoxNivelMinimo.IsEnabled = false;
			}
		}

		private void ActualizarSala(Sala sala)
		{
			Sala = sala;
			AsignarSalaAinterfaz();
		}

		private void ButtonListo_Click(object sender, RoutedEventArgs e)
		{
			Servidor.CanalDelServidor.AlternarListoParaJugar(SesionLocal);
		}

		private void IniciarJuego()
		{
			ObjetoDeInicializacionDeJuego inicializadorDeJuego = new ObjetoDeInicializacionDeJuego(TipoDeJuego.EnRed);
			GUIJuegoLocal juegoLocal = new GUIJuegoLocal(inicializadorDeJuego);
			Hide();
			juegoLocal.ShowDialog();
			Show();
		}

		private void ButtonRightArrow_Click(object sender, RoutedEventArgs e)
		{
			if (SkinsDisponibles.Count() > 1)
			{
				IndiceDeSkinSeleccionada = (IndiceDeSkinSeleccionada + 1) % SkinsDisponibles.Count();
				string skinSeleccionada = SkinsDisponibles.ElementAt(IndiceDeSkinSeleccionada);
				CambiarVistaPrevia(skinSeleccionada);
				CambiarSkin(skinSeleccionada, LogicaDeNegocios.ColorDeFicha.Negro);
				RecargarAnimaciones();
			}
		}

		private void RecargarAnimaciones()
		{
			ImageVistaPreviaBlanca.BeginStoryboard((Storyboard)Application.Current.Resources["vistaPreviaBlancoStoryboard"]);
			ImageVistaPreviaNegra.BeginStoryboard((Storyboard)Application.Current.Resources["vistaPreviaNegroStoryboard"]);
		}

		private void ButtonLeftArrow_Click(object sender, RoutedEventArgs e)
		{
			if (SkinsDisponibles.Count() > 1)
			{
				if (IndiceDeSkinSeleccionada == -1)
				{
					IndiceDeSkinSeleccionada = SkinsDisponibles.Count()-1;
				}

				string skinSeleccionada = SkinsDisponibles.ElementAt(IndiceDeSkinSeleccionada);
				IndiceDeSkinSeleccionada = (IndiceDeSkinSeleccionada - 1) % SkinsDisponibles.Count();
				CambiarVistaPrevia(skinSeleccionada);
				CambiarSkin(skinSeleccionada, LogicaDeNegocios.ColorDeFicha.Negro);
				RecargarAnimaciones();
			}
		}
	}
}
