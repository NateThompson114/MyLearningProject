using Azure.Messaging.EventHubs.Producer;
using EventerApi.Extensions;
using EventerApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Define the minimal API endpoint
app.MapPost("/emit-event", async (EventHubProducerClient producerClient) =>
    {
        await EventEmitterService.EmitEvent(producerClient);
        return Results.Ok("Event sent to Event Hub");
    })
    .WithName("EmitEvent")
    .WithOpenApi();

app.Run();