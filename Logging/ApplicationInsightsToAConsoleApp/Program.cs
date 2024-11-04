// See https://aka.ms/new-console-template for more information

// Step 1 - Add Microsoft.ApplicationInsights instead of Microsoft.Extensions.Logging.ApplicationInsights
// Step 2 - Add Microsoft.Extensions.DependencyInjection - Its important to add the full package, not just the abstractions.
// Step 3 - Add Microsoft.Extensions.Options.ConfigurationExtensions
// Step 4 - Add Microsoft.Extensions.Logging
// Step 5 - Add Microsoft.Extensions.Logging.ApplicationInsights

// Console.WriteLine("Hello, World!");

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using var channel = new InMemoryChannel();

try
{
    IServiceCollection services = new ServiceCollection();
    services.Configure<TelemetryConfiguration>(x => x.TelemetryChannel = channel);
    services.AddLogging(builder =>
    {
        builder.AddApplicationInsights(
            configureTelemetryConfiguration: teleConfig =>
                teleConfig.ConnectionString = "InstrumentationKey=331b798e-ba02-4c30-82c6-6bb30fe33afe;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/;ApplicationId=e1360525-7aef-4c2e-8df6-6f5ed506c2b0",
            configureApplicationInsightsLoggerOptions: _ => { });
    });
    
    var serviceProvider = services.BuildServiceProvider();
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    
    logger.LogInformation("Hello, world from Console!");
}
finally
{
    // Don't forget to flush the channel, otherwise the logs will not be written your provider (Application Insights).
    await channel.FlushAsync(default);
    await Task.Delay(1000); // <-- This is to give the channel time to flush the logs to the console.
}