using MG17NanoTrakLib;
using SolveWare_BurnInInstruments;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments
{
    public  class Thorlabs_TLX1 : InstrumentBase, IInstrumentBase
    {
        const int Delay_ms = 20;
        Thorlabs_TLX1Chassis MyChassis
        {
            get
            {
                return this._chassis as Thorlabs_TLX1Chassis;
            }
        }

        public Thorlabs_TLX1(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis) 
        {

        }        
       
        public void Reset() 
        {
            if (!this._chassis.IsOnline) return;
            if (this.SetLaserPower_SW)
            {
                this.SetLaserPower_SW = false;
            }
            if (this.SetVOAPower_SW)
            {
                this.SetVOAPower_SW = false;
            }
        }
        public void SetVOAOutModePower_mW(float value)
        {
            if (!this._chassis.IsOnline) return;
           // this.SetLaserPower_SW = false;
            this.SetVOAPower_SW = true;
            this.SetVOAMode_SW = true;
            this.SetOpticalOutputPower_mW = value;
        }
        public void SsetVOAOutModePower_dBm(float value) 
        {
            if (!this._chassis.IsOnline) return;
            //this.SetLaserPower_SW = false;
            this.SetVOAPower_SW = true;
            this.SetVOAMode_SW = true;
            this.SetOpticalOutputPowerValue_dBm = value;
        }
        public void SsetVOAAttenModePower_dBm(float value) 
        {
            if (!this._chassis.IsOnline) return;
            //this.SetLaserPower_SW = false;
            this.SetVOAPower_SW = true;
            this.SetVOAMode_SW = false;
            this.SetOpticalAttenuationValue = value;
        }
        public void SetLaserDither(int Channel,int offset)
        {
            SetDither_SW = true;
            SetGetITUChannel = Channel;
            SetGetFrequency = offset;
        }
        public bool SetLaserPower_SW
        {
            set
            {
                if (!this._chassis.IsOnline) return;
                string command = "";
                if (value) command = $"LAS:POWER 1";
                else command = $"LAS:POWER 0";
                this._chassis.Query(command, Delay_ms);
                //Thorlabs_TLX1Chassis.SendCommand(command);
            }
            get
            {
                if (!this._chassis.IsOnline) return false;
                string command = $"LASER:POWER?";
                //string ret=Thorlabs_TLX1Chassis.SendCommand(command);
                string ret = this._chassis.Query(command, Delay_ms);
                return Convert.ToInt32(ret) == 1 ? true : false;
            }
        }
        public bool SetDither_SW
        {
            set
            {
                if (!this._chassis.IsOnline) return;
                string command = "";
                if (value) command = $"LAS:D: 1";
                else command = $"LAS:D: 0";
                this._chassis.Query(command, Delay_ms);
                //Thorlabs_TLX1Chassis.SendCommand(command);
            }
            get
            {
                if (!this._chassis.IsOnline) return false;
                string command = $"LAS:D?";
                //string ret=Thorlabs_TLX1Chassis.SendCommand(command);
                string ret = this._chassis.Query(command, Delay_ms);
                return Convert.ToInt32(ret) == 1 ? true : false;
            }
        }
        public int SetGetITUChannel 
        {
            set
            {
                if (value < 1 || value > 96) return;
                if (!this._chassis.IsOnline) return;
                string command = $"LAS:CHAN: {value}";
                this._chassis.Query(command, Delay_ms);
            }
            get
            {
                if (!this._chassis.IsOnline) return -1;
                string command = $"LAS:CHAN?";
                string ret = this._chassis.Query(command, Delay_ms);
                return Convert.ToInt32(ret);
            }
        }
        //public int SetGetDither_offset 
        //{
        //    set
        //    {
        //        //if (value < 1 || value > 96) return;
        //        if (!this._chassis.IsOnline) return;
        //        string command = $"LAS:FINE {value}";
        //        this._chassis.Query(command, Delay_ms);
        //    }
        //    get
        //    {
        //        if (!this._chassis.IsOnline) return -1;
        //        string command = $"LAS:FINE?";
        //        string ret = this._chassis.Query(command, Delay_ms);
        //        return Convert.ToInt32(ret);
        //    }
        //}
        public int SetGetFrequency 
        {
            set
            {
               // if (value < 1 || value > 96) return;
                if (!this._chassis.IsOnline) return;
                string command = $"LAS:FINE: {value}";
                this._chassis.Query(command, Delay_ms);
            }
            get
            {
                if (!this._chassis.IsOnline) return -1;
                string command = $"LAS:FINE?";
                string ret = this._chassis.Query(command, Delay_ms);
                return Convert.ToInt32(ret);
            }
        }
        public string SetGetBand
        {
            set
            {
                // if (value < 1 || value > 96) return;
                if (!this._chassis.IsOnline) return;
                if (value != "CBand" && value != "LBand" && value != "1310") return;
                string command = $"LAS:FINE: {value}";
                this._chassis.Query(command, Delay_ms);
            }
            get
            {
                if (!this._chassis.IsOnline) return string.Empty;
                string command = $"LAS:SEL?";
                string ret = this._chassis.Query(command, Delay_ms);
                return ret;
            }
        }
        public bool SetVOAPower_SW
        {
            set
            {
                if (!this._chassis.IsOnline) return;
                string command = "";
                if (value) command = $"VOA:POWER: 1";
                else command = $"VOA:POWER: 0";
                this._chassis.Query(command, Delay_ms);
            }
            get
            {
                if (!this._chassis.IsOnline) return false;
                string command = $"VOA:POWER?";
                string ret = this._chassis.Query(command, Delay_ms);
                return Convert.ToInt32(ret) == 1 ? true : false;
            }
        }
        public bool SetVOAMode_SW
        {
            set
            {
                if (!this._chassis.IsOnline) return;
                string command = "";
                if (value) command = $"VOA:MODE: 1"; //true =out
                else command = $"VOA:MODE: 0";   //false =Atten
                this._chassis.Query(command, Delay_ms);
            }
            get
            {
                if (!this._chassis.IsOnline) return false;
                string command = $"VOA:MODE?";
                string ret = this._chassis.Query(command, Delay_ms);
                return Convert.ToInt32(ret) == 1 ? true : false;  //1=out  0=Atten
            }
        }

        public int SetGetSystemWavelength
        {
            set
            {
                if (!this._chassis.IsOnline) return;
                string command = $"SYS:WAVE: {value}";
                this._chassis.Query(command, Delay_ms);
            }
            get
            {
                if (!this._chassis.IsOnline) return -1;
                string command = $"SYS:WAVE?";
                string ret = this._chassis.Query(command, Delay_ms);
                return Convert.ToInt32(ret);
            }
        }
        public float SetOpticalAttenuationValue 
        {
            set
            {
                if (!this._chassis.IsOnline) return;
                string command = $"VOA:ATTEN: {value}";
                this._chassis.Query(command, Delay_ms);
            }
            get
            {
                if (!this._chassis.IsOnline) return -1;
                string command = $"VOA:ATTEN?";
                string ret = this._chassis.Query(command, Delay_ms);
                //float floatValue = 0.0F;
               // bool _success = float.TryParse(ret, out floatValue);
                //floatValue = Convert.ToSingle( ret);
                return Convert.ToSingle(ret);
                //return Convert.ToInt32(ret);
            }
        }
        public float SetOpticalOutputPowerValue_dBm
        {
            set
            {
                if (!this._chassis.IsOnline) return;
                string command = $"VOA:OUTPUT:DBM: {value}";
                this._chassis.Query(command, Delay_ms);
            }
            get
            {
                if (!this._chassis.IsOnline) return -1;
                string command = $"VOA:OUTPUT:DBM?";
                string ret = this._chassis.Query(command, Delay_ms);
                return Convert.ToSingle(ret);
            }
        }
        public float SetOpticalOutputPower_mW
        {
            set
            {
                if (!this._chassis.IsOnline) return;
                string command = $"VOA:OUTPUT:MW: {value}";
                this._chassis.Query(command, Delay_ms);
            }
            get
            {
                if (!this._chassis.IsOnline) return -1;
                string command = $"VOA:OUTPUT:MW?";
                string ret = this._chassis.Query(command, Delay_ms); ;
                return Convert.ToSingle(ret);
            }
        }
        public override void RefreshDataOnceCycle(CancellationToken token)
        {

        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {

        }
    }
}