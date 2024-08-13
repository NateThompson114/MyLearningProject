using BillingApp.Core.Entities;
using BillingApp.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace BillingApp.Infrastructure.Interceptor;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly List<AuditEntry> _auditEntries;

    public AuditInterceptor(IServiceProvider serviceProvider)
    {
        _auditEntries = serviceProvider.GetRequiredKeyedService<List<AuditEntry>>(KeyedServices.AuditEntries);
    }
    
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
        var startTime = DateTime.UtcNow;

        var auditEntries = eventData.Context.ChangeTracker
            .Entries()
            .Where(x => 
                x.Entity is not AuditEntry
                && x.State is EntityState.Added or EntityState.Modified or EntityState.Deleted
            )
            .Select(x => new AuditEntry
            {
                Id = Guid.NewGuid(),
                StartTimeUtc = startTime,
                Metadata = x.DebugView.LongView
            }).ToList();

        if (auditEntries.Count == 0)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);            
        }
        
        _auditEntries.AddRange(auditEntries);
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null)
        {
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }
        
        var endTime = DateTime.UtcNow;
        
        foreach (var auditEntry in _auditEntries)
        {
            auditEntry.EndTimeUtc = endTime;
            auditEntry.Succeeded = true;
        }
        
        if(_auditEntries.Count > 0)
        {
            eventData.Context.Set<AuditEntry>().AddRange(_auditEntries);
            _auditEntries.Clear();
            await eventData.Context.SaveChangesAsync(cancellationToken);
        }
        
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override async Task SaveChangesFailedAsync(DbContextErrorEventData eventData,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await base.SaveChangesFailedAsync(eventData, cancellationToken);

        if (eventData.Context is null)
        {
            return;
        }

        var endTime = DateTime.UtcNow;

        foreach (var auditEntry in _auditEntries)
        {
            auditEntry.EndTimeUtc = endTime;
            auditEntry.Succeeded = false;
            auditEntry.ErrorMessage = eventData.Exception.ToString();
        }

        if (_auditEntries.Count > 0)
        {
            // Create a new DbContext to save the audit entries
            // This is necessary because the original context is in an error state
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseSqlServer(eventData.Context.Database.GetConnectionString());
            await using var newContext = new DbContext(optionsBuilder.Options);

            newContext.Set<AuditEntry>().AddRange(_auditEntries);
            _auditEntries.Clear();
            
            try
            {
                await newContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Log the error if saving audit entries fails
                // You might want to use a proper logging framework here
                Console.WriteLine($"Failed to save audit entries for failed operation: {ex}");
            }
        }
    }
}