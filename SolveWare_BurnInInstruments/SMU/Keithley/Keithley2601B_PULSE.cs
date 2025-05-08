using System;
using System.Threading;

namespace SolveWare_BurnInInstruments
{


    public class Keithley2601B_PULSE : Keithley2602B
    {
        //private const int defaultRespondingTime_ms = 100;
        private const int dioTriggerOutChannel = 14;
       

        public Keithley2601B_PULSE(string name, string address, IInstrumentChassis chassis)
           : base(name, address, chassis)
        {


        }
        /// <summary>
        /// 脉冲输出时需切换为Pulser,恒流恒压输出时需切换为SMU
        /// </summary>
        /// <param name="mode"></param>
        public void SwitchSourceMode(SourceMode_2601B_Pulse mode)
        {
            switch (mode)
            {
                case SourceMode_2601B_Pulse.Pulser:
                    this._chassis.TryWrite("smua.pulser.enable = smua.ENABLE");
                    break;
                case SourceMode_2601B_Pulse.SMU:
                    this._chassis.TryWrite("smua.pulser.enable = smua.DISABLE");
                    break;
            }
        }
        public double TestVr(double current_A, double compliance_V)
        {
            double Vr;
            Reset();
            SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);
            SetSourceCurrentRange_A(Keithley2602BChannel.CHA, current_A);
            SetCurrent_A(Keithley2602BChannel.CHA, current_A);
            MeasureVoltageAutoRange(Keithley2602BChannel.CHA);
            SetSourceVoltageLimit_V(Keithley2602BChannel.CHA, compliance_V);
            SetNPLC(Keithley2602BChannel.CHA, 1.0);
            SetIsOutputOn(Keithley2602BChannel.CHA, true);
            Vr = MeasureVoltage_V(Keithley2602BChannel.CHA);
            SetIsOutputOn(Keithley2602BChannel.CHA, false);
            return Vr;
        }
        public double TestIr(double voltage_V, double compliance_I)
        {
            double Ir;
            Reset();
            SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceVoltageSenceCurrent);
            SetSourceVoltageRange_V(Keithley2602BChannel.CHA, voltage_V);
            SetVoltage_V(Keithley2602BChannel.CHA, voltage_V);
            MeasureCurrentAutoRange(Keithley2602BChannel.CHA);
            SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, compliance_I);
            SetNPLC(Keithley2602BChannel.CHA, 1.0);
            SetIsOutputOn(Keithley2602BChannel.CHA, true);
          
            Ir = MeasureCurrent_A(Keithley2602BChannel.CHA);
            SetIsOutputOn(Keithley2602BChannel.CHA, false);
            return Ir;
        }
        public void SetCurrentOutput(double current_A, double compliance_V)
        {
            Reset();
            SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);
            SetSourceCurrentRange_A(Keithley2602BChannel.CHA, current_A);
            SetCurrent_A(Keithley2602BChannel.CHA, current_A);
            MeasureVoltageAutoRange(Keithley2602BChannel.CHA);
            SetSourceVoltageLimit_V(Keithley2602BChannel.CHA, compliance_V);
            SetNPLC(Keithley2602BChannel.CHA, 1.0);
            SetIsOutputOn(Keithley2602BChannel.CHA, true);
        }
        public bool PulsedSweepCurrent(double smua_start_val,
                   double smua_stop_val,
                   double smua_complianceV,
                   double measDelay_s,
                   double integratingPeriod_s,
                   int sweepPoints,
                   double pulseWidth_s,
                   double period_s
                   //Action act
            )
        {
            if (this.IsOnline == false)
            {
                return false;
            }
            //Restore instrument defaults and clear the measure buffer.
            this._chassis.TryWrite("reset()");
            //act.BeginInvoke(null, null);
        
            this._chassis.TryWrite("smua.nvbuffer1.clear()");
            this._chassis.TryWrite("smua.nvbuffer2.clear()");
            // Disable the pulser.
            this._chassis.TryWrite("smua.pulser.enable = smua.DISABLE");
            this._chassis.TryWrite($"smua.trigger.count = {sweepPoints}");
            this._chassis.TryWrite("trigger.timer[1].count = smua.trigger.count - 1");
            this._chassis.TryWrite($"trigger.timer[1].delay = {period_s}");
            this._chassis.TryWrite("trigger.timer[1].passthrough = true");
            this._chassis.TryWrite("trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID");
            this._chassis.TryWrite("smua.trigger.source.action = smua.ENABLE");
            this._chassis.TryWrite($"smua.trigger.source.lineari({smua_start_val}, {smua_stop_val}, smua.trigger.count)");
            this._chassis.TryWrite($"smua.trigger.source.pulsewidth = {pulseWidth_s}");
            this._chassis.TryWrite("smua.trigger.source.stimulus = trigger.timer[1].EVENT_ID");
            //output trigger to dio 
            this._chassis.TryWrite($"digio.trigger[{dioTriggerOutChannel}].mode = digio.TRIG_RISINGM");
            this._chassis.TryWrite($"digio.trigger[{dioTriggerOutChannel}].stimulus = trigger.timer[1].EVENT_ID");

            this._chassis.TryWrite("smua.trigger.measure.action = smua.ENABLE");
            this._chassis.TryWrite($"smua.pulser.measure.delay = {measDelay_s}");
            this._chassis.TryWrite($"smua.pulser.measure.aperture = {integratingPeriod_s}");
            this._chassis.TryWrite("smua.trigger.measure.iv(smua.nvbuffer1, smua.nvbuffer2)");
            this._chassis.TryWrite($"smua.sense = smua.SENSE_REMOTE");
            this._chassis.TryWrite($"smua.pulser.rangei = {smua_stop_val}");
   
             
            this._chassis.TryWrite($"smua.pulser.rangev = {smua_complianceV}");
            this._chassis.TryWrite($"smua.pulser.protect.sourcev = {smua_complianceV}");
            this._chassis.TryWrite($"smua.pulser.protect.sensev = {smua_complianceV}");
            this._chassis.TryWrite("smua.pulser.enable = smua.ENABLE");
            this._chassis.TryWrite("smua.source.output = smua.OUTPUT_ON");
 
    
            this._chassis.TryWrite("smua.trigger.initiate()");

            this._chassis.TryWrite("waitcomplete()");
            this._chassis.TryWrite("smua.source.output = smua.OUTPUT_OFF");

            return true;
        }

        public string[] ReadData(int sweepPoints)
        {
            int timeOut_ms = 30 * 1000;

            string[] ret = new string[4];
            string Source1Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer1.readings)", sweepPoints), timeOut_ms);
            string Source1Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer2.readings)", sweepPoints), timeOut_ms);

            //var s1c = Source1Current.Split(',');
            //var s1v = Source1Voltage.Split(',');

            ret[0] = Source1Current;
            ret[1] = Source1Voltage;
            return ret;
        }

    }
    public enum SourceMode_2601B_Pulse
    {
        Pulser,
        SMU
    }
}