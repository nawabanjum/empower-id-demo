
using EmpowerID.Domain.Entities;

namespace EmpowerID.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> SearchProductsAsync(string? productName, int? categoryId, decimal? minPrice, decimal? maxPrice, string? description, DateTime? addedAtStart, DateTime? addedAtEnd, int pageSize, int pageNumber);
    }
}
