// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

//! This is great for services or long-running headless applications.
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(x =>
    {
        x.AddJsonConsole();
    })
    .Build();

//! This is how to get access to the logger.
var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Hello, world!");

host.Run();