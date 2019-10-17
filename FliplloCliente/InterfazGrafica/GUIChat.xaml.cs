using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ServiciosDeComunicacion;
using ServiciosDeComunicacion.Proxy;
using LogicaDeNegocios.Servicios;
using LogicaDeNegocios.ServiciosDeFlipllo;
using LogicaDeNegocios.ClasesDeDominio;
using System.Threading;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for Chat.xaml
	/// </summary>
	public partial class GUIMenuPrincipal : Window
	{
        public Servidor Servidor;
        public Chat Chat;
		public Sesion SesionLocal;
        

        public GUIMenuPrincipal(Sesion Sesion)
		{
            InitializeComponent();
            Chat = new Chat();
			SesionLocal = Sesion;
		
            ServiciosDeCallBack servicioDeCliente = new ServiciosDeCallBack();
            servicioDeCliente.ListaDeClientesConectadosEvent += ActualizarListaDeClientesConectados;
            servicioDeCliente.NuevoMensajeRecibidoEvent += MostrarNuevoMensaje;
            servicioDeCliente.ActualizarIDDeUsuarioEvent += ActualizarIDDeUsuario;
			
            Servidor = new Servidor(servicioDeCliente);
			Servidor.CrearCanal();
			Servidor.CanalDelServidor.ConectarDelChat(SesionLocal);

            if (SesionLocal.ID > 0)
            {
                MessageBox.Show("Usuario ya conectado");
            }
           
        }

        private void ActualizarListaDeClientesConectados(List<Sesion> clientesConectados)
        {
            Chat.UsuariosConectados = clientesConectados;
            DataGridUsuariosConectados.ItemsSource = Chat.UsuariosConectados;
        }

        private void MostrarNuevoMensaje(Mensaje mensaje)
        {
            Chat.MensajesRecibidos.Add(mensaje);
            TextBlockChatBox.Text = Chat.MensajesToString();
        }

        private void ActualizarIDDeUsuario(int id)
        {
            SesionLocal.ID = id;
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxMessageBox.Text) && SesionLocal.ID > 0)
            {
				Mensaje mensaje = new Mensaje
				{
					IDDeUsuario = SesionLocal.ID,
					Fecha = DateTime.Now,
					CuerpoDeMensaje = TextBoxMessageBox.Text
				};
				Servidor.CanalDelServidor.EnviarMensaje(mensaje);
                TextBoxMessageBox.Text = string.Empty;
            }
        }
    }

}
