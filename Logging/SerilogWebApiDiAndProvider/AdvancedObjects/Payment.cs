﻿namespace SerilogWebApiDiAndProvider.AdvancedObjects;

public class Payment
{
    public int PaymentId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Timestamp { get; set; }
}