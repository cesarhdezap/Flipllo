using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDeNegocios.ClasesDeDominio
{
    public class Puntuacion
    {

        public int PartidasJugadas { get; set; }
        public int Victorias { get; set; }
        public double ExperienciaTotal { get; set; }
        public double Nivel { get { return CalcularNivel(); } }

        private double CalcularNivel()
        {
            return ExperienciaTotal / 1000;
        }

    }
}
