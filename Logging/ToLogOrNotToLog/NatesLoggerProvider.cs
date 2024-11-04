namespace ToLogOrNotToLog;

/// <summary>
/// This is a VERY basic logger provider, this is a good starting point and could lead into some more advanced logging. Things like logging to a file, and or database.
/// </summary>
public class NatesLoggerProvider: ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new NatesLogger();
    }
    
    public void Dispose()
    {
    }
    
}

public class NatesLogger: ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return NullScope.Instance;
    }
    
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }
    
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        
        Console.WriteLine($"[{logLevel}({eventId.Id})] {formatter(state, exception)}");
    }
    
    internal sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new();
        private NullScope() { }
        public void Dispose() { }
        
    }
}