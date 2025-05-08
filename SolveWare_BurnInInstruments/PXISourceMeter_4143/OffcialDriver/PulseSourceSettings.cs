using NationalInstruments.ModularInstruments.NIDCPower;

namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Includes parameters to configure a DC power session common with pulse voltage/current output.
    /// </summary>
    public abstract class PulseSourceSettings
    {
        /// <summary>
        /// Specifies the length, in seconds, of the on phase of a pulse.
        /// </summary>
        public double PulseOnTime { get; set; } = 0.001;

        /// <summary>
        /// Specifies the length, in seconds, of the off phase of a pulse.
        /// </summary>
        public double PulseOffTime { get; set; } = 0.009;

        /// <summary>
        /// Determines when, in seconds, the device generates the Source Complete event, potentially starting a measurement if the Measure When property is set to Automatically After Source Complete.
        /// When working in Multi-Channel Synchronization mode
        /// On the primary device, this delay has to be long enough for all the secondary devices to finish programming the output and to settle. 
        /// On the secondary device(s), sets the delay to 0 so that the secondary device(s) are ready to receive the next trigger from the primary device as quickly as possible.
        /// </summary>
        public double SourceDelay { get; set; } = 0.0007;

        /// <summary>
        /// Specifies when, in seconds, the device generates the Pulse Complete event after generating the off level of a pulse
        /// </summary>
        public double PulseBiasDelay { get; set; } = 0.000;

        /// <summary>
        /// Specifies the measurement aperture time for the channel configuration which is the period during which an ADC reads the voltage or current on a power supply or SMU.        
        /// </summary>
        public double ApertureTime { get; set; } = 0.0002;

        /// <summary>
        /// Specifies local or remote sensing on the specified channel(s).
        /// Refer to the Local and Remote Sense topic in the NI DC Power Supplies and SMUs Help for more information about sensing on supported channels and about devices that support local and/or remote sensing.
        /// </summary>
        public DCPowerMeasurementSense SenseModel { get; set; } = DCPowerMeasurementSense.Local;

        /// <summary>
        /// Specifies the transient response. Refer to the Transient Response topic in the NI DC Power Supplies and SMUs Help for more information about transient response. 
        /// </summary>
        public DCPowerSourceTransientResponse TransientResponse { get; set; } = DCPowerSourceTransientResponse.Normal;
    }
}
