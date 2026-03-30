using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelo.Servicios.CambioDolar;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Numerics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicios
{
    public class TipoDeCambioServicio : ITipoDeCambioServicio
    {
        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClient;

        public TipoDeCambioServicio(IConfiguracion configuracion, IHttpClientFactory httpClient)
        {
            _configuracion = configuracion;
            _httpClient = httpClient;
        }

        public async Task<TipoDeCambio> Obtener(string fecha)
        {
            // Obtener endpoint dinámicamente desde configuración
            var endPoint = _configuracion.ObtenerMetodo("ApiEndPointsTipoCambio", "ObtenerTipoDeCambio");

            var servicioTipoDeCambio = _httpClient.CreateClient("ServicioTipoDeCambio");

            var token = _configuracion.ObtenerToken("ApiEndPointsTipoCambio");
            servicioTipoDeCambio.DefaultRequestHeaders.Clear();
            servicioTipoDeCambio.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var respuesta = await servicioTipoDeCambio.GetAsync(string.Format(endPoint, fecha));

            respuesta.EnsureSuccessStatusCode();
            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultadoDesrelizado = JsonSerializer.Deserialize<TipoDeCambio>(resultado, opciones);

            return resultadoDesrelizado;
        }
    }
}