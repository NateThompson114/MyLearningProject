﻿// See https://aka.ms/new-console-template for more information

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

//This would produce nothing because serilog while configured, does not have a sink, which is the same as a provider in the Microsoft.Extensions.Logging world.
var logger = new LoggerConfiguration()
    .WriteTo.Console(theme:AnsiConsoleTheme.Code)
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit:true )
    .CreateLogger();

Log.Logger = logger;

logger.Information("Hello, world!");

//This is important to flush the log and close the logger especially if you are using a file sink.
Log.CloseAndFlush();