namespace DefaultNamespace;

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

public class NatesLogeer: ILogger
{
    public IDisposable BeginScope<TState>(TState state)
    {
        return default;
    }
    
    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        Console.WriteLine($"[{eventId.Id}] {formatter(state, exception)}");
    }
}