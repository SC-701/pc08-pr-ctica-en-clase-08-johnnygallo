using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelo.Servicios.CambioDolar
{
    public class TipoDeCambio
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
        public List<Dato> Datos { get; set; }
    }
}
