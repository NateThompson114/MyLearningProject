using BillingApp.Core.Entities;
using BillingApp.Core.Interfaces;
using BillingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<AuditEntry>> GetAllAsync()
    {
        return await _context.AuditLogs.ToListAsync();
    }

    public async Task<AuditEntry?> GetByIdAsync(int id)
    {
        return await _context.AuditLogs.FindAsync(id);
    }
    
    public async Task AddLogAsync(AuditEntry entry)
    {
        _context.AuditLogs.Add(entry);
        await _context.SaveChangesAsync();
    }
}