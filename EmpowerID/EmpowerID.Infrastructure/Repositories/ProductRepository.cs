using EmpowerID.Domain.Entities;
using EmpowerID.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmpowerID.Infrastructure.Repositories
{
    public class ProductRepository(EmpowerIdDbContext dbContext) : IProductRepository
    {
        private readonly EmpowerIdDbContext _dbContext = dbContext;

        public async Task<List<Product>> SearchProductsAsync(string? productName, int? categoryId, decimal? minPrice, decimal? maxPrice, string? description, DateTime? addedAtStart, DateTime? addedAtEnd, int pageSize, int pageNumer)
        {
            //tested connection
            //var test = await _dbContext.Products.ToListAsync();

            var products = await _dbContext.Products
                    .FromSqlRaw("EXEC SearchProducts @productName={0}, @categoryId={1}, @minPrice= {2}, @maxPrice = {3}," +
                    "@description = {4}, @dateAddedStart= {5}, @dateAddedEnd = {6}, @pageSize = {7},@pageNumber = {8}",
                    productName, categoryId, minPrice, maxPrice, description, addedAtStart, addedAtEnd, pageSize, pageNumer)
                    .ToListAsync();

            return products;
        }
    }
}
