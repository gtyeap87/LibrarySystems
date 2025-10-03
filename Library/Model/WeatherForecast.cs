using System.ComponentModel.DataAnnotations;

namespace RestWebApi.Model;

/// <summary>
/// Represents weather forecast data with support for both v1 and v2 API features
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Unique identifier for the forecast
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The date of the forecast
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Temperature in Celsius
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Temperature in Fahrenheit (calculated)
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Weather condition summary
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Relative humidity percentage (v2 feature)
    /// Valid range: 0-100
    /// </summary>
    [Range(0, 100)]
    public int Humidity { get; set; }

    /// <summary>
    /// Wind speed in meters per second (v2 feature)
    /// </summary>
    [Range(0, double.MaxValue)]
    public double WindSpeed { get; set; }

    /// <summary>
    /// Concurrency token for optimistic concurrency control
    /// </summary>
    [Timestamp]
    public byte[]? RowVersion { get; set; }
}