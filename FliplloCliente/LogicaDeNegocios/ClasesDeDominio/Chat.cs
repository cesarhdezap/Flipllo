using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDeNegocios.ServiciosDeFlipllo;

namespace LogicaDeNegocios.ClasesDeDominio
{
    /// <summary>
    /// Clase local para mostrar los mensajes de un chat
    /// </summary>
    public class Chat
    {
        /// <summary>
        /// Lista de los usuarios conectados al chat
        /// </summary>
        public List<Sesion> UsuariosConectados { get; set; } = new List<Sesion>();
        /// <summary>
        /// Lista de los mensajes recibidos dentro del chat
        /// </summary>
        public List<Mensaje> MensajesRecibidos { get; set; } = new List<Mensaje>();

        /// <summary>
        /// Convierte un objeto Mensaje a una cadena formateada
        /// </summary>
        /// <param name="mensaje">El mensaje a convertir</param>
        /// <returns>El mensaje convertido a una cadena</returns>
        public string MensajeToString(Mensaje mensaje)
        {
            string resultado = string.Empty;
			string nombreDeUsuario = mensaje.NombreDeUsuario;
            resultado = resultado + "[" + mensaje.Fecha.ToString() + "] " + nombreDeUsuario + ": " + mensaje.CuerpoDeMensaje;
            return resultado;
        }

        /// <summary>
        /// Convierte todos los mensajes del chat a una sola cadena formateada
        /// </summary>
        /// <returns>La cadena formateada con todos los mensajes del chat</returns>
        public string MensajesToString()
        {
            string cadenaDeMensajes = string.Empty;

            foreach (Mensaje mensaje in MensajesRecibidos)
            {
                cadenaDeMensajes = cadenaDeMensajes + System.Environment.NewLine + MensajeToString(mensaje);
            }

            return cadenaDeMensajes;
        }
    }
}
