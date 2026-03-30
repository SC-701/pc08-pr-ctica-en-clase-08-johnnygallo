using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelo;
using Abstracciones.Interfaces.Reglas;

namespace Flujo
{
    public class ProductoFlujo : IProductoFlujo
    {
        private readonly IProductoDA _productoDA;
        private readonly ITipoDeCambioRegla _tipoDeCambioRegla;


        public ProductoFlujo(IProductoDA productoDA, ITipoDeCambioRegla tipoDeCambio)
        {
            _productoDA = productoDA;
            _tipoDeCambioRegla = tipoDeCambio;
        }


        public Task<Guid> Agregar(ProductoRequest producto)
        {
            return _productoDA.Agregar(producto);
        }

        public Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            return _productoDA.Editar(Id, producto);
        }

        public Task<Guid> Eliminar(Guid Id)
        {
            return _productoDA.Eliminar(Id);
        }

        public Task<IEnumerable<ProductoResponse>> Obtener()
        {
            return _productoDA.Obtener();
        }

        public async Task<ProductoDetalle> Obtener(Guid Id)
        {
            var producto = await _productoDA.Obtener(Id);
            var fechaHoy = DateTime.Now.ToString("yyyy/MM/dd");

            if (producto == null)
                return null;

            try
            {
                // Aplicar regla de tipo de cambio
                var tipoCambio = await _tipoDeCambioRegla.ValorColones(fechaHoy);
                producto.PrecioEnDolar = producto.Precio / tipoCambio;
                producto.PrecioDolar = tipoCambio;
            }
            catch
            {
                producto.PrecioEnDolar = 0; // fallback si falla la regla
            }

            return producto;
        }
    }
}
