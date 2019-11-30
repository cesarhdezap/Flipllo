using LogicaDeNegocios.ClasesDeDominio;
using LogicaDeNegocios.ObjetosDeAccesoADatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Pruebas.PruebasDeObjetosDeAccesoADatos
{
    [TestClass]
    public class PruebasDeUsuarioDAO
    {
        [TestMethod]
        public void ProbarGuardar()
        {
            Usuario usuario = new Usuario()
            {
                NombreDeUsuario = "Pepe",
                CodigoDeVerificacion = "a087",
                Contraseña = "pepepass",
                CorreoElectronico = "pepeq@correo.com",
                Estado = EstadoUsuario.Registrado,
                Puntuacion = new Puntuacion()
                {
                    PartidasJugadas = 0,
                    ExperienciaTotal = 0,
                    Victorias = 0
                }
            };

            UsuarioDao prueba = new UsuarioDao();
            prueba.Guardar(usuario);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ProbarValidarExistenciaDeCorreoYContraseña()
        {
            UsuarioDao prueba = new UsuarioDao();
            bool existen = prueba.ValidarExistenciaDeCorreoYContraseña("pepeq@correo.com", "pepepass");
            Assert.IsTrue(existen);
        }

        [TestMethod]
        public void ProbarCargarUsuarioPorId()
        {
            UsuarioDao prueba = new UsuarioDao();
            Usuario usuario = prueba.CargarUsuarioPorId(1);
            Assert.AreEqual(1, usuario.Id);
        }

        [TestMethod]
        public void ProbarConvertirListaDeUsuariosDeLogicaAUsuariosDB()
        {
            Usuario usuario = new Usuario()
            {
                NombreDeUsuario = "Pepe",
                CodigoDeVerificacion = "a087",
                Contraseña = "pepepass",
                CorreoElectronico = "pepe@correo.com",
                Estado = EstadoUsuario.Registrado,
                Puntuacion = new Puntuacion()
                {
                    PartidasJugadas = 0,
                    ExperienciaTotal = 0,
                    Victorias = 0
                }
            };

            Usuario usuario2 = new Usuario()
            {
                NombreDeUsuario = "Pipo",
                CodigoDeVerificacion = "a088",
                Contraseña = "pipopass",
                CorreoElectronico = "pipo@correo.com",
                Estado = EstadoUsuario.Registrado,
                Puntuacion = new Puntuacion()
                {
                    PartidasJugadas = 0,
                    ExperienciaTotal = 0,
                    Victorias = 0
                }
            };

            List<Usuario> usuarios = new List<Usuario>();
            usuarios.Add(usuario);
            usuarios.Add(usuario2);

            UsuarioDao usuarioDAO = new UsuarioDao();

            List<AccesoABaseDeDatos.Usuario> usuariosBD = usuarioDAO.ConvertirListaDeUsuariosDeLogicaAUsuariosDB(usuarios);
            if (usuariosBD.Count > 0)
            {
                if (usuariosBD[0].NombreDeUsuario == usuarios[0].NombreDeUsuario && usuariosBD[1].NombreDeUsuario == usuarios[1].NombreDeUsuario)
                {
                    Assert.IsTrue(true, "Nombres de usuario iguales");
                }
                else
                {
                    Assert.Fail("Nombres no iguales");
                }
            }
            else
            {
                Assert.Fail("UsuariosBD.Count <= 0");
            }
        }

        [TestMethod]
        public void ProbarConvertirUsuarioDeLogicaAUsuarioDeAccesoADatos()
        {
            Usuario usuario = new Usuario()
            {
                NombreDeUsuario = "Pepe",
                CodigoDeVerificacion = "a087",
                Contraseña = "pepepass",
                CorreoElectronico = "pepe@correo.com",
                Estado = EstadoUsuario.Registrado,
                Puntuacion = new Puntuacion()
                {
                    PartidasJugadas = 0,
                    ExperienciaTotal = 0,
                    Victorias = 0
                }
            };

            UsuarioDao usuarioDAO = new UsuarioDao();
            AccesoABaseDeDatos.Usuario usuarioBD = usuarioDAO.ConvertirUsuarioDeLogicaAUsuarioDeAccesoADatos(usuario);
            if (usuario.NombreDeUsuario == usuarioBD.NombreDeUsuario
                && usuario.Contraseña == usuarioBD.Contraseña
                && usuario.CodigoDeVerificacion == usuarioBD.CodigoDeVerificacion
                && usuario.CorreoElectronico == usuarioBD.CorreoElectronico)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail("Algun dato es diferente de la conversion.");
            }
        }
    }
}
