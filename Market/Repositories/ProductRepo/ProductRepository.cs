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
        public async Task<Guid?> AddProductAsync(ProductDto productDto)
        {
            Guid? newProductId = null;
            ProductStorage? storageProduct = null;
            try
            {
                Product? existingProduct = await context.Products.FirstOrDefaultAsync(sp => sp.Name == productDto.Name &&
                                                                                            sp.Description == productDto.Description);
                
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    if (existingProduct != null)
                    {
                        storageProduct = await context.ProductStorages.FirstOrDefaultAsync(ps => ps.StorageId == productDto.StorageId);

                        if(storageProduct != null && storageProduct.ProductId == existingProduct.Id && productDto.Price == storageProduct.Price)
                        {
                            storageProduct.Count += productDto.Count;
                        }
                        else
                        {
                            storageProduct = mapper.Map<ProductStorage>(productDto);
                            storageProduct.ProductId = existingProduct.Id;
                            context.ProductStorages.Add(storageProduct);                            
                        }
                        await context.SaveChangesAsync();
                        newProductId = existingProduct.Id;
                    }
                    else
                    {
                        Product? newProduct = mapper.Map<Product>(productDto);
                        context.Products.Add(newProduct);
                        await context.SaveChangesAsync();
                        newProductId = newProduct.Id;

                        CategoryProduct? categoryProduct = mapper.Map<CategoryProduct>(newProduct);
                        context.CategoryProducts.Add(categoryProduct);
                        await context.SaveChangesAsync();

                        storageProduct = mapper.Map<ProductStorage>(productDto);
                        storageProduct.ProductId = newProduct.Id;
                        context.ProductStorages.Add(storageProduct);                        
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                // Обрабатываем ошибку сохранения в базе данных
                throw new Exception("Failed to save product to database.", ex);
            }
            catch (Exception ex)
            {
                // Обрабатываем другие исключения
                throw new Exception("An error occurred while adding the product.", ex);
            }
            return newProductId;
        }
        /// <summary>
        /// Удаление продукта
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Product?> DeleteProductAsync(Guid? productId)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Product? deletedProduct = await context.Products
                    .Include(p => p.ProductStorages)
                    .Include(p => p.CategoryProducts)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (deletedProduct != null)
                {
                    // Удаляем связанные записи из таблицы CategoryProducts
                    context.CategoryProducts.RemoveRange(deletedProduct.CategoryProducts);
                    await context.SaveChangesAsync();

                    // Получаем связанные записи из таблицы ProductStorages
                    var relatedProductStorages = deletedProduct.ProductStorages.ToList();

                    // Удаляем связанные записи из таблицы ProductStorages
                    foreach (var productStorage in relatedProductStorages)
                    {
                        context.ProductStorages.Remove(productStorage);
                    }
                    await context.SaveChangesAsync();

                    // Удаляем сам продукт
                    context.Products.Remove(deletedProduct);
                    await context.SaveChangesAsync();

                    // Фиксируем транзакцию
                    await transaction.CommitAsync();

                    return deletedProduct;
                }

                return null;
            }
        }

        public async Task<Guid?> UpdateProductAsync(Guid? productId, ProductDto productDto)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Product? deletedProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (deletedProduct != null)
                {
                    // Поиск объекта хранения продукта
                    ProductStorage? productStorage = await context.ProductStorages.FirstOrDefaultAsync(ps => ps.ProductId == deletedProduct.Id);

                    if (productStorage != null)
                    {
                        // Удаление объекта хранения продукта
                        context.ProductStorages.Remove(productStorage);
                    }

                    // Удаление продукта
                    context.Products.Remove(deletedProduct);

                    // Сохранение изменений
                    await context.SaveChangesAsync();

                    // Фиксация транзакции
                    await transaction.CommitAsync();

                    return deletedProduct.Id;
                }
            }

            return null;
        }
    }
}

