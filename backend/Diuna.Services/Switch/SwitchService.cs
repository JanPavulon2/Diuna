using AutoMapper;
using Diuna.Services.State;
using Diuna.Services.Gpio;
using Diuna.Services.Managers;
using Diuna.Services.Switch;
using Microsoft.AspNetCore.SignalR;
using Diuna.SignalR.Hubs;
using System.Device.Gpio;
using Microsoft.Extensions.Logging;

public class SwitchService : ISwitchService
{
    private readonly IConfigManager _configManager;
    private readonly IStateManager _stateManager;
    private readonly IHubContext<SwitchHub> _hubContext;
    private readonly IGpioService _gpioService;
    private readonly ILogger<SwitchService> _logger;

    private readonly List<SwitchControl> _switches;

    public SwitchService(
        IHubContext<SwitchHub> hubContext,
        IGpioService gpioService,
        IConfigManager configManager,
        IStateManager stateManager,
        ILogger<SwitchService> logger,
        IMapper mapper)
    {
        _hubContext = hubContext;
        _gpioService = gpioService;
        _configManager = configManager;
        _stateManager = stateManager;
        _logger = logger;
        _switches = new List<SwitchControl>();
    }

    public void Initialize()
    {
        try
        {
            _logger.LogInformation("Initializing SwitchService...");
            _configManager.LoadConfig();

            // Set up default states based on configuration
            var defaultState = _configManager.Switches.ToDictionary(
                sc => sc.Tag,
                sc => new SwitchState
                {
                    Tag = sc.Tag,
                    IsOn = false // Default state for each switch is OFF
                });

            _stateManager.LoadState(defaultState);

            foreach (var switchConfig in _configManager.Switches)
            {
                var switchState = _stateManager.GetStateByTag(switchConfig.Tag);
                var switchControl = new SwitchControl(
                    _gpioService,
                    switchConfig.Tag,
                    switchConfig.Description,
                    switchConfig.ShortName,
                    switchConfig.ButtonPin,
                    switchConfig.LedPin,
                    switchConfig.RelayPin,
                    switchState.IsOn);

                switchControl.SetupButtonHandler();
                _switches.Add(switchControl);
                _logger.LogInformation($"Switch initialized: {switchConfig.Tag} ({switchConfig.ShortName})");

            }

            _logger.LogInformation("SwitchService initialization completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during SwitchService initialization.");
            throw new ApplicationException("Failed to initialize SwitchService.", ex);

        }
    }

    public IEnumerable<SwitchControl> GetAllSwitches() => _switches;

    public SwitchControl GetSwitchByTag(string tag) => _switches.FirstOrDefault(s => s.Tag == tag.ToString());

    public async Task ToggleSwitchAsync(string tag)   
    {
        try
        {
            var switchControl = GetSwitchByTag(tag);
            var isOn = !switchControl.IsOn;
            if (switchControl != null)
            {
                switchControl.Toggle();
                _stateManager.UpdateInMemoryState(tag, isOn);
                
                _logger.LogInformation($"Switch toggled: {tag} is now {(isOn ? "ON" : "OFF")}");

                _logger.LogInformation($"[Obtaining switch: {tag} configuration]");
                var switchConfig = _configManager.Switches.FirstOrDefault(s => s.Tag == tag);
                if (switchConfig == null) 
                    throw new Exception($"Switch with tag {tag} not found in configuration.");

                _logger.LogInformation($"[Writing {tag} pin information to Raspberry]");
                
                var pinValue = isOn ? PinValue.Low : PinValue.High;
                _gpioService.WritePin(switchConfig.RelayPin, pinValue);

                pinValue = isOn ? PinValue.High : PinValue.Low;
                _gpioService.WritePin(switchConfig.LedPin, pinValue);

                _stateManager.SaveStateToFile();
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", tag, isOn);
            }
            else
            {
                _logger.LogWarning($"Switch with tag {tag} not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error toggling switch {tag}.");
            throw new ApplicationException($"Failed to toggle switch {tag}.", ex);
        }

        //
        //var switchState = _stateManager.GetStateByTag(tag);
        //bool isOn = !switchState.IsOn;
        // Find the corresponding pin configuration and update its state
        

        // Set GPIO pin value based on the toggled state
        //var pinValue = isOn ? PinValue.Low : PinValue.High;
        //_gpioService.WritePin(switchConfig.RelayPin, pinValue);

        // Update state
        //_stateManager.UpdateInMemoryState(tag, isOn);

        //await _hubContext.Clients.All.SendAsync("ReceiveMessage", tag, isOn);
        
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
            _stateManager.UpdateInMemoryState(tag, switchControl.IsOn);
        }
    }

    public void TurnOffSwitch(string tag)
    {
        var switchControl = GetSwitchByTag(tag);
        if (switchControl != null)
        {
            switchControl.TurnOff();
            _stateManager.UpdateInMemoryState(tag, switchControl.IsOn);
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
