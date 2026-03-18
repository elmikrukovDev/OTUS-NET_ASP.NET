using Pcf.GivingToCustomer.Core.Domain;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Abstractions.Services;

public interface IPromoCodeService
{
    Task GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest req);
}