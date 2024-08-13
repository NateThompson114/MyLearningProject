using System.ComponentModel.DataAnnotations;

namespace BillingApp.Core.Entities;

public class Bill
{
    [Key]
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal Amount { get; set; }
    public DateTime BillingDate { get; set; }
}