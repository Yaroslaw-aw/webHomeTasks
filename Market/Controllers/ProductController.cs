using Market.DTO;
using Market.DTO.Caching;
using Market.Models;
using Market.Repositories.ProductRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository repository;
        private readonly Redis redis;

        public ProductController(IProductRepository repository, IDistributedCache redis)
        {
            this.repository = repository;
            this.redis = new Redis(redis);
        }

        /// <summary>
        /// Получение списка продуктов
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>?>> GetProducts()
        {
            if (redis.TryGetValue("products", out List<ProductDto>? productsRedis) && productsRedis != null) return productsRedis;
            IEnumerable<ProductDto>? products = await repository.GetProductsAsync();
            redis.SetData("products", products);
            return AcceptedAtAction("GetProducts", products);
        }

        /// <summary>
        /// Добавление продукта
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPost(template: "AddProduct")]
        public async Task<ActionResult<Product?>> AddProduct([FromBody] ProductDto productDto)
        {
            Guid? newProductId = await repository.AddProductAsync(productDto);
            redis.cache.Remove("products");
            return CreatedAtAction("AddProduct", newProductId);
        }

        /// <summary>
        /// Удаление продукта по Guid
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete(template: "DeleteProduct")]
        public async Task<ActionResult<Guid?>> DeleteProduct([FromBody] Guid? productId)
        {
            Product? deletetProduct = await repository.DeleteProductAsync(productId);
            return AcceptedAtAction(nameof(DeleteProduct), deletetProduct?.Id);
        }

        /// <summary>
        /// Изменение продукта
        /// </summary>
        /// <param name="updateProductDto"></param>
        /// <returns></returns>
        [HttpPut(template: "UpdateProduct")]
        public async Task<ActionResult<Guid>> UpdateProduct(UpdateProductDto updateProductDto)
        {
            Guid? productid = await repository.UpdateProductAsync(updateProductDto);
            return AcceptedAtAction(nameof(UpdateProduct), productid);
        }

        /// <summary>
        /// Формирование CSV-файла
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private string GetCsv(IEnumerable<ProductDto>? products)
        {
            StringBuilder sb = new StringBuilder();

            if (products != null)
                foreach (var product in products)
                {
                    sb.AppendLine(product.Name + ";" + product.Description + ";" + product.Price + ";" + product.StorageId);
                }

            return sb.ToString();
        }

        /// <summary>
        /// Возвращает ссылку для скачивания файла
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "GetProductsCsvUrl")]
        public async Task<ActionResult<string>> GetProductsCsvUrl()
        {
            string? content = string.Empty;

            if (redis.TryGetValue("products", out IEnumerable<ProductDto>? products)) content = GetCsv(products);
            else
            {
                products = await repository.GetProductsAsync();
                redis.SetData("products", products);
                
                content = GetCsv(products);
            }

            string? fileName = "products" + DateTime.Now.ToBinary().ToString() + ".csv";

            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), content);
            return "https://" + Request.Host.ToString() + "/static/" + fileName;
        }
    }
}