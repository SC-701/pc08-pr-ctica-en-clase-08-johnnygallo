using Abstracciones.Modelo.Servicios.CambioDolar;

namespace Abstracciones.Interfaces.Servicios
{
    public interface ITipoDeCambioServicio
    {
        Task<TipoDeCambio> Obtener(string fecha);
    }
}
