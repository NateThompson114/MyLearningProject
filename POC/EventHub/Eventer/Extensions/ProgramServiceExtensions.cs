using Azure.Data.AppConfiguration;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace Eventer.Extensions;

public static class ProgramServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        string appConfigConnectionString = "";

        services.AddSingleton(new ConfigurationClient(appConfigConnectionString));
        services.AddSingleton<EventHubProducerClient>(provider =>
        {
            var client = provider.GetRequiredService<ConfigurationClient>();
            var setting = client.GetConfigurationSetting("EventHubConnectionStringManual", null);
            string eventHubConnectionString = setting.Value.Value;
            return new EventHubProducerClient(eventHubConnectionString, "nt-ae-learningproject-ehub");
        });
    }
}