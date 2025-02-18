using Microsoft.AspNetCore.Mvc;

namespace firstProject.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private static WeatherForecast[] arr;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        arr = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }


    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return arr;
    }


    [HttpGet("{id}")]
    public ActionResult<WeatherForecast> Get(int id)
    {
        if (id > arr.Length)
            return NotFound();
        return arr[id];
    }
}
