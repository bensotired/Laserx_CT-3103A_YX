using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SolveWare_BurnInInstruments
{

    public class Yokogawa_SourceMeterBase : InstrumentBase, ISourceMeter_GS820
    {
        public Yokogawa_SourceMeterBase(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {


        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
      
        }
      
        public override void RefreshDataOnceCycle(CancellationToken token)
        {
          
        }


        public virtual bool GetIsOutputOn(Keithley2602BChannel ch)
        {
            return false;
        }
        public virtual double[] GetMeasureValues(Keithley2602BChannel ch, int points)
        {
            return null;
        }
        public virtual SourceMeterMode GetMode(Keithley2602BChannel ch)
        {
            return default(SourceMeterMode);
        }
        public virtual double[] GetSourceValues(Keithley2602BChannel ch, int points)
        {
            return null;
        }
        public virtual string IDN { get; }
        public virtual double MeasureCurrent_A(Keithley2602BChannel ch)
        {
            return 0;
        }
        public virtual void MeasureCurrentAutoRange(Keithley2602BChannel ch)
        {

        }
        public virtual double MeasureVoltage_V(Keithley2602BChannel ch)
        {
            return 0;
        }
        public virtual void MeasureVoltageAutoRange(Keithley2602BChannel ch)
        {

        }
        public virtual void Reset()
        {

        }
        public virtual void Reset(Keithley2602BChannel ch)
        {

        }
        public virtual void SetAutoRange_I_Enable(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, bool isEnable)
        {

        }
        public virtual void SetAutoRange_V_Enable(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, bool isEnable)
        {

        }
        public virtual void SetAutoZero(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, SourceMeterAutoZero autoMode)
        {

        }
        public virtual void SetComplianceCurrent_A(Keithley2602BChannel ch, double compVal)
        {

        }
        public virtual void SetComplianceVoltage_V(Keithley2602BChannel ch, double compVal)
        {

        }
        public virtual void SetCurrent_A(Keithley2602BChannel ch, double val)
        {

        }
        public virtual void SetCurrentPulseLevel(Keithley2602BChannel ch, double val)
        {

        }
        public virtual void SetIsOutputOn(Keithley2602BChannel ch, bool isEnable)
        {

        }
        public virtual void SetMeasureCurrentRange_A(Keithley2602BChannel ch, double val)
        {

        }
        public virtual void SetMeasureVoltageRange_V(Keithley2602BChannel ch, double val)
        {

        }
        public virtual void SetMode(Keithley2602BChannel ch, SourceMeterMode mode)
        {

        }
        public virtual void SetNPLC(Keithley2602BChannel ch, double val)
        {

        }
        /// <summary>
        /// 设置测量模式  4线或者2线
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="senceMode"></param>
        public virtual void SetSenceMode(Keithley2602BChannel ch, SourceMeterSenceMode senceMode)
        {

        }
        public virtual void SetSourceCurrentLimit_A(Keithley2602BChannel ch, double val)
        {

        }
        public virtual void SetSourceCurrentRange_A(Keithley2602BChannel ch, double val)
        {

        }
        public virtual void SetSourceVoltageLimit_V(Keithley2602BChannel ch, double val)
        {

        }
        public virtual void SetSourceVoltageRange_V(Keithley2602BChannel ch, double val)
        {

        }
        public virtual void SetVoltage_V(Keithley2602BChannel ch, double val)
        {

        }
        public virtual void SetVoltagePulseLevel(Keithley2602BChannel ch, double val)
        {

        }
        public virtual void Sweep(Keithley2602BChannel ch, SourceMeterMode mode, double startValue, double stopValue, int points, double delay_s)
        {

        }

        public virtual string[] SweepDualChannelsSYNC(
                           double smua_start_val,
                           double smua_stop_val,
                           double smua_step_val,
                           double smua_source_protect_volt_limit,
              
                           double smub_source_voltage_level,
                           double smub_sense_current_range,
                           double period_s,
                           double sourceDelay_s,
                           double senseDelay_s,
                           double nplc,
                           bool pulsedMode,
                           double pulseWidth
                           )
        {
            return null;
        }
        public virtual string[] SweepDualChannelsSYNC_WithRangeSettings(
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
                    )
        {
            return null;
        }

        public virtual string[] SweepNormalModeDualChannels(SourceMeterMode smua_mode,
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
                                                double trigger_delay_s)
        {
            return null;
        }
        public virtual string[] PulsedSweepDualChannels(SourceMeterMode smua_mode,
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
                                               double DutyCycle)
        {
            return null;
        }

        public virtual string[] ExternalTriggerSweep(Action startExternalTrigger, int points, double smua_nplc, double smua_delay_s)
        {
            return null;
        }
        public virtual void ConfigAuxiliaryOutput(double smua_start_val,
                                     double smua_stop_val,
                                     double smua_step_val,
                                     double voltLimit,
                                     double sourceDelay_s,
                                     double senseDelay_s,
                                     double nplc,
                                     bool pulsedMode,
                                     double pulseWidth)
        {

        }

        public virtual void OutputOn()
        {

        }

        public virtual void OutputOff()
        {

        }
    }
}