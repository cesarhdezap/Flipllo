using LogicaDeNegocios.ClasesDeDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogicaDeNegocios.Servicios.ServiciosDeLogicaDeJuego;
using System.Windows;

namespace LogicaDeNegocios.InteligenciaArtificial
{
	public static class InteligenciaArtificial
	{
		private static readonly int PROFUNDIDAD = 5;
		private static readonly int ZERO = 0;
		public static Point MejorMovimiento(Juego juego)
		{
			Point mejorMovimiento;
			Nodo raiz = new Nodo(juego.Clonar());
			int valorDeMejorMovimiento = ZERO;
			valorDeMejorMovimiento = raiz.Max(PROFUNDIDAD, juego.ColorDeJugadorActual);
			mejorMovimiento = raiz.Hijos.First(hijo => hijo.PuntuacionActual == valorDeMejorMovimiento).PosicionDeTirada;
			return mejorMovimiento;
		}	
	}

	public class Nodo
	{
		private static readonly int TAMAÑO_DE_TABLERO = 8;
		public List<Nodo> Hijos = new List<Nodo>();
		private Juego Juego;
		private bool Termino;
		public Point PosicionDeTirada; 
		public int PuntuacionActual;


		public Nodo(Juego juego)
		{
			Juego = juego;
			Termino = juego.JuegoTerminado;
		}

		
		private int EvaluarNodo(ColorDeFicha colorDeJugador)
		{
			int cuentaDeFichas = Juego.ObtenerCuentaDeFichas(colorDeJugador);
			PuntuacionActual = cuentaDeFichas;
			return cuentaDeFichas;
		}

		private void ExtenderArbol()
		{
			for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
			{
				for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
				{
					if (Juego.MovimientosLegales[i, j])
					{
						Point puntoDeTirada = new Point(i, j);
						Nodo nuevoNodo = new Nodo(Juego.Clonar())
						{
							PosicionDeTirada = puntoDeTirada
						};
						nuevoNodo.Juego.Tirar(puntoDeTirada);
						Hijos.Add(nuevoNodo);
					}
				}
			}
		}

		public int Max(int profundidad, ColorDeFicha colorMaximizando)
		{
			if (profundidad <= 0 || Termino)
			{
				return EvaluarNodo(colorMaximizando);
			}
			else
			{
				ExtenderArbol();
				PuntuacionActual = EvaluarNodo(colorMaximizando);
				foreach(Nodo hijo in Hijos)
				{
					PuntuacionActual = Math.Max(PuntuacionActual, hijo.Max(--profundidad, colorMaximizando));
				}
				return PuntuacionActual;
			}
		}
	}
}
