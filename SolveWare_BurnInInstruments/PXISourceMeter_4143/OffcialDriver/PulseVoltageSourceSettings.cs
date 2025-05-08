namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Includes parameters to configure a DC power session with pulse voltage output.
    /// </summary>
    public class PulseVoltageSourceSettings : PulseSourceSettings
    {
        /// <summary>
        /// Specifies the pulse voltage level range, in volts, on the specified channel(s).
        /// </summary>
        public double PulseVoltageLevelRange { get; set; } = 6;

        /// <summary>
        /// Specifies specifies the pulse current limit, in amps, on the specified channel(s).
        /// An upper current  limit with the positive value specified by this parameter and a lower current  limit with a symmetric negative value take effect simultaneously. The valid values for this parameter are defined by the current  limit range. 
        /// </summary>
        public double PulseCurrentLimit { get; set; } = 0.1;

        /// <summary>
        /// Specifies the pulse current limit range, in amps, on the specified channel(s). Refer to the Ranges topics for your device in the NI DC Power Supplies and SMUs Help for valid current limit ranges.  
        /// </summary>
        public double PulseCurrentLimitRange { get; set; } = 1;

        /// <summary>
        /// Specifies the pulse bias current limit, in amps, on the specified channel(s).
        /// An upper current  limit with the positive value specified by this parameter and a lower current  limit with a symmetric negative value take effect simultaneously. The valid values for this parameter are defined by the current  limit range. 
        /// </summary>
        public double PulseBiasCurrentLimit { get; set; } = 0.1;

        /// <summary>
        /// Specifies the pulse bias voltage level, in volts, for the output channel(s) generation..
        /// </summary>
        public double PulseBiasVoltageLevel { get; set; } = 0;
    }
}
