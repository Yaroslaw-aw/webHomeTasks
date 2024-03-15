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

        [HttpGet(template: "GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>?>> GetProducts()
        {
            IEnumerable<ProductDto>? products = await repository.GetProductsAsync();
            return AcceptedAtAction("GetProducts", products);
        }

        [HttpPost(template: "AddProduct")]
        public async Task<ActionResult<Product?>> AddProduct([FromQuery] ProductDto productDto)
        {
            Product? newProductId = await repository.AddProductAsync(productDto);
            return CreatedAtAction("AddProduct", newProductId);
        }

        //[HttpPut(template: "UpdateProduct")]
        //public async Task<ActionResult<Guid?>> UpdateProduct(ProductDto productDto)
        //{
        //    Product? newProductId = await repository.UpdateProductAsync(productDto);
        //    return AcceptedAtAction("UpdateProduct", newProductId?.Id);
        //}

        [HttpDelete(template: "DeleteProduct")]
        public async Task<ActionResult<Guid?>> DeleteProduct(Guid? productId)
        {
            Product? deletetProduct = await repository.DeleteProductAsync(productId);
            return AcceptedAtAction(nameof(DeleteProduct), deletetProduct?.Id);
        }

        //[HttpPost(template: "AddProduct")]
        //public async Task<ActionResult<Guid?>> AddProduct([FromQuery] string? name, string? description, decimal? price, CategoryDto categoryDto)
        //{
        //    Guid? newProductId = await repository.AddProductAsync(name, description, price, categoryDto);
        //    return CreatedAtAction("AddProduct", newProductId);
        //}
    }
}
