using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Cocona;
using Eventer.Extensions;
using Eventer.Helpers;
using Eventer.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

builder.Services.ConfigureServices();

var app = builder.Build();

// Define the command
app.AddCommand("emit-event", async (EventHubProducerClient producerClient) =>
{
    await EventEmitterService.EmitEvent(producerClient);
});

var provider = builder.Services.BuildServiceProvider();
var producerClient = provider.GetRequiredService<EventHubProducerClient>();
await EventEmitterService.EmitEvent(producerClient);

// Run the emit-event command directly
await app.RunAsync();