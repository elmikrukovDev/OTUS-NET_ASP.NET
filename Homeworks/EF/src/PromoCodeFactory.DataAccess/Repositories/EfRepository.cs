using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T>
        : IRepository<T>
        where T : BaseEntity
    {
        protected readonly SqliteContext _context;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(SqliteContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .ToListAsync();
        }

        public virtual Task<T> GetByIdAsync(Guid id)
        {
            return _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var existingEntity = await _dbSet.FindAsync(entity.Id) 
                ?? throw new InvalidOperationException("Not found!");

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            _context.Entry(existingEntity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null)
                return false;

            _context.Set<T>().Remove(existingEntity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
