using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiciosDeComunicacion;

namespace ServiciosDeComunicacion
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class ServiciosDeChat : IServiciosDeChat
	{
		public Chat Chat { get; set; } = new Chat();
		
		public void Conectar(Usuario usuario)
		{
			bool estaConectado = false;

			foreach (Usuario usuarioConectado in Chat.UsuariosConectadas)
			{
				if(usuarioConectado.NombreDeUsuario == usuario.NombreDeUsuario)
				{
					estaConectado = true;
				}
			}
	
			if (!estaConectado)
			{
				usuario.canalDeCallback = OperationContext.Current.GetCallbackChannel<IServiciosDeChatCallback>();
				Chat.UsuariosConectadas.Add(usuario);
				int IDAsignada = Chat.UsuariosConectadas.Count();
				usuario.canalDeCallback.EnviarIDUsuario(IDAsignada);
				
				//foreach (Usuario usuarioConectado in Chat.UsuariosConectadas)
				//{
				//	usuarioConectado.canalDeCallback.ActualizarListaDeUsuario(Chat.UsuariosConectadas);
				//}
			}
			
			
		}

		public void Desconectar(int IDDeUsuario)
		{
			Usuario usuarioADesconectar = Chat.UsuariosConectadas.FirstOrDefault(usuario => usuario.ID == IDDeUsuario);
			Chat.UsuariosConectadas.Remove(usuarioADesconectar);

			foreach(Usuario usuario in Chat.UsuariosConectadas)
			{
				usuario.canalDeCallback.ActualizarListaDeUsuario(Chat.UsuariosConectadas);
			}
		}
		
		

		public void EnviarMensaje(Mensaje mensaje)
		{
			foreach(Usuario usuario in Chat.UsuariosConectadas)
			{
				usuario.canalDeCallback.RecibirMensaje(mensaje);
			}
		}
	}
}
