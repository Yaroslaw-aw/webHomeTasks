using Market.DTO;
using Market.Models;
using Market.Repositories.CategotyRepo;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategotyController : Controller
    {
        private ICategoryRepository repository;

        public CategotyController(ICategoryRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost(template: "AddCategory")]
        public async Task<ActionResult<Category?>> AddCategory(CategoryDto categoryDto)
        {
            Category? newCategory = await repository.AddCategotyAsync(categoryDto);
            return CreatedAtAction("AddCategory", newCategory?.Id);
        }

        
    }
}
