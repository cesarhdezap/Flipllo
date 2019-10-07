using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using ServiciosDeComunicacion;
using System.Text;

namespace ServiciosDeComunicacion
{
    [ServiceContract]
    public interface IAdministradorDeUsuarios
    {
        [OperationContract]
        bool ValidarCuenta(Usuario usuario);

        [OperationContract]
        bool RegistrarCuenta(Usuario usuario);
    }

    [DataContract]
    public class Usuario
    {
        private int id;
        private string nombreDeUsuario;
        private string contraseña;
        private string correoElectronico;

        [DataMember]
        public int ID { get { return id; } set { id = value; } }
        [DataMember]
        public string NombreDeUsuario { get { return nombreDeUsuario; } set { nombreDeUsuario = value; } }
        [DataMember]
        public string Contraseña { get { return contraseña; } set { contraseña = value; } }
        [DataMember]
        public string CorreoElectronico { get { return correoElectronico; } set { correoElectronico = value; } }
		[IgnoreDataMember]
		public IServiciosDeChatCallback canalDeCallback { get; set; }
    }
}
