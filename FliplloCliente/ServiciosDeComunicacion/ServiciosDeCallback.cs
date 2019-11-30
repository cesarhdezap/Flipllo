using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDeNegocios.ServiciosDeFlipllo;

namespace ServiciosDeComunicacion
{
	
    public class ServiciosDeCallBack : IServiciosDeFliplloCallback
    {
        public delegate void NuevoMensajeRecibidoDelegate(Mensaje mensaje);
        public event NuevoMensajeRecibidoDelegate NuevoMensajeRecibidoEvent;

        public delegate void ActualizacionDeListaDeClientesConectadosDelegate(List<Sesion> clientesConectados);
        public event ActualizacionDeListaDeClientesConectadosDelegate ListaDeClientesConectadosEvent;

        public delegate void ActualizacionDeIDDeUsuarioDelegate(int id);
        public event ActualizacionDeIDDeUsuarioDelegate ActualizarIDDeUsuarioEvent;

		public delegate void RecibirSesionDelegate(Sesion sesion);
		public event RecibirSesionDelegate RecibirSesionEvent;

        public void ActualizarListaDeUsuarios(Sesion[] usuariosConectados)
        {
            ListaDeClientesConectadosEvent(usuariosConectados.ToList());
        }

        public void EnviarIDUsuario(int id)
        {
            ActualizarIDDeUsuarioEvent(id);
        }

		public void RecibirSesion(Sesion sesion)
		{
			RecibirSesionEvent(sesion);
		}

		public void PedirActualizarSesion()
		{
			throw new NotImplementedException();
		}

		public void RecibirSala(Sala sala)
		{
			throw new NotImplementedException();
		}

		public void ActualizarListaDeSesionesDeChat(Sesion[] usuariosConectados)
		{

		}

		public void RecibirMensajeGlobal(Mensaje mensaje)
		{
			NuevoMensajeRecibidoEvent(mensaje);
		}
	}
}
