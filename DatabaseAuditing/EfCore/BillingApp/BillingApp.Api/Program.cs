using BillingApp.Api.Endpoints;
using BillingApp.Api.Middleware;
using BillingApp.Core.Entities;
using BillingApp.Core.Enums;
using BillingApp.Core.Interfaces;
using BillingApp.Infrastructure.Data;
using BillingApp.Infrastructure.Interceptor;
using BillingApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddKeyedScoped<List<AuditEntry>>(KeyedServices.AuditEntries, (_, _) => new());
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.AddInterceptors(new AuditInterceptor(serviceProvider));
});

builder.Services.AddScoped<IBillRepository, BillRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Configuration.GetValue<bool>("UseSwagger", true))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Add the global error handler middleware
app.UseGlobalErrorHandler();

// Map the bill endpoints
app.MapBillEndpoints();
app.Run();