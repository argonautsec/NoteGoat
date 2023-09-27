namespace NoteGoat.Logging;

public class MemoryLogger : ILogger
{
    IEnumerable<string> _logs;

    public MemoryLogger(int size)
    {
        _logs = new List<string>(size);
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        var n = Environment.NewLine;
        lock (_logs)
        {
            var exc = "";
            if (exception != null)
            {
                exc = n + exception.GetType() + ":" + exception.Message + n + exception.StackTrace;
            }
            var log =
                "["
                + logLevel.ToString()
                + "]"
                + DateTime.Now.ToString()
                + formatter(state, exception)
                + n
                + exc;
        }
    }
}
