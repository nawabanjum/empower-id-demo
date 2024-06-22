using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using EmpowerID.Domain.DomainModels;
using EmpowerID.Domain.DomainServices;
using EmpowerID.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmpowerID.Infrastructure.DomainServices
{
    public class ProductSearchService : IProductSearchService
    {
        private readonly SearchClient _searchClient;
        private readonly ILogger<ProductSearchService> _logger;

        public ProductSearchService(IOptions<SearchClientSettings> searchSettings, ILogger<ProductSearchService> logger)
        {
            _logger = logger;
            var settings = searchSettings.Value;
            var credential = new AzureKeyCredential(settings.SearchServiceQueryApiKey);
            var serviceEndpoint = new Uri(settings.SearchServiceUri);

            _searchClient = new SearchClient(serviceEndpoint, settings.SearchIndex, credential);
        }

        public async Task<(List<ProductSearchModel>, long?)> SearchAsync(string? searchText, int? categoryId, decimal? minPrice, decimal? maxPrice, DateTime? dateStart, DateTime? dateEnd, int pageNumber, int pageSize)
        {
            _logger.LogDebug("Searching Products from Azure AI Search");
            var options = new SearchOptions
            {
                IncludeTotalCount = true,
                Skip = (pageNumber - 1) * pageSize,
                Size = pageSize
            };

            var filterExpressions = new List<string>();

            if (categoryId.HasValue)
                filterExpressions.Add($"category_id eq {categoryId.Value}");

            if (minPrice.HasValue)
                filterExpressions.Add($"Price ge {minPrice.Value}");

            if (maxPrice.HasValue)
                filterExpressions.Add($"Price le {maxPrice.Value}");

            if (dateStart.HasValue)
                filterExpressions.Add($"DateAdded ge {dateStart.Value:O}");

            if (dateEnd.HasValue)
                filterExpressions.Add($"DateAdded le {dateEnd.Value:O}");

            if (filterExpressions.Count > 0)
                options.Filter = string.Join(" and ", filterExpressions);

            _logger.LogDebug("Search Query: {searchText}\nFilters : {filters}",searchText,options.Filter);
            var results = await _searchClient.SearchAsync<ProductSearchModel>(searchText, options);
            var products = new List<ProductSearchModel>();

            await foreach (var result in results.Value.GetResultsAsync())
            {
                products.Add(result.Document);
            }

            return (products, results.Value.TotalCount);
        }
    }
}
