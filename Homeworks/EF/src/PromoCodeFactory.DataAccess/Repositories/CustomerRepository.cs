using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class CustomerRepository
        : EfRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SqliteContext context)
            : base(context)
        {

        }

        public async Task<IEnumerable<Customer>> GetWithPreference(Guid preferenceId)
        {
            return await _dbSet.Include(c => c.CustomerPreferences)
                .SelectMany(c => c.CustomerPreferences)
                .Where(cp => cp.PreferenceId == preferenceId)
                .Select(cp => cp.Customer)
                .ToListAsync();
        }

        public override async Task<Customer> UpdateAsync(Customer entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var existingEntity = await _dbSet
                    .Include(e => e.CustomerPreferences)
                    .FirstOrDefaultAsync(e => e.Id == entity.Id)
                ?? throw new InvalidOperationException("Not found!");

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            existingEntity.CustomerPreferences.Clear();
            foreach (var cp in entity.CustomerPreferences)
                existingEntity.CustomerPreferences.Add(cp);

            await _context.SaveChangesAsync();

            _context.Entry(existingEntity).State = EntityState.Detached;

            return entity;
        }
    }
}
