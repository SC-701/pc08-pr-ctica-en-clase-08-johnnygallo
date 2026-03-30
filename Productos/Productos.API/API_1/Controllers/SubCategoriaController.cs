using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriaController : ControllerBase, ISubCategoriaController
    {
        private readonly ISubCategoriaFlujo _subCategoriaFlujo;
        private readonly ILogger<SubCategoriaController> _logger;

        public SubCategoriaController(ISubCategoriaFlujo subCategoriaFlujo, ILogger<SubCategoriaController> logger)
        {
            _subCategoriaFlujo = subCategoriaFlujo;
            _logger = logger;
        }

        [HttpGet("{categoriaId}")]
        public async Task<IActionResult> ObtenerSubCategoriasPorCategoria(Guid categoriaId)
        {
            var resultado = await _subCategoriaFlujo.ObtenerSubCategoriasPorCategoria(categoriaId);

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }
    }
}