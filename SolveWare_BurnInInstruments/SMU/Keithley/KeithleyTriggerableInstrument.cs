using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class KeithleyTriggerableInstrument : InstrumentBase, ISourceMeter_Keithley
    {
        private const int defaultRespondingTime_ms = 100;
        public KeithleyTriggerableInstrument(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {

        }
        
        public TriggerDirection ArmLayerDirection
        {
            get
            {
                string command = ":ARM:DIR?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms).ToUpper();
                if (response.Contains("SOUR"))
                {
                    return TriggerDirection.SOURCE;
                }
                if (response.Contains("ACC"))
                {
                    return TriggerDirection.ACCEPTOR;
                }
                throw new NotSupportedException("Unknow response: " + response);
            }
            set
            {
                string command = ":ARM:DIR " + value.ToString();
                this._chassis.TryWrite(command);
            }
        }

        public ArmLayerOutputs ArmLayerOutput
        {
            get
            {
                string command = ":ARM:OUTP?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms).ToUpper();
                ArmLayerOutputs outputs = ArmLayerOutputs.NONE;
                if (response.Contains("TENT"))
                {
                    outputs = outputs | ArmLayerOutputs.TENTER;
                }
                if (response.Contains("TEX"))
                {
                    outputs = outputs | ArmLayerOutputs.TEXIT;
                }
                if (response.Contains("TRIG"))
                {
                    outputs = outputs | ArmLayerOutputs.TRIGGER;
                }
                return outputs;
            }
            set
            {
                string outputs = "";
                if ((value & ArmLayerOutputs.TENTER) > 0)
                {
                    outputs = "TENT";
                }
                if ((value & ArmLayerOutputs.TEXIT) > 0)
                {
                    if (outputs != "")
                    {
                        outputs += ",";
                    }
                    outputs += "TEX";
                }
                if ((value & ArmLayerOutputs.TRIGGER) > 0)
                {
                    if (outputs != "")
                    {
                        outputs += ",";
                    }
                    outputs += "TRIG";
                }
                if (outputs == "")
                {
                    outputs = "NONE";
                }
                string command = ":ARM:OUTP " + outputs;
                this._chassis.TryWrite(command);
            }
        }

        public TriggerEventSource ArmLayerSource
        {
            get
            {
                TriggerEventSource triggerSource;
                string response;
                //lock (this._chassis)
                {
                    response = this._chassis.Query(":ARM:SOUR?", defaultRespondingTime_ms).Trim();
                }
                switch (response)
                {
                    case "TLIN":
                        triggerSource = TriggerEventSource.Tlink;
                        break;
                    case "IMM":
                        triggerSource = TriggerEventSource.Immediate;
                        break;
                    default:
                        throw new NotSupportedException("Unknown Trigger Source");
                }
                return triggerSource;
            }
            set
            {
                string mode;
                if (value == TriggerEventSource.Immediate) mode = "IMM";
                else mode = "TLIN";
                this._chassis.TryWrite(":ARM:SOUR " + mode);
            }
        }

        public void ClearTrigger()
        {
            string commnad = ":TRIGger:CLEar";
            this._chassis.TryWrite(commnad);
        }


        /// <summary>
        /// Resets(clears) triggerable source meter after trigger sweep, so that it can be used for the next trigger sweep.
        /// </summary>
        /// <param name="sourceMeter"></param>
        public void SimpleResetTriggerableSourceMeter()
        {
            this.AbortTrigger();
            this.ClearTrigger();
        }

        /// <summary>
        /// Aborts trigger process.
        /// </summary>
        public void AbortTrigger()
        {
            string command = "Abort";
            this._chassis.TryWrite(command);
        }


        /// <summary>
        /// Reset triggerable source meter to normal mode (not trigger mode).
        /// </summary>
        /// <param name="sourceMeter"></param>
        public void ResetTriggerableSourceMeter()
        {
            this.AbortTrigger();
            this.ClearTrigger();
            this.ArmLayerDirection = TriggerDirection.ACCEPTOR;
            this.ArmLayerSource = TriggerEventSource.Immediate;
            this.ArmLayerOutput = ArmLayerOutputs.NONE;
            this.ArmCount = 1;

            this.TriggerLayerInput = TriggerLayerInputs.Source;
            this.TriggerLayerDirection = TriggerDirection.SOURCE;
            this.TriggerLayerSource = TriggerEventSource.Immediate;
            this.TriggerLayerOutput = TriggerLayerOutputs.None;

            this.TriggerCount = 1;

        }


        public void Trigger()
        {
            string command = ":INITiate";
            this._chassis.TryWrite(command);
        }

        public int ArmCount
        {
            get
            {
                string command = ":ARM:COUNT?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms);
                return int.Parse(response);
            }
            set
            {
                string command = ":ARM:COUNT " + value.ToString();
                this._chassis.TryWrite(command);
            }
        }
        public int TriggerCount
        {
            get
            {
                string command = ":TRIG:COUNT?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms);
                return int.Parse(response);
            }
            set
            {
                string command = ":TRIG:COUNT " + value.ToString();
                this._chassis.TryWrite(command);
            }
        }

        public TriggerDirection TriggerLayerDirection
        {
            get
            {
                string command = ":TRIG:DIR?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms).ToUpper();
                if (response.Contains("SOUR"))
                {
                    return TriggerDirection.SOURCE;
                }
                if (response.Contains("ACC"))
                {
                    return TriggerDirection.ACCEPTOR;
                }
                throw new NotSupportedException("Unknow response: " + response);
            }
            set
            {
                string command = ":TRIG:DIR " + value.ToString();
                this._chassis.TryWrite(command);
            }
        }

        public TriggerLayerInputs TriggerLayerInput
        {
            get
            {
                string command = ":TRIG:INPut?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms).ToUpper();
                TriggerLayerInputs inputs = TriggerLayerInputs.None;
                if (response.Contains("SOUR"))
                {
                    inputs = inputs | TriggerLayerInputs.Source;
                }
                if (response.Contains("DEL"))
                {
                    inputs = inputs | TriggerLayerInputs.Delay;
                }
                if (response.Contains("SENS"))
                {
                    inputs = inputs | TriggerLayerInputs.Sense;
                }
                return inputs;
            }
            set
            {
                string inputs = string.Empty;
                if ((value & TriggerLayerInputs.Delay) > 0)
                {
                    inputs = "DEL";
                }
                if ((value & TriggerLayerInputs.Sense) > 0)
                {
                    if (inputs != string.Empty)
                    {
                        inputs += ",";
                    }
                    inputs += "SENS";
                }
                if ((value & TriggerLayerInputs.Source) > 0)
                {
                    if (inputs != string.Empty)
                    {
                        inputs += ",";
                    }
                    inputs += "SOUR";
                }
                if (inputs == string.Empty)
                {
                    inputs = "NONE";
                }
                string command = ":TRIG:INPut " + inputs;
                this._chassis.TryWrite(command);
            }
        }
        public TriggerLayerOutputs TriggerLayerOutput
        {
            get
            {
                if (this._chassis.IsOnline == false) return TriggerLayerOutputs.None;

                TriggerLayerOutputs outputs = TriggerLayerOutputs.None;

                string command = ":TRIG:OUTP?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms).ToUpper();
                if (response.Contains("SOUR"))
                {
                    outputs |= TriggerLayerOutputs.Source;
                }
                if (response.Contains("DEL"))
                {
                    outputs |= TriggerLayerOutputs.Delay;
                }
                if (response.Contains("SENS"))
                {
                    outputs |= TriggerLayerOutputs.Sense;
                }
                return outputs;
            }
            set
            {
                string outputs = "";
                if ((value & TriggerLayerOutputs.Delay) > 0)
                {
                    outputs += "DEL";
                }
                if ((value & TriggerLayerOutputs.Sense) > 0)
                {
                    if (outputs != "")
                    {
                        outputs += ",";
                    }
                    outputs += "SENS";
                }
                if ((value & TriggerLayerOutputs.Source) > 0)
                {
                    if (outputs != "")
                    {
                        outputs += ",";
                    }
                    outputs += "SOUR";
                }
                if (outputs == "")
                {
                    outputs = "NONE";
                }
                string command = ":TRIG:OUTP " + outputs;
                this._chassis.TryWrite(command);
            }
        }

        public TriggerEventSource TriggerLayerSource
        {
            get
            {
                TriggerEventSource triggerSource;
                string response;
                //lock (this._chassis)
                {
                    response = this._chassis.Query(":TRIG:SOUR?", defaultRespondingTime_ms).Trim();
                }
                switch (response)
                {
                    case "TLIN":
                        triggerSource = TriggerEventSource.Tlink;
                        break;
                    case "IMM":
                        triggerSource = TriggerEventSource.Immediate;
                        break;
                    default:
                        throw new NotSupportedException("Unknown Trigger Source");
                }
                return triggerSource;
            }
            set
            {
                string mode;
                if (value == TriggerEventSource.Immediate) mode = "IMM";
                else mode = "TLIN";
                this._chassis.TryWrite(":TRIG:SOUR " + mode);
            }
        }  

        /// <summary>
        /// Trigger delay in second
        /// </summary>
        public double TriggerDelay_s
        {
            get
            {
                string command = ":TRIG:DEL?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms);
                return double.Parse(response);
            }
            set
            {
                string command = ":TRIG:DEL " + value.ToString();
                this._chassis.TryWrite(command);
            }
        }


        public int ArmLayerInputLine
        {
            get
            {
                string command = ":ARM:ILINE?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms);
                return int.Parse(response);
            }
            set
            {
                string command = ":ARM:ILINE " + value.ToString();
                this._chassis.TryWrite(command);
            }
        }

        public int ArmLayerOutputLine
        {
            get
            {
                string command = ":ARM:OLINE?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms);
                return int.Parse(response);
            }
            set
            {
                string command = ":ARM:OLINE " + value.ToString();
                this._chassis.TryWrite(command);
            }
        }

        public TriggerLine TriggerLayerInputLine
        {
            get
            {
                string command = ":TRIG:ILINE?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms);
                return (TriggerLine)int.Parse(response);
            }
            set
            {
                string command = ":TRIG:ILINE " + ((int)value).ToString();
                this._chassis.TryWrite(command);
            }
        }

        public TriggerLine TriggerLayerOutputLine
        {
            get
            {
                string command = ":TRIG:OLINE?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms);
                return (TriggerLine)int.Parse(response);
            }
            set
            {
                string command = ":TRIG:OLINE " + ((int)value).ToString();
                this._chassis.TryWrite(command);
            }
        }

        public void WaitForComplete()
        {
            string response = "";
            while (!response.Contains("1"))
            {
                response = this._chassis.Query("*OPC?", defaultRespondingTime_ms);
                System.Threading.Thread.Sleep(10);
            }
        }
        /// <summary>
        /// Data format for source meter
        /// </summary>
        public DataFormat dataFormat
        {
            get
            {
                string command = ":Format:Data?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms).ToUpper();
                if (response.Contains("ASC"))
                {
                    return DataFormat.ASCII;
                }

                return DataFormat.Real;
            }
            set
            {
                string command = ":Format:Data " + value.ToString();
                this._chassis.TryWrite(command);
            }
        }
        /// <summary>
        /// Fetches data from the instrument.
        /// <para>The data returned is UTF8 format.</para>
        /// </summary>
        /// <returns></returns>
        public bool IsReverseByteOrder
        {
            get
            {
                string command = ":Format:Border?";
                string response = this._chassis.Query(command, defaultRespondingTime_ms).ToUpper();
                if (response.Contains("SWAP"))
                {
                    return true;
                }
                return false;
            }
            set
            {
                string command = ":Format:Border ";
                if (value)
                {
                    command += "SWAP";
                }
                else
                {
                    command += "NORM";
                }

                this._chassis.TryWrite(command);
            }
        }
        public double[] FetchRealData()
        {
            this.dataFormat = DataFormat.Real;
            byte[] cmd = Encoding.ASCII.GetBytes(":FETCH?");
            byte[] data = this._chassis.Query(cmd, 60000);

            double[] vals = new double[(data.Length-3)/4];
            for (int i = 0; i < vals.Length; i++)
            {
                byte[] valueBytes = new byte[4];
                Array.Copy(data, 2 + i * 4, valueBytes, 0, 4);
                Array.Reverse(valueBytes);
                vals[i] = BitConverter.ToSingle(valueBytes, 0);
            }

            return vals;
        }
        public double[] FetchAsciiData()
        {
            this.dataFormat = DataFormat.ASCII;
            string value = this._chassis.Query(":FETCH?", 1000);
            string[] valueStringArray = value.Split(',');
            double[] values = new double[valueStringArray.Length];
            int sign = 1;

            for (int i = 0; i < valueStringArray.Length; i++)
            {
                try
                {
                    values[i] = double.Parse(valueStringArray[i]) * sign;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Unable to convert string {0} to double.", valueStringArray[i]), ex);
                }
            }
            return values;
        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public virtual void Reset()
        {
            throw new NotImplementedException();
        }
        public virtual void ZeroCorrection()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsCurrentSenseAutoRangeOn { set; get; }
        public virtual double CurrentSenseRange_A { get; set; }
       
        public virtual void SetupCurrentStairSweep(bool isMaster,
  double complianceVoltage_V, double measurementIntergrationPeriod,
  TriggerLine triggerInputLine, TriggerLine triggerOutputLine, int triggerCount, 
  bool remoteSense, SelectTerminal terminal, double start, double stop, double step)
        {
            throw new NotImplementedException();
        }
        public virtual void  SetupTrigger(bool isMaster, TriggerLine triggerInputLine, TriggerLine triggerOutputLine, int triggerCount, double nplc = 1.0)
        {
            throw new NotImplementedException();
        }

        public virtual double ReadVoltage_V()
        {
            throw new NotImplementedException();
        }

        public virtual double ReadCurrent_A()
        {
            throw new NotImplementedException();
        }

        public virtual double ReadCurrent6485_A()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsOutputOn { get; set; }
        public virtual bool IsCurrentSourceAutoRangeOn { get; set; }
        public virtual bool IsVoltageSenseAutoRangeOn { get; set; }
        public virtual bool IsVoltageSourceAutoRangeOn { get; set; }
        public virtual double CurrentCompliance_A { get; set; }
        public virtual double CurrentSetpoint_A { get; set; }
        public virtual SenseModeTypes SenseMode { get; set; }
        public virtual SourceModeTypes SourceMode { get; set; }
        public virtual SelectTerminal Terminal { get; set; }
        public virtual bool IsFourWireOn { get; set; }
        public virtual double VoltageCompliance_V { get; set; }
    }
}
