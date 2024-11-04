// See https://aka.ms/new-console-template for more information

using Serilog;

//This would produce nothing because serilog while configured, does not have a sink, which is the same as a provider in the Microsoft.Extensions.Logging world.
var logger = new LoggerConfiguration()
    .CreateLogger();

logger.Information("Hello, world!"); 