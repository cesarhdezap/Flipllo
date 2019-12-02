using LogicaDeNegocios.Servicios;
using LogicaDeNegocios.ServiciosDeFlipllo;
using System.ServiceModel;
using System.Windows;

namespace ServiciosDeComunicacion.Proxy
{
    public class Servidor : DuplexClientBase<IServiciosDeFlipllo>
    {
		public IServiciosDeFlipllo CanalDelServidor { get; set; }
		public CallBackDeFlipllo CanalDeCallback { get; set; }

		public Servidor(IServiciosDeFliplloCallback callback) : base (callback)
        {
			ServiciosDeComunicacion.CallBackDeFlipllo
			CanalDeCallback = (CallBackDeFlipllo)callback;
		}
		public void CrearCanal()
		{
			CanalDelServidor = ChannelFactory.CreateChannel();
		}

		public void RestaurarCanal()
		{
			CanalDelServidor = ChannelFactory.CreateChannel();
		}
    }
}
