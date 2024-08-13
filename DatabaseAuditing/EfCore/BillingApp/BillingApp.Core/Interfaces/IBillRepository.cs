using BillingApp.Core.Common;
using BillingApp.Core.Entities;

namespace BillingApp.Core.Interfaces;

public interface IBillRepository
{
    Task<ResultType<IEnumerable<Bill>>> GetAllAsync();
    Task<ResultType<Bill>> GetByIdAsync(int id);
    Task<ResultType<int>> AddAsync(Bill bill);
    Task<ResultType<Bill>> UpdateAsync(Bill bill);
    Task<ResultType<bool>> DeleteAsync(int id);
}