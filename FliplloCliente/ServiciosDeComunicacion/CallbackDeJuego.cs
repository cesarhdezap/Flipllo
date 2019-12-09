//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using LogicaDeNegocios.ServiciosDeJuego;

//namespace ServiciosDeComunicacion
//{
//	public class CallbackDeJuego : IServiciosDeJuegoCallback
//	{
//		public delegate void RecibirTableroDelegate(List<Ficha> tablero);
//		public event RecibirTableroDelegate RecibirTableroEvent;

//		public delegate void RecibirTiradaDelegate(Point tirada);
//		public event RecibirTiradaDelegate RecibirTiradaEvent;

//		public delegate void TerminarJuegoDelegate(int experenciaPorPuntos, int experenciaPorFichas, bool ganaste);
//		public event TerminarJuegoDelegate TerminarJuegoEvent;

//		public void RecibirTablero(Ficha[] tablero)
//		{
//			RecibirTableroEvent(tablero.ToList());
//		}

//		public void RecibirTirada(Point tirada)
//		{
//			RecibirTiradaEvent(tirada);
//		}

//		public void TerminarJuego(int experenciaPorPuntos, int experenciaPorFichas, bool ganaste)
//		{
//			TerminarJuegoEvent(experenciaPorPuntos, experenciaPorFichas, ganaste);
//		}
//	}
//}
