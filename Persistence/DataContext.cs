using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext : DbContext
{
    public string DbPath { get; }

    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

    public DataContext()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Join(folder, "weather.db"); // local sqlite file
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    public DbSet<Product> Products { get; set; }

}
