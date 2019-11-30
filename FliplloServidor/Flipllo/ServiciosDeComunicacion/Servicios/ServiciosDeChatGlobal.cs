using System.Linq;
using System.ServiceModel;
using ServiciosDeComunicacion.Clases;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;

namespace ServiciosDeComunicacion.Servicios
{

    public partial class ServiciosDeFlipllo : IServiciosDeFlipllo
	{
        public Chat ChatGlobal { get; set; } = new Chat();

        private void ConectarAlChatGlobal(Sesion sesion)
		{
            bool estaConectado = ChatGlobal.SesionesConectadas.Exists(s => s.Usuario.NombreDeUsuario == sesion.Usuario.NombreDeUsuario);
	
			if (!estaConectado)
			{
				sesion.CanalDeCallback = OperationContext.Current.GetCallbackChannel<IServiciosDeCallBack>();
				ChatGlobal.SesionesConectadas.Add(sesion);

                foreach (Sesion s in ChatGlobal.SesionesConectadas)
                {
                    s.CanalDeCallback = OperationContext.Current.GetCallbackChannel<IServiciosDeCallBack>();
                    s.CanalDeCallback.ActualizarListaDeSesionesDeChat(ChatGlobal.SesionesConectadas);
                }
                //System.ServiceModel.CommunicationObjectAbortedException
            }
        }
        /// <summary>
        /// Desconecta una sesion del chat global y se comunica con todos los usuarios conectados para actualizar
        /// su lista de usuarios conectados..
        /// </summary>
        /// <param name="IDDeUsuario"></param>
		private void DesconectarDelChatGlobal(string IDDeUsuario)
		{
			Sesion usuarioADesconectar = ChatGlobal.SesionesConectadas.FirstOrDefault(usuario => usuario.ID == IDDeUsuario);
			ChatGlobal.SesionesConectadas.Remove(usuarioADesconectar);

			foreach(Sesion usuario in ChatGlobal.SesionesConectadas)
			{
                usuario.CanalDeCallback = OperationContext.Current.GetCallbackChannel<IServiciosDeCallBack>();
                usuario.CanalDeCallback.ActualizarListaDeSesionesDeChat(ChatGlobal.SesionesConectadas);
                //System.ServiceModel.CommunicationObjectAbortedException
            }
        }

		public void EnviarMensajeAChatGlobal(Mensaje mensaje, Sesion sesion)
		{
            if (ValidarAutenticidadDeSesion(sesion))
            {
                ChatGlobal.SesionesConectadas.ForEach(s => s.CanalDeCallback.RecibirMensajeGlobal(mensaje));
            }
		}
    }
}
