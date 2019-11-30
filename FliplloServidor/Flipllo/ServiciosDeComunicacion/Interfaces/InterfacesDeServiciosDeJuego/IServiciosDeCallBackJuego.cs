using LogicaDeNegocios;
using LogicaDeNegocios.ClasesDeDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeJuego
{
    public interface IServiciosDeCallBackJuego
    {
        [OperationContract(IsOneWay = true)]
        void RecibirTablero(List<Ficha> tablero);
        [OperationContract(IsOneWay = true)]
        void TerminarJuego(int experenciaPorPuntos, int experenciaPorFichas, bool ganaste);
        [OperationContract(IsOneWay = true)]
        void RecibirTirada(Point tirada);

    }
}
