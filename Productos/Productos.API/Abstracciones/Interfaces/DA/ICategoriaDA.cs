using Abstracciones.Modelo;

namespace Abstracciones.Interfaces.DA
{
    public interface ICategoriaDA
    {
        Task<IEnumerable<Categoria>> ObtenerCategorias();
    }
}
