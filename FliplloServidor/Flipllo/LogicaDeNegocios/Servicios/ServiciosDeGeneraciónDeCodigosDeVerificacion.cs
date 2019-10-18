using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.Servicios
{
    public static class ServiciosDeGeneracionDeCodigosDeVerificacion
    {
        private static readonly string CARACTERES_VALIDOS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private static readonly Random generadorDeNumeros = new Random();

        public static string GenerarCodigo()
        {
            string codigoGenerado = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                codigoGenerado += CARACTERES_VALIDOS[generadorDeNumeros.Next(1, CARACTERES_VALIDOS.Length - 1)];
            }
            return codigoGenerado;
        }
    }
}
