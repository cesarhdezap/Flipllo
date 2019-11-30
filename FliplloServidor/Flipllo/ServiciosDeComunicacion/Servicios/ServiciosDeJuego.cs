using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeJuego;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiciosDeComunicacion.Servicios
{
    [ServiceBehavior(InstanceContextMode =InstanceContextMode.PerSession)]
    public class ServiciosDeJuego : IServiciosDeJuego
    {
        public void PedirActualizacionDeTablero(Sesion sesion)
        {
            throw new NotImplementedException();
        }

        public bool TirarFicha(Sesion sesion, Point point)
        {
            throw new NotImplementedException();
        }
    }
}
