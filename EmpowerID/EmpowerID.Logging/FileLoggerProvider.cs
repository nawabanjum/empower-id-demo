using EmpowerID.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmpowerID.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        public readonly AppSettings myConfig;

        public FileLoggerProvider(IOptions<AppSettings> _config)
        {
            myConfig = _config.Value;
            if (!Directory.Exists(myConfig.LogFolder))
            {
                Directory.CreateDirectory(myConfig.LogFolder);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
