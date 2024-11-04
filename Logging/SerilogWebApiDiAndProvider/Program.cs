using Serilog;
using SerilogWebApiDiAndProvider.AdvancedObjects;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Using Serilog.AspNetCore allows us to use the UseSerilog() extension method on the HostBuilder.
// This will allow us to use Serilog as the logging provider for the application and hijack the default logging.
// However, Serilog will not listen to the appsettings.json file, so we need to configure it manually otherwise we lose the ability to hotswap configures for the logger.
var logger = new LoggerConfiguration()
    // .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) // You can do this as a manual override, but it's not recommended.
    // .WriteTo.Console(theme:AnsiConsoleTheme.Code)
    .Destructure.ByTransforming<Payment>(x => new
    {
        // This will destructure the object into a new object with the properties PaymentId and UserId.
        x.PaymentId,
        x.UserId
    })
    .ReadFrom.Configuration(builder.Configuration) // This will read the configuration from the appsettings.json file, so we no longer need to provide the configuration manually.
    .CreateLogger();
Log.Logger = logger;

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

var payment = new Payment
{
    PaymentId = 1,
    UserId = Guid.NewGuid(),
    Timestamp = DateTime.Now
};

// By including the `@` symbol before the object, we can log the object as a structured object. Without the `@` symbol, the object type name would be logged as a string.
// [15:34:11 INF] New payment with data: {"PaymentId": 1, "UserId": "b97b9b66-ae2f-4a11-87e8-c6abf8f3db60", "Timestamp": "2024-11-04T15:34:11.5047408-05:00", "$type": "Payment"}
logger.Information("New payment with data: {@Payment}", payment);
// [15:42:29 INF] New payment with data: SerilogWebApiDiAndProvider.AdvancedObjects.Payment
logger.Information("New payment with data: {Payment}", payment);

var paymentData = new Dictionary<string, object>
{
    { "PaymentId", 1 },
    { "UserId", Guid.NewGuid() },
    { "Timestamp", DateTime.Now }
};

// However with a dictionary, list, or other collections, the object is automatically logged as a structured object, but does not include the type name.
//      [15:42:29 INF] New payment with data: {"PaymentId": 1, "UserId": "14c37fa4-f159-411d-ba9b-798f72a90634", "Timestamp": "2024-11-04T15:42:29.3777787-05:00"}
logger.Information("New payment with data: {PaymentData}", paymentData);
// To get the object name from a dictionary you must include the `$` symbol before the object.
//      [15:45:09 INF] New payment with data: System.Collections.Generic.Dictionary`2[System.String,System.Object]
logger.Information("New payment with data: {$PaymentData}", paymentData);



app.Run();