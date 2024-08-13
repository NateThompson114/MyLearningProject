using System.ComponentModel.DataAnnotations;

namespace BillingApp.Core.DTOs;

public class BillDto
{
    [MaxLength(100)]
    public required string CustomerName { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public required decimal Amount { get; set; }

    private DateTime? _billingDate;
    public DateTime BillingDate
    {
        get => _billingDate ?? DateTime.UtcNow;
        set => _billingDate = value;
    }
}