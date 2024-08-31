//using Microsoft.AspNetCore.SignalR;
//using System.Collections.Concurrent;

//namespace Diuna.SignalR.Hubs;

//public class SwitchHub : Hub
//{
//    private readonly IHubContext<SwitchHub> _hubContext;
//    public SwitchHub(IHubContext<SwitchHub> hubContext)
//    {
//        _hubContext = hubContext;
//    }

//    // Store connected client IDs
//    private static ConcurrentDictionary<string, string> ConnectedClients = new ConcurrentDictionary<string, string>();

//    public async Task BroadcastSwitchState(string switchTag, bool isOn)
//    {
//        Console.WriteLine($"Broadcasting switch state change: {switchTag} is {(isOn ? "On" : "Off")}");
//        await _hubContext.Clients.All.SendAsync("SwitchStateChanged", switchTag, isOn);
//    }

//    public override async Task OnConnectedAsync()
//    {
//        // Add the connected client to the list
//        ConnectedClients.TryAdd(Context.ConnectionId, Context.ConnectionId);

//        // Log connection
//        Console.WriteLine($"Client connected: {Context.ConnectionId}");
//        await base.OnConnectedAsync();
//    }

//    public override async Task OnDisconnectedAsync(Exception exception)
//    {
//        // Remove the disconnected client from the list
//        ConnectedClients.TryRemove(Context.ConnectionId, out _);

//        // Log disconnection
//        Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
//        await base.OnDisconnectedAsync(exception);
//    }

//    public async Task ListConnectedClients()
//    {
//        foreach (var clientId in ConnectedClients.Keys)
//        {
//            Console.WriteLine($"Connected client ID: {clientId}");
//        }

//        await Task.CompletedTask;
//    }
//}
