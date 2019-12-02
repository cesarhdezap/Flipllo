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
				string directorioDeLaAplicacion = Directory.GetCurrentDirectory();
				diccionarioDeRecursos.Source = new Uri(directorioDeLaAplicacion + "\\Recursos\\" + locale + ".xaml", UriKind.Absolute);
			}
			catch (System.Net.WebException e)
			{
				throw new RecursoNoEncontradoException("Resource " + locale + " not found.", e);
			}
			var recursoActual = Application.Current.Resources.MergedDictionaries.FirstOrDefault(recurso => recurso.Source.OriginalString.Contains("-"));

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
				throw new RecursoNoEncontradoException("Resource " + nombreDeSkin + colorDeSkin.ToString() + " not found.", e);
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
			const string carpetaDeRecursos = "Recursos";
			string directorioDeAplicacion = Directory.GetCurrentDirectory();
			string caminoACarpetaDeRecursos = directorioDeAplicacion + "\\" + carpetaDeRecursos;
			List<string> listaDeRecursosDeIdioma = Directory.GetFiles(caminoACarpetaDeRecursos).ToList();

			for (int i = 0; i<listaDeRecursosDeIdioma.Count;i++)
			{
				listaDeRecursosDeIdioma[i] = listaDeRecursosDeIdioma[i].Remove(0, caminoACarpetaDeRecursos.Length + 1);
				listaDeRecursosDeIdioma[i] = listaDeRecursosDeIdioma[i].Remove(listaDeRecursosDeIdioma[i].IndexOf("."), 5);
			}

			return listaDeRecursosDeIdioma;
		}
	}
}
