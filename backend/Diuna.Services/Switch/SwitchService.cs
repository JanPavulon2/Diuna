using AutoMapper;
using Diuna.Services.State;
using Diuna.Services.Gpio;
using Diuna.Services.Managers;
using Diuna.Services.Switch;
using Microsoft.AspNetCore.SignalR;
using Diuna.SignalR.Hubs;
using System.Device.Gpio;
using Microsoft.AspNetCore.Mvc;

public class SwitchService : ISwitchService
{
    private readonly IConfigManager _configManager;
    private readonly IStateManager _stateManager;
    private readonly IHubContext<SwitchHub> _hubContext;
    private readonly IGpioService _gpioService;
    
    private readonly List<SwitchControl> _switches;

    public SwitchService(
        IHubContext<SwitchHub> hubContext, 
        IGpioService gpioService, 
        IConfigManager configManager,
        IStateManager stateManager, 
        IMapper mapper)
    {
        _hubContext = hubContext;
        _gpioService = gpioService;
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

    public async Task ToggleSwitchAsync(string tag)   
    {
        //
        var switchState = _stateManager.GetStateByTag(tag);
        bool isOn = !switchState.IsOn;
        // Find the corresponding pin configuration and update its state
        var switchConfig = _configManager.Switches.FirstOrDefault(s => s.Tag == tag);
        if (switchConfig == null) throw new Exception($"Switch with tag {tag} not found in configuration.");


        // Set GPIO pin value based on the toggled state
        var pinValue = isOn ? PinValue.Low : PinValue.High;
        _gpioService.WritePin(switchConfig.RelayPin, pinValue);

        // Update state
        _stateManager.UpdateState(tag, isOn);

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", tag, isOn);
        
        // Notify all clients via SignalR
        //var switchHub = new SwitchHub(_hubContext);
        //await switchHub.BroadcastSwitchState(tag, isOn);
        //await _hubContext.Clients.All.SendAsync("SwitchStateChanged", tag, isOn);

        //await switchHub.ListConnectedClients();


        //await switchHub.BroadcastSwitchState(tag, isOn);

        // await _hubContext.Clients.All.SendAsync("SwitchStateChanged", tag, isOn);


        //if (config != null)
        //{
        //    _gpioService.WritePin(config.)
        //}
        ////
        //var switchControl = GetSwitchByTag(tag);
        //if (switchControl != null)
        //{
        //    switchControl.Toggle();
        //    _stateManager.UpdateState(tag, switchControl.IsOn);
        //}

        // await _hubContext.Clients.All.SendAsync("SwitchStateChanged", tag, switchControl.IsOn);
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
