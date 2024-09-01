namespace Diuna.Services.Managers;

public interface IConfigManager
{
    List<SwitchConfig> Switches { get; }

    void LoadConfig();
    void SaveConfig();
}
