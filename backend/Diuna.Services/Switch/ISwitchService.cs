using Diuna.Services.Switch;

public interface ISwitchService
{
    void Initialize();
    IEnumerable<SwitchControl> GetAllSwitches();
    SwitchControl GetSwitchByTag(string tag);
    Task ToggleSwitchAsync(string tag);
    void TurnOffSwitch(string tag);
    void TurnOnSwitch(string tag);
}