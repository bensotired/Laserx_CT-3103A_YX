using System;
using System.Collections.Generic;
using System.Threading;
using NationalInstruments.ModularInstruments.NIDCPower;


namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Includes APIs of DCPower which can be used in Optical Transceiver Measurement.
    /// </summary>
    public static class SmuUtility
    {
        /// <summary>
        /// Use this API to create a DC power session. This API creates a session for a single channel at a time. 
        /// </summary>
        /// <param name="resourceChannelSettings">Specifies the resource name and the channel name of the device for which the session is created. </param>
        /// <returns>the created DC power session.</returns>
        public static NIDCPower CreateSession(ResourceChannelSettings resourceChannelSettings)
        {
            string fullyQualifiedResourceNames = resourceChannelSettings.ResourceName + "/" + resourceChannelSettings.ChannelName;
            return new NIDCPower(fullyQualifiedResourceNames, true, String.Empty);
        }

        /// <summary>
        /// Use this API to configure a DC voltage sequence for a DC power session. 
        /// </summary>
        /// <param name="session">Specifies the DC power session to be configured.</param>
        /// <param name="sourceConfig">Specifies parameters to configure a voltage sequence.</param>
        /// <param name="sourceSequence">Specifies the series of current levels in the voltage sequence.</param>
        public static void ConfigureDCVoltageSequence(  NIDCPower session, DCVoltageSourceSettings sourceConfig, double[] sourceSequence)
        {
            session.Outputs[""].Source.Mode = DCPowerSourceMode.Sequence;
            session.Outputs[""].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
            session.Outputs[""].Source.SetSequence(sourceSequence);
            session.Outputs[""].Source.Voltage.VoltageLevelRange = sourceConfig.VoltageLevelRange;
            session.Outputs[""].Source.Voltage.CurrentLimit = sourceConfig.CurrentLimit;
            session.Outputs[""].Source.Voltage.CurrentLimitRange = sourceConfig.CurrentLimitRange;
            session.Outputs[""].Source.Voltage.CurrentLimitAutorange = sourceConfig.CurrentLimitAutorange;
            session.Outputs[""].Source.SourceDelay = new PrecisionTimeSpan(sourceConfig.SourceDelay);

            // PowerLineFrequency default value is 60 HZ, it can change to 50
            session.Outputs[""].Measurement.PowerLineFrequency = 60;
            session.Outputs[""].Measurement.ConfigureApertureTime(sourceConfig.ApertureTime, sourceConfig.ApertureTimeUnits);
            session.Outputs[""].Source.TransientResponse = sourceConfig.TransientResponse;
            session.Outputs[""].Measurement.Sense = sourceConfig.SenseModel;
        }

        /// <summary>
        /// Use this API to configure a DC current sequence for a DC power session.
        /// </summary>
        /// <param name="session">Specifies the DC power session to be configured.</param>
        /// <param name="sourceConfig">Specifies parameters to configure a current sequence.</param>
        /// <param name="sourceSequence">Specifies the series of current levels in the current sequence.</param>
        public static void ConfigureDCCurrentSequence2(NIDCPower session, DCCurrentSourceSettings sourceConfig, double[] sourceSequence, double sourceDelays)
        {
            PrecisionTimeSpan[] sDelays = new PrecisionTimeSpan[sourceSequence.Length];
            for (int i = 0; i < sDelays.Length; i++)
            {
                sDelays[i]= new PrecisionTimeSpan(sourceDelays);
            }            

            session.Outputs[""].Source.Mode = DCPowerSourceMode.Sequence;//
            session.Outputs[""].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;
            session.Outputs[""].Source.SetSequence(sourceSequence,sDelays);//
            session.Outputs[""].Source.Current.CurrentLevelRange = sourceConfig.CurrentLevelRange;
            session.Outputs[""].Source.Current.VoltageLimit = sourceConfig.VoltageLimit;
            session.Outputs[""].Source.Current.VoltageLimitRange = sourceConfig.VoltageLimitRange;
            session.Outputs[""].Source.SourceDelay = new PrecisionTimeSpan(sourceConfig.SourceDelay);
            // PowerLineFreq""fault value is 60 HZ, it can change to 50
            session.Outputs[""].Measurement.PowerLineFrequency = 60;//
            session.Outputs[""].Measurement.ConfigureApertureTime(sourceConfig.ApertureTime, sourceConfig.ApertureTimeUnits);
            session.Outputs[""].Source.TransientResponse = sourceConfig.TransientResponse;
            session.Outputs[""].Measurement.Sense = sourceConfig.SenseModel;
        }
        public static void ConfigureDCCurrentSequence(NIDCPower session, DCCurrentSourceSettings sourceConfig, double[] sourceSequence)
        {
            session.Outputs[""].Source.Mode = DCPowerSourceMode.Sequence;//
            session.Outputs[""].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;
            session.Outputs[""].Source.SetSequence(sourceSequence);//
            session.Outputs[""].Source.Current.CurrentLevelRange = sourceConfig.CurrentLevelRange;
            session.Outputs[""].Source.Current.VoltageLimit = sourceConfig.VoltageLimit;
            session.Outputs[""].Source.Current.VoltageLimitRange = sourceConfig.VoltageLimitRange;
            session.Outputs[""].Source.SourceDelay = new PrecisionTimeSpan(sourceConfig.SourceDelay);
            // PowerLineFreq""fault value is 60 HZ, it can change to 50
            session.Outputs[""].Measurement.PowerLineFrequency = 60;//
            session.Outputs[""].Measurement.ConfigureApertureTime(sourceConfig.ApertureTime, sourceConfig.ApertureTimeUnits);
            session.Outputs[""].Source.TransientResponse = sourceConfig.TransientResponse;
            session.Outputs[""].Measurement.Sense = sourceConfig.SenseModel;
        }
        /// <summary>
        /// Use this API to configures a DC power session with pulse current sequence.
        /// </summary>
        /// <param name="session">Specifies the DC power session to be configured.</param>
        /// <param name="sourceConfig">Includes parameters to configure a pulse current source sequence</param>
        /// <param name="sourceSequence">Includes sequence array for sweeping.</param>
        public static void ConfigurePulseCurrentSequence(  NIDCPower session, PulseCurrentSourceSettings sourceConfig, double[] sourceSequence)
        {
            session.Outputs[""].Source.Mode = DCPowerSourceMode.Sequence;
            session.Outputs[""].Source.Output.Function = DCPowerSourceOutputFunction.PulseCurrent;
            session.Outputs[""].Source.SetSequence(sourceSequence);
            session.Outputs[""].Source.PulseCurrent.CurrentLevelRange = sourceConfig.PulseCurrentLevelRange;
            session.Outputs[""].Source.PulseCurrent.VoltageLimit = sourceConfig.PulseVoltageLimit;
            session.Outputs[""].Source.PulseCurrent.VoltageLimitRange = sourceConfig.PulseVoltageLimitRange;
            session.Outputs[""].Source.PulseCurrent.BiasCurrentLevel = sourceConfig.PulseBiasCurrentLevel;
            session.Outputs[""].Source.PulseCurrent.BiasVoltageLimit = sourceConfig.PulseBiasVoltageLimit;
            session.Outputs[""].Source.PulseBiasDelay = new PrecisionTimeSpan(sourceConfig.PulseBiasDelay);
            session.Outputs[""].Source.PulseOnTime = sourceConfig.PulseOnTime;
            session.Outputs[""].Source.PulseOffTime = sourceConfig.PulseOffTime;
            session.Outputs[""].Source.SourceDelay = new PrecisionTimeSpan(sourceConfig.SourceDelay);
            session.Outputs[""].Measurement.ConfigureApertureTime(sourceConfig.ApertureTime, DCPowerMeasureApertureTimeUnits.Seconds);
            session.Outputs[""].Source.TransientResponse = sourceConfig.TransientResponse;
            session.Outputs[""].Measurement.Sense = sourceConfig.SenseModel;
        }

        /// <summary>
        /// Use this API to configures a DC power session with pulse voltage sequence.
        /// </summary>
        /// <param name="session">Specifies the DC power session to be configured.</param>
        /// <param name="sourceConfig">Includes parameters to configure a pulse voltage source sequence</param>
        /// <param name="sourceSequence">Includes sequence array for sweeping.</param>
        public static void ConfigurePulseVoltageSequence(  NIDCPower session, PulseVoltageSourceSettings sourceConfig, double[] sourceSequence)
        {
            session.Outputs[""].Source.Mode = DCPowerSourceMode.Sequence;
            session.Outputs[""].Source.Output.Function = DCPowerSourceOutputFunction.PulseVoltage;
            session.Outputs[""].Source.SetSequence(sourceSequence);
            session.Outputs[""].Source.PulseVoltage.VoltageLevelRange = sourceConfig.PulseVoltageLevelRange;
            session.Outputs[""].Source.PulseVoltage.CurrentLimit = sourceConfig.PulseCurrentLimit;
            session.Outputs[""].Source.PulseVoltage.CurrentLimitRange = sourceConfig.PulseCurrentLimitRange;
            session.Outputs[""].Source.PulseVoltage.BiasVoltageLevel = sourceConfig.PulseBiasVoltageLevel;
            session.Outputs[""].Source.PulseVoltage.BiasCurrentLimit = sourceConfig.PulseBiasCurrentLimit;
            session.Outputs[""].Source.PulseBiasDelay = new PrecisionTimeSpan(sourceConfig.PulseBiasDelay);
            session.Outputs[""].Source.PulseOnTime = sourceConfig.PulseOnTime;
            session.Outputs[""].Source.PulseOffTime = sourceConfig.PulseOffTime;
            session.Outputs[""].Source.SourceDelay = new PrecisionTimeSpan(sourceConfig.SourceDelay);
            session.Outputs[""].Measurement.ConfigureApertureTime(sourceConfig.ApertureTime, DCPowerMeasureApertureTimeUnits.Seconds);
            session.Outputs[""].Source.TransientResponse = sourceConfig.TransientResponse;
            session.Outputs[""].Measurement.Sense = sourceConfig.SenseModel;
        }

        /// <summary>
        ///  Use this API to configure multi-channel synchronization signals, including triggers and events between primary and secondary channels. 
        /// </summary>
        /// <param name="primarySession">Specifies the DC power session to be configured as the primary channel for synchronization.</param>
        /// <param name="secondarySessions">Specifies the DC power session(s) to be configured as the secondary channel(s) for synchronization.</param>
        /// <param name="measureCompleteEventDelay">Specifies the Measure Complete Event Delay on the primary device, in seconds. 
        /// The primary device has to delay generating the Measure Complete event (which triggers the next step in the sequence of the primary device) until all secondary device(s) have completed measurements.</param>
        public static void ConfigureMultiChannelSynchronization(NIDCPower primarySession, NIDCPower[] secondarySessions, double measureCompleteEventDelay)
        {
            // Configures the Source trigger for primary. On the primary device, the Source trigger is disabled (None).
            // This means that the device will source without waiting for a trigger.
            // When the device starts sourcing, it will export the Source trigger.
            primarySession.Outputs[""].Triggers.SourceTrigger.Disable();

            // Configures when to take measurements for primary.
            // On the primary device, takes a measurementas soon as the source unit completes (including any source delay).
            primarySession.Outputs[""].Measurement.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete;

            // Configures the Measure Complete Event Delay for primary.
            // The primary device has to delay generating the Measure Complete event (which triggers the next step of the primary's sequence) until all secondary device(s) have completed taking measurements.
            // The amount of time a secondary takes to measure can vary based on its configuration and model.
            primarySession.Outputs[""].Events.MeasureCompleteEvent.Delay = new PrecisionTimeSpan(measureCompleteEventDelay);
            //Thread.Sleep(10);
            primarySession.Outputs[""].Control.Commit();
            
            string sourceTriggerInputTerminal = BuildFullyQualifiedTerminalName(primarySession, "SourceTrigger");
            string measureTriggerInputTerminal = BuildFullyQualifiedTerminalName(primarySession, "SourceCompleteEvent");

            int secondaryCount = secondarySessions.GetLength(0);
            for (int i = 0; i < secondaryCount; i++)
            {
                // Configures the Source trigger for secondary.  On the secondary device(s), the source  trigger is the exported Source trigger from the primary device.
                secondarySessions[i].Outputs[""].Triggers.SourceTrigger.DigitalEdge.Configure(sourceTriggerInputTerminal, DCPowerTriggerEdge.Rising);

                // On the secondary device(s), takes a measurement when the source unit on the primary device completes.
                // This is accomplished by setting Measure When to On Measure Trigger and by setting the input terminal of the Measure Trigger to be the primary device's Source Complete event.
                secondarySessions[i].Outputs[""].Measurement.MeasureWhen = DCPowerMeasurementWhen.OnMeasureTrigger;
                secondarySessions[i].Outputs[""].Triggers.MeasureTrigger.DigitalEdge.Configure(measureTriggerInputTerminal, DCPowerTriggerEdge.Rising);

                secondarySessions[i].Outputs[""].Control.Commit();
            }
        }

        /// <summary>
        /// Use this API to initiate a multi-channel measurement test and fetch the measurement results. 
        /// </summary>
        /// <param name="primarySession">Specifies the DC power session configured as the primary channel for synchronization.</param>
        /// <param name="secondarySessions">Specifies the DC power session(s) configured as the secondary channel(s) for synchronization. </param>
        /// <param name="numberOfMeasurements">Specifies the number of measurement tests whose results you want to fetch. This value should be the same as the number of steps in the sequence of the primary device.</param>
        /// <param name="timeOut">Specifies the maximum time allowed for fetching measurement result to complete, in seconds.</param>
        /// <returns>the measurement results from primary and secondary devices. </returns>
        public static DCPowerFetchResult[] InitiateMultiChannelAndFetch(  NIDCPower primarySession, NIDCPower[] secondarySessions, int numberOfMeasurements, double timeOut)
        {
            int secondaryCount = secondarySessions.GetLength(0);

            // Initiates the secondary device(s) first and then initiabuszsztes the primary device.
            // The primary device has to be initiated after the secondary device(s) so that when it exports the Source trigger the secondary device(s) are already waiting for the Source trigger.
            for (int i = 0; i < secondaryCount; i++)
            {
                secondarySessions[i].Outputs[""].Control.Initiate();
            }
            primarySession.Outputs[""].Control.Initiate();

            DCPowerFetchResult[] measuredResults = new DCPowerFetchResult[secondaryCount + 1];
            PrecisionTimeSpan timeOutSpan = new PrecisionTimeSpan(timeOut);
 
            measuredResults[0] = primarySession.Measurement.Fetch("", timeOutSpan, numberOfMeasurements);
            primarySession.Outputs[""].Control.Abort();

            for (int i = 0; i < secondaryCount; i++)
            {
                measuredResults[i + 1] = secondarySessions[i].Measurement.Fetch("", timeOutSpan, numberOfMeasurements);
                secondarySessions[i].Outputs[""].Control.Abort();
            }

            return measuredResults;
        }

        /// <summary>
        /// Use this API to reset and close primary and secondary sessions.
        /// </summary>
        /// <param name="primarySession">Specifies the DC power session configured as the primary channel for synchronization.</param>
        /// <param name="secondarySessions">Specifies the secondary DC power sessions in multi-channel synchronization mode.</param>
        public static void CloseSessions(NIDCPower primarySession, NIDCPower[] secondarySessions)
        {
            //Quickly ensures all outputs are disabled and resets the device.
            primarySession.Utility.Reset();
            primarySession.Close();

            int secondaryCount = secondarySessions.GetLength(0);
            for (int i = 0; i < secondaryCount; i++)
            {
                secondarySessions[i].Utility.Reset();
                secondarySessions[i].Close();
            }
        }

        private static string BuildFullyQualifiedTerminalName(NIDCPower session, string localTerminalName)
        {
            string resource = session.DriverOperation.IOResourceDescriptor;
            int index = resource.IndexOf('/');
            string resourceName = "";
            string channelName = "";

            if (index != -1)
            {
                resourceName = resource.Substring(0, index);
                channelName = resource.Substring(index + 1);
            }
            else
            {
                throw new Exception("BuildFullyQualifiedTerminalName failed because can't find / in IOResourceDescriptor");
            }

            string modelName = session.Instruments[resourceName].Identity.InstrumentModel;
            if (modelName.Equals("NI PXI-4132", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format("/{0}/{1}", resourceName, localTerminalName);
            }
            else
            {
                return String.Format("/{0}/Engine{1}/{2}", resourceName, channelName, localTerminalName);
            }
        }
    }
}
