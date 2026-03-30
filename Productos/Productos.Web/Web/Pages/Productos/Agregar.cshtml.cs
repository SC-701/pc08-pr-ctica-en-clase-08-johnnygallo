using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Reglas;

namespace Web.Pages.Productos
{
    [Authorize(Roles = "2")]
    public class AgregarModel : PageModel
    {
        private IConfiguracion _configuracion;

        [BindProperty]
        public ProductoRequest producto { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> categoria { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> subCategoria { get; set; } = default!;

        public Guid categoriaSeleccionada { get; set; } = default!;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarProducto");
            var cliente = new HttpClient();

            var respuesta = await cliente.PostAsJsonAsync(endpoint, producto);
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("./Index");
        }

        public async Task<ActionResult> OnGet()
        {
            await ObtenerCategoriaAsync();
            return Page();
        }

        private async Task ObtenerCategoriaAsync()
        {
            using var cliente = ObtenerClienteConToken();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");
            
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var resultadoDeserializado = JsonSerializer.Deserialize<List<Categoria>>(resultado, opciones);
                categoria = resultadoDeserializado.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.Id.ToString(),
                                      Text = a.Nombre.ToString()
                                  }).ToList();
            }
        }

        public async Task<JsonResult> OnGetObtenerSubCategorias(Guid categoriaId)
        {
            var subCategorias = await ObtenerSubCategoriasAsync(categoriaId);
            return new JsonResult(subCategorias);
        }

        private async Task<List<SubCategoria>> ObtenerSubCategoriasAsync(Guid categoriaId)
        {
            using var cliente = ObtenerClienteConToken();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerSubCategorias");
            
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, categoriaId));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<SubCategoria>>(resultado, opciones);
            }
            return new List<SubCategoria>();
        }


        private HttpClient ObtenerClienteConToken()
        {
            var tokenClaim = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Token");
            var cliente = new HttpClient();
            if (tokenClaim != null)
                cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer", tokenClaim.Value);
            return cliente;
        }
    }
}
