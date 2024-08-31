using System.Device.Gpio;

namespace Diuna.Services.Gpio;

public class MockGpioService : IGpioService
{
    public MockGpioService()
    {
    }

    public void InitializePin(int pinNumber, PinMode mode)
    {
        Console.WriteLine($"[Mock] InitializePin: {pinNumber}, Mode: {mode}");
    }

    public void WritePin(int pinNumber, PinValue value)
    {
        Console.WriteLine($"[Mock] WritePin: {pinNumber}, Value: {value}");
    }

    public void RegisterButtonPressCallback(int pinNumber, PinChangeEventHandler callback)
    {
        Console.WriteLine($"[Mock] RegisterButtonPressCallback: {pinNumber}");
        // Simulate a button press event if needed during tests
    }

    public void Cleanup()
    {
        Console.WriteLine("[Mock] Cleanup called");
    }
}
