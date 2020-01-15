using ServiciosDeComunicacion.Interfaces;
using ServiciosDeComunicacion.Interfaces.Controladores;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using ServiciosDeComunicacion.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion.Clases
{
    public class HostDeServiciosDeJuego : IHostDeServicio
    {
        private IControladorDeActualizacionDePantalla ControladorDeActualizacionDePantalla;
        private ServiciosDeJuego ServiciosDeJuego;
        private ServiceHost HostDelServidor;
        private EstadoDelServidor EstadoDelServidor = EstadoDelServidor.Inactivo;

        public HostDeServiciosDeJuego(IControladorDeActualizacionDePantalla controladorDeListas, List<Sala> salas, List<Sesion> sesiones)
        {
            ControladorDeActualizacionDePantalla = controladorDeListas;
            ServiciosDeJuego = new ServiciosDeJuego(controladorDeListas, salas, sesiones);
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
                ServiciosDeJuego.SesionesConectadas.Clear();
                ControladorDeActualizacionDePantalla.ListaDeSesionesActualizado(ServiciosDeJuego.SesionesConectadas);
                EstadoDelServidor = EstadoDelServidor.Detenido;
                ControladorDeActualizacionDePantalla.EstadoDelServidorActualizado(GetType().Name, EstadoDelServidor);
            }
        }

        private string AbrirHost()
        {
            string mensajeDeErrorDeEstado = string.Empty;
            HostDelServidor = new ServiceHost(ServiciosDeJuego);

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
