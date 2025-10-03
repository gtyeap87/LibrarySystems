using Microsoft.EntityFrameworkCore;
using RestWebApi.Model;

namespace RestWebApi.Data;

public class WeatherForecastContext(DbContextOptions<WeatherForecastContext> options) : DbContext(options)
{
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Summary).HasMaxLength(100);
            entity.Property(e => e.Humidity).HasDefaultValue(0);
            entity.Property(e => e.WindSpeed).HasDefaultValue(0);
            entity.Property(e => e.RowVersion).IsRowVersion();
        });
    }
}