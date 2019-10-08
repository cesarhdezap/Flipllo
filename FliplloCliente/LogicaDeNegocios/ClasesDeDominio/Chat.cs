using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDeNegocios.ServiciosDeChat;

namespace LogicaDeNegocios.ClasesDeDominio
{
    public class Chat
    {
        public List<Usuario> UsuariosConectados { get; set; } = new List<Usuario>();
        public List<Mensaje> MensajesRecibidos { get; set; } = new List<Mensaje>();

        public string MensajesToString()
        {
            string cadenaDeMensajes = string.Empty;

            foreach (Mensaje mensaje in MensajesRecibidos)
            {
                cadenaDeMensajes = cadenaDeMensajes + System.Environment.NewLine + mensaje.ToString();
            }

            return cadenaDeMensajes;
        }
    }
}
