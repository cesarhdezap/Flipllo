using LogicaDeNegocios.ClasesDeDominio;
using ServiciosDeComunicacion.Interfaces;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using static LogicaDeNegocios.Servicios.ServiciosDeLogicaDeJuego;

namespace ServiciosDeComunicacion.Servicios
{
    public partial class ServiciosDeFlipllo
    {
        public List<Sala> SalasCreadas;
        private readonly int NUMERO_MAXIMO_DE_JUGADORES_EN_SALA = 2;

        public Sala CrearSala(Sala sala, Sesion sesion, ColorDeFicha colorDeFicha)
        {
            Sala salaAEnviar = new Sala();
            if (ValidarAutenticidadDeSesion(sesion)
                && !ValidarExistenciaDeSesionEnSalasCreadas(sesion)
                && ValidarDatosDeSala(sala)
                && !ValidarExistenciaDeSala(sala))
            {
                Sesion sesionCargada = SesionesConectadas.FirstOrDefault(s => s.ID == sesion.ID);
                Sala salaAGuardar = new Sala
                {
                    ID = Guid.NewGuid().ToString(),
                    Nombre = sala.Nombre,
                    NivelMaximo = sala.NivelMaximo,
                    NivelMinimo = sala.NivelMinimo,
                    NombreDeUsuarioCreador = sesionCargada.Usuario.NombreDeUsuario,
                    Estado = EstadoSala.Registrada,
                };

                Jugador jugador = new Jugador
                {
                    Color = colorDeFicha,
                    Sesion = sesionCargada,
                    ListoParaJugar = false
                };

                salaAGuardar.Jugadores = new List<Jugador>();
                salaAGuardar.Jugadores.Add(jugador);

                SalasCreadas.Add(salaAGuardar);
                salaAEnviar = salaAGuardar;
                if (ControladorServiciosDeFlipllo != null)
                    ControladorServiciosDeFlipllo.ListaDeSalasActualizado(SalasCreadas);


            }
            return salaAEnviar;
        }


        public void CambiarSkinDeFicha(Sesion sesion, string nombreSkin)
        {
            if (ValidarAutenticidadDeSesion(sesion))
            {
                Sala sala = BuscarSalaDeSesion(sesion);
                if (sala != null)
                {
                    foreach (Jugador jugador in sala.Jugadores)
                    {
                        if (jugador.Sesion.ID != sesion.ID)
                        {
                            try
                            {
                                jugador.Sesion.CanalDeCallback.SkinDeOponenteActualizada(nombreSkin);
                            }
                            catch (CommunicationObjectAbortedException e)
                            {
                                NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
                                CerrarSesion(sesion);
                                logger.Warn(e);
                            }
                        }
                    }
                }
            }
        }

        public void DesconectarDeSala(Sesion sesion)
        {
            Sala sala = BuscarSalaDeSesion(sesion);

            if (sala != null)
            {
                int indiceJugador = sala.Jugadores.FindIndex(j => j.Sesion.ID == sesion.ID);
                sala.Jugadores.RemoveAt(indiceJugador);
                if (sala.NombreDeUsuarioCreador == sesion.ID)
                {
                    foreach (Jugador jugador in sala.Jugadores)
                    {
                        jugador.Sesion.CanalDeCallback.SalaBorrada();
                    }
                    SalasCreadas.Remove(sala);
                }
                else
                {
                    foreach (Jugador jugador in sala.Jugadores)
                    {
                        Sala salaClonada = ClonarSala(sala);
                        try
                        {
                            jugador.Sesion.CanalDeCallback.ActualizarSala(salaClonada);
                        }
                        catch (CommunicationObjectAbortedException e)
                        {
                            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
                            CerrarSesion(sesion);
                            logger.Warn(e);
                        }
                        catch (ObjectDisposedException e)
                        {
                            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
                            CerrarSesion(sesion);
                            logger.Warn(e);
                        }
                    }
                }
                Sesion sesionCargada = SesionesConectadas.FirstOrDefault(s => s.ID == sesion.ID);

                if (sala.Jugadores.Count <= 0)
                {
                    SalasCreadas.Remove(sala);
                }

                if (ControladorServiciosDeFlipllo != null)
                    ControladorServiciosDeFlipllo.ListaDeSalasActualizado(SalasCreadas);
            }
        }

        public void CambiarConfiguracionDeLaSala(Sesion sesion, Sala sala)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Regresa la lista de salas registradas en el servidor, se regresan las salas con sesiones ya que
        /// nunca se registra una sesion con información sensible.
        /// </summary>
        /// <param name="sesion"></param>
        /// <returns>
        /// <see cref="SalasCreadas"/> cuando la <paramref name="sesion"/> es valida
        /// y una lista vacia cuando es inválida
        /// </returns>
        public List<Sala> SolicitarSalas(Sesion sesion)
        {
            List<Sala> salas = new List<Sala>();

            if (ValidarAutenticidadDeSesion(sesion))
            {
                salas = ClonarListaDeSalas(SalasCreadas);
                foreach (Sala sala in salas)
                {
                    RemoverIDsDeOtrasSesionesAListaDeJugadores(sala.Jugadores, sesion.ID);
                }
            }

            return salas;
        }

        /// <summary>
        /// Valida la <paramref name="sesion"/> y la <paramref name="sala"/> para registrar la sesión en esta
        /// </summary>
        /// <param name="sala"></param>
        /// <param name="sesion"></param>
        public void IngresarASala(Sesion sesion, Sala sala)
        {
            if (ValidarAutenticidadDeSesion(sesion)
                && ValidarExistenciaDeSala(sala)
                && !ValidarExistenciaDeSesionEnSalasCreadas(sesion))
            {
                Sala salaLocal = SalasCreadas.FirstOrDefault(s => s.ID == sala.ID);
                Sesion sesionLocal = SesionesConectadas.FirstOrDefault(s => s.ID == sesion.ID);
                if (salaLocal.Jugadores.Count < NUMERO_MAXIMO_DE_JUGADORES_EN_SALA)
                {
                    if (sesionLocal.Usuario.Puntuacion.Nivel >= salaLocal.NivelMinimo
                    && sesionLocal.Usuario.Puntuacion.Nivel <= salaLocal.NivelMaximo)
                    {
                        ColorDeFicha colorDeCreador = ColorDeFicha.Ninguno;
                        foreach (Jugador jugadorEnSala in salaLocal.Jugadores)
                        {
                            if (jugadorEnSala.Sesion.Usuario.NombreDeUsuario == salaLocal.NombreDeUsuarioCreador)
                            {
                                colorDeCreador = jugadorEnSala.Color;
                            }
                        }

                        Jugador jugador = new Jugador
                        {
                            Color = ColorContrario(colorDeCreador),
                            Sesion = sesionLocal,
                            ListoParaJugar = false
                        };

                        int indiceSalaLocal = SalasCreadas.IndexOf(salaLocal);
                        SalasCreadas[indiceSalaLocal].Jugadores.Add(jugador);


                        NotificarCambioEnSalaAJugadores(SalasCreadas[indiceSalaLocal]);

                        if (ControladorServiciosDeFlipllo != null)
                            ControladorServiciosDeFlipllo.ListaDeSalasActualizado(SalasCreadas);
                    }
                    else
                    {
                        int indiceSala = SalasCreadas.IndexOf(salaLocal);
                        SalasCreadas[indiceSala].Estado = EstadoSala.CupoLleno;
                        if (ControladorServiciosDeFlipllo != null)
                            ControladorServiciosDeFlipllo.ListaDeSalasActualizado(SalasCreadas);
                    }
                }

            }
        }


        /// <summary>
        /// Cambia el color del jugador creador de una sala y cambia el de los demás al color contrario
        /// </summary>
        /// <param name="sesion">Sesion creadora de la sala</param>
        /// <param name="color">Color de la sesion a cambiar</param>
        public void CambiarColorDeJugadorEnSala(Sesion sesion, ColorDeFicha color)
        {
            Sala sala = BuscarSalaDeSesion(sesion);
            if (sala != null)
            {
                if (sesion.Usuario.NombreDeUsuario == sala.NombreDeUsuarioCreador)
                {
                    int indiceSala = SalasCreadas.IndexOf(sala);
                    sala.Jugadores.FirstOrDefault(j => j.Sesion.ID == sesion.ID).Color = color;
                    if (color != ColorDeFicha.Ninguno)
                    {
                        foreach (Jugador jugador in sala.Jugadores)
                        {
                            jugador.Color = ColorContrario(color);
                        }
                    }

                    NotificarCambioEnSalaAJugadores(SalasCreadas[indiceSala]);
                }
            }
        }

        public void AlternarListoParaJugar(Sesion sesion)
        {
            Sala sala = BuscarSalaDeSesion(sesion);
            if (sala != null)
            {
                int indiceSala = SalasCreadas.IndexOf(sala);
                bool listoParaJugarActual = sala.Jugadores.FirstOrDefault(j => j.Sesion.ID == sesion.ID).ListoParaJugar;
                sala.Jugadores.FirstOrDefault(j => j.Sesion.ID == sesion.ID).ListoParaJugar = !listoParaJugarActual;
                SalasCreadas[indiceSala] = sala;
                NotificarCambioEnSalaAJugadores(SalasCreadas[indiceSala]);
            }
        }

        public bool IniciarJuego(Sesion sesion)
        {
            bool resultadoDeValidacion = false;

            if (ValidarAutenticidadDeSesion(sesion))
            {
                Sala sala = BuscarSalaDeSesion(sesion);

                if (sala != null)
                {
                    bool jugadoresListos = sala.Jugadores.All(j => j.ListoParaJugar == true);
                    if (jugadoresListos)
                    {
                        int indiceSala = SalasCreadas.IndexOf(sala);
                        SalasCreadas[indiceSala].Juego = new Juego();
                        NotificarCambioEnSalaAJugadores(SalasCreadas[indiceSala]);

                        foreach (Jugador jugador in sala.Jugadores)
                        {
                            jugador.Sesion.CanalDeCallback.JuegoIniciado();
                        }

                        resultadoDeValidacion = true;
                    }
                }
            }


            return resultadoDeValidacion;
        }


        private List<Sala> ClonarListaDeSalas(List<Sala> salas)
        {
            List<Sala> listaClonada = new List<Sala>();
            foreach (Sala sala in salas)
            {
                listaClonada.Add(ClonarSala(sala));
            }
            return listaClonada;
        }

        private Sala ClonarSala(Sala sala)
        {
            Sala salaClonada = new Sala
            {
                ID = sala.ID,
                Estado = sala.Estado,
                Juego = sala.Juego,
                NivelMaximo = sala.NivelMaximo,
                NivelMinimo = sala.NivelMinimo,
                Nombre = sala.Nombre,
                NombreDeUsuarioCreador = sala.NombreDeUsuarioCreador,
                Jugadores = new List<Jugador>()
            };

            foreach (Jugador jugador in sala.Jugadores)
            {
                Jugador jugadorDeSala = ClonarJugador(jugador);
                salaClonada.Jugadores.Add(jugadorDeSala);
            }

            return salaClonada;
        }

        /// <summary>
        /// Regresa un objeto con los atributos y clases iguales al <paramref name="jugador"/> menos el <see cref="Sesion.CanalDeCallback"/>
        /// ya que este es un objeto que no se debe clonar.
        /// </summary>
        /// <param name="jugador"></param>
        /// <returns></returns>
        private Jugador ClonarJugador(Jugador jugador)
        {
            Jugador jugadorDeSala = new Jugador
            {
                Color = jugador.Color,
                ListoParaJugar = jugador.ListoParaJugar,
                Sesion = new Sesion
                {
                    ID = jugador.Sesion.ID,
                    Creacion = jugador.Sesion.Creacion,
                    UltimaActualizacion = jugador.Sesion.UltimaActualizacion,
                    Usuario = new Usuario
                    {
                        Id = jugador.Sesion.Usuario.Id,
                        NombreDeUsuario = jugador.Sesion.Usuario.NombreDeUsuario,
                        Puntuacion = new Puntuacion
                        {
                            ExperienciaTotal = jugador.Sesion.Usuario.Puntuacion.ExperienciaTotal,
                            PartidasJugadas = jugador.Sesion.Usuario.Puntuacion.PartidasJugadas,
                            Victorias = jugador.Sesion.Usuario.Puntuacion.Victorias
                        }
                    }
                }
            };

            return jugadorDeSala;
        }

        /// <summary>
        /// Notifica por el canal de callback a las sesiones de la sala por <paramref name="indiceSala"/> con la nueva sala
        /// y unicamente la ID de su Sesion ya que la ID no debe mostrarse por motivos de seguridad
        /// </summary>
        /// <param name="indiceSala"></param>
        /// <param name="inicioDeJuego"></param>
        private void NotificarCambioEnSalaAJugadores(Sala sala, bool inicioDeJuego = false)
        {
            foreach (Jugador jugador in sala.Jugadores)
            {
                Sala salaDelJugador = ClonarSala(sala);
                RemoverIDsDeOtrasSesionesAListaDeJugadores(salaDelJugador.Jugadores, jugador.Sesion.ID);

                jugador.Sesion.CanalDeCallback.ActualizarSala(salaDelJugador);
            }
        }

        private void RemoverIDsDeOtrasSesionesAListaDeJugadores(List<Jugador> jugadores, string idSesion)
        {
            foreach (Jugador jugador in jugadores)
            {
                if (jugador.Sesion.ID != idSesion)
                {
                    jugador.Sesion.ID = string.Empty;
                }
            }
        }

        public List<Usuario> SolicitarTopDePuntuacionesDeUsuarios(Sesion sesion)
        {
            List<Usuario> usuarios = new List<Usuario>();
            if (ValidarAutenticidadDeSesion(sesion))
            {
                usuarios = new Usuario().CargarUsuariosPorMejorPuntuacion();
            }

            return usuarios;

        }

        private bool ValidarDatosDeSala(Sala sala)
        {
            bool resultadoDeValidacion = false;
            if (!string.IsNullOrEmpty(sala.Nombre)
                && sala.NivelMinimo >= 0
                && sala.NivelMaximo >= 0
                && sala.NivelMaximo >= sala.NivelMinimo)
            {
                resultadoDeValidacion = true;
            }
            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida si una sala existe en <see cref="SalasCreadas"/> por <see cref="Sala.ID"/> y <see cref="Sala.Nombre"/>
        /// </summary>
        /// <param name="sala"></param>
        /// <returns></returns>
        private bool ValidarExistenciaDeSala(Sala sala)
        {
            bool resultadoDeValidacion = false;
            bool idExiste = SalasCreadas.Exists(s => s.ID == sala.ID);
            bool nombreExiste = SalasCreadas.Exists(s => s.Nombre == sala.Nombre);
            if (idExiste && nombreExiste)
            {
                resultadoDeValidacion = true;
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
    }
}
