using ServiciosDeComunicacion.Interfaces;
using ServiciosDeComunicacion.Interfaces.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion.Clases
{
    public class HostDeServiciosDeJuego : IHostDeServicio
    {
        private IControladorDeActualizacionDePantalla controladorDeListas;

        public HostDeServiciosDeJuego(IControladorDeActualizacionDePantalla controladorDeListas)
        {
            this.controladorDeListas = controladorDeListas;
        }

        public void IniciarServidor()
        {
            throw new NotImplementedException();
        }

        public void PararServidor()
        {
            throw new NotImplementedException();
        }
    }
}
