using Market.DTO;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Repositories.ProductRepo
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<Product?> AddProductAsync(ProductDto productDto);
        //Task<Guid?> AddProductAsync([FromQuery] string? name, string? description, decimal? price,  CategoryDto categoryDto);
    }
}
