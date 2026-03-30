using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelo;

namespace Flujo
{
    public class SubCategoriaFlujo : ISubCategoriaFlujo
    {
        private readonly ISubCategoriaDA _subCategoriaDA;

        public SubCategoriaFlujo(ISubCategoriaDA subCategoriaDA)
        {
            _subCategoriaDA = subCategoriaDA;
        }

        public async Task<IEnumerable<SubCategoria>> ObtenerSubCategoriasPorCategoria(Guid categoriaId)
        {
            return await _subCategoriaDA.ObtenerSubCategoriasPorCategoria(categoriaId);
        }
    }
}