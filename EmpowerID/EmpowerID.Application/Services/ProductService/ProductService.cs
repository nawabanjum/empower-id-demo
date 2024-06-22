using AutoMapper;
using EmpowerID.Application.Dtos;
using EmpowerID.Application.RequestModels;
using EmpowerID.Domain.Repositories;

namespace EmpowerID.Application.Services.ProductService
{
    public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<ProductDto>> SearchProductsAsync(ProductSearchRequest request)
        {
            var products = await _productRepository.SearchProductsAsync(request.SearchText, request.CategoryId, request.MinPrice, request.MaxPrice, request.SearchText, request.DateAddedAtStart, request.DateAddedAtEnd, request.PageSize, request.PageNumber);

            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
