namespace Diuna.Models.Config;

public class ConfigData
{
    public Settings Settings { get; set; } = new Settings();
    public List<SwitchConfig> Switches { get; set; }
}
