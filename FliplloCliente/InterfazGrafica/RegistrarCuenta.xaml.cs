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
using System.Globalization;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion.Proxy;
using ServiciosDeComunicacion;
using LogicaDeNegocios.Servicios;
using static InterfazGrafica.Utilierias.UtilieriasDeElementosGraficos;
using System.ServiceModel;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for RegistrarCuenta.xaml
	/// </summary>
	public partial class GUIRegistrarCuenta : Window

	{
		public Servidor Servidor;
		public GUIRegistrarCuenta()
		{
			InitializeComponent();	
		}

		private void TextBoxNombreDeUsuario_TextChanged(object sender, TextChangedEventArgs e)
		{
			OcultarPista(TextBoxNombreDeUsuario, LabelNombreDeUsuario);
			MostrarEstadoDeValidacionNombreDeUsuario(TextBoxNombreDeUsuario);
		}

		private void TextBoxCorreoElectronico_TextChanged(object sender, TextChangedEventArgs e)
		{
			OcultarPista(TextBoxCorreoElectronico, LabelCorreoElectronico);
			MostrarEstadoDeValidacionCorreoElectronico(TextBoxCorreoElectronico);
		}

		private void TextBoxConfirmarCorreoElectronico_TextChanged(object sender, TextChangedEventArgs e)
		{
			OcultarPista(TextBoxConfirmarCorreoElectronico, LabelConfirmarCorreoElectronico);
			MostrarEstadoDeValidacionConfirmacion(TextBoxCorreoElectronico, TextBoxConfirmarCorreoElectronico);
		}

		private void TextBoxContraseña_TextChanged(object sender, TextChangedEventArgs e)
		{
			OcultarPista(TextBoxContraseña, LabelContraseña);
			MostrarEstadoDeValidacionContraseña(TextBoxContraseña);
		}
		private void TextBoxConfirmarContraseña_TextChanged(object sender, TextChangedEventArgs e)
		{
			OcultarPista(TextBoxConfirmarContraseña, LabelConfirmarContraseña);
			MostrarEstadoDeValidacionConfirmacion(TextBoxContraseña, TextBoxConfirmarContraseña);
		}

		private void ButtonCrearCuenta_Click(object sender, RoutedEventArgs e)
		{
			Usuario usuario = new Usuario()
			{
				CorreoElectronico = TextBoxCorreoElectronico.Text,
				NombreDeUsuario = TextBoxNombreDeUsuario.Text,
				Contraseña = TextBoxContraseña.Text
			};
			Servidor = new Servidor(new CallBackDeFlipllo());
			Servidor.CrearCanal();
			try
			{
				if (Servidor.CanalDelServidor.RegistrarUsuario(usuario))
				{
					GUICodigoDeConfirmacion codigoDeConfirmacion = new GUICodigoDeConfirmacion(usuario, Servidor);
					Hide();
					codigoDeConfirmacion.ShowDialog();
					Close();
				}
			}
			catch (TimeoutException)
			{
				MessageBox.Show(Application.Current.Resources["tiempoAgotado"].ToString(), Application.Current.Resources["algoAndMal"].ToString());
			}
			catch (CommunicationException)
			{
				MessageBox.Show(Application.Current.Resources["errorDeConexion"].ToString(), Application.Current.Resources["algoAndMal"].ToString());
			}
		}

		private void LabelTerminosDeUso_Click(object sender, RoutedEventArgs e)
		{
			GUITerminosDeServicio terminosDeServicio = new GUITerminosDeServicio();
			Hide();
			terminosDeServicio.ShowDialog();
			Show();	
		}
	}
}
