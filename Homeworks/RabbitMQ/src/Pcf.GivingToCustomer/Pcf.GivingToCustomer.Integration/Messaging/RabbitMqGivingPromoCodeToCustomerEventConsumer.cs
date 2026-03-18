using Pcf.GivingToCustomer.Core.Abstractions.Consumers;
using Pcf.GivingToCustomer.Core.Abstractions.Services;
using Pcf.GivingToCustomer.Core.Domain;
using System.Threading.Tasks;

namespace SurveyManageService.Infrastructure.Messaging;

public class RabbitMqGivingPromoCodeToCustomerEventConsumer(IEventConsumer consumer, IPromoCodeService promoCodeService) 
    : IGivingPromoCodeToCustomerEventConsumer
{
    private const string ExchangeName = "promocode-events";

    public async Task StartAsync()
    {
        await consumer.SubscribeAsync<GivePromoCodeRequest>(
            "promocode.give", 
            ExchangeName, 
            promoCodeService.GivePromoCodesToCustomersWithPreferenceAsync);
    }
}