using ToLogOrNotToLog.Examples;
using ToLogOrNotToLog.Models.Enum;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddLogging();

// builder.Logging.ClearProviders(); // <-- This all works the same even in a web application, with the same customizations. This would clear and then you could add only what you want.

// using var loggerFactory = LoggerFactory.Create(builder =>
// {
//     builder.AddConsole();
//     // builder.AddJsonConsole();
// });
// builder.Services.AddSingleton(loggerFactory);

var app = builder.Build();

// ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
// logger.LogInformation("Hello, world!");
// Returns
//  info: Program[0] --> Program is the log category and is set to the type of the Program class, so you set the category by using the type of the class
//  Hello, world!
//
// There are multiple log levels, such as Trace, Debug, Information, Warning, Error, Critical, and None. The default log level is Information.
// The log level can be set in the appsettings.json file or in the code.
//  trace is the most detailed log level, and none is the least detailed log level.
//  trace < debug < information < warning < error < critical < none
//  trace can also be used to log sensitive information, such as passwords, and should not be used in production.
//
// LogLevel logLevel = LogLevel.Information; // <-- Uncomment to look at the log level enum
// LoggerExtensions.LogTrace(logger, "This is a trace log message."); // <-- Uncomment to look at the log extension methods

ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
logger
    .StructuredLoggingExample()
    .EventIdExample()
    .ExceptionExample();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}