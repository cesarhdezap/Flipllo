using ServiciosDeComunicacion.Clases;
using ServiciosDeComunicacion.Interfaces;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace ServiciosDeComunicacion.Servicios
{

    public partial class ServiciosDeFlipllo : IServiciosDeFlipllo
    {
        public void EnviarMensaje(Mensaje mensaje, Sesion sesion)
        {
            if (ValidarAutenticidadDeSesion(sesion))
            {
                if (!ValidarExistenciaDeSesionEnSalasCreadas(sesion))
                {
                    foreach(Sesion sesionDeBusqueda in SesionesConectadas)
                    {
                        if (!ValidarExistenciaDeSesionEnSalasCreadas(sesionDeBusqueda))
                        {
                            sesionDeBusqueda.CanalDeCallback.RecibirMensaje(mensaje);
                        }
                    }
                }
                else
                {
                    Sala sala = BuscarSalaDeSesion(sesion);
                    foreach (Jugador jugador in sala.Jugadores)
                    {
                        jugador.Sesion.CanalDeCallback.RecibirMensaje(mensaje);
                    }
                }
            }
        }
    }
}
