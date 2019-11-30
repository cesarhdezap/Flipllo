using LogicaDeNegocios.ClasesDeDominio;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion;
using ServiciosDeComunicacion.Proxy;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using static LogicaDeNegocios.Servicios.ServiciosDeUsuario;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for Chat.xaml
	/// </summary>
	public partial class GUIMenuPrincipal : Window
	{
		public Servidor Servidor;
		public LogicaDeNegocios.ClasesDeDominio.Chat Chat;
		public Sesion SesionLocal;
		public Window Padre;

		public ServiciosDeCallBack CanalDeCallback;


		public GUIMenuPrincipal(Sesion Sesion, Servidor servidor, ServiciosDeCallBack canalDeCallback)
		{
			InitializeComponent();
			Chat = new LogicaDeNegocios.ClasesDeDominio.Chat();
			SesionLocal = Sesion;
			Servidor = servidor;
			CanalDeCallback = canalDeCallback;
			ContadorDeNivel.AsignarValores(SesionLocal.Usuario);
			ServiciosDeCallBack servicioDeCliente = new ServiciosDeCallBack();
			servicioDeCliente.ListaDeClientesConectadosEvent += ActualizarListaDeClientesConectados;
			servicioDeCliente.NuevoMensajeRecibidoEvent += MostrarNuevoMensaje;
			servicioDeCliente.ActualizarIDDeUsuarioEvent += ActualizarIDDeUsuario;
			
		}

		private void ActualizarListaDeClientesConectados(List<Sesion> clientesConectados)
		{
			Chat.UsuariosConectados = clientesConectados;
			DataGridUsuariosConectados.ItemsSource = Chat.UsuariosConectados;
		}

		private void MostrarNuevoMensaje(Mensaje mensaje)
		{
			Chat.MensajesRecibidos.Add(mensaje);
			TextBlockChat.Text = Chat.MensajesToString();
		}

		private void ActualizarIDDeUsuario(int id)
		{
			//SesionLocal.ID = id;
		}

		private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(TextBoxMensaje.Text) && SesionLocal.Usuario.Estado == EstadoUsuario.Registrado)
			{
				Mensaje mensaje = new Mensaje
				{
					//IDDeUsuario = SesionLocal.ID,
					Fecha = DateTime.Now,
					CuerpoDeMensaje = TextBoxMensaje.Text
				};
				try
				{
					Servidor.CanalDelServidor.EnviarMensajeAChatGlobal(mensaje, SesionLocal);
				}
				catch (TimeoutException)
				{
					MessageBox.Show(Application.Current.Resources["tiempoAgotado"].ToString(), Application.Current.Resources["algoAndaMal"].ToString());
				}
				catch (CommunicationException)
				{
					MessageBox.Show(Application.Current.Resources["errorDeConexion"].ToString(), Application.Current.Resources["algoAndaMal"].ToString());
				}
				TextBoxMensaje.Text = string.Empty;
			}
		}

		private void ButtonJugar_Click(object sender, RoutedEventArgs e)
		{
			GUIBuscadorDeLobby buscadorDeLobby = new GUIBuscadorDeLobby(Servidor, SesionLocal, CanalDeCallback);
			Hide();
			buscadorDeLobby.ShowDialog();
			Show();
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
			catch (TimeoutException)
			{
				MessageBox.Show(Application.Current.Resources["tiempoAgotado"].ToString(), Application.Current.Resources["algoAndaMal"].ToString());
			}
			catch (CommunicationException)
			{
				MessageBox.Show(Application.Current.Resources["errorDeConexion"].ToString(), Application.Current.Resources["algoAndaMal"].ToString());
			}
			finally
			{
				Close();
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			CerrarSesion();
			Padre.Show();
		}
	}

}
