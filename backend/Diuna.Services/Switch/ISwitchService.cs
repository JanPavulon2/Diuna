using Diuna.Models.Config;

public interface ISwitchService
{
    void Initialize();
    IEnumerable<SwitchConfig> GetAllSwitches();
    SwitchConfig GetSwitchByTag(string tag);
    Task SetSwitchAsync(string tag, bool on);


    Task ToggleSwitchAsync(string tag);
    void TurnOffSwitch(string tag);
    void TurnOnSwitch(string tag);
}