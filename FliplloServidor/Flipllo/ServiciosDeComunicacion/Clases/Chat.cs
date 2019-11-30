using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System.Collections.Generic;

namespace ServiciosDeComunicacion.Clases
{
	public class Chat
	{
		public List<Sesion> SesionesConectadas { get; set; } = new List<Sesion>();
		public List<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
	}
}
