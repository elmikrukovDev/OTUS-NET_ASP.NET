using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.Core.Service;

public interface IPreferenceService
{
    Task<IEnumerable<Preference>> GetPreferencesAsync();
}