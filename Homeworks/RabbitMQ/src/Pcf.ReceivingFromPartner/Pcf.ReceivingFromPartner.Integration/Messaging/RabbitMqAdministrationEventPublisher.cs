using System;
using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;

namespace Pcf.ReceivingFromPartner.Integration.Messaging;

public class RabbitMqAdministrationEventPublisher(IEventPublisher eventPublisher) 
    : IAdministrationEventPublisher
{
    private const string ExchangeName = "administration-events";

    public Task NotifyAdminAboutPartnerManagerPromoCode(Guid partnerManagerId) =>
        eventPublisher.PublishAsync(
            partnerManagerId,
            routingKey: "admin.notify",
            exchangeName: ExchangeName
        );
}