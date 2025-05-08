namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Includes parameters to configure a DC power session with DC current output.
    /// </summary>
    public class DCCurrentSourceSettings : DCSourceSettings
    {
        /// <summary>
        /// Specifies the current level range, in amps, for the specified channel(s). 
        /// </summary>
        public double CurrentLevelRange { get; set; } = 0.1;

        /// <summary>
        /// Specifies the voltage limit, in volts, on the specified output channel(s).
        /// An upper voltage limit with the positive value specified by this parameter and a lower voltage limit with a symmetric negative value take effect simultaneously. The valid values for this parameter are defined by the voltage limit range. 
        /// </summary>
        public double VoltageLimit { get; set; } = 3;

        /// <summary>
        /// Specifies the voltage limit range, in volts, for the specified channel(s). Refer to the Ranges topics for your device in the NI DC Power Supplies and SMUs Help for valid voltage limit ranges.
        /// </summary>
        public double VoltageLimitRange { get; set; } = 6;

        public DCCurrentSourceSettings() : base()
        {
        }
    }
}
