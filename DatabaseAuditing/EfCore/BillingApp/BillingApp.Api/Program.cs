using BillingApp.Core.Entities;
using BillingApp.Core.Interfaces;
using BillingApp.Infrastructure.Data;
using BillingApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Bill endpoints
app.MapGet("/api/bills", async (IBillRepository repo) =>
    await repo.GetAllAsync());

app.MapGet("/api/bills/{id}", async (int id, IBillRepository repo) =>
    await repo.GetByIdAsync(id) is Bill bill
        ? Results.Ok(bill)
        : Results.NotFound());

app.MapPost("/api/bills", async (Bill bill, IBillRepository repo) =>
{
    var id = await repo.AddAsync(bill);
    return Results.Created($"/api/bills/{id}", bill);
});

app.MapPut("/api/bills/{id}", async (int id, Bill bill, IBillRepository repo) =>
{
    var existingBill = await repo.GetByIdAsync(id);
    if (existingBill == null) return Results.NotFound();

    bill.Id = id;
    await repo.UpdateAsync(bill);
    return Results.NoContent();
});

app.MapDelete("/api/bills/{id}", async (int id, IBillRepository repo) =>
{
    await repo.DeleteAsync(id);
    return Results.NoContent();
});

app.Run();