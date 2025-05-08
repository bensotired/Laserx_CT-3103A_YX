using System;

namespace SolveWare_BurnInInstruments
{
    public interface ISourceMeter_GS820 : IInstrumentBase
    {
        void OutputOn();
        void OutputOff();
        bool GetIsOutputOn(Keithley2602BChannel ch);
        double[] GetMeasureValues(Keithley2602BChannel ch, int points);
        SourceMeterMode GetMode(Keithley2602BChannel ch);
        double[] GetSourceValues(Keithley2602BChannel ch, int points);
        string IDN { get; }
        double MeasureCurrent_A(Keithley2602BChannel ch);
        void MeasureCurrentAutoRange(Keithley2602BChannel ch);
        double MeasureVoltage_V(Keithley2602BChannel ch);
        void MeasureVoltageAutoRange(Keithley2602BChannel ch);
        void Reset();
        void Reset(Keithley2602BChannel ch);
        void SetAutoRange_I_Enable(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, bool isEnable);
        void SetAutoRange_V_Enable(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, bool isEnable);
        void SetAutoZero(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, SourceMeterAutoZero autoMode);
        void SetComplianceCurrent_A(Keithley2602BChannel ch, double compVal);
        void SetComplianceVoltage_V(Keithley2602BChannel ch, double compVal);
        void SetCurrent_A(Keithley2602BChannel ch, double val);
        void SetCurrentPulseLevel(Keithley2602BChannel ch, double val);
        void SetIsOutputOn(Keithley2602BChannel ch, bool isEnable);
        void SetMeasureCurrentRange_A(Keithley2602BChannel ch, double val);
        void SetMeasureVoltageRange_V(Keithley2602BChannel ch, double val);
        void SetMode(Keithley2602BChannel ch, SourceMeterMode mode);
        void SetNPLC(Keithley2602BChannel ch, double val);
        /// <summary>
        /// 设置测量模式  4线或者2线
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="senceMode"></param>
        void SetSenceMode(Keithley2602BChannel ch, SourceMeterSenceMode senceMode);
        void SetSourceCurrentLimit_A(Keithley2602BChannel ch, double val);
        void SetSourceCurrentRange_A(Keithley2602BChannel ch, double val);
        void SetSourceVoltageLimit_V(Keithley2602BChannel ch, double val);
        void SetSourceVoltageRange_V(Keithley2602BChannel ch, double val);
        void SetVoltage_V(Keithley2602BChannel ch, double val);
        void SetVoltagePulseLevel(Keithley2602BChannel ch, double val);
        void Sweep(Keithley2602BChannel ch, SourceMeterMode mode, double startValue, double stopValue, int points, double delay_s);

        string[] SweepDualChannelsSYNC(
                           double smua_start_val,
                           double smua_stop_val,
                           double smua_step_val,
                           double smua_volt_limit,
                           double smub_source_voltage_level,
                           double smub_sense_current_range,
                           double period_s,
                           double sourceDelay_s,
                           double senseDelay_s,
                           double nplc,
                           bool pulsedMode,
                           double pulseWidth
                           );

       string[] SweepDualChannelsSYNC_WithRangeSettings(
                double smua_start_val,
                double smua_stop_val,
                double smua_step_val,
                double smua_source_protect_volt_limit,
                double smua_sense_voltage_range,
                double smub_source_voltage_level,
                double smub_sense_protect_current_limit,
                double smub_sense_current_range,
                double period_s,
                double sourceDelay_s,
                double senseDelay_s,
                double nplc,
                bool pulsedMode,
                double pulseWidth
                );


        string[] SweepNormalModeDualChannels(SourceMeterMode smua_mode,
                                                double smua_source_current_range,
                                                double smua_source_current_limit,
                                                double smua_source_voltage_range,
                                                double smua_source_voltage_limit,
                                                double smua_measure_current_range,
                                                double smua_measure_voltage_range,
                                                double smua_nplc,
                                                double smua_start_val,
                                                double smua_stop_val,
                                                double smua_trigger_source_voltage_limit,
                                                double smua_trigger_source_current_limit,
                                                double smua_delay_s,

                                                SourceMeterMode smub_mode,
                                                double smub_source_current_range,
                                                double smub_source_current_limit,
                                                double smub_source_voltage_range,
                                                double smub_source_voltage_limit,
                                                double smub_measure_current_range,
                                                double smub_measure_voltage_range,
                                                double smub_nplc,
                                                double smub_start_val,
                                                double smub_stop_val,
                                                double smub_trigger_source_voltage_limit,
                                                double smub_trigger_source_current_limit,
                                                double smub_delay_s,

                                                int sweepPoints,
                                                double trigger_delay_s);
        string[] PulsedSweepDualChannels(SourceMeterMode smua_mode,
                                               double smua_source_current_range,
                                               double smua_source_current_limit,
                                               double smua_source_voltage_range,
                                               double smua_source_voltage_limit,
                                               double smua_measure_current_range,
                                               double smua_measure_voltage_range,
                                               double smua_start_val,
                                               double smua_stop_val,
                                               double smua_trigger_source_voltage_limit,
                                               double smua_trigger_source_current_limit,

                                               SourceMeterMode smub_mode,
                                               double smub_source_current_range,
                                               double smub_source_current_limit,
                                               double smub_source_voltage_range,
                                               double smub_source_voltage_limit,
                                               double smub_measure_current_range,
                                               double smub_measure_voltage_range,
                                               double smub_start_val,
                                               double smub_stop_val,
                                               double smub_trigger_source_voltage_limit,
                                               double smub_trigger_source_current_limit,

                                               int sweepPoints,
                                               double nplc_sec,
                                               double DutyRatio,
                                               double DutyCycle);

        string[] ExternalTriggerSweep(Action startExternalTrigger, int points, double smua_nplc, double smua_delay_s);
        void ConfigAuxiliaryOutput(double smua_start_val,
                                     double smua_stop_val,
                                     double smua_step_val,
                                     double voltLimit,
                                     double sourceDelay_s,
                                     double senseDelay_s,
                                     double nplc,
                                     bool pulsedMode,
                                     double pulseWidth);

    }
}