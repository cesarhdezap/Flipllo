using LogicaDeNegocios.ClasesDeDominio;
using ServiciosDeComunicacion.Interfaces.Controladores;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;

namespace ServiciosDeComunicacion.Servicios
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class ServiciosDeFlipllo : IServiciosDeFlipllo
    {
        public List<Sesion> SesionesConectadas;
        public IControladorDeActualizacionDePantalla ControladorServiciosDeFlipllo;

        public ServiciosDeFlipllo(List<Sesion> sesiones, List<Sala> salas, IControladorDeActualizacionDePantalla controladorServiciosDeFlipllo)
        {
            ControladorServiciosDeFlipllo = controladorServiciosDeFlipllo;
            SesionesConectadas = sesiones;
            SalasCreadas = salas;
        }


        public bool RegistrarUsuario(Usuario usuario)
        {
            bool resultadoDelRegistro = false;
            if (usuario != null)
            {
                resultadoDelRegistro = usuario.Registrar();
            }
            return resultadoDelRegistro;
        }

        /// <summary>
        /// Valida si el usuario esta registrado en la base de datos y realiza un callback con la <see cref="Sesion"/>,
        /// regresa <see cref="EstadoUsuario.Inexistente"/> en <see cref="Sesion.Usuario"/> para cuando no se encuentra en la base de datos o sus
        /// credenciales son inválidas para asegurar que el cliente no recibe información de la existencia del usuario
        /// en la base de datos.
        /// </summary>
        /// <param name="usuario"><see cref="Usuario.Contraseña"/> debe ser un SHA256</param>
        public void AutenticarUsuario(Usuario usuario)
        {
            Sesion sesion = new Sesion()
            {
                Usuario = new Usuario()
            };

            if (usuario.ValidarCredenciales())
            {
                Usuario usuarioCargado = new Usuario();
                try
                {
                    usuarioCargado = usuario.CargarUsuarioPorCorreo();
                }
                catch (InvalidOperationException)
                {
                    sesion.Usuario.Estado = EstadoUsuario.Inexistente;
                }

                if (usuarioCargado.Estado == EstadoUsuario.Registrado)
                {
                    bool correoConectadoAlServidor = SesionesConectadas.Exists(s => s.Usuario.CorreoElectronico == usuarioCargado.CorreoElectronico);
                    if (correoConectadoAlServidor)
                    {
                        sesion.Usuario.Estado = EstadoUsuario.ActualmenteConectado;
                    }
                    else
                    {
                        sesion = GenerarNuevaSesion(usuarioCargado);
                    }
                }
                else if (sesion.Usuario.Estado != EstadoUsuario.Inexistente)
                {
                    sesion.Usuario.Estado = usuarioCargado.Estado;
                }
            }
            else
            {
                sesion.Usuario.Estado = EstadoUsuario.Inexistente;
            }

            bool sesionEnviadaCorrectamente = false;
            sesion.CanalDeCallback = OperationContext.Current.GetCallbackChannel<IServiciosDeCallBack>();
            try
            {
                sesion.CanalDeCallback.RecibirSesion(sesion);
                sesionEnviadaCorrectamente = true;
            }
            catch (CommunicationException e)
            {
                NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Warn(e);
            }

            if (sesionEnviadaCorrectamente && sesion.Usuario.Estado == EstadoUsuario.Registrado)
            {
                SesionesConectadas.Add(sesion);
                ControladorServiciosDeFlipllo.ListaDeSesionesActualizado(SesionesConectadas);
                ConectarAlChatGlobal(sesion);
            }
        }

        public bool ValidarCodigoDeUsuario(Usuario usuario)
        {
            bool resultadoDeValidacion = false;
            if (usuario.ValidarCredenciales())
            {
                Usuario usuarioCargado;
                usuarioCargado = usuario.CargarUsuarioPorCorreo();
                if (usuario.CodigoDeVerificacion == usuarioCargado.CodigoDeVerificacion)
                {
                    usuarioCargado.Estado = EstadoUsuario.Registrado;
                    usuarioCargado.ActualizarEstadoNoValidadoARegistrado(usuarioCargado.Id);
                    resultadoDeValidacion = true;
                }
            }
            return resultadoDeValidacion;
        }

        public void ActualizarSesion(Sesion sesion)
        {
            throw new NotImplementedException();
        }

        public void CerrarSesion(Sesion sesion)
        {
            if (ValidarAutenticidadDeSesion(sesion))
            {
                SesionesConectadas.Remove(sesion);
                DesconectarDelChatGlobal(sesion.ID);
                ControladorServiciosDeFlipllo.ListaDeSesionesActualizado(SesionesConectadas);
            }
        }

        /// <summary>
        /// Añade los atributos de una nueva <see cref="Sesion"/>, con un <see cref="Sesion.usuario"/>
        /// sin información sensible a <see cref="SesionesConectadas"/>
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private Sesion GenerarNuevaSesion(Usuario usuario)
        {
            Sesion sesion = new Sesion
            {
                ID = Guid.NewGuid().ToString(),
                Creacion = DateTime.Now,
                UltimaActualizacion = DateTime.Now,
                Usuario = new Usuario
                {
                    Estado = usuario.Estado,
                    CorreoElectronico = usuario.CorreoElectronico,
                    NombreDeUsuario = usuario.NombreDeUsuario,
                    Puntuacion = usuario.Puntuacion
                }
            };

            return sesion;
        }

        /// <summary>
        /// Valida si la sesion que se mando coincide con la <see cref="Sesion.ID"/>
        /// y <see cref="Sesion.CodigoDeAutenticacion"/> en <see cref="SesionesConectadas"/>
        /// </summary>
        /// <param name="sesion"></param>
        /// <returns></returns>
        private bool ValidarAutenticidadDeSesion(Sesion sesion)
        {
            bool resultadoDeValidacion = false;
            if (!string.IsNullOrEmpty(sesion.ID))
            {
                Predicate<Sesion> buscarSesionPorID = s => s.ID == sesion.ID;
                resultadoDeValidacion = SesionesConectadas.Exists(buscarSesionPorID);
            }

            return resultadoDeValidacion;
        }
    }
}
