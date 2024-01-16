using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace TwoAuthentications.Extensions
{
    public static class BuilderExtensions
    {
        public static void ConfigureAuthentication(this WebApplicationBuilder builder )
        {
            IConfiguration configuration = builder.Configuration;
            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));
        }
    }
}
