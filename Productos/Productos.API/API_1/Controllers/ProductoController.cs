using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase, IProductoController

    {
        private IProductoFlujo _productoFlujo;

        private ILogger<ProductoController> _logger;

        public ProductoController(IProductoFlujo productoFlujo, ILogger<ProductoController> logger)
        {
            _productoFlujo = productoFlujo;
            _logger = logger;
        }

        #region Constructores
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] ProductoRequest producto)
        {
            var resultado = _productoFlujo.Agregar(producto);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado } );
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Editar([FromRoute] Guid Id, [FromBody] ProductoRequest producto)
        {
            if (!await VerificarProductoExiste(Id))
                return NotFound("El Producto ID no existe");

            var resultado = await _productoFlujo.Editar(Id, producto);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Eliminar([FromRoute]Guid Id)
        {
            if (!await VerificarProductoExiste(Id))
                return NotFound("El Producto ID no existe");

            var resultado = await _productoFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _productoFlujo.Obtener();
            if (!resultado.Any()) 
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Obtener([FromRoute]Guid Id)
        {
            if (!await VerificarProductoExiste(Id))
                return NotFound("El Producto ID no existe");

            var resultado = await _productoFlujo.Obtener(Id);
            return Ok(resultado);
        }
        #endregion

        #region Helpers
        private async Task<bool> VerificarProductoExiste(Guid Id)
        {
            var resultadoValidacion = false;
            var resultadoProductoExiste = await _productoFlujo.Obtener(Id);
            if (resultadoProductoExiste != null)
                resultadoValidacion = true;
            return resultadoValidacion;
        }
        #endregion
    }
}
