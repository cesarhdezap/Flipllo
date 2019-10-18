using static LogicaDeNegocios.Servicios.ServiciosDeValidacion;
using static LogicaDeNegocios.Servicios.ServiciosDeGeneracionDeCodigosDeVerificacion;
using static LogicaDeNegocios.Servicios.ServiciosDeEnvioDeCorreos;
using LogicaDeNegocios.ObjetosDeAccesoADatos;
using System;
using LogicaDeNegocios.Servicios;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.ClasesDeDominio
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Contraseña { get; set; }
        public string CorreoElectronico { get; set; }
        public EstadoUsuario Estado { get; set; }
        public Puntuacion Puntuacion { get; set; }
        public string CodigoDeVerificacion { get; set; }
        

        /// <summary>
        /// Registra un nuevo <see cref="Usuario"/> en la base de datos.
        /// </summary>
        /// <exception cref="FormatException"> Es invocada cuando los campos son inválidos.</exception>
        public void Registrar()
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();

            if (ValidarCamposParaRegistroVacios())
            {
                if (ValidarCamposParaRegistro())
                {
                    if (!usuarioDAO.CorreoExiste(CorreoElectronico) && !usuarioDAO.NombreDeUsuarioExiste(NombreDeUsuario))
                    {
                        CodigoDeVerificacion = GenerarCodigo();
                        Puntuacion = new Puntuacion();
                        Estado = EstadoUsuario.NoValidado;
                        usuarioDAO.Guardar(this);
                        EnviarCorreoDeVerficiacion(this);
                    }
                }
            }
            else
            {
                throw new FormatException("Error en Usuario.Registrar, uno de los campos es vacio.");
            }
        }
        /// <summary>
        /// Actualiza al usuario base con <paramref name="usuario"/>
        /// </summary>
        /// <param name="usuario">El usuario</param>
        public void Actualizar(Usuario usuario)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            usuarioDAO.ActualizarUsuarioPorID(this.Id, usuario);
        }

        /// <summary>
        /// Valida que los campos no esten vacios para el registro a base de datos, no válida <see cref="Usuario.Id"/>
        /// ya que su validación depende del metodo.
        /// </summary>
        /// <returns></returns>
        private bool ValidarCamposParaRegistroVacios()
        {
            bool resultadoDeValidacion = false;
            if (!string.IsNullOrEmpty(NombreDeUsuario)
                && !string.IsNullOrEmpty(Contraseña)
                && !string.IsNullOrEmpty(CorreoElectronico))
            {
                resultadoDeValidacion = true;
            }
            return resultadoDeValidacion;
        }

        public bool ValidarCredenciales()
        {
            bool resultadoDeValidacion = false;
            if (!string.IsNullOrEmpty(CorreoElectronico)
                && !string.IsNullOrEmpty(Contraseña))
            {
                UsuarioDAO usuarioDAO = new UsuarioDAO();
                resultadoDeValidacion = usuarioDAO.ValidarExistenciaDeCorreoYContraseña(CorreoElectronico, Contraseña);
            }
            return resultadoDeValidacion;
        }

        public bool ValidarCamposParaRegistro()
        {
            bool resultadoDeValidacion = false;
            if (ValidarCorreoElectronico(CorreoElectronico) && ValidarContraseña(Contraseña) && ValidarNombreDeUsuario(NombreDeUsuario))
            {
                resultadoDeValidacion = true;
            }
            return resultadoDeValidacion;
        }

        public Usuario CargarUsuarioPorCorreo()
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            Usuario usuario = usuarioDAO.CargarUsuarioPorCorreo(CorreoElectronico);
            return usuario;
        }
    }

    public enum EstadoUsuario
    {
        Indefinido,
        NoValidado,
        Registrado
    }
}
