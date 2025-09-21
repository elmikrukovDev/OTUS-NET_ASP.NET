using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IPreferenceRepository : IRepository<Preference>
    {
        Task<Preference> GetByDescription(string description);
    }
}