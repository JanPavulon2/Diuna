using System.Device.Gpio;

namespace Diuna.Services.Gpio;

public class GpioService : IGpioService
{
    private readonly GpioController _gpioController;

    public GpioService()
    {
        _gpioController = new GpioController();
    }

    public void InitializePin(int pinNumber, PinMode mode)
    {
        _gpioController.OpenPin(pinNumber, mode);
    }

    public void WritePin(int pinNumber, PinValue value)
    {
        _gpioController.Write(pinNumber, value);
    }

    public void RegisterButtonPressCallback(int pinNumber, PinChangeEventHandler callback)
    {
        _gpioController.RegisterCallbackForPinValueChangedEvent(pinNumber, PinEventTypes.Falling, callback);
    }

    public void Cleanup()
    {
        _gpioController.Dispose();
    }
}
