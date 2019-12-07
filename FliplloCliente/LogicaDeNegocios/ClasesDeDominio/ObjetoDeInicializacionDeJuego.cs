using LogicaDeNegocios.ServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogicaDeNegocios.ServiciosDeRecursos;

namespace LogicaDeNegocios.ClasesDeDominio
{
	public class ObjetoDeInicializacionDeJuego
	{
		public TipoDeJuego TipoDeJuego { get; }
		public string SkinNegra { get; }
		public string SkinBlanca { get; }
		public List<Jugador> Jugadores { get; }

		public ObjetoDeInicializacionDeJuego(TipoDeJuego tipoDeJuego, string skinNegra = "Default", string skinBlanca = "Default", List<Jugador> jugadores = null)
		{
			TipoDeJuego = tipoDeJuego;
			SkinBlanca = skinBlanca;
			SkinNegra = skinNegra;
			Jugadores = jugadores;
		}

		public void CargarSkins()
		{
			CambiarSkin(SkinBlanca, ColorDeFicha.Blanco);
			CambiarSkin(SkinNegra, ColorDeFicha.Negro);
		}
	}
}
