using Market.DTO;
using Market.Models;

namespace Market.Repositories.ProductRepo
{
    public interface IProductRepository
    {
        Task<IEnumerable<GetAllProductsDto>> GetProductsAsync();
        Task<Guid?> AddProductAsync(ProductDto productDto);
        Task<Product?> DeleteProductAsync(Guid? productId);
        Task<Guid?> UpdateProductAsync(UpdateProductDto updateProductDto);
        Task<Guid?> AddCategoryAsync(AddCategoryToProductDto toProductDto);
        Task<bool> ProductExistsAsync(Guid? productId);
    }
}
