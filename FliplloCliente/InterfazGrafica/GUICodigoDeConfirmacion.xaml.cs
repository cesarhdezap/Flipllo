using LogicaDeNegocios.Servicios;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion.Proxy;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static InterfazGrafica.Utilierias.UtilieriasDeElementosGraficos;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for GUICodigoDeConfirmacion.xaml
	/// </summary>
	public partial class GUICodigoDeConfirmacion : Window
	{
		private Servidor Servidor;
		private Usuario Usuario;
		public GUICodigoDeConfirmacion(Usuario usuario, Servidor servidor)
		{
			InitializeComponent();
			Servidor = servidor;
			Usuario = usuario;
		}

		private void TextBoxCodigoDeVerificacion_TextChanged(object sender, TextChangedEventArgs e)
		{
			OcultarPista(TextBoxCodigoDeVerificacion, LabelCodigoDeVerificacion);
			if (ServiciosDeValidacion.ValidarCodigoDeVerificacion(TextBoxCodigoDeVerificacion.Text))
			{
				Usuario.CodigoDeVerificacion = TextBoxCodigoDeVerificacion.Text;
				ValidarCodigoDeUsuario(Usuario);
			}
		}

		private async void ValidarCodigoDeUsuario(Usuario usuario)
		{
			int timeout = 100000;
			bool codigoValidacion = false;
			try
			{
				codigoValidacion = await Task.WhenAny(Task.Run<bool>(() => Servidor.CanalDelServidor.ValidarCodigoDeUsuario(usuario)),Task.Delay(timeout)) == Task.Run<bool>(() => Servidor.CanalDelServidor.ValidarCodigoDeUsuario(usuario));
			}
			catch (TimeoutException)
			{
				MessageBox.Show(Application.Current.Resources["tiempoAgotado"].ToString(), Application.Current.Resources["algoAndMal"].ToString());
			}
			catch (CommunicationException)
			{
				MessageBox.Show(Application.Current.Resources["errorDeConexion"].ToString(), Application.Current.Resources["algoAndMal"].ToString());
			}

			if (codigoValidacion)
			{
				MessageBox.Show(Application.Current.Resources["redirigidoParaIniciarSesion"].ToString(), Application.Current.Resources["exito"].ToString());
				Close();
			}
			else
			{
				MessageBox.Show(Application.Current.Resources["tiempoAgotado"].ToString(), Application.Current.Resources["algoAndaMal"].ToString());
			}
		}
	}
}
