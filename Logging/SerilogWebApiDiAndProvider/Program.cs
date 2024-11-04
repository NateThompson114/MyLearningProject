using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Using Serilog.AspNetCore allows us to use the UseSerilog() extension method on the HostBuilder.
// This will allow us to use Serilog as the logging provider for the application and hijack the default logging.
// However, Serilog will not listen to the appsettings.json file, so we need to configure it manually otherwise we lose the ability to hotswap configures for the logger.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(theme:AnsiConsoleTheme.Code)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();