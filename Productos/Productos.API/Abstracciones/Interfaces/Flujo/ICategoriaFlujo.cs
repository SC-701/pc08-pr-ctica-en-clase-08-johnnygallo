using Abstracciones.Modelo;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ICategoriaFlujo
    {
        Task<IEnumerable<Categoria>> ObtenerCategorias();
    }
}
