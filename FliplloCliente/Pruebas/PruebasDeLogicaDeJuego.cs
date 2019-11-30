using LogicaDeNegocios;
using LogicaDeNegocios.ClasesDeDominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;

namespace Pruebas
{
	[TestClass]
	public class PruebasDeLogicaDeJuego
	{
		[TestMethod]
		public void PruebaDeCalcularMovimientosLegales_TableroBase()
		{
			bool[,] valorEsperado = new bool[8, 8];
			valorEsperado[3, 2] = true;
			valorEsperado[2, 3] = true;
			valorEsperado[5, 4] = true;
			valorEsperado[4, 5] = true;

			Juego juego = new Juego();

			juego.Tablero.PonerFicha(new Point(3, 3), ColorDeFicha.Blanco);
			juego.Tablero.PonerFicha(new Point(4, 4), ColorDeFicha.Blanco);
			juego.Tablero.PonerFicha(new Point(3, 4), ColorDeFicha.Negro);
			juego.Tablero.PonerFicha(new Point(4, 3), ColorDeFicha.Negro);

			juego.CalcularMovimientosLegales();
			bool[,] valorObtenido = juego.MovimientosLegales;

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					try
					{
						Assert.AreEqual(valorObtenido[i, j], valorEsperado[i, j]);
					}
					catch (AssertFailedException e)
					{
						throw new AssertFailedException(e.Message + System.Environment.NewLine + i.ToString() + j.ToString());
					}
				}
			}
		}

		//[TestMethod]
		//public void PruebaDeCalcularMovimientosLegales_TableroLleno()
		//{
		//	bool[,] valorEsperado = new bool[8, 8];


		//	Juego juego = new Juego();

		//	for (int i = 0; i < 8; i++)
		//	{
		//		for (int j = 0; j < 8; j++)
		//		{
		//			juego.Tablero[i, j] = new Ficha { ColorActual = ColorDeFicha.Blanco };
		//		}
		//	}

		//	bool[,] valorObtenido = juego.CalcularMovimientosLegales();

		//	for (int i = 0; i < 8; i++)
		//	{
		//		for (int j = 0; j < 8; j++)
		//		{
		//			try
		//			{

		//				Assert.AreEqual(valorObtenido[i, j], valorEsperado[i, j]);
		//			}
		//			catch (AssertFailedException e)
		//			{
		//				throw new AssertFailedException(e.Message + System.Environment.NewLine + i.ToString() + j.ToString());
		//			}
		//		}
		//	}
		//}
		//[TestMethod]
		//public void PruebaDeCalcularMovimientosLegales_TableroVacio()
		//{
		//	bool[,] valorEsperado = new bool[8, 8];


		//	Juego juego = new Juego();

		//	for (int i = 0; i < 8; i++)
		//	{
		//		for (int j = 0; j < 8; j++)
		//		{
		//			juego.Tablero[i, j] = new Ficha { ColorActual = ColorDeFicha.Ninguno };
		//		}
		//	}

		//	bool[,] valorObtenido = juego.CalcularMovimientosLegales();

		//	for (int i = 0; i < 8; i++)
		//	{
		//		for (int j = 0; j < 8; j++)
		//		{
		//			try
		//			{

		//				Assert.AreEqual(valorObtenido[i, j], valorEsperado[i, j]);
		//			}
		//			catch (AssertFailedException e)
		//			{
		//				throw new AssertFailedException(e.Message + System.Environment.NewLine + i.ToString() + j.ToString());
		//			}
		//		}
		//	}
		//}
		//[TestMethod]
		//public void PruebaDeCalcularMovimientosLegales_TableroConTodosLosMovimientosPosiblesEnElCentro_TiraBlanco()
		//{
		//	bool[,] valorEsperado = new bool[8, 8];

		//	valorEsperado[1, 1] = true;
		//	valorEsperado[3, 1] = true;
		//	valorEsperado[5, 1] = true;
		//	valorEsperado[1, 3] = true;
		//	valorEsperado[5, 3] = true;
		//	valorEsperado[1, 5] = true;
		//	valorEsperado[3, 5] = true;
		//	valorEsperado[5, 5] = true;

		//	Juego juego = new Juego
		//	{
		//		ColorDeJugadorActual = ColorDeFicha.Blanco
		//	};
		//	juego.Tablero[2, 2] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[3, 2] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[4, 2] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[2, 3] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[4, 3] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[2, 4] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[3, 4] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[4, 4] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[3, 3] = new Ficha() { ColorActual = ColorDeFicha.Blanco };

		//	bool[,] valorObtenido = juego.CalcularMovimientosLegales();

		//	for (int i = 0; i < 8; i++)
		//	{
		//		for (int j = 0; j < 8; j++)
		//		{
		//			try
		//			{

		//				Assert.AreEqual(valorObtenido[i, j], valorEsperado[i, j]);
		//			}
		//			catch (AssertFailedException e)
		//			{
		//				throw new AssertFailedException(e.Message + System.Environment.NewLine + i.ToString() + j.ToString());
		//			}
		//		}
		//	}
		//}

		//[TestMethod]
		//public void PruebaDeCalcularMovimientosLegales_TableroConTodosLosMovimientosPosiblesEnElCentroTiraNegro()
		//{
		//	bool[,] valorEsperado = new bool[8, 8];

		//	Juego juego = new Juego
		//	{
		//		ColorDeJugadorActual = ColorDeFicha.Negro
		//	};
		//	juego.Tablero[2, 2] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[3, 2] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[4, 2] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[2, 3] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[4, 3] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[2, 4] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[3, 4] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[4, 4] = new Ficha() { ColorActual = ColorDeFicha.Negro };
		//	juego.Tablero[3, 3] = new Ficha() { ColorActual = ColorDeFicha.Blanco };

		//	bool[,] valorObtenido = juego.CalcularMovimientosLegales();

		//	for (int i = 0; i < 8; i++)
		//	{
		//		for (int j = 0; j < 8; j++)
		//		{
		//			try
		//			{

		//				Assert.AreEqual(valorObtenido[i, j], valorEsperado[i, j]);
		//			}
		//			catch (AssertFailedException e)
		//			{
		//				throw new AssertFailedException(e.Message + System.Environment.NewLine + i.ToString() + j.ToString());
		//			}
		//		}
		//	}
		//}
	}
}