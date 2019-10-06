using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogicaDeNegocios.Servicios
{
	public static class ServiciosDeValidacion
	{
		/// <summary>
		/// Expresión regular que valida que la cadena tenga al menos una letra seguida
		/// de un arroba, al menos otras dos letras, un punto y al menos otras dos letras.
		/// </summary>
		private static readonly Regex ExpresionRegularCorreoElectronico = new Regex(@"^(\D)+(\w)*((\.(\w)+)?)+@(\D)+(\w)*((\.(\D)+(\w)*)+)?(\.)[a-z]{2,}$");
		/// <summary>
		/// Expresión regular que valida que la cadena no tenga espacios en blanco y sea de 6 a 255 de longitud.
		/// </summary>
		private static readonly Regex ExpresionRegularContraseña = new Regex(@"^\S{6,255}$");
		/// <summary>
		/// Expresión regular que valida que el nombre de usuario solo tenga letras, numeros y guiones bajos, que no empieze ni termine con guin bajo, que no haya mas de un guin bajo consecutivo y que tenga entre 6 y 20 caracters de longitud.
		/// </summary>
		private static readonly Regex ExpresionRegularNombreDeUsuario = new Regex(@"^(?=.{6,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$");
		public const int TAMAÑO_MAXIMO_VARCHAR = 255;

		/// <summary>
		/// Valida la estructura de la cadena del correo del usuario.
		/// </summary>
		/// <param name="correoElectronico">Correo del usuario.</param>
		/// <returns>Si la cadena cumple con la validación.</returns>
		public static bool ValidarCorreoElectronico(string correoElectronico)
		{
			bool resultadoDeValidacion = false;

			if (correoElectronico.Length <= TAMAÑO_MAXIMO_VARCHAR)
			{
				if (ExpresionRegularCorreoElectronico.IsMatch(correoElectronico))
				{
					resultadoDeValidacion = true;
				}
			}

			return resultadoDeValidacion;
		}

		/// <summary>
		/// Valida la estructura de la cadena de la contraseña del usuario.
		/// </summary>
		/// <param name="contraseña">Contraseña del usuario.</param>
		/// <returns>Si la cadena cumple con la validación.</returns>
		public static bool ValidarContraseña(string contraseña)
		{
			bool resultadoDeValidacion = false;

			if (ExpresionRegularContraseña.IsMatch(contraseña))
			{
				resultadoDeValidacion = true;
			}

			return resultadoDeValidacion;
		}

		public static bool ValidarNombreDeUsuario(string nombreDeUsuario)
		{
			bool resultadoDeValidacion = false;

			if (ExpresionRegularNombreDeUsuario.IsMatch(nombreDeUsuario))
			{
				resultadoDeValidacion = true;
			}

			return resultadoDeValidacion;
		}
	}
}
