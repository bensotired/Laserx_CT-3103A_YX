namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Includes parameters to configure a DC power session with pulse current output.
    /// </summary>
    public class PulseCurrentSourceSettings : PulseSourceSettings
    {
        /// <summary>
        /// Specifies the pulse current level range, in amps, on the specified channel(s).
        /// </summary>
        public double PulseCurrentLevelRange { get; set; } = 0.1;

        /// <summary>
        /// Specifies the pulse voltage limit, in volts, on the specified output channel(s).
        /// An upper voltage limit with the positive value specified by this parameter and a lower voltage limit with a symmetric negative value take effect simultaneously. The valid values for this parameter are defined by the voltage limit range. 
        /// </summary>
        public double PulseVoltageLimit { get; set; } = 3;

        /// <summary>
        /// Specifies the pulse voltage limit, in volts, on the specified channel(s). Refer to the Ranges topics for your device in the NI DC Power Supplies and SMUs Help for valid voltage limit ranges.  
        /// </summary>
        public double PulseVoltageLimitRange { get; set; } = 6;

        /// <summary>
        /// Specifies the pulse bias voltage limit, in volts, on the specified channel(s).
        /// An upper voltage limit with the positive value specified by this parameter and a lower voltage limit with a symmetric negative value take effect simultaneously. The valid values for this parameter are defined by the voltage limit range. 
        /// </summary>
        public double PulseBiasVoltageLimit { get; set; } = 2;

        /// <summary>
        /// Specifies the pulse bias current level, in amps, on the specified channel(s).
        /// </summary>
        public double PulseBiasCurrentLevel { get; set; } = 0;
    }
}
