﻿using AutoMapper;
using Market.DTO;
using Market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<Storage> storages = await context.Set<Storage>().AsNoTracking().ToListAsync();
            IEnumerable<StorageDto> result = mapper.Map<IEnumerable<StorageDto>>(storages);
            return result;
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
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
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
                    await transaction.CommitAsync();
                    return deletedStorage;
                }
            }
            return null;
        }
    }
}
