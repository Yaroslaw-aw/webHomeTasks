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
        /// <param name="storageDto"></param>
        /// <returns></returns>
        [HttpPost(template: "AddStorage")]
        public async Task<ActionResult<Guid?>> AddStorage( StorageDto storageDto)
        {
            Guid? newStorageId = await repository.AddStorageAsync(storageDto);
            return CreatedAtAction("AddStorage", newStorageId);
        }

        /// <summary>
        /// Удаление склада по Guid
        /// </summary>
        /// <param name="storageId"></param>
        /// <returns></returns>
        [HttpDelete(template: "DeleteStorage")]
        public async Task<ActionResult<Guid?>> DeleteStorage(Guid? storageId)
        {
            Storage? deletetStorage = await repository.DeleteStorageAsync(storageId);
            return AcceptedAtAction(nameof(DeleteStorage), deletetStorage?.Id);
        }

        [HttpPut(template: "UpdateStorage")]
        public async Task<ActionResult<Guid?>> UpdateStorage([FromQuery] Guid storageId, StorageDto storageDto)
        {
            Guid? storageid = await repository.UpdateStorageAsync(storageId, storageDto);
            return AcceptedAtAction(nameof(UpdateStorage), storageid);
        }
    }
}
