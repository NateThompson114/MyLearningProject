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
            var validationResult = ValidationHelper.Validate(billDto);
            if (!validationResult.IsSuccess)
            {
                return Results.BadRequest(new { Errors = validationResult.Errors });
            }

            var bill = billDto.ToEntity();
            var result = await repo.AddAsync(bill);
            return result.IsSuccess
                ? Results.Created($"/api/bills/{result.Value}", bill.ToDto())
                : Results.BadRequest(new { Error = result.ErrorMessage });
        });

        app.MapPut("/api/bills/{id}", async (int id, BillDto billDto, IBillRepository repo) =>
        {
            var validationResult = ValidationHelper.Validate(billDto);
            if (!validationResult.IsSuccess)
            {
                return Results.BadRequest(new { Errors = validationResult.Errors });
            }

            var existingBillResult = await repo.GetByIdAsync(id);
            if (!existingBillResult.IsSuccess)
            {
                return Results.NotFound(new { Error = existingBillResult.ErrorMessage });
            }

            var existingBill = existingBillResult.Value;
            existingBill.UpdateEntityFromDto(billDto);
            var updateResult = await repo.UpdateAsync(existingBill);
            return updateResult.IsSuccess
                ? Results.NoContent()
                : Results.BadRequest(new { Error = updateResult.ErrorMessage });
        });

        app.MapGet("/api/bills/{id}", async (int id, IBillRepository repo) =>
        {
            var result = await repo.GetByIdAsync(id);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(new { Error = result.ErrorMessage });
        });

        app.MapGet("/api/bills", async (IBillRepository repo) =>
        {
            var result = await repo.GetAllAsync();
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(new { Error = result.ErrorMessage });
        });

        app.MapDelete("/api/bills/{id}", async (int id, IBillRepository repo) =>
        {
            var result = await repo.DeleteAsync(id);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(new { Error = result.ErrorMessage });
        });
    }
}