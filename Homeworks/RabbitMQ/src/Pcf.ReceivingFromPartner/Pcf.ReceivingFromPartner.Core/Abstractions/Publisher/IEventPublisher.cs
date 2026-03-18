using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;

public interface IEventPublisher
{
    Task PublishAsync<T>(
        T @event, string routingKey, string exchangeName);
}