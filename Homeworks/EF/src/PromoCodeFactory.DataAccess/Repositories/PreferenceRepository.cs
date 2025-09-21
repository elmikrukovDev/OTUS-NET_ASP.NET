using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Contexts;
using System;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class PreferenceRepository
        : EfRepository<Preference>, IPreferenceRepository
    {
        public PreferenceRepository(SqliteContext context)
            : base(context)
        {

        }

        public async Task<Preference> GetByDescription(string description)
        {
            return await _dbSet.SingleOrDefaultAsync(p => 
                p.Description.Equals(description));
        }
    }
}
