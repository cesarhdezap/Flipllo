using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.Servicios
{
    public static class ServiciosDeGeneracionDeCodigos
    {
        private static readonly string CARACTERES_VALIDOS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private static readonly Random generadorDeNumeros = new Random();
        private static readonly int LONGITUD_DE_CODIGO_DE_VERIFICACION = 5;

        public static string GenerarCodigoDeValidacionDeCorreo()
        {
            StringBuilder codigoGenerado = new StringBuilder();
            for (int i = 0; i < LONGITUD_DE_CODIGO_DE_VERIFICACION; i++)
            {
                codigoGenerado.Append(CARACTERES_VALIDOS[generadorDeNumeros.Next(1, CARACTERES_VALIDOS.Length - 1)]);
            }
            return codigoGenerado.ToString();
        }
    }
}
