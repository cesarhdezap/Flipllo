using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion.Proxy;
using System.Windows;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for GUICrearLobby.xaml
	/// </summary>
	public partial class GUICrearLobby : Window
	{
		private Servidor Servidor;
		private Sesion SesionLocal;
		public GUICrearLobby(Servidor servidor, Sesion sesion)
		{
			InitializeComponent();
			SesionLocal = sesion;
			Servidor = servidor;

		}

		private void ButtonCrearLobby_Click(object sender, RoutedEventArgs e)
		{
			string nombreDeSala = TextBoxNombreDeLobby.Text;
			int nivelMinimo = 0;
			int nivelMaximo = 0;
			if (int.TryParse(TextBoxNivelMinimo.Text, out nivelMinimo) && int.TryParse(TextBoxNivelMaximo.Text, out nivelMaximo));
			Sala salaResultado = new Sala()
			{
				Nombre = nombreDeSala,
				NivelMaximo = nivelMaximo,
				NivelMinimo = nivelMinimo
			};

			Servidor.CanalDelServidor.CrearSala(salaResultado, SesionLocal);
			Close();
		}
	}
}
