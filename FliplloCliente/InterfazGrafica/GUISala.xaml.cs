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
using static InterfazGrafica.ManejadorDeExcepciones.ManejadorDeExcepcionesDeComunicacion;
using LogicaDeNegocios;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using static LogicaDeNegocios.Servicios.ServiciosDeLogicaDeJuego;
using InterfazGrafica.ManejadorDeExcepciones;

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
		private string skinLocal = "Default";
		private string skinDelOponente = "Default";
		private LogicaDeNegocios.ColorDeFicha ColorDeFichaLocal = LogicaDeNegocios.ColorDeFicha.Negro;
		private CallBackDeFlipllo CanalDeFlipllo;
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
			CanalDeFlipllo.CambiarSkinEvent += CambiarSkinDelOponente;
			CanalDeFlipllo.RecibirMensajeEvent += RecibirMensaje;
			AñadirFichasDeTableroInicial();
			TableroBlanco.ItemsSource = FichasDeVistaPrevia;
			TableroNegro.ItemsSource = FichasDeVistaPrevia;
			SkinsDisponibles = ListarSkins();
			AsignarSalaAinterfaz();
			AsignarMiColor();
			VentanaDeChat.AsignarDatos(SesionLocal, Servidor, CanalDeFlipllo);
			AñadirFichasDeTableroInicial();
		}
			
		private void RecibirMensaje(Mensaje mensaje)
		{
			VentanaDeChat.RecibirMensaje(mensaje);
		}

		private void CambiarSkinDelOponente(string skin)
		{
			if (SkinsDisponibles.Contains(skin))
			{
				CambiarSkin(skin, ColorContrario(ColorDeFichaLocal));
				skinDelOponente = skin;
			}
		}

		private void AsignarMiColor()
		{
			foreach (Jugador jugador in Sala.Jugadores)
			{
				if (!string.IsNullOrEmpty(jugador.Sesion.ID))
				{
					ColorDeFichaLocal = (LogicaDeNegocios.ColorDeFicha)(int)jugador.Color;
				}
			}	
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
				ButtonIniciarJuego.IsEnabled = true;
			}
			else
			{
				TextBoxNivelMaximo.IsEnabled = false;
				ButtonIniciarJuego.IsEnabled = false;
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
			try
			{
				Servidor.CanalDelServidor.AlternarListoParaJugar(SesionLocal);
			}
			catch (Exception ex)
			{
				MensajeDeError mensajeDeError = ManejarExcepcion(ex);
				mensajeDeError.Mostrar();
			}
		}

		private void IniciarJuego()
		{
			ObjetoDeInicializacionDeJuego inicializadorDeJuego;
			

			if (ColorDeFichaLocal == LogicaDeNegocios.ColorDeFicha.Negro)
			{
				inicializadorDeJuego = new ObjetoDeInicializacionDeJuego(TipoDeJuego.EnRed, skinLocal, skinDelOponente, Sala.Jugadores.ToList());
			}
			else if (ColorDeFichaLocal == LogicaDeNegocios.ColorDeFicha.Blanco)
			{
				inicializadorDeJuego = new ObjetoDeInicializacionDeJuego(TipoDeJuego.EnRed, skinDelOponente, skinLocal, Sala.Jugadores.ToList());
			}
			else
			{
				inicializadorDeJuego = new ObjetoDeInicializacionDeJuego(TipoDeJuego.EnRed);
			}

			GUIJuego juegoLocal = new GUIJuego(inicializadorDeJuego, SesionLocal, CanalDeFlipllo, Servidor);
			Hide();
			juegoLocal.ShowDialog();
			ShowDialog();
		}

		private void ButtonRightArrow_Click(object sender, RoutedEventArgs e)
		{
			if (SkinsDisponibles.Count() > 1)
			{
				IndiceDeSkinSeleccionada = (IndiceDeSkinSeleccionada + 1) % SkinsDisponibles.Count();
				string skinSeleccionada = SkinsDisponibles.ElementAt(IndiceDeSkinSeleccionada);
				CambiarVistaPrevia(skinSeleccionada);
				CambiarSkin(skinSeleccionada, (LogicaDeNegocios.ColorDeFicha)(int)ColorDeFichaLocal);
				Servidor.CanalDelServidor.CambiarSkinDeFicha(SesionLocal, skinSeleccionada);
				skinLocal = skinSeleccionada;
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
				IndiceDeSkinSeleccionada = (IndiceDeSkinSeleccionada - 1) % SkinsDisponibles.Count();
				if (IndiceDeSkinSeleccionada <= -1)
				{
					IndiceDeSkinSeleccionada = SkinsDisponibles.Count()-1;
				}

				string skinSeleccionada = SkinsDisponibles.ElementAt(IndiceDeSkinSeleccionada);
				CambiarVistaPrevia(skinSeleccionada);
				CambiarSkin(skinSeleccionada, (LogicaDeNegocios.ColorDeFicha)(int)ColorDeFichaLocal);
				skinLocal = skinSeleccionada;
				Servidor.CanalDelServidor.CambiarSkinDeFicha(SesionLocal, skinSeleccionada);
				RecargarAnimaciones();
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			Servidor.CanalDelServidor.DesconectarDeSala(SesionLocal);
		}

		private void ButtonIniciarJuego_Click(object sender, RoutedEventArgs e)
		{
			Servidor.CanalDelServidor.IniciarJuego(SesionLocal);
		}
	}
}
