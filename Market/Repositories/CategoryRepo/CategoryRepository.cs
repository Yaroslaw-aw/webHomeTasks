using AutoMapper;
using Market.DTO;
using Market.Exceptions;
using Market.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;

namespace Market.Repositories.CategoryRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MarketContext context;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        public CategoryRepository(MarketContext context, IMapper mapper, IMemoryCache cache)
        {
            this.context = context;
            this.mapper = mapper;
            this.cache = cache;
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
                    Category? existingCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == categoryDto.Name);
                    if (existingCategory == null)
                    {
                        Category newCategory = mapper.Map<Category>(categoryDto);

                        await context.AddAsync(newCategory);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        newCategoryId = newCategory.Id;
                        cache.Remove("categories");
                    }
                    else
                    {
                        // Выбрасываем специфичное исключение, сообщающее о дубликате категории
                        throw new DuplicateCategoryException($"Category {categoryDto.Name} already exists. {existingCategory.Id}");
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
            if (cache.TryGetValue("categories", out List<CategoryDto>? categories) && categories != null) return categories;

            List<Category> ategories = await context.Set<Category>().AsNoTracking().ToListAsync();

            IEnumerable<CategoryDto> result = mapper.Map<IEnumerable<CategoryDto>>(ategories);

            cache.Set("categories", categories, TimeSpan.FromMinutes(30));

            return result;
        }


        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<Guid?> DeleteCategoryAsync(Guid? categoryId)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Category? deletedCategory = await context.Categories.FindAsync(categoryId);
                if (deletedCategory != null)
                {
                    // Находим все продукты, связанные с этой категорией
                    List<Product> productsWithCategory = await context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();

                    // Обновляем связанные продукты, устанавливая их категорию в null
                    foreach (var product in productsWithCategory)
                    {
                        product.CategoryId = null;
                    }

                    // Удаляем категорию
                    context.Categories.Remove(deletedCategory);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return deletedCategory.Id;
                }
                else
                {
                    throw new ArgumentException("Category not found.", nameof(categoryId));
                }
            }
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
