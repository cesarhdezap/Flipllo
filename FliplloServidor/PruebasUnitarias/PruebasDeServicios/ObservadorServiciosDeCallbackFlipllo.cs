using ServiciosDeComunicacion.Interfaces;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebasUnitarias.PruebasDeServicios
{
    public class ObservadorServiciosDeCallbackFlipllo : IServiciosDeCallBack
    {
        public List<Sesion> ResultadoActualizarListaDeSesionesDeChat;
        public Sala ResultadoRecibirSala;
        public Sesion ResultadoRecibirSesion;
        public Mensaje ResultadoRecibirMensaje;
        public Sala ResultadoActualizarSala;

        public void ActualizarListaDeSesionesDeChat(List<Sesion> usuariosConectados)
        {
            ResultadoActualizarListaDeSesionesDeChat = usuariosConectados;
        }

        public void ActualizarSala(Sala sala)
        {
            ResultadoActualizarSala = sala;
        }

        public void ColorDeJugadorActualizado()
        {
            throw new NotImplementedException();
        }

        public void JuegoIniciado()
        {
            throw new NotImplementedException();
        }

        public void PedirActualizarSesion()
        {
            throw new NotImplementedException();
        }

        public void RecibirMensaje(Mensaje mensaje)
        {
            ResultadoRecibirMensaje = mensaje;
        }

        public void RecibirSalaCreada(Sala sala)
        {
            ResultadoRecibirSala = sala;
        }        

        public void RecibirSesion(Sesion sesion)
        {
            ResultadoRecibirSesion = sesion;
        }

        public void SalaBorrada()
        {
            throw new NotImplementedException();
        }

        public void SkinDeFichaActualizada(Sala sala)
        {
            throw new NotImplementedException();
        }

        public void SkinDeOponenteActualizada(string skinNueva)
        {
            throw new NotImplementedException();
        }
    }
}
