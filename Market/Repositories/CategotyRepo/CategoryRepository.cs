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

        public async Task<Category?> AddCategotyAsync(CategoryDto categoryDto)
        {
            Guid? newCategoryId = null;
            Category? newCategory = default;
            using (context)
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    newCategory = mapper.Map<Category>(categoryDto);
                    newCategoryId = newCategory.Id;
                    context.Add(newCategory);
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
            return newCategory;
        }
    }
}
