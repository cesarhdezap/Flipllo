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
	public partial interface IServiciosDeCallBack
	{
		[OperationContract(IsOneWay = true)]
		void ActualizarListaDeUsuarios(List<Sesion> usuariosConectados);

		[OperationContract(IsOneWay = true )]
		void RecibirMensaje(Mensaje mensaje);

		[OperationContract(IsOneWay = true)]
		void EnviarIDUsuario(int id);

        [OperationContract(IsOneWay = true)]
        void RecibirSesion(Sesion sesion);

        [OperationContract(IsOneWay = true)]
        void PedirActualizarSesion();
    }
}
