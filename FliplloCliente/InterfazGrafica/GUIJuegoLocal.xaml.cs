using LogicaDeNegocios;
using LogicaDeNegocios.ClasesDeDominio;
using LogicaDeNegocios.ServiciosDeFlipllo;
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
	public partial class GUIJuegoLocal : Window
	{
		private ObservableCollection<Ficha> Fichas;
		private ObservableCollection<Ficha> FichasDeVistaPrevia;
		private readonly int TAMAÑO_DE_TABLERO = 8;
		private Juego Juego = new Juego();
		private List<Jugador> Jugadores = new List<Jugador>();
		private TipoDeJuego TipoDeJuego;
		private bool tiradaDisponible = true;
		public GUIJuegoLocal(TipoDeJuego tipoDeJuego, string skinNegra = "Alana", string skinBlanca = "Alana", List<Jugador> jugadores = null)
		{
			InitializeComponent();
			TipoDeJuego = tipoDeJuego;
			PanelPostJuego.Visibility = Visibility.Hidden;

			if (TipoDeJuego != TipoDeJuego.EnRed)
			{
				TextBlockChatBox.Visibility = Visibility.Hidden;
				LabelCajaDeMensaje.Visibility = Visibility.Hidden;
				TextBoxCajaDeMensaje.Visibility = Visibility.Hidden;
				ButtonEnviarMensaje.Visibility = Visibility.Hidden;
				RectangleEnviarMensaje.Visibility = Visibility.Hidden;
			}

			if (TipoDeJuego != TipoDeJuego.Local)
			{
				VistaPreviaBlanco.Visibility = Visibility.Hidden;
			}

			if(TipoDeJuego == TipoDeJuego.EnRed || jugadores != null)
			{
				Jugadores = jugadores;
				AsignarLadosDelTablero(skinNegra, skinBlanca);
			}
			else
			{
				CargarSkins(skinNegra, skinBlanca);
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
			FichasDeVistaPrevia = new ObservableCollection<Ficha>();
			Fichas = new ObservableCollection<Ficha>();
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
				if (TipoDeJuego == TipoDeJuego.EnRed)
				{
					tiradaDisponible = false;
					//Juego.Tirar(puntoDeTirada); Pero el metodo en la red
				}
				else if (TipoDeJuego == TipoDeJuego.Local)
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

		private void RecibirTirada(Point puntoDeTirada)
		{
			Juego.Tirar(puntoDeTirada);
			tiradaDisponible = true;
		}

		private void MostrarVistaPreviaDeFicha(object sender, RoutedEventArgs e)
		{
			Button boton = sender as Button;
			int indiceDeBoton = BotonesDeTablero.Children.IndexOf(boton);
			Point puntoDeTirada = new Point(indiceDeBoton % TAMAÑO_DE_TABLERO, indiceDeBoton / TAMAÑO_DE_TABLERO);
			Ficha fichaAAñadir = new Ficha
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
			List<Ficha> listaDeFichas = Juego.ObtenerFichasComoLista();
			Fichas.Clear();

			foreach (Ficha ficha in listaDeFichas)
			{
				Fichas.Add(ficha);
			}
		}

		private string ConvertirPuntoACoordenadasDeJuego(Point punto)
		{
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
			}
		}

		private void TextBoxCajaDeMensaje_TextChanged(object sender, TextChangedEventArgs e)
		{
			OcultarPista(TextBoxCajaDeMensaje, LabelCajaDeMensaje);
		}
	}
}
