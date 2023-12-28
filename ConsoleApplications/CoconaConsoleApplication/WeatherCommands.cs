using System.Text.Json;
using Cocona;
using CoconaConsoleApplication.Weather;

namespace CoconaConsoleApplication;

public class WeatherCommands
{
    private readonly IWeatherService _weatherService;

    public WeatherCommands(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }
    
    // If you dont add the attribute and only have one command any run command will be accepted
    // If you have more than one command without the attribute then the name of the method will be used
    [Command("weather")]
    public async Task Weather()
    {
        var weather = await _weatherService.GetWeatherForCityAsync("London");
        Console.WriteLine(JsonSerializer.Serialize(weather, new JsonSerializerOptions{WriteIndented = true}));
    }
    
    [Ignore]
    public async Task Weather2()
    {
        var weather = await _weatherService.GetWeatherForCityAsync("London");
        Console.WriteLine(JsonSerializer.Serialize(weather, new JsonSerializerOptions{WriteIndented = true}));
    }
}