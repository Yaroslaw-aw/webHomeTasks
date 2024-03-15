using AutoMapper;
using Market.DTO;
using Market.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Market.Repositories.CategotyRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        private MarketContext context;
        private IMapper mapper;

        public CategoryRepository(MarketContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Добавление категории
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        public async Task<Category?> AddCategoryAsync(CategoryDto categoryDto)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Category newCategory = mapper.Map<Category>(categoryDto);
                await context.AddAsync(newCategory);
                context.SaveChanges();
                transaction.Commit();
                return newCategory;
            }
        }

        /// <summary>
        /// Получение категорий
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            await context.SaveChangesAsync();
            using (context)
                return context.Categories.Select(mapper.Map<CategoryDto>).ToList();
        }

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category?> DeleteCategoryAsync(Guid? id)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Category? deletedCategory = context.Categories.FirstOrDefault(c => c.Id == id);
                if (deletedCategory != null)
                {
                    context.Categories.Remove(deletedCategory);
                    await context.SaveChangesAsync();
                    transaction.Commit();
                    return deletedCategory;
                }
            }
            return null;
        }


    }
}
