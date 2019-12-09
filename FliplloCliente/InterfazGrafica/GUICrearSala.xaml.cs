using InterfazGrafica.ManejadorDeExcepciones;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion.Proxy;
using System;
using System.Windows;
using System.Windows.Controls;
using static LogicaDeNegocios.Servicios.ServiciosDeLogicaDeJuego;
using static LogicaDeNegocios.ServiciosDeRecursos;
using static InterfazGrafica.ManejadorDeExcepciones.ManejadorDeExcepcionesDeComunicacion;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for GUICrearLobby.xaml
	/// </summary>
	public partial class GUICrearLobby : Window
	{
		public ColorDeFicha ColorElegido = ColorDeFicha.Negro;
		public Sala SalaCreada = new Sala();
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
				SalaCreada = salaResultado;
				Close();
			}
			else
			{
				MessageBox.Show(ObtenerRecursoDeTexto("soloEnterosMensaje"), ObtenerRecursoDeTexto("soloEnterosTitulo"));
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