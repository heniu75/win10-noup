using System;
using Microsoft.Extensions.Logging;

namespace Win10NoUp.Library
{
    // see https://stackoverflow.com/questions/44230373/is-there-a-way-to-format-the-output-format-in-net-core-logging
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly IDateTimeProvider _provider;

        public CustomLoggerProvider(IDateTimeProvider provider)
        {
            _provider = provider;
        }

        public void Dispose() { }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomConsoleLogger(categoryName, _provider);
        }

        public class CustomConsoleLogger : ILogger
        {
            private readonly string _categoryName;
            private readonly IDateTimeProvider _provider;

            public CustomConsoleLogger(string categoryName, IDateTimeProvider provider)
            {
                _categoryName = categoryName;
                _provider = provider;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                Console.WriteLine($"{_provider.Now.ToString("yyyy-MM-dd HH-mm-ss")} {logLevel}: {_categoryName}[{eventId.Id}]: {formatter(state, exception)}");
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}
