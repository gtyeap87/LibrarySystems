using RestWebApi.Model;

namespace RestWebApi.Repository;

/// <summary>
///
/// </summary>
public interface IWeatherForecastCommandRepository
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="forecast"></param>
    /// <returns></returns>
    Task<WeatherForecast> AddAsync(WeatherForecast forecast);

    /// <summary>
    ///
    /// </summary>
    /// <param name="forecast"></param>
    /// <returns></returns>
    Task<WeatherForecast> UpdateAsync(WeatherForecast forecast);
}