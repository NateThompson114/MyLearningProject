using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Eventer.Helpers;

namespace Eventer.Services;

public static class EventEmitterService
{
    public static async Task EmitEvent(EventHubProducerClient producerClient)
    {
        var randomRequestHelper = new RandomRequestHelper();
        var request =randomRequestHelper.Generate();

        string message = JsonSerializer.Serialize(request);

        using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
        eventBatch.TryAdd(new EventData(message));

        await producerClient.SendAsync(eventBatch);
        Console.WriteLine("Event sent to Event Hub");
    }
}