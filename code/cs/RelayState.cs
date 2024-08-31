namespace DiunaRelayControl
{
    // Class for storing relay states
    class RelayState
    {
        public bool R1 { get; set; } = false;
        public bool R2 { get; set; } = false;
        public bool R3 { get; set; } = false;
        public bool R4 { get; set; } = false;

        public bool GetRelayState(string relayName)
        {
            return relayName switch
            {
                "r1" => R1,
                "r2" => R2,
                "r3" => R3,
                "r4" => R4,
                _ => false
            };
        }
    }
}
