using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogicaDeNegocios.Excepciones;

namespace LogicaDeNegocios
{
	public static class ServiciosDeRecursos
	{
		public static void CambiarRecursoDeIdioma(string locale)
		{
			ResourceDictionary diccionarioDeRecursos = new ResourceDictionary();

			try
			{
				diccionarioDeRecursos.Source = new Uri("pack://application:,,,/Recursos_" + locale + ";component/Recursos.xaml", UriKind.Absolute);
			}
			catch (System.IO.IOException e)
			{
				throw new RecursoNoEncontradoException("Resource " + locale + " not found.", e);
			}
			var recursoActual = Application.Current.Resources.MergedDictionaries.FirstOrDefault(recurso => recurso.Source.OriginalString.EndsWith("Recursos.xaml"));

			if (recursoActual != null)
			{
				Application.Current.Resources.MergedDictionaries.Remove(recursoActual);
			}


			Application.Current.Resources.MergedDictionaries.Add(diccionarioDeRecursos);
		}

		public static void CambiarSkin(string nombreDeSkin, ColorDeFicha colorDeSkin)
		{
			ResourceDictionary diccionarioDeRecursos = new ResourceDictionary();

			try
			{
				diccionarioDeRecursos.Source = new Uri("pack://application:,,,/RecursosGraficos;component/" + nombreDeSkin + "/" + colorDeSkin.ToString() + ".xaml", UriKind.Absolute);
			}
			catch (System.IO.IOException e)
			{
				throw new RecursoNoEncontradoException("Resource " + nombreDeSkin + " not found.", e);
			}
			var recursoActual = Application.Current.Resources.MergedDictionaries.FirstOrDefault(recurso => recurso.Source.OriginalString.EndsWith(colorDeSkin.ToString() + ".xaml"));

			if (recursoActual != null)
			{
				Application.Current.Resources.MergedDictionaries.Remove(recursoActual);
			}

			Application.Current.Resources.MergedDictionaries.Add(diccionarioDeRecursos);
		}

		public static void CargarRecursosGraficosPorDefecto()
		{
			ResourceDictionary estructurasDeTableros = new ResourceDictionary
			{
				Source = new Uri("pack://application:,,,/RecursosGraficos;component/EstructurasDeTablero.xaml", UriKind.Absolute)
			};
			ResourceDictionary tableroPorDefecto = new ResourceDictionary
			{
				Source = new Uri("pack://application:,,,/RecursosGraficos;component/TableroPorDefecto.xaml", UriKind.Absolute)
			};
			ResourceDictionary skinPorDefectoBlanco = new ResourceDictionary
			{
				Source = new Uri("pack://application:,,,/RecursosGraficos;component/Alana/Blanco.xaml", UriKind.Absolute)
			};
			ResourceDictionary skinPorDefectoNegro = new ResourceDictionary
			{
				Source = new Uri("pack://application:,,,/RecursosGraficos;component/Alana/Negro.xaml", UriKind.Absolute)
			};

			Application.Current.Resources.MergedDictionaries.Add(estructurasDeTableros);
			Application.Current.Resources.MergedDictionaries.Add(tableroPorDefecto);
			Application.Current.Resources.MergedDictionaries.Add(skinPorDefectoBlanco);
			Application.Current.Resources.MergedDictionaries.Add(skinPorDefectoNegro);
		}

		public static List<string> ListarRecursosDeIdioma()
		{
			const string identificadorDeRecursosDeIdioma = "Recursos_";
			string[] recursosDeAplicacion = Directory.GetDirectories("pack://application:,,,/");
			List<string> listaDeRecursosDeAplicacion = new List<string>();
			listaDeRecursosDeAplicacion = recursosDeAplicacion.ToList();

			List<string> listaDeRecursosDeIdioma = recursosDeAplicacion.Where(recurso => recurso.Contains(identificadorDeRecursosDeIdioma)).ToList();
			foreach (string nombreDeRecurso in listaDeRecursosDeIdioma)
			{
				foreach (char caracter in identificadorDeRecursosDeIdioma)
				{
					nombreDeRecurso.TrimStart(caracter);
				}
			}

			return listaDeRecursosDeIdioma;
		}
	}
}
