using System.Collections.Generic;
using System.ServiceModel;

namespace ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo
{
    public partial interface IServiciosDeCallBack
	{
        [OperationContract(IsOneWay = true)]
        void RecibirSesion(Sesion sesion);

        [OperationContract(IsOneWay = true)]
        void PedirActualizarSesion();
    }
}
