using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeJuego
{
    [ServiceContract(CallbackContract = typeof(IServiciosDeCallBackJuego))]
    public interface IServiciosDeJuego
    {
        [OperationContract]
        bool TirarFicha(Sesion sesion, Point point);

        [OperationContract(IsOneWay = true)]
        void PedirActualizacionDeTablero(Sesion sesion);
    }
}
