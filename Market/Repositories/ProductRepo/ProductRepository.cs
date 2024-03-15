using AutoMapper;
using Market.DTO;
using Market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<Product> products = await context.Set<Product>().AsNoTracking().ToListAsync();
            IEnumerable<ProductDto> result = mapper.Map<IEnumerable<ProductDto>>(products);
            return result;
        }

        /// <summary>
        /// Добавление продукта
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public async Task<Guid?> AddProductAsync([FromQuery] ProductDto productDto)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Product? newProduct = mapper.Map<Product>(productDto);
                    context.Products.Add(newProduct);
                    await context.SaveChangesAsync();

                    var storageProduct = await context.ProductStorages.FirstOrDefaultAsync(sp => sp.ProductId == newProduct.Id);

                    if (storageProduct != null)
                    {
                        storageProduct.Count += newProduct.Count;
                        storageProduct.ProductId = newProduct.Id;
                        storageProduct.CategoryId = newProduct.CategoryId;
                    }
                    else
                    {
                        storageProduct = new ProductStorage
                        {
                            ProductId = newProduct.Id,
                            Count = newProduct.Count,
                            CategoryId = newProduct.CategoryId,
                            
                        };
                        context.ProductStorages.Add(storageProduct);
                    }

                    var categoryProduct = new CategoryProduct
                    {
                        CategoryId = newProduct.CategoryId,
                        ProductId = newProduct.Id
                    };
                    context.CategoryProducts.Add(categoryProduct);


                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return newProduct.Id;
                }
                catch
                { 
                    await transaction.RollbackAsync(); throw;
                }
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

        //public async Task<Guid?> UpdateProductAsync(ProductDto productDto)
        //{
        //    using (IDbContextTransaction tx = context.Database.BeginTransaction())
        //    {
        //        Product product = await context.Set<Product>().AsNoTracking().;
        //    }
        //}
    }
}
