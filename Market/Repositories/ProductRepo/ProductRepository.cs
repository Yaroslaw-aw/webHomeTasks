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

        // List<ProductDto> products = context.Products.Select(product => new Product()
        // {
        //      Id = product.Id,
        //      Name = product.Name,
        //      Description = product.Description
        // }).ToList(); // это комментарий для себя

        /// <summary>
        /// Получение списка продуктов из базы данных
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            using (context)
            {
                return context.Products.Select(mapper.Map<ProductDto>).ToList();
            }
        }

        public async Task<Product?> AddProductAsync([FromQuery] ProductDto productDto)
        {
            //ProductDto productDto = mapper.Map<Product>(productDto) };
            // ProductDto productDto
            Guid? newProductId = default;
            Product? newProduct = default;
            using (context)
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    newProduct = mapper.Map<Product>(productDto);
                    newProductId = newProduct.Id;
                    context.Set<Product>().Add(newProduct);
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
            return newProduct;
        }

        //public async Task<Guid?> AddProductAsync([FromQuery] string? name, string? description, decimal? price, CategoryDto categoryDto)
        //{
        //    ProductDto productDto = new ProductDto() { Name = name, Description = description, Price = price, Category = mapper.Map<Category>(categoryDto) };
        //    // ProductDto productDto
        //    Guid? newProductId = null;
        //    using (context)
        //    {
        //        using (IDbContextTransaction transaction = context.Database.BeginTransaction())
        //        {
        //            Product product = mapper.Map<Product>(productDto);
        //            newProductId = product.Id;
        //            context.Set<Product>().Add(product);
        //            context.SaveChanges();
        //            transaction.Commit();
        //        }
        //    }
        //    return newProductId;
        //}
    }
}
