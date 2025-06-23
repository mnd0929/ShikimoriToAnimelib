using Microsoft.Extensions.Logging;

namespace ShikimoriToAnimelib.Logging
{
    public class ConsoleLogger : ILogger
    {
        private readonly string CategoryName;

        private readonly Func<LogLevel, bool> Filter;

        public ConsoleLogger(string categoryName, Func<LogLevel, bool> filter = null!)
        {
            CategoryName = categoryName;
            Filter = filter ?? (level => true);
        }

        public bool IsEnabled(LogLevel logLevel) => Filter(logLevel);

        public IDisposable BeginScope<TState>(TState state) => ConsoleScope.Push(state!);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            string message = formatter?.Invoke(state, exception) ?? state!.ToString()!;

            if (string.IsNullOrEmpty(message) && exception == null) return;

            string scopes = ConsoleScope.CurrentScopes;
            string logLine = $"[{DateTime.UtcNow:HH:mm:ss}] ({logLevel}) {scopes}{message}";

            Console.ForegroundColor = logLevel switch
            {
                LogLevel.Error => ConsoleColor.DarkRed,
                LogLevel.Warning => ConsoleColor.DarkYellow,
                _ => Console.ForegroundColor
            };
            Console.WriteLine(logLine);
            Console.ResetColor();

            if (exception != null)
                Console.WriteLine(exception);
        }
    }
}
