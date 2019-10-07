using System;
using System.Collections.Generic;
using System.Linq;
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
using LogicaDeNegocios;
using LogicaDeNegocios.Servicios;
using System.Threading;

namespace InterfazGrafica
{
	/// <summary>
	/// Interaction logic for Chat.xaml
	/// </summary>
	public partial class Chat : Window
	{
		ServiciosDeHostDeChat ServidorDeChat;
		public Chat()
		{
			InitializeComponent();
			ServidorDeChat = new ServiciosDeHostDeChat();
			ServidorDeChat.IniciarServidor();
			Thread.Sleep(1000);
			if (ServidorDeChat.ServidorActivo)
			{
				LabelEstadoDelServidor.Content = "Servidor corriendo";
			}
			else
			{
				LabelEstadoDelServidor.Content = "Error";
			}
		}
	}
}
