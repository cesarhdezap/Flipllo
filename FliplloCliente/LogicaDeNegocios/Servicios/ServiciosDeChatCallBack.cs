using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDeNegocios.ServiciosDeChat;
using LogicaDeNegocios.ServiciosDeComunicacion;

namespace LogicaDeNegocios.Servicios
{
    public class ServiciosDeChatCallBack : IServiciosDeChatCallback
    {
        public delegate void NuevoMensajeRecibidoDelegate(Mensaje mensaje);
        public event NuevoMensajeRecibidoDelegate NuevoMensajeRecibidoEvent;

        public delegate void ActualizacionDeListaDeClientesConectadosDelegate(List<ServiciosDeChat.Usuario> clientesConectados);
        public event ActualizacionDeListaDeClientesConectadosDelegate ListaDeClientesConectadosEvent;

        public delegate void ActualizacionDeIDDeUsuarioDelegate(int id);
        public event ActualizacionDeIDDeUsuarioDelegate ActualizarIDDeUsuarioEvent;

        public void RecibirMensaje(Mensaje mensaje)
        {
            NuevoMensajeRecibidoEvent(mensaje);
        }

        public void ActualizarListaDeUsuario(ServiciosDeChat.Usuario[] usuariosConectados)
        {
            ListaDeClientesConectadosEvent(usuariosConectados.ToList());
        }

        public void EnviarIDUsuario(int id)
        {
            ActualizarIDDeUsuarioEvent(id);
        }
    }
}
