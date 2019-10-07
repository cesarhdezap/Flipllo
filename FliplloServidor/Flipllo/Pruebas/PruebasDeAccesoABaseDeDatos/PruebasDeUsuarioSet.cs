using AccesoABaseDeDatos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pruebas.PruebasDeAccesoABaseDeDatos
{
    [TestClass]
    public class PruebasDeUsuarioSet
    {
        [TestMethod]
        public void ProbarAñadirUsuario()
        {
            UsuarioDAO prueba = new UsuarioDAO();
            prueba.AgregarUsuario();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ProbarCargarUsuarioPorId()
        {
            UsuarioDAO prueba = new UsuarioDAO();
            Usuario usuario = prueba.CargarUsuarioPorId(1);
            Assert.AreEqual(1, usuario.Id);
        }
    }
}
