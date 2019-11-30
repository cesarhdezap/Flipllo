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
using static LogicaDeNegocios.ServiciosDeRecursos;
using static InterfazGrafica.Utilierias.UtilieriasDeElementosGraficos;
using static LogicaDeNegocios.Servicios.ServiciosDeEncriptacion;
using LogicaDeNegocios.ClasesDeDominio;
using System.Windows.Threading;

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
		private ServiciosDeCallBack CanalDeCallback = new ServiciosDeCallBack();
		DispatcherTimer Timer = new DispatcherTimer()
		{
			Interval = TimeSpan.FromSeconds(10)
		};
		public MainWindow()
		{
			InitializeComponent();
			string culturaPorDefecto = CultureInfo.CurrentCulture.Name;
			CanalDeCallback.RecibirSesionEvent += RecibirSesion;
			Servidor = new Servidor(CanalDeCallback);
			CargarRecursosGraficosPorDefecto();
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
				CambiarRecursoDeIdioma(locale);
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
					Contraseña = EncriptarCadena(PasswordBoxContraseña.Password)
				}
			};
			try
			{
				Timer.Tick += new EventHandler(RestaurarCanal);

				Servidor.CrearCanal();
				Timer.Start();
				Servidor.CanalDelServidor.AutenticarUsuario(SesionActual.Usuario);
			}
			catch (TimeoutException)
			{
				MessageBox.Show(Application.Current.Resources["tiempoAgotado"].ToString(), Application.Current.Resources["algoAndMal"].ToString());
			}
			catch (CommunicationException ex)
			{
				MessageBox.Show(Application.Current.Resources["errorDeConexion"].ToString(), Application.Current.Resources["algoAndaMal"].ToString());
				MessageBox.Show(ex.ToString());
			}
		}

		private void RestaurarCanal(object sender, EventArgs e)
		{
			Servidor.RestaurarCanal();	
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
			sesion.Usuario.Contraseña = PasswordBoxContraseña.Password;
			sesion.Usuario.CorreoElectronico = TextBoxNombreDeUsuario.Text;
			if (SesionActual.Usuario.Estado == EstadoUsuario.Inexistente)
			{
				MessageBox.Show(Application.Current.Resources["credencialesInvalidas"].ToString(), Application.Current.Resources["credencialesInvalidasTitulo"].ToString());
			}
			else
			{
				if (SesionActual.Usuario.Estado == EstadoUsuario.CodigoDeValidacionNovalido)
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
					GUIMenuPrincipal menuPrincipal = new GUIMenuPrincipal(SesionActual, Servidor, CanalDeCallback);
						menuPrincipal.Padre= this;
					Hide();
					menuPrincipal.Show();
					TextBoxNombreDeUsuario.Clear();
					PasswordBoxContraseña.Clear();
				}
			}
		}

		private void TextBoxNombreDeUsuario_TextChanged(object sender, TextChangedEventArgs e)
		{
			OcultarPista(TextBoxNombreDeUsuario, LabelNombreDeUsuario);
			ContadorDeNivel.SumarExperiencia(5000);
		}

		private void PasswordBoxContraseña_PasswordChanged(object sender, RoutedEventArgs e)
		{
			OcultarPista(PasswordBoxContraseña, LabelContraseña);
		}

		private void LabelJuegoLocalIA_Click(object sender, RoutedEventArgs e)
		{
			GUIJuegoLocal juegoLocal = new GUIJuegoLocal(TipoDeJuego.InteligenciaArtifical);
			Hide();
			juegoLocal.ShowDialog();
			Show();
			TextBoxNombreDeUsuario.Clear();
			PasswordBoxContraseña.Clear();
		}

		private void LabelJuegoLocalAmigo_Click(object sender, RoutedEventArgs e)
		{
			GUIJuegoLocal juegoLocal = new GUIJuegoLocal(TipoDeJuego.Local);
			Hide();
			juegoLocal.ShowDialog();
			Show();
			TextBoxNombreDeUsuario.Clear();
			PasswordBoxContraseña.Clear();
		}


	}
}
