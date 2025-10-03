using RestWebApi.Model;

namespace RestWebApi.Service
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast> AddForecastAsync(WeatherForecast forecast);

        Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync();

        Task<WeatherForecast?> GetForecastByIdAsync(int id);

        Task<WeatherForecast> UpdateForecastAsync(WeatherForecast forecast);
    }
}