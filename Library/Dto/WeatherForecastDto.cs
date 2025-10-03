namespace RestWebApi.Dto;

/// <summary>
/// Represents weather forecast data with support for both v1 and v2 API features
/// </summary>
public class WeatherForecastDto
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
}