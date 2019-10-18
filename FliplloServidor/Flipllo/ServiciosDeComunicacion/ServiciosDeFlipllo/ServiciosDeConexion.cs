using LogicaDeNegocios.ClasesDeDominio;
using ServiciosDeComunicacion.InterfacesDeServicios;
using LogicaDeNegocios.ObjetosDeAccesoADatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiciosDeComunicacion.ServiciosDeFlipllo
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class ServiciosDeFlipllo : IServiciosDeFlipllo
    {
        public List<Sesion> UsuariosConectados = new List<Sesion>();
        private IControladorDeServiciosDeFlipllo ControladorServiciosDeFlipllo;

        public ServiciosDeFlipllo(IControladorDeServiciosDeFlipllo controladorServiciosDeFlipllo)
        {
            ControladorServiciosDeFlipllo = controladorServiciosDeFlipllo;
        }


        public bool RegistrarUsuario(Usuario usuario)
        {
            bool resultadoDeRegistro = false;
            if (usuario != null)
            {
                try
                {
                    usuario.Registrar();
                    resultadoDeRegistro = true;
                }
                catch(Exception e)
                {
                    resultadoDeRegistro = false;
                }
            }
            return resultadoDeRegistro;
        }

        public void AutenticarUsuario(Usuario usuario)
        {
            Sesion sesion = new Sesion()
            {
                Creacion = DateTime.Now,
                UltimaActualizacion = DateTime.Now,
                Usuario = new Usuario()
            };
            sesion.CanalDeCallback = OperationContext.Current.GetCallbackChannel<IServiciosDeCallBack>();

            if (usuario.ValidarCredenciales())
            {
                Usuario usuarioCargado = usuario.CargarUsuarioPorCorreo();
                if (usuarioCargado.Estado == EstadoUsuario.Registrado)
                {
                    sesion.ID = UsuariosConectados.Count + 1;
                    sesion.Usuario.CorreoElectronico = usuario.CorreoElectronico;
                    UsuariosConectados.Add(sesion);
                    ControladorServiciosDeFlipllo.ListaDeSesionesActualizado(UsuariosConectados);
                }
                else
                {
                    sesion.Usuario.Estado = usuarioCargado.Estado;
                }
            }
            //CommunicationObjectAbortedException canal de callback agotado
            sesion.CanalDeCallback.RecibirSesion(sesion);
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
                    resultadoDeValidacion = true;
                }
            }
            return resultadoDeValidacion;
        }

        public void ActualizarSesion(Sesion sesion)
        {
            throw new NotImplementedException();
        }

    }
}
