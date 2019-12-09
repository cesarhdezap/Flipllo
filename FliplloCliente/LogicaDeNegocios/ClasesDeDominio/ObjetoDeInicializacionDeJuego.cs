using LogicaDeNegocios.ServiciosDeFlipllo;
using LogicaDeNegocios.ServiciosDeJuego;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogicaDeNegocios.ServiciosDeRecursos;

namespace LogicaDeNegocios.ClasesDeDominio
{
	/// <summary>
	/// Objeto de parametros para inicializar un juego
	/// </summary>
	public class ObjetoDeInicializacionDeJuego
	{
		/// <summary>
		/// El tiempo de juego a inicializar
		/// </summary>
		public TipoDeJuego TipoDeJuego { get; set; }

		/// <summary>
		/// El nombre de la skin negra que se usara en el juego
		/// </summary>
		public string SkinNegra { get; set; }

		/// <summary>
		/// El nombre de la skin blanca que se usara en el juego
		/// </summary>
		public string SkinBlanca { get; set; }

		/// <summary>
		/// Lista de los jugadores del juego
		/// </summary>
		public List<Jugador> Jugadores { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tipoDeJuego">El tipo de juego</param>
		/// <param name="skinNegra">La skin negra. Por defecto se usa la skin Default</param>
		/// <param name="skinBlanca">La skin negra. Por defecto se usa la skin Default</param>
		/// <param name="jugadores">Una lista de los jugadores a usar en el juego</param>
		public ObjetoDeInicializacionDeJuego(TipoDeJuego tipoDeJuego, string skinNegra = "Default", string skinBlanca = "Default", List<Jugador> jugadores = null)
		{
			TipoDeJuego = tipoDeJuego;
			SkinBlanca = skinBlanca;
			SkinNegra = skinNegra;
			Jugadores = jugadores;
		}

		public ObjetoDeInicializacionDeJuego()
		{
		}

		/// <summary>
		/// Carga las skins seleccionadas para cada jugador a memoria
		/// </summary>
		public void CargarSkins()
		{
			CambiarSkin(SkinBlanca, ColorDeFicha.Blanco);
			CambiarSkin(SkinNegra, ColorDeFicha.Negro);
		}
	}
}
