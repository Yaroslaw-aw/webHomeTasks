using Market.DTO;
using Market.Models;

namespace Market.Repositories.CategoryRepo
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<Guid?> AddCategoryAsync(CategoryDto categoryDto);
        Task<Guid?> DeleteCategoryAsync(Guid? categoryId);
        Task<Guid?> UpdateCategotyAsync(Guid categoryId, CategoryDto categoryDto);

    }
}
