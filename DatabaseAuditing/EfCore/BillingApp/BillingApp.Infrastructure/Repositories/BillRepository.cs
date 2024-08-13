using BillingApp.Core.Entities;
using BillingApp.Core.Interfaces;
using BillingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Infrastructure.Repositories;

public class BillRepository : IBillRepository
{
    private readonly AppDbContext _context;

    public BillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Bill>> GetAllAsync()
    {
        return await _context.Bills.ToListAsync();
    }

    public async Task<Bill> GetByIdAsync(int id)
    {
        return await _context.Bills.FindAsync(id);
    }

    public async Task<int> AddAsync(Bill bill)
    {
        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();
        return bill.Id;
    }

    public async Task UpdateAsync(Bill bill)
    {
        _context.Bills.Update(bill);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill != null)
        {
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
        }
    }
}