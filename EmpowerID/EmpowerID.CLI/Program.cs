
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EmpowerID.Domain.Settings;
using EmpowerID.Logging;
using EmpowerID.CLI;
using EmpowerID.Infrastructure;
using Microsoft.EntityFrameworkCore;
using EmpowerID.Application.Services.ProductService;
using EmpowerID.Domain.Entities;
using EmpowerID.Domain.Repositories;
using EmpowerID.Infrastructure.Repositories;
using EmpowerID.Domain.DomainServices;
using EmpowerID.Infrastructure.DomainServices;
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
    services.Configure<SearchClientSettings>(config.GetSection(nameof(SearchClientSettings)));
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
    services.AddScoped<IProductService, ProductService>();
    services.AddScoped<IProductSearchService, ProductSearchService>();
    services.AddScoped<IProductRepository, ProductRepository>();
    // Add Worker class as a singleton service
    services.AddSingleton<MainWorker>();
}).Build();

// Create a scope to resolve the Worker service
using (var serviceScope = host.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    try
    {
        // Resolve and run the Worker service
        var worker = services.GetRequiredService<MainWorker>();
        await worker.MainAsync(args);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred.");
    }
}

await host.RunAsync();