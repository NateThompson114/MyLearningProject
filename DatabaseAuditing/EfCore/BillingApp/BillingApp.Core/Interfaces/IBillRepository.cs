using BillingApp.Core.Entities;

namespace BillingApp.Core.Interfaces;

public interface IBillRepository
{
    Task<IEnumerable<Bill?>> GetAllAsync();
    Task<Bill?> GetByIdAsync(int id);
    Task<int> AddAsync(Bill bill);
    Task UpdateAsync(Bill bill);
    Task DeleteAsync(int id);
}