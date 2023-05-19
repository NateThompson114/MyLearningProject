using System.Net;
using System.Text.Json;
using InterceptorOAuthToNTLM.Helpers;
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

        //app.MapGet("/{**path}", async (string path, IHttpClientFactory httpClientFactory)=>
        //{
        //    var acceptablePaths = GetPropertiesFromClass.GetRouteInformation<Routes.Enpoints>();

        //    if (!path.StartsWith(Routes.Secure) || !acceptablePaths.Select(ap => ap.Value).Contains(path))
        //        return Results.NotFound();
            
        //    //switch (path)
        //    //{
        //    //    case 
        //    //}
        //    var client = httpClientFactory.CreateClient(Clients.NTLM);
        //    var newPath = path.ToLower().Substring($"{Routes.Secure}/".Length);
        //    var result = await client.GetAsync(newPath);

        //    if (result.IsSuccessStatusCode)
        //    {
        //        var content = await result.Content.ReadAsStringAsync();
        //        return Results.Ok(content);
        //    }
        //    else
        //    {
        //        return Results.Problem("Unable to get the data from the service");
        //    }
        //}).RequireAuthorization();

        //app.MapGet("/{**path}", async (string path, IHttpClientFactory httpClientFactory) =>
        //{
        //    if (!path.StartsWith($"{Routes.Secure}/"))
        //        return Results.NotFound();

        //    var newPath = path.ToLower().Substring($"{Routes.Secure}/".Length);

        //    string clientName;
        //    if (newPath.Contains(Routes.Enpoints.InternalApi))
        //    {
        //        newPath = "internal_api_uri" + newPath;
        //        clientName = Clients.InternalApi;
        //    }
        //    else if (newPath.Contains(Routes.Enpoints.PaymentProcessor))
        //    {
        //        newPath = "payment_processor_uri" + newPath;
        //        clientName = Clients.PaymentProcessor;
        //    }
        //    else if (newPath.Contains(Routes.Enpoints.PaymentProcessorV2))
        //    {
        //        newPath = "payment_processor_v2_uri" + newPath;
        //        clientName = Clients.PaymentProcessorV2;
        //    }
        //    else
        //    {
        //        return Results.BadRequest("Invalid path. Path must contain 'InternalApi', 'PaymentProcessor', or 'PaymentProcessorV2'.");
        //    }

        //    var client = httpClientFactory.CreateClient(clientName);
        //    var result = await client.GetAsync(newPath);

        //    if (result.IsSuccessStatusCode)
        //    {
        //        var content = await result.Content.ReadAsStringAsync();
        //        return Results.Ok(content);
        //    }
        //    else
        //    {
        //        return Results.Problem("Unable to get the data from the service");
        //    }
        //}).RequireAuthorization();

        app.MapGet("/{**path}", async (string path, IHttpClientFactory httpClientFactory) =>
        {
            if (!path.StartsWith($"{Routes.Secure}", StringComparison.CurrentCultureIgnoreCase))
                return Results.NotFound();

            var newPath = path.ToLower().Substring($"{Routes.Secure}/".Length-1);

            //string clientName;
            if (newPath.Contains(Routes.Enpoints.InternalApi, StringComparison.CurrentCultureIgnoreCase))
            {
                newPath = "internal_api_uri" + newPath;
                //clientName = Clients.InternalApi;
            }
            else if (newPath.Contains(Routes.Enpoints.PaymentProcessor))
            {
                newPath = "payment_processor_uri" + newPath;
                //clientName = Clients.PaymentProcessor;
            }
            else if (newPath.Contains(Routes.Enpoints.PaymentProcessorV2))
            {
                newPath = "payment_processor_v2_uri" + newPath;
                //clientName = Clients.PaymentProcessorV2;
            }
            else
            {
                return Results.BadRequest("Invalid path. Path must contain 'InternalApi', 'PaymentProcessor', or 'PaymentProcessorV2'.");
            }

            var client = httpClientFactory.CreateClient(Clients.NTLM);
            var result = await client.GetAsync(newPath);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                return Results.Ok(content);
            }
            else
            {
                return Results.Problem("Unable to get the data from the service");
            }
        }).RequireAuthorization();

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