using Abstracciones.Interfaces.DA;
using Abstracciones.Modelo;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DA
{
    public class ProductoDA : IProductoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ProductoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObetenerRepositorio();
        }
        public async Task<Guid> Agregar(ProductoRequest producto)
        {
            string query = @"AgregarProducto";
            var reultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Guid.NewGuid(),
                IdSubCategoria = producto.IdSubCategoria,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CodigoBarras = producto.CodigoBarras
            });
            return reultadoConsulta;
        }

        public async Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            await verificarProductoExiste(Id);
            string query = @"EditarProducto";
            var reultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Id,
                IdSubCategoria = producto.IdSubCategoria,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CodigoBarras = producto.CodigoBarras
            });
            return reultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            await verificarProductoExiste(Id);
            string query = @"EliminarProducto";
            var reultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Id
            });
            return reultadoConsulta;
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
            string query = @"ObtenerProductos";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductoResponse>(query);
            return resultadoConsulta;
        }

        public async Task<ProductoDetalle> Obtener(Guid Id)
        {
            string query = @"ObtenerProducto";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductoDetalle>(query, new
            {
                Id = Id
            });
            return resultadoConsulta.FirstOrDefault();
        }

        private async Task verificarProductoExiste(Guid Id)
        {
            ProductoDetalle? resultadoConsultaProducto = await Obtener(Id);
            if (resultadoConsultaProducto == null)
                throw new Exception("No se encontro el producto");
        }
    }
}
