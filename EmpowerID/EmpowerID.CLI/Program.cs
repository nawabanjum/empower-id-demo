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
using EmpowerID.Domain.Repositories;
using EmpowerID.Infrastructure.Repositories;
using EmpowerID.Domain.DomainServices;
using EmpowerID.Infrastructure.DomainServices;
using Azure.Identity;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Rest.Azure.Authentication;

IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

var con = config.GetConnectionString("DefaultConnection") ??
    throw new Exception("Connection string missing");

IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddDbContext<EmpowerIdDbContext>(options =>
                options.UseSqlServer(
                    con
                ));

    services.Configure<AppSettings>(config.GetSection("AppSettings"));
    services.Configure<SearchClientSettings>(config.GetSection(nameof(SearchClientSettings)));
    services.Configure<EtlServiceSettings>(config.GetSection(nameof(EtlServiceSettings)));
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    services.AddSingleton<EtlService>();
    services.AddSingleton<DataFactoryManagementClient>(provider =>
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        var subscriptionId = configuration["EtlServiceSettings:SubscriptionId"];
        var clientId = configuration["EtlServiceSettings:ClientId"];
        var clientSecret = configuration["EtlServiceSettings:ClientSecret"];
        var tenantId = configuration["EtlServiceSettings:TenantId"];
        var credentials = ApplicationTokenProvider.LoginSilentAsync(
                           tenantId, clientId, clientSecret).Result;
        return new DataFactoryManagementClient(credentials)
        {
            SubscriptionId = subscriptionId
        };
    });

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

    System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

    void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = e.ExceptionObject as Exception;
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, exception?.Message);
    }
    // Resolve and run the Worker service
    var worker = services.GetRequiredService<MainWorker>();
    await worker.MainAsync(args);
}

await host.RunAsync();