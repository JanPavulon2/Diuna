using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

public class Program2
{
    public static async Task Main(string[] args)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7191/switchhub")  // Replace with your API URL
            .Build();

        connection.On<string, bool>("ReceiveMessage", (switchTag, isOn) =>
        {
            Console.WriteLine($"{switchTag}: {isOn}");
        });

        try
        {
            await connection.StartAsync();
            Console.WriteLine("Connection started. Listening for messages...");

            Console.ReadLine();  // Keep the console app running
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}