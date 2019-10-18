using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LogicaDeNegocios.ClasesDeDominio;

namespace ServiciosDeComunicacion.InterfacesDeServicios
{
    public partial interface IServiciosDeFlipllo
    {
        [OperationContract(IsOneWay = true)]
        void AutenticarUsuario(Usuario usuario);

        [OperationContract]
        bool RegistrarUsuario(Usuario usuario);

        [OperationContract]
        bool ValidarCodigoDeUsuario(Usuario usuario);

        [OperationContract(IsOneWay = true)]
        void ActualizarSesion(Sesion sesion);
    }

    [DataContract]
    public class Sesion
    {
        private int id;
        private DateTime creacion;
        private DateTime ultimaActualizacion;
        private LogicaDeNegocios.ClasesDeDominio.Usuario usuario;

        [DataMember]
        public int ID { get { return id; } set { id = value; } }
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
