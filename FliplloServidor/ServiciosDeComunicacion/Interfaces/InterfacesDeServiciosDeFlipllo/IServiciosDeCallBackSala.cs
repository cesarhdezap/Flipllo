using System.ServiceModel;

namespace ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo
{
    public partial interface IServiciosDeCallBack
    {
        [OperationContract(IsOneWay = true)]
        void RecibirSalaCreada(Sala sala);

        [OperationContract(IsOneWay = true)]
        void ActualizarSala(Sala sala);

        [OperationContract(IsOneWay = true)]
        void JuegoIniciado();

        [OperationContract(IsOneWay = true)]
        void SkinDeOponenteActualizada(string skinNueva);

        [OperationContract(IsOneWay = true)]
        void SalaBorrada();

        [OperationContract(IsOneWay = true)]
        void ColorDeJugadorActualizado();
    }
}
