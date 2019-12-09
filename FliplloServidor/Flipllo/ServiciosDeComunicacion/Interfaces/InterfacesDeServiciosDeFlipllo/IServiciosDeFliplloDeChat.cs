using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo
{
    
	public partial interface IServiciosDeFlipllo 
	{
        [OperationContract(IsOneWay = true)]
        void EnviarMensaje(Mensaje mensaje, Sesion sesion);

    }

	[DataContract]
	public class Mensaje
	{
		[DataMember]
		public string CuerpoDeMensaje { get; set; }
		[DataMember]
		public DateTime Fecha { get; set; }
		[DataMember]
		public string NombreDeUsuario { get; set; }
	}
}
