using AccesoABaseDeDatos;
using LogicaDeNegocios.ClasesDeDominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicaDeNegocios.ObjetosDeAccesoADatos
{
    public class UsuarioDAO
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario">Es un usuario de la clase generada por el EntityFramework de AccesoABaseDeDatos</param>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException"></exception>
        public void Guardar(ClasesDeDominio.Usuario usuario)
        {
            using (ModelFliplloContainer context = new ModelFliplloContainer())
            {
                context.UsuarioSet.Add(ConvertirUsuarioDeLogicaAUsuarioDeAccesoADatos(usuario));
                context.SaveChanges();
            }
        }

        public ClasesDeDominio.Usuario CargarUsuarioPorId(int idUsuario)
        {
            AccesoABaseDeDatos.Usuario usuarioBD;
            using (ModelFliplloContainer context = new ModelFliplloContainer())
            {
                usuarioBD = context.UsuarioSet.Find(idUsuario);
            }
            ClasesDeDominio.Usuario usuario;
            if (usuarioBD != null)
            {
                usuario = ConvertirUsuarioDeAccesoADatosAUsuarioDeLogica(usuarioBD);
            }
            else
            {
                throw new InvalidOperationException("La id del usuario no existe");
            }
            return usuario;
        }

        public bool CorreoExiste(string correo)
        {
            List<AccesoABaseDeDatos.Usuario> usuariosContext;
            using (ModelFliplloContainer context = new ModelFliplloContainer())
            {
                usuariosContext = context.UsuarioSet.ToList();
            }
            bool resultadoDeExistencia = usuariosContext.Exists(usuario => usuario.CorreoElectronico == correo);

            return resultadoDeExistencia;
        }

        public bool NombreDeUsuarioExiste(string nombreDeUsuario)
        {
            List<AccesoABaseDeDatos.Usuario> usuariosContext;
            using (ModelFliplloContainer context = new ModelFliplloContainer())
            {
                usuariosContext = context.UsuarioSet.ToList();
            }
            bool resultadoDeExistencia = usuariosContext.Exists(usuario => usuario.NombreDeUsuario == nombreDeUsuario);

            return resultadoDeExistencia;
        }

        public ClasesDeDominio.Usuario CargarUsuarioPorCorreo(string correo)
        {
            ClasesDeDominio.Usuario usuario = new ClasesDeDominio.Usuario();
            if (CorreoExiste(correo))
            {
                AccesoABaseDeDatos.Usuario usuarioBD;

                using (ModelFliplloContainer context = new ModelFliplloContainer())
                {
                    usuarioBD = context.UsuarioSet.FirstOrDefault(usuarioBusqueda => usuarioBusqueda.CorreoElectronico == correo);
                }

                if (usuarioBD != null)
                {
                    usuario = ConvertirUsuarioDeAccesoADatosAUsuarioDeLogica(usuarioBD);
                }
            }
            else
            {
                throw new InvalidOperationException("La id del usuario no existe"); //Consider own exceptions
            }
            return usuario;
        }

        public bool ValidarExistenciaDeCorreoYContraseña(string correo, string contraseña)
        {
            bool resultadoDeExistencia = false;
            AccesoABaseDeDatos.Usuario usuarioLocalizado;
            using (ModelFliplloContainer context = new ModelFliplloContainer())
            {
                usuarioLocalizado = context.UsuarioSet.FirstOrDefault(usuario => usuario.CorreoElectronico == correo && usuario.Contraseña == contraseña);
            }
            if (usuarioLocalizado != null)
            {
                resultadoDeExistencia = true;
            }

            return resultadoDeExistencia;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuarios"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        private List<ClasesDeDominio.Usuario> ConvertirListaDeUsuariosBDaUsuariosLogica(List<AccesoABaseDeDatos.Usuario> usuarios)
        {
            List<ClasesDeDominio.Usuario> usuariosConvertidos = new List<ClasesDeDominio.Usuario>();
            foreach (AccesoABaseDeDatos.Usuario usuario in usuarios)
            {
                ClasesDeDominio.Usuario usuarioConvertido = new ClasesDeDominio.Usuario();
                usuarioConvertido.Id = usuario.Id;
                usuarioConvertido.NombreDeUsuario = usuario.NombreDeUsuario;
                usuarioConvertido.Estado = (EstadoUsuario)usuario.Estado;
                usuarioConvertido.CorreoElectronico = usuario.CorreoElectronico;
                usuarioConvertido.Contraseña = usuario.Contraseña;
                usuarioConvertido.Puntuacion = new Puntuacion
                {
                    ExperienciaTotal = usuario.ExperienciaTotal,
                    PartidasJugadas = usuario.PartidasJugadas,
                    Victorias = usuario.Victorias
                };

                usuariosConvertidos.Add(usuarioConvertido);
            }
            return usuariosConvertidos;
        }

        public void ActualizarUsuarioPorID(int IDUsuario, ClasesDeDominio.Usuario usuario)
        {

        }

        public List<AccesoABaseDeDatos.Usuario> ConvertirListaDeUsuariosDeLogicaAUsuariosDB(List<ClasesDeDominio.Usuario> usuarios)
        {
            List<AccesoABaseDeDatos.Usuario> usuariosConvertidos = new List<AccesoABaseDeDatos.Usuario>();
            foreach (ClasesDeDominio.Usuario usuario in usuarios)
            {
                AccesoABaseDeDatos.Usuario usuarioConvertido = new AccesoABaseDeDatos.Usuario();
                usuarioConvertido.Id = usuario.Id;
                usuarioConvertido.NombreDeUsuario = usuario.NombreDeUsuario;
                usuarioConvertido.Estado = (short)usuario.Estado;
                usuarioConvertido.Contraseña = usuario.Contraseña;
                usuarioConvertido.CodigoDeVerificacion = usuario.CodigoDeVerificacion;
                usuarioConvertido.CorreoElectronico = usuario.CorreoElectronico;

                usuarioConvertido.ExperienciaTotal = usuario.Puntuacion.ExperienciaTotal;
                usuarioConvertido.PartidasJugadas = (short)usuario.Puntuacion.PartidasJugadas;
                usuarioConvertido.Victorias = (short)usuario.Puntuacion.Victorias;
                usuariosConvertidos.Add(usuarioConvertido);
            }
            return usuariosConvertidos;
        }


        public AccesoABaseDeDatos.Usuario ConvertirUsuarioDeLogicaAUsuarioDeAccesoADatos(ClasesDeDominio.Usuario usuario)
        {
            List<ClasesDeDominio.Usuario> usuarios = new List<ClasesDeDominio.Usuario>();
            usuarios.Add(usuario);

            List<AccesoABaseDeDatos.Usuario> usuariosDeBaseDeDAtos = ConvertirListaDeUsuariosDeLogicaAUsuariosDB(usuarios);

            return usuariosDeBaseDeDAtos.First();
        }

        private ClasesDeDominio.Usuario ConvertirUsuarioDeAccesoADatosAUsuarioDeLogica(AccesoABaseDeDatos.Usuario usuario)
        {
            List<AccesoABaseDeDatos.Usuario> usuarios = new List<AccesoABaseDeDatos.Usuario>();
            usuarios.Add(usuario);

            List<ClasesDeDominio.Usuario> usuariosDeBaseDeDAtos = ConvertirListaDeUsuariosBDaUsuariosLogica(usuarios);

            return usuariosDeBaseDeDAtos.First();
        }
    }
}
