using Market.DTO;
using Market.Models;
using Market.Repositories.ProductRepo;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Получение списка продуктов
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>?>> GetProducts()
        {
            IEnumerable<ProductDto>? products = await repository.GetProductsAsync();
            return AcceptedAtAction("GetProducts", products);
        }

        /// <summary>
        /// Добавление продукта
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPost(template: "AddProduct")]
        public async Task<ActionResult<Product?>> AddProduct([FromQuery] ProductDto productDto)
        {
            Product? newProductId = await repository.AddProductAsync(productDto);
            return CreatedAtAction("AddProduct", newProductId);
        }

        /// <summary>
        /// Удаление продукта по Guid
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete(template: "DeleteProduct")]
        public async Task<ActionResult<Guid?>> DeleteProduct(Guid? productId)
        {
            Product? deletetProduct = await repository.DeleteProductAsync(productId);
            return AcceptedAtAction(nameof(DeleteProduct), deletetProduct?.Id);
        }
    }
}
