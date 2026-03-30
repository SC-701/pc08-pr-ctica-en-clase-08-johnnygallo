using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Web.Pages.Productos
{
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
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");
            var cliente = new HttpClient();

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

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProducto");
            var cliente = new HttpClient();

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
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");
            var cliente = new HttpClient();
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
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerSubCategorias");
            var cliente = new HttpClient();
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
    }
}
