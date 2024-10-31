// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;

// //! This is great for services or long-running headless applications.
// using IHost host = Host.CreateDefaultBuilder(args)
//     .ConfigureLogging(x =>
//     {
//         x.AddJsonConsole();
//     })
//     .Build();
//
// //! This is how to get access to the logger.
// var logger = host.Services.GetRequiredService<ILogger<Program>>();
//
// logger.LogInformation("Hello, world!");
//
// host.Run();

//! The logger factory is used to create loggers. Knowing this, you can create a custom logger, and handle logging in 99% of the cases.
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddJsonConsole();
});

ILogger logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Hello, world!");