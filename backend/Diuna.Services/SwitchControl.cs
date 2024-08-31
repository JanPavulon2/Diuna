using System.Device.Gpio;
using System.Diagnostics;

namespace Diuna.Services
{
    // Class to handle classic on/off switch 
    // Switch consists of button, LED and relay
    public class SwitchControl 
    {
        private readonly GpioController _gpioController;
        private Stopwatch _debounceTimer;  // Use Stopwatch for debounce
        private int _debounceTime = 200;   // Debounce time in milliseconds

        public string Tag { get; private set; }
        public string ShortName { get; private set; }
        public string Description { get; private set; }
        
        public int ButtonPin { get; private set; }
        public int LedPin { get; private set; }
        public int RelayPin { get; private set; }
        public bool IsOn { get; private set; }

        public SwitchControl(string tag, string shortName, string description, int buttonPin, int ledPin, int relayPin, bool isOn)
        {
            _gpioController = new GpioController();
            _debounceTimer = new Stopwatch();

            Tag = tag;
            ShortName = shortName;
            Description = description;

            ButtonPin = buttonPin;
            LedPin = ledPin;
            RelayPin = relayPin;
            IsOn = isOn;

            // Initialize GPIO pins
            _gpioController.OpenPin(buttonPin, PinMode.InputPullUp);  // Button with pull-up resistor
            _gpioController.OpenPin(ledPin, PinMode.Output);          // LED pin
            _gpioController.OpenPin(relayPin, PinMode.Output);        // Relay pin

            // Set initial state
            Set(IsOn);
        }

        // Setup button event handler
        public void SetupButtonHandler()
        {
             Console.WriteLine($"Setting up button handler for '{ShortName}' on GPIO {ButtonPin}");

            _gpioController.RegisterCallbackForPinValueChangedEvent(ButtonPin, PinEventTypes.Falling, (sender, args) =>
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
            IsOn = !IsOn;
            Set(IsOn);
        }

        public void Set(bool on)
        {
            if (on)
            {
                _gpioController.Write(RelayPin, PinValue.Low);   // Relay active-low: ON when set to Low
                _gpioController.Write(LedPin, PinValue.High);    // LED active-high: ON when set to High
            }
            else
            {
                _gpioController.Write(RelayPin, PinValue.High);  // Relay OFF
                _gpioController.Write(LedPin, PinValue.Low);     // LED OFF
            }
        }

        public void TurnOn()
        {
            Set(true);    
        }

        public void TurnOff()
        {
            Set(false);
        }
    }
}
