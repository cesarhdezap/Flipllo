using InterfazGrafica.ManejadorDeExcepciones;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion;
using ServiciosDeComunicacion.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using static InterfazGrafica.ManejadorDeExcepciones.ManejadorDeExcepcionesDeComunicacion;


namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for GUIBuscadorDeLobby.xaml
	/// </summary>
	public partial class GUIBuscadorDeLobby : Window
	{
		private List<Sala> Salas = new List<Sala>();
		private List<Sala> SalasFiltradas = new List<Sala>();
		private Servidor Servidor;
		private Sesion SesionLocal;
		private CallBackDeFlipllo CanalDeCallback;
		public GUIBuscadorDeLobby(Servidor servidor, Sesion sesion, CallBackDeFlipllo canalDeCallback)
		{
			InitializeComponent();
			SesionLocal = sesion;
			Servidor = servidor;
			CanalDeCallback = canalDeCallback;
			CanalDeCallback.RecibirSalaEvent += RecibirSala;
			CargarSalas();

		}

		private void ButtonRegresar_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ButtonActualizarListaDeLobbies_Click(object sender, RoutedEventArgs e)
		{
			CargarSalas();
			TextBoxBusqueda.Clear();
		}

		private void CargarSalas()
		{
			Salas = null;
			try
			{
				Salas = Servidor.CanalDelServidor.SolicitarSalas(SesionLocal).ToList();
			}
			catch (Exception ex)
			{
				MensajeDeError mensajeDeError = ManejarExcepcion(ex);
				mensajeDeError.Mostrar();
			}

			DataGridLobbies.ItemsSource = Salas;
		}

		private void ButtonUnirse_Click(object sender, RoutedEventArgs e)
		{
			int indiceDeSeleccion = DataGridLobbies.SelectedIndex;
			if (indiceDeSeleccion >= 0 && Salas.Count - 1 >= indiceDeSeleccion)
			{
				try
				{
					Servidor.CanalDelServidor.IngresarASala(SesionLocal, Salas.ElementAt(indiceDeSeleccion));
				}
				catch (Exception ex)
				{
					MensajeDeError mensajeDeError = ManejarExcepcion(ex);
					mensajeDeError.Mostrar();
				}

				CargarSalas();
				Sala salaEncontrada = BuscarSesionLocalEnListaDeSalas();

				if (salaEncontrada != null)
				{
					GUILobby Lobby = new GUILobby(Servidor, SesionLocal, CanalDeCallback, salaEncontrada);
					Hide();
					Lobby.ShowDialog();
					CargarSalas();
					ShowDialog();
				}
				else
				{
					MessageBox.Show("Sala no encontrada");
				}
			}
		}

		private Sala BuscarSesionLocalEnListaDeSalas()
		{
			Sala salaResultado = null;

			foreach (Sala sala in Salas)
			{
				foreach (Jugador jugador in sala.Jugadores)
				{
					if (jugador.Sesion.ID == SesionLocal.ID)
					{
						salaResultado = sala;
					}
				}
			}

			return salaResultado;
		}

		private void ButtonCrearLobby_Click(object sender, RoutedEventArgs e)
		{

			GUICrearLobby crearLobby = new GUICrearLobby();
			crearLobby.ShowDialog();
			Hide();
			Sala salaCreada = Servidor.CanalDelServidor.CrearSala(crearLobby.SalaCreada, SesionLocal, crearLobby.ColorElegido);
			GUILobby Lobby = new GUILobby(Servidor, SesionLocal, CanalDeCallback, salaCreada);
			Lobby.ShowDialog();
			ShowDialog();
		}

		private void RecibirSala(Sala sala)
		{
			Sala salaEncontrada = BuscarSesionLocalEnListaDeSalas();
			if (salaEncontrada != null)
			{
				GUILobby Lobby = new GUILobby(Servidor, SesionLocal, CanalDeCallback, salaEncontrada);
				Hide();
				Lobby.ShowDialog();
				ShowDialog();
			}
		}

		private void FiltrarSalas(string criterioDeBusqueda)
		{
			List<Sala> salasFiltradas = new List<Sala>();

			if (TextBoxBusqueda.Text != string.Empty)
			{
				salasFiltradas = Salas.FindAll(sala => sala.Nombre.Contains(criterioDeBusqueda));
				Salas = salasFiltradas;
			}

		}
	}
}
