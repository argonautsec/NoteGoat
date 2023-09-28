namespace NoteGoat.Logging;

public static class FileLoggerExtensions
{

        public const string NoteGoatLogFileName = "NoteGoat.log";
        public static string GetNoteGoatLogFilePath()
        {
                var cwd = Directory.GetCurrentDirectory();
                return Path.Join(cwd, NoteGoatLogFileName);
        }
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder)
        {
                var filePath = GetNoteGoatLogFilePath();
                builder.AddProvider(new FileLoggerProvider(filePath));
                return builder;
        }
}
