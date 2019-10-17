using LogicaDeNegocios.Excepciones;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion;
using ServiciosDeComunicacion.Proxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using static LogicaDeNegocios.ServiciosDeRecursosDeIdioma;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int PRIMER_INDICE_DE_COMBOBOX = 0;
		private Sesion SesionActual;
		private Servidor Servidor;
		public MainWindow()
		{
			InitializeComponent();

			string culturaPorDefecto = CultureInfo.CurrentCulture.Name;

			List<string> listaDeIdiomas = new List<string>
			{
				"en-US",
				"es-MX"
			};

			bool huboExcepcion = false;
			CargarRecursos(culturaPorDefecto);
			ComboBoxCambiarIdioma.ItemsSource = listaDeIdiomas;

			if (!huboExcepcion)
			{
				ComboBoxCambiarIdioma.SelectedValue = culturaPorDefecto;
			}
			else
			{
				ComboBoxCambiarIdioma.SelectedIndex = PRIMER_INDICE_DE_COMBOBOX;
			}

			ComboBoxCambiarIdioma.SelectedIndex = PRIMER_INDICE_DE_COMBOBOX;


		}

		/// <summary>
		///	Carga los recursos del <paramref name="locale"/> especificado.
		/// </summary>
		/// <param name="locale">El locale que cargar</param>
		private void CargarRecursos(string locale)
		{
			try
			{
				CambiarRecursos(locale);
			}
			catch (RecursoNoEncontradoException)
			{
				MessageBox.Show("The resource " + locale + " wasn't found. Using english as default.");
			}
		}

		private void ComboBoxCambiarIdioma_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CargarRecursos(ComboBoxCambiarIdioma.SelectedItem.ToString());
		}

		private void ButtonIniciarSesion_Click(object sender, RoutedEventArgs e)
		{

			SesionActual = new Sesion()
			{
				Usuario = new Usuario
				{
					CorreoElectronico = TextBoxNombreDeUsuario.Text,
					Contraseña = PasswordBoxContraseña.Password
				}
			};

			ServiciosDeCallBack serviciosDeCallBack = new ServiciosDeCallBack();
			serviciosDeCallBack.RecibirSesionEvent += RecibirSesion;
			Servidor = new Servidor(serviciosDeCallBack);
			try
			{
				Servidor.CrearCanal();
				Servidor.CanalDelServidor.AutenticarUsuario(SesionActual.Usuario);
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

		private void LabelCrearUnaCuenta_Click(object sender, RoutedEventArgs e)
		{
			GUIRegistrarCuenta registrarCuenta = new GUIRegistrarCuenta();
			Hide();
			registrarCuenta.ShowDialog();
			Show();
			TextBoxNombreDeUsuario.Clear();
			PasswordBoxContraseña.Clear();
		}


		private void RecibirSesion(Sesion sesion)
		{
			SesionActual = sesion;
			if (SesionActual.ID == 0)
			{
				MessageBox.Show(Application.Current.Resources["credencialesInvalidas"].ToString(), Application.Current.Resources["credencialesInvalidasTitulo"].ToString());
			}
			else
			{
				if (SesionActual.Usuario.Estado == EstadoUsuario.NoValidado)
				{
					GUICodigoDeConfirmacion codigoDeConfirmacion = new GUICodigoDeConfirmacion(SesionActual.Usuario, Servidor);
					Hide();
					codigoDeConfirmacion.ShowDialog();
					Show();
					TextBoxNombreDeUsuario.Clear();
					PasswordBoxContraseña.Clear();
				}
				else if (SesionActual.Usuario.Estado == EstadoUsuario.Registrado)
				{
					GUIMenuPrincipal chat = new GUIMenuPrincipal(SesionActual);
					Hide();
					chat.ShowDialog();
					Show();
					TextBoxNombreDeUsuario.Clear();
					PasswordBoxContraseña.Clear();
				}
			}
		}
	}
}
