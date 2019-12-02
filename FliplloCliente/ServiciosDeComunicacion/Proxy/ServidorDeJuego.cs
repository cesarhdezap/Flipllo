using LogicaDeNegocios.ServiciosDeJuego;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosDeComunicacion.Proxy
{
	public class ServidorDeJuego : DuplexClientBase<IServiciosDeJuego>
	{
		public IServiciosDeJuego CanalDelServidor { get; set; }

		public ServidorDeJuego(IServiciosDeJuegoCallback callback) : base(callback)
		{
		}
		public void CrearCanal()
		{
			CanalDelServidor = ChannelFactory.CreateChannel();
		}

		public void RestaurarCanal()
		{
			CanalDelServidor = ChannelFactory.CreateChannel();
		}

		public Sesion ConvertirSesion(LogicaDeNegocios.ServiciosDeFlipllo.Sesion sesionEntrante)
		{
			Sesion sesionSaliente = new Sesion()
			{
				ID = sesionEntrante.ID
			};

			return sesionSaliente;
		}
	

	}
}
