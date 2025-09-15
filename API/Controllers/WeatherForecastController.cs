using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly DataContext _context;

    public WeatherForecastController(DataContext context)
    {
        _context = context;
    }

    // GET /weatherforecast
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
        => _context.WeatherForecasts
                   .OrderBy(w => w.Id)
                   .ToList();

    // POST /weatherforecast
    // (hard-coded add for now; we’ll test with Postman later)
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var wf = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = 25,
            Summary = "EF saved!"
        };
        _context.WeatherForecasts.Add(wf);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = wf.Id }, wf);
    }
}
