using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>
        : IRepository<T>
        where T : BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task<T> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            Data = Data.Append(entity);
            return Task.FromResult(entity);
        }

        public Task<T> UpdateAsync(T entity)
        {
            Data = Data.Select(x => x.Id == entity.Id ? entity : x);
            return Task.FromResult(entity);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var entity = Data.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                return Task.FromResult(false);

            Data = Data.Where(x => x.Id != id);
            return Task.FromResult(true);
        }
    }
}