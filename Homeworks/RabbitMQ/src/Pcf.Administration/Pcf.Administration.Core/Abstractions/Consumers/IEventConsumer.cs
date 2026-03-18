using System;
using System.Threading.Tasks;

namespace Pcf.Administration.Core.Abstractions.Consumers;

public interface IEventConsumer
{
    Task SubscribeAsync<T>(string routingKey, string exchangeName, Func<T, Task> handleEvent);
}