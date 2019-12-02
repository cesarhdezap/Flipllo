using LogicaDeNegocios;
using LogicaDeNegocios.ClasesDeDominio;
using LogicaDeNegocios.ServiciosDeFlipllo;
using LogicaDeNegocios.ServiciosDeJuego;
using ServiciosDeComunicacion;
using ServiciosDeComunicacion.Proxy;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static InterfazGrafica.Utilierias.UtilieriasDeElementosGraficos;
using static LogicaDeNegocios.InteligenciaArtificial.InteligenciaArtificial;
using static LogicaDeNegocios.ServiciosDeRecursos;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for GUIJuegoLocal.xaml
	/// </summary>
	public partial class GUIJuegoLocal : Window, IServiciosDeJuegoCallback
	{
		private ObservableCollection<LogicaDeNegocios.Ficha> Fichas;
		private ObservableCollection<LogicaDeNegocios.Ficha> FichasDeVistaPrevia;
		private readonly int TAMAÑO_DE_TABLERO = 8;
		private Juego Juego = new Juego();
		private List<Jugador> Jugadores = new List<Jugador>();
		private bool tiradaDisponible = true;
		private LogicaDeNegocios.ServiciosDeFlipllo.Sesion SesionLocal;
		private CallbackDeJuego CanalDeCallback = new CallbackDeJuego();
		private ServidorDeJuego ServidorDeJuego;
		public GUIJuegoLocal(ObjetoDeInicializacionDeJuego Inicializador, LogicaDeNegocios.ServiciosDeFlipllo.Sesion Sesion = null, CallBackDeFlipllo CanalDeCallback = null, Servidor Servidor = null)
		{
			InitializeComponent();
			SesionLocal = Sesion;
			ServidorDeJuego = new ServidorDeJuego(this.CanalDeCallback);
			Juego.TipoDeJuego = Inicializador.TipoDeJuego;
			PanelPostJuego.Visibility = Visibility.Hidden;
			ElementosDePanelPostJuego.Padre = this;
			

			if (Juego.TipoDeJuego != TipoDeJuego.EnRed)
			{
				VentanaDeChat.Visibility = Visibility.Hidden;
				VentanaDeChat.AsignarDatos(Sesion, Servidor, CanalDeCallback);

			}

			if (Juego.TipoDeJuego != TipoDeJuego.Local)
			{
				VistaPreviaBlanco.Visibility = Visibility.Hidden;
			}

			if(Juego.TipoDeJuego == TipoDeJuego.EnRed || Inicializador.Jugadores != null)
			{
				Jugadores = Inicializador.Jugadores;
				AsignarLadosDelTablero(Inicializador.SkinNegra, Inicializador.SkinBlanca);
			}
			else
			{
				Inicializador.CargarSkins();
			}

			InicializarTablero();
			CargarFichas();
			ActualizarInterfaz();
		}

		private void AsignarLadosDelTablero(string skinNegra, string skinBlanca)
		{
			foreach (Jugador jugador in Jugadores)
			{
				if (jugador.Sesion.ID == string.Empty)
				{
					LabelOponente.Content = jugador.Sesion.Usuario;
					if (jugador.Color == LogicaDeNegocios.ServiciosDeFlipllo.ColorDeFicha.Blanco)
					{
						CargarSkins(skinNegra, skinBlanca);
					}
					else
					{
						CargarSkins(skinBlanca, skinNegra);
					}
				}
				else
				{
					LabelYo.Content = jugador.Sesion.Usuario;
				}
			}
		}

		private static void CargarSkins(string skinNegra, string skinBlanca)
		{
			CambiarSkin(skinBlanca, LogicaDeNegocios.ColorDeFicha.Blanco);
			CambiarSkin(skinNegra, LogicaDeNegocios.ColorDeFicha.Negro);
		}

		private void InicializarTablero()
		{
			FichasDeVistaPrevia = new ObservableCollection<LogicaDeNegocios.Ficha>();
			Fichas = new ObservableCollection<LogicaDeNegocios.Ficha>();
			TableroBlanco.ItemsSource = Fichas;
			TableroNegro.ItemsSource = Fichas;
			VistaPreviaBlanco.ItemsSource = FichasDeVistaPrevia;
			VistaPreviaNegro.ItemsSource = FichasDeVistaPrevia;

			for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
			{
				for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
				{
					Button botonDeTablero = new Button
					{
						Width = BotonesDeTablero.Width / TAMAÑO_DE_TABLERO,
						Height = BotonesDeTablero.Height / TAMAÑO_DE_TABLERO,
						Background = Brushes.Transparent,
						BorderBrush = Brushes.Transparent,
						IsTabStop = false
					};

					botonDeTablero.Click += ButtonTirar;
					botonDeTablero.MouseEnter += MostrarVistaPreviaDeFicha;
					botonDeTablero.MouseLeave += OcultarVistaPreviaDeFicha;

					BotonesDeTablero.Children.Add(botonDeTablero);
				}
			}
		}

		private void ButtonTirar(object sender, RoutedEventArgs e)
		{
			Button boton = sender as Button;
			int indiceDeBoton = BotonesDeTablero.Children.IndexOf(boton);
			Point puntoDeTirada = new Point(indiceDeBoton % TAMAÑO_DE_TABLERO, indiceDeBoton / TAMAÑO_DE_TABLERO);
			FichasDeVistaPrevia.Clear();

			if (Juego.SePuedeTirar(puntoDeTirada) && tiradaDisponible)
			{
				
				LabelUltimoMovimientoYo.Content = ConvertirPuntoACoordenadasDeJuego(puntoDeTirada);
				if (Juego.TipoDeJuego == TipoDeJuego.EnRed)
				{
					tiradaDisponible = false;
					ServidorDeJuego.CanalDelServidor.TirarFicha(ServidorDeJuego.ConvertirSesion(SesionLocal), puntoDeTirada);
				}
				else if (Juego.TipoDeJuego == TipoDeJuego.Local)
				{
					Juego.Tirar(puntoDeTirada);
					if (Juego.ColorDeJugadorActual == LogicaDeNegocios.ColorDeFicha.Blanco)
					{
						LabelUltimoMovimientoOponente.Content = ConvertirPuntoACoordenadasDeJuego(puntoDeTirada);
					}
					else
					{
						LabelUltimoMovimientoYo.Content = ConvertirPuntoACoordenadasDeJuego(puntoDeTirada);
					}
				}
				else
				{
					Juego.Tirar(puntoDeTirada);
					tiradaDisponible = false;
					Task.Factory.StartNew(() =>
					{
						TirarInteligenciaArtificial();
					});
				}
				CargarFichas();
				ActualizarInterfaz();
			}
		}

		private void TirarInteligenciaArtificial()
		{
			Thread.Sleep(2000);
			Dispatcher.Invoke(() =>
			{
				Juego juegoTemporal = Juego.Clonar();

				if (!juegoTemporal.JuegoTerminado)
				{
					Point puntoDeMejorMovimiento = MejorMovimiento(juegoTemporal);
					LabelUltimoMovimientoOponente.Content = ConvertirPuntoACoordenadasDeJuego(puntoDeMejorMovimiento);
					Juego.Tirar(puntoDeMejorMovimiento);
					CargarFichas();
					ActualizarInterfaz();
					tiradaDisponible = true;
				}
			});
		}

		private void MostrarVistaPreviaDeFicha(object sender, RoutedEventArgs e)
		{
			Button boton = sender as Button;
			int indiceDeBoton = BotonesDeTablero.Children.IndexOf(boton);
			Point puntoDeTirada = new Point(indiceDeBoton % TAMAÑO_DE_TABLERO, indiceDeBoton / TAMAÑO_DE_TABLERO);
			LogicaDeNegocios.Ficha fichaAAñadir = new LogicaDeNegocios.Ficha
			{
				Posicion = puntoDeTirada,
				ColorActual = Juego.ColorDeJugadorActual
			};
			if (!Fichas.Any(ficha => ficha.Posicion == fichaAAñadir.Posicion))
			{
				FichasDeVistaPrevia.Add(fichaAAñadir);
			}
		}

		private void OcultarVistaPreviaDeFicha(object sender, RoutedEventArgs e)
		{
			Button boton = sender as Button;
			if (FichasDeVistaPrevia.Count > 0)
			{
				FichasDeVistaPrevia.Remove(FichasDeVistaPrevia.First());
			}
		}

		private void CargarFichas()
		{
			List<LogicaDeNegocios.Ficha> listaDeFichas = Juego.ObtenerFichasComoLista();
			Fichas.Clear();

			foreach (LogicaDeNegocios.Ficha ficha in listaDeFichas)
			{
				Fichas.Add(ficha);
			}
		}

		private string ConvertirPuntoACoordenadasDeJuego(Point punto)
		{
			//TODO
			char letraDeX = 'A';
			string resultado = string.Empty;
			if (punto.X == 0)
			{
				letraDeX = 'A';
			}
			else if (punto.X == 1)
			{
				letraDeX = 'B';
			}
			else if (punto.X == 2)
			{
				letraDeX = 'C';
			}
			else if (punto.X == 3)
			{
				letraDeX = 'D';
			}
			else if (punto.X == 4)
			{
				letraDeX = 'E';
			}
			else if (punto.X == 5)
			{
				letraDeX = 'F';
			}
			else if (punto.X == 6)
			{
				letraDeX = 'G';
			}
			else if (punto.X == 7)
			{
				letraDeX = 'H';
			}
			punto.Y++;
			resultado = letraDeX + punto.Y.ToString();
			return resultado;
		}

		private void ActualizarInterfaz()
		{
			LabelMiNumeroDePiezas.Content = Juego.FichasNegras;
			LabelNumeroDePiezasOponente.Content = Juego.FichasBlancas;
			LabelPuntuacionYo.Content = Juego.PuntosNegros;
			LabelPuntuacionOponente.Content = Juego.PuntosBlancos;

			if (Juego.JuegoTerminado)
			{
				PanelPostJuego.Visibility = Visibility.Visible;
				PanelPostJuego.IsHitTestVisible = true;
				ElementosDePanelPostJuego.Mostrar(Juego);
			}
		}

		public void RecibirTablero(LogicaDeNegocios.ServiciosDeJuego.Ficha[] tablero)
		{
			throw new System.NotImplementedException();
		}

		public void TerminarJuego(int experenciaPorPuntos, int experenciaPorFichas, bool ganaste)
		{
			Juego.JuegoTerminado = true;
			ActualizarInterfaz();
		}

		public void RecibirTirada(Point tirada)
		{
			Juego.Tirar(tirada);
			tiradaDisponible = true;
		}
	}
}
