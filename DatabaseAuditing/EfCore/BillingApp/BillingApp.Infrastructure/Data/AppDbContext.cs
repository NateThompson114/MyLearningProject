using BillingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingApp.Infrastructure.Data;

// dotnet tool install --global dotnet-ef
// dotnet ef migrations add InitialCreate --project ../BillingApp.Infrastructure --startup-project ../BillingApp.Api
// dotnet ef database update --project ../BillingApp.Infrastructure --startup-project ../BillingApp.Api

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Bill> Bills { get; set; }
    public DbSet<AuditEntry> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add any additional configurations here
    }
}