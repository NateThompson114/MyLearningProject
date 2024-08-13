using BillingApp.Core.Entities;
using BillingApp.Core.Interfaces;
using BillingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Infrastructure.Repositories;

public class BillRepository(AppDbContext context) : IBillRepository
{
    public async Task<IEnumerable<Bill?>> GetAllAsync() => 
        await context.Bills.ToListAsync();

    public async Task<Bill?> GetByIdAsync(int id) => 
        await context.Bills.FindAsync(id);

    public async Task<int> AddAsync(Bill bill)
    {
        context.Bills.Add(bill);
        await context.SaveChangesAsync();
        return bill.Id;
    }

    public async Task UpdateAsync(Bill bill)
    {
        context.Bills.Update(bill);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var bill = await context.Bills.FindAsync(id);
        if (bill != null)
        {
            context.Bills.Remove(bill);
            await context.SaveChangesAsync();
        }
    }
}