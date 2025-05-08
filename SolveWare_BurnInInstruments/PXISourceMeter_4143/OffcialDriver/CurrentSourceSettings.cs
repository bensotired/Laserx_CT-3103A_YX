namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Includes parameters to configure a DC power session with current output.
    /// </summary>
    public class CurrentSourceSettings : DCSourceSettings
    {
        /// <summary>
        /// Specifies the current level range, in amps, for the specified channel(s).
        /// </summary>
        public double CurrentLevelRange { get; set; } = 0.1;

        /// <summary>
        /// Specifies the voltage limit, in volts, on the specified output channel(s).
        /// The limit is specified as a positive value, but symmetric positive and negative limits are enforced simultaneously.
        /// </summary>
        public double VoltageLimit { get; set; } = 2;

        /// <summary>
        /// Specifies the voltage limit range, in volts, on the specified channel(s).For valid ranges, refers to the Ranges topics for your device in the NI DC Power Supplies and SMUs Help.
        /// </summary>
        public double VoltageLimitRange { get; set; } = 6;

        public CurrentSourceSettings() : base()
        {
        }
    }
}
