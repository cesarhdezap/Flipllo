using ServiciosDeComunicacion.Interfaces;
using ServiciosDeComunicacion.Interfaces.Controladores;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion.Clases
{
    public class AdministradorDeHostDeServicios
    {
        private List<Sesion> SesionesConectadas;
        private List<Sala> SalasConectadas;
        private List<IHostDeServicio> ListaDeServicios;
        private IControladorDeActualizacionDePantalla ControladorDeListas;

        public AdministradorDeHostDeServicios(IControladorDeActualizacionDePantalla controladorDeListas)
        {
            SesionesConectadas = new List<Sesion>();
            SalasConectadas = new List<Sala>();
            ControladorDeListas = controladorDeListas;
            ListaDeServicios = new List<IHostDeServicio>();
            HostDeServiciosDeFlipllo hostServiciosDeFlipllo = new HostDeServiciosDeFlipllo(SesionesConectadas, SalasConectadas, controladorDeListas);
            ListaDeServicios.Add(hostServiciosDeFlipllo);
            HostDeServiciosDeJuego hostDeServiciosDeJuego = new HostDeServiciosDeJuego(controladorDeListas);
            //ListaDeServicios.Add(hostDeServiciosDeJuego);
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
}
