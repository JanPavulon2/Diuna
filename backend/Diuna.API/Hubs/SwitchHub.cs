using Microsoft.AspNetCore.SignalR;

namespace Diuna.API.Hubs;

public class SwitchHub : Hub
{
    public async Task SendMessage(string switchTag, bool isOn)
    {
        await Clients.All.SendAsync("ReceiveMessage", switchTag, isOn);
    }
}