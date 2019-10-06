using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiciosDeComunicacion
{
    public class ServiciosDeUsuario : IServiciosDeUsuario
    {
        public bool RegistrarCuenta(Usuario usuario)
        {
            
            return true;
        }

        public bool ValidarCuenta(Usuario usuario)
        {
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n****\n\n\n\n\n\n\n\n\n\n\n\n****\nValidar cuenta {0},{1},{2},{3}", usuario.ID, usuario.NombreDeUsuario, usuario.CorreoElectronico, usuario.Contraseña);
            return true;
        }
    }
}
