using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using System.Windows;

namespace LogicaDeNegocios
{
	/// <summary>
	/// Ficha dentro del tablero de Othello
	/// </summary>
	public class Ficha : ViewModelBase
	{
		/// <summary>
		/// Color actual de la ficha
		/// </summary>
		private ColorDeFicha colorActual;
		/// <summary>
		/// La posicion que la ficha tiene en el tablero
		/// </summary>
		private Point posicion;
		/// <summary>
		/// Indica si la ficha acaba de ser girada
		/// </summary>
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

		/// <summary>
		/// El color que la ficha tenia en el turno anterior
		/// </summary>
		public ColorDeFicha ColorAnterior { get; set; }

		/// <summary>
		/// Gira la ficha al corlor contrario del actual
		/// </summary>
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
	}
}