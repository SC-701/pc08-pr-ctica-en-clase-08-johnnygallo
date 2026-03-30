using Abstracciones.Modelo;

namespace Abstracciones.Interfaces.DA
{
    public interface ISubCategoriaDA
    {
        Task<IEnumerable<SubCategoria>> ObtenerSubCategoriasPorCategoria(Guid categoriaId);
    }
}
