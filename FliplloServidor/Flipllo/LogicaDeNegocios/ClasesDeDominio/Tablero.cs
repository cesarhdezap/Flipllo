using System.Windows;
using static LogicaDeNegocios.Servicios.ServiciosDeLogicaDeJuego;
namespace LogicaDeNegocios.ClasesDeDominio
{
    public class Tablero
    {
        public const int TAMAÑO_DE_TABLERO = 8;
        private Ficha[,] Fichas { get; set; } = new Ficha[TAMAÑO_DE_TABLERO, TAMAÑO_DE_TABLERO];

        public Tablero()
        {
            for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
            {
                for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
                {
                    Fichas[i, j] = new Ficha
                    {
                        ColorActual = ColorDeFicha.Ninguno
                    };
                }
            }
            Fichas[3, 3].ColorActual = ColorDeFicha.Blanco;
            Fichas[4, 4].ColorActual = ColorDeFicha.Blanco;
            Fichas[4, 3].ColorActual = ColorDeFicha.Negro;
            Fichas[3, 4].ColorActual = ColorDeFicha.Negro;
        }

        public Tablero Clonar()
        {
            Tablero tableroClonado = new Tablero();
            Ficha[,] fichasClonadas = new Ficha[TAMAÑO_DE_TABLERO, TAMAÑO_DE_TABLERO];

            for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
            {
                for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
                {
                    fichasClonadas[i, j] = new Ficha
                    {
                        ColorActual = Fichas[i, j].ColorActual,
                        ColorAnterior = Fichas[i, j].ColorAnterior,
                        FueGirada = Fichas[i, j].FueGirada
                    };
                }
            }

            tableroClonado.Fichas = fichasClonadas;

            return tableroClonado;
        }

        public bool[,] CalcularMovimientosLegales(ColorDeFicha colorDeFicha)
        {
            bool[,] movimientos = new bool[TAMAÑO_DE_TABLERO, TAMAÑO_DE_TABLERO];

            Point posicion = new Point();

            for (posicion.X = 0; posicion.X < TAMAÑO_DE_TABLERO; posicion.X++)
            {
                for (posicion.Y = 0; posicion.Y < TAMAÑO_DE_TABLERO; posicion.Y++)
                {
                    if (EsMovimientoLegal(posicion, colorDeFicha))
                    {
                        movimientos[(int)posicion.X, (int)posicion.Y] = true;
                    }
                    else
                    {
                        movimientos[(int)posicion.X, (int)posicion.Y] = false;
                    }
                }
            }

            return movimientos;
        }

        public int ContarFichas(ColorDeFicha colorDeFicha)
        {
            int cuenta = 0;

            for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
            {
                for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
                {
                    if (Fichas[i, j].ColorActual == colorDeFicha)
                    {
                        cuenta++;
                    }
                }
            }

            return cuenta;
        }

        public bool EsMovimientoLegal(Point posicion, ColorDeFicha colorDeJugadorTirando)
        {
            Point incremento = new Point(1, 0);
            bool resultadoDeValidacion = EsMovimientoLegalConOrientacion(posicion, colorDeJugadorTirando, incremento);
            if (!resultadoDeValidacion)
            {
                incremento = AsignarDireccion(incremento, Direccion.Abajo);
                resultadoDeValidacion = EsMovimientoLegalConOrientacion(posicion, colorDeJugadorTirando, incremento);
            }
            if (!resultadoDeValidacion)
            {
                incremento = AsignarDireccion(incremento, Direccion.AbajoDerecha);
                resultadoDeValidacion = EsMovimientoLegalConOrientacion(posicion, colorDeJugadorTirando, incremento);
            }
            if (!resultadoDeValidacion)
            {
                incremento = AsignarDireccion(incremento, Direccion.AbajoIzquierda);
                resultadoDeValidacion = EsMovimientoLegalConOrientacion(posicion, colorDeJugadorTirando, incremento);
            }
            if (!resultadoDeValidacion)
            {
                incremento = AsignarDireccion(incremento, Direccion.Arriba);
                resultadoDeValidacion = EsMovimientoLegalConOrientacion(posicion, colorDeJugadorTirando, incremento);
            }
            if (!resultadoDeValidacion)
            {
                incremento = AsignarDireccion(incremento, Direccion.ArribaDerecha);
                resultadoDeValidacion = EsMovimientoLegalConOrientacion(posicion, colorDeJugadorTirando, incremento);
            }
            if (!resultadoDeValidacion)
            {
                incremento = AsignarDireccion(incremento, Direccion.ArribaIzquierda);
                resultadoDeValidacion = EsMovimientoLegalConOrientacion(posicion, colorDeJugadorTirando, incremento);
            }
            if (!resultadoDeValidacion)
            {
                incremento = AsignarDireccion(incremento, Direccion.Derecha);
                resultadoDeValidacion = EsMovimientoLegalConOrientacion(posicion, colorDeJugadorTirando, incremento);
            }
            if (!resultadoDeValidacion)
            {
                incremento = AsignarDireccion(incremento, Direccion.Izquierda);
                resultadoDeValidacion = EsMovimientoLegalConOrientacion(posicion, colorDeJugadorTirando, incremento);
            }

            return resultadoDeValidacion;
        }

        private bool EsMovimientoValido(Point punto)
        {
            bool resultadoDeValidacion = false;

            if (EsCasillaDentroDeTablero(punto))
            {
                if (Fichas[(int)punto.X, (int)punto.Y].ColorActual == ColorDeFicha.Ninguno)
                {
                    resultadoDeValidacion = true;
                }
            }

            return resultadoDeValidacion;
        }

        public bool EsMovimientoLegalConOrientacion(Point posicion, ColorDeFicha colorDeJugadorTirando, Point incremento)
        {
            Point posicionActual = new Point(posicion.X, posicion.Y);
            bool movimientoPosible = false;
            if (EsMovimientoValido(posicionActual))
            {
                movimientoPosible = true;
            }
            bool hayPiezasIntermedias = false;
            bool movimientoEncontrado = false;
            int i = 0;
            while (i <= TAMAÑO_DE_TABLERO && !movimientoEncontrado && movimientoPosible)
            {
                i++;
                posicionActual.X += incremento.X;
                posicionActual.Y += incremento.Y;
                if (EsCasillaDentroDeTablero(posicionActual))
                {
                    ColorDeFicha colorCasillaActual = Fichas[(int)posicionActual.X, (int)posicionActual.Y].ColorActual;
                    if (colorCasillaActual == ColorContrario(colorDeJugadorTirando))
                    {
                        hayPiezasIntermedias = true;
                    }
                    else
                    {
                        if (hayPiezasIntermedias && colorCasillaActual == colorDeJugadorTirando)
                        {
                            movimientoEncontrado = true;
                        }
                        else
                        {
                            movimientoPosible = false;
                        }
                    }
                }
                else
                {
                    movimientoPosible = false;
                }
            }
            return movimientoEncontrado;
        }

        public bool EsCasillaDentroDeTablero(Point punto)
        {
            bool resultadoDeValidacion = false;

            if ((int)punto.X < TAMAÑO_DE_TABLERO && (int)punto.X >= 0)
            {
                if ((int)punto.Y < TAMAÑO_DE_TABLERO && (int)punto.Y >= 0)
                {
                    resultadoDeValidacion = true;
                }
            }

            return resultadoDeValidacion;
        }

        public void PonerFicha(Point punto, ColorDeFicha colorDeJugador)
        {
            Ficha fichaTirada = new Ficha()
            {
                ColorActual = colorDeJugador,
                Posicion = punto
            };

            Fichas[(int)punto.X, (int)punto.Y] = fichaTirada;
        }

        public void Girar(Point punto)
        {
            Fichas[(int)punto.X, (int)punto.Y].Girar();
            Fichas[(int)punto.X, (int)punto.Y].FueGirada = true;
        }

        public void LimpiarGiros()
        {
            for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
            {
                for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
                {
                    Fichas[i, j].FueGirada = false;
                }
            }
        }

        public void Girar(int x, int y)
        {
            Fichas[x, y].Girar();
            Fichas[x, y].FueGirada = true;
        }

        public Ficha GetFicha(Point punto)
        {
            return Fichas[(int)punto.X, (int)punto.Y];
        }

        public Ficha GetFicha(int x, int y)
        {
            return Fichas[x, y];
        }
    }
}

public enum ColorDeFicha
{
    Negro,
    Blanco,
    Ninguno,
}