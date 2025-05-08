using NationalInstruments.ModularInstruments.NIDCPower;

namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Includes parameters to configure a DC power session with DC voltage output.
    /// </summary>
    public class DCVoltageSourceSettings : DCSourceSettings
    {
        /// <summary>
        /// Specifies the voltage level range, in volts, for the specified channel(s).
        /// </summary>
        public double VoltageLevelRange { get; set; } = 6;

        /// <summary>
        /// Specifies the current limit on the specified channel.
        /// An upper current limit with the positive value specified by this parameter and a lower current limit with a symmetric negative value take effect simultaneously. The valid values for this parameter are defined by the current limit range. 
        /// </summary>
        public double CurrentLimit { get; set; } = 0.1;

        /// <summary>
        /// Specifies the current limit range, in amps, for the specified channel(s). Refer to the Ranges topics for your device in the NI DC Power Supplies and SMUs Help for valid current limit ranges. 
        /// </summary>
        public double CurrentLimitRange { get; set; } = 0.1;
        public DCPowerSourceCurrentLimitAutorange CurrentLimitAutorange { get; set; } = DCPowerSourceCurrentLimitAutorange.Off;
        public DCVoltageSourceSettings() : base()
        {
        }
    }
}
