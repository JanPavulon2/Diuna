using Diuna.Services.Switch;

public interface ISwitchService
{
    IEnumerable<SwitchControl> GetAllSwitches();
    SwitchControl GetSwitchByTag(string tag);
    void ToggleSwitch(string tag);
    void TurnOffSwitch(string tag);
    void TurnOnSwitch(string tag);
}