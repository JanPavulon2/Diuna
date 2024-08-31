using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace DiunaRelayControl
{
    class Program
    {        
        static bool _running = true;

        static void Main(string[] args)
        {
            // var sensor = new Dht21(4);  // Use GPIO4

            // while (true)
            // {
            //     var data = sensor.Read();
            //     if (data.HasValue)
            //     {
            //         Console.WriteLine($"Humidity: {data.Value.Humidity}%");
            //         Console.WriteLine($"Temperature: {data.Value.Temperature}°C");
            //     }
            //     else
            //     {
            //         Console.WriteLine("Failed to read from sensor");
            //     }

            //     Thread.Sleep(2000);  // Wait 2 seconds between readings
            // }

            var configManager = new ConfigManager();
            var switches = new List<SwitchControl>();

            try
            {
                Console.CancelKeyPress += (sender, e) => 
                {
                    Console.WriteLine("Terminating application...");
                    _running = false;
                    e.Cancel = true;
                };

                // Load switches from config
                foreach (var switchConfig in configManager.Switches)
                {
                    var switchControl = new SwitchControl(
                        switchConfig.Name,
                        switchConfig.ButtonPin,
                        switchConfig.LedPin,
                        switchConfig.RelayPin,
                        switchConfig.IsOn
                    );
                    switchControl.SetupButtonHandler();
                    switches.Add(switchControl);
                }

                Console.WriteLine("Working in background... Press Ctrl+C to stop.");

                // Main loop, runs until _running is set to false (on Ctrl+C)
                while (_running)
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            Console.WriteLine("Saving config");
            configManager.SaveConfig();
        }
    }
}
