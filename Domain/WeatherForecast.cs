namespace Domain;

public class WeatherForecast
{
    public int Id { get; set; }                 // PK for EF
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}
