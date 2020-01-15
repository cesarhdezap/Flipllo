using ServiciosDeComunicacion.Interfaces;
using ServiciosDeComunicacion.Interfaces.Controladores;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion.Clases
{
    public class AdministradorDeHostDeServicios
    {
        public List<Sesion> SesionesConectadas;
        private List<Sala> SalasConectadas = new List<Sala>();
        private List<IHostDeServicio> ListaDeServicios;
        private IControladorDeActualizacionDePantalla ControladorDeListas;
        HostDeServiciosDeFlipllo hostServiciosDeFlipllo;

        public AdministradorDeHostDeServicios(IControladorDeActualizacionDePantalla controladorDeInterfaz)
        {
            SesionesConectadas = new List<Sesion>();

            ControladorDeListas = controladorDeInterfaz;
            ListaDeServicios = new List<IHostDeServicio>();
            hostServiciosDeFlipllo = new HostDeServiciosDeFlipllo(SesionesConectadas, SalasConectadas, controladorDeInterfaz);
            HostDeServiciosDeJuego hostDeServiciosDeJuego = new HostDeServiciosDeJuego(controladorDeInterfaz, SalasConectadas, SesionesConectadas);
            ListaDeServicios.Add(hostDeServiciosDeJuego);
            ListaDeServicios.Add(hostServiciosDeFlipllo);
        }

        public void IniciarServicios()
        {
            ListaDeServicios.ForEach(s => s.IniciarServidor());
        }

        public void PararServicios()
        {
            ListaDeServicios.ForEach(s => s.PararServidor());
        }

        public void LimpiarSesiones()
        {
            SesionesConectadas.Clear();
            ControladorDeListas.ListaDeSesionesActualizado(SesionesConectadas);
        }

        public void LimpiarSalas()
        {
            SalasConectadas.Clear();
            ControladorDeListas.ListaDeSalasActualizado(SalasConectadas);
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
