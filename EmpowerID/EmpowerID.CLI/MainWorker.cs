using EmpowerID.Application.RequestModels;
using EmpowerID.Application.Services.ProductService;
using EmpowerID.Domain.Entities;
using EmpowerID.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmpowerID.CLI
{
    public class MainWorker
    {
        private readonly ILogger<MainWorker> _logger;
        private readonly IOptions<AppSettings> _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IProductService _productService;

        public MainWorker(ILogger<MainWorker> logger, IOptions<AppSettings> options, IServiceProvider serviceProvider, IProductService productService)
        {
            _logger = logger;
            _options = options;
            _serviceProvider = serviceProvider;
            _productService = productService;
        }

        public async Task MainAsync(string[] args)
        {

            _logger.LogInformation($"Main worker running at {DateTimeOffset.UtcNow}");
            bool isExist = false;

            while (!isExist)
            {
                await Console.Out.WriteLineAsync("Enter 1 for Product Search ");
                await Console.Out.WriteLineAsync("Enter 0 to exist app");

                var input = ReadIntegerInput("Enter 1 for Product Search \nEnter 0 to exist app");

                switch (input)
                {
                    case 0:
                        isExist = true; break;
                    case 1:
                        await SearchProductsAsync();
                        break;

                    default:
                        await Console.Out.WriteLineAsync("Invalid Option");
                        break;
                }

            }

        }

        private async Task SearchProductsAsync()
        {
            await Console.Out.WriteLineAsync("Please enter search filter(s)");
            ProductSearchRequest request = new ProductSearchRequest
            {
                ProductName = ReadStringInput("Name: "),
                Description = ReadStringInput("Description: "),
                CategoryId = ReadIntegerInput("Category Id: ", true),
                DateAddedAtStart = ReadDateInput("Added At Start: (yyyy-MM-dd)", true),
                DateAddedAtEnd = ReadDateInput("Added At End: (yyyy-MM-dd)", true),
                MaxPrice = ReadDecimalInput("Max Price: ", true),
                MinPrice = ReadDecimalInput("Min Price: ", true),

            };
            while(true)
            {
                var results = await _productService.SearchProductsAsync(request);
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(results, options);
                Console.WriteLine(jsonString);
            }

        }

        private string ReadStringInput(string prompt)
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
                string input = Console.ReadLine();
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
                string input = Console.ReadLine();
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
                string input = Console.ReadLine();
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
