using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using ServiciosDeComunicacion.InterfacesDeServicios;

namespace ServiciosDeComunicacion.InterfacesDeServicios
{
	[ServiceContract(CallbackContract = typeof(IServiciosDeCallBack))]
	public partial interface IServiciosDeFlipllo 
	{
		[OperationContract(IsOneWay = true)]
		void ConectarDelChat(Sesion usuario);
        [OperationContract(IsOneWay = true)]
        void DesconectarDelChat(int IDDeUsuario);
        [OperationContract(IsOneWay = true)]
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
