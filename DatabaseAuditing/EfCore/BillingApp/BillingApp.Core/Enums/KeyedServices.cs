using Ardalis.SmartEnum;

namespace BillingApp.Core.Enums;

public class KeyedServices : SmartEnum<KeyedServices>
{
    public static readonly KeyedServices AuditEntries = new(nameof(AuditEntries), 1);
    
    public KeyedServices(string name, int value) : base(name, value)
    {
    }
}