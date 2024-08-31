using System.Device.Gpio;
using System.Diagnostics;

namespace DiunaRelayControl
{
    public class Dht21
    {
        private int _pin;
        private GpioController _gpioController;

        public Dht21(int pin)
        {
            _pin = pin;
            _gpioController = new GpioController();
        }

        public (double Humidity, double Temperature)? Read()
        {
            byte[] data = new byte[5]; // DHT21 sends 40 bits, split into 5 bytes
            _gpioController.OpenPin(_pin, PinMode.Output);

            // Send the start signal to the sensor
            _gpioController.Write(_pin, PinValue.Low);
            Thread.Sleep(20);  // Hold the pin low for at least 18ms (spec requires 1-10ms)
            _gpioController.Write(_pin, PinValue.High);
            Thread.Sleep(40);  // Hold the pin high for 20-40 microseconds

            // Switch to input mode to read the response
            _gpioController.SetPinMode(_pin, PinMode.InputPullUp);

            // Wait for the sensor's response (80us low, 80us high)
            if (WaitForState(PinValue.Low, 80) && WaitForState(PinValue.High, 80))
            {
                // Read 40 bits (5 bytes) of data
                for (int i = 0; i < 40; i++)
                {
                    // Wait for the pin to go low (start of bit signal)
                    if (!WaitForState(PinValue.Low, 50))
                        return null;  // Read error

                    // Measure the duration of the high signal
                    var pulseStart = Stopwatch.GetTimestamp();
                    if (!WaitForState(PinValue.High, 70))
                        return null;  // Read error
                    var pulseEnd = Stopwatch.GetTimestamp();

                    // If the high signal is long, it's a 1, otherwise it's a 0
                    var pulseDuration = (pulseEnd - pulseStart) * (1_000_000L / Stopwatch.Frequency);
                    int byteIndex = i / 8;
                    data[byteIndex] <<= 1;
                    if (pulseDuration > 40)  // 26-28us for 0, 70us for 1
                        data[byteIndex] |= 1;
                }

                // Verify checksum (sum of first 4 bytes should match the 5th byte)
                if (data[4] == ((data[0] + data[1] + data[2] + data[3]) & 0xFF))
                {
                    // Calculate humidity and temperature
                    double humidity = ((data[0] << 8) + data[1]) * 0.1;
                    double temperature = (((data[2] & 0x7F) << 8) + data[3]) * 0.1;
                    if ((data[2] & 0x80) != 0)  // Negative temperature
                    {
                        temperature = -temperature;
                    }
                    return (humidity, temperature);
                }
                else
                {
                    Console.WriteLine("Checksum failed");
                    return null;  // Checksum failed
                }
            }

            return null;
        }

        // Helper function to wait for a specific GPIO state for a given time
        private bool WaitForState(PinValue state, int timeoutMicros)
        {
            var stopwatch = Stopwatch.StartNew();
            while (_gpioController.Read(_pin) != state)
            {
                if (stopwatch.ElapsedTicks * (1_000_000L / Stopwatch.Frequency) >= timeoutMicros)
                    return false;
            }
            return true;
        }
    }
}
