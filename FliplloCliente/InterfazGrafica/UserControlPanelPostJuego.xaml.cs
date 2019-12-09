using LogicaDeNegocios.ClasesDeDominio;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using static LogicaDeNegocios.ServiciosDeRecursos;
using System.Windows.Shapes;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for GUIPanelPostJuego.xaml
	/// </summary>
	public partial class GUIPanelPostJuego : UserControl
	{

		public Window Padre;

		public double DURACION_DE_ANIMACION { get; private set; } = 500;
		public const int OPACIDAD_DESTINO = 1;
		public const int EXPERIENCIA_OBTENIDA_POR_VICTORIA = 100;

		public GUIPanelPostJuego()
		{
			InitializeComponent();
			Opacity = 0;
		}

		private void ButtonGG_Click(object sender, RoutedEventArgs e)
		{
			Padre.Close();
		}

		private void AnimarEntrada()
		{
			Duration duracionDeAnimacion = new Duration(TimeSpan.FromMilliseconds(DURACION_DE_ANIMACION));
			DoubleAnimation animacion = new DoubleAnimation(OPACIDAD_DESTINO, duracionDeAnimacion);
			this.BeginAnimation(UserControl.OpacityProperty, animacion);
		}

		public void TerminarJuego(int ExperienciaPorPuntos, int ExperienciaPorFichas, bool Victoria)
		{
			int experienciaObtenida = ExperienciaPorPuntos + ExperienciaPorFichas;
			string desgloseDeExperiencia = Application.Current.Resources["puntos"].ToString() + ": " + ExperienciaPorFichas + Environment.NewLine +
						Application.Current.Resources["fichas"].ToString() + ": " + ExperienciaPorFichas + Environment.NewLine;
			if (Victoria)
			{
				desgloseDeExperiencia.Concat(Application.Current.FindResource("victoria").ToString() + ": " + EXPERIENCIA_OBTENIDA_POR_VICTORIA + Environment.NewLine);
				experienciaObtenida += EXPERIENCIA_OBTENIDA_POR_VICTORIA;
			}
			desgloseDeExperiencia.Concat(Application.Current.FindResource("total").ToString() + ": " + EXPERIENCIA_OBTENIDA_POR_VICTORIA);

			TextBlockDesgloseDeExperiencia.Text = desgloseDeExperiencia + ": " + experienciaObtenida;
		}

		public void AsignarGanador(string nombreDeGanador)
		{
			LabelMensajeDeVictoria.Content = nombreDeGanador + ObtenerRecursoDeTexto("gana");
		}

		public void Mostrar(Juego juego)
		{
			if (juego.TipoDeJuego != TipoDeJuego.EnRed)
			{
				LabelEstadisticas.Visibility = Visibility.Hidden;
				LabelExperienciaGanada.Visibility = Visibility.Hidden;
				LabelPartidasJugadas.Visibility = Visibility.Hidden;
				LabelPorcentajeDeVictorias.Visibility = Visibility.Hidden;
				LabelPuntuacionFinalColorDelOponente.Visibility = Visibility.Hidden;
				LabelPuntuacionFinalMiColor.Visibility = Visibility.Hidden;
				LabelValorDePartidasJugadas.Visibility = Visibility.Hidden;
				LabelValorDePorcentajeDeVictorias.Visibility = Visibility.Hidden; 
				LabelValorDeVictorias.Visibility = Visibility.Hidden;
				LabelValorPuntuacionFinalColorDelOponente.Visibility = Visibility.Hidden;
				LabelValorPuntuacionFinalMiColor.Visibility = Visibility.Hidden;
				LabelVictorias.Visibility = Visibility.Hidden;
				ContadorDeNivel.Visibility = Visibility.Hidden;

				if(juego.TipoDeJuego == TipoDeJuego.Local)
				{
					LabelMiNombreDeUsuario.Content = Application.Current.Resources["negras"];
					LabelNombreDeUsuarioOponente.Content = Application.Current.Resources["blancas"];
					if (juego.FichasBlancas > juego.FichasNegras)
					{
						LabelMensajeDeVictoria.Content = Application.Current.Resources["blancoGana"];
					}
					else if (juego.FichasBlancas < juego.FichasNegras)
					{
						LabelMensajeDeVictoria.Content = Application.Current.Resources["negroGana"];
					}
					else
					{
						LabelMensajeDeVictoria.Content = Application.Current.Resources["empate"];
					}
				}
				else
				{
					LabelMiNombreDeUsuario.Content = Application.Current.Resources["tu"];
					LabelNombreDeUsuarioOponente.Content = Application.Current.Resources["alana"];
					if (juego.FichasBlancas > juego.FichasNegras)
					{
						LabelMensajeDeVictoria.Content = Application.Current.Resources["alanaGana"];
					}
					else if (juego.FichasBlancas < juego.FichasNegras)
					{
						LabelMensajeDeVictoria.Content = Application.Current.Resources["alanaPierde"];
					}
					else
					{
						LabelMensajeDeVictoria.Content =  Application.Current.Resources["alanaEmpate"];
					}
				}

				
			}

			AnimarEntrada();
		}
	}
}
