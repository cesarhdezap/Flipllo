using LogicaDeNegocios.ObjetosDeAccesoADatos;
using System;
using static LogicaDeNegocios.Servicios.ServiciosDeEnvioDeCorreos;
using static LogicaDeNegocios.Servicios.ServiciosDeGeneracionDeCodigos;
using static LogicaDeNegocios.Servicios.ServiciosDeValidacion;

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
        /// <returns>Si el registro fue exitoso</returns>
        /// <exception cref="FormatException"> Es invocada cuando los campos son inválidos.</exception>
        public bool Registrar()
        {
            bool resultadoDelRegistro = false;
            if (ValidarCamposParaRegistro())
            {
                UsuarioDao usuarioDAO = new UsuarioDao();
                if (!usuarioDAO.ValidarExistenciaDeCorreo(CorreoElectronico) 
                    && !usuarioDAO.ValidarExistenciaDeNombreDeUsuario(NombreDeUsuario))
                {
                    CodigoDeVerificacion = GenerarCodigoDeValidacionDeCorreo();
                    Puntuacion = new Puntuacion();
                    Estado = EstadoUsuario.CodigoDeValidacionNovalido;
                    Contraseña = Servicios.ServiciosDeEncriptación.EncriptarCadena(Contraseña);
                    usuarioDAO.Guardar(this);
                    resultadoDelRegistro = true;
                    EnviarCorreoDeVerficiacion(this);
                }
            }
            return resultadoDelRegistro;
        }
        /// <summary>
        /// Actualiza al usuario base con <paramref name="usuario"/>
        /// </summary>
        /// <param name="usuario">El usuario</param>
        public void Actualizar(Usuario usuario)
        {
            UsuarioDao usuarioDAO = new UsuarioDao();
            usuarioDAO.ActualizarEstadoPorID(this.Id, usuario.Estado);
        }

        /// <summary>
        /// Valida que los campos no esten vacios para el registro a base de datos, no válida <see cref="Usuario.Id"/>
        /// ya que su validación depende del metodo.
        /// </summary>
        /// <returns></returns>
        private bool ValidarCamposParaRegistro()
        {
            bool resultadoDeValidacion = false;
            if (ValidarNombreDeUsuario(NombreDeUsuario)
                && ValidarContraseña(Contraseña)
                && ValidarCorreoElectronico(CorreoElectronico))
            {
                resultadoDeValidacion = true;
            }
            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida la correctitud del <see cref="CorreoElectronico"/> y 
        /// <see cref="Contraseña"/>, luego valida la existencia de estos en base de datos para 
        /// evitar consultas excesivas
        /// </summary>
        /// <returns></returns>
        public bool ValidarCredenciales()
        {
            bool resultadoDeValidacion = false;
            if (ValidarCorreoElectronico(CorreoElectronico)
                && ValidarContraseña(Contraseña))
            {
                UsuarioDao usuarioDAO = new UsuarioDao();
                resultadoDeValidacion = usuarioDAO.ValidarExistenciaDeCorreoYContraseña(CorreoElectronico, Contraseña);
            }
            return resultadoDeValidacion;
        }

        /// <summary>
        /// Carga un usuario existente en este objeto por el atributo <see cref="Usuario.CorreoElectronico"/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"> Si los datos del usuario no existen</exception>
        public Usuario CargarUsuarioPorCorreo()
        {
            UsuarioDao usuarioDAO = new UsuarioDao();
            Usuario usuario = usuarioDAO.CargarUsuarioPorCorreo(CorreoElectronico);
            return usuario;
        }

        public void ActualizarEstadoNoValidadoARegistrado(int id)
        {
            UsuarioDao usuarioDAO = new UsuarioDao();
            usuarioDAO.ActualizarEstadoPorID(id, EstadoUsuario.Registrado);
        }
    }

    public enum EstadoUsuario
    {
        Indefinido,
        Inexistente,
        CodigoDeValidacionNovalido,
        Registrado,
        ActualmenteConectado
    }
}
