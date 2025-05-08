using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NationalInstruments.VisaNS;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SolveWare_BurnInInstruments
{
    public class KeithleyDMM6500 : InstrumentBase
    {
        public KeithleyDMM6500(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
        }
        private const int Delay_ms = 50;
        private const string ErrStr = "Instrument not connected";
        string endchar = "\r\n";
        public string IDN
        {
            get
            {
                if (this.IsOnline)
                {
                    var idn = this._chassis.Query("*IDN?", Delay_ms);
                    return idn;
                }
                return "Offline DMM6500";
            }
        }

        public void Reset()
        {
            if (this.IsOnline == false)
            {
                return;
            }
            this._chassis.TryWrite("reset()");

            ////复位触发线
            //TriggerLine triggerOutIOChannel = TriggerLine.Line1;
            ////复位触发线
            //triggerIOChannel(triggerOutIOChannel);
        }

        public void SetDMMFunction(DMMFunction func = DMMFunction.FUNC_DC_CURRENT)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                string cmd = $"dmm.measure.func = dmm.{func}";
                this._chassis.TryWrite(cmd);
            }
            catch (Exception ex)
            {

            }
        }

        public bool SenseAutoRangeEnable
        {
            set
            {
                try
                {
                    if (this.IsOnline == false)
                    {
                        return;
                    }
                    if (value)
                        this._chassis.TryWrite("dmm.measure.autorange = dmm.ON");
                    else
                        this._chassis.TryWrite("dmm.measure.autorange = dmm.OFF");
                }
                catch (Exception ex)
                {

                }

            }
        }

        public void SetSenseRange(double range)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"dmm.measure.sense.range = {range}");
            }
            catch (Exception ex)
            {

            }
        }
        
        public double GetSenseRange()
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return 0.0;
                }
                string val = this._chassis.Query("print(dmm.measure.sense.range)", Delay_ms);
                return Convert.ToDouble(val);
            }
            catch (Exception ex)
            {
                return double.NaN;
            }

        }

        public double Read()
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return 0.0;
                }
                string resp = this._chassis.Query($"print(dmm.measure.read())", Delay_ms);
                return Convert.ToDouble(resp);
            }
            catch (Exception ex)
            {
                return double.NaN;
            }
        }

        public void SetExtInTrigger()
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite("trigger.ext.reset()\r\n");
                this._chassis.TryWrite("trigger.extin.clear()\r\n");
                this._chassis.TryWrite("trigger.extin.edge = trigger.EDGE_RISING\r\n");
            }
            catch (Exception ex)
            {
            }

        }

        public void SetTriggeredMeasureCurrent(double range_A, int steps, double nplc, TriggerLine triggerLine = TriggerLine.Line0)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                string triggerEvent = string.Empty;
                if (triggerLine == 0)
                {
                    this._chassis.TryWrite("trigger.extin.clear()" + endchar);
                    triggerEvent = "EVENT_EXTERNAL";
                }
                else
                {
                    //this.Chassis.Write($"trigger.digin[{triggerLine}].clear()");
                    this._chassis.TryWrite($"digio.line[{(int)triggerLine}].mode =  digio.MODE_TRIGGER_IN" + endchar);
                    this._chassis.TryWrite($"trigger.digin[{(int)triggerLine}].edge = trigger.EDGE_RISING" + endchar);
                    triggerEvent = $"EVENT_DIGIO{(int)triggerLine}";
                }

                this._chassis.TryWrite($"dmm.measure.func = dmm.FUNC_DC_CURRENT" + endchar);
                this._chassis.TryWrite($"dmm.measure.range = {range_A}" + endchar);
                this._chassis.TryWrite($"dmm.measure.nplc = {nplc}" + endchar);
                this._chassis.TryWrite($"trigger.model.load(\"Empty\")" + endchar);

                this._chassis.TryWrite($"trigger.model.setblock(1, trigger.BLOCK_BUFFER_CLEAR, defbuffer1)" + endchar);
                this._chassis.TryWrite($"trigger.model.setblock(2, trigger.BLOCK_WAIT, trigger.{triggerEvent})" + endchar);
                this._chassis.TryWrite($"trigger.model.setblock(3, trigger.BLOCK_MEASURE_DIGITIZE, defbuffer1)" + endchar);
                this._chassis.TryWrite($"trigger.model.setblock(4, trigger.BLOCK_BRANCH_COUNTER, {steps}, 2)" + endchar);

            }
            catch (Exception ex)
            {
            }
        }

        public double SetTriggeredDigitizeCurrent(double range_A, int steps, int samplesPerStep, TriggerLine triggerLine = 0, int sampleRate = 1000000)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return range_A;
                }

                string triggerEvent = string.Empty;
                const int delay_ms = 5;
                if (triggerLine == 0)
                {
                    this._chassis.TryWrite("trigger.extin.clear()" + endchar); Thread.Sleep(delay_ms);
                    triggerEvent = "EVENT_EXTERNAL";
                }
                else
                {
                    //this.Chassis.Write($"trigger.digin[{triggerLine}].clear()");
                    this._chassis.TryWrite($"digio.line[{(int)triggerLine}].mode =  digio.MODE_TRIGGER_IN" + endchar); Thread.Sleep(delay_ms);
                    this._chassis.TryWrite($"trigger.digin[{(int)triggerLine}].edge = trigger.EDGE_RISING" + endchar); Thread.Sleep(delay_ms);
                    triggerEvent = $"EVENT_DIGIO{(int)triggerLine}"; Thread.Sleep(delay_ms);
                }

                this._chassis.TryWrite($"dmm.digitize.func = dmm.FUNC_DIGITIZE_CURRENT" + endchar); Thread.Sleep(delay_ms);
                this._chassis.TryWrite($"dmm.digitize.range = {range_A}" + endchar); Thread.Sleep(delay_ms);
                this._chassis.TryWrite($"dmm.digitize.samplerate = {sampleRate}" + endchar); Thread.Sleep(delay_ms);
                this._chassis.TryWrite($"trigger.model.load(\"Empty\")" + endchar); Thread.Sleep(delay_ms);
                this._chassis.TryWrite($"trigger.model.setblock(1, trigger.BLOCK_BUFFER_CLEAR, defbuffer1)" + endchar); Thread.Sleep(delay_ms);
                this._chassis.TryWrite($"trigger.model.setblock(2, trigger.BLOCK_WAIT, trigger.{triggerEvent})" + endchar); Thread.Sleep(delay_ms);
                this._chassis.TryWrite($"trigger.model.setblock(3, trigger.BLOCK_MEASURE_DIGITIZE, defbuffer1, {samplesPerStep})" + endchar); Thread.Sleep(delay_ms);
                this._chassis.TryWrite($"trigger.model.setblock(4, trigger.BLOCK_BRANCH_COUNTER, {steps}, 2)" + endchar); Thread.Sleep(delay_ms);

                //回读实际的Range
                string strRealRange_A = this._chassis.Query($"print(dmm.digitize.range)" + endchar, Delay_ms);
                double dRealRange_A = double.Parse(strRealRange_A);

                return dRealRange_A;
            }
            catch (Exception ex)
            {
                return double.NaN;
            }

        }

        public void EnableScreen(bool enable)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                if (enable)
                {
                    this._chassis.TryWrite("display.changescreen(display.SCREEN_HOME)" + endchar);
                }
                else
                {
                    this._chassis.TryWrite("display.changescreen(display.SCREEN_PROCESSING)" + endchar);
                }
            }
            catch (Exception ex)
            {
                return;
            }

        }

        public void InitiateTriggerModel()
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite("trigger.model.initiate()" + endchar);
                System.Threading.Thread.Sleep(100);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                string state = this._chassis.Query("print(trigger.model.state())" + endchar, Delay_ms);
                while (state.Contains("WAITING") == false && sw.ElapsedMilliseconds < 5000)
                {
                    state = this._chassis.Query("print(trigger.model.state())" + endchar, Delay_ms);
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                return;
            }
            

        }

        public void AbortTriggerModel()
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                string state = this._chassis.Query("print(trigger.model.state())" + endchar, Delay_ms);
                if (state.Contains("WAITING"))
                {
                    this._chassis.TryWrite("trigger.model.abort()" + endchar);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                return;
            }
            
        }

        public void SetBufferSize(int val)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }

                string cmd = $"defbuffer1.capacity={val}";
                this._chassis.TryWrite(cmd + endchar);
                cmd = $"defbuffer1.clear()";
                this._chassis.TryWrite(cmd + endchar);

                string n = this._chassis.Query("print(defbuffer1.capacity)" + endchar, Delay_ms);
                string m = this._chassis.Query("print(defbuffer1.n)" + endchar, Delay_ms);
            }
            catch (Exception ex)
            {
                return;
            }
            
        }

        public double[] ReadData(int dataCount, int timeout_ms = 5000)
        {
            if (this.IsOnline == false)
            {
                return new double[0];
            }
            double[] data = new double[dataCount];

            try
            {
                string n = this._chassis.Query("print(defbuffer1.n)" + endchar, Delay_ms);

                Stopwatch sw = new Stopwatch();

                sw.Start();
                int existingData = Convert.ToInt32(n);
                while (existingData < dataCount && sw.ElapsedMilliseconds < timeout_ms)
                {
                    existingData = Convert.ToInt32(this._chassis.Query("print(defbuffer1.n)" + endchar, Delay_ms));
                    Thread.Sleep(100);
                }
                if (sw.ElapsedMilliseconds >= timeout_ms)
                    return data;

                this._chassis.DefaultBufferSize = 99999999;
                string cmd = "printbuffer(1,defbuffer1.n,defbuffer1)" + endchar;
                byte[] bcmd = Encoding.ASCII.GetBytes(cmd);

                _chassis.TryWrite("format.data = format.REAL32" + endchar);

                var dataBytes = this._chassis.Query(bcmd, 4 * dataCount + 3);
                //string strdata = this.Chassis.Query(cmd);//, 4 * dataCount+2);

                byte[] singleDataBytes = new byte[4];
                List<double> dataList = new List<double>();
                for (int i = 0; i < dataBytes.Length - 3; i += 4)
                {
                    Array.Copy(dataBytes, i + 2, singleDataBytes, 0, 4);
                    var tempVal = BitConverter.ToSingle(singleDataBytes, 0);
                    dataList.Add(tempVal);
                }
                data = dataList.ToArray();

                //var tempData = resp.Split(',');
                //for (int i = 0; i < data.Length; i++)
                //{
                //    data[i] = Convert.ToDouble(tempData[i]);
                //}
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _chassis.TryWrite("format.data = format.ASCII" + endchar);
            }


            return data;
        }



































        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
        }
    }
    public enum DMMFunction
    {
        FUNC_DC_VOLTAGE,
        FUNC_RESISTANCE,
        FUNC_AC_VOLTAGE,
        FUNC_ACV_FREQUENCY,
        FUNC_4W_RESISTANCE,
        FUNC_ACV_PERIOD,
        FUNC_DC_CURRENT,
        FUNC_DIODE,
        FUNC_DCV_RATIO,
        FUNC_AC_CURRENT,
        FUNC_CAPACITANCE,
        FUNC_TEMPERATURE,
        FUNC_CONTINUITY,

    }
}
