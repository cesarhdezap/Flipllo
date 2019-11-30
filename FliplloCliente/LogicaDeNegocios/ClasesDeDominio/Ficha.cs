using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using System.Windows;

namespace LogicaDeNegocios
{
	public class Ficha : ViewModelBase
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
			set { this.posicion = value; RaisePropertyChanged(() => this.Posicion); }
		}
		public ColorDeFicha ColorActual
		{
			get { return colorActual; }
			set { this.ColorAnterior = this.colorActual; this.colorActual = value; RaisePropertyChanged(() => this.ColorActual); }
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