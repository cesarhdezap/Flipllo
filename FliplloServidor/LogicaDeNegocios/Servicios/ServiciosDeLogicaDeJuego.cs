using System.Windows;

namespace LogicaDeNegocios.Servicios
{
    public static class ServiciosDeLogicaDeJuego
    {
        private static readonly int TAMAÑO_DE_TABLERO = 8;
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

        public enum Direccion
        {
            Arriba,
            Derecha,
            Izquierda,
            Abajo,
            ArribaDerecha,
            AbajoDerecha,
            AbajoIzquierda,
            ArribaIzquierda
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
            for (int i = 0; i < TAMAÑO_DE_TABLERO; i++)
            {
                for (int j = 0; j < TAMAÑO_DE_TABLERO; j++)
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