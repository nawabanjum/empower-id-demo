using EmpowerID.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpowerID.CLI
{
    public  class MainWorker
    {
        private readonly ILogger<MainWorker> _logger;
        private readonly IOptions<AppSettings> _options;
        private readonly IServiceProvider _serviceProvider;
        public MainWorker(ILogger<MainWorker> logger, IOptions<AppSettings> options, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _options = options;
            _serviceProvider = serviceProvider;
        }
       
        public  void Main(string[] args) {
            _logger.LogWarning($"Main worker running at {DateTimeOffset.UtcNow}");
        }

       
    }
}
