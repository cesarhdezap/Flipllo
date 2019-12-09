using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiciosDeComunicacion.Clases;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using ServiciosDeComunicacion.Servicios;

namespace PruebasUnitarias.PruebasDeServicios
{
    [TestClass]
    public class PruebasDeServiciosDeFliplloSala
    {
        [TestMethod]
        public void ProbarIngresarASala_OperacionNormal_Void()
        {
            ObservadorServiciosDeCallbackFlipllo observadorServiciosDeCallbackFlipllo = new ObservadorServiciosDeCallbackFlipllo();
            ServiciosDeFlipllo serviciosDeFlipllo = new ServiciosDeFlipllo();
            HostDeServiciosDeFlipllo hostDeServiciosDeFlipllo;
            
        }
    }
}
