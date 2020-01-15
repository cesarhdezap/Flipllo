using System;
using System.Collections.Generic;
using LogicaDeNegocios.ClasesDeDominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PruebasUnitarias.PruebasDeClasesDeDominio
{
    [TestClass]
    public class PruebasDeUsuario
    {
        [TestMethod]
        public void ProbarRegistrar()
        {
            Usuario usuario = new Usuario()
            {
                NombreDeUsuario = "RegistroDeUsuario",
                CodigoDeVerificacion = "2083",
                Contraseña = "registrodeusuariopass",
                CorreoElectronico = "registrodeusuario@correo.com",
                //Hacer otra prueba donde se manden puntuacion y estado modificado para que el sistema lo modifique bien.
            };
            usuario.Registrar();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ProbarCargarUsuarioPorCorreo()
        {
            Usuario usuario = new Usuario();
            usuario.CorreoElectronico = "pipa@correo.com";
            usuario.CargarUsuarioPorCorreo();
            Assert.AreEqual("QNTSQ", usuario.CodigoDeVerificacion);
        }

        [TestMethod]
        public void ProbarCargarTopUsuariosPorPuntuacion()
        {
            Usuario usuario = new Usuario();
            List<Usuario> usuarios = usuario.CargarUsuariosPorMejorPuntuacion();
            Assert.AreEqual(8, usuarios.Count);
        }
    }
}
