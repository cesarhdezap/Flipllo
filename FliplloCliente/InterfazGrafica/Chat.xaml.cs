using LogicaDeNegocios.ClasesDeDominio;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion;
using ServiciosDeComunicacion.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for Chat.xaml
	/// </summary>
	public partial class GUIChat : UserControl
	{
		public Servidor Servidor;
		public Sesion SesionLocal;
		private LogicaDeNegocios.ClasesDeDominio.Chat Chat;
		public CallBackDeFlipllo CanalDeCallback;

		public GUIChat()
		{
			InitializeComponent();
			Chat = new LogicaDeNegocios.ClasesDeDominio.Chat();
			
			CallBackDeFlipllo servicioDeCliente = new CallBackDeFlipllo();
			servicioDeCliente.ActualizarListaDeSesionesDeChatEvent += ActualizarListaDeClientesConectados;
			servicioDeCliente.RecibirMensajeEvent += RecibirMensaje;
		}

		public void AsignarDatos(Sesion Sesion, Servidor servidor, CallBackDeFlipllo canalDeCallback)
		{
			SesionLocal = Sesion;
			Servidor = servidor;
			CanalDeCallback = canalDeCallback;
		}

		public void ActualizarListaDeClientesConectados(List<Sesion> clientesConectados)
		{
			Chat.UsuariosConectados = clientesConectados;
		}
		
		public void RecibirMensaje(Mensaje mensaje)
		{
			Chat.MensajesRecibidos.Add(mensaje);
			TextBlockChat.Text = Chat.MensajesToString();
		}

		private void ButtonEnviarMensaje_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(TextBoxMensaje.Text) && SesionLocal.Usuario.Estado == EstadoUsuario.Registrado)
			{
				Mensaje mensaje = new Mensaje
				{
					NombreDeUsuario = SesionLocal.Usuario.NombreDeUsuario,
					Fecha = DateTime.Now,
					CuerpoDeMensaje = TextBoxMensaje.Text
				};
				try
				{
					Servidor.CanalDelServidor.EnviarMensaje(mensaje, SesionLocal);
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
	}
}
