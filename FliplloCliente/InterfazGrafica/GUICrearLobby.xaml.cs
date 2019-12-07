using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion.Proxy;
using System.Windows;
using System.Windows.Controls;
using static LogicaDeNegocios.Servicios.ServiciosDeLogicaDeJuego;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for GUICrearLobby.xaml
	/// </summary>
	public partial class GUICrearLobby : Window
	{
		private Servidor Servidor;
		private Sesion SesionLocal;
		private ColorDeFicha ColorElegido = ColorDeFicha.Negro;
		public GUICrearLobby(Servidor servidor, Sesion sesion)
		{
			InitializeComponent();
			SesionLocal = sesion;
			Servidor = servidor;
			ButtonCambiarColorANegro.IsEnabled = false;

		}

		public GUICrearLobby()
		{
			InitializeComponent();
			ButtonCambiarColorANegro.IsEnabled = false;
		}

		private void ButtonCrearLobby_Click(object sender, RoutedEventArgs e)
		{
			string nombreDeSala = TextBoxNombreDeLobby.Text;
			if (int.TryParse(TextBoxNivelMinimo.Text, out int nivelMinimo) && int.TryParse(TextBoxNivelMaximo.Text, out int nivelMaximo))
			{
				Sala salaResultado = new Sala()
				{
					Nombre = nombreDeSala,
					NivelMaximo = nivelMaximo,
					NivelMinimo = nivelMinimo
				};

				Servidor.CanalDelServidor.CrearSala(salaResultado, SesionLocal, ColorElegido);
				Close();
			}
		}

		private void CambiarColorElegido()
		{
			LogicaDeNegocios.ColorDeFicha colorConvertido = (LogicaDeNegocios.ColorDeFicha)(int)ColorElegido;
			ColorElegido = (ColorDeFicha)(int)ColorContrario(colorConvertido);
		}

		private void ButtonCambiarColorANegro_Click(object sender, RoutedEventArgs e)
		{
			CambiarColorElegido();
			ButtonCambiarColorANegro.IsEnabled = false;
			ButtonCambiarColorABlanco.IsEnabled = true;
		}

		private void ButtonCambiarColorABlanco_Click(object sender, RoutedEventArgs e)
		{
			CambiarColorElegido();
			ButtonCambiarColorABlanco.IsEnabled = false;
			ButtonCambiarColorANegro.IsEnabled = true;
		}
	}
}