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
        /// Получение всех продуктов
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "GetProducts")]
        public async Task<ActionResult<IEnumerable<GetAllProductsDto>?>> GetProducts()
        {
            if (redis.TryGetValue("allproducts", out List<GetAllProductsDto>? productsRedis) && productsRedis != null) return productsRedis;
            IEnumerable<GetAllProductsDto>? products = await repository.GetProductsAsync();
            if (products == null) return NotFound();
            redis.SetData("allproducts", products);
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
            redis.cache.Remove("products");
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
            redis.cache.Remove("products");
            return AcceptedAtAction(nameof(UpdateProduct), productid);
        }

        [HttpGet(template: "ExistsProduct")]
        public async Task<ActionResult<bool>> ExistsProduct(Guid productId)
        {
            return AcceptedAtAction(nameof(ExistsProduct), await repository.ProductExistsAsync(productId));
        }

        /// <summary>
        /// Формирование CSV-файла
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private string GetCsv(IEnumerable<GetAllProductsDto>? products)
        {
            StringBuilder sb = new StringBuilder();

            if (products != null)
                foreach (var product in products)
                {
                    sb.AppendLine(product.Name + ";" + product.Price + ";" + product.Count + ";" + product.Description + ";" + product.StorageName);
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
            // пробуем достать список продуктов из редиса
            if (redis.TryGetValue("allproducts", out IEnumerable<GetAllProductsDto>? products)) content = GetCsv(products);
            else
            {   // достаём продукты из репозитория
                products = await repository.GetProductsAsync();
                redis.SetData("products", products); // кэшируем в редис
                // формируем строку для CSV-файла
                content = GetCsv(products);
            }
            // генерируем название файла
            string? fileName = "products" + DateTime.Now.ToBinary().ToString() + ".csv";
            // записываем в папку StaticFiles CSV-файл, который формируем из подготовленной строки
            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), content);
            // возвращаем строку для скачивани файла, в которой добавлен пусть static, прописанный в файле Program.cs
            return "https://" + Request.Host.ToString() + "/static/" + fileName;
        }


        [HttpPut(template: "AddCategory")]
        public async Task<ActionResult<Guid?>> AddCategory(AddCategoryToProductDto toProductDto)
        {
            Guid? updatedProductId = await repository.AddCategoryAsync(toProductDto);
            return AcceptedAtAction(nameof(AddCategory), updatedProductId);
        }
    }
}