using Destructurama.Attributed;

namespace SerilogWebApiDiAndProvider.AdvancedObjects;

public class Payment
{
    public int PaymentId { get; set; }
    public Guid UserId { get; set; }
    
    [LogMasked(ShowFirst = 3, PreserveLength = true, ShowLast = 3)]
    public string Email { get; set; }
    public DateTime Timestamp { get; set; }
}