namespace DOOH.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

public class AdboardStatusHub : Hub
{
    public async Task SendAdboardStatus(string adboardId, bool status)
    {
        await Clients.All.SendAsync("ReceiveAdboardStatus",adboardId, status);
    }
}