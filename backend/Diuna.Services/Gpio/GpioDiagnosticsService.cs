using Diuna.Models.Gpio;
using System.Device.Gpio;

namespace Diuna.Services.Gpio;

public class GpioDiagnosticsService 
{
    private readonly GpioController _gpioController;

    public GpioDiagnosticsService()
    {
        _gpioController = new GpioController();
    }

    public List<GpioInfo> GetGpioInfo(int maxPinNumber)
    {
        var gpioInfoList = new List<GpioInfo>();

        for (int pin = 0; pin <= maxPinNumber; pin++)
        {
            var gpioInfo = new GpioInfo { PinNumber = pin };

            try
            {
                // Try to open the pin as an input
                _gpioController.OpenPin(pin, PinMode.Input);
                gpioInfo.IsOpen = true;
                gpioInfo.Mode = PinMode.Input;
                gpioInfo.Value = _gpioController.Read(pin);

                // Close the pin after reading
                _gpioController.ClosePin(pin);
            }
            catch (Exception exInput)
            {
                gpioInfo.Error = exInput;
            }

            try
            {
                // Try to open the pin as an output
                _gpioController.OpenPin(pin, PinMode.Output);
                gpioInfo.IsOpen = true;
                gpioInfo.Mode = PinMode.Output;

                // Set a default value and read it back
                _gpioController.Write(pin, PinValue.High);
                gpioInfo.Value = _gpioController.Read(pin);

                // Close the pin after reading
                _gpioController.ClosePin(pin);
            }
            catch (Exception exOutput)
            {
                gpioInfo.Error = gpioInfo.Error ?? exOutput;
            }

            gpioInfoList.Add(gpioInfo);
        }

        return gpioInfoList;
    }

    public void PrintGpioInfo(List<GpioInfo> gpioInfoList)
    {
        foreach (var gpioInfo in gpioInfoList)
        {
            Console.WriteLine($"Pin: {gpioInfo.PinNumber}");
            Console.WriteLine($"  IsOpen: {gpioInfo.IsOpen}");
            Console.WriteLine($"  Mode: {gpioInfo.Mode}");
            Console.WriteLine($"  Value: {gpioInfo.Value}");

            if (gpioInfo.Error != null)
            {
                Console.WriteLine($"  Error: {gpioInfo.Error.Message}");
            }

            Console.WriteLine();
        }
    }
}
