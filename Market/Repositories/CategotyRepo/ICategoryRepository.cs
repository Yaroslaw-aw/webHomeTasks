using Market.DTO;
using Market.Models;

namespace Market.Repositories.CategotyRepo
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<Category?> AddCategoryAsync(CategoryDto categoryDto);
        //Task<Category?> UpdateCategotyAsync();
        Task<Category?> DeleteCategoryAsync(Guid? id);

    }
}
