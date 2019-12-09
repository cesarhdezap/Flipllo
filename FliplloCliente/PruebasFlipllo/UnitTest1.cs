using System;
using System.Collections.Generic;
using LogicaDeNegocios;
using LogicaDeNegocios.ClasesDeDominio;
using LogicaDeNegocios.ServiciosDeFlipllo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using static LogicaDeNegocios.InteligenciaArtificial.InteligenciaArtificial;
using static LogicaDeNegocios.ServiciosDeRecursos;
using LogicaDeNegocios.Excepciones;
using System.IO;
using System.Linq;

namespace PruebasFlipllo
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void ProbarMensajeToString()
		{
			DateTime fechaDePrueba = new DateTime(10);
			Chat chat = new Chat();
			Mensaje mensajeAProbar = new Mensaje
			{
				CuerpoDeMensaje = "Hola!",
				NombreDeUsuario = "Pipo",
				Fecha = new DateTime(10)
			};


			string cadenaEsperada = "[" + fechaDePrueba.ToString() + "] " + mensajeAProbar.NombreDeUsuario + ": " + mensajeAProbar.CuerpoDeMensaje;
			string cadenaObtenida = chat.MensajeToString(mensajeAProbar);
			Assert.AreEqual(cadenaEsperada, cadenaObtenida);
		}


		[TestMethod]
		public void ProbarClonarJuego()
		{
			Juego juegoEsperado = new Juego();
			Juego juegoClonado = juegoEsperado.Clonar();
			Assert.AreEqual(juegoEsperado.TipoDeJuego, juegoClonado.TipoDeJuego);
		}

		[TestMethod]
		public void ProbarObtenerCuentaDeFichas_Negro()
		{
			Juego juegoDePrueba = new Juego();
			LogicaDeNegocios.ColorDeFicha color = LogicaDeNegocios.ColorDeFicha.Negro;
			Assert.AreEqual(2, juegoDePrueba.ObtenerCuentaDeFichas((LogicaDeNegocios.ColorDeFicha)(int)color));
		}

		[TestMethod]
		public void ProbarObtenerCuentaDeFichas_Blanco()
		{
			Juego juegoDePrueba = new Juego();
			LogicaDeNegocios.ColorDeFicha color = LogicaDeNegocios.ColorDeFicha.Blanco;
			Assert.AreEqual(2, juegoDePrueba.ObtenerCuentaDeFichas((LogicaDeNegocios.ColorDeFicha)(int)color));
		}

		[TestMethod]
		public void ProbarObtenerFichasComoLista()
		{
			Juego juegoDePrueba = new Juego();
			List<Ficha> resultadoEsperado = new List<Ficha>
			{
				new Ficha
				{
					Posicion = new Point(3,3),
					ColorActual = LogicaDeNegocios.ColorDeFicha.Blanco
				},
				new Ficha
				{
					Posicion = new Point(4,3),
					ColorActual = LogicaDeNegocios.ColorDeFicha.Negro
				},
				new Ficha
				{
					Posicion = new Point(3,4),
					ColorActual = LogicaDeNegocios.ColorDeFicha.Negro
				},
				new Ficha
				{
					Posicion = new Point(4,4),
					ColorActual = LogicaDeNegocios.ColorDeFicha.Blanco
				},

			};
		}

		[TestMethod]
		public void ProbarObjetoInicializadorDeJuego()
		{
			ObjetoDeInicializacionDeJuego inicializadorCreado = new ObjetoDeInicializacionDeJuego(TipoDeJuego.Local);
			ObjetoDeInicializacionDeJuego inicializadorEsperado = new ObjetoDeInicializacionDeJuego()
			{
				TipoDeJuego = TipoDeJuego.Local,
				SkinBlanca = "Default",
				SkinNegra = "Default",
				Jugadores = null
			};

			Assert.AreEqual(inicializadorCreado.TipoDeJuego, inicializadorEsperado.TipoDeJuego);
		}
		
		[TestMethod]
		public void ProbarClonarTableroNegro()
		{
			Tablero tableroEsperado = new Tablero();
			Tablero tableroClonado = tableroEsperado.Clonar();

			Assert.AreEqual(tableroEsperado.ContarFichas(LogicaDeNegocios.ColorDeFicha.Negro), tableroClonado.ContarFichas(LogicaDeNegocios.ColorDeFicha.Negro));
		}

		[TestMethod]
		public void ProbarClonarTableroBlanco()
		{
			Tablero tableroEsperado = new Tablero();
			Tablero tableroClonado = tableroEsperado.Clonar();

			Assert.AreEqual(tableroEsperado.ContarFichas(LogicaDeNegocios.ColorDeFicha.Blanco), tableroClonado.ContarFichas(LogicaDeNegocios.ColorDeFicha.Blanco));
		}

		[TestMethod]
		public void ProbarContarFichaNegra()
		{
			Tablero tablero = new Tablero();
			int cuenta = tablero.ContarFichas(LogicaDeNegocios.ColorDeFicha.Negro);
			Assert.AreEqual(2, cuenta);
		}

		[TestMethod]
		public void ProbarContarFichaBlanca()
		{
			Tablero tablero = new Tablero();
			int cuenta = tablero.ContarFichas(LogicaDeNegocios.ColorDeFicha.Blanco);
			Assert.AreEqual(2, cuenta);
		}

		[TestMethod]
		public void ProbarEsmovimientoLegalPosicion1_RegresaVerdadero()
		{
			Point puntoDeTirada = new Point(3, 2);
			Tablero tablero = new Tablero();
			Assert.IsTrue(tablero.EsMovimientoLegal(puntoDeTirada, LogicaDeNegocios.ColorDeFicha.Negro));
		}

		[TestMethod]
		public void ProbarEsmovimientoLegalPosicion2_RegresaVerdadero()
		{
			Point puntoDeTirada = new Point(2, 3);
			Tablero tablero = new Tablero();
			Assert.IsTrue(tablero.EsMovimientoLegal(puntoDeTirada, LogicaDeNegocios.ColorDeFicha.Negro));
		}

		[TestMethod]
		public void ProbarEsmovimientoLegalPosicion3_RegresaVerdadero()
		{
			Point puntoDeTirada = new Point(4, 5);
			Tablero tablero = new Tablero();
			Assert.IsTrue(tablero.EsMovimientoLegal(puntoDeTirada, LogicaDeNegocios.ColorDeFicha.Negro));
		}

		[TestMethod]
		public void ProbarEsmovimientoLegalPosicion4_RegresaFalso()
		{
			Point puntoDeTirada = new Point(7, 4);
			Tablero tablero = new Tablero();
			Assert.IsFalse(tablero.EsMovimientoLegal(puntoDeTirada, LogicaDeNegocios.ColorDeFicha.Negro));
		}

		[TestMethod]
		public void ProbarEsmovimientoLegalPosicion1_RegresaFalso()
		{
			Point puntoDeTirada = new Point(0, 0);
			Tablero tablero = new Tablero();
			Assert.IsFalse(tablero.EsMovimientoLegal(puntoDeTirada, LogicaDeNegocios.ColorDeFicha.Negro));
		}

		[TestMethod]
		public void ProbarEsmovimientoLegalPosicion2_RegresaFalso()
		{
			Point puntoDeTirada = new Point(1, 1);
			Tablero tablero = new Tablero();
			Assert.IsFalse(tablero.EsMovimientoLegal(puntoDeTirada, LogicaDeNegocios.ColorDeFicha.Negro));
		}

		[TestMethod]
		public void ProbarEsmovimientoLegalPosicion3_RegresaFalso()
		{
			Point puntoDeTirada = new Point(6, 6);
			Tablero tablero = new Tablero();
			Assert.IsFalse(tablero.EsMovimientoLegal(puntoDeTirada, LogicaDeNegocios.ColorDeFicha.Negro));
		}

		[TestMethod]
		public void ProbarEsmovimientoLegalPosicion4_RegresaVerdadero()
		{
			Point puntoDeTirada = new Point(5, 4);
			Tablero tablero = new Tablero();
			Assert.IsTrue(tablero.EsMovimientoLegal(puntoDeTirada, LogicaDeNegocios.ColorDeFicha.Negro));
		}

		[TestMethod]
		public void ProbarEsCasillaDentroDeTableroPosicion1_RegresaVerdadero()
		{
			Point puntoDeTirada = new Point(0, 0);
			Tablero tablero = new Tablero();
			Assert.IsTrue(tablero.EsCasillaDentroDeTablero(puntoDeTirada));
		}

		[TestMethod]
		public void ProbarEsCasillaDentroDeTableroPosicion2_RegresaVerdadero()
		{
			Point puntoDeTirada = new Point(1, 1);
			Tablero tablero = new Tablero();
			Assert.IsTrue(tablero.EsCasillaDentroDeTablero(puntoDeTirada));
		}

		[TestMethod]
		public void ProbarEsCasillaDentroDeTableroPosicion3_RegresaVerdadero()
		{
			Point puntoDeTirada = new Point(7, 7);
			Tablero tablero = new Tablero();
			Assert.IsTrue(tablero.EsCasillaDentroDeTablero(puntoDeTirada));
		}

		[TestMethod]
		public void ProbarEsCasillaDentroDeTableroPosicion4_RegresaVerdadero()
		{
			Point puntoDeTirada = new Point(4, 4);
			Tablero tablero = new Tablero();
			Assert.IsTrue(tablero.EsCasillaDentroDeTablero(puntoDeTirada));
		}

		[TestMethod]
		public void ProbarEsCasillaDentroDeTableroPosicion5_RegresaVerdadero()
		{
			Point puntoDeTirada = new Point(0, 7);
			Tablero tablero = new Tablero();
			Assert.IsTrue(tablero.EsCasillaDentroDeTablero(puntoDeTirada));
		}

		[TestMethod]
		public void ProbarEsCasillaDentroDeTableroPosicion1_RegresaFalso()
		{
			Point puntoDeTirada = new Point(-999999999, -9999999999);
			Tablero tablero = new Tablero();
			Assert.IsFalse(tablero.EsCasillaDentroDeTablero(puntoDeTirada));
		}

		[TestMethod]
		public void ProbarEsCasillaDentroDeTableroPosicion2_RegresaFalso()
		{
			Point puntoDeTirada = new Point(99999999999, 9999999999999);
			Tablero tablero = new Tablero();
			Assert.IsFalse(tablero.EsCasillaDentroDeTablero(puntoDeTirada));
		}

		[TestMethod]
		public void ProbarEsCasillaDentroDeTableroPosicion3_RegresaFalso()
		{
			Point puntoDeTirada = new Point(8, 8);
			Tablero tablero = new Tablero();
			Assert.IsFalse(tablero.EsCasillaDentroDeTablero(puntoDeTirada));
		}

		[TestMethod]
		public void ProbarEsCasillaDentroDeTableroPosicion4_RegresaFalso()
		{
			Point puntoDeTirada = new Point(-1, -1);
			Tablero tablero = new Tablero();
			Assert.IsFalse(tablero.EsCasillaDentroDeTablero(puntoDeTirada));
		}

		[TestMethod]
		public void ProbarEsCasillaDentroDeTableroPosicion5_RegresaFalso()
		{
			Point puntoDeTirada = new Point(8, -1);
			Tablero tablero = new Tablero();
			Assert.IsFalse(tablero.EsCasillaDentroDeTablero(puntoDeTirada));
		}

		[TestMethod]
		public void ProbarGetFichaPorPunto1()
		{
			Point punto = new Point(3, 3);
			Tablero tablero = new Tablero();
			Assert.AreEqual(tablero.GetFicha(punto).ColorActual, LogicaDeNegocios.ColorDeFicha.Blanco);
		}

		[TestMethod]
		public void ProbarGetFichaPorPunto2()
		{
			Point punto = new Point(3, 4);
			Tablero tablero = new Tablero();
			Assert.AreEqual(tablero.GetFicha(punto).ColorActual, LogicaDeNegocios.ColorDeFicha.Negro);
		}

		[TestMethod]
		public void ProbarGetFichaPorPunto3()
		{
			Point punto = new Point(4, 4);
			Tablero tablero = new Tablero();
			Assert.AreEqual(tablero.GetFicha(punto).ColorActual, LogicaDeNegocios.ColorDeFicha.Blanco);
		}

		[TestMethod]
		public void ProbarGetFichaPorPunto4()
		{
			Point punto = new Point(4, 3);
			Tablero tablero = new Tablero();
			Assert.AreEqual(tablero.GetFicha(punto).ColorActual, LogicaDeNegocios.ColorDeFicha.Negro);
		}

		[TestMethod]
		public void ProbarMejorMovimiento()
		{
			Point puntoEsperado = new Point(2, 3);
			Juego juego = new Juego();
			Point puntoObtenido = MejorMovimiento(juego);
			Assert.AreEqual(puntoEsperado, puntoObtenido);
		}

		[TestMethod]
		public void ProbarMejorMovimiento2()
		{
			Point puntoEsperado = new Point(2, 2);
			Juego juego = new Juego();
			juego.Tirar(new Point(2, 3));
			Point puntoObtenido = MejorMovimiento(juego);
			Assert.AreEqual(puntoEsperado, puntoObtenido);
		}

		[TestMethod]
		public void ProbarListarRecursosDeIdioma()
		{
			List<string> listaEsperada = new List<string>();
			listaEsperada.Add("Default");
			listaEsperada.Add("Alana");
			List<string> listaObtenida = ListarSkins();

			Assert.AreEqual(listaEsperada.Count, listaObtenida.Count);
		}

		[TestMethod]
		public void ProbarListarSkins()
		{
			List<string> listaEsperada = new List<string>();
			listaEsperada.Add("en-US");
			listaEsperada.Add("es-MX");
			List<string> listaObtenida = ListarRecursosDeIdioma();

			Assert.AreEqual(listaEsperada.Count, listaObtenida.Count);
		}
	}
}