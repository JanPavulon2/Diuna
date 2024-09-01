using Diuna.Services.Gpio;
using System.Device.Gpio;
using System.Diagnostics;

namespace Diuna.Services.Switch;

// Class to handle classic on/off switch 
// Switch consists of button, LED and relay
public class SwitchControl
{
    private readonly IGpioService _gpioService;
    private readonly Stopwatch _debounceTimer;
    private int _debounceTime = 200;

    public string Tag { get; private set; }
    public string ShortName { get; private set; }
    public string Description { get; private set; }

    public int ButtonPin { get; private set; }
    public int LedPin { get; private set; }
    public int RelayPin { get; private set; }

    public bool IsOn { get; private set; }

    public SwitchControl()
    {
        _debounceTimer = new Stopwatch();

        _gpioService = new MockGpioService();
    }

    public SwitchControl(IGpioService gpioService, string tag, string shortName, string description, int buttonPin, int ledPin, int relayPin, bool isOn)
    {
        _gpioService = gpioService;
        _debounceTimer = new Stopwatch();

        Tag = tag;
        ShortName = shortName;
        Description = description;

        ButtonPin = buttonPin;
        LedPin = ledPin;
        RelayPin = relayPin;

        IsOn = isOn;

        InitializePins();
    }

    private void InitializePins()
    {

        _gpioService.InitializePin(ButtonPin, PinMode.InputPullUp);
        _gpioService.InitializePin(LedPin, PinMode.Output);
        _gpioService.InitializePin(RelayPin, PinMode.Output);

        Set(IsOn); // Set initial state
    }

    // Setup button event handler
    public void SetupButtonHandler()
    {
        Console.WriteLine($"Setting up button handler for '{ShortName}' on GPIO {ButtonPin}");

        _gpioService.RegisterButtonPressCallback(ButtonPin, (sender, args) =>
        {
            if (_debounceTimer.ElapsedMilliseconds > _debounceTime)
            {
                Console.WriteLine($"Button pressed on '{ShortName}'");
                Toggle();
                _debounceTimer.Restart();  // Reset the debounce timer
            }
        });

        _debounceTimer.Start();  // Start the debounce timer

        // Additionally, register Rising event to see when the button is released
        //_gpioController.RegisterCallbackForPinValueChangedEvent(ButtonPin, PinEventTypes.Rising, (sender, args) =>
        //{
        //    Console.WriteLine($"Button released on {NameTag}");
        //});
    }

    public void Toggle()
    {
        Set(!IsOn);
    }

    public void TurnOn()
    {
        Set(true);
    }

    public void TurnOff()
    {
        Set(false);
    }

    public void Set(bool on)
    {
        if (on)
        {
            _gpioService.WritePin(RelayPin, PinValue.Low);   // Relay active-low: ON when set to Low
            _gpioService.WritePin(LedPin, PinValue.High);    // LED active-high: ON when set to High
        }
        else
        {
            _gpioService.WritePin(RelayPin, PinValue.High);  // Relay OFF
            _gpioService.WritePin(LedPin, PinValue.Low);     // LED OFF
        }

        IsOn = on;
    }
}
