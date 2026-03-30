using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Reglas;

namespace Web.Pages.Productos
{
    [Authorize(Roles = "1")]
    public class IndexModel : PageModel
    {
        private IConfiguracion _configuracion;
        public IList<ProductoResponse> productos { get; set; } = default!;
        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGet()
        {

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductos");
            using var cliente = ObtenerClienteConToken();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                productos = JsonSerializer.Deserialize<List<ProductoResponse>>(resultado, opciones);
            }
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
