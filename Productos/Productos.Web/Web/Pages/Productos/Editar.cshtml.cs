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
    public class EditarModel : PageModel
    {
        private IConfiguracion _configuracion;
        [BindProperty]
        public ProductoResponse productoResponse { get; set; } = default!;
        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = default!;
        [BindProperty]
        public List<SelectListItem> subCategorias { get; set; } = default!;
        [BindProperty]
        public Guid categoriaSeleccionada { get; set; } = default!;
        [BindProperty]
        public Guid subCategoriaSeleccionado { get; set; } = default!;
        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task<ActionResult> OnGet(Guid? id)
        {
            if (id == null)
                return NotFound();

            using var cliente = ObtenerClienteConToken();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");
            

            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                await ObtenerCategoriaAsync();
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                productoResponse = JsonSerializer.Deserialize<ProductoResponse>(resultado, opciones);
                if (productoResponse != null)
                {
                    categoriaSeleccionada = Guid.Parse(categorias.Where(m => m.Text == productoResponse.Categoria).FirstOrDefault().Value);
                    subCategorias = (await ObtenerSubCategoriasAsync(categoriaSeleccionada)).Select(a =>
                        new SelectListItem
                        {
                            Value = a.Id.ToString(),
                            Text = a.Nombre.ToString(),
                            Selected = a.Nombre == productoResponse.SubCategoria
                        }).ToList();
                    subCategoriaSeleccionado = Guid.Parse(subCategorias.Where(m => m.Text == productoResponse.SubCategoria).FirstOrDefault().Value);
                }

            }
            return Page();
        }
        public async Task<ActionResult> OnPost()
        {
            if (productoResponse.Id == Guid.Empty)
                return NotFound();


            if (!ModelState.IsValid)
                return Page();

            using var cliente = ObtenerClienteConToken();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProducto");
            

            var respuesta = await cliente.PutAsJsonAsync<ProductoRequest>(string.Format(endpoint, productoResponse.Id), 
                new ProductoRequest { 
                    IdSubCategoria = subCategoriaSeleccionado, 
                    Nombre = productoResponse.Nombre, 
                    Descripcion = productoResponse.Descripcion, 
                    Precio = productoResponse.Precio, 
                    Stock = productoResponse.Stock,
                    CodigoBarras = productoResponse.CodigoBarras
                });
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("./Index");
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
                categorias = resultadoDeserializado.Select(a =>
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
