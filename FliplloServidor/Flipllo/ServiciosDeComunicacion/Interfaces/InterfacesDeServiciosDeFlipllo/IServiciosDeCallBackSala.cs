using System.ServiceModel;

namespace ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo
{
    public partial interface IServiciosDeCallBack
    {
        [OperationContract(IsOneWay = true)]
        void RecibirSala(Sala sala);
    }
}
