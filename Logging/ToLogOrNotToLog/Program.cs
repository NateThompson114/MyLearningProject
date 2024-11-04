using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging.ApplicationInsights;
using ToLogOrNotToLog;
using ToLogOrNotToLog.Examples;
using ToLogOrNotToLog.Extensions;
using ToLogOrNotToLog.Models.Enum;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// This allows you to choose your providers, your customizations to your logging based on anything, in this case the environment.
builder.Logging.ClearProviders();

if(builder.Environment.IsDevelopment())
{
    // builder.Logging.AddConsole();
    // builder.Logging.AddProvider(new NatesLoggerProvider());
    builder.Logging.AddJsonConsole(jBuilder =>
    {
        jBuilder.IncludeScopes = true;
        jBuilder.JsonWriterOptions = new JsonWriterOptions
        {
            Indented = true
        };
        jBuilder.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff zzz";
    });
    
}
else
{
    // Application Insights is a asynchronous logger, so it will not block the main thread, however that also means it takes time for logs to show up in application insights.
    builder.Logging.AddApplicationInsights(
        configureTelemetryConfiguration: teleConfig =>
            teleConfig.ConnectionString = "InstrumentationKey=331b798e-ba02-4c30-82c6-6bb30fe33afe;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/;ApplicationId=e1360525-7aef-4c2e-8df6-6f5ed506c2b0",
        configureApplicationInsightsLoggerOptions: _ => { });
}


// builder.Logging.AddFilter<ApplicationInsightsLogger>(filter => { });

// builder.Logging
//     .FilterExample();

// builder.Logging.ClearProviders(); // <-- This all works the same even in a web application, with the same customizations. This would clear and then you could add only what you want.

// using var loggerFactory = LoggerFactory.Create(builder =>
// {
//     builder.AddConsole();
//     // builder.AddJsonConsole();
// });
// builder.Services.AddSingleton(loggerFactory);

var app = builder.Build();

#region Logging Information

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

#endregion

ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();

using (logger.BeginTimedOperation("Handling new Payment"))
{
    logger
        .LogMessageScopeExample();
}
// logger
    // .StructuredLoggingExample()
    // .EventIdExample()
    // .ExceptionExample()
    // .LogMessageTemplateFormattingExample()
    // .LogMessageWithComplexObjectExample()
    // .LogMessageScopeExample();

// Logger Provider from the DI from Microsoft Web Applications support hot reloading, if you change the default level in appsettings.json, it will change the level of the logger and allow you to see the changes in real-time.
logger.LogWarning("Starting a example of what the minimum level does and how to hot reload it.");

while (true)
{
    logger.LogInformation("This is repeated every second.");
    await Task.Delay(1000);
}

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