using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Abstractions.Consumers;

public interface IGivingPromoCodeToCustomerEventConsumer
{
    Task StartAsync();
}