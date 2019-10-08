using LogicaDeNegocios.ServiciosDeChat;
using System.ServiceModel;

namespace LogicaDeNegocios.Proxy
{
    public class ChatDelServidorProxy : DuplexClientBase<IServiciosDeChat>
    {
        public ChatDelServidorProxy(IServiciosDeChatCallback callback) : base (callback)
        {
        }
    }
}
