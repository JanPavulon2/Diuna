using Diuna.Models.Config;

namespace Diuna.Services.Managers;

public interface IConfigManager
{
    List<SwitchConfig> SwitchesConfig { get; }
    Settings Settings { get; }

    void LoadConfig();
    void SaveConfig();
}
