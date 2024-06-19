
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EmpowerID.Domain.Settings;
using EmpowerID.Logging;
using EmpowerID.CLI;
using EmpowerID.Infrastructure;
using Microsoft.EntityFrameworkCore;
var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
// Ask the service provider for the configuration abstraction.
IConfiguration config = builder;


var con = config.GetConnectionString("DefaultConnection");
if (con == null)
{
    throw new Exception("Connection string missing");
}
IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddDbContext<EmpowerIdDbContext>(options =>
                options.UseSqlServer(
                    con
                ));
    services.Configure<AppSettings>(config.GetSection("AppSettings"));
    services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
}).Build();
