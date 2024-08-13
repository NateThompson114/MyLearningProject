using BillingApp.Core.DTOs;
using BillingApp.Core.Exceptions;
using BillingApp.Core.Helpers;
using BillingApp.Core.Interfaces;
using BillingApp.Core.Mappings;

namespace BillingApp.Api.Endpoints;

public static class BillEndpoints
    {
        public static void MapBillEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/bills", CreateBill);
            app.MapPut("/api/bills/{id}", UpdateBill);
            app.MapGet("/api/bills/{id}", GetBill);
            app.MapGet("/api/bills", GetAllBills);
            app.MapDelete("/api/bills/{id}", DeleteBill);
        }

        private static async Task<IResult> CreateBill(BillDto billDto, IBillRepository repo)
        {
            try
            {
                var validationResult = ValidationHelper.Validate(billDto);
                if (!validationResult.IsSuccess)
                {
                    throw new CustomException(string.Join(", ", validationResult.Errors), 400);
                }

                var bill = billDto.ToEntity();
                var result = await repo.AddAsync(bill);
                return Results.Created($"/api/bills/{result.Value}", bill);
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomException($"An error occurred while creating the bill: {ex.Message}", 500);
            }
        }

        private static async Task<IResult> UpdateBill(int id, BillDto billDto, IBillRepository repo)
        {
            try
            {
                var validationResult = ValidationHelper.Validate(billDto);
                if (!validationResult.IsSuccess)
                {
                    throw new CustomException(string.Join(", ", validationResult.Errors), 400);
                }

                var existingBillResult = await repo.GetByIdAsync(id);
                var existingBill = existingBillResult.Value;
                existingBill.UpdateEntityFromDto(billDto);
                var updateResult = await repo.UpdateAsync(existingBill);
                return Results.NoContent();
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomException($"An error occurred while updating the bill: {ex.Message}", 500);
            }
        }

        private static async Task<IResult> GetBill(int id, IBillRepository repo)
        {
            try
            {
                var result = await repo.GetByIdAsync(id);
                return Results.Ok(result.Value);
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomException($"An error occurred while retrieving the bill: {ex.Message}", 500);
            }
        }

        private static async Task<IResult> GetAllBills(IBillRepository repo)
        {
            try
            {
                var result = await repo.GetAllAsync();
                return Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                throw new CustomException($"An error occurred while retrieving bills: {ex.Message}", 500);
            }
        }

        private static async Task<IResult> DeleteBill(int id, IBillRepository repo)
        {
            try
            {
                var result = await repo.DeleteAsync(id);
                return Results.NoContent();
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomException($"An error occurred while deleting the bill: {ex.Message}", 500);
            }
        }
    }