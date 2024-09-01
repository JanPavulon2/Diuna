using Diuna.Services.Helpers;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Diuna.Services.Managers;

public class ConfigManager : IConfigManager
{
    private readonly string _configFilePath;
    private readonly ILogger<ConfigManager> _logger;

    public List<SwitchConfig> Switches { get; private set; }

    public ConfigManager(ILogger<ConfigManager> logger)
    {  
        var solutionDirectory = PathHelper.GetSolutionDirectory();
        _configFilePath = Path.Combine(solutionDirectory, "config", "config.json");
        _logger = logger;
    }

    public void LoadConfig()
    {
        try
        {
            if (File.Exists(_configFilePath))
            {
                _logger.LogInformation($"Loading configuration from {_configFilePath}.");

                var configJson = File.ReadAllText(_configFilePath);
                var configData = JsonSerializer.Deserialize<ConfigData>(configJson);
                Switches = configData.Switches ?? GetDefaultConfig();

                _logger.LogInformation("Configuration loaded successfully.");
            }
            else
            {
                _logger.LogWarning($"Config file not found at {_configFilePath}. Using default configuration.");
                Switches = GetDefaultConfig();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading configuration.");
            throw new ApplicationException("Failed to load configuration.", ex);
        }
            // Default config with 4 switches
            //Switches = new List<SwitchConfig>
            //    {
            //        new SwitchConfig { Tag = "Switch1", ButtonPin = 5, LedPin = 21, RelayPin = 26 },
            //        new SwitchConfig { Tag = "Switch2", ButtonPin = 11, LedPin = 20, RelayPin = 19 },
            //        new SwitchConfig { Tag = "Switch3", ButtonPin = 9, LedPin = 16, RelayPin = 13 },
            //        new SwitchConfig { Tag = "Switch4", ButtonPin = 10, LedPin = 12, RelayPin = 6 }
            //    };    
    }

    public void SaveConfig()
    {
        try
        {
            var configData = new ConfigData { Switches = Switches };
            var configJson = JsonSerializer.Serialize(configData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configFilePath, configJson);
            _logger.LogInformation("Configuration saved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving configuration.");
            throw new ApplicationException("Failed to save configuration.", ex);
        }
    }

    private List<SwitchConfig> GetDefaultConfig()
    {
        return new List<SwitchConfig>
            {
                new SwitchConfig { Tag = "Switch1", Description = "Bulb switch for the terrarium", ShortName = "Terrarium Bulb", ButtonPin = 23, LedPin = 21, RelayPin = 26 },
                new SwitchConfig { Tag = "Switch2", Description = "LED Strip 1", ShortName = "LED Strip 1", ButtonPin = 18, LedPin = 20, RelayPin = 19 },
                new SwitchConfig { Tag = "Switch3", Description = "LED Strip 2", ShortName = "LED Strip 2", ButtonPin = 15, LedPin = 16, RelayPin = 13 },
                new SwitchConfig { Tag = "Switch4", Description = "LED Strip 3", ShortName = "LED Strip 3", ButtonPin = 14, LedPin = 12, RelayPin = 6 }
            };
    }
}
