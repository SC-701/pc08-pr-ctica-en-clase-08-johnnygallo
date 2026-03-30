using Abstracciones.Interfaces.DA;
using Abstracciones.Modelo;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DA
{
    public class CategoriaDA : ICategoriaDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public CategoriaDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObetenerRepositorio();
        }

        public async Task<IEnumerable<Categoria>> ObtenerCategorias()
        {
            string query = @"ObtenerCategorias";
            var resultadoConsulta = await _sqlConnection.QueryAsync<Categoria>(query);
            return resultadoConsulta;
        }
    }
}