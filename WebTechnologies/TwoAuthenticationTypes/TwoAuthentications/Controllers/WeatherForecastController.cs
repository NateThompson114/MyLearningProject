using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwoAuthentications.Extensions;

namespace TwoAuthentications.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return GetWeatherForecasts();
        }

        [HttpGet("GetWeatherForecastV1")]
        [Authorize]
        public IEnumerable<WeatherForecast> GetAvidAuth()
        {
            return GetWeatherForecasts();
        }
        
        [HttpGet("GetWeatherForecastV2")]
        [Authorize(AuthenticationSchemes = BuilderExtensions.CustomBearerOptions.AvidAuth)]
        public IEnumerable<WeatherForecast> GetAzureAppConfig()
        {
            return GetWeatherForecasts();
        }

        private static IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}
