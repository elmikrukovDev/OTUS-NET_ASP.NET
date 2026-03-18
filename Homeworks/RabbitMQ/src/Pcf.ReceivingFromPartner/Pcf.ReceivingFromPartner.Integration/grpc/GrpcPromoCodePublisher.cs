using Pcf.Grpc;
using Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;
using Pcf.ReceivingFromPartner.Core.Domain.Events;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Integration.grpc;

public class GrpcPromoCodePublisher(CustomerService.CustomerServiceClient client) 
    : IGivingPromoCodeToCustomerEventPublisher
{
    public async Task GivePromoCodeToCustomer(PromoCodeGivenEvent promoCode)
    {
        var request = new GivePromoCodeRequest
        {
            PromoCodeId = promoCode.PromoCodeId.ToString(),
            PromoCode = promoCode.PromoCode,
            PartnerId = promoCode.PartnerId.ToString(),
            PreferenceId = promoCode.PreferenceId.ToString(),
            BeginDate = promoCode.BeginDate,
            EndDate = promoCode.EndDate
        };

        await client.GivePromoCodeAsync(request);
    }
}