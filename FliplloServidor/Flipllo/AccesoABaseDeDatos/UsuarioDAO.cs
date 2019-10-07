using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoABaseDeDatos
{
    public class UsuarioDAO
    {
        public void AgregarUsuario()
        {
            using (var context = new ModelFliplloContainer())
            {
                var usuario = new Usuario
                {
                    NombreDeUsuario = "pepe",
                    CorreoElectronico = "pepe@correo.com",
                    Contraseña = "pepepass",
                    Puntuacion = new Puntuacion()
                    {
                        ExperienciaTotal = 10,
                        PartidasJugadas = 2,
                        Victorias = 1,
                    }
                };
                context.UsuarioSet.Add(usuario);

                context.SaveChanges();
            }
        }

        public Usuario CargarUsuarioPorId(int id)
        {
            var usuario = new Usuario();
            using (var context = new ModelFliplloContainer())
            {
                usuario = context.UsuarioSet.Find(id);
            }
            return usuario;
        }
    }
}
