﻿using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion;
using ServiciosDeComunicacion.Proxy;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


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
			Salas = Servidor.CanalDelServidor.SolicitarSalas(SesionLocal).ToList();
			DataGridLobbies.ItemsSource = Salas;
		}

		private void ButtonUnirse_Click(object sender, RoutedEventArgs e)
		{
			int indiceDeSeleccion = DataGridLobbies.SelectedIndex;
			if (indiceDeSeleccion >= 0 && Salas.Count - 1 >= indiceDeSeleccion)
			{
				Servidor.CanalDelServidor.IngresarASala(SesionLocal, Salas.ElementAt(indiceDeSeleccion));
				CargarSalas();
				Sala salaEncontrada = BuscarSesionLocalEnListaDeSalas();
				if (salaEncontrada != null)
				{
					GUILobby Lobby = new GUILobby(Servidor, SesionLocal, CanalDeCallback, salaEncontrada);
					Hide();
					Lobby.ShowDialog();
					CargarSalas();
					Show();
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
			GUICrearLobby crearLobby = new GUICrearLobby(Servidor, SesionLocal);
			Hide();
			crearLobby.ShowDialog();
			Show();
		}

		private void RecibirSala(Sala sala)
		{
			GUILobby lobby = new GUILobby(Servidor, SesionLocal, CanalDeCallback, sala);
			Hide();
			lobby.ShowDialog();
			Show();
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
