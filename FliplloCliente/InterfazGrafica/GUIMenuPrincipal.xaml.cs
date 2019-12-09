using InterfazGrafica.ManejadorDeExcepciones;
using LogicaDeNegocios.ClasesDeDominio;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion;
using ServiciosDeComunicacion.Proxy;
using System;
using System.Windows;
using static InterfazGrafica.ManejadorDeExcepciones.ManejadorDeExcepcionesDeComunicacion;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for Chat.xaml
	/// </summary>
	public partial class GUIMenuPrincipal : Window
	{
		public Servidor Servidor;
		public Chat Chat;
		public Sesion SesionLocal;
		public Window Padre;

		public CallBackDeFlipllo CanalDeCallback;


		public GUIMenuPrincipal(Sesion Sesion, Servidor servidor, CallBackDeFlipllo canalDeCallback)
		{
			InitializeComponent();
			Chat = new Chat();
			SesionLocal = Sesion;
			Servidor = servidor;
			CanalDeCallback = canalDeCallback;
			VentanaDeChat.AsignarDatos(SesionLocal, servidor, canalDeCallback);
			ContadorDeNivel.AsignarValores(SesionLocal.Usuario);
			canalDeCallback.RecibirMensajeEvent += RecibirMensaje;
		}

		private void RecibirMensaje(Mensaje mensaje)
		{
			VentanaDeChat.RecibirMensaje(mensaje);
		}


		private void ButtonJugar_Click(object sender, RoutedEventArgs e)
		{
			GUIBuscadorDeLobby buscadorDeLobby = new GUIBuscadorDeLobby(Servidor, SesionLocal, CanalDeCallback);
			Hide();
			buscadorDeLobby.ShowDialog();
			ShowDialog();
		}

		private void ButtonBotin_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ButtonContactanos_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://t.me/pachinster");
			System.Diagnostics.Process.Start("https://t.me/cesarhdezap");
		}

		private void ButtoGitHub_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/cesarhdezap/Flipllo");
		}

		private void ButtonSalir_Click(object sender, RoutedEventArgs e)
		{
			CerrarSesion();
		}

		private void CerrarSesion()
		{
			try
			{
				Servidor.CanalDelServidor.CerrarSesion(SesionLocal);
			}
			catch (Exception ex)
			{
				MensajeDeError mensajeDeError = ManejarExcepcion(ex);
				mensajeDeError.Mostrar();
			}
			finally
			{
				Close();
				Padre.Show();
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			CerrarSesion();
		}
	}

}
