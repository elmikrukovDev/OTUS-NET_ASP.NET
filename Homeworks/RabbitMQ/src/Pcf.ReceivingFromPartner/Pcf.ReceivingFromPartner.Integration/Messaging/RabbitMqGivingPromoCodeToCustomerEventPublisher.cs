using Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;
using Pcf.ReceivingFromPartner.Core.Domain.Events;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Integration.Messaging;

public class RabbitMqGivingPromoCodeToCustomerEventPublisher(IEventPublisher eventPublisher)
    : IGivingPromoCodeToCustomerEventPublisher
{
    private const string ExchangeName = "promocode-events";

    public Task GivePromoCodeToCustomer(PromoCodeGivenEvent promoCode) =>
        eventPublisher.PublishAsync(
            promoCode,
            routingKey: "promocode.give",
            exchangeName: ExchangeName
        );
}