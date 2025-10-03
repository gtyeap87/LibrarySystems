using Microsoft.EntityFrameworkCore;
using RestWebApi.Data;
using RestWebApi.Model;

namespace RestWebApi.Repository;

/// <summary>
/// Repository for managing weather forecast data with logging capabilities
/// </summary>
public class WeatherForecastRepository(
    WeatherForecastContext context,
    ILogger<WeatherForecastRepository> logger) : IWeatherForecastQueryRepository, IWeatherForecastCommandRepository
{
    private readonly WeatherForecastContext _context = context;
    private readonly ILogger<WeatherForecastRepository> _logger = logger;

    /// <inheritdoc/>
    public async Task<IEnumerable<WeatherForecast>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all weather forecasts");

        var forecasts = await _context.WeatherForecasts.ToListAsync();
        _logger.LogInformation("Retrieved {Count} weather forecasts", forecasts.Count);
        return forecasts;
    }

    /// <inheritdoc/>
    public async Task<WeatherForecast?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving weather forecast with ID: {Id}", id);

        var forecast = await _context.WeatherForecasts.FindAsync(id);
        if (forecast == null)
        {
            _logger.LogWarning("Weather forecast with ID: {Id} not found", id);
        }
        return forecast;
    }

    /// <inheritdoc/>
    public async Task<WeatherForecast> UpdateAsync(WeatherForecast forecast)
    {
        _logger.LogInformation("Updating weather forecast with ID: {Id}", forecast.Id);

        var existingForecast = await _context.WeatherForecasts
            .FirstOrDefaultAsync(f => f.Id == forecast.Id);

        if (existingForecast == null)
        {
            _logger.LogWarning("Weather forecast with ID: {Id} not found for update", forecast.Id);
            throw new KeyNotFoundException($"Weather forecast with ID {forecast.Id} not found.");
        }

        _context.Entry(existingForecast).CurrentValues.SetValues(forecast);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully updated weather forecast with ID: {Id}", forecast.Id);
        return existingForecast;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="forecast"></param>
    /// <returns></returns>
    public async Task<WeatherForecast> AddAsync(WeatherForecast forecast)
    {
        _logger.LogInformation("Adding new weather forecast for date: {Date}", forecast.Date);

        _context.WeatherForecasts.Add(forecast);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully added new weather forecast with ID: {Id}", forecast.Id);
        return forecast;
    }
}