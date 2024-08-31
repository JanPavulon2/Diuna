using Diuna.Services.Helpers;
using Diuna.Services.State;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Diuna.Services.Managers
{
    public class StateManager : IStateManager
    {
        private readonly string _stateFilePath;
        private Dictionary<string, SwitchState> _stateDictionary;

        public StateManager()
        {
            var solutionDirectory = PathHelper.GetSolutionDirectory();
            _stateFilePath = Path.Combine(solutionDirectory, "config", "state.json");

            LoadState();
        }

        /// <summary>
        /// Loads the switch state from a JSON file.
        /// </summary>
        public void LoadState()
        {
            if (File.Exists(_stateFilePath))
            {
                try
                {
                    var stateJson = File.ReadAllText(_stateFilePath);
                    _stateDictionary = JsonSerializer.Deserialize<Dictionary<string, SwitchState>>(stateJson) ?? new Dictionary<string, SwitchState>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load state: {ex.Message}");
                    _stateDictionary = new Dictionary<string, SwitchState>(); // Fallback to an empty dictionary on error
                }
            }
            else
            {
                _stateDictionary = new Dictionary<string, SwitchState>(); // Start with an empty dictionary if no state file is found
            }
        }

        public void SaveState()
        {
            try
            {
                var stateJson = JsonSerializer.Serialize(_stateDictionary, new JsonSerializerOptions { WriteIndented = true });

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(_stateFilePath));

                File.WriteAllText(_stateFilePath, stateJson);
                Console.WriteLine("State saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save state: {ex.Message}");
            }
        }

        public SwitchState GetStateByTag(string tag)
        {
            _stateDictionary.TryGetValue(tag, out var switchState);
            return switchState;
        }

        public void UpdateState(string tag, bool isOn)
        {
            if (!_stateDictionary.TryGetValue(tag, out var switchState))
            {
                switchState = new SwitchState { Tag = tag, IsOn = isOn };
                _stateDictionary[tag] = switchState;
            }
            else
            {
                switchState.IsOn = isOn;
            }

            SaveState(); // Save the updated state to the file
        }
    }
}
