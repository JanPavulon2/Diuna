using System.Device.Gpio;

namespace Diuna.Models.Gpio;

public class GpioInfo
{
    public int PinNumber { get; set; }
    public bool IsOpen { get; set; }
    public PinMode? Mode { get; set; }
    public PinValue? Value { get; set; }
    public Exception Error { get; set; }
}
