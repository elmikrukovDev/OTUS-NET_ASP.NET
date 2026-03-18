using System;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Abstractions.Consumers;

public interface IEventConsumer
{
    Task SubscribeAsync<T>(string routingKey, string exchangeName, Func<T, Task> handleEvent);
}