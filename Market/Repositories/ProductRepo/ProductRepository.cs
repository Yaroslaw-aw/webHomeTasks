using AutoMapper;
using Market.DTO;
using Market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace Market.Repositories.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private MarketContext context;
        private IMapper mapper;

        public ProductRepository(IMapper mapper, MarketContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получение списка продуктов из базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            await context.SaveChangesAsync();
            using (context)
            {
                return context.Products.Select(mapper.Map<ProductDto>).ToList();
            }
        }

        /// <summary>
        /// Добавление продукта
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public async Task<Product?> AddProductAsync([FromQuery] ProductDto productDto)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Product? newProduct = mapper.Map<Product>(productDto);
                await context.Set<Product>().AddAsync(newProduct);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return newProduct;
            }
        }

        /// <summary>
        /// Удаление продукта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product?> DeleteProductAsync(Guid? id)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Product? deletedProduct = context.Products.FirstOrDefault(p => p.Id == id);
                if (deletedProduct != null)
                {
                    context.Products.Remove(deletedProduct);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return deletedProduct;
                }                
            }
            return null;
        }
    }
}
