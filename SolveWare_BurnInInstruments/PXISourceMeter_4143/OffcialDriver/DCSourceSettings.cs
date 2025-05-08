using NationalInstruments.ModularInstruments.NIDCPower;

namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Includes parameters to configure a DC power session common with DC voltage/current output.
    /// </summary>
    public abstract class DCSourceSettings
    {
        /// <summary>
        /// Specifies a period of time, in seconds, for which the device has to wait before generating the Source Complete event. A measurement automatically starts after the device generates the Source Complete event if the Measure When property is set to Automatically After Source Complete. 
        /// When the Multi-Channel Synchronization mode applies, 
        /// On the primary device, set the delay to a sufficiently long period to ensure that all the secondary devices can finish programming the output and settle.   
        /// On the secondary device(s), set the delay to 0 so that the secondary device(s) are ready to receive the next trigger from the primary device as quickly as possible.
        /// </summary>
        public double SourceDelay { get; set; } = 0.001;

        /// <summary>
        /// Specifies the units of the Aperture Time property for channel configuration. Refer to the Aperture Time topic in the NI DC Power Supplies and SMUs Help for more information about how to configure your aperture time. 
        /// </summary>
        public DCPowerMeasureApertureTimeUnits ApertureTimeUnits { get; set; } = DCPowerMeasureApertureTimeUnits.Seconds;

        /// <summary>
        /// Specifies the measurement aperture time for channel configuration. Aperture time is a period during which an ADC reads the voltage or current on a power supply or SMU.         
        /// </summary>
        public double ApertureTime { get; set; } = 0.01;

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
