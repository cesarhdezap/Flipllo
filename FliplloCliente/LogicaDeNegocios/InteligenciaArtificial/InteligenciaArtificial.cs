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
	/// <summary>
	/// Los servicios de la inteligencia artifical del juego
	/// </summary>
	public static class InteligenciaArtificial
	{
		/// <summary>
		/// La profundidad de busqueda por defecto del algoritmo
		/// </summary>
		private static readonly int PROFUNDIDAD = 5;

		/// <summary>
		/// El numero 0
		/// </summary>
		private static readonly int ZERO = 0;

		/// <summary>
		/// Calcula el mejor movimiento posible en el juego especificado
		/// </summary>
		/// <param name="juego">El juego</param>
		/// <returns>El mejor movimiento posible</returns>
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

	/// <summary>
	/// Un nodo para el algoritmo de busqueda de la inteligencia artifical
	/// </summary>
	public class Nodo
	{
		/// <summary>
		/// EL tamaño del tablero
		/// </summary>
		private static readonly int TAMAÑO_DE_TABLERO = 8;

		/// <summary>
		/// Una lista de los hijos del nodo
		/// </summary>
		public List<Nodo> Hijos = new List<Nodo>();

		/// <summary>
		/// El juego actual del nodo
		/// </summary>
		private Juego Juego;

		/// <summary>
		/// Indica si el juego del nodo termino o no
		/// </summary>
		private bool Termino;

		/// <summary>
		/// La posicion en la que se tiro para llegar a este nodo
		/// </summary>
		public Point PosicionDeTirada; 

		/// <summary>
		/// La puntuacion del nodo
		/// </summary>
		public int PuntuacionActual;


		public Nodo(Juego juego)
		{
			Juego = juego;
			Termino = juego.JuegoTerminado;
		}

		/// <summary>
		/// Calcula el valor de un nodo
		/// </summary>
		/// <param name="colorDeJugador">El color del jugador para el que evalua</param>
		/// <returns>El valor del nodo actual</returns>
		private int EvaluarNodo(ColorDeFicha colorDeJugador)
		{
			int cuentaDeFichas = Juego.ObtenerCuentaDeFichas(colorDeJugador);
			PuntuacionActual = cuentaDeFichas;
			return cuentaDeFichas;
		}

		/// <summary>
		/// Extiende el arbol  on todos los movimientos posibles del juego del nodo actual
		/// </summary>
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

		/// <summary>
		/// Calcula el resultado del algoritmo min-max recursivamente
		/// </summary>
		/// <param name="profundidad">La profundidad de busqueda actual</param>
		/// <param name="colorMaximizando">El color de jugador para el que se esta maximizando</param>
		/// <returns>El valor del mejor movimiento</returns>
		internal int Max(int profundidad, ColorDeFicha colorMaximizando)
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
