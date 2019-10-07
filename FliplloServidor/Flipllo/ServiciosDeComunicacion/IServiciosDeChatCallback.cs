using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion
{
	[ServiceContract]
	public interface IServiciosDeChatCallback
	{
		[OperationContract(IsOneWay = true)]
		void ActualizarListaDeUsuario(List<Usuario> usuariosConectados);

		[OperationContract(IsOneWay = true )]
		void RecibirMensaje(Mensaje mensaje);

		[OperationContract(IsOneWay = true)]
		void EnviarIDUsuario(int id);
	}
}
