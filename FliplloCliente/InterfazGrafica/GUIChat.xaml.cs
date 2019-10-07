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
using LogicaDeNegocios.Proxy;
using LogicaDeNegocios.Servicios;
using LogicaDeNegocios.ServiciosDeChat;
using LogicaDeNegocios.ClasesDeDominio;
using System.Threading;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for Chat.xaml
	/// </summary>
	public partial class GUIChat : Window
	{
        public ChatDelServidorProxy ProxyDelChatDelServidor;
        public IServiciosDeChat CanalDelServidor;
        public Chat Chat;
        public Usuario UsuarioLocal;

        public GUIChat()
		{
            InitializeComponent();
            Chat = new Chat();

            UsuarioLocal = new Usuario()
            {
                NombreDeUsuario = "Pipo",
                Contraseña = "pipopass",
                CorreoElectronico = "pipo@correo.com",
            };

			
            var servicioDeChatDeCliente = new ServiciosDeChatCallBack();
            servicioDeChatDeCliente.ListaDeClientesConectadosEvent += ActualizarListaDeClientesConectados;
            servicioDeChatDeCliente.NuevoMensajeRecibidoEvent += MostrarNuevoMensaje;
            servicioDeChatDeCliente.ActualizarIDDeUsuarioEvent += ActualizarIDDeUsuario;


            ProxyDelChatDelServidor = new ChatDelServidorProxy(servicioDeChatDeCliente);
            CanalDelServidor = ProxyDelChatDelServidor.ChannelFactory.CreateChannel();

            CanalDelServidor.Conectar(UsuarioLocal);

            if (UsuarioLocal.ID > 0)
            {
                MessageBox.Show("Usuario ya conectado");
            }
            

        }

        private void ActualizarListaDeClientesConectados(List<Usuario> clientesConectados)
        {
            DataGridUsuariosConectados.DataContext = clientesConectados;
            MessageBox.Show("Hola soy yo jsjs");
        }

        private void MostrarNuevoMensaje(Mensaje mensaje)
        {
            Chat.MensajesRecibidos.Add(mensaje);
            TextBlockChatBox.Text = Chat.MensajesToString();
        }

        private void ActualizarIDDeUsuario(int id)
        {
            UsuarioLocal.ID = id;
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxMessageBox.Text) && UsuarioLocal.ID > 0)
            {
                Mensaje mensaje = new Mensaje();
                mensaje.IDDeUsuario = UsuarioLocal.ID;
                mensaje.Fecha = DateTime.Now;
                mensaje.CuerpoDeMensaje = TextBoxMessageBox.Text;
                CanalDelServidor.EnviarMensaje(mensaje);
            }
        }
    }

}
