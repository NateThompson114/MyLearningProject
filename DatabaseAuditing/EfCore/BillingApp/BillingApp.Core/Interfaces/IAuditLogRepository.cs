using BillingApp.Core.Entities;

namespace BillingApp.Core.Interfaces;

public interface IAuditLogRepository
{
    Task AddLogAsync(AuditEntry entry);
}