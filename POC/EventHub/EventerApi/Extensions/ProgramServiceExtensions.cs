using System.Text.Json;
using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Messaging.EventHubs.Producer;
using Azure.Security.KeyVault.Secrets;
using EventerApi.Models;

namespace EventerApi.Extensions;

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
            
            var keyVaultUri = JsonSerializer.Deserialize<KeyVaultUri>(setting.Value.Value);

            var secretClient = new SecretClient(new Uri(keyVaultUri.Uri), new DefaultAzureCredential());
            KeyVaultSecret? secret = null;
            
            try
            {
                secret = secretClient.GetSecret("EventHubConnectionString");
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                // throw new InvalidOperationException("The secret 'EventHubConnectionString' was not found in the Key Vault.", ex);
                Console.WriteLine("The secret 'EventHubConnectionString' was not found in the Key Vault.", ex);
            }

            string eventHubConnectionString = secret?.Value ?? "";
            return new EventHubProducerClient(eventHubConnectionString,"nt-ae-learningproject-ehub");
        });
    }
}