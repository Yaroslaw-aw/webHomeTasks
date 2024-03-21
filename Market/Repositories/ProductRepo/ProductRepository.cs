using AutoMapper;
using Market.DTO;
using Market.DTO.Caching;
using Market.Models;
using Market.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Market.Repositories.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly MarketContext context;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        

        public ProductRepository(IMapper mapper, MarketContext context, IMemoryCache cache)
        {
            this.context = context;
            this.mapper = mapper;
            this.cache = cache;            
        }

        /// <summary>
        /// Получение списка продуктов из базы данных
        /// </summary>
        /// <returns></returns>        
        public async Task<IEnumerable<GetAllProductsDto>?> GetProductsAsync()
        {
            if (cache.TryGetValue("allproducts", out List<GetAllProductsDto>? productsList) && productsList != null) return productsList;

            List<ProductStorage> productStorages = await context.ProductStorages.AsNoTracking().ToListAsync();

            if (productStorages == null) return null;

            List<GetAllProductsDto> products = new List<GetAllProductsDto>(productStorages.Count);

            // Перебираем полученные склады
            foreach (var productStorage in productStorages)
            {                
                Storage? storage = await context.Storages.FirstOrDefaultAsync(s => s.Id == productStorage.StorageId);
                string? storageName = storage?.Name;                

                // Создаем DTO записи и добавляем в список
                GetAllProductsDto? productDto = new GetAllProductsDto
                {
                    Name = productStorage.Name,
                    Description = productStorage.Description,
                    Price = productStorage.Price,
                    Count = productStorage.Count,
                    StorageName = storageName,
                };
                products.Add(productDto);
            }
            cache.Set("allproducts", products, TimeSpan.FromMinutes(30));
            return products;
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
                Product? existingProduct = await context.Products.FirstOrDefaultAsync(ep => ep.Name == productDto.Name &&
                                                                                            ep.Description == productDto.Description);

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    if (existingProduct != null)
                    {
                        storageProduct = await context.ProductStorages.FirstOrDefaultAsync(ps => ps.StorageId == productDto.StorageId);

                        if (storageProduct != null && storageProduct.ProductId == existingProduct.Id && productDto.Price == storageProduct.Price)
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
                        //await context.SaveChangesAsync();
                        newProductId = newProduct.Id;

                        CategoryProduct? categoryProduct = mapper.Map<CategoryProduct>(newProduct);
                        categoryProduct.CategoryId = productDto.CategoryId;
                        context.CategoryProducts.Add(categoryProduct);

                        storageProduct = mapper.Map<ProductStorage>(productDto);
                        storageProduct.ProductId = newProduct.Id;
                        context.ProductStorages.Add(storageProduct);
                        await context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    cache.Remove("products");
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
                    List<ProductStorage> relatedProductStorages = deletedProduct.ProductStorages.ToList();

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

        /// <summary>
        /// Обновление продукта
        /// </summary>
        /// <param name="updateProductDto"></param>
        /// <returns></returns>
        public async Task<Guid?> UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Product? updatedProduct = await context.Products
                    .Include(p => p.ProductStorages)
                    .FirstOrDefaultAsync(p => p.Id == updateProductDto.productId);

                if (updatedProduct != null)
                {
                    // изменяем запись о продукте в таблице продуктов
                    mapper.Map(updateProductDto, updatedProduct);

                    // получаем список складов, на которых находится этот продукт
                    List<ProductStorage> updatedProductStorages = updatedProduct.ProductStorages.ToList();

                    
                    if (updatedProductStorages != null)
                    {
                        // на каждом складе меняем наш продукт в соответствии с введенными изменениями
                        foreach (ProductStorage? productStorage in updatedProductStorages)
                        {
                            mapper.Map(updateProductDto, productStorage);
                        }
                    }

                    // Сохранение изменений
                    await context.SaveChangesAsync();

                    // Фиксация транзакции
                    await transaction.CommitAsync();

                    return updatedProduct.Id;
                }
            }
            return null;
        }

        public async Task<Guid?> AddCategoryAsync(AddCategoryToProductDto toProductDto)
        {
            using (context)
            {
                Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == toProductDto.productId);
                if (product != null)
                {
                    CategoryProduct? newCategoryProduct = mapper.Map<CategoryProduct>(toProductDto);
                    context.CategoryProducts.Add(newCategoryProduct);
                }
                await context.SaveChangesAsync();
                return product?.Id;
            }
        }

        public async Task<bool> ProductExistsAsync(Guid? productId)
        {
            return await context.Products.AnyAsync(product => product.Id == productId);
        }
    }
}

