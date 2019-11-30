using System.Collections.Generic;
using System.Windows;
using static LogicaDeNegocios.Servicios.ServiciosDeLogicaDeJuego;

namespace LogicaDeNegocios.ClasesDeDominio
{
	public class Juego
	{
		public ColorDeFicha ColorDeJugadorActual { get; set; } = ColorDeFicha.Negro;
		public const int TAMAÑO_DE_TABLERO = 8;
		public Tablero Tablero { get; set; } = new Tablero();
		public int FichasBlancas { get; set; } = 0;
		public int FichasNegras { get; set; } = 0;
		public int PuntosNegros { get; set; } = 0;
		public int PuntosBlancos { get; set; } = 0;
		public bool[,] MovimientosLegales { get; set; } = new bool[TAMAÑO_DE_TABLERO, TAMAÑO_DE_TABLERO];
		private bool[,] PiezasAGirar { get; set; } = new bool[TAMAÑO_DE_TABLERO, TAMAÑO_DE_TABLERO];
		public bool JuegoTerminado { get; set; } = false;

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

		private void CalcularMovimientosLegales()
		{
			MovimientosLegales = Tablero.CalcularMovimientosLegales(ColorDeJugadorActual);
		}

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

		private void ContarFichas()
		{
			FichasBlancas = Tablero.ContarFichas(ColorDeFicha.Blanco);
			FichasNegras = Tablero.ContarFichas(ColorDeFicha.Negro);
		}

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

		public bool SePuedeTirar(Point tirada)
		{
			bool resultado = false;

			if (Tablero.EsMovimientoLegal(tirada, ColorDeJugadorActual))
			{
				resultado = true;
			}

			return resultado;
		}

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

			int puntosASumar = (int)((float)cuentaDePuntos * multiplicadorDePuntos)*10;

			if (ColorDeJugadorActual == ColorDeFicha.Blanco)
			{
				PuntosBlancos += puntosASumar;
			}
			else
			{
				PuntosNegros += puntosASumar;
			}
		}

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
