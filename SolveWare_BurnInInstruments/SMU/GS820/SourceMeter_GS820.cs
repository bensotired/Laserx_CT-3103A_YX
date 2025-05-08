using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace SolveWare_BurnInInstruments
{

    public class SourceMeter_GS820 : Yokogawa_SourceMeterBase, ISourceMeter_GS820
    {
        const int Delay_ms = 50;
        public SourceMeter_GS820(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {

        }

        const char terminator = '\n';
        public override void OutputOn()
        {
            this._chassis.TryWrite(":OUTP ON");
        }
        public override void OutputOff()
        {
            this._chassis.TryWrite(":OUTP OFF");
        }


        public string IDN
        {
            get
            {
                if (this.IsOnline)
                {
                    var idn = this._chassis.Query("*IDN?", Delay_ms);
                    return idn;
                }
                return "Offline YokogawaGS820";
            }
        }
        public override void Reset()
        {
            if (this.IsOnline == false)
            {
                return;
            }
            this._chassis.TryWrite("*RST");
            //this.Reset(Keithley2602BChannel.CHA);
            //this.Reset(Keithley2602BChannel.CHB);
        }
        public override void Reset(Keithley2602BChannel ch)
        {

            if (this.IsOnline == false)
            {
                return;
            }
            this.Reset();
        }

        private string GetChHeader(Keithley2602BChannel ch)
        {
            string chheader = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    chheader = ":CHAN1";
                    break;
                case Keithley2602BChannel.CHB:
                    chheader = ":CHAN2";
                    break;
            }
            return chheader;
        }
        public bool GetIsOutputOn(Keithley2602BChannel ch)
        {
            if (!this.IsOnline)
            { return false; }

            var cmd = string.Format("{0}:OUTP:STAT?", GetChHeader(ch));
            var ret = this._chassis.Query(cmd, Delay_ms).TrimEnd(terminator);
            switch (ret)
            {
                case "0":
                    return false;
                    break;
                case "1":
                    return true;
                    break;
            }
            return false;
        }
        public override void SetIsOutputOn(Keithley2602BChannel ch, bool isEnable)
        {
            if (!this.IsOnline)
            { return; }
            var en = "";
            switch (isEnable)
            {
                case true:
                    en = "ON";
                    break;
                case false:
                    en = "OFF";
                    break;
            }
            var cmd = string.Format("{0}:OUTP:STAT {1}", GetChHeader(ch), en);
            this._chassis.TryWrite(cmd);
        }

        public double GetReading(Keithley2602BChannel ch)
        {
            if (this.IsOnline == false) return 0.0;
            var cmdsour = string.Format("{0}:READ?", GetChHeader(ch));
            var ret = this._chassis.Query(cmdsour, Delay_ms).TrimEnd(terminator);
            return Convert.ToDouble(ret);
        }
        public SourceMeterMode GetMode(Keithley2602BChannel ch)
        {
            if (!this.IsOnline)
            { return SourceMeterMode.Unknown; }

            var cmdsour = string.Format("{0}:SOUR:FUNC?", GetChHeader(ch));
            var retsour = this._chassis.Query(cmdsour, Delay_ms).TrimEnd(terminator);
            var cmdsens = string.Format("{0}:SENS:FUNC?", GetChHeader(ch));
            var retsens = this._chassis.Query(cmdsens, Delay_ms).TrimEnd(terminator);

            switch (retsour + retsens)
            {
                case "VOLTCURR":
                    return SourceMeterMode.SourceVoltageSenceCurrent;
                    break;
                case "CURRVOLT":
                    return SourceMeterMode.SourceCurrentSenceVoltage;
                    break;
            }
            return SourceMeterMode.Unknown;
        }
        public YOKOGAWASourceResponMode GetResponMode(Keithley2602BChannel ch)
        {
            if (!this.IsOnline)
            { return YOKOGAWASourceResponMode.Normal; }

            var cmdsour = string.Format("{0}:SOUR:RESP?", GetChHeader(ch));
            var retsour = this._chassis.Query(cmdsour, Delay_ms).TrimEnd(terminator);
            switch (retsour)
            {
                case "NORM":
                    return YOKOGAWASourceResponMode.Normal;
                    break;
                case "STAB":
                    return YOKOGAWASourceResponMode.Stable;
                    break;
            }
            return YOKOGAWASourceResponMode.Normal;
        }

        public void SetResponMode(Keithley2602BChannel ch, YOKOGAWASourceResponMode respMode)
        {
            if (!this.IsOnline)
            { return; }

            var cmdsour = "";
            switch (respMode)
            {
                case YOKOGAWASourceResponMode.Normal:

                    cmdsour = string.Format("{0}:SOUR:RESP NORM", GetChHeader(ch));
                    break;
                case YOKOGAWASourceResponMode.Stable:
                    cmdsour = string.Format("{0}:SOUR:RESP STAB", GetChHeader(ch));
                    break;
            }
            this._chassis.TryWrite(cmdsour);
        }
        public YOKOGAWASourceShapeMode GetShapeMode(Keithley2602BChannel ch)
        {
            if (!this.IsOnline)
            { return YOKOGAWASourceShapeMode.DC; }
            var sm = default(YOKOGAWASourceShapeMode);
            var cmdsour = string.Format("{0}:SOUR:SHAP?", GetChHeader(ch));
            var retsour = this._chassis.Query(cmdsour, Delay_ms).TrimEnd(terminator);
            switch (retsour)
            {
                case "DC":
                    sm = YOKOGAWASourceShapeMode.DC;
                    break;
                case "PULS":
                    sm = YOKOGAWASourceShapeMode.Pulse;
                    break;
            }
            return sm;
        }
        public void SetShapeMode(Keithley2602BChannel ch, YOKOGAWASourceShapeMode smode)
        {
            if (!this.IsOnline)
            { return; }
            var cmdsour = "";
            switch (smode)
            {
                case YOKOGAWASourceShapeMode.DC:

                    cmdsour = string.Format("{0}:SOUR:SHAP DC", GetChHeader(ch));
                    break;
                case YOKOGAWASourceShapeMode.Pulse:
                    cmdsour = string.Format("{0}:SOUR:RESP PULS", GetChHeader(ch));
                    break;
            }
            this._chassis.TryWrite(cmdsour);
        }     /// <summary>
              /// 设置输出源触发模式
              /// </summary>
              /// <param name="ch"></param>
              /// <param name="tMode"></param>
        public void SetSourceTriggerMode(Keithley2602BChannel ch, YOKOGAWA_SOURCE_TriggerMode tMode)
        {
            if (!this.IsOnline)
            { return; }

            var cmdsour = "";
            switch (tMode)
            {
                case YOKOGAWA_SOURCE_TriggerMode.AUX:
                    cmdsour = string.Format("{0}:SOUR:TRIG AUX", GetChHeader(ch));
                    break;
                case YOKOGAWA_SOURCE_TriggerMode.EXT:
                    cmdsour = string.Format("{0}:SOUR:TRIG EXT", GetChHeader(ch));
                    break;
                case YOKOGAWA_SOURCE_TriggerMode.TIM1:
                    cmdsour = string.Format("{0}:SOUR:TRIG TIM1", GetChHeader(ch));
                    break;
                case YOKOGAWA_SOURCE_TriggerMode.TIM2:
                    cmdsour = string.Format("{0}:SOUR:TRIG TIM2", GetChHeader(ch));
                    break;
                case YOKOGAWA_SOURCE_TriggerMode.SENS:
                    cmdsour = string.Format("{0}:SOUR:TRIG SENS", GetChHeader(ch));
                    break;
            }
            this._chassis.TryWrite(cmdsour);
        }
        /// <summary>
        /// 查询输出源触发模式
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public YOKOGAWA_SOURCE_TriggerMode GetSourceTriggerMode(Keithley2602BChannel ch)
        {
            if (!this.IsOnline)
            { return YOKOGAWA_SOURCE_TriggerMode.EXT; }

            var cmdsour = string.Format("{0}:SOUR:TRIG?", GetChHeader(ch));
            var ret = this._chassis.Query(cmdsour, Delay_ms).TrimEnd(terminator);
            var stmode = default(YOKOGAWA_SOURCE_TriggerMode);
            switch (ret)
            {
                case "AUX":
                    stmode = YOKOGAWA_SOURCE_TriggerMode.AUX;
                    break;
                case "EXT":
                    stmode = YOKOGAWA_SOURCE_TriggerMode.EXT;
                    break;
                case "TIM1":
                    stmode = YOKOGAWA_SOURCE_TriggerMode.TIM1;
                    break;
                case "TIM2":
                    stmode = YOKOGAWA_SOURCE_TriggerMode.TIM2;
                    break;
                case "SENS":
                    stmode = YOKOGAWA_SOURCE_TriggerMode.SENS;
                    break;
            }
            return stmode;
        }
        /// <summary>
        /// 设置扫描源触发模式
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="tMode"></param>
        public void SetSweepTriggerMode(Keithley2602BChannel ch, YOKOGAWA_SWEEP_TriggerMode tMode)
        {
            if (!this.IsOnline)
            { return; }

            var cmdsour = "";
            switch (tMode)
            {
                case YOKOGAWA_SWEEP_TriggerMode.AUX:
                    cmdsour = string.Format("{0}:SWE:TRIG AUX", GetChHeader(ch));
                    break;
                case YOKOGAWA_SWEEP_TriggerMode.EXT:
                    cmdsour = string.Format("{0}:SWE:TRIG EXT", GetChHeader(ch));
                    break;
                case YOKOGAWA_SWEEP_TriggerMode.TIM1:
                    cmdsour = string.Format("{0}:SWE:TRIG TIM1", GetChHeader(ch));
                    break;
                case YOKOGAWA_SWEEP_TriggerMode.TIM2:
                    cmdsour = string.Format("{0}:SWE:TRIG TIM2", GetChHeader(ch));
                    break;
                case YOKOGAWA_SWEEP_TriggerMode.SENS:
                    cmdsour = string.Format("{0}:SWE:TRIG SENS", GetChHeader(ch));
                    break;
            }
            this._chassis.TryWrite(cmdsour);
        }
        /// <summary>
        /// 查询输出源触发模式
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public YOKOGAWA_SWEEP_TriggerMode GetSweepTriggerMode(Keithley2602BChannel ch)
        {
            if (!this.IsOnline)
            { return YOKOGAWA_SWEEP_TriggerMode.EXT; }

            var cmdsour = string.Format("{0}:SWE:TRIG?", GetChHeader(ch));
            var ret = this._chassis.Query(cmdsour, Delay_ms).TrimEnd(terminator);
            var stmode = default(YOKOGAWA_SWEEP_TriggerMode);
            switch (ret)
            {
                case "AUX":
                    stmode = YOKOGAWA_SWEEP_TriggerMode.AUX;
                    break;
                case "EXT":
                    stmode = YOKOGAWA_SWEEP_TriggerMode.EXT;
                    break;
                case "TIM1":
                    stmode = YOKOGAWA_SWEEP_TriggerMode.TIM1;
                    break;
                case "TIM2":
                    stmode = YOKOGAWA_SWEEP_TriggerMode.TIM2;
                    break;
                case "SENS":
                    stmode = YOKOGAWA_SWEEP_TriggerMode.SENS;
                    break;
            }
            return stmode;
        }
        /// <summary>
        /// 查询测量源触发模式
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public YOKOGAWA_SENSE_TriggerMode GetSenseTriggerMode(Keithley2602BChannel ch)
        {
            if (!this.IsOnline)
            { return YOKOGAWA_SENSE_TriggerMode.SWE; }

            var cmdsour = string.Format("{0}:SENS:TRIG?", GetChHeader(ch));
            var ret = this._chassis.Query(cmdsour, Delay_ms).TrimEnd(terminator);
            var stmode = default(YOKOGAWA_SENSE_TriggerMode);
            switch (ret)
            {
                case "AUX":
                    stmode = YOKOGAWA_SENSE_TriggerMode.AUX;
                    break;
                case "SWE":
                    stmode = YOKOGAWA_SENSE_TriggerMode.SWE;
                    break;
                case "SOUR":
                    stmode = YOKOGAWA_SENSE_TriggerMode.SOUR;
                    break;
                case "TIM1":
                    stmode = YOKOGAWA_SENSE_TriggerMode.TIM1;
                    break;
                case "TIM2":
                    stmode = YOKOGAWA_SENSE_TriggerMode.TIM2;
                    break;
                case "IMM":
                    stmode = YOKOGAWA_SENSE_TriggerMode.IMM;
                    break;
            }
            return stmode;
        }
        /// <summary>
        /// 设置测量源触发模式
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="tMode"></param>
        public void SetSenseTriggerMode(Keithley2602BChannel ch, YOKOGAWA_SENSE_TriggerMode tMode)
        {
            if (!this.IsOnline)
            { return; }

            var cmdsour = "";
            switch (tMode)
            {
                case YOKOGAWA_SENSE_TriggerMode.SOUR:
                    cmdsour = string.Format("{0}:SENS:TRIG SOUR", GetChHeader(ch));
                    break;
                case YOKOGAWA_SENSE_TriggerMode.SWE:
                    cmdsour = string.Format("{0}:SENS:TRIG SWE", GetChHeader(ch));
                    break;
                case YOKOGAWA_SENSE_TriggerMode.AUX:
                    cmdsour = string.Format("{0}:SENS:TRIG AUX", GetChHeader(ch));
                    break;
                case YOKOGAWA_SENSE_TriggerMode.TIM1:
                    cmdsour = string.Format("{0}:SENS:TRIG TIM1", GetChHeader(ch));
                    break;
                case YOKOGAWA_SENSE_TriggerMode.TIM2:
                    cmdsour = string.Format("{0}:SENS:TRIG TIM2", GetChHeader(ch));
                    break;
                case YOKOGAWA_SENSE_TriggerMode.IMM:
                    cmdsour = string.Format("{0}:SENS:TRIG IMM", GetChHeader(ch));
                    break;
            }
            this._chassis.TryWrite(cmdsour);
        }
        public void SetWorkMode(Keithley2602BChannel ch, YOKOGAWASourceWorkMode wMode)
        {
            if (!this.IsOnline)
            { return; }

            var cmdsour = "";
            switch (wMode)
            {
                case YOKOGAWASourceWorkMode.FIX:
                    cmdsour = string.Format("{0}:SOUR:MODE FIX", GetChHeader(ch));
                    break;
                case YOKOGAWASourceWorkMode.LIST:
                    cmdsour = string.Format("{0}:SOUR:MODE LIST", GetChHeader(ch));
                    break;
                case YOKOGAWASourceWorkMode.SING:
                    cmdsour = string.Format("{0}:SOUR:MODE SING", GetChHeader(ch));
                    break;
                case YOKOGAWASourceWorkMode.SWE:
                    cmdsour = string.Format("{0}:SOUR:MODE SWE", GetChHeader(ch));
                    break;
            }
            this._chassis.TryWrite(cmdsour);
        }

        public YOKOGAWASourceWorkMode GetWorkMode(Keithley2602BChannel ch)
        {
            if (!this.IsOnline)
            { return YOKOGAWASourceWorkMode.FIX; }
            var sm = default(YOKOGAWASourceWorkMode);
            var cmdsour = string.Format("{0}:SOUR:MODE?", GetChHeader(ch));
            var retsour = this._chassis.Query(cmdsour, Delay_ms).TrimEnd(terminator);
            switch (retsour)
            {
                case "FIX":
                    sm = YOKOGAWASourceWorkMode.FIX;
                    break;
                case "LIST":
                    sm = YOKOGAWASourceWorkMode.LIST;
                    break;
                case "SING":
                    sm = YOKOGAWASourceWorkMode.SING;
                    break;
                case "SWE":
                    sm = YOKOGAWASourceWorkMode.SWE;
                    break;
            }
            return sm;
        }
        public void SetSenceDelay_ms(Keithley2602BChannel ch, int delay_ms)
        {
            if (!this.IsOnline)
            { return; }
            var cmdsour = "";
            if (delay_ms <= 0)
            {
                cmdsour = string.Format("{0}:SENS:DEL MIN", GetChHeader(ch));
            }
            else
            {
                cmdsour = string.Format("{0}:SENS:DEL {1}", GetChHeader(ch), Convert.ToInt16(delay_ms / 1000.0));
            }

            this._chassis.TryWrite(cmdsour);
        }


        public override double MeasureCurrent_A(Keithley2602BChannel ch)
        {
            if (!this.IsOnline)
            { return double.NaN; }

            var cmd = string.Format("{0}:MEAS?", GetChHeader(ch));
            var ret = this._chassis.Query(cmd, Delay_ms).TrimEnd(terminator);
            var val = Convert.ToDouble(ret);
            return val;
        }

        public override void MeasureCurrentAutoRange(Keithley2602BChannel ch)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SENS:CURR:RANG:AUTO ON", this.GetChHeader(ch));
            this._chassis.TryWrite(cmd);
        }

        public override double MeasureVoltage_V(Keithley2602BChannel ch)
        {
            if (!this.IsOnline)
            { return double.NaN; }

            var cmd = string.Format("{0}:MEAS?", GetChHeader(ch));
            var ret = this._chassis.Query(cmd, Delay_ms).TrimEnd(terminator);
            var val = Convert.ToDouble(ret);
            return val;
        }

        public override void MeasureVoltageAutoRange(Keithley2602BChannel ch)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SENS:VOLT:RANG:AUTO ON", this.GetChHeader(ch));
            this._chassis.TryWrite(cmd);
        }

        public override void SetAutoRange_I_Enable(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, bool isEnable)
        {
            if (this.IsOnline == false)
            {
                return;
            }

            string cmd = string.Empty;
            string smu = this.GetChHeader(ch);
            string minorCmd1 = string.Empty;
            string minorCmd2 = string.Empty;

            switch (funcMode)
            {
                case SourceMeterFuncitonMode.Source:
                    minorCmd1 = "SOUR";

                    break;
                case SourceMeterFuncitonMode.Measure:
                    minorCmd1 = "SENS";
                    break;
            }
            if (isEnable)
            {
                minorCmd2 = "CURR:RANG:AUTO ON";
            }
            else
            {
                minorCmd2 = "CURR:RANG:AUTO OFF";

            }
            //smuX.source.autorangei = smuX.AUTORANGE_OFF Disable current source autorange. 
            cmd = string.Format("{0}:{1}:{2}", smu, minorCmd1, minorCmd2);
            this._chassis.TryWrite(cmd);
        }

        public override void SetAutoRange_V_Enable(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, bool isEnable)
        {
            if (this.IsOnline == false)
            {
                return;
            }

            string cmd = string.Empty;
            string smu = this.GetChHeader(ch);
            string minorCmd1 = string.Empty;
            string minorCmd2 = string.Empty;

            switch (funcMode)
            {
                case SourceMeterFuncitonMode.Source:
                    minorCmd1 = "SOUR";

                    break;
                case SourceMeterFuncitonMode.Measure:
                    minorCmd1 = "SENS";
                    break;
            }
            if (isEnable)
            {
                minorCmd2 = "VOLT:RANG:AUTO ON";
            }
            else
            {
                minorCmd2 = "VOLT:RANG:AUTO OFF";

            }
            //smuX.source.autorangei = smuX.AUTORANGE_OFF Disable current source autorange. 
            cmd = string.Format("{0}:{1}:{2}", smu, minorCmd1, minorCmd2);
            this._chassis.TryWrite(cmd);
        }

        public override void SetAutoZero(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, SourceMeterAutoZero autoMode)
        {
            if (this.IsOnline == false)
            {
                return;
            }

            string cmd = string.Empty;
            string smu = this.GetChHeader(ch);
            string minorCmd1 = string.Empty;
            string minorCmd2 = string.Empty;

            switch (funcMode)
            {
                case SourceMeterFuncitonMode.Source:
                    minorCmd1 = "SOUR";
                    ///gs820不支持source auto zero
                    return;

                case SourceMeterFuncitonMode.Measure:
                    minorCmd1 = "SENS";
                    break;
            }
            switch (autoMode)
            {
                case SourceMeterAutoZero.AUTOZERO_OFF:
                    minorCmd2 = "ZERO:AUTO OFF";

                    break;
                case SourceMeterAutoZero.AUTOZERO_ONCE:
                case SourceMeterAutoZero.AUTOZERO_AUTO:
                    minorCmd2 = "ZERO:AUTO ON";
                    break;
            }


            cmd = string.Format("{0}:{1}:{2}", smu, minorCmd1, minorCmd2);
            this._chassis.TryWrite(cmd);
        }

        public override void SetComplianceCurrent_A(Keithley2602BChannel ch, double compVal)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SOUR:CURR:PROT:LEV {1}", this.GetChHeader(ch), compVal);
            this._chassis.TryWrite(cmd);
        }

        public override void SetComplianceVoltage_V(Keithley2602BChannel ch, double compVal)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SOUR:VOLT:PROT:LEV {1}", this.GetChHeader(ch), compVal);
            this._chassis.TryWrite(cmd);
        }

        public override void SetCurrent_A(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SOUR:CURR:LEV {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }

        public override void SetCurrentPulseLevel(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SOUR:CURR:PULS:BASE {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }



        public override void SetMeasureCurrentRange_A(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SENS:CURR:RANG {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }

        public override void SetMeasureVoltageRange_V(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SENS:VOLT:RANG {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }

        public override void SetMode(Keithley2602BChannel ch, SourceMeterMode mode)
        {
            if (!this.IsOnline)
            { return; }
            var format = "";
            switch (mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    format = "{0}:SOUR:FUNC CURR;{0}:SENS:FUNC VOLT";
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    format = "{0}:SOUR:FUNC VOLT;{0}:SENS:FUNC CURR";
                    break;
            }
            var cmd = string.Format(format, GetChHeader(ch));
            this._chassis.TryWrite(cmd);
        }
        public override void SetNPLC(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SENS:NPLC {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }

        public override void SetSenceMode(Keithley2602BChannel ch, SourceMeterSenceMode senceMode)
        {
            if (!this.IsOnline)
            { return; }
            var en4w = "";
            switch (senceMode)
            {
                case SourceMeterSenceMode.SENSE_REMOTE:
                    en4w = "{0}:SENS:REM ON";
                    break;
                case SourceMeterSenceMode.SENSE_LOCAL:
                    en4w = "{0}:SENS:REM OFF";
                    break;
                default:
                    return;
            }
            var cmd = string.Format(en4w, GetChHeader(ch));
            this._chassis.TryWrite(cmd);
        }

        public override void SetSourceCurrentLimit_A(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SOUR:CURR:PROT:LEV {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }

        public override void SetSourceCurrentRange_A(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SOUR:CURR:RANG {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }

        public override void SetSourceVoltageLimit_V(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SOUR:VOLT:PROT:LEV {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }

        public override void SetSourceVoltageRange_V(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SOUR:VOLT:RANG {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }

        public override void SetVoltage_V(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SOUR:VOLT:LEV {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }

        public override void SetVoltagePulseLevel(Keithley2602BChannel ch, double val)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SOUR:VOLT:PULS:BASE {1}", this.GetChHeader(ch), val);
            this._chassis.TryWrite(cmd);
        }
        public void SetSweepCount(Keithley2602BChannel ch, int count)
        {
            if (!this.IsOnline) return;
            var cmd = string.Format("{0}:SWE:COUN {1}", this.GetChHeader(ch), count);
            this._chassis.TryWrite(cmd);
        }
        public int GetSweepCount(Keithley2602BChannel ch)
        {
            if (!this.IsOnline) return 0;
            var cmd = string.Format("{0}:SWE:COUN?", this.GetChHeader(ch));
            var temp = this._chassis.Query(cmd, Delay_ms).TrimEnd(terminator);
            return Convert.ToInt16(temp);
        }
        public override void Sweep(Keithley2602BChannel ch, SourceMeterMode mode, double startValue, double stopValue, int points, double delay_s)
        {
            var chheader = this.GetChHeader(ch);

            //up 20210911 屏蔽这个reset 影响漏电的测试
            //this._chassis.TryWrite("*RST");
            //双重屏蔽！
            //this.Reset();

            //this._chassis.TryWrite(string.Format("{0}:SOUR:FUNC CURR",              chheader));
            this.SetMode(ch, mode);
            Thread.Sleep(5);
            //this._chassis.TryWrite(string.Format("{0}:SOUR:RANG 1A",                chheader));
            switch (mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    this.SetAutoRange_I_Enable(ch, SourceMeterFuncitonMode.Source, true); Thread.Sleep(5);
                    this.SetAutoRange_V_Enable(ch, SourceMeterFuncitonMode.Measure, true); Thread.Sleep(5);
                    this._chassis.TryWrite(string.Format("{0}:SOUR:LEV 0.0A", chheader)); Thread.Sleep(5);
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    this.SetAutoRange_V_Enable(ch, SourceMeterFuncitonMode.Source, true);
                    this.SetAutoRange_I_Enable(ch, SourceMeterFuncitonMode.Measure, true);
                    this._chassis.TryWrite(string.Format("{0}:SOUR:LEV 0.0V", chheader)); Thread.Sleep(5);
                    break;
            }

            //this._chassis.TryWrite(string.Format("{0}:SOUR:PROT:LINK ON",           chheader));
            //this._chassis.TryWrite(string.Format("{0}:SOUR:PROT:LEV 6",             chheader));
            //this._chassis.TryWrite(string.Format("{0}:SOUR:PROT:STAT ON",           chheader));

            this._chassis.TryWrite(string.Format("{0}:SOUR:DEL {1}", chheader, (delay_s <= 0) ? "MIN" : delay_s.ToString())); Thread.Sleep(5);
            this._chassis.TryWrite(string.Format("{0}:SWE:TRIG EXT", chheader)); Thread.Sleep(5);
            this._chassis.TryWrite(string.Format("{0}:SOUR:MODE SWE", chheader)); Thread.Sleep(5);

            //this._chassis.TryWrite(string.Format("{0}:SOUR:CURR:SWE:SPAC LIN", chheader));
            //this._chassis.TryWrite(string.Format("{0}:SOUR:CURR:SWE:STAR 0A", chheader));
            //this._chassis.TryWrite(string.Format("{0}:SOUR:CURR:SWE:STOP 0.5A", chheader));
            //this._chassis.TryWrite(string.Format("{0}:SOUR:CURR:SWE:STEP 0.01A", chheader));
            var tmparr = ArrayMath.CalculateArray(startValue, stopValue, points, 3); Thread.Sleep(5);
            if (tmparr.Length < 2)
            {
                this.Reset(); Thread.Sleep(5);
                return;
            }
            var stepvalue = Math.Round(tmparr[1] - tmparr[0], 6);

            this._chassis.TryWrite(string.Format("{0}:SOUR:SWE:SPAC LIN", chheader)); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:SOUR:SWE:STAR {1}", chheader, startValue)); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:SOUR:SWE:STOP {1}", chheader, stopValue)); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:SOUR:SWE:POIN {1}", chheader, points)); Thread.Sleep(20);//直接扫描点数量
            this._chassis.TryWrite(string.Format("{0}:SOUR:SWE:STEP {1}", chheader, Math.Abs(stepvalue))); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:SOUR:TRIG SENS;", chheader)); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:SENS:TRIG SOUR", chheader)); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:SENS:MODE FIX", chheader)); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:SENS ON", chheader)); Thread.Sleep(20);
            //this._chassis.TryWrite(string.Format("{0}:SENS:NPLC 1", chheader)); Thread.Sleep(20);//这里屏蔽，用外面设定的值
            this._chassis.TryWrite(string.Format("{0}:SENS:ZERO:AUTO ON", chheader)); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:SENS:TRIG SOUR", chheader)); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:SENS:DEL MIN", chheader)); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:OUTP ON", chheader)); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format("{0}:SWE:COUN 1", chheader)); Thread.Sleep(50);

            this._chassis.TryWrite(":TRAC:STAT ON;"); Thread.Sleep(20);
            this._chassis.TryWrite(string.Format(":TRAC:POIN {0};", points)); Thread.Sleep(20);
            this._chassis.TryWrite(":STAR"); Thread.Sleep(5);
            while (true)
            {
                if (this._chassis.Query(":TRAC?", Delay_ms).Contains("0"))
                {
                    break;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
            this._chassis.TryWrite(":TRAC:STAT OFF;"); Thread.Sleep(5);
            this._chassis.TryWrite(string.Format("{0}:OUTP OFF", chheader)); Thread.Sleep(5);

            this._chassis.TryWrite(string.Format(":TRAC{0}:DATA:FORM ASC;", chheader)); Thread.Sleep(5);

            //var ttm = this._chassis.Query(":TRAC:CHAN1:DATA:READ? TM");
            //var ttma = ttm.Split(',');

            //var tml = this._chassis.Query(":TRAC:CHAN1:DATA:READ? ML");
            //var tmla = tml.Split(',');
            //===========================这个是测量输出=====================
            var tmf = this._chassis.Query(string.Format(":TRAC{0}:DATA:READ? ML", chheader), Delay_ms); Thread.Sleep(5);
            var tmfa = tmf.Split(',');
            //===========================这个是测量输出=====================
            //var tdo = this._chassis.Query(":TRAC:CHAN1:DATA:READ? DO");
            //var tdoa = tdo.Split(',');

            //var tdi = this._chassis.Query(":TRAC:CHAN1:DATA:READ? DI");
            //var tdia = tdi.Split(',');

            //var tsf = this._chassis.Query(":TRAC:CHAN1:DATA:READ? SF");
            //var tsfa = tsf.Split(',');
            //===========================这个是源输出=====================
            var tsl = this._chassis.Query(string.Format(":TRAC{0}:DATA:READ? SL", chheader), Delay_ms); Thread.Sleep(5);
            var tsla = tsl.Split(',');
            //===========================这个是源输出=====================
            //var tlc = this._chassis.Query(":TRAC:CHAN1:DATA:READ? LC");
            //var tlca = tlc.Split(',');

            //var thc = this._chassis.Query(":TRAC:CHAN1:DATA:READ? HC");
            //var thca = thc.Split(',');

            //var tcp = this._chassis.Query(":TRAC:CHAN1:DATA:READ? CP");
            //var tcpa = tcp.Split(',');

        }
        public double[] GetMeasureValues(Keithley2602BChannel ch, int points)
        {
            var tmf = this._chassis.Query(string.Format(":TRAC{0}:DATA:READ? ML", this.GetChHeader(ch)), Delay_ms);
            var tmfa = tmf.Split(',');
            List<double> ret = new List<double>();
            for (int i = 0; i < tmfa.Length; i++)
            {
                ret.Add(Convert.ToDouble(tmfa[i]));
            }
            return ret.ToArray();
        }
        public double[] GetSourceValues(Keithley2602BChannel ch, int points)
        {
            var tsl = this._chassis.Query(string.Format(":TRAC{0}:DATA:READ? SL", this.GetChHeader(ch)), Delay_ms);
            var tsla = tsl.Split(',');
            List<double> ret = new List<double>();
            for (int i = 0; i < tsla.Length; i++)
            {
                ret.Add(Convert.ToDouble(tsla[i]));
            }
            return ret.ToArray();
        }

        /// <summary>
        /// 操
        /// </summary>

        public string[] SweepDualChannelsUsingTimer(SourceMeterMode smua_mode,
                                                   double smua_source_current_range,
                                                   double smua_source_current_limit,
                                                   double smua_source_voltage_range,
                                                   double smua_source_voltage_limit,
                                                   double smua_measure_current_range,
                                                   double smua_measure_voltage_range,
                                                   double smua_nplc_s,
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
                                                   double smub_nplc_s,
                                                   double smub_start_val,
                                                   double smub_stop_val,
                                                   double smub_trigger_source_voltage_limit,
                                                   double smub_trigger_source_current_limit,
                                                   double smub_delay_s,

                                                   int sweepPoints,
                                                    bool pulsedMode,                                 //脉冲开关
                                                    double DutyRatio,                               //占空比
                                                    double TimerPeriod_s                                //Timer时间
                                                                                                        //double pulseWidth,
                                                                                                        //double pulsePeriod,   //dc sweep minimal pulsePeriod value is 0.01 (10ms)
                                                                                                        //double nplcT
            )
        {
            return null;
        }


        public string[] SweepDualChannelsUsingTimer(
                                                   SourceMeterMode smua_mode,
                                                   double smua_source_current_range,
                                                   double smua_source_current_limit,
                                                   double smua_source_voltage_range,
                                                   double smua_source_voltage_limit,
                                                   double smua_measure_current_range,
                                                   double smua_measure_voltage_range,
                                                   //double smua_nplc,
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
                                                   //double smub_nplc,
                                                   double smub_start_val,
                                                   double smub_stop_val,
                                                   double smub_trigger_source_voltage_limit,
                                                   double smub_trigger_source_current_limit,
                                                   double smub_delay_s,
                                                   int sweepPoints,
                                                   bool pulsedMode,
                                                   double pulseWidth,
                                                   double pulsePeriod,   //dc sweep minimal pulsePeriod value is 0.01 (10ms)
                                                   double nplcT
            )


        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            this._chassis.TryWrite("*RST");
            this._chassis.TryWrite(":SYNC:CHAN ON");
            SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
            SetSenceMode(Keithley2602BChannel.CHB, SourceMeterSenceMode.SENSE_LOCAL);
            #region ch A config
            var source_A_Values = ArrayMath.CalculateArray(smua_start_val, smua_stop_val, sweepPoints, 6);

            //this._chassis.TryWrite(":CHAN1:SOUR:VOLT:SWE:SPAC LIN");

            switch (smua_mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    {
                        this._chassis.TryWrite(":CHAN1:SOUR:FUNC CURR");
                        this._chassis.TryWrite(":CHAN1:SENS:FUNC VOLT");

                        this._chassis.TryWrite(":CHAN1:SOUR:CURR:RANG:AUTO OFF");
                        this._chassis.TryWrite(":CHAN1:SENS:VOLT:RANG:AUTO OFF");

                        this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STAR {0}A", smua_start_val));
                        this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STOP {0}A", smua_stop_val));
                        this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STEP {0}A", source_A_Values[1] - source_A_Values[0]));
                    }
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    {
                        this._chassis.TryWrite(":CHAN1:SOUR:FUNC VOLT");
                        this._chassis.TryWrite(":CHAN1:SENS:FUNC CURR");

                        this._chassis.TryWrite(":CHAN1:SOUR:VOLT:RANG:AUTO OFF");
                        this._chassis.TryWrite(":CHAN1:SENS:CURR:RANG:AUTO OFF");

                        this._chassis.TryWrite(string.Format(":CHAN1:SOUR:VOLT:SWE:STAR {0}V", smua_start_val));
                        this._chassis.TryWrite(string.Format(":CHAN1:SOUR:VOLT:SWE:STOP {0}V", smua_stop_val));
                        this._chassis.TryWrite(string.Format(":CHAN1:SOUR:VOLT:SWE:STEP {0}V", source_A_Values[1] - source_A_Values[0]));
                    }
                    break;
            }

            this._chassis.TryWrite(":CHAN1:SWE:TRIG EXT");
            this._chassis.TryWrite(":CHAN1:SWE:COUN 1;");
            this._chassis.TryWrite(":CHAN1:SOUR:MODE SWE");
            this._chassis.TryWrite(":CHAN1:SOUR:TRIG TIMER1");
            this._chassis.TryWrite(":CHAN1:SENS:TRIG SOUR");
            double nplc = 1;
            if (pulsedMode)
            {
                nplc = pulseWidth / 0.02;
                if (nplc > 1)
                {
                    nplc = 1;
                }
                else if (nplc > 0.1)
                {
                    nplc = 0.1;
                }
                else if (nplc > 0.01)
                {
                    nplc = 0.01;
                }
                else if (nplc > 0.001)
                {
                    nplc = 0.001;
                }
                else
                {
                    nplc = 0.0001;
                }
                this._chassis.TryWrite(string.Format(":CHAN1:SENS:NPLC {0}", nplc));
                this._chassis.TryWrite(string.Format(":CHAN2:SENS:NPLC {0}", nplc));
            }
            else
            {
                nplc = pulsePeriod / 2.5 / 0.02;
                //pulsePeriod = nplcT * 0.02 * 2;

                this._chassis.TryWrite(string.Format(":CHAN1:SENS:NPLC {0}", nplc));
                this._chassis.TryWrite(string.Format(":CHAN2:SENS:NPLC {0}", nplc));

                //if (nplc > 1)
                //{
                //    nplc = 1;
                //}
                //else if (nplc > 0.1)
                //{
                //    nplc = 0.1;
                //}
                //else if (nplc > 0.01)
                //{
                //    nplc = 0.01;
                //}
                //else if (nplc > 0.001)
                //{
                //    nplc = 0.001;
                //}
                //else
                //{
                //    nplc = 0.0001;
                //}
            }
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:DEL {0}", "MIN"));
            //this._chassis.TryWrite(string.Format(":CHAN1:SENS:DEL {0}", "MIN"));
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:DEL {0}", "0.001"));//20210330 Sense delay 改为1ms -Jerry

            if (pulsedMode)
            {
                this._chassis.TryWrite(":CHAN1:SOUR:SHAP PULSE");
                this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:PULSE:WIDTH {0}", pulseWidth));
            }
            this._chassis.TryWrite(string.Format(":TRIGger:TIMer1 {0:0.000}", pulsePeriod));//0.02
            this._chassis.TryWrite(":CHAN1:SOUR:CURR:PROT ON");
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:PROT:LEV {0}A", smua_source_current_limit));
            this._chassis.TryWrite(":CHAN1:SOUR:VOLT:PROT ON");
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:VOLT:PROT:LEV {0}V", smua_source_voltage_limit));

            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:RANG {0}", smua_source_current_range));
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:VOLT:RANG {0}", smua_source_voltage_range));
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:CURR:RANG {0}", smua_measure_current_range));
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:VOLT:RANG {0}", smua_measure_voltage_range));
            #endregion

            #region ch B config
            var isSource_B_DoSweep = smub_start_val == smub_stop_val ? false : true;

            this._chassis.TryWrite(":CHAN2:SENS:TRIG TIMer1");

            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:DEL {0}", "MIN"));
            //this._chassis.TryWrite(string.Format(":CHAN2:SENS:DEL {0}", "MIN"));
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:DEL {0}", "0.001"));//20210330 Sense delay 改为1ms -Jerry

            this._chassis.TryWrite(":CHAN2:SOUR:CURR:PROT ON");
            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:CURR:PROT:LEV {0}A", smub_source_current_limit));
            this._chassis.TryWrite(":CHAN2:SOUR:VOLT:PROT ON");
            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:VOLT:PROT:LEV {0}V", smub_source_voltage_limit));
            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:VOLT:RANG {0}", smub_source_voltage_range));
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:CURR:RANG {0}", smub_measure_current_range));

            if (isSource_B_DoSweep)
            {
                var source_B_Values = ArrayMath.CalculateArray(smub_start_val, smub_stop_val, sweepPoints, 6);
                //多值
                this._chassis.TryWrite(":CHAN2:SWE:TRIG SENS");
                this._chassis.TryWrite(":CHAN2:SWE:COUN 1;");
                this._chassis.TryWrite(":CHAN2:SOUR:MODE SWE");
                this._chassis.TryWrite(":CHAN2:SOUR:CURR:SWE:SPAC LIN");
                switch (smub_mode)
                {


                    case SourceMeterMode.SourceCurrentSenceVoltage:
                        {
                            this._chassis.TryWrite(":CHAN2:SOUR:FUNC CURR");
                            this._chassis.TryWrite(":CHAN2:SENS:FUNC VOLT");

                            this._chassis.TryWrite(":CHAN2:SOUR:CURR:RANG:AUTO OFF");
                            this._chassis.TryWrite(":CHAN2:SENS:VOLT:RANG:AUTO OFF");

                            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:CURR:SWE:STAR {0}A", smub_start_val));
                            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:CURR:SWE:STOP {0}A", smub_stop_val));
                            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:CURR:SWE:STEP {0}A", source_B_Values[1] - source_B_Values[0]));
                        }
                        break;
                    case SourceMeterMode.SourceVoltageSenceCurrent:
                        {
                            this._chassis.TryWrite(":CHAN2:SOUR:FUNC VOLT");
                            this._chassis.TryWrite(":CHAN2:SENS:FUNC CURR");

                            this._chassis.TryWrite(":CHAN2:SOUR:VOLT:RANG:AUTO OFF");
                            this._chassis.TryWrite(":CHAN2:SENS:CURR:RANG:AUTO OFF");

                            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:VOLT:SWE:STAR {0}V", smua_start_val));
                            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:VOLT:SWE:STOP {0}V", smua_stop_val));
                            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:VOLT:SWE:STEP {0}V", source_B_Values[1] - source_B_Values[0]));
                        }
                        break;
                }

            }
            else
            {
                //单值
                this._chassis.TryWrite(":CHAN2:SOUR:MODE SING");
                switch (smub_mode)
                {
                    case SourceMeterMode.SourceCurrentSenceVoltage:
                        {
                            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:CURR:LEV {0}A", smub_start_val));
                        }
                        break;
                    case SourceMeterMode.SourceVoltageSenceCurrent:
                        {

                            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:VOLT:LEV {0}V", smub_start_val));
                        }
                        break;
                }
            }

            #endregion


            //等待start信号
            this._chassis.TryWrite(":OUTP ON");
            this._chassis.TryWrite(":TRIGger:HOLD 1");
            this._chassis.TryWrite(":TRIGger:TSYNc");
            //开启trace存储
            this._chassis.TryWrite(":TRAC:STAT ON;");
            this._chassis.TryWrite(string.Format(":TRAC:POIN {0};", sweepPoints));
            this._chassis.TryWrite(":TRAC:CHAN1:DATA:FORM BIN;");
            this._chassis.TryWrite(":TRAC:CHAN2:DATA:FORM BIN;");
            //释放start信号
            Thread.Sleep(500);
            this._chassis.TryWrite(":TRIGger:HOLD 0");
            this._chassis.TryWrite(":STAR");

            //Thread.Sleep((int)(smua_nplc * 20 * sweepPoints+2000));
            while (true)
            {
                string resp = this._chassis.Query(":TRAC?", Delay_ms);
                if (resp.Contains("0"))
                {
                    break;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }

            this._chassis.TryWrite(":TRAC:STAT OFF;"); Thread.Sleep(5);

            this._chassis.TryWrite(":OUTP OFF");
            #region CH1 Source Values
            this._chassis.TryWrite(":TRAC:CHAN1:DATA:READ? SL");
            Thread.Sleep(100);
            byte[] ch1SourceBytes = ((NiVisaChassis)this._chassis).ReadByteArray();
            double[] ch1SourceValues = new double[(ch1SourceBytes.Length - 11) / 8];
            StringBuilder ch1SourceStringBuilder = new StringBuilder();
            for (int i = 0; i < (ch1SourceBytes.Length - 11) / 8; i++)
            {
                byte[] value = new byte[8];
                Array.Copy(ch1SourceBytes, 10 + i * 8, value, 0, 8);
                Array.Reverse(value);
                ch1SourceValues[i] = BitConverter.ToDouble(value, 0);
                ch1SourceStringBuilder.Append(ch1SourceValues[i]);
                if (i != (ch1SourceBytes.Length - 11) / 8 - 1)
                {
                    ch1SourceStringBuilder.Append(",");
                }
            }
            #endregion
            #region CH1 Measure Values
            this._chassis.TryWrite(":TRAC:CHAN1:DATA:READ? ML");
            Thread.Sleep(100);
            byte[] ch1MeasureBytes = ((NiVisaChassis)this._chassis).ReadByteArray();
            double[] ch1MeasureValues = new double[(ch1MeasureBytes.Length - 11) / 8];
            StringBuilder ch1MeasureStringBuilder = new StringBuilder();
            for (int i = 0; i < (ch1MeasureBytes.Length - 11) / 8; i++)
            {
                byte[] value = new byte[8];
                Array.Copy(ch1MeasureBytes, 10 + i * 8, value, 0, 8);
                Array.Reverse(value);
                ch1MeasureValues[i] = BitConverter.ToDouble(value, 0);
                ch1MeasureStringBuilder.Append(ch1MeasureValues[i]);
                if (i != (ch1MeasureBytes.Length - 11) / 8 - 1)
                {
                    ch1MeasureStringBuilder.Append(",");
                }
            }
            #endregion
            #region CH2 Source Values
            this._chassis.TryWrite(":TRAC:CHAN2:DATA:READ? SL");
            Thread.Sleep(100);
            byte[] ch2SourceBytes = ((NiVisaChassis)this._chassis).ReadByteArray();
            double[] ch2SourceValues = new double[(ch1SourceBytes.Length - 11) / 8];
            StringBuilder ch2SourceStringBuilder = new StringBuilder();
            for (int i = 0; i < (ch2SourceBytes.Length - 11) / 8; i++)
            {
                byte[] value = new byte[8];
                Array.Copy(ch2SourceBytes, 10 + i * 8, value, 0, 8);
                Array.Reverse(value);
                ch2SourceValues[i] = BitConverter.ToDouble(value, 0);
                ch2SourceStringBuilder.Append(ch2SourceValues[i]);
                if (i != (ch2SourceBytes.Length - 11) / 8 - 1)
                {
                    ch2SourceStringBuilder.Append(",");
                }
            }
            #endregion
            #region CH2 Measure Values
            this._chassis.TryWrite(":TRAC:CHAN2:DATA:READ? ML");
            Thread.Sleep(100);
            byte[] ch2MeasureBytes = ((NiVisaChassis)this._chassis).ReadByteArray();
            double[] ch2MeasureValues = new double[(ch2MeasureBytes.Length - 11) / 8];
            StringBuilder ch2MeasureStringBuilder = new StringBuilder();
            for (int i = 0; i < (ch2MeasureBytes.Length - 11) / 8; i++)
            {
                byte[] value = new byte[8];
                Array.Copy(ch2MeasureBytes, 10 + i * 8, value, 0, 8);
                Array.Reverse(value);
                ch2MeasureValues[i] = BitConverter.ToDouble(value, 0);
                ch2MeasureStringBuilder.Append(ch2MeasureValues[i]);
                if (i != (ch2MeasureBytes.Length - 11) / 8 - 1)
                {
                    ch2MeasureStringBuilder.Append(",");
                }
            }

            #endregion
            string[] results = new string[4];
            results[0] = ch1SourceStringBuilder.ToString();
            results[1] = ch1MeasureStringBuilder.ToString();
            results[2] = ch2MeasureStringBuilder.ToString();
            results[3] = ch2SourceStringBuilder.ToString();

            ////===========================这个是测量输出=====================

            //var sourA_MesuVals = this._chassis.Query(":TRAC:CHAN1:DATA:READ? ML"); Thread.Sleep(5);
            //var tmfa = sourA_MesuVals.Split(',');
            ////===========================这个是源输出=====================
            //var sourA_SourVals = this._chassis.Query(":TRAC:CHAN1:DATA:READ? SL"); Thread.Sleep(5);
            //var tsla = sourA_SourVals.Split(',');

            ////===========================这个是测量输出=====================
            //var sourB_MesuVals = this._chassis.Query(":TRAC:CHAN2:DATA:READ? ML"); Thread.Sleep(5);
            //var tmfa1 = sourB_MesuVals.Split(',');
            ////===========================这个是源输出=====================
            //var sourB_SourVals = this._chassis.Query(":TRAC:CHAN2:DATA:READ? SL"); Thread.Sleep(5);
            //var tsla1 = sourB_SourVals.Split(',');


            //results[0] = sourA_SourVals;
            //results[1] = sourA_MesuVals;
            //results[2] = sourB_MesuVals;
            //results[3] = sourB_SourVals;

            return results;
        }
        public void SetSourceDelay_s(Keithley2602BChannel ch, double sourceDelay_s)
        {
            string chStr = ch == Keithley2602BChannel.CHA ? "CHAN1" : "CHAN2";
            if (sourceDelay_s == 0)
            {
                this._chassis.TryWrite(string.Format(@":{0}:SOUR:DEL {1}", chStr, "MIN"));   //设置通道1输出延迟为最小值，王礼霖给的值是0.002，可配置
            }
            else
            {
                this._chassis.TryWrite(string.Format(@":{0}:SOUR:DEL {1}", chStr, sourceDelay_s));
            }
        }
        public void SetSenceDelay_s(Keithley2602BChannel ch, double senseDelay_s)
        {
            string chStr = ch == Keithley2602BChannel.CHA ? "CHAN1" : "CHAN2";
            if (senseDelay_s == 0)
            {
                this._chassis.TryWrite(string.Format(":{0}:SENS:DEL {1}", chStr, "MIN"));   //设置通道1输出延迟为最小值，王礼霖给的值是0.002，可配置
            }
            else
            {
                this._chassis.TryWrite(string.Format(":{0}:SENS:DEL {1}", chStr, senseDelay_s));
            }
        }
        public override void ConfigAuxiliaryOutput(double start_val,
                                   double stop_val,
                                   double step_val,
                                   double voltLimit,
                                   double sourceDelay_s,
                                   double senseDelay_s,
                                   double nplc,
                                   bool pulsedMode,
                                   double pulseWidth)
        {
            this._chassis.TryWrite("*RST");            //重置仪表
            this._chassis.TryWrite(":SYNC:MODE SLAVe");
            this._chassis.TryWrite(":SYNC:CHAN ON");   //设置通道同步         
            #region CH A config
            this._chassis.TryWrite(":CHAN1:SOUR:FUNC CURR");            //设置通道1输出电流
            this._chassis.TryWrite(":CHAN1:SOUR:CURR:RANG:AUTO OFF");    //关闭通道1输出自动量程
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:RANG {0}A", stop_val));         //设置通道1电流输出量程适应扫描输出最大值
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:PROT:LINK ON"));
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:CURR:LEV {0}", start_val));
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:VOLT:RANG:AUTO OFF"));
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:VOLT:RANG {0}V", voltLimit));
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:VOLT:PROT:LEV {0}V", voltLimit));
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:PROT:STAT ON"));
            if (sourceDelay_s == 0)
            {
                this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:DEL {0}", "MIN"));   //设置通道1输出延迟为最小值，王礼霖给的值是0.002，可配置
            }
            else
            {
                this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:DEL {0}", sourceDelay_s));
            }
            this._chassis.TryWrite(":CHAN1:SOUR:MODE SWE");             //设置通道1输出模式为扫描
            this._chassis.TryWrite(":CHAN1:SOUR:CURR:SWE:SPAC LIN");    //设置通道1扫描方式为线性扫描
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STEP {0}A", step_val)); //设置通道1扫描步进0.001A
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STAR {0}A", start_val));     //设置通道1扫描起始电流0A
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STOP {0}A", stop_val));   //设置通道1扫描起始电流1.2A       
            this._chassis.TryWrite(":CHAN1:SENS:REM ON");                //设置通道1远端测量（4线法）
            this._chassis.TryWrite(":CHAN1:SENS:FUNC VOLT");             //设置通道1测量电压
            this._chassis.TryWrite(":CHAN1:SENS:VOLT:RANG:AUTO OFF");   //关闭通道1测量自动量程
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:VOLT:RANG {0}V", voltLimit));         //设置通道1电压测量量程2V
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:NPLC {0}", nplc));               //设置通道1测量NPLC，王礼霖给的值是0.05，可配置
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:DEL {0}", senseDelay_s));              //设置通道1测量延迟为最小值，王礼霖给的值是0.001，可配置
            if (pulsedMode)
            {
                this._chassis.TryWrite(":CHAN1:SOUR:SHAP PULSE");
                this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:PULSE:WIDTH {0}", pulseWidth));
                this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:PULSE:BASE {0}", "0.01"));
            }
            #endregion
            #region CH B config
            this._chassis.TryWrite(":CHAN2:SOUR:FUNC CURR");            //设置通道2输出电流
            this._chassis.TryWrite(":CHAN2:SOUR:CURR:RANG:AUTO OFF");    //关闭通道2输出自动量程
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:CURR:RANG {0}A", stop_val));         //设置通道2电流输出量程适应扫描输出最大值
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:PROT:LINK ON"));
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:CURR:LEV {0}", start_val));
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:VOLT:RANG:AUTO OFF"));
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:VOLT:RANG {0}V", voltLimit));
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:VOLT:PROT:LEV {0}V", voltLimit));
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:PROT:STAT ON"));
            if (sourceDelay_s == 0)
            {
                this._chassis.TryWrite(string.Format(@"CHAN2:SOUR:DEL {0}", "MIN"));   //设置通道2输出延迟为最小值，王礼霖给的值是0.002，可配置
            }
            else
            {
                this._chassis.TryWrite(string.Format(@"CHAN2:SOUR:DEL {0}", sourceDelay_s));
            }

            this._chassis.TryWrite(":CHAN2:SOUR:MODE SWE");             //设置通道2输出模式为扫描
            this._chassis.TryWrite(":CHAN2:SOUR:CURR:SWE:SPAC LIN");    //设置通道2扫描方式为线性扫描
            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:CURR:SWE:STEP {0}A", step_val)); //设置通道2扫描步进0.001A
            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:CURR:SWE:STAR {0}A", start_val));     //设置通道2扫描起始电流0A
            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:CURR:SWE:STOP {0}A", stop_val));   //设置通道2扫描起始电流1.2A       
            this._chassis.TryWrite(":CHAN2:SENS:REM ON");                //设置通道2远端测量（4线法）
            this._chassis.TryWrite(":CHAN2:SENS:FUNC VOLT");             //设置通道2测量电压
            this._chassis.TryWrite(":CHAN2:SENS:VOLT:RANG:AUTO OFF");   //关闭通道2测量自动量程
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:VOLT:RANG {0}V", voltLimit));         //设置通道2电压测量量程2V
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:NPLC {0}", nplc));               //设置通道2测量NPLC，王礼霖给的值是0.05，可配置
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:DEL {0}", senseDelay_s));              //设置通道2测量延迟为最小值，王礼霖给的值是0.001，可配置
            if (pulsedMode)
            {
                this._chassis.TryWrite(":CHAN2:SOUR:SHAP PULSE");
                this._chassis.TryWrite(string.Format(":CHAN2:SOUR:CURR:PULSE:WIDTH {0}", pulseWidth));
                this._chassis.TryWrite(string.Format(":CHAN2:SOUR:CURR:PULSE:BASE {0}", "0.01"));
            }
            #endregion
            this._chassis.TryWrite(":OUTP ON");                         //打开输出
            Thread.Sleep(1000);
            this._chassis.TryWrite(":STAR");                            //开启扫描
            Thread.Sleep(1000);
        }
        public override string[] SweepDualChannelsSYNC(
                                   double smua_start_val,
                                   double smua_stop_val,
                                   double smua_step_val,
                                   double smua_voltLimit,
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
            this._chassis.TryWrite("*RST");            //重置仪表
            Thread.Sleep(Delay_ms);
            this._chassis.TryWrite(":SYNC:MODE MASTer");
            this._chassis.TryWrite(":SYNC:CHAN ON");                    //设置通道同步         
            this._chassis.TryWrite(string.Format(":TRIGger:TIMer1 {0}", period_s));             //设置时钟1触发周期

            int sweepPoints = ArrayMath.CalculateArray(smua_start_val, smua_stop_val, smua_step_val).Length;
            //#region CH1 config
            #region Source
            this._chassis.TryWrite(":CHAN1:SOUR:FUNC CURR");            //设置通道1输出电流
            this._chassis.TryWrite(":CHAN1:SOUR:CURR:RANG:AUTO OFF");    //关闭通道1输出自动量程
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:RANG {0}A", smua_stop_val));         //设置通道1电流输出量程适应扫描输出最大值
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:PROT:LINK ON"));
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:CURR:LEV {0}", smua_start_val));
            //this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:VOLT:RANG:AUTO OFF"));
            //this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:VOLT:RANG {0}V", smua_voltLimit));
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:VOLT:PROT:LEV {0}V", smua_voltLimit));//10
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:PROT:STAT ON"));
            if (sourceDelay_s == 0)
            {
                this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:DEL {0}", "MIN"));   //设置通道1输出延迟为最小值，王礼霖给的值是0.002，可配置
            }
            else
            {
                this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:DEL {0}", sourceDelay_s));
            }

            #endregion
            #region Sweep
            this._chassis.TryWrite(":CHAN1:SOUR:MODE SWE");             //设置通道1输出模式为扫描
            this._chassis.TryWrite(":CHAN1:SOUR:CURR:SWE:SPAC LIN");    //设置通道1扫描方式为线性扫描
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STEP {0}A", smua_step_val)); //设置通道1扫描步进0.001A
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STAR {0}A", smua_start_val));     //设置通道1扫描起始电流0A
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STOP {0}A", smua_stop_val));   //设置通道1扫描起始电流1.2A
            //this._chassis.TryWrite(":CHAN1:SWE:COUN 1;");       --        //设置通道1扫描次数为1
            this._chassis.TryWrite(":CHAN1:SWE:TRIG EXT");              //设置通道1扫描开始的触发信号来自外部，即通过Start指令
            this._chassis.TryWrite(":CHAN1:SOUR:TRIG TIM1");            //设置通道1输出的触发源为时钟1
            //this._chassis.TryWrite(":CHAN1:SENS:TRIG SOUR");    --        //设置通道1测量的触发源为输出完成信号          
            #endregion
            #region Sense
            this._chassis.TryWrite(":CHAN1:SENS:REM ON");                //设置通道1远端测量（4线法）
            this._chassis.TryWrite(":CHAN1:SENS:FUNC VOLT");             //设置通道1测量电压
            this._chassis.TryWrite(":CHAN1:SENS:VOLT:RANG:AUTO OFF");   //关闭通道1测量自动量程
            this._chassis.TryWrite(":CHAN1:SENS:VOLT:RANG 10V");         //设置通道1电压测量量程2V
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:NPLC {0}", nplc));               //设置通道1测量NPLC，王礼霖给的值是0.05，可配置
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:DEL {0}", senseDelay_s));              //设置通道1测量延迟为最小值，王礼霖给的值是0.001，可配置
            //this._chassis.TryWrite(":CHAN1:SENS:AVER:STAT 1;COUN 10");
            this._chassis.TryWrite(string.Format(@"CHAN1:SENS:ZERO:AUTO 1"));

            if (pulsedMode)
            {
                this._chassis.TryWrite(":CHAN1:SOUR:SHAP PULSE");
                this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:PULSE:WIDTH {0}", pulseWidth));
                this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:PULSE:BASE {0}", "0.01"));
            }
            #endregion




            this._chassis.TryWrite(":CHAN2:SOUR:FUNC VOLT");            //设置通道2的输出功能为电压
            this._chassis.TryWrite(":CHAN2:SOUR:VOLT:RANG:AUTO OFF");         //设置电压输出量程为2V
            this._chassis.TryWrite(":CHAN2:SOUR:VOLT:RANG 2V");

            if (sourceDelay_s == 0)
            {
                this._chassis.TryWrite(string.Format(":CHAN2:SOUR:DEL {0}", "MIN"));  //设置通道1输出延迟为最小值，王礼霖给的值是0.002，可配置
            }
            else
            {
                this._chassis.TryWrite(string.Format(":CHAN2:SOUR:DEL {0}", sourceDelay_s));
            }
            this._chassis.TryWrite(":CHAN2:SOUR:MODE FIX");
            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:VOLT:LEV {0}V", 0));
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:PROT:LINK ON"));

            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:CURR:PROT:LEV {0}", smub_sense_current_range));
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:PROT:STAT ON"));

            #region Sense
            this._chassis.TryWrite(":CHAN2:SENS:TRIG SOUR");
            this._chassis.TryWrite(":CHAN2:SENS:REM OFF");               //设置通道2测量为本地测量，两线法
            this._chassis.TryWrite(":CHAN2:SENS:FUNC CURR");             //设置通道2的测量功能为电流
            this._chassis.TryWrite(":CHAN2:SENS:CURR:RANG:AUTO OFF");   //关闭自动量程
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:CURR:RANG {0}", smub_sense_current_range));       //设置电流测量量程为20mA
            //this._chassis.TryWrite(":CHAN2:SENS:AVER:STAT 1;COUN 10"); //++
            this._chassis.TryWrite(string.Format(@":CHAN2:SENS:ZERO:AUTO 1"));
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:NPLC {0}", nplc));                  //设置测量NPLC
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:DEL {0}", senseDelay_s));              //设置通道1测量延迟为最小值，王礼霖给的值是0.001，可配置
            #endregion


            this._chassis.TryWrite(":TRAC:STAT ON");                    //打开存储功能
            this._chassis.TryWrite(string.Format(":TRAC:POIN {0}", sweepPoints));                  //设置存储点数为1001
            this._chassis.TryWrite(":TRAC:CHAN1:DATA:FORM BIN");        //设置通道1存储数据格式为ASCII
            this._chassis.TryWrite(":TRAC:CHAN2:DATA:FORM BIN");        //设置通道2存储数据格式为ASCII
            this._chassis.TryWrite(":OUTP ON");                         //打开输出

            /////////////////////////////////////////
            Stopwatch st = new Stopwatch();
            st.Restart();
            Thread.Sleep(10);
            st.Stop();
            var a = st.ElapsedMilliseconds;

            st.Restart();
            this._chassis.TryWrite(":STAR");                            //开启扫描
            st.Stop();
            var b = st.ElapsedMilliseconds;


            st.Restart();
            //Thread.Sleep(1000);
            st.Stop();
            var c = st.ElapsedMilliseconds;

            st.Restart();
            while (true)
            {
                string resp = this._chassis.Query(":TRAC?", Delay_ms);         //查询扫描是否结束
                if (resp.Contains("0"))
                {
                    break;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
            st.Stop();
            var d = st.ElapsedMilliseconds;

            string AllTime = $"a:{a}ms   b:{b}ms   c:{c}ms   d:{d}ms";






            /////////////////////////////////////////





            string[] results = new string[4];
            int chAdataCount = int.Parse(this._chassis.Query(":TRAC:CHAN1:ACT?", Delay_ms));
            int chBdataCount = int.Parse(this._chassis.Query(":TRAC:CHAN2:ACT?", Delay_ms));

            if (chAdataCount != chBdataCount)
            {
                return results;
            }

            int dataCount = chAdataCount;
            this._chassis.TryWrite(":TRAC:STAT OFF;"); Thread.Sleep(5);

            this._chassis.TryWrite(":OUTP OFF");
            ((NiVisaChassis)this._chassis).DefaultBufferSize = chAdataCount * 8 * 2;
            //===========================CHA Source=====================
            this._chassis.TryWrite(":TRAC:CHAN1:DATA:READ? SL"); Thread.Sleep(5);
            byte[] sourA_SourValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();
            double[] CHA_Sours = new double[dataCount];
            for (int i = 0; i < dataCount; i++)
            {
                byte[] valueBytes = new byte[8];
                Array.Copy(sourA_SourValsBytes, 10 + i * 8, valueBytes, 0, 8);
                Array.Reverse(valueBytes);
                CHA_Sours[i] = BitConverter.ToDouble(valueBytes, 0);
                results[0] += CHA_Sours[i].ToString() + ",";
            }

            //===========================CHA MEAS=====================
            this._chassis.TryWrite(":TRAC:CHAN1:DATA:READ? ML"); Thread.Sleep(5);
            byte[] sourA_MesuValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();

            double[] CHA_Meas = new double[dataCount];
            for (int i = 0; i < dataCount; i++)
            {
                byte[] valueBytes = new byte[8];
                Array.Copy(sourA_MesuValsBytes, 10 + i * 8, valueBytes, 0, 8);
                Array.Reverse(valueBytes);
                CHA_Meas[i] = BitConverter.ToDouble(valueBytes, 0);
                results[1] += CHA_Meas[i].ToString() + ",";
            }
            //===========================CHB Source=====================
            this._chassis.TryWrite(":TRAC:CHAN2:DATA:READ? SL"); Thread.Sleep(5);
            byte[] sourB_SourValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();

            double[] CHB_Sours = new double[dataCount];
            for (int i = 0; i < dataCount; i++)
            {
                byte[] valueBytes = new byte[8];
                Array.Copy(sourB_SourValsBytes, 10 + i * 8, valueBytes, 0, 8);
                Array.Reverse(valueBytes);
                CHB_Sours[i] = BitConverter.ToDouble(valueBytes, 0);
                results[3] += CHB_Sours[i].ToString() + ",";
            }

            //===========================CHB MEAS=====================
            this._chassis.TryWrite(":TRAC:CHAN2:DATA:READ? ML"); Thread.Sleep(5);
            byte[] sourB_MesuValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();

            double[] CHB_Meas = new double[dataCount];
            for (int i = 0; i < dataCount; i++)
            {
                byte[] valueBytes = new byte[8];
                Array.Copy(sourB_MesuValsBytes, 10 + i * 8, valueBytes, 0, 8);
                Array.Reverse(valueBytes);
                CHB_Meas[i] = BitConverter.ToDouble(valueBytes, 0);
                results[2] += CHB_Meas[i].ToString() + ",";
            }
            results[0] = results[0].TrimEnd(',');
            results[1] = results[1].TrimEnd(',');
            results[2] = results[2].TrimEnd(',');
            results[3] = results[3].TrimEnd(',');

            return results;











            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            //var source_A_Values = ArrayMath.CalculateArray(smua_start_val, smua_stop_val, smua_step_val, 6);
            //double stepSize = source_A_Values[1] - source_A_Values[0];
            ////电压清零

            //this._chassis.TryWrite("*RST");
            //this._chassis.TryWrite(":SYNC:MODE MASTer");
            ////重置仪表
            //this._chassis.TryWrite(":SYNC:CHAN ON");                    //设置通道同步         
            //this._chassis.TryWrite(string.Format(":TRIGger:TIMer1 {0}", period_s));             //设置时钟1触发周期  0.005

            //#region ch A config
            //this._chassis.TryWrite(":CHAN1:SOUR:FUNC CURR");            //设置通道1输出电流
            //this._chassis.TryWrite(":CHAN1:SOUR:CURR:RANG:AUTO OFF");    //关闭通道1输出自动量程
            //this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:RANG {0}A", smua_stop_val));         //设置通道1电流输出量程适应扫描输出最大值  0.1
            //this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:PROT:LINK ON"));
            //this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:CURR:LEV {0}", smua_start_val));//0.001

            //this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:VOLT:PROT:LEV {0}V", 2.5));//2.5
            //this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:PROT:STAT ON"));
            //if (sourceDelay_s == 0)
            //{
            //    this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:DEL {0}", "MIN"));   //设置通道1输出延迟为最小值，王礼霖给的值是0.002，可配置
            //}
            //else
            //{
            //    this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:DEL {0}", sourceDelay_s));
            //}

            //this._chassis.TryWrite(":CHAN1:SOUR:MODE SWE");             //设置通道1输出模式为扫描
            //this._chassis.TryWrite(":CHAN1:SOUR:CURR:SWE:SPAC LIN");    //设置通道1扫描方式为线性扫描
            //this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STEP {0}A", stepSize)); //设置通道1扫描步进0.001A
            //this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STAR {0}A", smua_start_val));     //设置通道1扫描起始电流0A
            //this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STOP {0}A", smua_stop_val));   //设置通道1扫描起始电流1.2A
            //this._chassis.TryWrite(":CHAN1:SWE:TRIG EXT");              //设置通道1扫描开始的触发信号来自外部，即通过Start指令
            //this._chassis.TryWrite(":CHAN1:SOUR:TRIG TIM1");            //设置通道1输出的触发源为时钟1

            //this._chassis.TryWrite(":CHAN1:SENS:REM ON");                //设置通道1远端测量（4线法）
            //this._chassis.TryWrite(":CHAN1:SENS:FUNC VOLT");             //设置通道1测量电压
            //this._chassis.TryWrite(":CHAN1:SENS:VOLT:RANG:AUTO OFF");   //关闭通道1测量自动量程
            //this._chassis.TryWrite(":CHAN1:SENS:VOLT:RANG 3V");         //设置通道1电压测量量程2V
            //this._chassis.TryWrite(string.Format(":CHAN1:SENS:NPLC {0}", nplc));               //设置通道1测量NPLC，王礼霖给的值是0.05，可配置
            //this._chassis.TryWrite(string.Format(":CHAN1:SENS:DEL {0}", senseDelay_s));              //设置通道1测量延迟为最小值，王礼霖给的值是0.001，可配置
            //this._chassis.TryWrite(string.Format(@"CHAN1:SENS:ZERO:AUTO 1"));


            //if (pulsedMode)
            //{
            //    this._chassis.TryWrite(":CHAN1:SOUR:SHAP PULSE");
            //    this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:PULSE:WIDTH {0}", pulseWidth));
            //    this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:PULSE:BASE {0}", "0.01"));
            //}
            //#endregion



            //this._chassis.TryWrite(":CHAN2:SOUR:FUNC VOLT");            //设置通道2的输出功能为电压
            //this._chassis.TryWrite(":CHAN2:SOUR:VOLT:RANG:AUTO OFF");         //设置电压输出量程为2V
            ////this._chassis.TryWrite(":CHAN2:SOUR:VOLT:RANG 2V");         //设置电压输出量程为2V
            //if (sourceDelay_s == 0)
            //{
            //    this._chassis.TryWrite(string.Format(":CHAN2:SOUR:DEL {0}", "MIN"));  //设置通道1输出延迟为最小值，王礼霖给的值是0.002，可配置
            //}
            //else
            //{
            //    this._chassis.TryWrite(string.Format(":CHAN2:SOUR:DEL {0}", sourceDelay_s));
            //}
            ////设置通道1输出延迟为最小值，王礼霖给的值是0.002，可配置
            //this._chassis.TryWrite(":CHAN2:SOUR:MODE FIX");            //设置通道2的输出模式为单值
            //this._chassis.TryWrite(string.Format(":CHAN2:SOUR:VOLT:LEV {0}V", 0));           //设置电压输出值为1V  smub_source_voltage_level
            //this._chassis.TryWrite(string.Format(@"CHAN2:SOUR:PROT:LINK ON"));
            //this._chassis.TryWrite(string.Format(@"CHAN2:SOUR:CURR:PROT:LEV {0}", smub_sense_current_range));
            //this._chassis.TryWrite(string.Format(@"CHAN2:SOUR:PROT:STAT ON"));


            //this._chassis.TryWrite(":CHAN2:SENS:TRIG SOUR");
            //this._chassis.TryWrite(":CHAN2:SENS:REM OFF");               //设置通道2测量为本地测量，两线法  
            //this._chassis.TryWrite(":CHAN2:SENS:FUNC CURR");             //设置通道2的测量功能为电流  changge
            //this._chassis.TryWrite(":CHAN2:SENS:CURR:RANG:AUTO OFF");   //关闭自动量程  changge
            //this._chassis.TryWrite(string.Format(":CHAN2:SENS:CURR:RANG {0}", smub_sense_current_range));     //设置电流测量量程为20mA  changge
            //this._chassis.TryWrite(string.Format(@"CHAN2:SENS:ZERO:AUTO 1"));
            //this._chassis.TryWrite(string.Format(":CHAN2:SENS:NPLC {0}", nplc));                  //设置测量NPLC
            //this._chassis.TryWrite(string.Format(":CHAN2:SENS:DEL {0}", senseDelay_s));              //设置通道1测量延迟为最小值，王礼霖给的值是0.001，可配置



            //this._chassis.TryWrite(":TRAC:STAT ON");                    //打开存储功能
            //this._chassis.TryWrite(string.Format(":TRAC:POIN {0}", sweepPoints));                  //设置存储点数为1001
            //this._chassis.TryWrite(":TRAC:CHAN1:DATA:FORM BIN");        //设置通道1存储数据格式为ASCII
            //this._chassis.TryWrite(":TRAC:CHAN2:DATA:FORM BIN");        //设置通道2存储数据格式为ASCII
            //this._chassis.TryWrite(":OUTP ON");                         //打开输出
            //Thread.Sleep(100);
            //sw.Restart();
            //this._chassis.TryWrite(":STAR");                            //开启扫描
            //Thread.Sleep(100);


            //while (true)
            //{
            //    string resp = this._chassis.Query(":TRAC?", Delay_ms);         //查询扫描是否结束
            //    if (resp.Contains("0"))
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        Thread.Sleep(50);
            //    }
            //}
            //sw.Stop();
            //string[] results = new string[4];
            //int chAdataCount = int.Parse(this._chassis.Query(":TRAC:CHAN1:ACT?", Delay_ms));
            //int chBdataCount = int.Parse(this._chassis.Query(":TRAC:CHAN2:ACT?", Delay_ms));

            //if (chAdataCount != chBdataCount)
            //{
            //    return results;
            //}

            //int dataCount = chAdataCount;
            //this._chassis.TryWrite(":TRAC:STAT OFF;"); Thread.Sleep(5);



            //this._chassis.TryWrite(":OUTP OFF");


            //double n = sw.ElapsedMilliseconds / 1000.0;


            //((NiVisaChassis)this._chassis).DefaultBufferSize = chAdataCount * 8 * 2;

            ////===========================CHA Source=====================
            //this._chassis.TryWrite(":TRAC:CHAN1:DATA:READ? SL"); Thread.Sleep(5);
            //byte[] sourA_SourValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();
            //double[] CHA_Sours = new double[dataCount];
            //for (int i = 0; i < dataCount; i++)
            //{
            //    byte[] valueBytes = new byte[8];
            //    Array.Copy(sourA_SourValsBytes, 10 + i * 8, valueBytes, 0, 8);
            //    Array.Reverse(valueBytes);
            //    CHA_Sours[i] = BitConverter.ToDouble(valueBytes, 0);
            //    results[0] += CHA_Sours[i].ToString() + ",";
            //}

            ////===========================CHA MEAS=====================
            //this._chassis.TryWrite(":TRAC:CHAN1:DATA:READ? ML"); Thread.Sleep(5);
            //byte[] sourA_MesuValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();

            //double[] CHA_Meas = new double[dataCount];
            //for (int i = 0; i < dataCount; i++)
            //{
            //    byte[] valueBytes = new byte[8];
            //    Array.Copy(sourA_MesuValsBytes, 10 + i * 8, valueBytes, 0, 8);
            //    Array.Reverse(valueBytes);
            //    CHA_Meas[i] = BitConverter.ToDouble(valueBytes, 0);
            //    results[1] += CHA_Meas[i].ToString() + ",";
            //}
            ////===========================CHB Source=====================
            //this._chassis.TryWrite(":TRAC:CHAN2:DATA:READ? SL"); Thread.Sleep(5);
            //byte[] sourB_SourValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();

            //double[] CHB_Sours = new double[dataCount];
            //for (int i = 0; i < dataCount; i++)
            //{
            //    byte[] valueBytes = new byte[8];
            //    Array.Copy(sourB_SourValsBytes, 10 + i * 8, valueBytes, 0, 8);
            //    Array.Reverse(valueBytes);
            //    CHB_Sours[i] = BitConverter.ToDouble(valueBytes, 0);
            //    results[3] += CHB_Sours[i].ToString() + ",";
            //}

            ////===========================CHB MEAS=====================
            //this._chassis.TryWrite(":TRAC:CHAN2:DATA:READ? ML"); Thread.Sleep(5);
            //byte[] sourB_MesuValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();

            //double[] CHB_Meas = new double[dataCount];
            //for (int i = 0; i < dataCount; i++)
            //{
            //    byte[] valueBytes = new byte[8];
            //    Array.Copy(sourB_MesuValsBytes, 10 + i * 8, valueBytes, 0, 8);
            //    Array.Reverse(valueBytes);
            //    CHB_Meas[i] = BitConverter.ToDouble(valueBytes, 0);
            //    results[2] += CHB_Meas[i].ToString() + ",";
            //}
            //results[0] = results[0].TrimEnd(',');
            //results[1] = results[1].TrimEnd(',');
            //results[2] = results[2].TrimEnd(',');
            //results[3] = results[3].TrimEnd(',');

            //return results;

        }
        public override string[] SweepDualChannelsSYNC_WithRangeSettings(
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
            this._chassis.TryWrite("*RST");            //重置仪表
            Thread.Sleep(Delay_ms);
            this._chassis.TryWrite(":SYNC:MODE MASTer");
            this._chassis.TryWrite(":SYNC:CHAN ON");                    //设置通道同步         
            this._chassis.TryWrite(string.Format(":TRIGger:TIMer1 {0}", period_s));             //设置时钟1触发周期

            int sweepPoints = ArrayMath.CalculateArray(smua_start_val, smua_stop_val, smua_step_val).Length;
            //#region CH1 config
            #region Source
            this._chassis.TryWrite(":CHAN1:SOUR:FUNC CURR");            //设置通道1输出电流
            this._chassis.TryWrite(":CHAN1:SOUR:CURR:RANG:AUTO OFF");    //关闭通道1输出自动量程
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:RANG {0}A", smua_stop_val));         //设置通道1电流输出量程适应扫描输出最大值
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:PROT:LINK ON"));
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:CURR:LEV {0}", smua_start_val));
            //this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:VOLT:RANG:AUTO OFF"));
            //this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:VOLT:RANG {0}V", smua_voltLimit));
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:VOLT:PROT:LEV {0}V", smua_source_protect_volt_limit));//10
            this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:PROT:STAT ON"));
            if (sourceDelay_s == 0)
            {
                this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:DEL {0}", "MIN"));   //设置通道1输出延迟为最小值，王礼霖给的值是0.002，可配置
            }
            else
            {
                this._chassis.TryWrite(string.Format(@"CHAN1:SOUR:DEL {0}", sourceDelay_s));
            }

            #endregion
            #region Sweep
            this._chassis.TryWrite(":CHAN1:SOUR:MODE SWE");             //设置通道1输出模式为扫描
            this._chassis.TryWrite(":CHAN1:SOUR:CURR:SWE:SPAC LIN");    //设置通道1扫描方式为线性扫描
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STEP {0}A", smua_step_val)); //设置通道1扫描步进0.001A
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STAR {0}A", smua_start_val));     //设置通道1扫描起始电流0A
            this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:SWE:STOP {0}A", smua_stop_val));   //设置通道1扫描起始电流1.2A

            this._chassis.TryWrite(":CHAN1:SWE:TRIG EXT");              //设置通道1扫描开始的触发信号来自外部，即通过Start指令
            this._chassis.TryWrite(":CHAN1:SOUR:TRIG TIM1");            //设置通道1输出的触发源为时钟1
      
            #endregion
            #region Sense
            this._chassis.TryWrite(":CHAN1:SENS:REM ON");                //设置通道1远端测量（4线法）
            this._chassis.TryWrite(":CHAN1:SENS:FUNC VOLT");             //设置通道1测量电压
            this._chassis.TryWrite(":CHAN1:SENS:VOLT:RANG:AUTO OFF");   //关闭通道1测量自动量程
            //this._chassis.TryWrite(":CHAN1:SENS:VOLT:RANG 10V");         //设置通道1电压测量量程2V  
            //--Ben备注 20230314  开放通道1测量电压范围参数
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:VOLT:RANG {0}V", smua_sense_voltage_range));         //设置通道1电压测量量程2V
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:NPLC {0}", nplc));               //设置通道1测量NPLC，王礼霖给的值是0.05，可配置
            this._chassis.TryWrite(string.Format(":CHAN1:SENS:DEL {0}", senseDelay_s));              //设置通道1测量延迟为最小值，王礼霖给的值是0.001，可配置
       
            this._chassis.TryWrite(string.Format(@"CHAN1:SENS:ZERO:AUTO 1"));

            if (pulsedMode)
            {
                this._chassis.TryWrite(":CHAN1:SOUR:SHAP PULSE");
                this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:PULSE:WIDTH {0}", pulseWidth));
                this._chassis.TryWrite(string.Format(":CHAN1:SOUR:CURR:PULSE:BASE {0}", "0.01"));
            }
            #endregion
 
            this._chassis.TryWrite(":CHAN2:SOUR:FUNC VOLT");            //设置通道2的输出功能为电压
            this._chassis.TryWrite(":CHAN2:SOUR:VOLT:RANG:AUTO OFF");         //设置电压输出量程为2V
            this._chassis.TryWrite(":CHAN2:SOUR:VOLT:RANG 2V");

            if (sourceDelay_s == 0)
            {
                this._chassis.TryWrite(string.Format(":CHAN2:SOUR:DEL {0}", "MIN"));  //设置通道1输出延迟为最小值，王礼霖给的值是0.002，可配置
            }
            else
            {
                this._chassis.TryWrite(string.Format(":CHAN2:SOUR:DEL {0}", sourceDelay_s));
            }
            this._chassis.TryWrite(":CHAN2:SOUR:MODE FIX");
            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:VOLT:LEV {0}V", 0));
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:PROT:LINK ON"));

            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:CURR:PROT:LEV {0}", smub_sense_protect_current_limit));//0.02
            this._chassis.TryWrite(string.Format(@":CHAN2:SOUR:PROT:STAT ON"));

            #region Sense
            this._chassis.TryWrite(":CHAN2:SENS:TRIG SOUR");
            this._chassis.TryWrite(":CHAN2:SENS:REM OFF");               //设置通道2测量为本地测量，两线法
            this._chassis.TryWrite(":CHAN2:SENS:FUNC CURR");             //设置通道2的测量功能为电流
            this._chassis.TryWrite(":CHAN2:SENS:CURR:RANG:AUTO OFF");   //关闭自动量程
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:CURR:RANG {0}", smub_sense_current_range)); //0.001      //设置电流测量量程为20mA
   
            this._chassis.TryWrite(string.Format(@":CHAN2:SENS:ZERO:AUTO 1"));
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:NPLC {0}", nplc));                  //设置测量NPLC
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:DEL {0}", senseDelay_s));              //设置通道1测量延迟为最小值，王礼霖给的值是0.001，可配置
            #endregion


            this._chassis.TryWrite(":TRAC:STAT ON");                    //打开存储功能
            this._chassis.TryWrite(string.Format(":TRAC:POIN {0}", sweepPoints));                  //设置存储点数为1001
            this._chassis.TryWrite(":TRAC:CHAN1:DATA:FORM BIN");        //设置通道1存储数据格式为ASCII
            this._chassis.TryWrite(":TRAC:CHAN2:DATA:FORM BIN");        //设置通道2存储数据格式为ASCII
            this._chassis.TryWrite(":OUTP ON");                         //打开输出

            /////////////////////////////////////////
        
            Thread.Sleep(10);
       
            this._chassis.TryWrite(":STAR");                            //开启扫描
 
            while (true)
            {
                string resp = this._chassis.Query(":TRAC?", Delay_ms);         //查询扫描是否结束
                if (resp.Contains("0"))
                {
                    break;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
          

 

            string[] results = new string[4];
            int chAdataCount = int.Parse(this._chassis.Query(":TRAC:CHAN1:ACT?", Delay_ms));
            int chBdataCount = int.Parse(this._chassis.Query(":TRAC:CHAN2:ACT?", Delay_ms));

            if (chAdataCount != chBdataCount)
            {
                return results;
            }

            int dataCount = chAdataCount;
            this._chassis.TryWrite(":TRAC:STAT OFF;"); Thread.Sleep(5);

            this._chassis.TryWrite(":OUTP OFF");
            ((NiVisaChassis)this._chassis).DefaultBufferSize = chAdataCount * 8 * 2;
            //===========================CHA Source=====================
            this._chassis.TryWrite(":TRAC:CHAN1:DATA:READ? SL"); Thread.Sleep(5);
            byte[] sourA_SourValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();
            double[] CHA_Sours = new double[dataCount];
            for (int i = 0; i < dataCount; i++)
            {
                byte[] valueBytes = new byte[8];
                Array.Copy(sourA_SourValsBytes, 10 + i * 8, valueBytes, 0, 8);
                Array.Reverse(valueBytes);
                CHA_Sours[i] = BitConverter.ToDouble(valueBytes, 0);
                results[0] += CHA_Sours[i].ToString() + ",";
            }

            //===========================CHA MEAS=====================
            this._chassis.TryWrite(":TRAC:CHAN1:DATA:READ? ML"); Thread.Sleep(5);
            byte[] sourA_MesuValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();

            double[] CHA_Meas = new double[dataCount];
            for (int i = 0; i < dataCount; i++)
            {
                byte[] valueBytes = new byte[8];
                Array.Copy(sourA_MesuValsBytes, 10 + i * 8, valueBytes, 0, 8);
                Array.Reverse(valueBytes);
                CHA_Meas[i] = BitConverter.ToDouble(valueBytes, 0);
                results[1] += CHA_Meas[i].ToString() + ",";
            }
            //===========================CHB Source=====================
            this._chassis.TryWrite(":TRAC:CHAN2:DATA:READ? SL"); Thread.Sleep(5);
            byte[] sourB_SourValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();

            double[] CHB_Sours = new double[dataCount];
            for (int i = 0; i < dataCount; i++)
            {
                byte[] valueBytes = new byte[8];
                Array.Copy(sourB_SourValsBytes, 10 + i * 8, valueBytes, 0, 8);
                Array.Reverse(valueBytes);
                CHB_Sours[i] = BitConverter.ToDouble(valueBytes, 0);
                results[3] += CHB_Sours[i].ToString() + ",";
            }

            //===========================CHB MEAS=====================
            this._chassis.TryWrite(":TRAC:CHAN2:DATA:READ? ML"); Thread.Sleep(5);
            byte[] sourB_MesuValsBytes = ((NiVisaChassis)this._chassis).ReadByteArray();

            double[] CHB_Meas = new double[dataCount];
            for (int i = 0; i < dataCount; i++)
            {
                byte[] valueBytes = new byte[8];
                Array.Copy(sourB_MesuValsBytes, 10 + i * 8, valueBytes, 0, 8);
                Array.Reverse(valueBytes);
                CHB_Meas[i] = BitConverter.ToDouble(valueBytes, 0);
                results[2] += CHB_Meas[i].ToString() + ",";
            }
            results[0] = results[0].TrimEnd(',');
            results[1] = results[1].TrimEnd(',');
            results[2] = results[2].TrimEnd(',');
            results[3] = results[3].TrimEnd(',');

            return results;
        }

        public override string[] ExternalTriggerSweep(Action startExternalTrigger, int points, double smua_nplc, double smua_delay_s)
        {
            string[] results = new string[8];


            this._chassis.TryWrite(":CHAN2:SOUR:FUNC VOLT");
            this._chassis.TryWrite(":CHAN2:SENS:FUNC CURR");

            this._chassis.TryWrite(":CHAN2:SOUR:VOLT:RANG:AUTO ON");
            this._chassis.TryWrite(":CHAN2:SENS:CURR:RANG:AUTO ON");

            this._chassis.TryWrite(":CHAN2:SOUR:MODE SING");
            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:VOLT:LEV {0}V", 0));

            this._chassis.TryWrite(":CHAN2:SWE:TRIG EXT");
            this._chassis.TryWrite(":CHAN2:SOUR:TRIG EXT");
            this._chassis.TryWrite(":CHAN2:SWE:COUN 1");

            //传参
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:NPLC {0}", 0.01));//0.01
            this._chassis.TryWrite(string.Format(":CHAN2:SOUR:DEL {0}", 0.0001));//0.0001
            this._chassis.TryWrite(string.Format(":CHAN2:SENS:DEL {0}", 0.0001));//0.0001

            this._chassis.TryWrite(":ROUT:BNC:STAR INP");
            this._chassis.TryWrite(":ROUT:BNC:TRIG INP");

            this._chassis.TryWrite(":CHAN2:OUTP ON");
            //this._chassis.TryWrite(":CHAN1:SWE:COUN 1");

            this._chassis.TryWrite(":TRAC:STAT ON;");
            //this._chassis.TryWrite(":TRAC:POIN 900;");
            this._chassis.TryWrite(string.Format(":TRAC:POIN {0};", points));

            this._chassis.TryWrite(":TRAC:CHAN2:DATA:FORM ASC;");
            Thread.Sleep(500);
            //this._chassis.TryWrite(":STAR");

            startExternalTrigger.Invoke();
            Thread.Sleep(200);
            while (true)
            {
                string resp = this._chassis.Query(":TRAC?", Delay_ms);
                if (resp.Contains("0"))
                {
                    break;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
            this._chassis.TryWrite(":TRAC:STAT OFF;");
            this._chassis.TryWrite(":CHAN2:OUTP OFF");

            string respVal = this._chassis.Query(":TRAC:CHAN2:DATA:READ? ML", Delay_ms);
            return respVal.Split(',');
        }

    }
}