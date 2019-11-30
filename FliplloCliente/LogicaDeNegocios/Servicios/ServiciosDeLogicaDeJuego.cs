using LogicaDeNegocios.ClasesDeDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogicaDeNegocios.Servicios
{
	public static class ServiciosDeLogicaDeJuego
	{
		public static Point AsignarDireccion(Point incremento, Direccion direccion)
		{
			if (direccion == Direccion.Arriba)
			{
				incremento.X = 0;
				incremento.Y = 1;
			}
			else if (direccion == Direccion.Derecha)
			{
				incremento.X = 1;
				incremento.Y = 0;
			}
			else if (direccion == Direccion.Abajo)
			{
				incremento.X = 0;
				incremento.Y = -1;
			}
			else if (direccion == Direccion.Izquierda)
			{
				incremento.X = -1;
				incremento.Y = 0;
			}
			else if (direccion == Direccion.ArribaDerecha)
			{
				incremento.X = 1;
				incremento.Y = 1;
			}
			else if (direccion == Direccion.AbajoDerecha)
			{
				incremento.X = 1;
				incremento.Y = -1;
			}
			else if (direccion == Direccion.AbajoIzquierda)
			{
				incremento.X = -1;
				incremento.Y = -1;
			}
			else if (direccion == Direccion.ArribaIzquierda)
			{
				incremento.X = -1;
				incremento.Y = 1;
			}

			return incremento;
		}


		public static ColorDeFicha ColorContrario(ColorDeFicha colorDeJugador)
		{
			ColorDeFicha resultado = ColorDeFicha.Ninguno;
			if (colorDeJugador == ColorDeFicha.Blanco)
			{
				resultado = ColorDeFicha.Negro;
			}
			else if (colorDeJugador == ColorDeFicha.Negro)
			{
				resultado = ColorDeFicha.Blanco;
			}
			return resultado;
		}

		public static bool[,] SumarTableros(bool[,] tableroBase, bool[,] tableroASumar)
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (tableroASumar[i, j])
					{
						tableroBase[i, j] = true;
					}
				}
			}

			return tableroBase;
		}

	}
}
