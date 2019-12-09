using ServiciosDeComunicacion.Interfaces;
using ServiciosDeComunicacion.Interfaces.Controladores;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeJuego;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiciosDeComunicacion.Servicios
{
    [ServiceBehavior(InstanceContextMode =InstanceContextMode.Single)]
    public class ServiciosDeJuego : IServiciosDeJuego
    {
        private readonly int MULTIPLICADOR_DE_EXPERIENCIA_DE_FICHAS = 10;
        private readonly int EXPERIENCIA_POR_GANAR = 100;
        public List<Sesion> SesionesConectadas;
        public List<Sala> SalasCreadas;
        IControladorDeActualizacionDePantalla ControladorDeActualizacionDePantalla;

        public ServiciosDeJuego(IControladorDeActualizacionDePantalla controladorDeListas, List<Sala> salas, List<Sesion> sesiones)
        {
            ControladorDeActualizacionDePantalla = controladorDeListas;
            SalasCreadas = salas;
            SesionesConectadas = sesiones;
        }

        public void PedirActualizacionDeTablero(Sesion sesion)
        {
            Sala sala = BuscarSalaDeSesion(sesion);
            if (sala != null)
            {
                if (sala.Juego != null)
                {
                    OperationContext.Current.GetCallbackChannel<IServiciosDeCallBackJuego>().RecibirTablero(sala.Juego.ObtenerFichasComoLista());
                }
            }
        }

        public void ActualizarCanalDeCallback(Sesion sesion)
        {
            if (ValidarAutenticidadDeSesion(sesion))
            {
                Sala sala = BuscarSalaDeSesion(sesion);
                if (sala != null && sala.Juego != null)
                {
                    foreach(Jugador jugador in sala.Jugadores)
                    {
                        if (jugador.Sesion.ID == sesion.ID)
                        {
                            jugador.CanalDeCallbackJuego = OperationContext.Current.GetCallbackChannel<IServiciosDeCallBackJuego>();
                        }
                    }
                }
            }
        }

        public bool TirarFicha(Sesion sesion, Point point)
        {
            bool resultadoDeValidacion = false;
            Sala sala = BuscarSalaDeSesion(sesion);
            if (sala != null && sala.Juego != null)
            {
                if (sala.Jugadores.FirstOrDefault(s => s.Sesion.ID == sesion.ID).CanalDeCallbackJuego != null)
                {
                    if (sala.Juego.SePuedeTirar(point))
                    {
                        sala.Juego.Tirar(point);
                        resultadoDeValidacion = true;
                    }
                }
                    
            }

            if (resultadoDeValidacion)
            {
                foreach(Jugador jugador in sala.Jugadores)
                {
                    if (jugador.CanalDeCallbackJuego != null && jugador.Sesion.ID != sesion.ID)
                    {
                        jugador.CanalDeCallbackJuego.RecibirTirada(point);
                    }
                }

                if (sala.Juego.JuegoTerminado)
                {
                    NotificarJugadoresDeJuegoTerminado(sala);
                    //Borrar sala
                }
            }

            return resultadoDeValidacion;
        }

        private void NotificarJugadoresDeJuegoTerminado(Sala sala)
        {
            foreach (Jugador jugador in sala.Jugadores)
            {
                if (jugador.CanalDeCallbackJuego != null)
                {

                    int numeroDeFichas = sala.Juego.ObtenerCuentaDeFichas(jugador.Color);
                    int experienciaPorFichas = numeroDeFichas * MULTIPLICADOR_DE_EXPERIENCIA_DE_FICHAS;
                    int experienciaPorPuntos = sala.Juego.ObtenerPuntosPorColor(jugador.Color);
                    bool ganaste = sala.Juego.CalcularColorGanador() == jugador.Color;

                    int experienciaTotalGanada = experienciaPorFichas + experienciaPorPuntos;
                    if (ganaste)
                        experienciaTotalGanada += EXPERIENCIA_POR_GANAR;

                    jugador.Sesion.Usuario.AumentarPuntuacion(ganaste, experienciaTotalGanada);

                    jugador.CanalDeCallbackJuego.TerminarJuego(experienciaPorPuntos, experienciaPorFichas, ganaste);
                }
            }
        }

        /// <summary>
        /// Regresa la <see cref="Sala"/> en la que esta la <paramref name="sesion"/>
        /// </summary>
        /// <param name="sesion"></param>
        /// <returns><see cref="Sala"/> o <see cref="null"/> si no se encuentra.</returns>
        private Sala BuscarSalaDeSesion(Sesion sesion)
        {
            Sala sala = null;
            if (ValidarAutenticidadDeSesion(sesion)
                && ValidarExistenciaDeSesionEnSalasCreadas(sesion))
            {
                foreach (Sala salaBusqueda in SalasCreadas)
                {
                    bool existe = salaBusqueda.Jugadores.Exists(j => j.Sesion.ID == sesion.ID);
                    if (existe)
                    {
                        sala = salaBusqueda;
                        break;
                    }
                }
            }

            return sala;
        }

        /// <summary>
        /// Valida si la sesion que se mando coincide con la <see cref="Sesion.ID"/>
        /// y <see cref="Sesion.CodigoDeAutenticacion"/> en <see cref="SesionesConectadas"/>
        /// </summary>
        /// <param name="sesion"></param>
        /// <returns></returns>
        private bool ValidarAutenticidadDeSesion(Sesion sesion)
        {
            bool resultadoDeValidacion = false;
            if (!string.IsNullOrEmpty(sesion.ID))
            {
                Predicate<Sesion> buscarSesionPorID = s => s.ID == sesion.ID;
                resultadoDeValidacion = SesionesConectadas.Exists(buscarSesionPorID);
            }

            return resultadoDeValidacion;
        }

        private bool ValidarExistenciaDeSesionEnSalasCreadas(Sesion sesion)
        {
            bool sesionExiste = false;
            foreach (Sala sala in SalasCreadas)
            {
                bool existe = sala.Jugadores.Exists(j => j.Sesion.ID == sesion.ID);
                if (existe)
                {
                    sesionExiste = true;
                    break;
                }
            }
            return sesionExiste;
        }

    }
}
