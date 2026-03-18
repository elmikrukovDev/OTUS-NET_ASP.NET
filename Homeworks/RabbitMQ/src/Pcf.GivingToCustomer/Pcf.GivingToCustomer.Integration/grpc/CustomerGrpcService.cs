using Grpc.Core;
using Pcf.GivingToCustomer.Core.Abstractions.Services;
using Pcf.Grpc;
using System;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Integration.grpc;

public class CustomerGrpcService(IPromoCodeService promoCodeService) 
    : CustomerService.CustomerServiceBase
{
    public override async Task<GivePromoCodeResponse> GivePromoCode(
        GivePromoCodeRequest request,
        ServerCallContext context)
    {
        var model = new Core.Domain.GivePromoCodeRequest
        {
            PromoCodeId = Guid.Parse(request.PromoCodeId),
            PromoCode = request.PromoCode,
            PartnerId = Guid.Parse(request.PartnerId),
            PreferenceId = Guid.Parse(request.PreferenceId),
            BeginDate = request.BeginDate,
            EndDate = request.EndDate
        };

        await promoCodeService.GivePromoCodesToCustomersWithPreferenceAsync(model);
        return new GivePromoCodeResponse { Success = true };
    }
}