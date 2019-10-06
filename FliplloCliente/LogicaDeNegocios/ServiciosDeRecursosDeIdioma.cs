using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogicaDeNegocios.Excepciones;

namespace LogicaDeNegocios
{
	public static class ServiciosDeRecursosDeIdioma
	{
		public static void CambiarRecursos(string locale)
		{
			ResourceDictionary diccionarioDeRecursos = new ResourceDictionary();

			try
			{
				diccionarioDeRecursos.Source = new Uri("pack://application:,,,/Recursos_" + locale + ";component/Recursos.xaml", UriKind.Absolute);
			}
			catch (System.IO.FileNotFoundException e)
			{
				throw new RecursoNoEncontradoException("Resource " + locale + " not found.",e);
			}
			var recursoActual = Application.Current.Resources.MergedDictionaries.FirstOrDefault(recurso => recurso.Source.OriginalString.EndsWith("Recursos.xaml"));

			if (recursoActual != null)
			{
				Application.Current.Resources.MergedDictionaries.Remove(recursoActual);
			}

			Application.Current.Resources.MergedDictionaries.Add(diccionarioDeRecursos);
		}
	}
}
