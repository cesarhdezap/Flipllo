using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo
{
    public partial interface IServiciosDeCallBack
    {
        [OperationContract(IsOneWay = true)]
        void RecibirMensaje(Mensaje mensaje);
    }
}
