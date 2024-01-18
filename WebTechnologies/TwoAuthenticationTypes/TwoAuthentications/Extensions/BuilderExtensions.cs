using AvidXchange.Enterprise.AvidPlatforms.AvidAuth.AspNetCore.JwtBearer.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace TwoAuthentications.Extensions
{
    public static class BuilderExtensions
    {
        public static void ConfigureAuthentication(this WebApplicationBuilder builder )
        {
            IConfiguration configuration = builder.Configuration;

            var azureAdOptions = configuration.GetSection("AzureAd").Get<AzureAdOptions>();
            var avidAuthOptions = configuration.GetSection("AvidAuth").Get<AvidAuthOptions>();

            var avidAuthJwtBearerOptions = new AvidAuthJwtBearerOptions(authority: avidAuthOptions.Authority,
                audience: avidAuthOptions.Audience);

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(CustomBearerOptions.AvidAuth, options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.Authority = avidAuthJwtBearerOptions.Authority;
                    options.Audience = avidAuthJwtBearerOptions.Audience;
                    options.Validate();

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context => avidAuthJwtBearerOptions.Events.MessageReceived(context),
                        OnTokenValidated = context => avidAuthJwtBearerOptions.Events.OnTokenValidated(context),
                        OnAuthenticationFailed = context =>
                            avidAuthJwtBearerOptions.Events.OnAuthenticationFailed(context),
                        OnForbidden = context => avidAuthJwtBearerOptions.Events.OnForbidden(context),
                        OnChallenge = context => avidAuthJwtBearerOptions.Events.OnChallenge(context),
                    };
                })
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

            builder.Services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(
                        JwtBearerDefaults.AuthenticationScheme
                        ,CustomBearerOptions.AvidAuth
                    )
                    .Build();
            });
        }

        public class AvidAuthOptions
        {
            public string? Info { get; set; }
            public string? Audience { get; set; }
            public string? Authority { get; set; }
        }

        public class AzureAdOptions
        {
            public string? Instance { get; set; }
            public string? Domain { get; set; }
            public string? TenantId { get; set; }
            public string? ClientId { get; set; }
            public string? CallbackPath { get; set; }
        }
        
        public static class CustomBearerOptions
        {
            public const string AvidAuth = "AvidAuth";
        }
    }
}
