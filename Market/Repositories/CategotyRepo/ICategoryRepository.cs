using Market.DTO;
using Market.Models;

namespace Market.Repositories.CategotyRepo
{
    public interface ICategoryRepository
    {
        Task<Category?> AddCategotyAsync(CategoryDto categoryDto);
    }
}
