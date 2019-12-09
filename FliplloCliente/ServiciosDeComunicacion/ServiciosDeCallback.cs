using LogicaDeNegocios.ServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiciosDeComunicacion
{

	public class CallBackDeFlipllo : IServiciosDeFliplloCallback
	{

		public delegate void RecibirSesionDelegate(Sesion sesion);
		public event RecibirSesionDelegate RecibirSesionEvent;

		public delegate void PedirActualizarSesionDelegate();
		public event PedirActualizarSesionDelegate PedirActualizarSesionEvent;

		public delegate void RecibirSalaDelegate(Sala sala);
		public event RecibirSalaDelegate RecibirSalaEvent;

		public delegate void RecibirMensajeDelegate(Mensaje mensaje);
		public event RecibirMensajeDelegate RecibirMensajeEvent;

		public delegate void ActualizarSalaDelegate(Sala sala);
		public event ActualizarSalaDelegate ActualizarSalaEvent;

		public delegate void JuegoIniciadoDelegate();
		public event JuegoIniciadoDelegate JuegoIniciadoEvent;

		public delegate void CambiarSkinDelegate(string skin);
		public event CambiarSkinDelegate CambiarSkinEvent;

		public void JuegoIniciado()
		{
			JuegoIniciadoEvent();
		}

		public void ActualizarSala(Sala sala)
		{
			ActualizarSalaEvent(sala);
		}

		public void RecibirSesion(Sesion sesion)
		{
			RecibirSesionEvent(sesion);
		}

		public void PedirActualizarSesion()
		{
			PedirActualizarSesionEvent();
		}

		public void RecibirMensaje(Mensaje mensaje)
		{
			RecibirMensajeEvent(mensaje);
		}

		public void SkinDeOponenteActualizada(string skin)
		{
			CambiarSkinEvent(skin);
		}

		public void RecibirSalaCreada(Sala sala)
		{
			RecibirSalaEvent(sala);
		}

		public void SalaBorrada()
		{
			throw new NotImplementedException();
		}

		public void ColorDeJugadorActualizado()
		{
			throw new NotImplementedException();
		}
	}
}
