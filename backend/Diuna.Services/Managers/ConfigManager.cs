using Diuna.Services.Helpers;
using System.Text.Json;

namespace Diuna.Services.Managers;
public partial class ConfigManager : IConfigManager
{
    private readonly string _configFilePath;
    public List<SwitchConfig> Switches { get; set; }

    public ConfigManager()
    { 
        // Construct the path to the config file in the Diuna\config directory
        var solutionDirectory = PathHelper.GetSolutionDirectory();
        _configFilePath = Path.Combine(solutionDirectory, "config", "config.json");

        LoadConfig();
    }

    public void LoadConfig()
    {
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
            Switches = new List<SwitchConfig>(); // Start with an empty list if no config file is found

            // Default config with 4 switches
            //Switches = new List<SwitchConfig>
            //    {
            //        new SwitchConfig { Tag = "Switch1", ButtonPin = 5, LedPin = 21, RelayPin = 26 },
            //        new SwitchConfig { Tag = "Switch2", ButtonPin = 11, LedPin = 20, RelayPin = 19 },
            //        new SwitchConfig { Tag = "Switch3", ButtonPin = 9, LedPin = 16, RelayPin = 13 },
            //        new SwitchConfig { Tag = "Switch4", ButtonPin = 10, LedPin = 12, RelayPin = 6 }
            //    };

            //SaveConfig();
        }
    }

    public void SaveConfig()
    {
        // Define file path (saving to home directory in this example)
        //string _configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "config.json");

        Console.WriteLine($"Saving current state to {_configFilePath}");

        var config = new ConfigData { Switches = Switches };
        var configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        
        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(_configFilePath));

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

    /// <summary>
    /// Adds or updates a switch configuration and saves the changes to the file.
    /// </summary>
    /// <param name="switchConfig">The switch configuration to add or update.</param>
    public void AddOrUpdateSwitchConfig(SwitchConfig switchConfig)
    {
        var existingConfig = Switches.Find(s => s.Tag == switchConfig.Tag);
        if (existingConfig != null)
        {
            // Update existing configuration
            existingConfig.Description = switchConfig.Description;
            existingConfig.ShortName = switchConfig.ShortName;
            existingConfig.ButtonPin = switchConfig.ButtonPin;
            existingConfig.LedPin = switchConfig.LedPin;
            existingConfig.RelayPin = switchConfig.RelayPin;
        }
        else
        {
            // Add new configuration
            Switches.Add(switchConfig);
        }
        SaveConfig(); // Save the updated configuration to the file
    }
}
