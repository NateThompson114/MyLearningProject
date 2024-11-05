using Microsoft.Extensions.Logging;

namespace HighProfmanceLogging;
public class PaymentService
{
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(ILogger<PaymentService> logger)
    {
        _logger = logger;
    }

    public void CreatePayment(string email, decimal amount, int productId)
    {
        // Do some work
        _logger.LogInformation("Customer {email} purchased product {productId} for {amount}", email, productId, amount);
    }
}
