using System.Net;
using System.Text.Json;
using InterceptorOAuthToNTLM.Models;
using InterceptorOAuthToNTLM.Statics;
using InterceptorOAuthToNTLM.TempItems;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InterceptorOAuthToNTLM.Enpoints;

public static class Endpoints
{
    public static void AddInterceptorEndpoint(this WebApplication app)
    {

        app.MapGet("/{**path}", (string path, IHttpClientFactory httpClientFactory)=>
        {

            var client = httpClientFactory.CreateClient(Clients.NTLM);
            var result = await client.GetAsync("YourServiceEndpoint");

            // Rest of your code here
        });
    }

    public static void AddJwtTokenReceiver(this WebApplication app)
    {
        app.MapPost("/getToken", async ([FromBody] ClientCredentialsRequest credentials, IHttpClientFactory httpClientFactory) =>
        {
            if (!credentials.IsValid())
                return Results.BadRequest();

            var tokenEndpoint = "YourTokenEndpoint"; // Replace with your token endpoint
            var client = httpClientFactory.CreateClient(Clients.OAuth);

            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", credentials.ClientId!),
                new KeyValuePair<string, string>("client_secret", credentials.ClientSecret!),
                new KeyValuePair<string, string>("scope", credentials.Scope),
            });

            //var response = await client.PostAsync(tokenEndpoint, requestBody);
            var gandalf = new Gandalf5000();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(gandalf.GenerateJwtToken(credentials.ClientId, credentials.ClientSecret,
                    credentials.Scope)),
            };

            if (!response.IsSuccessStatusCode) 
                return Results.Problem("Unable to get the token");

            var content = await response.Content.ReadAsStringAsync();
            return Results.Ok(content);

        }).AllowAnonymous();
    }
}