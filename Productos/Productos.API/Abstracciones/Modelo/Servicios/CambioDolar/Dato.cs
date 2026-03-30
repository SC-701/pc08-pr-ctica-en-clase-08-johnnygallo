using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelo.Servicios.CambioDolar
{
    public class Dato
    {
        public string Titulo { get; set; }
        public string Periodicidad { get; set; }
        public List<Indicador> Indicadores { get; set; }
    }
}
