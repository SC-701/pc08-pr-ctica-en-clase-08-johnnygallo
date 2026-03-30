using Abstracciones.Modelo;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ISubCategoriaFlujo
    {
        Task<IEnumerable<SubCategoria>> ObtenerSubCategoriasPorCategoria(Guid categoriaId);
    }
}
