using LogicaDeNegocios;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using ServiciosDeComunicacion.Interfaces.InterfacesDeServiciosDeFlipllo;

namespace ServiciosDeComunicacion.Servicios
{
    public partial class ServiciosDeFlipllo
    {
        public List<Sala> SalasCreadas;
        private readonly int NUMERO_MAXIMO_DE_JUGADORES_EN_SALA = 2;

        public void CrearSala(Sala sala, Sesion sesion)
        {
            if (ValidarAutenticidadDeSesion(sesion) 
                && !ValidarExistenciaDeSesionEnSalasCreadas(sesion)
                &&  ValidarDatosDeSala(sala) 
                && !ValidarExistenciaDeSala(sala))
            {
                Sesion sesionCargada = SesionesConectadas.Find(s => s.ID == sesion.ID);
                Jugador jugador = new Jugador
                {
                    Color = ColorDeFicha.Ninguno,
                    Sesion = sesionCargada,
                    ListoParaJugar = false
                };
                sala.IDSesionCreadora = sesion.ID;
                sala.Jugadores = new List<Jugador>();
                sala.Jugadores.Add(jugador);
                sala.ID = Guid.NewGuid().ToString();
                sala.Estado = EstadoSala.Registrada;
                
                bool sesionEnviadaCorrectamente = false;
                sesion.CanalDeCallback = OperationContext.Current.GetCallbackChannel<IServiciosDeCallBack>();
                try
                {
                    sesion.CanalDeCallback.RecibirSala(sala);
                    sesionEnviadaCorrectamente = true;
                }
                catch (CommunicationException e)
                {
                    NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
                    logger.Warn(e);
                }

                if (sesionEnviadaCorrectamente)
                {
                    SalasCreadas.Add(sala);
                    ControladorServiciosDeFlipllo.ListaDeSalasActualizado(SalasCreadas);
                }
            }
        }

        public void BorrarSala(Sesion sesion)
        {
            Sala sala = BuscarSalaDeSesion(sesion);

            if (sala != null)
            {
                if (ValidarExistenciaDeSala(sala)
                    && sala.IDSesionCreadora == sesion.ID)
                {
                    SalasCreadas.Remove(sala);
                }
            }
            
        }

        public void DesconectarDeSala(Sesion sesion)
        {
            throw new NotImplementedException();
            //SI ES EL DUEÑO BORRAR
        }

        public void CambiarConfiguracionDeLaSala(Sesion sesion, Sala sala)
        {
            throw new NotImplementedException();
        }

        public void IniciarJuego(Sesion sesion)
        {
            //Si los dos estan listos
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
            //Regresar todo menos ids mas que la de sesion
            List<Sala> salas = new List<Sala>(SalasCreadas);

            if (ValidarAutenticidadDeSesion(sesion))
            {
                foreach (Sala sala in salas)
                {
                    foreach (Jugador jugador in sala.Jugadores)
                    {
                        if (jugador.Sesion.ID != sesion.ID)
                        {
                            jugador.Sesion.ID = string.Empty;
                        }
                    }
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
                if (salaLocal.Jugadores.Count < NUMERO_MAXIMO_DE_JUGADORES_EN_SALA)
                {
                    if (sesion.Usuario.Puntuacion.Nivel >= salaLocal.NivelMinimo
                    && sesion.Usuario.Puntuacion.Nivel <= salaLocal.NivelMaximo
                    )
                    {
                        Jugador jugador = new Jugador
                        {
                            Color = ColorDeFicha.Ninguno,
                            Sesion = sesion,
                            ListoParaJugar = false
                        };

                        int indiceSalaLocal = SalasCreadas.IndexOf(salaLocal);
                        SalasCreadas[indiceSalaLocal].Jugadores.Add(jugador);
                        ControladorServiciosDeFlipllo.ListaDeSalasActualizado(SalasCreadas);
                    }
                }
                else
                {
                    int indiceSala = SalasCreadas.IndexOf(salaLocal);
                    SalasCreadas[indiceSala].Estado = EstadoSala.CupoLleno;
                    ControladorServiciosDeFlipllo.ListaDeSalasActualizado(SalasCreadas);
                }
                
            }
        }

        public void CambiarColorDeJugadorEnSala(Sesion sesion, ColorDeFicha color)
        {
            throw new NotImplementedException();
        }

        public void AlternarListoParaJugar(Sesion sesion)
        {
            Sala sala = BuscarSalaDeSesion(sesion);
            if (sala != null)
            {

            }
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
