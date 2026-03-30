namespace Abstracciones.Interfaces.Reglas
{
    public interface ITipoDeCambioRegla
    {
        Task<decimal> ValorColones(string fecha);
    }
}
