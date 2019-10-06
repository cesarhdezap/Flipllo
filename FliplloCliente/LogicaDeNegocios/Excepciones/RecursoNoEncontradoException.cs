using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.Excepciones
{
	public class RecursoNoEncontradoException : Exception
	{
		public RecursoNoEncontradoException()
		{
		}

		public RecursoNoEncontradoException(string mensaje)
			: base(mensaje)
		{
		}

		public RecursoNoEncontradoException(string mensaje, Exception excepcionInterna)
			: base(mensaje, excepcionInterna)
		{
		}
	}
}
