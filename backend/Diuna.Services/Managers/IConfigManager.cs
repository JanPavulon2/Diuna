namespace Diuna.Services.Managers;

public interface IConfigManager
{
    List<SwitchConfig> Switches { get; set; }

    void LoadConfig();
    void SaveConfig();
    void AddOrUpdateSwitchConfig(SwitchConfig switchConfig);
}
