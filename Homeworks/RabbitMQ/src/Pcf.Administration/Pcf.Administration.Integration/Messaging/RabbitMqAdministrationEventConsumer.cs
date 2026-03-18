using Pcf.Administration.Core.Abstractions.Consumers;
using Pcf.Administration.Core.Abstractions.Services;

namespace Pcf.Administration.Integration.Messaging;

public class RabbitMqAdministrationEventConsumer(IEventConsumer consumer, IPromoCodeService promoCodeService) 
    : IAdministrationEventConsumer
{
    private const string ExchangeName = "administration-events";

    public async Task StartAsync()
    {
        await consumer.SubscribeAsync<Guid>(
            "admin.notify", 
            ExchangeName, 
            promoCodeService.UpdateAppliedPromocodesAsync);

    }
}