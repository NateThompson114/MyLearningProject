using HighProfmanceLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<PaymentService>();
    })
    .Build();

var example = host.Services.GetRequiredService<PaymentService>();

example.CreatePayment("user@email.com", 100.00m, 1);