using ToLogOrNotToLog.Models.Enum;

namespace ToLogOrNotToLog.Examples;

public static class Examples
{
    private const int PaymentId = 987654321;
    public static ILogger StructuredLoggingExample(this ILogger logger)
    {
        // Bad and good (Structured Logging) examples of logging
    
        logger.LogInformation($"Payment Id: {PaymentId}");                  // {"EventId":0,"LogLevel":"Information","Category":"Program","Message":"Payment Id: 987654321","State":{"Message":"Payment Id: 987654321","{OriginalFormat}":"Payment Id: 987654321"}} 
        logger.LogInformation("Payment Id: " + PaymentId);                  // {"EventId":0,"LogLevel":"Information","Category":"Program","Message":"Payment Id: 987654321","State":{"Message":"Payment Id: 987654321","{OriginalFormat}":"Payment Id: 987654321"}}
        logger.LogInformation(string.Format("Payment Id: {0}", PaymentId)); // {"EventId":0,"LogLevel":"Information","Category":"Program","Message":"Payment Id: 987654321","State":{"Message":"Payment Id: 987654321","{OriginalFormat}":"Payment Id: 987654321"}}
        logger.LogInformation("Payment Id: {PaymentId}", PaymentId);        // {"EventId":0,"LogLevel":"Information","Category":"Program","Message":"Payment Id: 987654321","State":{"Message":"Payment Id: 987654321","PaymentId":987654321,"{OriginalFormat}":"Payment Id: {PaymentId}"}}

        return logger;
    }

    public static ILogger EventIdExample(this ILogger logger)
    {
        int? paymentId = null;
        
        if (paymentId is null)
        {
            logger.LogWarning(LogEvents.PaymentMissingId, "Payment Id is missing.");    // {"EventId":1001,"LogLevel":"Warning","Category":"Program","Message":"Payment Id is missing.","State":{"Message":"Payment Id is missing.","{OriginalFormat}":"Payment Id is missing."}}
            logger.LogWarning(LogEvents.PaymentMissingId, "This shouldn't happen!");    // {"EventId":1001,"LogLevel":"Warning","Category":"Program","Message":"This shouldn\u0027t happen!","State":{"Message":"This shouldn\u0027t happen!","{OriginalFormat}":"This shouldn\u0027t happen!"}}
            logger.LogWarning(LogEvents.PaymentMissingId, "but id did...");             // {"EventId":1001,"LogLevel":"Warning","Category":"Program","Message":"but id did...","State":{"Message":"but id did...","{OriginalFormat}":"but id did..."}}
            logger.LogWarning(LogEvents.PaymentMissingId, "This is a example that can be used to have a grouping event id, that can be used to filter logs.");
            // {"EventId":1001,"LogLevel":"Warning","Category":"Program","Message":"This is a example that can be used to have a grouping event id, that can be used to filter logs.","State":{"Message":"This is a example that can be used to have a grouping event id, that can be used to filter logs.","{OriginalFormat}":"This is a example that can be used to have a grouping event id, that can be used to filter logs."}}
        }

        return logger;
    }
}