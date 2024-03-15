using Market.DTO;
using Market.Models;

namespace Market.Repositories.StorageRepo
{
    public interface IStorageRepository
    {
        Task<IEnumerable<StorageDto>> GetStoragesAsync();
        Task<Guid?> AddStorageAsync(StorageDto StorageDto);
        Task<Storage?> DeleteStorageAsync(Guid? StorageId);
    }
}
