using BillingApp.Core.DTOs;
using BillingApp.Core.Helpers;
using BillingApp.Core.Interfaces;
using BillingApp.Core.Mappings;

namespace BillingApp.Api.Extensions;

public static class BillingAppApiExtensions
{
    public static void RegisterBillingApiEndpoints(this WebApplication app)
    {
        app.MapPost("/api/bills", async (BillDto billDto, IBillRepository repo) =>
        {
            var (isValid, errorMessages) = ValidationHelper.Validate(billDto);
            if (!isValid)
            {
                return Results.BadRequest(new { Errors = errorMessages });
            }

            var bill = billDto.ToEntity();
            var id = await repo.AddAsync(bill);
            return Results.Created($"/api/bills/{id}", bill.ToDto());
        });

        app.MapPut("/api/bills/{id}", async (int id, BillDto billDto, IBillRepository repo) =>
        {
            var (isValid, errorMessages) = ValidationHelper.Validate(billDto);
            if (!isValid)
            {
                return Results.BadRequest(new { Errors = errorMessages });
            }

            var existingBill = await repo.GetByIdAsync(id);
            if (existingBill == null)
            {
                return Results.NotFound();
            }

            existingBill.UpdateEntityFromDto(billDto);
            await repo.UpdateAsync(existingBill);
            return Results.NoContent();
        });

        app.MapGet("/api/bills/{id}", async (int id, IBillRepository repo) =>
        {
            var bill = await repo.GetByIdAsync(id);
            return bill != null ? Results.Ok(bill) : Results.NotFound();
        });

        app.MapGet("/api/bills", async (IBillRepository repo) =>
        {
            var bills = await repo.GetAllAsync();
            return Results.Ok(bills);
        });
        
        app.MapDelete("/api/bills/{id}", async (int id, IBillRepository repo) =>
        {
            await repo.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}