using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace SalasUfsWeb.Logging
{
    public sealed class TextFileLoggerProvider : ILoggerProvider
    {
        private readonly object lockObject = new object();
        private readonly StreamWriter writer;

        public TextFileLoggerProvider()
        {
            try
            {
                var logsDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
                Directory.CreateDirectory(logsDirectory);

                var fileName = $"logs-{DateTime.Now:yyyyMMdd-HHmmss}.txt";
                var filePath = Path.Combine(logsDirectory, fileName);

                writer = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    AutoFlush = true
                };
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Nao foi possivel iniciar o logger em arquivo.");
                Console.Error.WriteLine(exception);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TextFileLogger(categoryName, writer, lockObject);
        }

        public void Dispose()
        {
            writer?.Dispose();
        }
    }

    public sealed class TextFileLogger : ILogger
    {
        private readonly string categoryName;
        private readonly StreamWriter writer;
        private readonly object lockObject;

        public TextFileLogger(string categoryName, StreamWriter writer, object lockObject)
        {
            this.categoryName = categoryName;
            this.writer = writer;
            this.lockObject = lockObject;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return writer != null && logLevel != LogLevel.None;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel) || formatter == null)
            {
                return;
            }

            var message = formatter(state, exception);

            if (string.IsNullOrWhiteSpace(message) && exception == null)
            {
                return;
            }

            lock (lockObject)
            {
                writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{logLevel}] [{categoryName}] {message}");

                if (exception != null)
                {
                    writer.WriteLine(exception);
                }
            }
        }
    }
}
