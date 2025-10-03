using RestWebApi.Model;
using RestWebApi.Repository;

namespace RestWebApi.Service
{
    public class WeatherForecastService(
        IWeatherForecastQueryRepository queryRepo,
        IWeatherForecastCommandRepository commandRepo) : IWeatherForecastService
    {
        private readonly IWeatherForecastQueryRepository _queryRepo = queryRepo;
        private readonly IWeatherForecastCommandRepository _commandRepo = commandRepo;

        public async Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync()
        {
            return await _queryRepo.GetAllAsync();
        }

        public async Task<WeatherForecast?> GetForecastByIdAsync(int id)
        {
            return await _queryRepo.GetByIdAsync(id);
        }

        public async Task<WeatherForecast> UpdateForecastAsync(WeatherForecast forecast)
        {
            ArgumentNullException.ThrowIfNull(forecast);
            if (forecast.Id <= 0)
            {
                throw new ArgumentException("Invalid forecast ID.");
            }
            // Assuming you have a command repository for updates
            if (_queryRepo is Repository.IWeatherForecastCommandRepository)
            {
                return await _commandRepo.UpdateAsync(forecast);
            }
            else
            {
                throw new InvalidOperationException("The repository does not support update operations.");
            }
        }

        public async Task<WeatherForecast> AddForecastAsync(WeatherForecast forecast)
        {
            // Assuming you have a command repository for updates
            if (_queryRepo is IWeatherForecastCommandRepository)
            {
                return await _commandRepo.AddAsync(forecast);
            }
            else
            {
                throw new InvalidOperationException("The repository does not support update operations.");
            }
        }
    }
}