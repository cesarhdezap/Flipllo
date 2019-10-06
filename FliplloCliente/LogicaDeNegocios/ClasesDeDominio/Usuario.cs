using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.ClasesDeDominio
{
	public class Usuario
	{
		public int ID { get; set; }
		public string NombreDeUsuario { get; set; }
		public string Contraseña { get; set; }
		public string CorreoElectronico { get; set; }

		public bool Validar()
		{
			return true;
		}
	} 
}
