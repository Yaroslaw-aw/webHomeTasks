using Market.DTO;
using Market.Models;
using Market.Repositories.CategoryRepo;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private ICategoryRepository repository;

        public CategoryController(ICategoryRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Получение списка категорий
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "GetCategorys")]
        public async Task<ActionResult<IEnumerable<CategoryDto>?>> GetCategorys()
        {
            IEnumerable<CategoryDto>? categories = await repository.GetCategoriesAsync();
            return AcceptedAtAction("GetCategorys", categories);
        }

        /// <summary>
        /// Добавление категории по имени и описанию
        /// </summary>
        /// <param name="CategoryDto"></param>
        /// <returns></returns>
        [HttpPost(template: "AddCategory")]
        public async Task<ActionResult<Guid?>> AddCategory([FromQuery] CategoryDto CategoryDto)
        {
            Guid? newCategoryId = await repository.AddCategoryAsync(CategoryDto);
            return CreatedAtAction("AddCategory", newCategoryId);
        }

        /// <summary>
        /// Удаление категории по Guid
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpDelete(template: "DeleteCategory")]
        public async Task<ActionResult<Guid?>> DeleteCategory(Guid? CategoryId)
        {
            Guid? deletedCategoryId = await repository.DeleteCategoryAsync(CategoryId);
            return AcceptedAtAction(nameof(DeleteCategory), deletedCategoryId);
        }

        [HttpPut(template: "UpdateCategory")]
        public async Task<ActionResult<Guid>> UpdateCategory([FromQuery] Guid categoryId, CategoryDto categoryDto)
        {
            Guid? productid = await repository.UpdateCategotyAsync(categoryId, categoryDto);
            return AcceptedAtAction(nameof(UpdateCategory), productid);
        }
    }
}