using System.Collections.Generic;
using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Cache;

public interface IPreferenceCacheService
{
    Task<IEnumerable<Preference>> GetPreferencesAsync();
}