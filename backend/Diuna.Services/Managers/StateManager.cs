using Diuna.Models.State;
using Diuna.Services.Helpers;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Diuna.Services.Managers;

public class StateManager : IStateManager
{
    private readonly string _stateFilePath;
    private readonly ILogger<StateManager> _logger;
    private Dictionary<string, SwitchState> _state;

    public StateManager(ILogger<StateManager> logger)
    {
        var solutionDirectory = PathHelper.GetSolutionDirectory();
        _stateFilePath = Path.Combine(solutionDirectory, "config", "state.json");

        _logger = logger;
        _state = new Dictionary<string, SwitchState>();

        LoadStateFromFile(new Dictionary<string, SwitchState>());
    }

    public SwitchState GetStateByTag(string tag)
    {
        // Todo - make sure if right
        return _state.ContainsKey(tag) ? _state[tag] : new SwitchState { Tag = tag, IsOn = false };
    }

    public void LoadStateFromFile(Dictionary<string, SwitchState> defaultState)
    {
        try
        {
            if (File.Exists(_stateFilePath))
            {
                _logger.LogInformation($"Loading state from {_stateFilePath}.");
                var stateJson = File.ReadAllText(_stateFilePath);
                var loadedState = JsonSerializer.Deserialize<Dictionary<string, SwitchState>>(stateJson) ?? new Dictionary<string, SwitchState>();
                _state = loadedState ?? new Dictionary<string, SwitchState>();

                _logger.LogInformation("State loaded successfully.");
            }
            else
            {
                _logger.LogWarning($"State file not found at {_stateFilePath}. Using default state.");
                _state = defaultState;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading state.");
            throw new ApplicationException("Failed to load state.", ex);
        }
    }

    public void SaveStateToFile()
    {
        try
        {
            _logger.LogInformation($"Saving state to {_stateFilePath}.");
            var stateJson = JsonSerializer.Serialize(_state, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_stateFilePath, stateJson);
            _logger.LogInformation("State saved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving state.");
            throw new ApplicationException("Failed to save state.", ex);
        }
    }

    public void UpdateInMemoryState(string tag, bool isOn)
    {
        _logger.LogInformation($"Getting state of: {tag}");

        if (!_state.TryGetValue(tag, out var switchState))
        {
            _logger.LogWarning($"State for tag {tag} does not exist. Creating new entry.");
            switchState = new SwitchState { Tag = tag, IsOn = isOn };
            _state[tag] = switchState;
        }
        else
        {
            switchState.IsOn = isOn;
        }

        _logger.LogInformation($"State updated: {tag} is now {(isOn ? "ON" : "OFF")}");
    }
}
