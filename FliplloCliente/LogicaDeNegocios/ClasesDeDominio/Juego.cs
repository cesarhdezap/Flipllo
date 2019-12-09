using System.Collections.Generic;
using System.Windows;
using static LogicaDeNegocios.Servicios.ServiciosDeLogicaDeJuego;

namespace LogicaDeNegocios.ClasesDeDominio
{
	/// <summary>
	/// Clase con servicios para manejar un juego de Othello
	/// </summary>
	public class Juego
	{
		/// <summary>
		/// El color del jugador que tiene el turno actual
		/// </summary>
		public ColorDeFicha ColorDeJugadorActual { get; set; } = ColorDeFicha.Negro;

		/// <summary>
		/// Tamaño del tablero
		/// </summary>
		public const int TAMAÑO_DE_TABLERO = 8;

		/// <summary>
		/// El tablero del juego
		/// </summary>
		public Tablero Tablero { get; set; } = new Tablero();

		/// <summary>
		/// La cantidad de fichas blancas que tiene el tablero actualmente
		/// </summary>
		public int FichasBlancas { get; set; } = 2;

		/// <summary>
		/// La cantidad de fichas negras que tiene el tablero actualmente
		/// </summary>
		public int FichasNegras { get; set; } = 2;

		/// <summary>
		/// La puntuacion del jugador negro
		/// </summary>
		public int PuntosNegros { get; set; } = 0;

		/// <summary>
		/// La puntuacion del jugador blanco
		/// </summary>
		public int PuntosBlancos { get; set; } = 0;

		/// <summary>
		/// El tipo del juego
		/// </summary>
		public TipoDeJuego TipoDeJuego { get; set; }	

		/// <summary>
		/// Matriz bidimencional que representa el tablero, una posicion es un movimiento legal si es verdadera
		/// </summary>
		public bool[,] MovimientosLegales { get; set; } = new bool[TAMAÑO_DE_TABLERO, TAMAÑO_DE_TABLERO];

		/// <summary>
		/// Piezas que se deben girar a resultado del ultimo movimiento realizado
		/// </summary>
		private bool[,] PiezasAGirar { get; set; } = new bool[TAMAÑO_DE_TABLERO, TAMAÑO_DE_TABLERO];

		/// <summary>
		/// Indica si el juego acabo
		/// </summary>
		public bool JuegoTerminado { get; set; } = false;

		/// <summary>
		/// Clona el objeto juego actual por valores
		/// </summary>
		/// <returns>Clon profundo del objeto juego original</returns>
		public Juego Clonar()
		{
			return new Juego
			{
				Tablero = this.Tablero.Clonar(),
				FichasBlancas = this.FichasBlancas,
				FichasNegras = this.FichasNegras,
				ColorDeJugadorActual = this.ColorDeJugadorActual,
				MovimientosLegales = this.MovimientosLegales,
				PiezasAGirar = this.PiezasAGirar,
				JuegoTerminado = this.JuegoTerminado
			};
		}

		public Juego()
		{
			CalcularMovimientosLegales();
			ContarFichas();
			Termino();
			CalcularPuntos();
		}

		/// <summary>
		/// Calcula los movimientos legales para el siguiente movimiento
		/// </summary>
		private void CalcularMovimientosLegales()
		{
			MovimientosLegales = Tablero.CalcularMovimientosLegales(ColorDeJugadorActual);
		}

		/// <summary>
		/// Calcula las piezas a como resultado del ultimo movimiento
		/// </summary>
		/// <param name="tirada">La posicion donde se tiro</param>
		/// <param name="colorDeJugadorTirando">El color del jugador que tiro</param>
		/// <returns></returns>
		private bool[,] CalcularPiezasAGirar(Point tirada, ColorDeFicha colorDeJugadorTirando)
		{
			bool[,] piezas = new bool[TAMAÑO_DE_TABLERO, TAMAÑO_DE_TABLERO];

			Point incremento = new Point(1, 0);

			incremento = AsignarDireccion(incremento, Direccion.Abajo);
			piezas = SumarTableros(piezas, SeDebeGirar(tirada, incremento, colorDeJugadorTirando));
			incremento = AsignarDireccion(incremento, Direccion.AbajoDerecha);
			piezas = SumarTableros(piezas, SeDebeGirar(tirada, incremento, colorDeJugadorTirando));
			incremento = AsignarDireccion(incremento, Direccion.AbajoIzquierda);
			piezas = SumarTableros(piezas, SeDebeGirar(tirada, incremento, colorDeJugadorTirando));
			incremento = AsignarDireccion(incremento, Direccion.Arriba);
			piezas = SumarTableros(piezas, SeDebeGirar(tirada, incremento, colorDeJugadorTirando));
			incremento = AsignarDireccion(incremento, Direccion.ArribaDerecha);
			piezas = SumarTableros(piezas, SeDebeGirar(tirada, incremento, colorDeJugadorTirando));
			incremento = AsignarDireccion(incremento, Direccion.ArribaIzquierda);
			piezas = SumarTableros(piezas, SeDebeGirar(tirada, incremento, colorDeJugadorTirando));
			incremento = AsignarDireccion(incremento, Direccion.Derecha);
			piezas = SumarTableros(piezas, SeDebeGirar(tirada, incremento, colorDeJugadorTirando));
			incremento = AsignarDireccion(incremento, Direccion.Izquierda);
			piezas = SumarTableros(piezas, SeDebeGirar(tirada, incremento, colorDeJugadorTirando));

			return piezas;
		}

		/// <summary>
		/// Indica si una ficha en especifico se debe a resultado de una tirada
		/// </summary>
		/// <param name="tirada">La posicion de la tirada</param>
		/// <param name="incremento">El incremento actual</param>
		/// <param name="colorDeJugadorTirando">El color ddel jugador que realizola tirada</param>
		/// <returns></returns>
		private bool[,] SeDebeGirar(Point tirada, Point incremento, ColorDeFicha colorDeJugadorTirando)
		{
			bool finalEncontrado = false;
			bool hayPiezasIntermedias = false;
			bool haySaltos = false;
			bool[,] piezasAGirar = new bool[TAMAÑO_DE_TABLERO, TAMAÑO_DE_TABLERO];
			bool[,] piezasMarcadas = new bool[TAMAÑO_DE_TABLERO, TAMAÑO_DE_TABLERO];
			Point posicionActual = new Point(tirada.X, tirada.Y);
			int i = 0;
			while (i <= TAMAÑO_DE_TABLERO && !finalEncontrado)
			{
				i++;
				posicionActual.X += incremento.X;
				posicionActual.Y += incremento.Y;
				bool casillaDentroDeTablero = Tablero.EsCasillaDentroDeTablero(posicionActual);

				if (casillaDentroDeTablero)
				{
					ColorDeFicha colorDeCasillaActual = Tablero.GetFicha(posicionActual).ColorActual;
					if (colorDeCasillaActual == ColorDeFicha.Ninguno)
					{
						haySaltos = true;
					}
					else if (colorDeCasillaActual == ColorContrario(colorDeJugadorTirando))
					{
						piezasMarcadas[(int)posicionActual.X, (int)posicionActual.Y] = true;
						hayPiezasIntermedias = true;
					}
					else if (colorDeCasillaActual == colorDeJugadorTirando)
					{
						finalEncontrado = true;
					}
				}
			}

			if (finalEncontrado && hayPiezasIntermedias && !haySaltos)
			{
				piezasAGirar = piezasMarcadas;
			}

			return piezasAGirar;
		}

		/// <summary>
		/// Cuenta la cantidad de fichas de cada color del juego
		/// </summary>
		private void ContarFichas()
		{
			FichasBlancas = Tablero.ContarFichas(ColorDeFicha.Blanco);
			FichasNegras = Tablero.ContarFichas(ColorDeFicha.Negro);
		}

		/// <summary>
		/// Determina si el juego ha terminado
		/// </summary>
		private void Termino()
		{
			JuegoTerminado = true;

			MovimientosLegales = Tablero.CalcularMovimientosLegales(ColorDeJugadorActual);
			for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
			{
				for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
				{
					if (MovimientosLegales[i, j])
					{
						JuegoTerminado = false;
					}
				}
			}
		}

		/// <summary>
		/// Determina si una tirada es valida
		/// </summary>
		/// <param name="tirada">La posicion de la tirada</param>
		/// <returns>Representa si la tirada es valida o no</returns>
		public bool SePuedeTirar(Point tirada)
		{
			bool resultado = false;

			if (Tablero.EsMovimientoLegal(tirada, ColorDeJugadorActual))
			{
				resultado = true;
			}

			return resultado;
		}

		/// <summary>
		/// Hace una tirada con el color de jugador actual del juego en la posicion especificada
		/// </summary>
		/// <param name="tirada">La posicion en donde  tirar</param>
		public void Tirar(Point tirada)
		{
			Tablero.LimpiarGiros();

			if (Tablero.EsMovimientoLegal(tirada, ColorDeJugadorActual))
			{
				Tablero.PonerFicha(tirada, ColorDeJugadorActual);
				PiezasAGirar = CalcularPiezasAGirar(tirada, ColorDeJugadorActual);


				for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
				{
					for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
					{
						if (PiezasAGirar[i, j])
						{
							Tablero.Girar(i, j);
						}
					}
				}

				CalcularPuntos();
				ColorDeJugadorActual = ColorContrario(ColorDeJugadorActual);
				Termino();
				CalcularMovimientosLegales();
			}

			ContarFichas();
		}

		/// <summary>
		/// Calcula la cantidad de puntos total de ambos jugadores
		/// </summary>
		private void CalcularPuntos()
		{
			int cuentaDePuntos = 0;
			float multiplicadorDePuntos = 1;

			for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
			{
				for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
				{
					if (PiezasAGirar[i, j])
					{
						multiplicadorDePuntos += (float)0.3;
						cuentaDePuntos++;
					}
				}
			}

			int puntosASumar = (int)((float)cuentaDePuntos * multiplicadorDePuntos) * 10;

			if (ColorDeJugadorActual == ColorDeFicha.Blanco)
			{
				PuntosBlancos += puntosASumar;
			}
			else
			{
				PuntosNegros += puntosASumar;
			}
		}

		/// <summary>
		/// Obtiene las fichas del tablero como una lista
		/// </summary>
		/// <returns>Lista de fichas del tablero</returns>
		public List<Ficha> ObtenerFichasComoLista()
		{
			List<Ficha> listaDeFichas = new List<Ficha>();

			for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
			{
				for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
				{
					Ficha fichaActual = Tablero.GetFicha(i, j);

					if (fichaActual.ColorActual != ColorDeFicha.Ninguno)
					{
						fichaActual.Posicion = new Point(i, j);
						listaDeFichas.Add(fichaActual);
					}
				}
			}

			return listaDeFichas;
		}

		/// <summary>
		/// Obtiene la cantidad de fichas del color especificado
		/// </summary>
		/// <param name="colorDeFicha">Color de ficha del que se desea conocer la cantidad de fichas</param>
		/// <returns>La cantidad de fichas del color especificado</returns>
		public int ObtenerCuentaDeFichas(ColorDeFicha colorDeFicha)
		{
			int cuenta = 0;

			if (colorDeFicha == ColorDeFicha.Blanco)
			{
				cuenta = FichasBlancas;
			}
			if (colorDeFicha == ColorDeFicha.Negro)
			{
				cuenta = FichasNegras;
			}

			return cuenta;
		}
	}
}
