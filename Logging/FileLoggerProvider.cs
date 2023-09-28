using System.Collections.Concurrent;

namespace NoteGoat.Logging;

public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, FileLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);
    private readonly string _filePath;

    public FileLoggerProvider(string filePath)
    {
        _filePath = filePath;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(
                categoryName,
                name => new FileLogger(_filePath, categoryName)
        );
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
