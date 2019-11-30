﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.Servicios
{
	public static class ServiciosDeEncriptacion
	{
		/// <summary>
		/// Encripta con hash 256 la contraseña del usuario.
		/// </summary>
		/// <param name="contraseña">Contraseña en cadena de carácteres.</param>
		/// <returns>Cadena con la contraseña en SHA256.</returns>
		public static string EncriptarCadena(string contraseña)
		{
			StringBuilder cadenaFinal = new StringBuilder();

			using (SHA256 hash = SHA256.Create())
			{
				byte[] ContrasenaEncriptada = hash.ComputeHash(Encoding.UTF8.GetBytes(contraseña));

				try
				{
					for (int indice = 0; indice < ContrasenaEncriptada.Length; indice++)
					{
						cadenaFinal.Append(ContrasenaEncriptada[indice].ToString("x2"));
					}

				}
				catch (IOException excepcionIO)
				{
					Console.WriteLine("\n Excepcion: " + excepcionIO.StackTrace.ToString());
				}
			}

			return cadenaFinal.ToString();
		}
	}
}
