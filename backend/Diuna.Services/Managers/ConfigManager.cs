using Diuna.Models.Config;
using Diuna.Services.Helpers;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Diuna.Services.Managers;

public class ConfigManager : IConfigManager
{
    private readonly string _configFilePath;
    private ConfigData _configData;
    private readonly ILogger<ConfigManager> _logger;


    public Settings Settings => _configData.Settings;
    public List<SwitchConfig> Switches => _configData.Switches;

    public ConfigManager(ILogger<ConfigManager> logger)
    {  
        var solutionDirectory = PathHelper.GetSolutionDirectory();
        _configFilePath = Path.Combine(solutionDirectory, "config", "config.json");
        _logger = logger;

        _configData = new ConfigData();
        LoadConfigFromFile();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
    }

    public void LoadConfigFromFile()
    {
        try
        {
            var defaultConfig = GetDefaultConfig();

            if (File.Exists(_configFilePath))
            {
                _logger.LogInformation($"Loading configuration from {_configFilePath}.");

                var configJson = File.ReadAllText(_configFilePath);
                var configData = JsonSerializer.Deserialize<ConfigData>(configJson);

                if (configData != null)
                {
                    _logger.LogInformation("Configuration loaded successfully.");
                    _configData = configData;
                }
                else
                {
                    _logger.LogWarning("Deserialized config data is null, using default configuration.");
                    _configData = GetDefaultConfig();
                }
            }
            else
            {
                _logger.LogWarning($"Config file not found at {_configFilePath}. Using default configuration.");
                _configData = GetDefaultConfig();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading configuration.");
            throw new ApplicationException("Failed to load configuration.", ex);
        }
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

    private ConfigData GetDefaultConfig()
    {
        return new ConfigData
        {
            Settings = new Settings
            {
                DebounceTime = 200,
                RefreshInterval = 5000
            },
            Switches = new List<SwitchConfig>
            {
                new SwitchConfig { Tag = "Switch1", Description = "Bulb switch for the terrarium", ShortName = "Terrarium Bulb", ButtonPin = 23, LedPin = 21, RelayPin = 26 },
                new SwitchConfig { Tag = "Switch2", Description = "LED Strip 1", ShortName = "LED Strip 1", ButtonPin = 18, LedPin = 20, RelayPin = 19 },
                new SwitchConfig { Tag = "Switch3", Description = "LED Strip 2", ShortName = "LED Strip 2", ButtonPin = 15, LedPin = 16, RelayPin = 13 },
                new SwitchConfig { Tag = "Switch4", Description = "LED Strip 3", ShortName = "LED Strip 3", ButtonPin = 14, LedPin = 12, RelayPin = 6 }
            }
        };
    }
}
