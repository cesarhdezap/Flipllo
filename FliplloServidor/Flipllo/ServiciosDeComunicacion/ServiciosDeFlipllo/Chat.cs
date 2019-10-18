using ServiciosDeComunicacion.InterfacesDeServicios;
using System.Collections.Generic;

namespace ServiciosDeComunicacion.ServiciosDeFlipllo
{
	public class Chat
	{
		public List<Sesion> UsuariosConectados { get; set; } = new List<Sesion>();
		public List<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
	}
}
