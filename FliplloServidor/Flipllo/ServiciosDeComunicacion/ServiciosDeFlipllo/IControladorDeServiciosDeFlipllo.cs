using ServiciosDeComunicacion.InterfacesDeServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion.ServiciosDeFlipllo
{
    public interface IControladorDeServiciosDeFlipllo
    {
        void ListaDeSesionesActualizado(List<Sesion> sesiones);
    }
}
