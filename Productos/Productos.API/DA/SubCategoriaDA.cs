using Abstracciones.Interfaces.DA;
using Abstracciones.Modelo;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DA
{
    public class SubCategoriaDA : ISubCategoriaDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public SubCategoriaDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObetenerRepositorio();
        }

        public async Task<IEnumerable<SubCategoria>> ObtenerSubCategoriasPorCategoria(Guid categoriaId)
        {
            string query = @"ObtenerSubCategoriasPorCategoria";
            var resultadoConsulta = await _sqlConnection.QueryAsync<SubCategoria>(query, new
            {
                CategoriaId = categoriaId
            });
            return resultadoConsulta;
        }
    }
}