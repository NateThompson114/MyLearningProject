using Microsoft.Extensions.Logging;

namespace HighPerformanceLogging;

public static partial class LoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Information, EventId = 5001, Message = "Customer {email} purchased product {productId} for {amount}")]
    public static partial void LogPaymentCreation(this ILogger logger, string email, decimal amount, int productId);
}