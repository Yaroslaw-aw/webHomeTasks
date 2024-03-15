using Market.DTO;
using Market.Models;
using Market.Repositories.StorageRepo;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    public class StorageController : Controller
    {
        private IStorageRepository repository;
        public StorageController(IStorageRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Получение списка категорий
        /// </summary>
        /// <returns></returns>
        [HttpGet(template: "GetStorages")]
        public async Task<ActionResult<IEnumerable<StorageDto>?>> GetStorages()
        {
            IEnumerable<StorageDto>? Storages = await repository.GetStoragesAsync();
            return AcceptedAtAction("GetStorages", Storages);
        }

        /// <summary>
        /// Добавление склада по названию и описанию
        /// </summary>
        /// <param name="StorageDto"></param>
        /// <returns></returns>
        [HttpPost(template: "AddStorage")]
        public async Task<ActionResult<Storage?>> AddStorage([FromQuery] StorageDto StorageDto)
        {
            Storage? newStorage = await repository.AddStorageAsync(StorageDto);
            return CreatedAtAction("AddStorage", newStorage);
        }

        /// <summary>
        /// Удаление склада по Guid
        /// </summary>
        /// <param name="StorageId"></param>
        /// <returns></returns>
        [HttpDelete(template: "DeleteStorage")]
        public async Task<ActionResult<Guid?>> DeleteStorage(Guid? StorageId)
        {
            Storage? deletetStorage = await repository.DeleteStorageAsync(StorageId);
            return AcceptedAtAction(nameof(DeleteStorage), deletetStorage?.Id);
        }
    }
}
