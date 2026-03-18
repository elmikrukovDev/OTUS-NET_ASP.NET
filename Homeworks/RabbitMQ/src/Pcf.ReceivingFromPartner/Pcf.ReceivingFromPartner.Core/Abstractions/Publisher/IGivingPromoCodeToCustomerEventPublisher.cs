using Pcf.ReceivingFromPartner.Core.Domain.Events;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;

public interface IGivingPromoCodeToCustomerEventPublisher
{
    public Task GivePromoCodeToCustomer(PromoCodeGivenEvent promoCode);
}