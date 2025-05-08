using System;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class Keithley_6485 : KeithleyTriggerableInstrument,ISourceMeter_Keithley
    {
        private const double maxPowerLineCycles = 10;
        private const double minPowerLineCycles = 0.01;
        private const int defaultRespondingTime_ms = 10;
        public Keithley_6485(string name, string address, IInstrumentChassis chassis)
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
    
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }
        private string _instrumentIdn;
        /// <summary>
        /// Gets IDN of the instrument.
        /// </summary>
        public string InstrumentIDN
        {
            get
            {
                if (string.IsNullOrEmpty(this._instrumentIdn))
                {
                    if (!this.IsOnline)
                    {
                        this._instrumentIdn = "";
                    }
                    else
                    {
                        this._instrumentIdn = this._chassis.Query("*IDN?", defaultRespondingTime_ms);
                    }
                }
                return this._instrumentIdn;
            }
        }
        public ControlAutoZero AutoZero
        {
            get
            {
                //if (this._chassis.IsOnline == false) return ControlAutoZero.Off;
                ControlAutoZero autoZero;
                int response;
                //lock (this._chassis)
                {
                    response = Convert.ToInt16(this._chassis.Query(":SYST:AZER:STAT?", defaultRespondingTime_ms));
                }
                switch (response)
                {
                    case 1:
                        autoZero = ControlAutoZero.On;
                        break;
                    case 0:
                        autoZero = ControlAutoZero.Off;
                        break;
                    default:
                        throw new NotSupportedException("Unknown Mode");
                }
                return autoZero;
            }
            set
            {
                string state = null;
                switch (value)
                {
                    case ControlAutoZero.Off:
                        state = "OFF";
                        break;
                    case ControlAutoZero.On:
                        state = "ON";
                        break;
                    case ControlAutoZero.Once:
                        state = "ONCE";
                        break;
                    default:
                        throw new NotSupportedException("Unknown AutoZero Option");
                }
                this._chassis.TryWrite(":SYST:AZER:STAT " + state);
            }
        }
        public override double ReadCurrent6485_A()
        {
            string resp = this._chassis.Query("READ?", Timeout_ms);
            double val = double.Parse(resp.Split(',')[0].TrimEnd('A'));
            return val;
        }

        public override void Initialize()
        {
            this._chassis.TryWrite("INIT");
            //this._chassis.TryWrite("CONF:CURR:DC");
            //this._chassis.TryWrite("ARM:COUNT 1");
        }
        public override void Reset()
        {
            this._chassis.TryWrite("*RST");
        }
        /// <summary>
        /// The following command sequence will perform one zero corrected amps measurement, need to be run when the current is determined as zero.        
        /// </summary>
        public override void ZeroCorrection()
        {
            this.Reset();
            this._chassis.TryWrite("SYST:ZCH ON");//Enable zero check.
            this._chassis.TryWrite("CURR:RANG 2e-9");//Select the 2nA range
            this._chassis.TryWrite("INIT");//Trigger reading to be used as zero correction
            this._chassis.TryWrite("SYST:ZCOR:ACQ");//Use last reading taken as zero correct value.
            this._chassis.TryWrite("SYST:ZCOR ON");//Perform zero correction.
            this._chassis.TryWrite("CURR:RANG:AUTO ON");//Enable auto range.
            this._chassis.TryWrite("SYST:ZCH OFF");//Disable zero check
        }

        public override bool IsCurrentSenseAutoRangeOn
        {
            get
            {
                if (Convert.ToBoolean(Convert.ToByte(this._chassis.Query(":SENS:CURR:RANG:AUTO?", defaultRespondingTime_ms)))) return true;
                else return false;
            }
            set
            {
                if (value) this._chassis.TryWrite(":SENS:CURR:RANG:AUTO ON");
                else this._chassis.TryWrite(":SENS:CURR:RANG:AUTO OFF");
            }
        }
        public override double CurrentSenseRange_A
        {
            get
            {
                double response;
                //lock (this._chassis)
                {
                    response = double.Parse(this._chassis.Query(":CURR:RANG?", defaultRespondingTime_ms));
                }
                return response;
            }
            set
            {
                this._chassis.TryWrite(":CURR:RANG " + value.ToString());
            }
        }
        public double IMeasurementIntegrationPeriod
        {
            get
            {
                double iIntegrationPeriod = 0;
                //lock (this._chassis)
                {
                    iIntegrationPeriod = Convert.ToDouble(this._chassis.Query(":CURR:NPLC?", defaultRespondingTime_ms));
                }
                return iIntegrationPeriod;
            }
            set
            {
                if (value > minPowerLineCycles)
                {
                    if (value > maxPowerLineCycles) value = maxPowerLineCycles;
                }
                else
                {
                    value = minPowerLineCycles;
                }
                this._chassis.TryWrite(":CURR:NPLC " + value.ToString());
            }
        } 
        //public DataFormat dataFormat
        //{
        //    get
        //    {
        //        string command = ":Format:Data?";
        //        string response = this._chassis.Query(command, defaultRespondingTime_ms).ToUpper();
        //        if (response.Contains("ASC"))
        //        {
        //            return DataFormat.ASCII;
        //        }

        //        return DataFormat.Real;
        //    }
        //    set
        //    {
        //        string command = ":Format:Data " + value.ToString();
        //        this._chassis.TryWrite(command);
        //    }
        //}
        //public bool IsReverseByteOrder
        //{
        //    get
        //    {
        //        string command = ":Format:Border?";
        //        string response = this._chassis.Query(command, defaultRespondingTime_ms).ToUpper();
        //        if (response.Contains("SWAP"))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    set
        //    {
        //        string command = ":Format:Border ";
        //        if (value)
        //        {
        //            command += "SWAP";
        //        }
        //        else
        //        {
        //            command += "NORM";
        //        }

        //        this._chassis.TryWrite(command);
        //    }
        //}
        public void SetDataElements()
        {
            string command = ":Form:Elem READ";
            this._chassis.TryWrite(command);
        }
        public void SetBufferSize(int size)
        {
            string command = "TRAC:POIN "+ size.ToString();
            this._chassis.TryWrite(command);
            command = "TRAC:FEED SENS";
            this._chassis.TryWrite(command);
            command = "TRAC:FEED:CONT NEXT";
            this._chassis.TryWrite(command);
        }
        public override void SetupTrigger(bool isMaster, TriggerLine triggerInputLine, TriggerLine triggerOutputLine, int triggerCount, double nplc = 1.0)
        {
            this._chassis.TryWrite("*CLS");
            this._chassis.TryWrite(":TRACE:CLE");
            this.AbortTrigger();
            this.ClearTrigger();

            this.IMeasurementIntegrationPeriod = nplc;
            this.SetDataElements();
            //this.dataFormat = DataFormat.ASCII;
            //this.dataFormat = DataFormat.Real;
            //this.IsReverseByteOrder = true;

            this.ArmLayerDirection = TriggerDirection.SOURCE;
            this.ArmLayerSource = TriggerEventSource.Immediate;
            if (triggerInputLine > 0)
            {
                this.TriggerLayerSource = TriggerEventSource.Tlink;
            }
            else
            {
                this.TriggerLayerSource = TriggerEventSource.Immediate;
            }
            if (isMaster)
            {
                //this.TriggerLayerOutputs = TriggerLayerOutputs.Source;
                //this.TriggerLayerInput = TriggerLayerInputs.Source;
                this.TriggerLayerDirection = TriggerDirection.SOURCE;
            }
            else
            {
                //this.TriggerLayerOutputs = TriggerLayerOutputs.Sense;
                //this.TriggerLayerInput = TriggerLayerInputs.Sense;
                this.TriggerLayerDirection = TriggerDirection.ACCEPTOR;
            }
            if (triggerOutputLine > 0)
            {
                this.TriggerLayerOutput = TriggerLayerOutputs.Sense;
            }
            else
            {
                this.TriggerLayerOutput = TriggerLayerOutputs.None;
            }

            if (triggerInputLine > 0)
            {
                this.TriggerLayerInputLine = triggerInputLine;
            }
            this.TriggerLayerOutputLine = triggerOutputLine;
            this.ArmCount = 1;
            this.TriggerCount = triggerCount;
            this.SetBufferSize(triggerCount);
            //this.AutoZero = ControlAutoZero.Once;
        }
    }
}
