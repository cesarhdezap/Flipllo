using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion
{
	[ServiceContract(CallbackContract = typeof(IServiciosDeChatCallback))]
	public interface IServiciosDeChat 
	{
		[OperationContract]
		void Conectar(Usuario usuario);
		[OperationContract]
		void Desconectar(int IDDeUsuario);
		[OperationContract]
		void EnviarMensaje(Mensaje mensaje);
	}
	[DataContract]
	public class Mensaje
	{
		[DataMember]
		public string CuerpoDeMensaje { get; set; }
		[DataMember]
		public DateTime Fecha { get; set; }
		[DataMember]
		public int IDDeUsuario { get; set; }
	}
}
