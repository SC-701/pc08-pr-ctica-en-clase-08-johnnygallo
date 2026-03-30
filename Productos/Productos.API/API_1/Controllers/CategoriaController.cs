using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase, ICategoriaController
    {
        private readonly ICategoriaFlujo _categoriaFlujo;
        private readonly ILogger<CategoriaController> _logger;

        public CategoriaController(ICategoriaFlujo categoriaFlujo, ILogger<CategoriaController> logger)
        {
            _categoriaFlujo = categoriaFlujo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCategorias()
        {
            var resultado = await _categoriaFlujo.ObtenerCategorias();

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }
    }
}