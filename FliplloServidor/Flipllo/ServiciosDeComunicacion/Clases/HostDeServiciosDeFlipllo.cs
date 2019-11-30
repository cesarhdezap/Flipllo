
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
        private ServiciosDeFlipllo ServiciosDeFlipllo;
        private EstadoDelServidor EstadoDelServidor = EstadoDelServidor.Inactivo;

        public HostDeServiciosDeFlipllo(List<Sesion> sesiones, List<Sala> salas, IControladorDeActualizacionDePantalla controladorServiciosDeFlipllo)
        {
            ControladorDeActualizacionDePantalla = controladorServiciosDeFlipllo;
            ServiciosDeFlipllo = new ServiciosDeFlipllo(sesiones, salas, controladorServiciosDeFlipllo);
        }

        public async void IniciarServidor()
        {
            string mensajeDelServidor = await Task.Run<string>(AbrirHost);
            ControladorDeActualizacionDePantalla.EstadoDelServidorActualizado(EstadoDelServidor, mensajeDelServidor);
        }

        public void PararServidor()
        {
            HostDelServidor.Abort();
            ServiciosDeFlipllo.SesionesConectadas.Clear();
            ServiciosDeFlipllo.ControladorServiciosDeFlipllo.ListaDeSesionesActualizado(ServiciosDeFlipllo.SesionesConectadas);
            EstadoDelServidor = EstadoDelServidor.Detenido;
            ControladorDeActualizacionDePantalla.EstadoDelServidorActualizado(EstadoDelServidor);
        }

        private string AbrirHost()
        {
            string mensajeDeErrorDeEstado = string.Empty;
            HostDelServidor = new ServiceHost(ServiciosDeFlipllo);

            if (!(HostDelServidor.State == CommunicationState.Opened))
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

        public void LimpiarSesiones()
        {
            ServiciosDeFlipllo.SesionesConectadas.Clear();
        }

        public void LimpiarSalas()
        {
            ServiciosDeFlipllo.SalasCreadas.Clear();
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
