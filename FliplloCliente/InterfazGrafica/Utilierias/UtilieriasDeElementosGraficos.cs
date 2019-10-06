
using Microsoft.Win32;
using System;
using System.Windows;
using static LogicaDeNegocios.Servicios.ServiciosDeValidacion;
using System.Windows.Controls;
using System.Windows.Media;

namespace InterfazGrafica.Utilierias
{
	public static class UtilieriasDeElementosGraficos
	{
		public static void MostrarEstadoDeValidacionContraseña(TextBox textBoxContraseña)
		{
			if (ValidarContraseña(textBoxContraseña.Text))
			{
				textBoxContraseña.BorderBrush = Brushes.Green;
				OcultarToolTip(textBoxContraseña);
			}
			else
			{
				textBoxContraseña.BorderBrush = Brushes.Red;
				MostrarToolTip(textBoxContraseña, Application.Current.Resources["contraseñaInvalida"].ToString());
			}
		}

		public static void MostrarEstadoDeValidacionConfirmacion(TextBox textBoxCampo, TextBox textBoxConfirmarCampo)
		{
			if (textBoxCampo.Text == textBoxConfirmarCampo.Text)
			{
				textBoxConfirmarCampo.BorderBrush = Brushes.Green;
				OcultarToolTip(textBoxConfirmarCampo);
			}
			else
			{
				textBoxConfirmarCampo.BorderBrush = Brushes.Red;
				MostrarToolTip(textBoxConfirmarCampo, Application.Current.Resources["confirmacionInvalida"].ToString());
			}
		}

		public static void MostrarEstadoDeValidacionCorreoElectronico(TextBox textBoxCorreoElectronico)
		{
			if (ValidarCorreoElectronico(textBoxCorreoElectronico.Text))
			{
				textBoxCorreoElectronico.BorderBrush = Brushes.Green;
				OcultarToolTip(textBoxCorreoElectronico);
			}
			else
			{
				textBoxCorreoElectronico.BorderBrush = Brushes.Red;
				MostrarToolTip(textBoxCorreoElectronico, Application.Current.Resources["correoElectronicoInvalido"].ToString());
			}
		}

		public static void MostrarEstadoDeValidacionNombreDeUsuario(TextBox textBoxNombreDeUsuario)
		{
			if (ValidarNombreDeUsuario(textBoxNombreDeUsuario.Text))
			{
				textBoxNombreDeUsuario.BorderBrush = Brushes.Green;
				OcultarToolTip(textBoxNombreDeUsuario);
			}
			else
			{
				textBoxNombreDeUsuario.BorderBrush = Brushes.Red;
				MostrarToolTip(textBoxNombreDeUsuario, Application.Current.Resources["nombreDeUsuarioInvalido"].ToString());
			}
		}

		public static void OcultarPista(TextBox textBox, Label pista)
		{
			if (textBox.Text == string.Empty)
			{
				pista.Visibility = Visibility.Visible;
			}
			else
			{
				pista.Visibility = Visibility.Hidden;
			}
		}

		public static void OcultarPista(PasswordBox passwordBox, Label pista)
		{
			if (passwordBox.Password == string.Empty)
			{
				pista.Visibility = Visibility.Visible;
			}
			else
			{
				pista.Visibility = Visibility.Hidden;
			}
		}

		private static void MostrarToolTip(Control controlGrafico, string mensaje)
		{
			if (controlGrafico.ToolTip == null)
			{
				controlGrafico.ToolTip = new ToolTip()
				{
					Content = mensaje,
					Placement = System.Windows.Controls.Primitives.PlacementMode.Right,
				};
			}

			((ToolTip)controlGrafico.ToolTip).IsEnabled = true;
			ToolTipService.SetPlacementTarget((ToolTip)controlGrafico.ToolTip, controlGrafico);
			((ToolTip)controlGrafico.ToolTip).IsOpen = true;
		}

		private static void OcultarToolTip(Control controlGrafico)
		{
			if (controlGrafico.ToolTip != null)
			{
				((ToolTip)controlGrafico.ToolTip).IsOpen = false;
				((ToolTip)controlGrafico.ToolTip).IsEnabled = false;
				controlGrafico.ToolTip = null;
			}
		}
	}
}
