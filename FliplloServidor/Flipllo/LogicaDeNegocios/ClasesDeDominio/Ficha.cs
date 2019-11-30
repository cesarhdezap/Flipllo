using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogicaDeNegocios.ClasesDeDominio
{
    public class Ficha
    {
        private ColorDeFicha colorActual;
        private Point posicion;
        private bool fueGirada;

        public bool FueGirada
        {
            get { return this.fueGirada; }
            set { this.fueGirada = value; }
        }
        public Point Posicion
        {
            get { return this.posicion; }
            set { this.posicion = value; }
        }
        public ColorDeFicha ColorActual
        {
            get { return ColorActual; }
            set { this.ColorAnterior = this.ColorActual; this.ColorActual = value; }
        }
        public ColorDeFicha ColorAnterior { get; set; }

        public void Girar()
        {
            if (ColorActual == ColorDeFicha.Blanco)
            {
                ColorActual = ColorDeFicha.Negro;
                ColorAnterior = ColorDeFicha.Blanco;
            }
            else
            {
                ColorActual = ColorDeFicha.Blanco;
                ColorAnterior = ColorDeFicha.Negro;
            }
        }

        public void RevertirColor()
        {
            ColorActual = ColorAnterior;
        }
    }
}