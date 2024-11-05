using Microsoft.Extensions.Logging;

namespace HighPerformanceLogging;
public class PaymentService
{
    private readonly ILogger<PaymentService> _logger;

    // This static object will be initialized once and reused for all calls to LogPayment
    // The string, decimal, and int parameters are now generic type parameters and not passed as an array of objects
    // Because they are now generic type parameters, the compiler can inline the calls to LogPayment, and not box the parameters
    // The event id is a struct so no memory allocation
    // The format string is a literal, so there is no real overhead
    // Finally the LogLevel is a enum or value type
    // private static readonly Action<ILogger, string, decimal, int, Exception?> LogPayment =
    //     LoggerMessage.Define<string, decimal, int>(
    //         LogLevel.Information,
    //         new EventId(1001, nameof(CreatePayment)),
    //         "Customer {email} purchased product {productId} for {amount}");

    public PaymentService(ILogger<PaymentService> logger)
    {
        _logger = logger;
    }

    public void CreatePayment(string email, decimal amount, int productId)
    {
        // Do some work
        
        // this._logger.LogInformation("Customer {email} purchased product {productId} for {amount}", new object[3]
        // {
        //     (object) email,
        //     (object) productId,
        //     (object) amount
        // });
        // _logger.LogInformation("Customer {email} purchased product {productId} for {amount}", email, productId, amount);
        
        // PaymentService.LogPayment = LoggerMessage.Define<string, Decimal, int>(LogLevel.Information, new EventId(1001, "CreatePayment"), "Customer {email} purchased product {productId} for {amount}");
        // LogPayment(_logger, email, amount, productId, null);
        
        // Doing this is the same as the above method where we created a static, and defined the parameters to avoid boxing,
        // but uses the source generator to define everything you would need, and in a as close to bare metal as C# gets
        _logger.LogPaymentCreation(email,amount,productId);
    }
}
