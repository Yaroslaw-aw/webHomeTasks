using Market.DTO;
using Market.Models;

namespace Market.Repositories.CategoryRepo
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<Guid?> AddCategoryAsync(CategoryDto categoryDto);
        //Task<Category?> UpdateCategotyAsync();
        Task<Guid?> DeleteCategoryAsync(Guid? id);

    }
}
