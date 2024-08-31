using System.Text.Json;

namespace Diuna.Services
{
    public partial class ConfigManager
    {
        private readonly string _configFilePath = "/home/pi/Diuna/config.json";  // Use full path on Raspberry Pi

        // private readonly string _configFilePath = "config.json";

        public List<SwitchConfig> Switches { get; set; }

        public ConfigManager()
        {
            LoadConfig();
        }

        public void LoadConfig()
        {
            string _configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config.json");

            if (File.Exists(_configFilePath))
            {
                var fileFullPath = Path.GetFullPath(_configFilePath);
                Console.WriteLine($"Loading config from file: {fileFullPath}");

                var configJson = File.ReadAllText(_configFilePath);
                var config = JsonSerializer.Deserialize<ConfigData>(configJson);
                Switches = config.Switches;

                Console.WriteLine("Config loaded successfully.");
            }
            else
            {
                Console.WriteLine("Config file not found. Using default settings.");

                // Default config with 4 switches
                Switches = new List<SwitchConfig>
                {
                    new SwitchConfig { Tag = "Switch1", ButtonPin = 5, LedPin = 21, RelayPin = 26, IsOn = false },
                    new SwitchConfig { Tag = "Switch2", ButtonPin = 11, LedPin = 20, RelayPin = 19, IsOn = false },
                    new SwitchConfig { Tag = "Switch3", ButtonPin = 9, LedPin = 16, RelayPin = 13, IsOn = false },
                    new SwitchConfig { Tag = "Switch4", ButtonPin = 10, LedPin = 12, RelayPin = 6, IsOn = false }
                };
                SaveConfig();
            }
        }

        public void SaveConfig()
        {
            // Define file path (saving to home directory in this example)
            string _configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config.json");

            Console.WriteLine($"Saving current state to {_configFilePath}");

            var config = new ConfigData { Switches = Switches };
            var configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

            try
            {
                File.WriteAllText(_configFilePath, configJson);
                Console.WriteLine("Config successfully saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save config: {ex.Message}");
            }
        }
    }
}
