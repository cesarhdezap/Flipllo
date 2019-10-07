using System;
using System.Collections.Generic;
using System.Text;

namespace ServiciosDeComunicacion
{
	public class Chat
	{
		public List<Usuario> UsuariosConectadas { get; set; } = new List<Usuario>();
		public List<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
	}
}
