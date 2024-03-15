using Market.DTO;
using Market.Models;

namespace Market.Repositories.ProductRepo
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<Product?> AddProductAsync(ProductDto productDto);
        Task<Product?> DeleteProductAsync(Guid? productId);
    }
}
