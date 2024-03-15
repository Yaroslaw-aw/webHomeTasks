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

        [HttpGet(template: "GetProduct")]
        public async Task<ActionResult<IEnumerable<ProductDto>?>> GetProducts()
        {
            IEnumerable<ProductDto>? products = await repository.GetProductsAsync();
            return AcceptedAtAction("GetProduct", products);
        }

        [HttpPost(template: "AddProduct")]
        public async Task<ActionResult<Guid?>> AddProduct([FromQuery] ProductDto productDto)
        {
            Product? newProductId = await repository.AddProductAsync(productDto);
            return CreatedAtAction("AddProduct", newProductId?.Id);
        }

        //[HttpPost(template: "AddProduct")]
        //public async Task<ActionResult<Guid?>> AddProduct([FromQuery] string? name, string? description, decimal? price, CategoryDto categoryDto)
        //{
        //    Guid? newProductId = await repository.AddProductAsync(name, description, price, categoryDto);
        //    return CreatedAtAction("AddProduct", newProductId);
        //}
    }
}
