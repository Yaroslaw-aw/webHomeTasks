using AutoMapper;
using Market.DTO;
using Market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace Market.Repositories.StorageRepo
{
    public class StorageRepository : IStorageRepository
    {
        private MarketContext context;
        private IMapper mapper;

        public StorageRepository(IMapper mapper, MarketContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получение списка складов
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<StorageDto>> GetStoragesAsync()
        {
            await context.SaveChangesAsync();
            using (context)
            {
                return context.Storages.Select(mapper.Map<StorageDto>).ToList();
            }
        }

        /// <summary>
        /// Добавление склада
        /// </summary>
        /// <param name="StorageDto"></param>
        /// <returns></returns>
        public async Task<Storage?> AddStorageAsync([FromQuery] StorageDto StorageDto)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Storage? newStorage = mapper.Map<Storage>(StorageDto);
                await context.Set<Storage>().AddAsync(newStorage);
                context.SaveChanges();
                transaction.Commit();
                return newStorage;
            }
        }

        /// <summary>
        /// Удаление склада
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Storage?> DeleteStorageAsync(Guid? id)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                Storage? deletedStorage = context.Storages.FirstOrDefault(p => p.Id == id);
                if (deletedStorage != null)
                {
                    context.Storages.Remove(deletedStorage);
                    await context.SaveChangesAsync();
                    transaction.Commit();
                    return deletedStorage;
                }
            }
            return null;
        }
    }
}
