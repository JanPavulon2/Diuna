//using Microsoft.AspNetCore.SignalR;

//namespace Diuna.API.Hubs;

//public class SwitchHub : Hub
//{

//    private readonly ILogger<SwitchHub> _logger;

//    public SwitchHub(ILogger<SwitchHub> logger)
//    {
//        _logger = logger;
//    }

//    public async Task SendMessage(string switchTag, bool isOn)
//    {
//        // Log information about the message being sent
//        _logger.LogInformation("SendMessage invoked with SwitchTag: {SwitchTag}, IsOn: {IsOn}", switchTag, isOn);

//        try
//        {
//            // Send message to all clients
//            await Clients.All.SendAsync("ReceiveMessage", switchTag, isOn);

//            // Log success
//            _logger.LogInformation("Message successfully sent to all clients.");
//        }
//        catch (Exception ex)
//        {
//            // Log any errors
//            _logger.LogError(ex, "Error occurred while sending message with SwitchTag: {SwitchTag}", switchTag);
//            throw; // Re-throw the exception if you want it to propagate
//        }
//    }
//}