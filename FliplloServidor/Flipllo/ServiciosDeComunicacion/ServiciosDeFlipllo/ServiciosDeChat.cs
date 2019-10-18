using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiciosDeComunicacion.InterfacesDeServicios;
using ServiciosDeComunicacion;
using LogicaDeNegocios.ClasesDeDominio;

namespace ServiciosDeComunicacion.ServiciosDeFlipllo
{
	
	public partial class ServiciosDeFlipllo : IServiciosDeFlipllo
	{
        public Chat Chat { get; set; } = new Chat();

        public void ConectarDelChat(Sesion sesion)
		{
			bool estaConectado = false;

			foreach (Sesion usuarioConectado in Chat.UsuariosConectados)
			{
				if(usuarioConectado.Usuario.NombreDeUsuario == sesion.Usuario.NombreDeUsuario)
				{
					estaConectado = true;
				}
			}
	
			if (!estaConectado)
			{
				sesion.CanalDeCallback = OperationContext.Current.GetCallbackChannel<IServiciosDeCallBack>();
				Chat.UsuariosConectados.Add(sesion);
				int IDAsignada = Chat.UsuariosConectados.Count();
				sesion.CanalDeCallback.EnviarIDUsuario(IDAsignada);

                foreach (Sesion usuarioConectado in Chat.UsuariosConectados)
                {
                    usuarioConectado.CanalDeCallback.ActualizarListaDeUsuarios(Chat.UsuariosConectados);
                }
            }
		}

		public void DesconectarDelChat(int IDDeUsuario)
		{
			Sesion usuarioADesconectar = Chat.UsuariosConectados.FirstOrDefault(usuario => usuario.ID == IDDeUsuario);
			Chat.UsuariosConectados.Remove(usuarioADesconectar);

			foreach(Sesion usuario in Chat.UsuariosConectados)
			{
				usuario.CanalDeCallback.ActualizarListaDeUsuarios(Chat.UsuariosConectados);
			}
		}

		public void EnviarMensaje(Mensaje mensaje)
		{
			foreach(Sesion usuario in Chat.UsuariosConectados)
			{
				usuario.CanalDeCallback.RecibirMensaje(mensaje);
			}
		}
    }
}
