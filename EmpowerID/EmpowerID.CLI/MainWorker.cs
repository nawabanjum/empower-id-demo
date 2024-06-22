using EmpowerID.Application.RequestModels;
using EmpowerID.Application.Services.ProductService;
using EmpowerID.Domain.DomainServices;
using EmpowerID.Domain.Settings;
using EmpowerID.Infrastructure.DomainServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EmpowerID.CLI
{
    public class MainWorker
    {
        private readonly ILogger<MainWorker> _logger;
        private readonly IOptions<AppSettings> _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IProductService _productService;
        private readonly IProductSearchService _productSearchService;
        private readonly EtlService _etlService;

        public MainWorker(ILogger<MainWorker> logger, IOptions<AppSettings> options, IServiceProvider serviceProvider, IProductService productService,
            IProductSearchService productSearchService, EtlService etlService)
        {
            _logger = logger;
            _options = options;
            _serviceProvider = serviceProvider;
            _productService = productService;
            _productSearchService = productSearchService;
            _etlService = etlService;
        }

        public async Task MainAsync(string[] args)
        {
            _logger.LogInformation("Main worker running at {time}", DateTime.UtcNow);
            bool isExist = false;

            while (!isExist)
            {
                try
                {
                    _logger.LogDebug("Getting User Input");
                    var input = ReadIntegerInput("Enter 1 for Product Search \nEnter 2 to Run ETL pipeline\nEnter 0 to exist app: ");

                    switch (input)
                    {
                        case 0:
                            _logger.LogDebug("User selected to existed the app.");
                            isExist = true; break;
                        case 1:
                            _logger.LogDebug("User selected to search products");
                            await SearchProductsAsync();
                            break;
                        case 2:
                            _logger.LogDebug("User selected to Run ETL pipeline");
                            await _etlService.RunPipelineAsync();
                            break;

                        default:
                            await Console.Out.WriteLineAsync("Invalid Option Selected");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error has occured");
                }
            }

        }

        private async Task SearchProductsAsync()
        {
            await Console.Out.WriteLineAsync("Please enter search filter(s)");
            ProductSearchRequest request = new()
            {
                SearchText = ReadStringInput("Search Text: "),
                CategoryId = ReadIntegerInput("Category Id: ", true),
                DateAddedAtStart = ReadDateInput("Added At Start: (yyyy-MM-dd)", true),
                DateAddedAtEnd = ReadDateInput("Added At End: (yyyy-MM-dd)", true),
                MaxPrice = ReadDecimalInput("Max Price: ", true),
                MinPrice = ReadDecimalInput("Min Price: ", true),

            };

            while (true)
            {

                _logger.LogDebug("Loading page: {pageNumber}", request.PageNumber);
                //Search from database
                //var results = await _productService.SearchProductsAsync(request);

                //Search from azure
                (var results, long? total) = await _productSearchService.SearchAsync(request.SearchText, request.CategoryId, request.MinPrice,
                    request.MaxPrice, request.DateAddedAtStart, request.DateAddedAtEnd, request.PageNumber, request.PageSize);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(results, options);
                Console.WriteLine(jsonString);

                if (total.HasValue)
                    _logger.LogDebug("Total Records: {total}", total);

                var next = ReadIntegerInput("Enter 1 for next page\nEnter 0 to exit");
                if (next == 1 && results != null && results.Count == request.PageSize)
                {
                    request.PageSize++;
                    continue;
                }

                _logger.LogDebug("No More data is available ");
                break;
            }

        }

        private static string? ReadStringInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        static int? ReadIntegerInput(string prompt, bool isOptional = false)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (int.TryParse(input, out result))
                {
                    break;
                }
                else if (isOptional)
                {
                    return null;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
            }
            return result;
        }

        static decimal? ReadDecimalInput(string prompt, bool isOptional = false)
        {
            decimal result;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (decimal.TryParse(input, out result))
                {
                    break;
                }
                else if (isOptional)
                {
                    return null;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
            }
            return result;
        }


        static DateTime? ReadDateInput(string prompt, bool isOptional = false)
        {
            DateTime result;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (DateTime.TryParse(input, out result))
                {
                    break;
                }
                else if (isOptional)
                {
                    return null;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid date in the format yyyy-MM-dd.");
                }
            }
            return result;
        }
    }
}
