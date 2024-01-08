using System.Text.Json;
using Cocona;
using CoconaConsoleApplication;
using CoconaConsoleApplication.Weather;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// // Run Name --lastname LastName
// CoconaApp.Run(([Argument] string name, [Option] string? lastname) =>
// {
//     Console.WriteLine($"Hello, {name} {lastname}!");    
// });

var builder = CoconaApp.CreateBuilder();


builder.Services.AddLogging(logging =>
{
    logging.AddFilter("System.Net.Http", LogLevel.Error); 
});
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IWeatherService, OpenWeatherMapService>();

var app = builder.Build();

// app.AddCommand("weather", async (IWeatherService weatherService) =>
// {
//     var weather = await weatherService.GetWeatherForCityAsync("London");
//     Console.WriteLine(JsonSerializer.Serialize(weather, new JsonSerializerOptions{WriteIndented = true}));
// });

app.AddCommands<WeatherCommands>();

app.Run();
