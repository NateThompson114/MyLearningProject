using InterceptorOAuthToNTLM.Enpoints;
using InterceptorOAuthToNTLM.Extensions;
using InterceptorOAuthToNTLM.TempItems;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddJwtOAuthToken();
builder.AddNTLMClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.AddSwaggerGenWithOAuth();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.AddJwtTokenReceiver();

app.MapPost("/validateToken", (string token) =>
{
    var gandalf = new Gandalf5000();

    return gandalf.ValidateJwtToken(token);
});

app.AddInterceptorEndpoint();
app.MapGet("/test", [Authorize]() => "Hello World");

app.Run();
