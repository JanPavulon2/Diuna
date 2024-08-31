using AutoMapper;
using Diuna.Services.State;
using Diuna.Services.Gpio;
using Diuna.Services.Managers;
using Diuna.Services.Switch;

public class SwitchService : ISwitchService
{
    private readonly IConfigManager _configManager;
    private readonly IStateManager _stateManager;
    private readonly List<SwitchControl> _switches;

    public SwitchService(IGpioService gpioService, IConfigManager configManager, IStateManager stateManager, IMapper mapper)
    {
        _configManager = configManager;
        _stateManager = stateManager;
        _switches = new List<SwitchControl>();

        // Initialize switches based on config and state
        foreach (var switchConfig in _configManager.Switches)
        {
            var switchState = _stateManager.GetStateByTag(switchConfig.Tag);
            if (switchState == null)
            {
                switchState = new SwitchState
                {
                    Tag = switchConfig.Tag,
                    IsOn = false
                };
            }
            //var switchState = _stateManager.GetStateByTag(switchConfig.Tag) ?? new SwitchState { Tag = switchConfig.Tag, IsOn = false };

            var switchControl = new SwitchControl(
                gpioService,
                switchConfig.Tag,
                switchConfig.Description,
                switchConfig.ShortName,
                switchConfig.ButtonPin,
                switchConfig.LedPin,
                switchConfig.RelayPin,
                switchState.IsOn);
            //switchState.IsOn);

            switchControl.SetupButtonHandler();
            _switches.Add(switchControl);
        }
    }

    public IEnumerable<SwitchControl> GetAllSwitches() => _switches;

    public SwitchControl GetSwitchByTag(string tag) => _switches.FirstOrDefault(s => s.Tag == tag.ToString());

    public void ToggleSwitch(string tag)
    {
        var switchControl = GetSwitchByTag(tag);
        if (switchControl != null)
        {
            switchControl.Toggle();
            _stateManager.UpdateState(tag, switchControl.IsOn);
        }
    }

    public void TurnOnSwitch(string tag)
    {
        var switchControl = GetSwitchByTag(tag);
        if (switchControl != null)
        {
            switchControl.TurnOn();
            _stateManager.UpdateState(tag, switchControl.IsOn);
        }
    }

    public void TurnOffSwitch(string tag)
    {
        var switchControl = GetSwitchByTag(tag);
        if (switchControl != null)
        {
            switchControl.TurnOff();
            _stateManager.UpdateState(tag, switchControl.IsOn);
        }
    }
}


//using System.Collections.Generic;
//using System.Linq;

//namespace Diuna.Services
//{
//    public class SwitchService
//    {
//        private readonly ConfigManager _configManager;
//        private readonly List<SwitchControl> _switches;

//        public SwitchService()
//        {
//            _configManager = new ConfigManager();
//            _switches = new List<SwitchControl>();

//            // Initialize switches based on config
//            foreach (var switchConfig in _configManager.Switches)
//            {
//                var switchControl = new SwitchControl(
//                    switchConfig.Tag,
//                    switchConfig.Description,
//                    switchConfig.ShortName,
//                    switchConfig.ButtonPin,
//                    switchConfig.LedPin,
//                    switchConfig.RelayPin,
//                    switchConfig.IsOn
//                );
//                switchControl.SetupButtonHandler();
//                _switches.Add(switchControl);
//            }
//        }

//        public IEnumerable<SwitchControl> GetAllSwitches() => _switches;

//        public SwitchControl GetSwitchByTag(string tag) => _switches.FirstOrDefault(s => s.Tag == tag);

//        public void ToggleSwitch(string tag)
//        {
//            var switchControl = GetSwitchByTag(tag);
//            switchControl?.Toggle();
//        }

//        public void TurnOnSwitch(string tag)
//        {
//            var switchControl = GetSwitchByTag(tag);
//            switchControl?.TurnOn();
//        }

//        public void TurnOffSwitch(string tag)
//        {
//            var switchControl = GetSwitchByTag(tag);
//            switchControl?.TurnOff();
//        }

//        public void SaveConfiguration() => _configManager.SaveConfig();
//    }
//}
