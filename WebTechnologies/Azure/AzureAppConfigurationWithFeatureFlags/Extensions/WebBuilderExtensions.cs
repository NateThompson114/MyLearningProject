using Azure.Identity;
using Microsoft.FeatureManagement;

namespace AzureAppConfigurationWithFeatureFlags.Extensions;

public static class WebBuilderExtensions
{
    public static void AddAzureAppConfigurationAndKeyVaultServices(this WebApplicationBuilder builder)
    {
        var appConfigConnectionString = builder.Configuration.GetConnectionString("AppConfig");
        // var credential = new DefaultAzureCredential(); //<-1 Should use for application also removes dangerous connection string

        builder.Services.AddAzureAppConfiguration();
        builder.Services.AddFeatureManagement();

        builder.Configuration.AddAzureAppConfiguration(options =>
        {
            // options.Connect(new Uri(appConfigConnectionString), credential); //<-1 Should use with the credential
            options.Connect(appConfigConnectionString);
            options.UseFeatureFlags(flagOptions =>
            {
                flagOptions.CacheExpirationInterval = TimeSpan.FromSeconds(5); //This can be set to 5 minutes to reduce the pooling
            });
        });
    }
}
