using System.Text;
using InterceptorOAuthToNTLM.Filter;
using InterceptorOAuthToNTLM.Handlers;
using InterceptorOAuthToNTLM.Statics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InterceptorOAuthToNTLM.Extensions;

public static class ServicesExtensions
{
    public static void AddJwtOAuthToken(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "GandalfTheGray",
                    ValidAudience = "TheHobbits",
                    IssuerSigningKey = new SymmetricSecurityKey("I am a servant of the Secret Fire, wielder of the flame of Anor. You cannot pass."u8.ToArray())
                };
            });
        builder.Services.AddAuthorization();
    }

    public static void AddNTLMClient(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<HttpMessageHandler>(sp =>
            new NTLMHandler { InnerHandler = new HttpClientHandler() });

        builder.Services.AddHttpClient(Clients.NTLM, client =>
        {
            client.BaseAddress = new Uri("YourServiceURL");
        }).ConfigurePrimaryHttpMessageHandler<NTLMHandler>();
    }

    public static void AddSwaggerGenWithOAuth(this WebApplicationBuilder builder)
    {
        const string bearer = "bearer";
        var bearerToken = $"{bearer} Token";

        const string authTerm = "Authorization";

        builder.Services.Configure<SwaggerGenOptions>(config =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Description = $"JWT {authTerm} header using the {bearer} scheme. \r\n\r\n" +
                        $"Enter '{bearer} [space] and then your token in the text input below. \r\n\r\n" +
                        $"Example: {bearer} 123abc",
                Name = authTerm,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = bearer
            };

            config.AddSecurityDefinition(bearerToken, securityScheme);

            config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = bearerToken
                            },
                            Scheme = bearer,
                            Name = authTerm,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
        });

        builder.Services.AddSwaggerGen(opts =>
        {
            const string title = "Our Interceptor API";
            const string description = "This is a Web API that demonstrates Interceptor.";
            var terms = new Uri("https://localhost:7175/terms");

            var license = new OpenApiLicense()
            {
                Name = "This is my full license information or a link to it"
            };
            var contact = new OpenApiContact()
            {
                Name = "AvidXChange Helpdesk",
                Email = "support@avidxchange.com",
                Url = new Uri("https://www.avidxchange.com/support")
            };

            opts.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = $"{title} v1",
                //Title = $"{title} v1 (deprecated)",
                Description = description,
                TermsOfService = terms,
                License = license,
                Contact = contact
            });

            //opts.SwaggerDoc("v2", new OpenApiInfo
            //{
            //    Version = "v2",
            //    Title = $"{title} v2",
            //    Description = description,
            //    TermsOfService = terms,
            //    License = license,
            //    Contact = contact
            //});
        });
    }
}