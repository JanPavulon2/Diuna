using System.Device.Gpio;

namespace Diuna.Services.Gpio;

public interface IGpioService
{
    void InitializePin(int pinNumber, PinMode mode);
    void WritePin(int pinNumber, PinValue value);
    void RegisterButtonPressCallback(int pinNumber, PinChangeEventHandler callback);
    void Cleanup();
}
