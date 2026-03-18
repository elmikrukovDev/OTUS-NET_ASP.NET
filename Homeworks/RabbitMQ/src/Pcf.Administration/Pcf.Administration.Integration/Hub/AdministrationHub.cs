using Pcf.Administration.Core.Abstractions.Services;
using SignalRHub = Microsoft.AspNetCore.SignalR.Hub;

namespace Pcf.Administration.Integration.Hub;

public class AdministrationHub(IPromoCodeService promoCodeService)
    : SignalRHub
{
    public async Task NotifyAdministration(Guid partnerManagerId) =>
        await promoCodeService.UpdateAppliedPromocodesAsync(partnerManagerId);
}