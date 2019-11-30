using LogicaDeNegocios.ServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static LogicaDeNegocios.Servicios.ServiciosDeUsuario;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for ContadorDeNivel.xaml
	/// </summary>
	public partial class ContadorDeNivel : UserControl
	{
		private const int EXPERIENCIA_POR_NIVEL = 1000;
		private const int DURACION_DE_ANIMACION = 1500;
		private const int DURACION_DE_ANIMACION2 = 1;
		private double ExperienciaActual = 0;
		private int NivelRepresentado = 0;
		private int SumaDeExperienciaActual = 0;
		public DispatcherTimer dispatcherTimer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMilliseconds(DURACION_DE_ANIMACION),
		};
		public ContadorDeNivel()
		{
			InitializeComponent();
		}

		public void AsignarValores(Usuario usuario)
		{
			ExperienciaActual = usuario.Puntuacion.ExperienciaTotal;
			NivelRepresentado = CalcularNivel(usuario.Puntuacion.ExperienciaTotal);
			LabelNivel.Content = NivelRepresentado;
			LabelNombreDeUsuario.Content = usuario.NombreDeUsuario;
			ProgressBarProgresoDeNivel.Maximum = EXPERIENCIA_POR_NIVEL;
			ProgressBarProgresoDeNivel.Value = usuario.Puntuacion.ExperienciaTotal % EXPERIENCIA_POR_NIVEL;
		}

		public void SumarExperiencia(int experienciaASumar)
		{
			SumaDeExperienciaActual = experienciaASumar;
			dispatcherTimer.Tick += new EventHandler(AnimarSubidaDeNivel);
			if (experienciaASumar + ExperienciaActual >= EXPERIENCIA_POR_NIVEL)
			{
				dispatcherTimer.Start();
			}	
		}

		public void AnimarSubidaDeNivel(object sender, EventArgs e)
		{
			if (ExperienciaActual == 0 || ExperienciaActual == 1000)
			{

			}
			if (SumaDeExperienciaActual + ExperienciaActual >= EXPERIENCIA_POR_NIVEL)
			{
				AnimarDePosicionActual(EXPERIENCIA_POR_NIVEL);
				ExperienciaActual = 0;
				NivelRepresentado++;
				LabelNivel.Content = NivelRepresentado;
				SumaDeExperienciaActual -= EXPERIENCIA_POR_NIVEL;
			}
			else
			{
				AnimarDePosicionActual(SumaDeExperienciaActual);
				dispatcherTimer.Stop();
			}
		}

		public void AnimarDePosicionActual(int experienciaSumada)
		{

			Duration duracionDeAnimacion = new Duration(TimeSpan.FromMilliseconds(DURACION_DE_ANIMACION));
			DoubleAnimation animacion = new DoubleAnimation(experienciaSumada, duracionDeAnimacion);
			ProgressBarProgresoDeNivel.BeginAnimation(ProgressBar.ValueProperty, animacion);
		}
	}
}
