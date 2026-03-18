using System;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;

public interface IAdministrationEventPublisher
{
    Task NotifyAdminAboutPartnerManagerPromoCode(Guid partnerManagerId);
}