using ServiciosDeComunicacion.InterfacesDeServicios;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion.ServiciosDeFlipllo
{
    public class ServiciosDeHost
    {
        private ServiceHost HostDelServidor;
        private IControladorDeServiciosDeHost ControladorDeServiciosDeHost;
        private ServiciosDeFlipllo ServiciosDeFlipllo;
        private EstadoDelServidor EstadoDelServidor = EstadoDelServidor.Inactivo;

        public ServiciosDeHost(IControladorDeServiciosDeHost controladorServicioHost, IControladorDeServiciosDeFlipllo controladorServiciosDeFlipllo)
        {
            ControladorDeServiciosDeHost = controladorServicioHost;
            ServiciosDeFlipllo = new ServiciosDeFlipllo(controladorServiciosDeFlipllo);
        }

        public async void IniciarServidor()
        {
            EstadoDelServidor = await Task.Run<EstadoDelServidor>(IniciarHost);
            ControladorDeServiciosDeHost.EstadoDelServidorActualizado(EstadoDelServidor);
        }

        public void PararServidor()
        {
            PararHost();
            EstadoDelServidor = EstadoDelServidor.Detenido;
        }

        private EstadoDelServidor IniciarHost()
        {
            HostDelServidor = new ServiceHost(ServiciosDeFlipllo);
            EstadoDelServidor estado = EstadoDelServidor.Inactivo;

            if (!(HostDelServidor.State == CommunicationState.Opened))
            {
                try
                {
                    HostDelServidor.Open();
                    estado = EstadoDelServidor.Activo;
                }
                catch (CommunicationObjectFaultedException e)
                {
                    estado = EstadoDelServidor.Incomunicado;
                    PararHost();
                }
                catch (CommunicationException e)
                {
                    estado = EstadoDelServidor.Incomunicado;
                    PararHost();
                }
            }
            

            return estado;
        }

        private void PararHost()
        {
            HostDelServidor.Abort();
        }        
    }

    public enum EstadoDelServidor
    {
        Incomunicado,
        Inactivo,
        Activo,
        Detenido
    }
}
