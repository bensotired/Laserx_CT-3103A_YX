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
    public class TED4015 : InstrumentBase
    {
        public TED4015(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
        }
        private const int Delay_ms = 50;
        private const string ErrStr = "Instrument not connected";
        string endchar = "\r\n";

        public override void Initialize()
        {
            if (this.IsOnline)
            {
                try
                {
                    //var a = IDN;
                    //var idn = this._chassis.Query("OUTP?", Delay_ms);
                    ////var b = GetTemperatureSetpoint();
                    ////SetTemperatureSetpoint(25);

                    //var t = GetTempTemperature();
                    //IsOutPut(true);
                    //var idn2 = this._chassis.Query("OUTP?", Delay_ms);
                    ////SetCurrentLimit(5);



                    //var idn4 = this._chassis.Query("OUTP?", Delay_ms);
                    Reset();
                }
                catch (Exception ex)
                {

                }
                

            }

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
                return ErrStr;
            }
        }

        public int ErrorQuery()
        {
            if (this.IsOnline == false)
            {
                return 0;
            }
            var resut = this._chassis.Query("SYST:ERR?", Delay_ms);
            var resutArray = resut.Split(',');
            return int.Parse(resutArray[0]);
        }
        public void Reset()
        {
            if (this.IsOnline == false)
            {
                return;
            }
            this._chassis.TryWrite("*RST");

        }

        public double CurrentObjectTemperature
        {
            get
            {
                return GetTempTemperature(); //返回温度
            }
        }

        public double GetTempTemperature()
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return 0;
                }

                this._chassis.TryWrite("CONF:TEMP");
                this._chassis.TryWrite("INIT");

                var resut = this._chassis.Query("FETCh?", Delay_ms);
                return double.Parse(resut);
            }
            catch (Exception ex)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataOnceCycle exception:{ex.Message}-{ex.StackTrace}.";
                throw new Exception(msg, ex);
            }
        }
        public void IsOutPut(bool on)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                if (on)
                {
                    this._chassis.TryWrite("OUTP ON");
                }
                else
                {
                    this._chassis.TryWrite("OUTP OFF");
                }
            }
            catch (Exception ex)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataOnceCycle exception:{ex.Message}-{ex.StackTrace}.";
                throw new Exception(msg, ex);
            }
        }

        public void SetCurrentLimit(double value)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOUR:CURR:LIM {value}");


            }
            catch (Exception ex)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataOnceCycle exception:{ex.Message}-{ex.StackTrace}.";
                throw new Exception(msg, ex);
            }
        }
        //SOUR:TEMP:LCON:GAIN 1.0;INT 0.1;DER 0.0;PER 1
        public void SetPID(double P, double I, double D,double oscillation)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOUR:TEMP:LCON:GAIN {P};INT {I};DER {D};PER {oscillation}");


            }
            catch (Exception ex)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataOnceCycle exception:{ex.Message}-{ex.StackTrace}.";
                throw new Exception(msg, ex);
            }
        }
        public void SetRTB(double R,double T,double beta)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SENS:TEMP:THER:EXP:R0 {R};T0 {T};BETA {beta}");


            }
            catch (Exception ex)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataOnceCycle exception:{ex.Message}-{ex.StackTrace}.";
                throw new Exception(msg, ex);
            }
        }

        public void SetTemperatureSetpoint(double temp)
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return;
                }
                this._chassis.TryWrite($"SOUR:TEMP {temp}C");


            }
            catch (Exception ex)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataOnceCycle exception:{ex.Message}-{ex.StackTrace}.";
                throw new Exception(msg, ex);
            }
        }
        public double GetTemperatureSetpoint()
        {
            try
            {
                if (this.IsOnline == false)
                {
                    return 0;
                }
                var idn = this._chassis.Query("SOUR:TEMP?", Delay_ms);
                return double.Parse(idn);

            }
            catch (Exception ex)
            {
                string msg = $"{this.Name} address[{this.Address}] chassis [{this._chassis.Name}] resource [{this._chassis.Resource}]- RefreshDataOnceCycle exception:{ex.Message}-{ex.StackTrace}.";
                throw new Exception(msg, ex);
            }
        }


        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }
    }
}
