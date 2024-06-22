using EmpowerID.Domain.DomainModels;

namespace EmpowerID.Domain.DomainServices
{
    public interface IProductSearchService
    {
        Task<(List<ProductSearchModel>, long?)> SearchAsync(string? searchText, int? categoryId, decimal? minPrice, decimal? maxPrice, DateTime? dateStart, DateTime? dateEnd, int pageNumber, int pageSize);
    }
}
