using InterfazGrafica.ManejadorDeExcepciones;
using LogicaDeNegocios.Servicios;
using LogicaDeNegocios.ServiciosDeFlipllo;
using ServiciosDeComunicacion.Proxy;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static LogicaDeNegocios.ServiciosDeRecursos;
using static InterfazGrafica.Utilierias.UtilieriasDeElementosGraficos;
using static InterfazGrafica.ManejadorDeExcepciones.ManejadorDeExcepcionesDeComunicacion;

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
			int timeout = 1000;
			bool codigoValidacion = false;
			try
			{
				codigoValidacion = await Task.WhenAny(Task.Run<bool>(() => Servidor.CanalDelServidor.ValidarCodigoDeUsuario(usuario)), Task.Delay(timeout)) == Task.Run<bool>(() => Servidor.CanalDelServidor.ValidarCodigoDeUsuario(usuario));
			}
			catch (Exception ex)
			{
				MensajeDeError mensajeDeError = ManejarExcepcion(ex);
				mensajeDeError.Mostrar();
			}

			if (codigoValidacion)
			{
				MessageBox.Show(ObtenerRecursoDeTexto("redirigidoParaIniciarSesion"), ObtenerRecursoDeTexto("exito"));
				Close();
			}
			else
			{
				MessageBox.Show(ObtenerRecursoDeTexto("tiempoAgotado"), ObtenerRecursoDeTexto("algoAndaMal"));
			}
		}
	}
}
