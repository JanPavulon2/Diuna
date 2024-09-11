using System;
using System.Device.Gpio;
using System.Threading;

class ShiftRegister74HC595
{
    private int dataPin;
    private int clockPin;
    private int latchPin;
    private GpioController gpioController;

    public ShiftRegister74HC595(int data, int clock, int latch)
    {
        dataPin = data;
        clockPin = clock;
        latchPin = latch;
        gpioController = new GpioController();

        gpioController.OpenPin(dataPin, PinMode.Output);
        gpioController.OpenPin(clockPin, PinMode.Output);
        gpioController.OpenPin(latchPin, PinMode.Output);
    }

    // Shift out 8 bits of data to the 74HC595 shift register
    public void ShiftOut(byte value)
    {
        gpioController.Write(latchPin, PinValue.Low);  // Prepare for shifting

        for (int i = 0; i < 8; i++)
        {
            var bit = (value & (1 << (7 - i))) > 0 ? PinValue.High : PinValue.Low;
            gpioController.Write(dataPin, bit);

            // Pulse the clock pin
            gpioController.Write(clockPin, PinValue.High);
            Thread.Sleep(1);  // Small delay for pulse width
            gpioController.Write(clockPin, PinValue.Low);
        }

        gpioController.Write(latchPin, PinValue.High);  // Apply shifted data to outputs
    }
}

public class Program2
{
    public static void Main()
    {
        // GPIO pins for 74HC595 connections (customize based on your wiring)
        int dataPin = 17;   // SER (Data)
        int clockPin = 22;  // SRCLK (Clock)
        int latchPin = 27;  // RCLK (Latch)

        var shiftRegister = new ShiftRegister74HC595(dataPin, clockPin, latchPin);

        // Light patterns for LEDs on Q1 and Q2:
        byte[] ledPatterns = {
            0b00000010,  // Q1 on (second LED)
            0b00000100,  // Q2 on (third LED)
            0b00000110,   // Q1 and Q2 on (both LEDs)
            0b00001000   // Q3 
        };

        // Cycle through the patterns
        while (true)
        {
            foreach (var pattern in ledPatterns)
            {
                shiftRegister.ShiftOut(pattern);  // Shift the pattern into the 74HC595
                Thread.Sleep(1000);  // Wait 1 second between each change
            }
        }
    }
}


// using Microsoft.AspNetCore.SignalR.Client;
// using Microsoft.Extensions.Logging;

// public class Program2
// {
//     public static async Task Main(string[] args)
//     {
//         var connection = new HubConnectionBuilder()
//             .WithUrl("https://localhost:7191/switchhub")  // Replace with your API URL
//             .Build();

//         connection.On<string, bool>("ReceiveMessage", (switchTag, isOn) =>
//         {
//             Console.WriteLine($"{switchTag}: {isOn}");
//         });

//         try
//         {
//             await connection.StartAsync();
//             Console.WriteLine("Connection started. Listening for messages...");

//             Console.ReadLine();  // Keep the console app running
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"An error occurred: {ex.Message}");
//         }
//     }
// }