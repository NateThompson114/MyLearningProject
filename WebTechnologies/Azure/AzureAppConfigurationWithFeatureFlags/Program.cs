using AzureAppConfigurationWithFeatureFlags;
using Microsoft.Extensions.Options;
using AzureAppConfigurationWithFeatureFlags.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureAppConfigurationAndKeyVaultServices();

var app = builder.Build();

app.UseAzureAppConfiguration();

app.MapGet("/", () => "Hello World!");
app.MapGet("Test", async ([FromServices] IFeatureManager manger) => await manger.IsEnabledAsync(nameof(Flags.Test)));
app.MapGet("Missing", ([FromServices] IFeatureManager manger) => manger.IsEnabledAsync(nameof(Flags.Missing)));

app.Run();
