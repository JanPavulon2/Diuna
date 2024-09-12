using AutoMapper;
using Diuna.Services.Gpio;
using Diuna.Services.Managers;
using Microsoft.AspNetCore.SignalR;
using Diuna.SignalR.Hubs;
using System.Device.Gpio;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Diuna.Models.Config;
using Diuna.Models.State;

namespace Diuna.Services.Switch;

public class SwitchService : ISwitchService
{
    private readonly IConfigManager _configManager;
    private readonly IStateManager _state;
    private readonly IHubContext<SwitchHub> _hubContext;
    private readonly IGpioService _gpioService;
    private readonly ILogger<SwitchService> _logger;
    private readonly IMapper _mapper;

    //private readonly List<SwitchConfig> _switches;

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
        _state = stateManager;
        _logger = logger;
        //_switches = new List<SwitchConfig>();
        _mapper = mapper;
    }

    private void InitializeSwitchPins(SwitchConfig switchConfig)
    {
        Console.WriteLine($"Initializing {switchConfig.Tag} pins. Button: {switchConfig.ButtonPin}, LED: {switchConfig.LedPin}, Relay: {switchConfig.RelayPin}.");

        _gpioService.InitializePin(switchConfig.ButtonPin, PinMode.InputPullUp);
        _gpioService.InitializePin(switchConfig.LedPin, PinMode.Output);
        _gpioService.InitializePin(switchConfig.RelayPin, PinMode.Output);
    }

    private void RegisterSwitchButtonHandler(SwitchConfig switchConfig)
    {
        Console.WriteLine($"Registering button handler for '{switchConfig.ShortName}' on GPIO {switchConfig.ButtonPin}");

        var _debounceTimer = new Stopwatch();

        _gpioService.RegisterButtonPressCallback(switchConfig.ButtonPin, async (sender, args) =>
        {
            if (_debounceTimer.ElapsedMilliseconds > _configManager.Settings.DebounceTime)
            {
                Console.WriteLine($"Button pressed on '{switchConfig.ShortName}'");

                await ToggleSwitchAsync(switchConfig.Tag);

                _debounceTimer.Restart(); // Reset the debounce timer
            }
        });

        _debounceTimer.Start();

        //_debounceTimer.Start();  // Start the debounce timer

        // Additionally, register Rising event to see when the button is released
        //_gpioController.RegisterCallbackForPinValueChangedEvent(ButtonPin, PinEventTypes.Rising, (sender, args) =>
        //{
        //    Console.WriteLine($"Button released on {NameTag}");
        //});
    }

    public void Initialize()
    {
        try
        {
            _logger.LogInformation("Initializing SwitchService...");

            _configManager.LoadConfig();

            // Initialize switches based on config and state
            foreach (var switchConfig in _configManager.SwitchesConfig)
            {
                InitializeSwitchPins(switchConfig);
                RegisterSwitchButtonHandler(switchConfig);

                var switchState = _state.GetStateByTag(switchConfig.Tag);

                SetSwitchAsync(switchConfig, switchState.IsOn);

                _logger.LogInformation($"Switch initialized: {switchConfig.Tag} ({switchConfig.ShortName})");
            }

            _logger.LogInformation("SwitchService initialized.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during SwitchService initialization.");
            throw new ApplicationException("Failed to initialize SwitchService.", ex);
        }
    }

    public IEnumerable<SwitchConfig> GetAllSwitches() => _configManager.SwitchesConfig;

    public SwitchConfig GetSwitchByTag(string tag) => _configManager.SwitchesConfig.FirstOrDefault(s => s.Tag == tag.ToString());

    //        // Additionally, register Rising event to see when the button is released
    //        //_gpioController.RegisterCallbackForPinValueChangedEvent(ButtonPin, PinEventTypes.Rising, (sender, args) =>
    //        //{
    //        //    Console.WriteLine($"Button released on {NameTag}");
    //        //});
    //    }
    public async Task SetSwitchAsync(string tag, bool on)
    {
        var switchConfig = GetSwitchByTag(tag);
        await SetSwitchAsync(switchConfig, on);
    }

    public async Task SetSwitchAsync(SwitchConfig switchConfig, bool on)
    {
        try
        {
            var currentState = _state.GetStateByTag(switchConfig.Tag);

            if (on)
            {
                _gpioService.WritePin(switchConfig.RelayPin, PinValue.Low);   // Relay active-low: ON when set to Low
                _gpioService.WritePin(switchConfig.LedPin, PinValue.High);    // LED active-high: ON when set to High
            }
            else
            {
                _gpioService.WritePin(switchConfig.RelayPin, PinValue.High);  // Relay OFF
                _gpioService.WritePin(switchConfig.LedPin, PinValue.Low);     // LED OFF
            }

            if (currentState.IsOn != on)
            {
                _state.UpdateInMemoryState(currentState.Tag, on);
                _state.SaveStateToFile();

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", switchConfig.Tag, on);
                _logger.LogInformation($"Switch toggled: {switchConfig.Tag} is now {(on ? "ON" : "OFF")}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error toggling switch {switchConfig.Tag}.");
            throw new ApplicationException($"Failed to toggle switch {switchConfig.Tag}.", ex);
        }
    }


    public async Task ToggleSwitchAsync(string tag)
    {

        var switchConfig = GetSwitchByTag(tag);
        var switchState = _state.GetStateByTag(tag);
        await SetSwitchAsync(switchConfig, !switchState.IsOn);
    }

    public void TurnOnSwitch(string tag)
    {
        //var switchConfig = _configManager.SwitchesConfig = GetSwitchByTag(tag);
        //if (switchControl != null)
        //{
        //    //switchControl.TurnOn();
        //    //_stateManager.UpdateInMemoryState(tag, switchControl.IsOn);
        //    _stateManager.SaveStateToFile();
        //}
    }

    public void TurnOffSwitch(string tag)
    {
        //var switchControl = GetSwitchByTag(tag);
        //if (switchControl != null)
        //{
        //    //switchControl.TurnOff();
        //    //_stateManager.UpdateInMemoryState(tag, switchControl.IsOn);
        //    _stateManager.SaveStateToFile();
        //}
    }

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
