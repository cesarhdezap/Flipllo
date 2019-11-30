using LogicaDeNegocios;
using LogicaDeNegocios.ClasesDeDominio;
using ServiciosDeComunicacion.Clases;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo
{
    public partial interface IServiciosDeFlipllo
    {
        /// <summary>
        /// Crea una sala en el servidor
        /// </summary>
        /// <param name="sala"><see cref="Sala"/> con <s"/></param>
        /// <param name="sesion"></param>
        [OperationContract(IsOneWay = true)]
        void CrearSala(Sala sala, Sesion sesion);

        [OperationContract(IsOneWay = true)]
        void BorrarSala(Sesion sesion);

        [OperationContract]
        List<Sala> SolicitarSalas(Sesion sesion);

        [OperationContract]
        void IngresarASala(Sesion sesion, Sala sala);

        [OperationContract(IsOneWay = true)]
        void DesconectarDeSala(Sesion sesion);

        [OperationContract]
        void CambiarColorDeJugadorEnSala(Sesion sesion, ColorDeFicha color);

        [OperationContract]
        void CambiarConfiguracionDeLaSala(Sesion sesion, Sala sala);

        [OperationContract]
        void IniciarJuego(Sesion sesion);

        [OperationContract(IsOneWay = true)]
        void AlternarListoParaJugar(Sesion sesion);
    }

   [DataContract]
    public class Sala
    {
        private string id;
        private string nombre;
        private int nivelMinimo;
        private int nivelMaximo;
        private Chat chat;
        private string idSesionCreadora;
        private List<Jugador> jugadores;
        private EstadoSala estado;
        private Juego juego;

        [DataMember]
        public string ID { get { return id; } set { id = value; } }
        [DataMember]
        public string IDSesionCreadora { get { return idSesionCreadora; } set { idSesionCreadora = value; } }
        [DataMember]
        public string Nombre { get { return nombre; } set { nombre = value; } }
        [DataMember]
        public int NivelMinimo { get { return nivelMinimo; } set { nivelMinimo = value; } }
        [DataMember]
        public int NivelMaximo { get { return nivelMaximo; } set { nivelMaximo = value; } }
        [DataMember]
        public Chat ChatDeSala { get { return chat; } set { chat = value; } }
        [DataMember]
        public List<Jugador> Jugadores { get { return jugadores; } set { jugadores = value; } }
        [DataMember]
        public EstadoSala Estado { get { return estado; } set { estado = value; } }
        [IgnoreDataMember]
        public Juego Juego { get { return juego; } set { juego = value; } }
    }

    [DataContract]
    public class Jugador
    {
        private ColorDeFicha color;
        private Sesion sesion;
        private bool listoParaJugar;
        [DataMember]
        public ColorDeFicha Color { get { return color; } set { color = value; } }
        [DataMember]
        public Sesion Sesion { get { return sesion; } set { sesion = value; } }
        [DataMember]
        public bool ListoParaJugar { get { return listoParaJugar; } set { listoParaJugar = value; } }
    }

    public enum EstadoSala
    {
        Inexistente,
        Registrada,
        EnPartida,
        CupoLleno
    }
}
