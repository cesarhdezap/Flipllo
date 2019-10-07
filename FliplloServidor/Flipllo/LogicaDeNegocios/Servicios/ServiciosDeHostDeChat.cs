using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading;
using ServiciosDeComunicacion;

namespace LogicaDeNegocios.Servicios
{
	public class ServiciosDeHostDeChat
	{
		private Thread HiloDeEscucha;
		public bool ServidorActivo = false;

		public void IniciarServidor()
		{
			HiloDeEscucha = new Thread(IniciarHost);
			HiloDeEscucha.Start();
		}

		private void IniciarHost()
		{
			using (ServiceHost host = new ServiceHost(typeof(ServiciosDeComunicacion.ServiciosDeChat)))
			{
				host.Open();
				ServidorActivo = true;
				while (ServidorActivo);
			}
		}

		public void PararHost()
		{
			HiloDeEscucha.Abort();
			ServidorActivo = false;
		}
	}
}
