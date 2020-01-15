
using ServiciosDeComunicacion.Interfaces;
using ServiciosDeComunicacion.Interfaces.Controladores;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using ServiciosDeComunicacion.Servicios;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion.Clases
{
    public class HostDeServiciosDeFlipllo : IHostDeServicio
    {
        private ServiceHost HostDelServidor;
        private IControladorDeActualizacionDePantalla ControladorDeActualizacionDePantalla;
        private ServiciosDeFlipllo ServicioDeFlipllo;
        private EstadoDelServidor EstadoDelServidor = EstadoDelServidor.Inactivo;

        public HostDeServiciosDeFlipllo(List<Sesion> sesiones, List<Sala> salas, IControladorDeActualizacionDePantalla controladorServiciosDeFlipllo)
        {
            ControladorDeActualizacionDePantalla = controladorServiciosDeFlipllo;
            ServicioDeFlipllo = new ServiciosDeFlipllo(sesiones, salas, controladorServiciosDeFlipllo);
        }

        public async void IniciarServidor()
        {
            if (EstadoDelServidor != EstadoDelServidor.Activo)
            {
                string mensajeDelServidor = await Task.Run<string>(AbrirHost);
                ControladorDeActualizacionDePantalla.EstadoDelServidorActualizado(GetType().Name, EstadoDelServidor, mensajeDelServidor);
            }
        }

        public void PararServidor()
        {
            if (EstadoDelServidor != EstadoDelServidor.Detenido)
            {
                if (HostDelServidor != null)
                {
                    HostDelServidor.Abort();
                }
                ServicioDeFlipllo.SesionesConectadas.Clear();
                ServicioDeFlipllo.ControladorServiciosDeFlipllo.ListaDeSesionesActualizado(ServicioDeFlipllo.SesionesConectadas);
                EstadoDelServidor = EstadoDelServidor.Detenido;
                ControladorDeActualizacionDePantalla.EstadoDelServidorActualizado(GetType().Name, EstadoDelServidor);
            }
        }

        private string AbrirHost()
        {
            string mensajeDeErrorDeEstado = string.Empty;
            HostDelServidor = new ServiceHost(ServicioDeFlipllo);

            if (!(HostDelServidor.State == CommunicationState.Opened) && EstadoDelServidor != EstadoDelServidor.Activo)
            {
                try
                {
                    HostDelServidor.Open();
                    EstadoDelServidor = EstadoDelServidor.Activo;
                }
                catch (CommunicationObjectFaultedException e)
                {
                    mensajeDeErrorDeEstado = e.Message.ToString();
                    HostDelServidor.Abort();
                    EstadoDelServidor = EstadoDelServidor.Incomunicado;
                }
                catch (CommunicationException e)
                {
                    mensajeDeErrorDeEstado = e.Message.ToString();
                    HostDelServidor.Abort();
                    EstadoDelServidor = EstadoDelServidor.Incomunicado;
                }
            }
            return mensajeDeErrorDeEstado;
        }
    }

    
}
