namespace Diuna.Models.Config;

public class SwitchConfig
{
    public string Tag { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }

    public int ButtonPin { get; set; }
    public int LedPin { get; set; }
    public int RelayPin { get; set; }
}

