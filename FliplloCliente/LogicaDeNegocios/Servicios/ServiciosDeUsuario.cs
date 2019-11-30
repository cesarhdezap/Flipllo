using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.Servicios
{
	public static class ServiciosDeUsuario
	{
		private const int EXPERIENCIA_POR_NIVEL = 1000;

		public static int CalcularNivel(double Experiencia)
		{
			int nivel = (int)Experiencia / EXPERIENCIA_POR_NIVEL;
			return nivel;
		}
	}
}
