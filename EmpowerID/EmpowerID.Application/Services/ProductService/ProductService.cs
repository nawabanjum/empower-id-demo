﻿using AutoMapper;
using EmpowerID.Application.Dtos;
using EmpowerID.Application.RequestModels;
using EmpowerID.Domain.Repositories;

namespace EmpowerID.Application.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> SearchProductsAsync(ProductSearchRequest request)
        {
            var products = await _productRepository.SearchProductsAsync(request.ProductName,request.CategoryId,request.MinPrice,request.MaxPrice,request.Description,request.DateAddedAtStart,request.DateAddedAtEnd,request.Take,request.Skip);

            return _mapper.Map<List<ProductDto>>(products); 
        }
    }
}
