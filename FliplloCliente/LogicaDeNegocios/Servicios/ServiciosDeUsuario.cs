using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.Servicios
{
	public static class ServiciosDeUsuario
	{
		/// <summary>
		/// La experiencia por cada nivel necesario
		/// </summary>
		private const int EXPERIENCIA_POR_NIVEL = 1000;

		/// <summary>
		/// Calcula el nivel de un jugador tomando en cuenta su experiencia
		/// </summary>
		/// <param name="Experiencia">La experiencia del jugador</param>
		/// <returns>El nivel del jugador</returns>
		public static int CalcularNivel(double Experiencia)
		{
			int nivel = (int)Experiencia / EXPERIENCIA_POR_NIVEL;
			return nivel;
		}
	}
}
