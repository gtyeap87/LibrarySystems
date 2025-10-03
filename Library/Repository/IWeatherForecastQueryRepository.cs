using RestWebApi.Model;

namespace RestWebApi.Repository
{
    /// <summary>
    ///
    /// </summary>
    public interface IWeatherForecastQueryRepository
    {
        Task<IEnumerable<WeatherForecast>> GetAllAsync();

        Task<WeatherForecast?> GetByIdAsync(int id);
    }
}