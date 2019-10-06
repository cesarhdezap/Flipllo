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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using LogicaDeNegocios.ServiciosDeComunicacion;
using static LogicaDeNegocios.ServiciosDeRecursosDeIdioma;
using LogicaDeNegocios.Excepciones;
using LogicaDeNegocios;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int PRIMER_INDICE_DE_COMBOBOX = 0;

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
			catch (RecursoNoEncontradoException e)
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
			//TODO
		}

		private void LabelCrearUnaCuenta_Click(object sender, RoutedEventArgs e)
		{
			RegistrarCuenta registrarCuenta = new RegistrarCuenta();
			Hide();
			registrarCuenta.ShowDialog();
			TextBoxNombreDeUsuario.Clear();
			PasswordBoxContraseña.Clear();
		}
	}
}
