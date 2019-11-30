using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using LogicaDeNegocios.ClasesDeDominio;

namespace ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo
{
    [ServiceContract(CallbackContract = typeof(IServiciosDeCallBack))]
    public partial interface IServiciosDeFlipllo
    {
        [OperationContract(IsOneWay = true)]
        void AutenticarUsuario(Usuario usuario);

        [OperationContract]
        bool RegistrarUsuario(Usuario usuario);

        /// <summary>
        /// Valida el codigo del usuario con <see cref="Usuario.CodigoDeVerificacion"/>,
        /// <see cref="Usuario.CorreoElectronico"/> y <see cref="Usuario.Contraseña"/>
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [OperationContract]
        bool ValidarCodigoDeUsuario(Usuario usuario);

        [OperationContract(IsOneWay = true)]
        void ActualizarSesion(Sesion sesion);

        [OperationContract(IsOneWay = true)]
        void CerrarSesion(Sesion sesion);
    }

    [DataContract]
    public class Sesion
    {
        private string id;
        private DateTime creacion;
        private DateTime ultimaActualizacion;
        private LogicaDeNegocios.ClasesDeDominio.Usuario usuario;

        [DataMember]
        public string ID { get { return id; } set { id = value; } }
        [DataMember]
        public DateTime Creacion { get { return creacion; } set { creacion = value; } }
        [DataMember]
        public Usuario Usuario { get { return usuario; } set { usuario = value; } }
        [DataMember]
        public DateTime UltimaActualizacion { get { return ultimaActualizacion; } set { ultimaActualizacion = value; } }
		[IgnoreDataMember]
		public IServiciosDeCallBack CanalDeCallback { get; set; }
    }

}
