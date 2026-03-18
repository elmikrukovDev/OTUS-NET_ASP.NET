using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;

namespace Pcf.ReceivingFromPartner.Integration;

public class SignalRAdministrationPublisher : IAdministrationEventPublisher
{
    private readonly HubConnection _connection;

    public SignalRAdministrationPublisher()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:8091/administrationHub")
            .WithAutomaticReconnect()
            .Build();

        _connection.StartAsync().GetAwaiter().GetResult();
    }

    public async Task NotifyAdminAboutPartnerManagerPromoCode(Guid partnerManagerId) =>
        await _connection.InvokeAsync("NotifyAdministration", partnerManagerId);
}