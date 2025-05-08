using NationalInstruments.VisaNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    //public enum Keithley2602BChannel
    //{
    //    CHA,
    //    CHB
    //}

    //public enum SourceMeterSenceMode : int
    //{
    //    SENSE_LOCAL = 0,//0 or smuX.SENSE_LOCAL: Selects local sense (2-wire) 
    //    SENSE_REMOTE = 1, //1 or smuX.SENSE_REMOTE: Selects remote sense (4-wire)
    //    SENSE_CALA = 3 //3 or smuX.SENSE_CALA: Selects calibration sense mode 
    //}
    //public enum SourceMeterFuncitonMode
    //{
    //    Source,
    //    Measure
    //}
    //public enum SourceMeterAutoZero : int
    //{
    //    AUTOZERO_OFF = 0,//0 or smuX.AUTOZERO_OFF: Autozero disabled 
    //    AUTOZERO_ONCE = 1, //1 or smuX.AUTOZERO_ONCE: Performs autozero once, then disables autozero 
    //    AUTOZERO_AUTO = 2 //2 or smuX.AUTOZERO_AUTO: Automatic checking of reference and zero measurements; 
    //}

    public class Keithley2602B : InstrumentBase//, ISourceMeter_Keithley
    {
        private const int defaultRespondingTime_ms = 1000;

        public Keithley2602B(string name, string address, IInstrumentChassis chassis)
           : base(name, address, chassis)
        {


        }
        /// <summary>
        /// Communication timeout in millionsecond
        /// </summary>
        public override int Timeout_ms
        {
            get
            {
                return this._chassis.Timeout_ms;
            }
            set
            {
                this._chassis.Timeout_ms = value;
            }
        }
        public override void TurnOnline(bool isOnline)
        {
            this._chassis.BuildConnection(5000);
            this._isOnline = true;
        }

        public string IDN
        {
            get
            {
                if (this.IsOnline)
                {
                    var idn = this._chassis.Query("*IDN?",  defaultRespondingTime_ms);
                    return idn;
                }
                return "Offline K2602B";
            }
        }
        public void Reset()
        {
            if (this.IsOnline == false)
            {
                return;
            }
            SetIsOutputOn(Keithley2602BChannel.CHA, false);
            SetIsOutputOn(Keithley2602BChannel.CHB, false);
            this._chassis.TryWrite("reset()");
        }
        public virtual void ResetTriggerLine(TriggerLine triggerIOChannel)
        {
            ResetTriggerLine((int)(triggerIOChannel));
        }

        public virtual void ResetTriggerLine(int triggerIOChannel)
        {
            this._chassis.TryWrite($"digio.trigger[{(int)triggerIOChannel}].reset()");  //Trigger input
            //this.Chassis.Write($"digio.trigger[{(int)triggerIOChannel}].mode = digio.TRIG_RISINGA" + endchar);  //Trigger input

        }

        public void Reset(Keithley2602BChannel ch)
        {

            if (this.IsOnline == false)
            {
                return;
            }
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    this._chassis.TryWrite("smua.reset()");
                    break;
                case Keithley2602BChannel.CHB:
                    this._chassis.TryWrite("smub.reset()");
                    break;
            }
        }

        public double MeasureVoltage_V(Keithley2602BChannel ch)
        {
            if (this.IsOnline == false)
            {
                return 0.0;
            }
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    this._chassis.TryWrite("reading = smua.measure.v()");
                    break;
                case Keithley2602BChannel.CHB:
                    this._chassis.TryWrite("reading = smub.measure.v()");
                    break;
            }
            var ret = this._chassis.Query("print(reading)",defaultRespondingTime_ms);

            double val = double.NaN;
            double.TryParse(ret, out val);
            return val;
        }
        public double MeasureCurrent_A(Keithley2602BChannel ch)
        {
            if (this.IsOnline == false)
            {
                return 0.0;
            }
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    this._chassis.TryWrite("reading = smua.measure.i()");
                    break;
                case Keithley2602BChannel.CHB:
                    this._chassis.TryWrite("reading = smub.measure.i()");
                    break;
            }

            var ret = this._chassis.Query("print(reading)", defaultRespondingTime_ms);

            double val = double.NaN;
            double.TryParse(ret, out val);
            return val;
        }
        public void SetIsOutputOn(Keithley2602BChannel ch, bool isEnable)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = isEnable ? "smua.source.output =smua.OUTPUT_ON" : "smua.source.output = smua.OUTPUT_OFF";
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = isEnable ? "smub.source.output =smub.OUTPUT_ON" : "smub.source.output = smub.OUTPUT_OFF";
                    break;
            }
            this._chassis.TryWrite(cmd);
        }
        float _currentSetPoint_A = 0f;
        public float CurrentSetpoint_A
        {
            get
            {
                return this._currentSetPoint_A;
            }
            set
            {
                Reset();
                SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);
                SetSourceCurrentRange_A(Keithley2602BChannel.CHA, value);
                SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
                SetComplianceVoltage_V(Keithley2602BChannel.CHA, 2.5);
                SetCurrent_A(Keithley2602BChannel.CHA, value);
                if (GetIsOutputOn(Keithley2602BChannel.CHA) == false)
                {
                    SetIsOutputOn(Keithley2602BChannel.CHA, true);
                }
            }
        }
        float _VoltageSetpoint_V = 0f;
        public float VoltageSetpoint_V
        {
            get
            {
                return this._VoltageSetpoint_V;
            }
            set
            {
                this.SetVoltage_V(Keithley2602BChannel.CHA, value);
            }
        }
        float _VoltageSetpoint_PD_V = 0f;
        public float VoltageSetpoint_PD_V
        {
            get
            {
                return this._VoltageSetpoint_PD_V;
            }
            set
            {
                this.SetVoltage_V(Keithley2602BChannel.CHB, value);
            }
        }
        public void SetupSMU_LD(double Current_mA, double complianceVoltage_V)
        {
            Reset();
            SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);
            SetSourceCurrentRange_A(Keithley2602BChannel.CHA, Current_mA/1000);
            SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
            SetComplianceVoltage_V(Keithley2602BChannel.CHA, complianceVoltage_V); 
            SetCurrent_A(Keithley2602BChannel.CHA, Current_mA/1000);
            if (GetIsOutputOn(Keithley2602BChannel.CHA) == false)
            {
                SetIsOutputOn(Keithley2602BChannel.CHA, true);
            }
        }

        public double ReadVoltage_V()
        {
            double volt = MeasureVoltage_V(Keithley2602BChannel.CHA);
            return Convert.ToSingle(volt);
        }
        public double ReadCurrent_PD_A()
        {
            if (GetIsOutputOn(Keithley2602BChannel.CHB) == false)
            {
                SetIsOutputOn(Keithley2602BChannel.CHB, true);
            }
            double curr = MeasureCurrent_A(Keithley2602BChannel.CHB);
            return Convert.ToSingle(curr);
        }
        public bool IsOutputEnable
        {
            get
            {
                bool CHA = GetIsOutputOn(Keithley2602BChannel.CHA);
                bool CHB = GetIsOutputOn(Keithley2602BChannel.CHB);
                if (CHA && CHB) return true;
                else return false;
            }
            set
            {
                SetIsOutputOn(Keithley2602BChannel.CHA, value);
                SetIsOutputOn(Keithley2602BChannel.CHB, value);
            }
        }
        public bool GetIsOutputOn(Keithley2602BChannel ch)
        {
            if (this.IsOnline == false)
            {
                return false;
            }
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    this._chassis.TryWrite("reading = smua.source.output");
                    break;
                case Keithley2602BChannel.CHB:
                    this._chassis.TryWrite("reading = smub.source.output");
                    break;
            }
            var ret = this._chassis.Query("print(reading)", defaultRespondingTime_ms);
            Thread.Sleep(10);
            double val = double.NaN;
            double.TryParse(ret, out val);
            bool isOpOn = val == 1.0 ? true : false;
            return isOpOn;
        }

        public double[] GetMeasureValues(Keithley2602BChannel ch, int points)
        {
            if (this.IsOnline == false)
            {
                return null;
            }
            List<double> temp = new List<double>();
            string cmd = "";
            string ret = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("printbuffer(1, {0}, smua.nvbuffer1.readings)", points);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("printbuffer(1, {0}, smub.nvbuffer1.readings)", points);
                    break;
            }
            ret = this._chassis.Query(cmd, defaultRespondingTime_ms);
            var retArr = ret.Split(',');
            foreach (var item in retArr)
            {
                temp.Add(Convert.ToDouble(item));
            }
            double[] measValues = temp.ToArray();
            return measValues;
        }
        public double[] GetSourceValues(Keithley2602BChannel ch, int points)
        {
            if (this.IsOnline == false)
            {
                return null;
            }
            List<double> temp = new List<double>();
            string cmd = "";
            string ret = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("printbuffer(1, {0}, smua.nvbuffer1.sourcevalues)", points);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("printbuffer(1, {0}, smub.nvbuffer1.sourcevalues)", points);
                    break;
            }
            ret = this._chassis.Query(cmd, defaultRespondingTime_ms);
            var retArr = ret.Split(',');
            foreach (var item in retArr)
            {
                temp.Add(Convert.ToDouble(item));
            }
            double[] sourValues = temp.ToArray();
            return sourValues;
        }
        public void Sweep(Keithley2602BChannel ch,
                          SourceMeterMode mode,
                          double startValue,
                          double stopValue,
                          int points,                          
                          double delay_s
                        )
        {
            if (this.IsOnline == false)
            {
                return;
            }
            
            List<double> temp = new List<double>();

            switch (mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    {
                        switch (ch)
                        {
                            case Keithley2602BChannel.CHA:
                                {
                                    string cmd = string.Format("SweepILinMeasureV(smua, {0}, {1}, {2}, {3})", startValue, stopValue, delay_s, points);
                                    this._chassis.TryWrite(cmd);
                                }
                                break;
                            case Keithley2602BChannel.CHB:
                                {
                                    string cmd = string.Format("SweepILinMeasureV(smub, {0}, {1}, {2}, {3})", startValue, stopValue, delay_s, points);
                                    this._chassis.TryWrite(cmd);
                                }
                                break;
                        }
                    }
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    {
                        switch (ch)
                        {
                            case Keithley2602BChannel.CHA:
                                {
 
                                    string cmd = string.Format("SweepVLinMeasureI(smua, {0}, {1}, {2}, {3})", startValue, stopValue, delay_s, points);
                                    this._chassis.TryWrite(cmd);
                                }
                                break;
                            case Keithley2602BChannel.CHB:
                                {
                                    string cmd = string.Format("SweepVLinMeasureI(smub, {0}, {1}, {2}, {3})", startValue, stopValue, delay_s, points);
                                    this._chassis.TryWrite(cmd);
                                }
                                break;
                        }
                    }
                    break;
            }
            var estimateDelay_ms = (delay_s+0.02)* points * 1000;
            Thread.Sleep(Convert.ToInt32(estimateDelay_ms));
            //while (true)
            //{
            //    this._chassis.TryWrite("*OPC?");
            //    var resp = Chassis.ReadString();

            //    if (resp.Equals("1\n"))
            //    {
            //        break;
            //    }
            //    System.Threading.Thread.Sleep(100);
            //}
        }
        public void SetComplianceCurrent_A(Keithley2602BChannel ch, double compVal)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.source.limiti = {0}", compVal);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.source.limiti = {0}", compVal);
                    break;
            }
            this._chassis.TryWrite(cmd);
        }
        public void SetComplianceVoltage_V(Keithley2602BChannel ch, double compVal)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.source.limitv = {0}", compVal);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.source.limitv = {0}", compVal);
                    break;
            }
            this._chassis.TryWrite(cmd);
        }
        public void SetVoltage_V(Keithley2602BChannel ch, double val)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.source.levelv = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.source.levelv = {0}", val);
                    break;
            }
            this._chassis.TryWrite(cmd);
        }
        public void SetCurrent_A(Keithley2602BChannel ch, double val)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.source.leveli = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.source.leveli = {0}", val);
                    break;
            }

            this._chassis.TryWrite(cmd);
        }
        public void SetSourceVoltageRange_V(Keithley2602BChannel ch, double val)  //liang 20191226  
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.source.rangev = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.source.rangev = {0}", val);
                    break;
            }

            this._chassis.TryWrite(cmd);
        }
        public void SetSourceCurrentRange_A(Keithley2602BChannel ch, double val)  //liang 20191226  
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.source.rangei = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.source.rangei = {0}", val);
                    break;
            }

            this._chassis.TryWrite(cmd);
        }
        public void MeasureCurrentAutoRange(Keithley2602BChannel ch)//liang 20191226
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = "smua.measure.autorangei = smua.AUTORANGE_ON";
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = "smub.measure.autorangei = smub.AUTORANGE_ON";
                    break;
            }
            this._chassis.TryWrite(cmd);
        }
        public void MeasureVoltageAutoRange(Keithley2602BChannel ch)//liang 20191227
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = "smua.measure.autorangev = smua.AUTORANGE_ON";
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = "smub.measure.autorangev = smub.AUTORANGE_ON";
                    break;
            }
            this._chassis.TryWrite(cmd);
        }
        public void SetMeasureVoltageRange_V(Keithley2602BChannel ch, double val)//liang 20191227
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.measure.rangev = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.measure.rangev = {0}", val);
                    break;
            }

            this._chassis.TryWrite(cmd);
        }
        public void SetMeasureCurrentRange_A(Keithley2602BChannel ch, double val)//liang 20200417
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.measure.rangei = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.measure.rangei = {0}", val);
                    break;
            }
            this._chassis.TryWrite(cmd);
        }
        public SourceMeterMode GetMode(Keithley2602BChannel ch)
        {

            if (this.IsOnline == false)
            {
                return SourceMeterMode.Unknown;
            }
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    this._chassis.TryWrite("reading = smua.source.func");
                    break;
                case Keithley2602BChannel.CHB:
                    this._chassis.TryWrite("reading = smub.source.func");
                    break;
            }
            var ret = this._chassis.Query("print(reading)", defaultRespondingTime_ms);
            Thread.Sleep(10);
            double val = double.NaN;
            double.TryParse(ret, out val);
            SourceMeterMode mode = SourceMeterMode.Unknown;
            if (val == 0.0)
            {
                mode = SourceMeterMode.SourceCurrentSenceVoltage;
            }
            else if (val == 1.0)
            {
                mode = SourceMeterMode.SourceVoltageSenceCurrent;
            }
            return mode;
        }
        public void SetMode(Keithley2602BChannel ch, SourceMeterMode mode)
        {

            if (this.IsOnline == false)
            {
                return;
            }

            string cmd = string.Empty;
            string smu = string.Empty;
            string minorCmd = string.Empty;
            string minorCmd2 = string.Empty;
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    smu = "smua";
                    break;
                case Keithley2602BChannel.CHB:
                    smu = "smub";
                    break;
            }
            switch (mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    minorCmd = "OUTPUT_DCAMPS";
                    minorCmd2 = "display.{0}.measure.func =  display.MEASURE_DCVOLTS";
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    minorCmd = "OUTPUT_DCVOLTS";
                    minorCmd2 = "display.{0}.measure.func =  display.MEASURE_DCAMPS";
                    break;
            }
            cmd = string.Format("{0}.source.func = {0}.{1}", smu, minorCmd);
            this._chassis.TryWrite(cmd);
            cmd = string.Format(minorCmd2, smu);
            this._chassis.TryWrite(cmd);
        }
        public void SetSenceMode(Keithley2602BChannel ch, SourceMeterSenceMode senceMode)
        {
            if (this.IsOnline == false)
            {
                return;
            }

            string cmd = string.Empty;
            string smu = string.Empty;
            string minorCmd = string.Empty;
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    smu = "smua";
                    break;
                case Keithley2602BChannel.CHB:
                    smu = "smub";
                    break;
            }
            switch (senceMode)
            {
                case SourceMeterSenceMode.SENSE_REMOTE:
                    minorCmd = "SENSE_REMOTE";
                    break;
                case SourceMeterSenceMode.SENSE_LOCAL:
                    minorCmd = "SENSE_LOCAL";
                    break;
                case SourceMeterSenceMode.SENSE_CALA:
                    minorCmd = "SENSE_CALA";
                    break;
            }
            //"smua.sense = smua.SENSE_REMOTE
            cmd = string.Format("{0}.sense = {0}.{1}", smu, minorCmd);
            this._chassis.TryWrite(cmd);
        }
        public void SetAutoRange_I_Enable(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, bool isEnable)
        {
            //smuX.measure.autorangei = smuX.AUTORANGE_ON Enable current measure autorange. 
            //smuX.measure.autorangev = smuX.AUTORANGE_ON Enable voltage measure autorange. 
            //smuX.measure.autorangei = smuX.AUTORANGE_OFF Disable current measure autorange. 
            //smuX.measure.autorangev = smuX.AUTORANGE_OFF Disable voltage measure autorange. 
            //smuX.source.autorangei = smuX.AUTORANGE_ON Enable current source autorange.
            //smuX.source.autorangev = smuX.AUTORANGE_ON Enable voltage source autorange. 
            //smuX.source.autorangei = smuX.AUTORANGE_OFF Disable current source autorange. 
            //smuX.source.autorangev = smuX.AUTORANGE_OFF Disable voltage source autorange. 
            if (this.IsOnline == false)
            {
                return;
            }

            string cmd = string.Empty;
            string smu = string.Empty;
            string minorCmd1 = string.Empty;
            string minorCmd2 = string.Empty;
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    smu = "smua";
                    break;
                case Keithley2602BChannel.CHB:
                    smu = "smub";
                    break;
            }
            switch (funcMode)
            {
                case SourceMeterFuncitonMode.Source:
                    minorCmd1 = "source";

                    break;
                case SourceMeterFuncitonMode.Measure:
                    minorCmd1 = "measure";
                    break;
            }
            if (isEnable)
            {
                minorCmd2 = "AUTORANGE_ON";
            }
            else
            {
                minorCmd2 = "AUTORANGE_OFF";

            }
            //smuX.source.autorangei = smuX.AUTORANGE_OFF Disable current source autorange. 
            cmd = string.Format("{0}.{1}.autorangei = {0}.{2}", smu, minorCmd1, minorCmd2);
            this._chassis.TryWrite(cmd);
        }
        public void SetAutoRange_V_Enable(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, bool isEnable)
        {
            //smuX.measure.autorangei = smuX.AUTORANGE_ON Enable current measure autorange. 
            //smuX.measure.autorangev = smuX.AUTORANGE_ON Enable voltage measure autorange. 
            //smuX.measure.autorangei = smuX.AUTORANGE_OFF Disable current measure autorange. 
            //smuX.measure.autorangev = smuX.AUTORANGE_OFF Disable voltage measure autorange. 
            //smuX.source.autorangei = smuX.AUTORANGE_ON Enable current source autorange.
            //smuX.source.autorangev = smuX.AUTORANGE_ON Enable voltage source autorange. 
            //smuX.source.autorangei = smuX.AUTORANGE_OFF Disable current source autorange. 
            //smuX.source.autorangev = smuX.AUTORANGE_OFF Disable voltage source autorange. 
            if (this.IsOnline == false)
            {
                return;
            }

            string cmd = string.Empty;
            string smu = string.Empty;
            string minorCmd1 = string.Empty;
            string minorCmd2 = string.Empty;
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    smu = "smua";
                    break;
                case Keithley2602BChannel.CHB:
                    smu = "smub";
                    break;
            }
            switch (funcMode)
            {
                case SourceMeterFuncitonMode.Source:
                    minorCmd1 = "source";

                    break;
                case SourceMeterFuncitonMode.Measure:
                    minorCmd1 = "measure";
                    break;
            }
            if (isEnable)
            {
                minorCmd2 = "AUTORANGE_ON";
            }
            else
            {
                minorCmd2 = "AUTORANGE_OFF";

            }
            //smuX.source.autorangei = smuX.AUTORANGE_OFF Disable current source autorange. 
            cmd = string.Format("{0}.{1}.autorangev = {0}.{2}", smu, minorCmd1, minorCmd2);
            this._chassis.TryWrite(cmd);
        }
        public void SetSourceVoltageLimit_V(Keithley2602BChannel ch, double val)  //liang 20191226  
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.source.limitv = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.source.limitv = {0}", val);
                    break;
            }

            this._chassis.TryWrite(cmd);
        }
        public void SetSourceCurrentLimit_A(Keithley2602BChannel ch, double val)  //liang 20191226  
        {
            if (this.IsOnline == false)
            {
                return;
            }
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.source.limiti = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.source.limiti = {0}", val);
                    break;
            }

            this._chassis.TryWrite(cmd);
        }
        public void SetAutoZero(Keithley2602BChannel ch, SourceMeterFuncitonMode funcMode, SourceMeterAutoZero autoMode)
        {
            //0 or smuX.AUTOZERO_OFF: Autozero disabled 
            //1 or smuX.AUTOZERO_ONCE: Performs autozero once, then disables autozero 
            //2 or smuX.AUTOZERO_AUTO: Automatic checking of reference and zero measurements; 
            if (this.IsOnline == false)
            {
                return;
            }

            string cmd = string.Empty;
            string smu = string.Empty;
            string minorCmd1 = string.Empty;
            string minorCmd2 = string.Empty;
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    smu = "smua";
                    break;
                case Keithley2602BChannel.CHB:
                    smu = "smub";
                    break;
            }
            switch (funcMode)
            {
                case SourceMeterFuncitonMode.Source:
                    minorCmd1 = "source";
                    break;
                case SourceMeterFuncitonMode.Measure:
                    minorCmd1 = "measure";
                    break;
            }
            switch (autoMode)
            {
                case SourceMeterAutoZero.AUTOZERO_AUTO:
                    minorCmd2 = "AUTOZERO_AUTO";
                    break;
                case SourceMeterAutoZero.AUTOZERO_OFF:
                    minorCmd2 = "AUTOZERO_OFF";

                    break;
                case SourceMeterAutoZero.AUTOZERO_ONCE:
                    minorCmd2 = "AUTOZERO_ONCE";
                    break;
            }
            //"smua.measure.autozero = smua.AUTOZERO_ONCE"
            cmd = string.Format("{0}.{1}.autozero = {0}.{2}", smu, minorCmd1, minorCmd2);
            this._chassis.TryWrite(cmd);
        }
        public void SetNPLC(Keithley2602BChannel ch, double val)
        {
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.measure.nplc = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.measure.nplc = {0}", val);
                    break;
            }
            this._chassis.TryWrite(cmd);
        }

        public void SetBufferSize(Keithley2602BChannel channel, int val)
        {
            if (this.IsOnline == false)
            {
                return;
            }

            string ch = channel == Keithley2602BChannel.CHA ? "a" : "b";

            this._chassis.TryWrite($"reading = smu{ch}.nvbuffer1.capacity");

            string currval = this._chassis.Query("print(reading)", defaultRespondingTime_ms);

            if (double.Parse(currval) < val)
            {
                this._chassis.TryWrite($"smu{ch}.nvbuffer1 = smua.makebuffer({val})");
            }

            this._chassis.TryWrite($"reading = smu{ch}.nvbuffer2.capacity");

            currval = this._chassis.Query("print(reading)", defaultRespondingTime_ms);

            if (double.Parse(currval) < val)
            {
                this._chassis.TryWrite($"smu{ch}.nvbuffer2 = smua.makebuffer({val})");
            }

        }




        public void SetVoltagePulseLevel(Keithley2602BChannel ch, double val)
        {
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.source.levelv = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.source.levelv = {0}", val);
                    break;
            }
            this._chassis.TryWrite(cmd);
        }
        public void SetCurrentPulseLevel(Keithley2602BChannel ch, double val)
        {
            string cmd = "";
            switch (ch)
            {
                case Keithley2602BChannel.CHA:
                    cmd = string.Format("smua.source.leveli = {0}", val);
                    break;
                case Keithley2602BChannel.CHB:
                    cmd = string.Format("smub.source.leveli = {0}", val);
                    break;
            }
            this._chassis.TryWrite(cmd);
        }
        public string[] SweepNormalModeDualChannels(SourceMeterMode smua_mode,
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
            this.Reset();
            #region normal config - smuA

            this.Reset(Keithley2602BChannel.CHA);
            //输出电流/电压模式
            this.SetMode(Keithley2602BChannel.CHA, smua_mode);//0

            switch (smua_mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    {
                        //四线制测量
                        this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
                        //关闭电流输出自动量程
                        this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
                        //this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
                        //设置输出电流量程
                        this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, smua_source_current_range);
                        //================================================================================
                        this.SetSourceVoltageRange_V(Keithley2602BChannel.CHA, smua_source_voltage_range);
                        //================================================================================
                        //设置输出电流上限
                        this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, smua_source_current_limit);
                        this.SetSourceVoltageLimit_V(Keithley2602BChannel.CHA, smua_source_voltage_limit);
                        //关闭自动清零
                        this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
                        //关闭测量自动量程
                        this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, false);
                        //设置电压量程3V
                        this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_measure_voltage_range);
                    }
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    {
                        //四线制测量
                        this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
                        //关闭电流输出自动量程
                        this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
                        this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
                        //设置输出电流量程
                        this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, smua_source_current_range);
                        this.SetSourceVoltageRange_V(Keithley2602BChannel.CHA, smua_source_voltage_range);
                        //设置输出电流上限
                        this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, smua_source_current_limit);
                        this.SetSourceVoltageLimit_V(Keithley2602BChannel.CHA, smua_source_voltage_limit);
                        //关闭自动清零
                        this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
                        //关闭测量自动量程
                        this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, false);
                        //设置电流量程3V
                        this.SetMeasureCurrentRange_A(Keithley2602BChannel.CHA, smua_measure_current_range);
                    }
                    break;
            }
            this.SetNPLC(Keithley2602BChannel.CHA, smua_nplc); //0.01
            //this.SetNPLC(Keithley2602BChannel.CHA, 0.01); 
            string CHa_measure_delay = smua_delay_s == 0 ? "smua.DELAY_AUTO" : smua_delay_s.ToString();
            //-- A timer will be used to set the measure delay and synchronize the measurement between the two SMUs so set the built in delay to 0.
            _chassis.TryWrite(string.Format("smua.measure.delay = {0}", CHa_measure_delay));
            //this._chassis.TryWrite("smua.measure.delay = 0.01");
            //清理buffer
            _chassis.TryWrite("smua.nvbuffer1.clear()");
            //this._chassis.TryWrite("smua.nvbuffer1.collecttimestamps= 1");
            _chassis.TryWrite("smua.nvbuffer2.clear()");
            //this._chassis.TryWrite("smua.nvbuffer2.collecttimestamps= 1");
            #endregion
            #region normal config - smuB
            this.Reset(Keithley2602BChannel.CHB);
            this.SetMode(Keithley2602BChannel.CHB, smub_mode);

            switch (smub_mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    {
                        this.SetSenceMode(Keithley2602BChannel.CHB, SourceMeterSenceMode.SENSE_LOCAL);
                        this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
                        this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
                        //set pulse value = 0 , because no pulse require in this case 
                        this.SetCurrentPulseLevel(Keithley2602BChannel.CHB, 0);
                        //================================================================================
                        this.SetSourceCurrentRange_A(Keithley2602BChannel.CHB, smub_source_current_range);
                        //================================================================================
                        this.SetSourceVoltageRange_V(Keithley2602BChannel.CHB, smub_source_voltage_range);

                        this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHB, smub_source_current_limit);
                        //================================================================================
                        this.SetSourceVoltageLimit_V(Keithley2602BChannel.CHB, smub_source_voltage_limit);
                        //================================================================================
                        this.SetAutoZero(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
                        this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, false);
                        this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHB, smua_measure_voltage_range);
                    }
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    {
                        this.SetSenceMode(Keithley2602BChannel.CHB, SourceMeterSenceMode.SENSE_LOCAL);
                        //this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
                        this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
                        //set pulse value = 0 , because no pulse require in this case 
                        //this.SetVoltagePulseLevel(Keithley2602BChannel.CHB, 0);
                        //================================================================================
                        //this.SetSourceCurrentRange_A(Keithley2602BChannel.CHB, smub_source_current_range);
                        //================================================================================
                        this.SetSourceVoltageRange_V(Keithley2602BChannel.CHB, smub_source_voltage_range);
                        this.SetVoltage_V(Keithley2602BChannel.CHB, 0);
                        this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHB, smub_source_current_limit);
                        //================================================================================
                        //this.SetSourceVoltageLimit_V(Keithley2602BChannel.CHB, smub_source_voltage_limit);
                        //================================================================================
                        this.SetAutoZero(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
                        this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, false);
                        this.SetMeasureCurrentRange_A(Keithley2602BChannel.CHB, smub_measure_current_range);
                    }
                    break;
            }
            this.SetNPLC(Keithley2602BChannel.CHB, smub_nplc);//0.01 
            //this.SetNPLC(Keithley2602BChannel.CHB, 1);

            //this._chassis.TryWrite("smub.measure.delay				= 0");
            //this._chassis.TryWrite("smub.measure.delay				= 0.1");
            string CHb_measure_delay = smub_delay_s == 0 ? "smub.DELAY_AUTO" : smub_delay_s.ToString();
            _chassis.TryWrite(string.Format("smub.measure.delay = {0}", CHb_measure_delay));//0.05

            _chassis.TryWrite("smub.nvbuffer1.clear()");
            // this._chassis.TryWrite("smub.nvbuffer1.collecttimestamps= 1");
            _chassis.TryWrite("smub.nvbuffer2.clear()");
            // this._chassis.TryWrite("smub.nvbuffer2.collecttimestamps= 1");
            #endregion

            #region trigger config
            //this._chassis.TryWrite(string.Format("trigger.timer[1].count = {0}", sweepPoints));//sweepPoints =1201   timerCount = 1201-1// 
            //this._chassis.TryWrite(string.Format("trigger.timer[1].delay			= {0}", trigger_delay_s));//0.1
            ////this._chassis.TryWrite(string.Format("trigger.timer[1].delay			= {0}", delay_s));
            //this._chassis.TryWrite("trigger.timer[1].passthrough	= true");
            //this._chassis.TryWrite("trigger.timer[1].stimulus		=  smua.trigger.ARMED_EVENT_ID");

            switch (smua_mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    _chassis.TryWrite(string.Format("smua.trigger.source.lineari({0}, {1}, {2})", smua_start_val, smua_stop_val, sweepPoints));
                    _chassis.TryWrite(string.Format("smua.trigger.source.limitv		= {0}", smua_trigger_source_voltage_limit));
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    _chassis.TryWrite(string.Format("smua.trigger.source.linearv({0}, {1}, {2})", smua_start_val, smua_stop_val, sweepPoints));
                    _chassis.TryWrite(string.Format("smua.trigger.source.limiti		= {0}", smua_trigger_source_current_limit));
                    break;
            }
             _chassis.TryWrite("smua.trigger.source.action		= smua.ENABLE");
             _chassis.TryWrite("smua.trigger.measure.action		= smua.ENABLE");
             _chassis.TryWrite("smua.trigger.source.stimulus	= smub.trigger.MEASURE_COMPLETE_EVENT_ID");
             _chassis.TryWrite("smua.trigger.measure.stimulus	= smua.trigger.SOURCE_COMPLETE_EVENT_ID");
             _chassis.TryWrite("smua.trigger.measure.iv(smua.nvbuffer1, smua.nvbuffer2)");
             _chassis.TryWrite("smua.trigger.endsweep.action	= smua.SOURCE_IDLE");
            _chassis.TryWrite(string.Format("smua.trigger.count				= {0}", sweepPoints));


            switch (smub_mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                     _chassis.TryWrite(string.Format("smub.trigger.source.lineari({0}, {1}, {2})", smub_start_val, smub_stop_val, sweepPoints));
                    _chassis.TryWrite(string.Format("smub.trigger.source.limitv		= {0}", smub_trigger_source_voltage_limit));
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                     _chassis.TryWrite(string.Format("smub.trigger.source.linearv({0}, {1}, {2})", smub_start_val, smub_stop_val, sweepPoints));
                    _chassis.TryWrite(string.Format("smub.trigger.source.limiti		= {0}", smub_trigger_source_current_limit));
                    break;
            }

            _chassis.TryWrite("smub.trigger.source.action		= smub.ENABLE");
            _chassis.TryWrite("smub.trigger.measure.action		= smub.ENABLE");
            _chassis.TryWrite("smub.trigger.measure.iv(smub.nvbuffer1, smub.nvbuffer2)");
            _chassis.TryWrite("smub.trigger.arm.stimulus		= 0");
            _chassis.TryWrite("smub.trigger.source.stimulus	    = 0");
            _chassis.TryWrite("smub.trigger.measure.stimulus	= smua.trigger.SOURCE_COMPLETE_EVENT_ID");
            _chassis.TryWrite("smub.trigger.endsweep.action	= smub.SOURCE_HOLD");
            _chassis.TryWrite("smua.trigger.endsweep.action	= smua.SOURCE_HOLD");
            _chassis.TryWrite(string.Format("smub.trigger.count				= {0}", sweepPoints));


             _chassis.TryWrite("smua.source.output				= smua.OUTPUT_ON");
             _chassis.TryWrite("smub.source.output				= smub.OUTPUT_ON");
             _chassis.TryWrite("smub.trigger.initiate()");
             _chassis.TryWrite("smua.trigger.initiate()");
            _chassis.TryWrite("smua.trigger.source.set()");
            #endregion
            int singleSampleDelay = trigger_delay_s * 1000 < 20 ? 20 : (int)(trigger_delay_s * 1000);

            int estimateSweepTime = Convert.ToInt32(sweepPoints * singleSampleDelay) + 200;
            //int estimateSweepTime = Convert.ToInt32(sweepPoints * (0.1 ) * 1000) + 500;
            Thread.Sleep(estimateSweepTime);
            //while (true)
            //{
            //    this._chassis.TryWrite("*OPC?");
            //    var resp = Chassis.ReadString();

            //    if (resp.Equals("1\n"))
            //    {
            //        break;
            //    }
            //    System.Threading.Thread.Sleep(100);
            //}
            this._chassis.TryWrite("smua.source.output = smua.OUTPUT_OFF");
            this._chassis.TryWrite("smub.source.output = smub.OUTPUT_OFF");
            this._chassis.TryWrite("format.data = format.ASCII");
            this._chassis.TryWrite("format.asciiprecision = 4"); //this._chassis.TryWrite("reset()");

            string[] ret = new string[4];
            string Source1Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source1Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source2Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source2Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);

            var s1c = Source2Current.Split(',');
            var s1v = Source2Voltage.Split(',');
            var s2c = Source2Current.Split(',');
            var s2v = Source2Voltage.Split(',');

            ret[0] = Source1Current;
            ret[1] = Source1Voltage;
            ret[2] = Source2Current;
            ret[3] = Source2Voltage;
            return ret;
        }
        public string[] SweepDualChannels(
                                   double smua_start_val,
                           double smua_stop_val,
                           double smua_complianceV,
                           double smub_source_voltage_level,
                           double smub_sense_current_range,
                           double sourceDelay_s,
                           double senseDelay_s,
                           double nplc,
                           int sweepPoints,
                           bool pulsedMode,
                           double pulseWidth,
                           double period)
        {

            this.Reset();
            #region normal config - smuA
            //输出电流/电压模式
            this.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);//0
            //四线制测量
            this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
            //关闭电流输出自动量程
            this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
            //设置输出电流量程
            this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, smua_stop_val);
            //设置输出电流上限
            this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, smua_stop_val);
            //关闭自动清零
            this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
            //设置电压量程
            this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_complianceV);

            this.SetNPLC(Keithley2602BChannel.CHA, nplc); //0.01

            string CHa_measure_delay = senseDelay_s == 0 ? "smua.DELAY_AUTO" : senseDelay_s.ToString();
            //-- A timer will be used to set the measure delay and synchronize the measurement between the two SMUs so set the built in delay to 0.
            this._chassis.TryWrite(string.Format("smua.measure.delay = {0}", CHa_measure_delay));
            //清理buffer
            this._chassis.TryWrite("smua.nvbuffer1.clear()");
            this._chassis.TryWrite("smua.nvbuffer2.clear()");
            #endregion
            #region normal config - smuB
            this.Reset(Keithley2602BChannel.CHB);
            this.SetMode(Keithley2602BChannel.CHB, SourceMeterMode.SourceVoltageSenceCurrent);
            this.SetSenceMode(Keithley2602BChannel.CHB, SourceMeterSenceMode.SENSE_LOCAL);
            this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
            this.SetSourceVoltageRange_V(Keithley2602BChannel.CHB, 0.2);
            this.SetVoltage_V(Keithley2602BChannel.CHB, smub_source_voltage_level);
            this.SetAutoZero(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
            this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, false);
            this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHB, smub_sense_current_range);
            this.SetMeasureCurrentRange_A(Keithley2602BChannel.CHB, smub_sense_current_range);
            this.SetNPLC(Keithley2602BChannel.CHB, nplc);//0.01 

            string CHb_measure_delay = senseDelay_s == 0 ? "smub.DELAY_AUTO" : senseDelay_s.ToString();
            this._chassis.TryWrite(string.Format("smub.measure.delay = {0}", CHb_measure_delay));//0.05

            this._chassis.TryWrite("smub.nvbuffer1.clear()");
            this._chassis.TryWrite("smub.nvbuffer2.clear()");
            #endregion

            #region trigger config
            this._chassis.TryWrite(string.Format("smua.trigger.source.lineari({0}, {1}, {2})", smua_start_val, smua_stop_val, sweepPoints));
            this._chassis.TryWrite(string.Format("smua.trigger.source.limitv		= {0}", smua_complianceV));

            this._chassis.TryWrite("smua.trigger.source.action		= smua.ENABLE");
            this._chassis.TryWrite("smua.trigger.measure.action		= smua.ENABLE");
            this._chassis.TryWrite("smua.trigger.source.stimulus	= smub.trigger.MEASURE_COMPLETE_EVENT_ID");
            this._chassis.TryWrite("smua.trigger.measure.stimulus	= smua.trigger.SOURCE_COMPLETE_EVENT_ID");
            this._chassis.TryWrite("smua.trigger.measure.iv(smua.nvbuffer1, smua.nvbuffer2)");
            this._chassis.TryWrite("smua.trigger.endsweep.action	= smua.SOURCE_IDLE");
            this._chassis.TryWrite(string.Format("smua.trigger.count				= {0}", sweepPoints));

            this._chassis.TryWrite(string.Format("smub.trigger.source.linearv({0}, {1}, {2})", 0, 0, sweepPoints));
            this._chassis.TryWrite(string.Format("smub.trigger.source.limiti		= {0}", 0.2));

            this._chassis.TryWrite("smub.trigger.source.action		= smub.ENABLE");
            this._chassis.TryWrite("smub.trigger.measure.action		= smub.ENABLE");
            this._chassis.TryWrite("smub.trigger.measure.iv(smub.nvbuffer1, smub.nvbuffer2)");
            this._chassis.TryWrite("smub.trigger.arm.stimulus		= 0");
            this._chassis.TryWrite("smub.trigger.source.stimulus	    = 0");
            this._chassis.TryWrite("smub.trigger.measure.stimulus	= smua.trigger.SOURCE_COMPLETE_EVENT_ID");
            this._chassis.TryWrite("smub.trigger.endsweep.action	= smub.SOURCE_HOLD");
            this._chassis.TryWrite("smua.trigger.endsweep.action	= smua.SOURCE_HOLD");
            this._chassis.TryWrite(string.Format("smub.trigger.count				= {0}", sweepPoints));


            this._chassis.TryWrite("smua.source.output				= smua.OUTPUT_ON");
            this._chassis.TryWrite("smub.source.output				= smub.OUTPUT_ON");
            this._chassis.TryWrite("smub.trigger.initiate()");
            this._chassis.TryWrite("smua.trigger.initiate()");
            this._chassis.TryWrite("smua.trigger.source.set()");
            #endregion
            int singleSampleDelay = period * 1000 < 20 ? 20 : (int)(period * 1000);

            int estimateSweepTime = Convert.ToInt32(sweepPoints * singleSampleDelay) + 200;
            //int estimateSweepTime = Convert.ToInt32(sweepPoints * (0.1 ) * 1000) + 500;
            Thread.Sleep(estimateSweepTime);
            //while (true)
            //{
            //    this._chassis.TryWrite("*OPC?");
            //    var resp = Chassis.ReadString();

            //    if (resp.Equals("1\n"))
            //    {
            //        break;
            //    }
            //    System.Threading.Thread.Sleep(100);
            //}
            this._chassis.TryWrite("smua.source.output = smua.OUTPUT_OFF");
            this._chassis.TryWrite("smub.source.output = smub.OUTPUT_OFF");
            this._chassis.TryWrite("format.data = format.ASCII");
            this._chassis.TryWrite("format.asciiprecision = 4"); //this._chassis.TryWrite("reset()");

            string[] ret = new string[4];
            string Source1Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source1Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source2Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source2Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);

            var s1c = Source2Current.Split(',');
            var s1v = Source2Voltage.Split(',');
            var s2c = Source2Current.Split(',');
            var s2v = Source2Voltage.Split(',');

            ret[0] = Source1Current;
            ret[1] = Source1Voltage;
            ret[2] = Source2Current;
            ret[3] = Source2Voltage;
            return ret;
        }
        public string[] SweepSingleChannelUsingTimer(double smua_start_val,
                   double smua_stop_val,
                   double smua_complianceV,
                   double sourceDelay_s,
                   double senseDelay_s,
                   double nplc,
                   int sweepPoints,
                   bool pulsedMode,
                   double pulseWidth,
                   double period)
        {
            this.Reset(Keithley2602BChannel.CHA);
            #region normal config - smuA
            //输出电流/电压模式
            this.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);//0
            //四线制测量
            this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
            //关闭电流输出自动量程
            this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
            //设置输出电流量程
            this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, smua_stop_val);
            //设置输出电流上限
            this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, smua_stop_val);
            //关闭自动清零
            this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
            //设置电压量程
            this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, false);
            this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_complianceV);
            this.SetComplianceVoltage_V(Keithley2602BChannel.CHA, smua_complianceV);
            this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_complianceV);

            this.SetNPLC(Keithley2602BChannel.CHA, nplc); //0.01

            string CHa_measure_delay = senseDelay_s == 0 ? "smua.DELAY_AUTO" : senseDelay_s.ToString();
            //-- A timer will be used to set the measure delay and synchronize the measurement between the two SMUs so set the built in delay to 0.
            this._chassis.TryWrite(string.Format("smua.measure.delay = {0}", CHa_measure_delay));
            //清理buffer
            this._chassis.TryWrite("smua.nvbuffer1.clear()");
            this._chassis.TryWrite("smua.nvbuffer2.clear()");
            #endregion
            #region trigger config
            //this._chassis.TryWrite("display.trigger.clear()");
            //Timer1 contrlls the pulse period
            this._chassis.TryWrite(string.Format("smua.trigger.source.lineari({0}, {1}, {2})", smua_start_val, smua_stop_val, sweepPoints));
            this._chassis.TryWrite(string.Format("smua.trigger.source.limitv		= {0}", smua_complianceV));

            this._chassis.TryWrite(string.Format("trigger.timer[1].count = {0}", sweepPoints));//sweepPoints =1201   timerCount = 1201-1// 
            this._chassis.TryWrite(string.Format("trigger.timer[1].delay			= {0}", period));//0.1
            this._chassis.TryWrite("trigger.timer[1].passthrough	= true");
            this._chassis.TryWrite("trigger.timer[1].stimulus		=  smua.trigger.ARMED_EVENT_ID");

            //Timer2 controls the measurement
            this._chassis.TryWrite(string.Format("trigger.timer[2].count = {0}", 1));
            // Set the measure delay long enough so that measurements start after the pulse
            // has settled, but short enough that it fits within the width of the pulse. 
            //this._chassis.TryWrite(string.Format("trigger.timer[2].delay			= {0}", pulseWidth - (1 / 50) * nplc - 60e-6));
            this._chassis.TryWrite(string.Format("trigger.timer[2].delay			= {0}", senseDelay_s));
            this._chassis.TryWrite("trigger.timer[2].passthrough	= false");
            this._chassis.TryWrite("trigger.timer[2].stimulus		=  trigger.timer[1].EVENT_ID");

            if (pulsedMode)
            {
                //Timer3 controls the pulse width
                this._chassis.TryWrite(string.Format("trigger.timer[3].count = {0}", 1));
                this._chassis.TryWrite(string.Format("trigger.timer[3].delay	= {0}", pulseWidth));
                this._chassis.TryWrite("trigger.timer[3].passthrough	= false");
                this._chassis.TryWrite("trigger.timer[3].stimulus		=  trigger.timer[1].EVENT_ID");
            }


            this._chassis.TryWrite("smua.trigger.measure.action		= smua.ENABLE");
            this._chassis.TryWrite("smua.trigger.measure.iv(smua.nvbuffer1, smua.nvbuffer2)");
            if (pulsedMode)
            {
                this._chassis.TryWrite("smua.trigger.endpulse.stimulus	= trigger.timer[3].EVENT_ID");
                this._chassis.TryWrite("smua.trigger.endpulse.action	= smua.SOURCE_IDLE");
            }
            this._chassis.TryWrite("smua.trigger.endsweep.action	= smua.SOURCE_IDLE");
            this._chassis.TryWrite(string.Format("smua.trigger.count	= {0}", sweepPoints));
            this._chassis.TryWrite("smua.trigger.arm.stimulus		= 0");
            this._chassis.TryWrite("smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID");
            this._chassis.TryWrite("smua.trigger.measure.stimulus	= trigger.timer[2].EVENT_ID");

            this._chassis.TryWrite("smua.trigger.source.action		= smua.ENABLE");

         
            this._chassis.TryWrite("smua.source.output				= smua.OUTPUT_ON");
            this._chassis.TryWrite("smua.trigger.initiate()");
            //this._chassis.TryWrite("smua.trigger.source.set()");
            #endregion

            int estimateSweepTime_ms = Convert.ToInt32(sweepPoints * period * 1000) + 1000;
            //int estimateSweepTime = Convert.ToInt32(sweepPoints * (0.1 ) * 1000) + 500;
            Thread.Sleep(estimateSweepTime_ms);

            this._chassis.TryWrite("smua.source.output = smua.OUTPUT_OFF");
            this._chassis.TryWrite("format.data = format.ASCII");
            this._chassis.TryWrite("format.asciiprecision = 4"); //this._chassis.TryWrite("reset()");

            string[] ret = new string[4];
            string Source1Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source1Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);

            var s1c = Source1Current.Split(',');
            var s1v = Source1Voltage.Split(',');

            ret[0] = Source1Current;
            ret[1] = Source1Voltage;

            return ret;

        }
        public string[] SweepDualChannelsUsingTimer(
                   double smua_start_val,
                   double smua_stop_val,
                   double smua_complianceV,
                   double smub_source_voltage_level,
                   double smub_sense_current_range,
                   double sourceDelay_s,
                   double senseDelay_s,
                   double nplc,
                   int sweepPoints,
                   bool pulsedMode,
                   double pulseWidth,
                   double period) //liv
        {
            this.Reset();

            #region config timer

            //Timer1 contrlls the pulse period
            this._chassis.TryWrite(string.Format("trigger.timer[1].count = {0}", sweepPoints));//sweepPoints =1201   timerCount = 1201-1// 
            this._chassis.TryWrite(string.Format("trigger.timer[1].delay			= {0}", period));//0.1
            this._chassis.TryWrite("trigger.timer[1].passthrough	= true");
            this._chassis.TryWrite("trigger.timer[1].stimulus =  smua.trigger.ARMED_EVENT_ID");

            //Timer2 controls the measurement
            this._chassis.TryWrite(string.Format("trigger.timer[2].count = {0}", 1));
            // Set the measure delay long enough so that measurements start after the pulse
            // has settled, but short enough that it fits within the width of the pulse. 
            this._chassis.TryWrite(string.Format("trigger.timer[2].delay = {0}", senseDelay_s));
            this._chassis.TryWrite("trigger.timer[2].passthrough	= false");
            this._chassis.TryWrite("trigger.timer[2].stimulus = trigger.timer[1].EVENT_ID");

            if (pulsedMode)
            {
                //Timer3 controls the pulse width
                this._chassis.TryWrite(string.Format("trigger.timer[3].count = {0}", 1));
                this._chassis.TryWrite(string.Format("trigger.timer[3].delay	= {0}", pulseWidth));
                this._chassis.TryWrite("trigger.timer[3].passthrough	= false");
                this._chassis.TryWrite("trigger.timer[3].stimulus =  trigger.timer[1].EVENT_ID");
            }
            #endregion

            #region SMU A
            //输出电流/电压模式
            this.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);//0
            //四线制测量
            this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
            //关闭电流输出自动量程
            this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
            //设置输出电流量程
            this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, smua_stop_val);
            //设置输出电流上限
            this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, smua_stop_val);
            //关闭自动清零
            this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
            //设置电压量程
            this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, false);
            this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_complianceV);
            this.SetComplianceVoltage_V(Keithley2602BChannel.CHA, smua_complianceV);
            this.SetNPLC(Keithley2602BChannel.CHA, nplc);
            this._chassis.TryWrite(string.Format("smua.trigger.source.lineari({0}, {1}, {2})", smua_start_val, smua_stop_val, sweepPoints));
            this._chassis.TryWrite(string.Format("smua.trigger.source.limitv	= {0}", smua_complianceV));
            this._chassis.TryWrite("smua.trigger.measure.action = smua.ENABLE");
            this._chassis.TryWrite("smua.trigger.measure.iv(smua.nvbuffer1, smua.nvbuffer2)");
            if (pulsedMode)
            {
                this._chassis.TryWrite("smua.trigger.endpulse.stimulus	= trigger.timer[3].EVENT_ID");
                this._chassis.TryWrite("smua.trigger.endpulse.action	= smua.SOURCE_IDLE");
            }
            this._chassis.TryWrite("smua.trigger.endsweep.action	= smua.SOURCE_IDLE");
            this._chassis.TryWrite(string.Format("smua.trigger.count	= {0}", sweepPoints));
            this._chassis.TryWrite("smua.trigger.arm.stimulus = 0");
            this._chassis.TryWrite("smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID");
            this._chassis.TryWrite("smua.trigger.measure.stimulus = trigger.timer[2].EVENT_ID");
            this._chassis.TryWrite("smua.trigger.source.action = smua.ENABLE");
            this._chassis.TryWrite("smua.nvbuffer1.clear()");
            this._chassis.TryWrite("smua.nvbuffer2.clear()");
            #endregion
            #region SMU B
            this.SetMode(Keithley2602BChannel.CHB, SourceMeterMode.SourceVoltageSenceCurrent);
            this.SetSenceMode(Keithley2602BChannel.CHB, SourceMeterSenceMode.SENSE_LOCAL);
            this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
            this.SetSourceVoltageRange_V(Keithley2602BChannel.CHB, smub_source_voltage_level);
            this.SetVoltage_V(Keithley2602BChannel.CHB, smub_source_voltage_level);
            this.SetAutoZero(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
            this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, false);
            this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHB, smub_sense_current_range);
            this.SetMeasureCurrentRange_A(Keithley2602BChannel.CHB, smub_sense_current_range);
            this.SetNPLC(Keithley2602BChannel.CHB, nplc);
            string listVal = "{" + smub_source_voltage_level.ToString() + "}";
            this._chassis.TryWrite($"smub.trigger.source.listv({listVal})");
            this._chassis.TryWrite(string.Format("smub.trigger.source.limiti	= {0}", smub_sense_current_range));
            if (pulsedMode)
            {
                this._chassis.TryWrite("smub.trigger.endpulse.action	= smub.SOURCE_IDLE");
                this._chassis.TryWrite("smub.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID");
            }
            this._chassis.TryWrite(string.Format("smub.trigger.count	= {0}", sweepPoints));
            this._chassis.TryWrite("smub.trigger.source.action = smub.ENABLE");
            this._chassis.TryWrite("smub.trigger.measure.action = smub.ENABLE");

            this._chassis.TryWrite("smub.trigger.endsweep.action	= smub.SOURCE_IDLE");
            this._chassis.TryWrite("smub.trigger.measure.iv(smub.nvbuffer1, smub.nvbuffer2)");
            this._chassis.TryWrite("smub.trigger.arm.stimulus		= 0");
            this._chassis.TryWrite("smub.trigger.source.stimulus	    = trigger.timer[1].EVENT_ID");
            this._chassis.TryWrite("smub.trigger.measure.stimulus	= trigger.timer[2].EVENT_ID");

            this._chassis.TryWrite("smub.nvbuffer1.clear()");
            this._chassis.TryWrite("smub.nvbuffer2.clear()");
            #endregion

            #region start trigger
            this._chassis.TryWrite("smub.source.output = smub.OUTPUT_ON");
            Thread.Sleep(100);
            this._chassis.TryWrite("smua.source.output = smua.OUTPUT_ON");           
            this._chassis.TryWrite("smub.trigger.initiate()");
            this._chassis.TryWrite("smua.trigger.initiate()");
            int estimateSweepTime_ms = Convert.ToInt32(sweepPoints * period * 1000) + 1000;
            this._chassis.Timeout_ms = estimateSweepTime_ms * 2;
            this._chassis.TryWrite("waitcomplete()");
            Thread.Sleep(estimateSweepTime_ms);
            #endregion
            #region Query Data
            this._chassis.TryWrite("smua.source.output = smua.OUTPUT_OFF");
            this._chassis.TryWrite("smub.source.output = smub.OUTPUT_OFF");
            this._chassis.TryWrite("format.data = format.ASCII");
            this._chassis.TryWrite("format.asciiprecision = 4"); //Chassis.Write("reset()");

            string[] ret = new string[4];
            string Source1Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source1Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source2Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source2Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);

            var s1c = Source1Current.Split(',');
            var s1v = Source1Voltage.Split(',');
            var s2c = Source2Current.Split(',');
            var s2v = Source2Voltage.Split(',');

            ret[0] = Source1Current;
            ret[1] = Source1Voltage;
            ret[2] = Source2Current;
            ret[3] = Source2Voltage;
            return ret;
            #endregion
      
        #region 备份20230914
        //this.Reset();
        //#region normal config - smuA
        ////输出电流/电压模式
        //this.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);//0
        ////四线制测量
        //this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
        ////关闭电流输出自动量程
        //this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
        ////设置输出电流量程
        //this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, smua_stop_val);
        ////设置输出电流上限
        //this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, smua_stop_val);
        ////关闭自动清零
        //this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
        ////设置电压量程
        //this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, false);
        //this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_complianceV);
        //this.SetComplianceVoltage_V(Keithley2602BChannel.CHA, smua_complianceV);
        //this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_complianceV);

        //this.SetNPLC(Keithley2602BChannel.CHA, nplc); //0.01

        //string CHa_measure_delay = senseDelay_s == 0 ? "smua.DELAY_AUTO" : senseDelay_s.ToString();
        ////-- A timer will be used to set the measure delay and synchronize the measurement between the two SMUs so set the built in delay to 0.
        //this._chassis.TryWrite(string.Format("smua.measure.delay = {0}", CHa_measure_delay));
        ////清理buffer
        //this._chassis.TryWrite("smua.nvbuffer1.clear()");
        //this._chassis.TryWrite("smua.nvbuffer2.clear()");
        //#endregion
        //#region normal config - smuB
        //this.Reset(Keithley2602BChannel.CHB);
        //this.SetMode(Keithley2602BChannel.CHB, SourceMeterMode.SourceVoltageSenceCurrent);
        //this.SetSenceMode(Keithley2602BChannel.CHB, SourceMeterSenceMode.SENSE_LOCAL);
        //this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
        ////this.SetSourceVoltageRange_V(Keithley2602BChannel.CHB, 0.2);
        //////this.SetVoltage_V(Keithley2602BChannel.CHB, smub_source_voltage_level);
        ////this._chassis.TryWrite(string.Format("smub.trigger.source.listv({{0}})", smub_source_voltage_level));
        //this.SetSourceVoltageRange_V(Keithley2602BChannel.CHB, smub_source_voltage_level);
        //this.SetVoltage_V(Keithley2602BChannel.CHB, smub_source_voltage_level);
        //this.SetAutoZero(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
        //this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, false);
        //this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHB, smub_sense_current_range);
        //this.SetMeasureCurrentRange_A(Keithley2602BChannel.CHB, smub_sense_current_range);
        //this.SetNPLC(Keithley2602BChannel.CHB, nplc);//0.01 

        ////this._chassis.TryWrite(string.Format("smub.trigger.source.linearv({0}, {1}, {2})", smub_source_voltage_level, smub_source_voltage_level, sweepPoints));
        //string listVal = "{" + smub_source_voltage_level.ToString() + "}";
        //this._chassis.TryWrite($"smub.trigger.source.listv({listVal})");
        //string CHb_measure_delay = senseDelay_s == 0 ? "smub.DELAY_AUTO" : senseDelay_s.ToString();
        //this._chassis.TryWrite(string.Format("smub.measure.delay = {0}", CHb_measure_delay));//0.05

        //this._chassis.TryWrite("smub.nvbuffer1.clear()");
        //this._chassis.TryWrite("smub.nvbuffer2.clear()");
        //#endregion
        //#region trigger config
        ////this._chassis.TryWrite("display.trigger.clear()");
        ////Timer1 contrlls the pulse period
        //this._chassis.TryWrite(string.Format("smua.trigger.source.lineari({0}, {1}, {2})", smua_start_val, smua_stop_val, sweepPoints));
        //this._chassis.TryWrite(string.Format("smua.trigger.source.limitv		= {0}", smua_complianceV));

        //this._chassis.TryWrite(string.Format("trigger.timer[1].count = {0}", sweepPoints));//sweepPoints =1201   timerCount = 1201-1// 
        //this._chassis.TryWrite(string.Format("trigger.timer[1].delay			= {0}", period));//0.1
        //this._chassis.TryWrite("trigger.timer[1].passthrough	= true");
        //this._chassis.TryWrite("trigger.timer[1].stimulus		=  smua.trigger.ARMED_EVENT_ID");

        ////Timer2 controls the measurement
        //this._chassis.TryWrite(string.Format("trigger.timer[2].count = {0}", 1));
        //// Set the measure delay long enough so that measurements start after the pulse
        //// has settled, but short enough that it fits within the width of the pulse. 
        ////this._chassis.TryWrite(string.Format("trigger.timer[2].delay			= {0}", pulseWidth - (1 / 50) * nplc - 60e-6));
        //this._chassis.TryWrite(string.Format("trigger.timer[2].delay={0}", senseDelay_s));
        //this._chassis.TryWrite("trigger.timer[2].passthrough	= false");
        //this._chassis.TryWrite("trigger.timer[2].stimulus		=  trigger.timer[1].EVENT_ID");

        //if (pulsedMode)
        //{
        //    //Timer3 controls the pulse width
        //    this._chassis.TryWrite(string.Format("trigger.timer[3].count = {0}", 1));
        //    this._chassis.TryWrite(string.Format("trigger.timer[3].delay	= {0}", pulseWidth));
        //    this._chassis.TryWrite("trigger.timer[3].passthrough	= false");
        //    this._chassis.TryWrite("trigger.timer[3].stimulus		=  trigger.timer[1].EVENT_ID");
        //}


        //this._chassis.TryWrite("smua.trigger.measure.action		= smua.ENABLE");
        //this._chassis.TryWrite("smua.trigger.measure.iv(smua.nvbuffer1, smua.nvbuffer2)");
        //if (pulsedMode)
        //{
        //    this._chassis.TryWrite("smua.trigger.endpulse.stimulus	= trigger.timer[3].EVENT_ID");
        //    this._chassis.TryWrite("smua.trigger.endpulse.action	= smua.SOURCE_IDLE");
        //}
        //this._chassis.TryWrite("smua.trigger.endsweep.action	= smua.SOURCE_IDLE");
        //this._chassis.TryWrite(string.Format("smua.trigger.count	= {0}", sweepPoints));
        //this._chassis.TryWrite("smua.trigger.arm.stimulus		= 0");
        //this._chassis.TryWrite("smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID");
        //this._chassis.TryWrite("smua.trigger.measure.stimulus	= trigger.timer[2].EVENT_ID");

        //this._chassis.TryWrite("smua.trigger.source.action		= smua.ENABLE");

        //this._chassis.TryWrite(string.Format("smub.trigger.source.linearv({0}, {1}, {2})", 0, 0, sweepPoints));
        //this._chassis.TryWrite(string.Format("smub.trigger.source.limiti		= {0}", 0.2));

        //if (pulsedMode)
        //{
        //    this._chassis.TryWrite("smub.trigger.endpulse.action	= smub.SOURCE_IDLE");
        //    this._chassis.TryWrite("smub.trigger.endpulse.stimulus	= trigger.timer[3].EVENT_ID");
        //}
        //this._chassis.TryWrite(string.Format("smub.trigger.count	= {0}", sweepPoints));
        //this._chassis.TryWrite("smub.trigger.source.action		= smub.ENABLE");
        //this._chassis.TryWrite("smub.trigger.measure.action		= smub.ENABLE");

        //this._chassis.TryWrite("smub.trigger.endsweep.action	= smub.SOURCE_IDLE");
        //this._chassis.TryWrite("smub.trigger.measure.iv(smub.nvbuffer1, smub.nvbuffer2)");
        //this._chassis.TryWrite("smub.trigger.arm.stimulus		= 0");
        //this._chassis.TryWrite("smub.trigger.source.stimulus	    = trigger.timer[1].EVENT_ID");
        //this._chassis.TryWrite("smub.trigger.measure.stimulus	= trigger.timer[2].EVENT_ID");


        //this._chassis.TryWrite("smua.source.output				= smua.OUTPUT_ON");
        //this._chassis.TryWrite("smub.source.output				= smub.OUTPUT_ON");
        //this._chassis.TryWrite("smub.trigger.initiate()");
        //this._chassis.TryWrite("smua.trigger.initiate()");
        ////this._chassis.TryWrite("smua.trigger.source.set()");
        //#endregion
        //int estimateSweepTime_ms = Convert.ToInt32(sweepPoints * period * 1000) + 1000;
        ////int estimateSweepTime = Convert.ToInt32(sweepPoints * (0.1 ) * 1000) + 500;
        //Thread.Sleep(estimateSweepTime_ms);

        //this._chassis.TryWrite("smua.source.output = smua.OUTPUT_OFF");
        //this._chassis.TryWrite("smub.source.output = smub.OUTPUT_OFF");
        //this._chassis.TryWrite("format.data = format.ASCII");
        //this._chassis.TryWrite("format.asciiprecision = 4"); //this._chassis.TryWrite("reset()");

        //string[] ret = new string[4];
        //string Source1Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
        //string Source1Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);
        //string Source2Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
        //string Source2Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);

        //var s1c = Source1Current.Split(',');
        //var s1v = Source1Voltage.Split(',');
        //var s2c = Source2Current.Split(',');
        //var s2v = Source2Voltage.Split(',');

        //ret[0] = Source1Current;
        //ret[1] = Source1Voltage;
        //ret[2] = Source2Current;
        //ret[3] = Source2Voltage;
        //return ret;
        #endregion
    }
        /// <summary>
        /// 生成连续固定幅度的脉冲电流，并同步采样通道B的电流
        /// </summary>
        /// <param name="smua_pulseLevel"></param>
        /// <param name="smua_complianceV"></param>
        /// <param name="smub_source_voltage_level"></param>
        /// <param name="smub_sense_current_range"></param>
        /// <param name="senseDelay_s"></param>
        /// <param name="nplc"></param>
        /// <param name="pulseCount"></param>
        /// <param name="pulseWidth"></param>
        /// <param name="period"></param>
        /// <returns></returns>

        public string[] PulseTrainSyncSampling(
          double smua_pulseLevel,
          double smua_complianceV,
          double smub_source_voltage_level,
          double smub_sense_current_range,
          double senseDelay_s,
          double nplc,
          int pulseCount,
          double pulseWidth,
          double period)
        {
            this.Reset();

            #region config timer

            //Timer1 contrlls the pulse period
            this._chassis.TryWrite(string.Format("trigger.timer[1].count = {0}", pulseCount));//sweepPoints =1201   timerCount = 1201-1// 
            this._chassis.TryWrite(string.Format("trigger.timer[1].delay			= {0}", period));//0.1
            this._chassis.TryWrite("trigger.timer[1].passthrough	= true");
            this._chassis.TryWrite("trigger.timer[1].stimulus =  smua.trigger.ARMED_EVENT_ID");

            //Timer2 controls the measurement
            this._chassis.TryWrite(string.Format("trigger.timer[2].count = {0}", 1));
            // Set the measure delay long enough so that measurements start after the pulse
            // has settled, but short enough that it fits within the width of the pulse. 
            this._chassis.TryWrite(string.Format("trigger.timer[2].delay = {0}", senseDelay_s));
            this._chassis.TryWrite("trigger.timer[2].passthrough	= false");
            this._chassis.TryWrite("trigger.timer[2].stimulus = trigger.timer[1].EVENT_ID");

            //Timer3 controls the pulse width
            this._chassis.TryWrite(string.Format("trigger.timer[3].count = {0}", 1));
            this._chassis.TryWrite(string.Format("trigger.timer[3].delay	= {0}", pulseWidth));
            this._chassis.TryWrite("trigger.timer[3].passthrough	= false");
            this._chassis.TryWrite("trigger.timer[3].stimulus =  trigger.timer[1].EVENT_ID");

            #endregion

            #region SMU A
            //输出电流/电压模式
            this.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);//0
            //四线制测量
            this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
            //关闭电流输出自动量程
            this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
            //设置输出电流量程
            this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, smua_pulseLevel);
            //设置输出电流上限
            this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, smua_pulseLevel);
            //关闭自动清零
            this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
            //设置电压量程
            this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, false);
            this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_complianceV);
            this.SetComplianceVoltage_V(Keithley2602BChannel.CHA, smua_complianceV);
            this.SetNPLC(Keithley2602BChannel.CHA, nplc);
            string sweepList = "{" + smua_pulseLevel.ToString() + "}";
            this._chassis.TryWrite($"smua.trigger.source.listi({sweepList})");
            this._chassis.TryWrite(string.Format("smua.trigger.source.limitv	= {0}", smua_complianceV));
            this._chassis.TryWrite("smua.trigger.measure.action = smua.ENABLE");
            this._chassis.TryWrite("smua.trigger.measure.iv(smua.nvbuffer1, smua.nvbuffer2)");
            this._chassis.TryWrite("smua.trigger.endpulse.stimulus	= trigger.timer[3].EVENT_ID");
            this._chassis.TryWrite("smua.trigger.endpulse.action	= smua.SOURCE_IDLE");
            this._chassis.TryWrite("smua.trigger.endsweep.action	= smua.SOURCE_IDLE");
            this._chassis.TryWrite(string.Format("smua.trigger.count	= {0}", pulseCount));
            this._chassis.TryWrite("smua.trigger.arm.stimulus = 0");
            this._chassis.TryWrite("smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID");
            this._chassis.TryWrite("smua.trigger.measure.stimulus = trigger.timer[2].EVENT_ID");
            this._chassis.TryWrite("smua.trigger.source.action = smua.ENABLE");
            this._chassis.TryWrite("smua.nvbuffer1.clear()");
            this._chassis.TryWrite("smua.nvbuffer2.clear()");
            #endregion
            #region SMU B
            this.SetMode(Keithley2602BChannel.CHB, SourceMeterMode.SourceVoltageSenceCurrent);
            this.SetSenceMode(Keithley2602BChannel.CHB, SourceMeterSenceMode.SENSE_LOCAL);
            this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
            this.SetSourceVoltageRange_V(Keithley2602BChannel.CHB, smub_source_voltage_level);
            this.SetVoltage_V(Keithley2602BChannel.CHB, smub_source_voltage_level);
            this.SetAutoZero(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
            this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, false);
            this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHB, smub_sense_current_range);
            this.SetMeasureCurrentRange_A(Keithley2602BChannel.CHB, smub_sense_current_range);
            this.SetNPLC(Keithley2602BChannel.CHB, nplc);


            sweepList = "{" + smub_source_voltage_level.ToString() + "}";
            this._chassis.TryWrite($"smub.trigger.source.listv({sweepList})");
            this._chassis.TryWrite(string.Format("smub.trigger.source.limiti	= {0}", smub_sense_current_range));

            this._chassis.TryWrite("smub.trigger.endpulse.action	= smub.SOURCE_IDLE");
            this._chassis.TryWrite("smub.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID");

            this._chassis.TryWrite(string.Format("smub.trigger.count	= {0}", pulseCount));
            this._chassis.TryWrite("smub.trigger.source.action = smub.ENABLE");
            this._chassis.TryWrite("smub.trigger.measure.action = smub.ENABLE");

            this._chassis.TryWrite("smub.trigger.endsweep.action	= smub.SOURCE_IDLE");
            this._chassis.TryWrite("smub.trigger.measure.iv(smub.nvbuffer1, smub.nvbuffer2)");
            this._chassis.TryWrite("smub.trigger.arm.stimulus		= 0");
            this._chassis.TryWrite("smub.trigger.source.stimulus	    = trigger.timer[1].EVENT_ID");
            this._chassis.TryWrite("smub.trigger.measure.stimulus	= trigger.timer[2].EVENT_ID");

            this._chassis.TryWrite("smub.nvbuffer1.clear()");
            this._chassis.TryWrite("smub.nvbuffer2.clear()");
            #endregion

            #region start trigger

            this._chassis.TryWrite("smub.source.output = smub.OUTPUT_ON");
            Thread.Sleep(100);
            this._chassis.TryWrite("smua.source.output = smua.OUTPUT_ON");

            this._chassis.TryWrite("smub.trigger.initiate()");
            this._chassis.TryWrite("smua.trigger.initiate()");
            int estimateSweepTime_ms = Convert.ToInt32(pulseCount * period * 1000) + 1000;
            this._chassis.Timeout_ms = estimateSweepTime_ms * 2;
            this._chassis.TryWrite("waitcomplete()");
            #endregion

            this._chassis.TryWrite("smua.source.output = smua.OUTPUT_OFF");
            this._chassis.TryWrite("smub.source.output = smub.OUTPUT_OFF");
            this._chassis.TryWrite("format.data = format.ASCII");
            this._chassis.TryWrite("format.asciiprecision = 4"); //Chassis.Write("reset()");

            string[] ret = new string[4];
            string Source1Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer1.readings)", pulseCount), defaultRespondingTime_ms);
            string Source1Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer2.readings)", pulseCount), defaultRespondingTime_ms);
            string Source2Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer1.readings)", pulseCount), defaultRespondingTime_ms);
            string Source2Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer2.readings)", pulseCount), defaultRespondingTime_ms);


            var s1c = Source1Current.Split(',');
            var s1v = Source1Voltage.Split(',');
            var s2c = Source2Current.Split(',');
            var s2v = Source2Voltage.Split(',');

            ret[0] = Source1Current;
            ret[1] = Source1Voltage;
            ret[2] = Source2Current;
            ret[3] = Source2Voltage;
            return ret;
        }
        
        public string[] PulsedSweepDualChannels(SourceMeterMode smua_mode,
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
                                                double nplc,
                                                double pulseWidth,
                                                double pulsePeriod)
        {
            this.Reset();
            #region normal config - smuA

            this.Reset(Keithley2602BChannel.CHA);
            //输出电流/电压模式
            this.SetMode(Keithley2602BChannel.CHA, smua_mode);//0

            switch (smua_mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    {
                        //四线制测量
                        this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
                        //关闭电流输出自动量程
                        this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
                        //this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
                        //设置输出电流量程
                        this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, smua_source_current_range);
                        this.SetCurrent_A(Keithley2602BChannel.CHA, 0);
                        this.SetSourceVoltageLimit_V(Keithley2602BChannel.CHA, smua_source_voltage_limit);
                        //================================================================================
                        //this.SetSourceVoltageRange_V(Keithley2602BChannel.CHA, smua_source_voltage_range);
                        //================================================================================
                      
                        ////设置输出电流上限
                        //this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, smua_source_current_limit);
               
                        //关闭自动清零
                        this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
                        //关闭测量自动量程
                        this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, false);
                        //设置电压量程3V
                        this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_measure_voltage_range);
                    }
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    {
                        //四线制测量
                        this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
                        //关闭电流输出自动量程
                        this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
                        this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
                        //设置输出电流量程
                        this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, smua_source_current_range);
                        this.SetSourceVoltageRange_V(Keithley2602BChannel.CHA, smua_source_voltage_range);
                        //设置输出电流上限
                        this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, smua_source_current_limit);
                        this.SetSourceVoltageLimit_V(Keithley2602BChannel.CHA, smua_source_voltage_limit);
                        //关闭自动清零
                        this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
                        //关闭测量自动量程
                        this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, false);
                        //设置电流量程3V
                        this.SetMeasureCurrentRange_A(Keithley2602BChannel.CHA, smua_measure_current_range);
                    }
                    break;
            }
            this.SetNPLC(Keithley2602BChannel.CHA, nplc); //0.01

     
            //-- A timer will be used to set the measure delay and synchronize the measurement between the two SMUs so set the built in delay to 0.
            string CHa_measure_delay = "0";
            this._chassis.TryWrite(string.Format("smua.measure.delay = {0}", CHa_measure_delay));

            //清理buffer
            this._chassis.TryWrite("smua.nvbuffer1.clear()");
            //this._chassis.TryWrite("smua.nvbuffer1.collecttimestamps= 1");
            this._chassis.TryWrite("smua.nvbuffer2.clear()");
            //this._chassis.TryWrite("smua.nvbuffer2.collecttimestamps= 1");
            #endregion
            #region normal config - smuB
            this.Reset(Keithley2602BChannel.CHB);
            this.SetMode(Keithley2602BChannel.CHB, smub_mode);

            switch (smub_mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    {
                        this.SetSenceMode(Keithley2602BChannel.CHB, SourceMeterSenceMode.SENSE_LOCAL);
                        this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
                        this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
                        //set pulse value = 0 , because no pulse require in this case 
                        //this.SetCurrentPulseLevel(Keithley2602BChannel.CHB, 0);
                        //================================================================================
                        this.SetSourceCurrentRange_A(Keithley2602BChannel.CHB, smub_source_current_range);
                        //================================================================================
                        this.SetSourceVoltageRange_V(Keithley2602BChannel.CHB, smub_source_voltage_range);

                        this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHB, smub_source_current_limit);
                        //================================================================================
                        this.SetSourceVoltageLimit_V(Keithley2602BChannel.CHB, smub_source_voltage_limit);
                        //================================================================================
                        this.SetAutoZero(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
                        this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, false);
                        this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHB, smua_measure_voltage_range);
                    }
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    {
                        this.SetSenceMode(Keithley2602BChannel.CHB, SourceMeterSenceMode.SENSE_LOCAL);
                        //this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
                        this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
                        //set pulse value = 0 , because no pulse require in this case 
                        //this.SetVoltagePulseLevel(Keithley2602BChannel.CHB, 0);
                        //================================================================================
                        //this.SetSourceCurrentRange_A(Keithley2602BChannel.CHB, smub_source_current_range);
                        //================================================================================
                        this.SetSourceVoltageRange_V(Keithley2602BChannel.CHB, smub_source_voltage_range);
                        this.SetVoltage_V(Keithley2602BChannel.CHB, 1);
                        //this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHB, smub_source_current_limit);
                        //================================================================================
                        this.SetSourceVoltageLimit_V(Keithley2602BChannel.CHB, smub_source_voltage_limit);
                        //================================================================================
                        this.SetAutoZero(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
                        this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, false);
                        this.SetMeasureCurrentRange_A(Keithley2602BChannel.CHB, smub_measure_current_range);
                    }
                    break;
            }
            this.SetNPLC(Keithley2602BChannel.CHB, nplc);

            string CHb_measure_delay = "0";
            this._chassis.TryWrite(string.Format("smub.measure.delay = {0}", CHb_measure_delay));//0.05

            this._chassis.TryWrite("smub.nvbuffer1.clear()");
            this._chassis.TryWrite("smub.nvbuffer2.clear()");
 
            #endregion

            #region trigger config
            //this._chassis.TryWrite("display.trigger.clear()");
            //Timer1 contrlls the pulse period
            this._chassis.TryWrite(string.Format("trigger.timer[1].count = {0}", sweepPoints));//sweepPoints =1201   timerCount = 1201-1// 
            this._chassis.TryWrite(string.Format("trigger.timer[1].delay			= {0}", pulsePeriod));//0.1
            this._chassis.TryWrite("trigger.timer[1].passthrough	= true");
            this._chassis.TryWrite("trigger.timer[1].stimulus		=  smua.trigger.ARMED_EVENT_ID");

            //Timer2 controls the measurement
            this._chassis.TryWrite(string.Format("trigger.timer[2].count = {0}", 1)); 
            // Set the measure delay long enough so that measurements start after the pulse
	        // has settled, but short enough that it fits within the width of the pulse. 
            this._chassis.TryWrite(string.Format("trigger.timer[2].delay			= {0}", pulseWidth - (1 / 50) * nplc - 60e-6)); 
            this._chassis.TryWrite("trigger.timer[2].passthrough	= false");
            this._chassis.TryWrite("trigger.timer[2].stimulus		=  trigger.timer[1].EVENT_ID");

            //Timer3 controls the pulse width
            this._chassis.TryWrite(string.Format("trigger.timer[3].count = {0}", 1));
            this._chassis.TryWrite(string.Format("trigger.timer[3].delay	= {0}", pulseWidth));
            this._chassis.TryWrite("trigger.timer[3].passthrough	= false");
            this._chassis.TryWrite("trigger.timer[3].stimulus		=  trigger.timer[1].EVENT_ID");

            switch (smua_mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    this._chassis.TryWrite(string.Format("smua.trigger.source.lineari({0}, {1}, {2})", smua_start_val, smua_stop_val, sweepPoints));
                    this._chassis.TryWrite(string.Format("smua.trigger.source.limitv		= {0}", smua_trigger_source_voltage_limit));
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    this._chassis.TryWrite(string.Format("smua.trigger.source.linearv({0}, {1}, {2})", smua_start_val, smua_stop_val, sweepPoints));
                    this._chassis.TryWrite(string.Format("smua.trigger.source.limiti		= {0}", smua_trigger_source_current_limit));
                    break;
            }
            this._chassis.TryWrite("smua.trigger.measure.action		= smua.ENABLE");
            this._chassis.TryWrite("smua.trigger.measure.iv(smua.nvbuffer1, smua.nvbuffer2)");
            this._chassis.TryWrite("smua.trigger.endpulse.action	= smua.SOURCE_IDLE");
	        this._chassis.TryWrite("smua.trigger.endsweep.action	= smua.SOURCE_IDLE");
	        this._chassis.TryWrite(string.Format("smua.trigger.count	= {0}", sweepPoints));
	        this._chassis.TryWrite("smua.trigger.arm.stimulus		= 0");
	        this._chassis.TryWrite("smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID");
	        this._chassis.TryWrite("smua.trigger.measure.stimulus	= trigger.timer[2].EVENT_ID");
	        this._chassis.TryWrite("smua.trigger.endpulse.stimulus	= trigger.timer[3].EVENT_ID");
	        this._chassis.TryWrite("smua.trigger.source.action		= smua.ENABLE");




            switch (smub_mode)
            {
                case SourceMeterMode.SourceCurrentSenceVoltage:
                    this._chassis.TryWrite(string.Format("smub.trigger.source.lineari({0}, {1}, {2})", smub_start_val, smub_stop_val, sweepPoints));
                    this._chassis.TryWrite(string.Format("smub.trigger.source.limitv		= {0}", smub_trigger_source_voltage_limit));
                    break;
                case SourceMeterMode.SourceVoltageSenceCurrent:
                    this._chassis.TryWrite(string.Format("smub.trigger.source.linearv({0}, {1}, {2})", smub_start_val, smub_stop_val, sweepPoints));
                    this._chassis.TryWrite(string.Format("smub.trigger.source.limiti		= {0}", smub_trigger_source_current_limit));
                    break;
            }

            this._chassis.TryWrite(string.Format("smub.trigger.count	= {0}", sweepPoints));  
            this._chassis.TryWrite("smub.trigger.source.action		= smub.ENABLE");
            this._chassis.TryWrite("smub.trigger.measure.action		= smub.ENABLE");   
            this._chassis.TryWrite("smub.trigger.endpulse.action	= smub.SOURCE_IDLE");
	        this._chassis.TryWrite("smub.trigger.endsweep.action	= smub.SOURCE_IDLE");
            this._chassis.TryWrite("smub.trigger.measure.iv(smub.nvbuffer1, smub.nvbuffer2)");
            this._chassis.TryWrite("smub.trigger.arm.stimulus		= 0");
            this._chassis.TryWrite("smub.trigger.source.stimulus	    = trigger.timer[1].EVENT_ID");
            this._chassis.TryWrite("smub.trigger.measure.stimulus	= trigger.timer[2].EVENT_ID");
            this._chassis.TryWrite("smub.trigger.endpulse.stimulus	= trigger.timer[3].EVENT_ID");
       



            this._chassis.TryWrite("smua.source.output				= smua.OUTPUT_ON");
            this._chassis.TryWrite("smub.source.output				= smub.OUTPUT_ON");
            this._chassis.TryWrite("smub.trigger.initiate()");
            this._chassis.TryWrite("smua.trigger.initiate()");
            //this._chassis.TryWrite("smua.trigger.source.set()");
            #endregion
 

            int estimateSweepTime = Convert.ToInt32(sweepPoints * pulsePeriod * 1000) + 1000;
            //int estimateSweepTime = Convert.ToInt32(sweepPoints * (0.1 ) * 1000) + 500;
            Thread.Sleep(estimateSweepTime);
  
            this._chassis.TryWrite("smua.source.output = smua.OUTPUT_OFF");
            this._chassis.TryWrite("smub.source.output = smub.OUTPUT_OFF");
            this._chassis.TryWrite("format.data = format.ASCII");
            this._chassis.TryWrite("format.asciiprecision = 4"); //this._chassis.TryWrite("reset()");

            string[] ret = new string[4];
            string Source1Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source1Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source2Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer1.readings)", sweepPoints), defaultRespondingTime_ms);
            string Source2Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer2.readings)", sweepPoints), defaultRespondingTime_ms);

            var s1c = Source2Current.Split(',');
            var s1v = Source2Voltage.Split(',');
            var s2c = Source2Current.Split(',');
            var s2v = Source2Voltage.Split(',');

            ret[0] = Source1Current;
            ret[1] = Source1Voltage;
            ret[2] = Source2Current;
            ret[3] = Source2Voltage;
            return ret;
        }
        public void WriteCommand(string command)
        {
            this._chassis.TryWrite(command);
        }
        public string QueryCommand(string command)
        {
            return this._chassis.Query(command, defaultRespondingTime_ms);
        }
        public string[] ExternalTriggerSweep(Action startExternalTrigger,int points, double smua_nplc, double smua_delay_s)
        {
            throw new NotImplementedException();
        }

        public string[] SweepDualChannelsSYNC(
                                        SourceMeterMode smua_mode,
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
                                        double smua_delay_s,                              //SourceDelay_sec

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
                                        double smub_delay_s,                             //MeasureDelay_sec

                                        int sweepPoints,                                 //扫描点

                                        bool pulsedMode,                                 //脉冲开关
                                        double DutyRatio,                               //占空比

                                        double TimerPeriod_s                                //Timer时间
                                        )
        {
            throw new NotImplementedException();
        }
        public void GenerateInfinitePulseTrain(double pulseLevel_A, double compliance_V, double pulseWidth, double period)
        {
            this.Reset();
            //输出电流模式
            this.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);//0
            //四线制测量
            this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_LOCAL);
            //关闭电流输出自动量程
            this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
            //设置输出电流量程
            this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, pulseLevel_A);
            //设置输出电流上限
            this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, pulseLevel_A);
            //关闭自动清零
            this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
            //设置电压量程
            this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, false);
            this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, compliance_V);
            this.SetComplianceVoltage_V(Keithley2602BChannel.CHA, compliance_V);
            this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, compliance_V);


           this._chassis.TryWrite("smua.trigger.source.listi({" + pulseLevel_A + "})");
           this._chassis.TryWrite(string.Format("smua.trigger.source.limitv	= {0}", compliance_V));
           this._chassis.TryWrite(string.Format("smua.trigger.source.action = smua.ENABLE"));
           this._chassis.TryWrite(string.Format("smua.trigger.measure.action = smua.DISABLE"));

           this._chassis.TryWrite(string.Format("trigger.timer[1].count = 0"));
           this._chassis.TryWrite(string.Format("trigger.timer[1].delay = {0}", period));
           this._chassis.TryWrite("trigger.timer[1].passthrough	= true");
           this._chassis.TryWrite("trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID");

           this._chassis.TryWrite(string.Format("trigger.timer[2].count = 1"));
           this._chassis.TryWrite(string.Format("trigger.timer[2].delay = {0}", pulseWidth));
           this._chassis.TryWrite("trigger.timer[2].passthrough	= false");
           this._chassis.TryWrite("trigger.timer[2].stimulus = trigger.timer[1].EVENT_ID");

           this._chassis.TryWrite("smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID");
           this._chassis.TryWrite("smua.trigger.endpulse.action	= smua.SOURCE_IDLE");
           this._chassis.TryWrite("smua.trigger.endpulse.stimulus = trigger.timer[2].EVENT_ID");
           this._chassis.TryWrite("smua.trigger.count = 0");
           this._chassis.TryWrite("smua.source.output = smua.OUTPUT_ON");
           this._chassis.TryWrite("smua.trigger.initiate()");

        }
        public void StopPulseTrain()
        {
            this._chassis.TryWrite("smua.abort()");
            this.Reset();
        }
        #region 前面有个新的
        //public string[] PulseTrainSyncSampling(
        //   double smua_pulseLevel,
        //   double smua_complianceV,
        //   double smub_source_voltage_level,
        //   double smub_sense_current_range,
        //   double senseDelay_s,
        //   double nplc,
        //   int pulseCount,
        //   double pulseWidth,
        //   double period)
        //{
        //    this.Reset();
        //    #region normal config - smuA
        //    //输出电流/电压模式
        //    this.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceCurrentSenceVoltage);//0
        //    //四线制测量
        //    this.SetSenceMode(Keithley2602BChannel.CHA, SourceMeterSenceMode.SENSE_REMOTE);
        //    //关闭电流输出自动量程
        //    this.SetAutoRange_I_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Source, false);
        //    //设置输出电流量程
        //    this.SetSourceCurrentRange_A(Keithley2602BChannel.CHA, smua_pulseLevel);
        //    //设置输出电流上限
        //    this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHA, smua_pulseLevel);
        //    //关闭自动清零
        //    this.SetAutoZero(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
        //    //设置电压量程
        //    this.SetAutoRange_V_Enable(Keithley2602BChannel.CHA, SourceMeterFuncitonMode.Measure, false);
        //    this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_complianceV);
        //    this.SetComplianceVoltage_V(Keithley2602BChannel.CHA, smua_complianceV);
        //    this.SetMeasureVoltageRange_V(Keithley2602BChannel.CHA, smua_complianceV);

        //    this.SetNPLC(Keithley2602BChannel.CHA, nplc); //0.01

        //    string CHa_measure_delay = senseDelay_s == 0 ? "smua.DELAY_AUTO" : senseDelay_s.ToString();
        //    //-- A timer will be used to set the measure delay and synchronize the measurement between the two SMUs so set the built in delay to 0.
        //    this._chassis.TryWrite(string.Format("smua.measure.delay = {0}", CHa_measure_delay));
        //    //清理buffer
        //    this._chassis.TryWrite("smua.nvbuffer1.clear()");
        //    this._chassis.TryWrite("smua.nvbuffer2.clear()");
        //    #endregion
        //    #region normal config - smuB
        //    this.Reset(Keithley2602BChannel.CHB);
        //    this.SetMode(Keithley2602BChannel.CHB, SourceMeterMode.SourceVoltageSenceCurrent);
        //    this.SetSenceMode(Keithley2602BChannel.CHB, SourceMeterSenceMode.SENSE_LOCAL);
        //    this.SetAutoRange_V_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Source, false);
        //    this.SetSourceVoltageRange_V(Keithley2602BChannel.CHB, 0.2);
        //    this.SetVoltage_V(Keithley2602BChannel.CHB, smub_source_voltage_level);
        //    this.SetAutoZero(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);
        //    this.SetAutoRange_I_Enable(Keithley2602BChannel.CHB, SourceMeterFuncitonMode.Measure, false);
        //    this.SetSourceCurrentLimit_A(Keithley2602BChannel.CHB, smub_sense_current_range);
        //    this.SetMeasureCurrentRange_A(Keithley2602BChannel.CHB, smub_sense_current_range);
        //    this.SetNPLC(Keithley2602BChannel.CHB, nplc);//0.01 

        //    string CHb_measure_delay = senseDelay_s == 0 ? "smub.DELAY_AUTO" : senseDelay_s.ToString();
        //    this._chassis.TryWrite(string.Format("smub.measure.delay = {0}", CHb_measure_delay));//0.05

        //    this._chassis.TryWrite("smub.nvbuffer1.clear()");
        //    this._chassis.TryWrite("smub.nvbuffer2.clear()");
        //    #endregion
        //    #region trigger config
        //    //Chassis.Write("display.trigger.clear()");
        //    //Timer1 contrlls the pulse period
        //    this._chassis.TryWrite("smua.trigger.source.listi({" + smua_pulseLevel + "})");
        //    //Chassis.Write(string.Format("smua.trigger.source.listi({0}, {1}, {2})", smua_start_val, smua_stop_val, pulseCount));
        //    this._chassis.TryWrite(string.Format("smua.trigger.source.limitv		= {0}", smua_complianceV));

        //    this._chassis.TryWrite(string.Format("trigger.timer[1].count = {0}", pulseCount));//sweepPoints =1201   timerCount = 1201-1// 
        //    this._chassis.TryWrite(string.Format("trigger.timer[1].delay			= {0}", period));//0.1
        //    this._chassis.TryWrite("trigger.timer[1].passthrough	= true");
        //    this._chassis.TryWrite("trigger.timer[1].stimulus		=  smua.trigger.ARMED_EVENT_ID");

        //    //Timer2 controls the measurement
        //    this._chassis.TryWrite(string.Format("trigger.timer[2].count = {0}", 1));
        //    // Set the measure delay long enough so that measurements start after the pulse
        //    // has settled, but short enough that it fits within the width of the pulse. 
        //    //Chassis.Write(string.Format("trigger.timer[2].delay			= {0}", pulseWidth - (1 / 50) * nplc - 60e-6));
        //    this._chassis.TryWrite(string.Format("trigger.timer[2].delay			= {0}", senseDelay_s));
        //    this._chassis.TryWrite("trigger.timer[2].passthrough	= false");
        //    this._chassis.TryWrite("trigger.timer[2].stimulus		=  trigger.timer[1].EVENT_ID");


        //    //Timer3 controls the pulse width
        //    this._chassis.TryWrite(string.Format("trigger.timer[3].count = {0}", 1));
        //    this._chassis.TryWrite(string.Format("trigger.timer[3].delay	= {0}", pulseWidth));
        //    this._chassis.TryWrite("trigger.timer[3].passthrough	= false");
        //    this._chassis.TryWrite("trigger.timer[3].stimulus		=  trigger.timer[1].EVENT_ID");



        //    this._chassis.TryWrite("smua.trigger.measure.action		= smua.ENABLE");
        //    this._chassis.TryWrite("smua.trigger.measure.iv(smua.nvbuffer1, smua.nvbuffer2)");

        //    this._chassis.TryWrite("smua.trigger.endpulse.stimulus	= trigger.timer[3].EVENT_ID");
        //    this._chassis.TryWrite("smua.trigger.endpulse.action	= smua.SOURCE_IDLE");

        //    this._chassis.TryWrite("smua.trigger.endsweep.action	= smua.SOURCE_IDLE");
        //    this._chassis.TryWrite(string.Format("smua.trigger.count	= {0}", pulseCount));
        //    this._chassis.TryWrite("smua.trigger.arm.stimulus		= 0");
        //    this._chassis.TryWrite("smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID");
        //    this._chassis.TryWrite("smua.trigger.measure.stimulus	= trigger.timer[2].EVENT_ID");

        //    this._chassis.TryWrite("smua.trigger.source.action		= smua.ENABLE");

        //    this._chassis.TryWrite(string.Format("smub.trigger.source.linearv({0}, {1}, {2})", 0, 0, pulseCount));
        //    this._chassis.TryWrite(string.Format("smub.trigger.source.limiti		= {0}", 0.2));


        //    this._chassis.TryWrite("smub.trigger.endpulse.action	= smub.SOURCE_IDLE");
        //    this._chassis.TryWrite("smub.trigger.endpulse.stimulus	= trigger.timer[3].EVENT_ID");

        //    this._chassis.TryWrite(string.Format("smub.trigger.count	= {0}", pulseCount));
        //    this._chassis.TryWrite("smub.trigger.source.action		= smub.ENABLE");
        //    this._chassis.TryWrite("smub.trigger.measure.action		= smub.ENABLE");

        //    this._chassis.TryWrite("smub.trigger.endsweep.action	= smub.SOURCE_IDLE");
        //    this._chassis.TryWrite("smub.trigger.measure.iv(smub.nvbuffer1, smub.nvbuffer2)");
        //    this._chassis.TryWrite("smub.trigger.arm.stimulus		= 0");
        //    this._chassis.TryWrite("smub.trigger.source.stimulus	    = trigger.timer[1].EVENT_ID");
        //    this._chassis.TryWrite("smub.trigger.measure.stimulus	= trigger.timer[2].EVENT_ID");


        //    this._chassis.TryWrite("smua.source.output				= smua.OUTPUT_ON");
        //    this._chassis.TryWrite("smub.source.output				= smub.OUTPUT_ON");
        //    this._chassis.TryWrite("smub.trigger.initiate()");
        //    this._chassis.TryWrite("smua.trigger.initiate()");
        //    //Chassis.Write("smua.trigger.source.set()");
        //    #endregion
        //    int estimateSweepTime_ms = Convert.ToInt32(pulseCount * period * 1000) + 1000;
        //    //int estimateSweepTime = Convert.ToInt32(sweepPoints * (0.1 ) * 1000) + 500;
        //    Thread.Sleep(estimateSweepTime_ms);

        //    this._chassis.TryWrite("smua.source.output = smua.OUTPUT_OFF");
        //    this._chassis.TryWrite("smub.source.output = smub.OUTPUT_OFF");
        //    this._chassis.TryWrite("format.data = format.ASCII");
        //    this._chassis.TryWrite("format.asciiprecision = 4"); //Chassis.Write("reset()");

        //    string[] ret = new string[4];
        //    string Source1Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer1.readings)", pulseCount), defaultRespondingTime_ms);
        //    string Source1Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smua.nvbuffer2.readings)", pulseCount), defaultRespondingTime_ms);
        //    string Source2Current = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer1.readings)", pulseCount), defaultRespondingTime_ms);
        //    string Source2Voltage = this._chassis.Query(string.Format("printbuffer(1, {0}, smub.nvbuffer2.readings)", pulseCount), defaultRespondingTime_ms);


        //    var s1c = Source1Current.Split(',');
        //    var s1v = Source1Voltage.Split(',');
        //    var s2c = Source2Current.Split(',');
        //    var s2v = Source2Voltage.Split(',');

        //    ret[0] = Source1Current;
        //    ret[1] = Source1Voltage;
        //    ret[2] = Source2Current;
        //    ret[3] = Source2Voltage;
        //    return ret;
        //}
        #endregion



        public string[] SingleChannelSweepOutputTrigger(
           Keithley2602BChannel channel,
           SourceMeterMode mode,
           double start_val,
          double stop_val,
          int sweepPoints,
          double limit_val,
          double period,
          TriggerLine triggerOutIOChannel,
          double measureDelay_s = 50E-6,
          bool pulsedMode = false,
          double pulseWidth = 0.0005
          )
        {
            if (this.IsOnline == false)
            {
                return new string[0];
            }
            const int cmddelay_ms = 0;
            string ch = channel == Keithley2602BChannel.CHA ? "a" : "b";


            _chassis.TryWrite($"smu{ch}.measure.delay = {measureDelay_s}"); Thread.Sleep(cmddelay_ms);
            #region normal config
            //输出电流/电压模式
            this.SetMode(channel, mode);//0

            //四线制测量
            this.SetSenceMode(channel, SourceMeterSenceMode.SENSE_REMOTE);

            if (mode == SourceMeterMode.SourceCurrentSenceVoltage)
            {
                this.SetAutoRange_I_Enable(channel, SourceMeterFuncitonMode.Source, false);
                this.SetSourceCurrentRange_A(channel, Math.Max(Math.Abs(start_val), Math.Abs(stop_val)));

                this.SetAutoRange_V_Enable(channel, SourceMeterFuncitonMode.Measure, false);
                this.SetMeasureVoltageRange_V(channel, limit_val);
                this.SetComplianceVoltage_V(channel, limit_val);

                _chassis.TryWrite($"smu{ch}.trigger.source.lineari({start_val}, {stop_val}, {sweepPoints})");
                _chassis.TryWrite($"smu{ch}.trigger.source.limitv	= {limit_val}");

            }
            else if (mode == SourceMeterMode.SourceVoltageSenceCurrent)
            {
                this.SetAutoRange_V_Enable(channel, SourceMeterFuncitonMode.Source, false);
                this.SetSourceVoltageRange_V(channel, Math.Max(Math.Abs(start_val), Math.Abs(stop_val)));

                this.SetAutoRange_I_Enable(channel, SourceMeterFuncitonMode.Measure, false);
                this.SetMeasureCurrentRange_A(channel, limit_val);
                this.SetComplianceCurrent_A(channel, limit_val);

                _chassis.TryWrite($"smu{ch}.trigger.source.linearv({start_val}, {stop_val}, {sweepPoints})");
                _chassis.TryWrite($"smu{ch}.trigger.source.limiti	= {limit_val}");
            }

            //设置缓冲区
            this.SetBufferSize(channel, sweepPoints);

            this.SetAutoZero(channel, SourceMeterFuncitonMode.Measure, SourceMeterAutoZero.AUTOZERO_ONCE);

            double nplc = Math.Round((period - measureDelay_s) * 0.5 / 0.02, 6);
            if (pulsedMode)
            {
                nplc = Math.Round((pulseWidth - measureDelay_s) * 0.5 / 0.02, 6);
            }
            this.SetNPLC(channel, nplc); //0.01

            int estimateSweepTime_ms = Convert.ToInt32(sweepPoints * period * 1000) + 3000;  //20240201 +1000 -> + 3000 测试
            this._chassis.Timeout_ms = estimateSweepTime_ms;

            //// A timer will be used to set the measure delay and synchronize the measurement between the two SMUs so set the built in delay to 0.
            //Chassis.Write($"smu{ch}.measure.delay = {measureDelay_s}" + endchar);Thread.Sleep(cmddelay_ms);
            //清理buffer
            _chassis.TryWrite($"smu{ch}.nvbuffer1.clear()"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"smu{ch}.nvbuffer2.clear()"); Thread.Sleep(cmddelay_ms);
            #endregion
            // Disable the pulser.
            //this.Chassis.Write("smua.pulser.enable = smua.DISABLE" + endchar);
            #region trigger config
            _chassis.TryWrite(string.Format("trigger.timer[1].count = {0}", sweepPoints)); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite(string.Format("trigger.timer[1].delay			= {0}", period)); Thread.Sleep(cmddelay_ms);//0.1
            _chassis.TryWrite("trigger.timer[1].passthrough	= true"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"trigger.timer[1].stimulus = smu{ch}.trigger.ARMED_EVENT_ID"); Thread.Sleep(cmddelay_ms);

            //Timer2 controls the measurement
            _chassis.TryWrite(string.Format("trigger.timer[2].count = {0}", 1)); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite(string.Format("trigger.timer[2].delay	= {0}", measureDelay_s)); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite("trigger.timer[2].passthrough	= false"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite("trigger.timer[2].stimulus = trigger.timer[1].EVENT_ID"); Thread.Sleep(cmddelay_ms);

            if (pulsedMode)
            {
                //Timer3 controls the pulse width
                _chassis.TryWrite(string.Format("trigger.timer[3].count = {0}", 1)); Thread.Sleep(cmddelay_ms);
                _chassis.TryWrite(string.Format("trigger.timer[3].delay	= {0}", pulseWidth)); Thread.Sleep(cmddelay_ms);
                _chassis.TryWrite("trigger.timer[3].passthrough	= false"); Thread.Sleep(cmddelay_ms);
                _chassis.TryWrite("trigger.timer[3].stimulus =  trigger.timer[1].EVENT_ID"); Thread.Sleep(cmddelay_ms);
            }

            _chassis.TryWrite($"digio.trigger[{(int)triggerOutIOChannel}].mode = digio.TRIG_RISINGM"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"digio.trigger[{(int)triggerOutIOChannel}].pulsewidth = 0.00001"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"digio.trigger[{(int)triggerOutIOChannel}].stimulus = trigger.timer[2].EVENT_ID"); Thread.Sleep(cmddelay_ms);

            _chassis.TryWrite($"smu{ch}.trigger.measure.action		= smu{ch}.ENABLE"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"smu{ch}.trigger.measure.iv(smu{ch}.nvbuffer1, smu{ch}.nvbuffer2)"); Thread.Sleep(cmddelay_ms);
            if (pulsedMode)
            {
                _chassis.TryWrite($"smu{ch}.trigger.endpulse.stimulus	= trigger.timer[3].EVENT_ID"); Thread.Sleep(cmddelay_ms);
                _chassis.TryWrite($"smu{ch}.trigger.endpulse.action	= smu{ch}.SOURCE_IDLE"); Thread.Sleep(cmddelay_ms);
            }
            _chassis.TryWrite($"smu{ch}.trigger.endsweep.action	= smu{ch}.SOURCE_IDLE"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"smu{ch}.trigger.count = {sweepPoints}"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"smu{ch}.trigger.arm.stimulus = 0"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"smu{ch}.trigger.source.stimulus	= trigger.timer[1].EVENT_ID"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"smu{ch}.trigger.measure.stimulus	= trigger.timer[2].EVENT_ID"); Thread.Sleep(cmddelay_ms);
            Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"smu{ch}.trigger.source.action = smu{ch}.ENABLE"); Thread.Sleep(cmddelay_ms);
            _chassis.TryWrite($"smu{ch}.source.output = smu{ch}.OUTPUT_ON"); Thread.Sleep(cmddelay_ms);
            Thread.Sleep(500);
            _chassis.TryWrite($"smu{ch}.trigger.initiate()");
            #endregion

            //Chassis.Write("waitcomplete()" + endchar);

            Thread.Sleep(estimateSweepTime_ms);

            _chassis.TryWrite($"smu{ch}.source.output = smu{ch}.OUTPUT_OFF");
            _chassis.TryWrite("format.data = format.ASCII");
            _chassis.TryWrite("format.asciiprecision = 6"); //Chassis.Write("reset()" + endchar);

            this._chassis.DefaultBufferSize = 99999999;
            string[] ret = new string[4];
            string Source1Current = _chassis.Query($"printbuffer(1, {sweepPoints}, smu{ch}.nvbuffer1.readings)",defaultRespondingTime_ms);
            string Source1Voltage = _chassis.Query($"printbuffer(1, {sweepPoints}, smu{ch}.nvbuffer2.readings)",defaultRespondingTime_ms);

            var s1c = Source1Current.Split(',');
            var s1v = Source1Voltage.Split(',');

            ret[0] = Source1Current.TrimEnd(new char[] { ',', '\r', '\n' });
            ret[1] = Source1Voltage.TrimEnd(new char[] { ',', '\r', '\n' });

            return ret;

        }















        public override void RefreshDataOnceCycle(CancellationToken token)
        {
           
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
         
        }
    }
}