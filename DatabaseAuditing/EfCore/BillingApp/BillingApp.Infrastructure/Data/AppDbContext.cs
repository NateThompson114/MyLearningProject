using BillingApp.Core.Entities;
using BillingApp.Infrastructure.Interceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BillingApp.Infrastructure.Data;

// This is through jetbrains console, there are easier methods to do this if your using visual studio
// dotnet tool install --global dotnet-ef
// dotnet ef migrations add InitialCreate --project ../BillingApp.Infrastructure --startup-project ../BillingApp.Api
// dotnet ef database update --project ../BillingApp.Infrastructure --startup-project ../BillingApp.Api

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
    {
    }

    public DbSet<Bill> Bills { get; set; }
    public DbSet<AuditEntry> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>()
            .Property(b => b.Amount)
            .HasPrecision(18, 2);
    }
}