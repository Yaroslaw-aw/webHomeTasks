using AutoMapper;
using Market.DTO;
using Market.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Market.Repositories.CategoryRepo
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
        public async Task<Guid?> AddCategoryAsync(CategoryDto categoryDto)
        {
            Guid? newCategoryId = null;
            try
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    bool isCategoryExists = await context.Categories.AnyAsync(c => c.Name == categoryDto.Name);
                    if (isCategoryExists == false)
                    {
                        Category newCategory = mapper.Map<Category>(categoryDto);

                        await context.AddAsync(newCategory);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        newCategoryId = newCategory.Id;
                    }
                    else
                    {
                        // выбросить какое-нибудь исключение, сообщающее о попытке добавить дубликат?
                    }
                }
            }
            catch
            (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return newCategoryId;
        }

        /// <summary>
        /// Получение категорий
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            List<Category> ategories = await context.Set<Category>().AsNoTracking().ToListAsync();
            IEnumerable<CategoryDto> result = mapper.Map<IEnumerable<CategoryDto>>(ategories);
            return result;
        }


        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Guid?> DeleteCategoryAsync(Guid? id)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Category? deletedCategory = context.Categories.FirstOrDefault(c => c.Id == id);
                if (deletedCategory != null)
                {
                    context.Categories.Remove(deletedCategory);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return deletedCategory.Id;
                }
            }
            return null;
        }

        public async Task<Guid?> UpdateCategotyAsync(Guid categoryId, CategoryDto categoryDto)
        {
            using (IDbContextTransaction tx = context.Database.BeginTransaction())
            {
                Category? category = await context.Categories.FirstOrDefaultAsync(p => p.Id == categoryId);

                if (category != null)
                {
                    mapper.Map(categoryDto, category);
                    await context.SaveChangesAsync();
                    tx.Commit();
                    return categoryId;
                }
                return null;
            }
        }
    }
}
