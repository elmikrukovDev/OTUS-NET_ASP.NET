using System.Threading.Tasks;

namespace Pcf.Administration.Core.Abstractions.Consumers;

public interface IAdministrationEventConsumer
{
    Task StartAsync();
}