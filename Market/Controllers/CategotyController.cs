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

        [HttpGet(template: "GetCategorys")]
        public async Task<ActionResult<IEnumerable<CategoryDto>?>> GetCategorys()
        {
            IEnumerable<CategoryDto>? Categorys = await repository.GetCategoriesAsync();
            return AcceptedAtAction("GetCategorys", Categorys);
        }

        [HttpPost(template: "AddCategory")]
        public async Task<ActionResult<Guid?>> AddCategory([FromQuery] CategoryDto CategoryDto)
        {
            Category? newCategory = await repository.AddCategoryAsync(CategoryDto);
            return CreatedAtAction("AddCategory", newCategory?.Id);
        }


        [HttpDelete(template: "DeleteCategory")]
        public async Task<ActionResult<Guid?>> DeleteCategory(Guid? CategoryId)
        {
            Category? deletetCategory = await repository.DeleteCategoryAsync(CategoryId);
            return AcceptedAtAction(nameof(DeleteCategory), deletetCategory?.Id);
        }
    }
}
        //new { id = newCategory?.Id, name = newCategory?.Name, description = newCategory?.Description }
