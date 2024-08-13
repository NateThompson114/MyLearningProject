using BillingApp.Core.Common;
using BillingApp.Core.Entities;
using BillingApp.Core.Interfaces;
using BillingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Infrastructure.Repositories;

 public class BillRepository(AppDbContext context) : IBillRepository
 {
     public async Task<ResultType<IEnumerable<Bill>>> GetAllAsync()
    {
        var bills = await context.Bills.ToListAsync();
        return ResultType<IEnumerable<Bill>>.Success(bills);
    }

    public async Task<ResultType<Bill>> GetByIdAsync(int id)
    {
        var bill = await context.Bills.FindAsync(id);
        return bill != null 
            ? ResultType<Bill>.Success(bill) 
            : ResultType<Bill>.Failure($"Bill with id {id} not found");
    }

    public async Task<ResultType<int>> AddAsync(Bill bill)
    {
        context.Bills.Add(bill);
        await context.SaveChangesAsync();
        return ResultType<int>.Success(bill.Id);
    }

    public async Task<ResultType<Bill>> UpdateAsync(Bill bill)
    {
        context.Entry(bill).State = EntityState.Modified;
        try
        {
            await context.SaveChangesAsync();
            return ResultType<Bill>.Success(bill);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await BillExists(bill.Id))
            {
                return ResultType<Bill>.Failure($"Bill with id {bill.Id} not found");
            }
            else
            {
                throw;
            }
        }
    }

    public async Task<ResultType<bool>> DeleteAsync(int id)
    {
        var bill = await context.Bills.FindAsync(id);
        if (bill == null)
        {
            return ResultType<bool>.Failure($"Bill with id {id} not found");
        }

        context.Bills.Remove(bill);
        await context.SaveChangesAsync();
        return ResultType<bool>.Success(true);
    }

    private async Task<bool> BillExists(int id)
    {
        return await context.Bills.AnyAsync(e => e.Id == id);
    }
}