using Market.DTO;
using Market.Models;

namespace Market.Repositories.ProductRepo
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<Guid?> AddProductAsync(ProductDto productDto);
        Task<Product?> DeleteProductAsync(Guid? productId);
        //Task<Guid?> UpdateProductAsync(ProductDto productDto);
    }
}
