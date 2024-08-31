using System.Collections.Generic;
using System.Linq;

namespace Diuna.Services
{
    public class SwitchService
    {
        private readonly ConfigManager _configManager;
        private readonly List<SwitchControl> _switches;

        public SwitchService()
        {
            _configManager = new ConfigManager();
            _switches = new List<SwitchControl>();

            // Initialize switches based on config
            foreach (var switchConfig in _configManager.Switches)
            {
                var switchControl = new SwitchControl(
                    switchConfig.Tag,
                    switchConfig.Description,
                    switchConfig.ShortName,
                    switchConfig.ButtonPin,
                    switchConfig.LedPin,
                    switchConfig.RelayPin,
                    switchConfig.IsOn
                );
                switchControl.SetupButtonHandler();
                _switches.Add(switchControl);
            }
        }

        public IEnumerable<SwitchControl> GetAllSwitches() => _switches;

        public SwitchControl GetSwitchByTag(string tag) => _switches.FirstOrDefault(s => s.Tag == tag);

        public void ToggleSwitch(string tag)
        {
            var switchControl = GetSwitchByTag(tag);
            switchControl?.Toggle();
        }

        public void TurnOnSwitch(string tag)
        {
            var switchControl = GetSwitchByTag(tag);
            switchControl?.TurnOn();
        }

        public void TurnOffSwitch(string tag)
        {
            var switchControl = GetSwitchByTag(tag);
            switchControl?.TurnOff();
        }

        public void SaveConfiguration() => _configManager.SaveConfig();
    }
}
