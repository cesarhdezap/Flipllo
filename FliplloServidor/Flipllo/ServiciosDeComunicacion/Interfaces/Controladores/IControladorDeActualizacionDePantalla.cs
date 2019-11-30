using ServiciosDeComunicacion.Clases;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System.Collections.Generic;

namespace ServiciosDeComunicacion.Interfaces.Controladores
{
    public interface IControladorDeActualizacionDePantalla
    {
        void ListaDeSesionesActualizado(List<Sesion> sesiones);
        void ListaDeSalasActualizado(List<Sala> salas);
        void EstadoDelServidorActualizado(EstadoDelServidor estadoDelServidor, string mensaje = null);
    }
}
