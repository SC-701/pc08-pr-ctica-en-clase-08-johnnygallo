using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;

namespace Reglas
{
    public class TipoDeCambioRegla : ITipoDeCambioRegla
    {
        private readonly ITipoDeCambioServicio _tipoDeCambioServicio;
        private readonly IConfiguracion _configuracion;

        public TipoDeCambioRegla(ITipoDeCambioServicio tipoCambioServicio, IConfiguracion configuracion)
        {
            _tipoDeCambioServicio = tipoCambioServicio;
            _configuracion = configuracion;
        }
        public async Task<decimal> ValorColones(string fecha)
        {
            var tipoCambio = await _tipoDeCambioServicio.Obtener(fecha);

            if (tipoCambio?.Datos == null || !tipoCambio.Datos.Any())
                throw new Exception("No se pudo obtener el tipo de cambio del dólar.");

            var indicador = tipoCambio.Datos[0].Indicadores.FirstOrDefault();

            if (indicador?.Series == null || !indicador.Series.Any())
                throw new Exception("No hay datos disponibles para el tipo de cambio.");

            var valor = indicador.Series.First().valorDatoPorPeriodo;

            return valor;
        }
    }
}
