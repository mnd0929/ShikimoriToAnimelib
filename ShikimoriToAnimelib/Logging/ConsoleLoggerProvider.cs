using Microsoft.Extensions.Logging;

namespace ShikimoriToAnimelib.Logging
{
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        private readonly Func<LogLevel, bool> Filter;

        public ConsoleLoggerProvider(Func<LogLevel, bool> filter = null!)
        {
            Filter = filter;
        }

        public ILogger CreateLogger(string categoryName) =>
            new ConsoleLogger(categoryName, Filter);

        public void Dispose() { }
    }
}
