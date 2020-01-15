using LogicaDeNegocios;
using LogicaDeNegocios.ClasesDeDominio;
using ServiciosDeComunicacion.Clases;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeJuego;
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
        [OperationContract/*(IsOneWay = true)*/]
        Sala CrearSala(Sala sala, Sesion sesion, ColorDeFicha color);

        [OperationContract]
        List<Sala> SolicitarSalas(Sesion sesion);

        [OperationContract(IsOneWay = true)]
        void IngresarASala(Sesion sesion, Sala sala);

        [OperationContract(IsOneWay = true)]
        void DesconectarDeSala(Sesion sesion);

        [OperationContract(IsOneWay = true)]
        void CambiarColorDeJugadorEnSala(Sesion sesion, ColorDeFicha color);

        [OperationContract(IsOneWay = true)]
        void CambiarConfiguracionDeLaSala(Sesion sesion, Sala sala);

        [OperationContract(IsOneWay = true)]
        void AlternarListoParaJugar(Sesion sesion);

        [OperationContract]
        public bool IniciarJuego(Sesion sesion);

        [OperationContract(IsOneWay = true)]
        void CambiarSkinDeFicha(Sesion sesion, string nombreSkin);

        [OperationContract]
        List<Usuario> SolicitarTopDePuntuacionesDeUsuarios(Sesion sesion);
    }
}
