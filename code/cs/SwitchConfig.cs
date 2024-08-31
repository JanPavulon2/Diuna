namespace DiunaRelayControl
{
    public partial class ConfigManager
    {
        public class SwitchConfig
        {
            public string Name { get; set; }
            public int ButtonPin { get; set; }
            public int LedPin { get; set; }
            public int RelayPin { get; set; }
            public bool IsOn { get; set; }
        }
    }
}
