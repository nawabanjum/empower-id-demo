using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace EmpowerID.Logging
{
    public class FileLogger([NotNull] FileLoggerProvider fileLoggerProvider) : ILogger
    {
        private readonly FileLoggerProvider _fileLoggerProvider = fileLoggerProvider ?? throw new ArgumentNullException(nameof(fileLoggerProvider));

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        private readonly Mutex _mutex = new(false);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var fullPath = _fileLoggerProvider.myConfig.LogFolder + "/" + _fileLoggerProvider.myConfig.LogFilePath.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var logRecord = string.Format("{0} [{1}] {2} {3}", "[" + DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "]", logLevel.ToString(), formatter(state, exception), exception != null ? exception.ToString() : "");
            WriteInFile(logRecord, fullPath);
        }

        private void WriteInFile(string text, string path)
        {
            _mutex.WaitOne();
            try
            {
                using StreamWriter stream = File.AppendText(path);
                stream.WriteLine(text);
                stream.Close();

            }
            finally
            {
                _mutex.ReleaseMutex();
            }

        }
    }
}
