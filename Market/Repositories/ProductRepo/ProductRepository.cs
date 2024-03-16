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

                        if(storageProduct != null && storageProduct.ProductId == existingProduct.Id)
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
                        context.SaveChanges();
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

        public async Task<Guid?> UpdateProductAsync(Guid? productId, ProductDto productDto)
        {
            using (IDbContextTransaction tx = context.Database.BeginTransaction())
            {
                Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (product != null)
                {
                    context.Products.Remove(product);

                    product = mapper.Map<Product>(productDto);
                    context.Products.Add(product);
                    await context.SaveChangesAsync();
                    ProductStorage? productStorage = context.ProductStorages.FirstOrDefault(ps => ps.ProductId == product.Id);
                    if (productStorage != null)
                    {
                        context.ProductStorages.Remove(productStorage);
                        productStorage = mapper.Map<ProductStorage>(productDto);
                        //productStorage.Count = productDto.Count;
                        //productStorage.Name = productDto.Name;
                        //productStorage.Description = productDto.Description;
                        context.ProductStorages.Add(productStorage);
                        await context.SaveChangesAsync();
                    }

                    tx.Commit();
                    return product.Id;
                }
                return null;
            }
        }
    }
}

