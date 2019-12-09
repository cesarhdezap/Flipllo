using LogicaDeNegocios.ClasesDeDominio;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeJuego;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion.Interfaces
{
    [DataContract]
    public class Sala
    {
        private string id;
        private string nombre;
        private int nivelMinimo;
        private int nivelMaximo;
        private string idSesionCreadora;
        private List<Jugador> jugadores;
        private EstadoSala estado;
        private Juego juego;

        [DataMember]
        public string ID { get { return id; } set { id = value; } }
        [DataMember]
        public string NombreDeUsuarioCreador { get { return idSesionCreadora; } set { idSesionCreadora = value; } }
        [DataMember]
        public string Nombre { get { return nombre; } set { nombre = value; } }
        [DataMember]
        public int NivelMinimo { get { return nivelMinimo; } set { nivelMinimo = value; } }
        [DataMember]
        public int NivelMaximo { get { return nivelMaximo; } set { nivelMaximo = value; } }
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
        private IServiciosDeCallBackJuego canalDeCallbackJuego;

        [DataMember]
        public ColorDeFicha Color { get { return color; } set { color = value; } }
        [DataMember]
        public Sesion Sesion { get { return sesion; } set { sesion = value; } }
        [DataMember]
        public bool ListoParaJugar { get { return listoParaJugar; } set { listoParaJugar = value; } }
        [IgnoreDataMember]
        public IServiciosDeCallBackJuego CanalDeCallbackJuego { get { return canalDeCallbackJuego; } set { canalDeCallbackJuego = value; } }
    }

    public enum EstadoSala
    {
        Inexistente,
        Registrada,
        EnPartida,
        CupoLleno
    }
}
