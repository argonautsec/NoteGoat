using System.Text;

namespace NoteGoat.Logging;

public class FileLogger : ILogger
{
        private readonly string _filePath;
        private readonly string _category;

        public FileLogger(string filePath, string category)
        {
                _filePath = filePath;
                _category = category;
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
                Func<TState, Exception?, string> format
        )
        {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder
                        .Append("[")
                        .Append(logLevel)
                        .Append("] ")
                        .Append(_category)
                        .Append(" [")
                        .Append(DateTime.Now)
                        .Append("] ")
                        .AppendLine(format(state, exception));
                if (exception != null)
                {
                        stringBuilder
                                .Append(exception.GetType())
                                .Append(':')
                                .AppendLine(exception.Message)
                                .AppendLine(exception.StackTrace);
                }
                var log = stringBuilder.ToString();
                File.AppendAllText(_filePath, log);
        }
}
