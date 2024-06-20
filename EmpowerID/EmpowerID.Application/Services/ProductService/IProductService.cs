using EmpowerID.Application.Dtos;
using EmpowerID.Application.RequestModels;

namespace EmpowerID.Application.Services.ProductService
{
    public interface IProductService
    {
        Task<List<ProductDto>> SearchProductsAsync(ProductSearchRequest request);
    }
}
