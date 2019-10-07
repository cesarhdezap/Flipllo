using ServiciosDeComunicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogicaDeNegocios
{
    public class ServiciosDeHost
    {
        private Thread HiloDeEscucha;
        public bool ServidorActivo = false;

        public void IniciarServidor()
        {
            HiloDeEscucha = new Thread(IniciarHost);
            HiloDeEscucha.Name = DateTime.Now.ToString();
            HiloDeEscucha.Start();
        }

        private void IniciarHost()
        {
            using (ServiceHost host = new ServiceHost(typeof(ServiciosDeComunicacion.ServiciosDeComunicacion)))
            {
                host.Open();
                ServidorActivo = true;
                while (ServidorActivo) ;
            }
        }

        public void PararHost()
        {
            HiloDeEscucha.Abort();
            ServidorActivo = false;
        }
    }
}
