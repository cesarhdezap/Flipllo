﻿using System;
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

        public string MensajeToString(Mensaje mensaje)
        {
            string resultado = string.Empty;
            string nombreDeUsuario = UsuariosConectados.First(usuario => usuario.Sesion.ID == mensaje.IDDeUsuario).NombreDeUsuario;
            resultado = resultado + "[" + mensaje.Fecha.ToString() + "] " + nombreDeUsuario + ": " + mensaje.CuerpoDeMensaje;
            return resultado;
        }

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