using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    /// <summary>
    /// Keithley 24xx driver.
    /// </summary>
    public class Keithley_24xx : KeithleyTriggerableInstrument, ISourceMeter_Keithley
    {
        private const double maxPowerLineCycles = 10;
        private const double minPowerLineCycles = 0.01;
        private const int defaultRespondingTime_ms = 100;

        /// <summary>
        /// Initializes an instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="slot"></param>
        /// <param name="subSlot"></param>
        /// <param name="chassis"></param>
        public Keithley_24xx(string name, string address, IInstrumentChassis chassis)
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
     

        #region ISourceMeter

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

        public override void Reset()
        {
            this._chassis.TryWrite("*RST");
        }

        /// <summary>
        /// This command is used to enable or disable auto zerozero, or to force an
        ///immediate one-time auto zero update if auto zero is disabled. When auto
        ///zero is enabled, accuracy is optimized. When auto zero is disabled,
        ///speed is increased at the expense of accuracy.
        /// </summary>
        /// 


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



        /// <summary>
        /// Sets current range Eg. Default, down, up, max, min.
        /// </summary>
        ///
        public CurrentRange CurrentRangeEnumeration
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                string range;
                switch (value)
                {
                    case CurrentRange.Default:
                        range = "DEF";
                        break;
                    case CurrentRange.Down:
                        range = "DOWN";
                        break;
                    case CurrentRange.Maximum:
                        range = "MAX";
                        break;
                    case CurrentRange.Minimum:
                        range = "MIN";
                        break;
                    case CurrentRange.Up:
                        range = "UP";
                        break;
                    default:
                        throw new NotSupportedException("Unknown Current Range Setting");
                }
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:CURR:RANG " + range));
            }
        }

        /// <summary>
        /// Gets or sets display resolution.
        /// </summary>
        public SetDisplayDigit DisplayDigits
        {
            get
            {
                SetDisplayDigit displayDigits;
                string response;
                //lock (this._chassis)
                {
                    ;
                    response = this._chassis.Query(":DISP:DIG?", defaultRespondingTime_ms).Trim();
                }
                switch (response)
                {
                    case "4":
                        displayDigits = SetDisplayDigit.Four;
                        break;
                    case "5":
                        displayDigits = SetDisplayDigit.Five;
                        break;
                    case "6":
                        displayDigits = SetDisplayDigit.Six;
                        break;
                    case "7":
                        displayDigits = SetDisplayDigit.Seven;
                        break;
                    default:
                        throw new NotSupportedException("Unknown Number of Display Digits");
                }
                return displayDigits;
            }
            set
            {
                string digits = null;
                switch (value)
                {
                    case SetDisplayDigit.Four:
                        digits = "4";
                        break;
                    case SetDisplayDigit.Five:
                        digits = "5";
                        break;
                    case SetDisplayDigit.Six:
                        digits = "6";
                        break;
                    case SetDisplayDigit.Seven:
                        digits = "7";
                        break;
                    default:
                        throw new NotSupportedException("Unknow Number of Digits");
                }
                this._chassis.TryWrite(":DISP:DIG " + digits);
            }
        }
        CurrentSourceMode iSourceMode;
        /// <summary>
        /// Gets or sets DC sourcing mode for current source.
        /// </summary>
        public CurrentSourceMode ISourceMode
        {
            get
            {
               
                string response;
                //lock (this._chassis)
                {
                    response = this._chassis.Query(BuildCommand(":SOUR{0}:CURR:MODE?"), defaultRespondingTime_ms).Trim();
                }
                switch (response)
                {
                    case "FIX":
                        iSourceMode = CurrentSourceMode.Fixed;
                        break;
                    case "LIST":
                        iSourceMode = CurrentSourceMode.List;
                        break;
                    case "SWE":
                        iSourceMode = CurrentSourceMode.Sweep;
                        break;
                    default:
                        throw new NotSupportedException("Unknown Mode");
                }
                return iSourceMode;
            }
            set
            {
                string state = null;
                switch (value)
                {
                    case CurrentSourceMode.Fixed:
                        state = "FIX";
                        break;
                    case CurrentSourceMode.List:
                        state = "LIST";
                        break;
                    case CurrentSourceMode.Sweep:
                        state = "SWE";
                        break;
                    default:
                        throw new NotSupportedException("Unknown Mode");
                }
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:CURR:MODE " + state));
            }
        }

        SenseModeTypes _offlineSenseMode;
        /// <summary>
        /// Gets or sets sense mode.
        /// </summary>
        public override SenseModeTypes SenseMode
        {
            set
            {
                _offlineSenseMode = value;
                string mode = null;
                if (value == SenseModeTypes.AllOn || value == SenseModeTypes.AllOff)
                {
                    if (value == SenseModeTypes.AllOn)
                    {
                        this._chassis.TryWrite(BuildCommand(":SENS{0}:FUNC:ON:ALL"));
                    }
                    if (value == SenseModeTypes.AllOff)
                    {
                        this._chassis.TryWrite(BuildCommand(":SENS{0}:FUNC:OFF:ALL"));
                    }
                }
                else
                {
                    mode = "";
                    if ((value & SenseModeTypes.Voltage) > 0) mode = "'VOLT'";

                    if ((value & SenseModeTypes.Current) > 0)
                    {
                        if (mode != "")
                        {
                            mode += ",";
                        }
                        mode += "'CURR'";
                    }
                    if ((value & SenseModeTypes.Resistance) > 0)
                    {
                        if (mode != "")
                        {
                            mode += ",";
                        }
                        mode += "'RES'";
                    }
                    this._chassis.TryWrite(BuildCommand(":SENS{0}:FUNC:ON " + mode));
                }
            }

            get
            {
                //if (this._chassis.IsOnline == false) return _offlineSenseMode;
                string command = BuildCommand(":SENS{0}:FUNC:STAT? ");
                int currentEnable = int.Parse(this._chassis.Query(command + "\"CURR\"", defaultRespondingTime_ms));
                int voltageEnable = int.Parse(this._chassis.Query(command + "\"VOLT\"", defaultRespondingTime_ms));
                int resistanceEnable = int.Parse(this._chassis.Query(command + "\"RES\"", defaultRespondingTime_ms));
                int result = currentEnable * (int)SenseModeTypes.Current + voltageEnable * (int)SenseModeTypes.Voltage
                    + resistanceEnable * (int)SenseModeTypes.Resistance;

                return (SenseModeTypes)Enum.Parse(typeof(SenseModeTypes), result.ToString());
            }
        }

        private SourceModeTypes _offlineSourceMode;
        /// <summary>
        /// Gets or sets source mode.
        /// </summary>
        public override SourceModeTypes SourceMode
        {
            get
            {
                SourceModeTypes sourceMode;
                string response;
                //lock (this._chassis)
                {
                    var cmd = BuildCommand(":SOUR{0}:FUNC?");
                    response = this._chassis.Query(cmd, defaultRespondingTime_ms).Trim();
                }
                switch (response)
                {
                    case "VOLT":
                        sourceMode = SourceModeTypes.Voltage;
                        break;
                    case "CURR":
                        sourceMode = SourceModeTypes.Current;
                        break;
                    default:
                        throw new NotSupportedException("Unknown Mode");
                }
                return sourceMode;
            }
            set
            {
                _offlineSourceMode = value;
                string mode;
                if (value == SourceModeTypes.Voltage) mode = "VOLT";
                else mode = "CURR";
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:FUNC " + mode));
            }
        }
        /// <summary>
        /// Gets or sets which terminal is for output.
        /// </summary>
        public override SelectTerminal Terminal
        {
            get
            {
                SelectTerminal terminal;
                string response;
                //lock (this._chassis)
                {
                    response = this._chassis.Query(":ROUT:TERM?", defaultRespondingTime_ms).Trim();
                }
                switch (response)
                {
                    case "FRON":
                        terminal = SelectTerminal.Front;
                        break;
                    case "REAR":
                        terminal = SelectTerminal.Rear;
                        break;
                    default:
                        throw new NotSupportedException("Unknown State");
                }
                return terminal;
            }
            set
            {
                if (value == SelectTerminal.Front) this._chassis.TryWrite(":ROUT:TERM FRON");
                else this._chassis.TryWrite(":ROUT:TERM REAR");
            }
        }


        /// <summary>
        /// Gets or sets DC sourcing mode for V-Source.
        /// </summary>
        public VoltageSourceMode VSourceMode
        {
            get
            {
                VoltageSourceMode vSourceMode;
                string response;
                //lock (this._chassis)
                {
                    response = this._chassis.Query((BuildCommand(":SOUR{0}:VOLT:MODE?")), defaultRespondingTime_ms).Trim();

                }
                switch (response)
                {
                    case "FIX":
                        vSourceMode = VoltageSourceMode.Fixed;
                        break;
                    case "LIST":
                        vSourceMode = VoltageSourceMode.List;
                        break;
                    case "SWE":
                        vSourceMode = VoltageSourceMode.Sweep;
                        break;
                    default:
                        throw new NotSupportedException("Unknown Mode");
                }
                return vSourceMode;
            }
            set
            {
                string state = null;
                switch (value)
                {
                    case VoltageSourceMode.Fixed:
                        state = "FIX";
                        break;
                    case VoltageSourceMode.List:
                        state = "LIST";
                        break;
                    case VoltageSourceMode.Sweep:
                        state = "SWE";
                        break;
                    default:
                        throw new NotSupportedException("Unknown Mode");
                }
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:VOLT:MODE " + state));
            }
        }

        /// <summary>
        /// Sets voltage range Eg. Default, down, up, max, min.
        /// </summary>
        public VoltageRange VoltageRangeEnumeration
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                string range;
                switch (value)
                {
                    case VoltageRange.Default:
                        range = "DEF";
                        break;
                    case VoltageRange.Down:
                        range = "DOWN";
                        break;
                    case VoltageRange.Maximum:
                        range = "MAX";
                        break;
                    case VoltageRange.Minimum:
                        range = "MIN";
                        break;
                    case VoltageRange.Up:
                        range = "UP";
                        break;
                    default:
                        throw new NotSupportedException("Unknown Voltage Range Setting");
                }
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:VOLT:RANG " + range));
            }
        }

        /// <summary>
        /// Checks whether the instrument is corrret.
        /// </summary>
        public void CheckIDN()
        {
            string myId = "KEITHLEY INSTRUMENTS INC.,MODEL 24";
            string idn = this.InstrumentIDN;

            if (idn.StartsWith(myId) == false)
            {
                throw new NotSupportedException(string.Format("The instrument IDN '{0}' doesn't start with '{1}'.", idn, myId));
            }
        }

        /// <summary>
        /// Gets or sets current compliance limit in A.
        /// </summary>
        public override double CurrentCompliance_A
        {
            get
            {
                double response;
                //lock (this._chassis)
                {
                    response = double.Parse(this._chassis.Query(BuildCommand(":SENS{0}:CURR:PROT?"), defaultRespondingTime_ms));
                }
                return response;
            }
            set
            {
                this._chassis.TryWrite(BuildCommand(":SENS{0}:CURR:PROT " + value.ToString()));
            }
        }

        /// <summary>
        /// Gets or sets current range in A.
        /// </summary>
        public override double CurrentSenseRange_A
        {
            get
            {
                double response;
                //lock (this._chassis)
                {
                    response = double.Parse(this._chassis.Query(BuildCommand(":SENS{0}:CURR:RANG?"), defaultRespondingTime_ms));
                }
                return response;
            }
            set
            {
                SourceModeTypes temp = this.SourceMode;
                if (SourceMode == SourceModeTypes.Current)
                {
                    SourceMode = SourceModeTypes.Voltage;
                }
                this._chassis.TryWrite(BuildCommand(":SENS{0}:CURR:RANG " + value.ToString()));
                if (temp == SourceModeTypes.Current) SourceMode = SourceModeTypes.Current;
            }
        }

        public double CurrentSourceRange_A
        {
            get
            {
                double response;
                //lock (this._chassis)
                {
                    response = double.Parse(this._chassis.Query(BuildCommand(":SOUR{0}:CURR:RANG?"), defaultRespondingTime_ms));
                }
                return response;
            }
            set
            {
                SourceModeTypes temp = this.SourceMode;
                if (SourceMode == SourceModeTypes.Voltage)
                {
                    SourceMode = SourceModeTypes.Current;
                }
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:CURR:RANG " + value.ToString()));
                if (temp == SourceModeTypes.Voltage) SourceMode = SourceModeTypes.Voltage;
            }
        }

        double _offlineCurrentSetpoint_A;
        /// <summary>
        /// Gets or sets current set point in A.
        /// </summary>
        public override double CurrentSetpoint_A
        {
            get
            {
                double response;
                //lock (this._chassis)
                {
                    response = double.Parse(this._chassis.Query(BuildCommand(":SOUR{0}:CURR:LEV?"), defaultRespondingTime_ms));
                }
                return response;
            }
            set
            {
                _offlineCurrentSetpoint_A = value;
                string command = BuildCommand(":SOUR{0}:CURR:LEV " + value.ToString());
                this._chassis.TryWrite(command);
            }
        }
        /// <summary>
        /// Reads current value in A.
        /// </summary>
        /// <returns></returns>
        public override double ReadCurrent_A()
        {
            string retval;
            //lock (this._chassis)
            {
                retval = this._chassis.Query(":FORM:ELEM CURR;:READ?", defaultRespondingTime_ms);
            }
            return double.Parse(retval);
        }

        /// <summary>
        /// Reads voltage value in V.
        /// </summary>
        /// <returns></returns>
        public override double ReadVoltage_V()
        {
            string retval;
            //lock (this._chassis)
            {
                //this.SetDataElements(DataElement.Voltage);
                //retval = this.FetchData();
                this._chassis.TryWrite(BuildCommand(":FORM:ELEM VOLT"));
                Thread.Sleep(100);
                retval = this._chassis.Query(BuildCommand(":READ?"), defaultRespondingTime_ms);
            }
            return double.Parse(retval);
        }

        /// <summary>
        /// Gets or sets voltage compliance limit in V.
        /// </summary>
        public override double VoltageCompliance_V
        {
            get
            {
                double response;
                //lock (this._chassis)
                {
                    response = Convert.ToDouble(this._chassis.Query(BuildCommand(":SENS{0}:VOLT:PROT?"), defaultRespondingTime_ms));
                }
                return response;
            }
            set
            {
                this._chassis.TryWrite(BuildCommand(":SENS{0}:VOLT:PROT " + value.ToString()));
            }
        }

        /// <summary>
        /// Gets or sets voltage range in V.
        /// </summary>
        public double VoltageSenseRange_V
        {
            get
            {
                double response;
                //lock (this._chassis)
                {
                    response = Convert.ToDouble(this._chassis.Query(BuildCommand(":SENS{0}:VOLT:RANG?"), defaultRespondingTime_ms));
                }
                return response;
            }
            set
            {
                SourceModeTypes temp = this.SourceMode;
                if (SourceMode == SourceModeTypes.Voltage)
                {
                    SourceMode = SourceModeTypes.Current;
                }
                this._chassis.TryWrite(BuildCommand(":SENS{0}:VOLT:RANG " + value.ToString()));
                if (temp == SourceModeTypes.Voltage) SourceMode = SourceModeTypes.Voltage;
            }
        }

        public double VoltageSourceRange_V
        {
            get
            {
                double response;
                //lock (this._chassis)
                {
                    response = Convert.ToDouble(this._chassis.Query(BuildCommand(":SOUR{0}:VOLT:RANG?"), defaultRespondingTime_ms));
                }
                return response;
            }
            set
            {
                SourceModeTypes temp = this.SourceMode;
                if (SourceMode == SourceModeTypes.Current)
                {
                    SourceMode = SourceModeTypes.Voltage;
                }
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:VOLT:RANG " + value.ToString()));
                if (temp == SourceModeTypes.Current) SourceMode = SourceModeTypes.Current;
            }
        }


        private double _offlineVoltageSetpoint_V;
        /// <summary>
        /// Gets or sets voltage set point in V.
        /// </summary>
        public double VoltageSetpoint_V
        {
            get
            {
                double response;
                //lock (this._chassis)
                {
                    response = Convert.ToDouble(this._chassis.Query(BuildCommand(":SOUR{0}:VOLT:LEV?"), defaultRespondingTime_ms));
                }
                return response;
            }
            set
            {
                _offlineVoltageSetpoint_V = value;
                string command = BuildCommand(":SOUR{0}:VOLT:LEV " + value.ToString());

                this._chassis.TryWrite(command);

            }
        }

        /// <summary>
        /// Clears input triggers.
        /// </summary>
        public void ClearInputTriggers()
        {
            this._chassis.TryWrite(":TRIG:CLE");
        }

        /// <summary>
        /// Gets or sets whether auto output off is enabled.
        /// <para>
        /// With auto output-off enabled, an :INITiate (or :READ? or MEASure?) will
        ///start source-measure operation. The output will turn on at the beginning
        ///of each SDM (source-delay-measure) cycle and turn off after each measurement
        ///is completed.
        /// </para>
        /// <para>
        /// With auto output-off disabled, the source output must be on before an
        ///:INITiate or :READ? can be used to start source-measure operation. The
        ///:MEASure? command will automatically turn on the source output.
        ///Once operation is started, the source output will stay on even after the
        ///instrument returns to the idle state. Auto output-off disabled is the *RST
        ///and :SYSTem:PRESet default.
        /// </para>
        /// </summary>
        public bool IsAutoOutputOff
        {
            get
            {
                //lock (this._chassis)
                {
                    if (Convert.ToBoolean(Convert.ToByte(this._chassis.Query(BuildCommand(":SOUR{0}:CLE:AUTO?"), defaultRespondingTime_ms)))) return true;
                    else return false;
                }
            }
            set
            {
                string state;
                if (value) state = "1";
                else state = "0";
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:CLE:AUTO " + state));
            }
        }

        /// <summary>
        /// Gets or sets whether beep sound is on.
        /// </summary>
        public bool IsBeepOn
        {
            get
            {
                //lock (this._chassis)
                {
                    if (Convert.ToBoolean(Convert.ToByte(this._chassis.Query(":SYST:BEEP:STAT?", defaultRespondingTime_ms)))) return true;
                    else return false;
                }
            }
            set
            {
                string state;
                if (value) state = "1";
                else state = "0";
                this._chassis.TryWrite(":SYST:BEEP:STAT " + state);
            }
        }

        /// <summary>
        /// Gets or sets whether current range is auto.
        /// </summary>
        public override  bool IsCurrentSourceAutoRangeOn
        {
            get
            {
                //lock (this._chassis)
                {
                    if (Convert.ToBoolean(Convert.ToByte(this._chassis.Query(BuildCommand(":SOUR{0}:CURR:RANG:AUTO?"), defaultRespondingTime_ms)))) return true;
                    else return false;
                }
            }
            set
            {
                if (value) this._chassis.TryWrite(BuildCommand(":SOUR{0}:CURR:RANG:AUTO 1"));
                else this._chassis.TryWrite(BuildCommand(":SOUR{0}:CURR:RANG:AUTO 0"));
            }
        }

        public override bool IsCurrentSenseAutoRangeOn
        {
            get
            {
                //lock (this._chassis)
                {
                    if (Convert.ToBoolean(Convert.ToByte(this._chassis.Query(BuildCommand(":SENS{0}:CURR:RANG:AUTO?"), defaultRespondingTime_ms)))) return true;
                    else return false;
                }
            }
            set
            {
                if (value) this._chassis.TryWrite(BuildCommand(":SENS{0}:CURR:RANG:AUTO 1"));
                else this._chassis.TryWrite(BuildCommand(":SENS{0}:CURR:RANG:AUTO 0"));
            }
        }

       


        public override bool IsVoltageSenseAutoRangeOn
        {
            get
            {
                lock (this._chassis)
                {
                    if (Convert.ToBoolean(Convert.ToByte(this._chassis.Query(BuildCommand(":SENS{0}:VOLT:RANG:AUTO?"), defaultRespondingTime_ms)))) return true;

                    else return false;
                }
            }
            set
            {
                string state;
                if (value) state = "1";
                else state = "0";
                this._chassis.TryWrite(BuildCommand(":SENS{0}:VOLT:RANG:AUTO " + state));
            }
        }

      

        /// <summary>
        /// Gets or sets whether four wire is on.
        /// </summary>
        public override bool IsFourWireOn
        {
            get
            {
                //lock (this._chassis)
                {
                    if (Convert.ToBoolean(Convert.ToByte(this._chassis.Query(":SYST:RSEN?", defaultRespondingTime_ms)))) return true;
                    else return false;
                }
            }
            set
            {
                string state;
                if (value) state = "1";
                else state = "0";
                this._chassis.TryWrite(":SYST:RSEN " + state);
            }
        }

        bool _offlineOutputOn;
        /// <summary>
        /// Gets or sets whether output is enabled.
        /// </summary>
        public override bool IsOutputOn
        {
            get
            {
                //lock (this._chassis)
                {
                    if (Convert.ToBoolean(Convert.ToByte(this._chassis.Query(BuildCommand(":OUTP{0}?"), defaultRespondingTime_ms)))) return true;
                    else return false;
                }
            }
            set
            {
                _offlineOutputOn = value;
                string state;
                if (value) state = "1";
                else state = "0";
                this._chassis.TryWrite(BuildCommand(":OUTP{0} " + state));
            }
        }

        /// <summary>
        /// Gets or sets whether voltage range is auto.
        /// </summary>
        public override bool IsVoltageSourceAutoRangeOn
        {
            get
            {
                //lock (this._chassis)
                {
                    if (Convert.ToBoolean(Convert.ToByte(this._chassis.Query(BuildCommand(":SOUR{0}:VOLT:RANG:AUTO?"), defaultRespondingTime_ms)))) return true;
                    else return false;
                }
            }
            set
            {
                string state;
                if (value) state = "1";
                else state = "0";
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:VOLT:RANG:AUTO " + state));
            }
        }

      

        public double IMeasurementIntegrationPeriod
        {
            get
            {
                double iIntegrationPeriod = 0;
                //lock (this._chassis)
                {
                    iIntegrationPeriod = Convert.ToDouble(this._chassis.Query(BuildCommand(":SENS{0}:CURR:NPLC?"), defaultRespondingTime_ms));
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
                this._chassis.TryWrite(BuildCommand(":SENS{0}:CURR:NPLC " + value.ToString()));
            }
        }

        public double VMeasurementIntegrationPeriod
        {
            get
            {
                double vIntegrationPeriod = 0;
                //lock (this._chassis)
                {
                    vIntegrationPeriod = Convert.ToDouble(this._chassis.Query(BuildCommand(":SENS{0}:VOLT:NPLC?"), defaultRespondingTime_ms));
                }
                return vIntegrationPeriod;
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
                this._chassis.TryWrite(BuildCommand(":SENS{0}:VOLT:NPLC " + value.ToString()));
            }
        }


        public bool IsCurrentInCompliance
        {
            get
            {
                string command = BuildCommand(":SENS{0}:CURR:PROT:TRIP?");
                string response;
                //lock (this._chassis)
                {
                    response = this._chassis.Query(command, defaultRespondingTime_ms).Trim();
                }
                if (int.Parse(response) == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsVoltageInCompliance
        {
            get
            {
                string command = BuildCommand(":SENS{0}:VOLT:PROT:TRIP?");
                string response;
                //lock (this._chassis)
                {
                    response = this._chassis.Query(command, defaultRespondingTime_ms).Trim();
                }
                if (int.Parse(response) == 1)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Sets voltage in V with refreshing to trigger reading.
        /// </summary>
        /// <param name="voltage_V"></param>
        public void SetVoltageWithRefreshing(double voltage_V)
        {
            string command = BuildCommand(":SOUR{0}:VOLT:LEV " + voltage_V.ToString());

            command += ";:INIT";

            this._chassis.TryWrite(command);
        }

        /// <summary>
        /// Sets current in A with refreshing to trigger reading.
        /// </summary>
        /// <param name="current_A"></param>
        public void SetCurrentWithRefreshing(double current_A)
        {
            string command = BuildCommand(":SOUR{0}:CURR:LEV " + current_A.ToString() + ";:INIT");
            this._chassis.TryWrite(command);
        }


        public bool IsSourceDelayAuto
        {
            get
            {
                string command = BuildCommand(":SOUR{0}:DEL:AUTO?");
                string response = this._chassis.Query(command, defaultRespondingTime_ms).Trim();
                if (int.Parse(response) == 1) return true;
                return false;
            }
            set
            {
                string command = "1";
                if (!value) command = "0";
                command = BuildCommand(":SOUR{0}:DEL:AUTO ") + command;
                this._chassis.TryWrite(command);
            }
        }

        public double SourceDelay_s
        {
            get
            {
                string command = BuildCommand(":SOUR{0}:DEL?");
                string response = this._chassis.Query(command, defaultRespondingTime_ms);
                return double.Parse(response);
            }
            set
            {
                string command = BuildCommand(":SOUR{0}:DEL ") + value.ToString();
                this._chassis.TryWrite(command);
            }
        }

        public void SetDataElements(DataElement element, params DataElement[] elements)
        {
            string elementString = element.ToString();
            foreach (var item in elements)
            {
                elementString += "," + item.ToString();
            }
            string command = ":Form:Elem " + elementString;
            this._chassis.TryWrite(command);
        }



        /// <summary>
        /// Adds slot number to command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected string BuildCommand(string command)
        {
            return string.Format(command, 1);
        }
        #endregion

        public int SourceNumber
        {
            get { return 1; }
        }

        public SenseModeTypes SenseAbility
        {
            get { return SenseModeTypes.Current | SenseModeTypes.Voltage | SenseModeTypes.Resistance; }
        }

        public SourceModeTypes SourceAbility
        {
            get { return SourceModeTypes.Current | SourceModeTypes.Voltage; }
        }

        //public IKeithleyTriggerable TriggerControl { get; private set; }

        #region k2400 only
        /// <summary>
        /// Sets the k2400 to local control mode.
        /// </summary>
        public void SetToLocal()
        {
            this._chassis.TryWrite(":syst:key 23");
        }

        /// <summary>
        /// Gets or sets whether close display is on.
        /// </summary>
        public bool IsDisplayOn
        {
            get
            {
                //lock (this._chassis)
                {
                    if (Convert.ToBoolean(Convert.ToByte(this._chassis.Query(":DISP:ENAB?", defaultRespondingTime_ms)))) return true;
                    else return false;
                }
            }
            set
            {
                string state;
                if (value) state = "1";
                else state = "0";
                this._chassis.TryWrite(":DISP:ENAB " + state);
            }
        }

        /// <summary>
        /// Reads power value in W.
        /// </summary>
        /// <returns></returns>
        public double ReadPower_W()
        {
            return this.ReadCurrent_A() * this.ReadVoltage_V();
        }

        /// <summary>
        /// Gets or sets a value indicating whether concurrent measurement enabled.
        /// <para>This command is used to enable or disable the ability of the instrument
        /// to measure more than one function simultaneously. When enabled, the
        /// instrument will measure the functions that are selected.</para>
        /// <para></para>
        /// When disabled, only one measurement function can be enabled. When
        /// making the transition from :CONCurrent ON to :CONCurrent OFF, the
        /// voltage (VOLT:DC) measurement function will be selected. All other
        /// measurement functions will be disabled. Use the :FUNCTion[:ON]
        /// command to select one of the other measurement functions.
        /// </summary>
        /// <value>
        /// <c>true</c> if concurrent measurement is enabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsConcurrentMeasurementEnabled
        {
            get
            {
                string command = this.BuildCommand(":SENSE{0}:FUNCTION:CONCURRENT?");
                int myReturn = Convert.ToInt16(this._chassis.Query(command, defaultRespondingTime_ms));
                if (myReturn == 0)
                {
                    return false;
                }

                return true;
            }
            set
            {
                string myValue = value ? "1" : "0";
                string command = this.BuildCommand(":SENSE{0}:FUNCTION:CONCURRENT " + myValue);
                this._chassis.TryWrite(command);

            }
        }
        #endregion
        public Func<int> Sign { get; private set; }
        public void SetSignFunction(Func<int> sign)
        {
            this.Sign = sign;
        }

        #region trigger
        /// <summary>
        /// Setups a source meter as voltage source/ current sense for trigger.
        /// </summary>
        /// 
        /// <param name="isMaster">Indicates whether the smu is master for trigger or slaver.</param>
        /// <param name="complianceCurrent_A">Compliance current in A.
        /// <para></para>If compliance voltage is not positive, compliance voltage will not be set.</param>
        /// <param name="measurementIntergrationPeriod">NPLC value. If measurementIntergrationPeriod is not positive, NPLC value not be set.</param>
        /// <param name="triggerInputLine">The trigger signal input line.
        /// <para></para>
        /// If the trigger signal input line is None, TriggerLayerSource will be TriggerEventSource.Immediate</param>
        /// <param name="triggerOutputLine">The trigger signal output line.</param>
        /// <param name="triggerCount">The count of trigger.</param>
        /// <param name="start">The current array to be set for trigger.</param>
        public void SetupVoltageStairSweep(bool isMaster,
            double complianceCurrent_A, double measurementIntergrationPeriod,
            TriggerLine triggerInputLine, TriggerLine triggerOutputLine, int triggerCount, bool remoteSense, SelectTerminal terminal, double start, double stop, double step)
        {
            this.Timeout_ms = 60 * 1000;
            this.ResetTriggerableSourceMeter();
            this.SourceMode = SourceModeTypes.Voltage;
            this.SenseMode = SenseModeTypes.Current;
            this.IsFourWireOn = remoteSense;
            this.Terminal = terminal;
            this.IsVoltageSourceAutoRangeOn = false;
            this.VoltageSourceRange_V = stop;
            this.VoltageSetpoint_V = start;
            this.CurrentCompliance_A = complianceCurrent_A;
            this.IsCurrentSenseAutoRangeOn = false;
            this.CurrentSenseRange_A = complianceCurrent_A;
            this.IMeasurementIntegrationPeriod = measurementIntergrationPeriod;

            this._chassis.TryWrite("*CLS");
            this._chassis.TryWrite(":TRACE:CLE");
            this.AutoZero = ControlAutoZero.Off;
            this.IsSourceDelayAuto = true;
            this.TriggerDelay_s = 0;
            this.SetDataElements(DataElement.Current);

            this.VSourceMode = VoltageSourceMode.Sweep;
            this.VoltageSweepStart = start;
            this.VoltageSweepStop = stop;
            this.VoltageSweepStep = step;

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
                this.TriggerLayerInput = TriggerLayerInputs.Source;
                this.TriggerLayerDirection = TriggerDirection.SOURCE;
            }
            else
            {
                this.TriggerLayerInput = TriggerLayerInputs.Sense;
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
            this.AutoZero = ControlAutoZero.Once;
            this.TriggerCount = triggerCount;
            this.IsOutputOn = true;
        }
        public void SetupFixedVoltageSweep(bool isMaster,
           double complianceCurrent_A, double measurementIntergrationPeriod,
           TriggerLine triggerInputLine, TriggerLine triggerOutputLine, int triggerCount, bool remoteSense, SelectTerminal terminal, double voltage)
        {
            this.Timeout_ms = 60 * 1000;
            this.ResetTriggerableSourceMeter();
            this.SourceMode = SourceModeTypes.Voltage;
            this.SenseMode = SenseModeTypes.Current;
            this.IsFourWireOn = remoteSense;
            this.Terminal = terminal;
            this.IsVoltageSourceAutoRangeOn = false;
            this.VoltageSourceRange_V = voltage;
            this.VoltageSetpoint_V = voltage;
            this.CurrentCompliance_A = complianceCurrent_A;
            this.IsCurrentSenseAutoRangeOn = false;
            this.CurrentSenseRange_A = complianceCurrent_A;
            this.IMeasurementIntegrationPeriod = measurementIntergrationPeriod;

            this._chassis.TryWrite("*CLS");
            this._chassis.TryWrite(":TRACE:CLE");
            this.AutoZero = ControlAutoZero.Off;
            this.IsSourceDelayAuto = true;
            this.TriggerDelay_s = 0;
            this.SetDataElements(DataElement.Current);

            this.VSourceMode = VoltageSourceMode.Fixed;

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
                this.TriggerLayerInput = TriggerLayerInputs.Source;
                this.TriggerLayerDirection = TriggerDirection.SOURCE;
            }
            else
            {
                this.TriggerLayerInput = TriggerLayerInputs.Sense;
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
            this.AutoZero = ControlAutoZero.Once;
            this.TriggerCount = triggerCount;
            this.IsOutputOn = true;
        }

        /// <summary>
        /// Setups a source meter as current source/voltage sense for trigger.
        /// </summary>
        /// <param name="smu">The source meter unit to be set.</param>
        /// <param name="isMaster">Indicates whether the smu is master for trigger or slaver.</param>
        /// <param name="complianceVoltage_V">Compliance voltage in V.
        /// <para></para>If compliance voltage is not positive, compliance voltage will not be set.</param>
        /// <param name="voltageRange_V">Voltage range in V. 
        /// <para></para>If voltage range is negative or 0, voltage range is auto.
        /// <para></para>If voltage range is double.NaN, voltage range will not be set.</param>
        /// <param name="measurementIntergrationPeriod">NPLC value. If measurementIntergrationPeriod is not positive, NPLC value not be set.</param>
        /// <param name="triggerInputLine">The trigger signal input line.
        /// <para></para>
        /// If the trigger signal input line is None, TriggerLayerSource will be TriggerEventSource.Immediate</param>
        /// <param name="triggerOutputLine">The trigger signal output line.</param>
        /// <param name="triggerCount">The count of trigger.</param>
        /// <param name="dataElements">The data elements to be stored in memory when trigger.</param>
        /// <param name="current_A">The current array to be set for trigger.</param>

        public override void SetupCurrentStairSweep(bool isMaster,
   double complianceVoltage_V, double measurementIntergrationPeriod,
   TriggerLine triggerInputLine, TriggerLine triggerOutputLine, int triggerCount, bool remoteSense, SelectTerminal terminal, double start, double stop, double step)
        {
            this.Timeout_ms = 60 * 1000;
            this.ResetTriggerableSourceMeter();
            this.SourceMode = SourceModeTypes.Current;
            this.SenseMode = SenseModeTypes.Voltage;
            this.IsFourWireOn = remoteSense;
            this.Terminal = terminal;
            this.IsCurrentSourceAutoRangeOn = false;
            this.CurrentSourceRange_A = stop;
            this.CurrentSetpoint_A = start;
            this.VoltageCompliance_V = complianceVoltage_V;
            this.IsVoltageSenseAutoRangeOn = false;
            this.VoltageSenseRange_V = complianceVoltage_V;
            this.VMeasurementIntegrationPeriod = measurementIntergrationPeriod;

            this._chassis.TryWrite("*CLS");
            this._chassis.TryWrite(":TRACE:CLE");
            this.AutoZero = ControlAutoZero.Off;

            this.IsSourceDelayAuto = true;
            this.TriggerDelay_s = 0;
            this.TriggerCount = triggerCount;

            this.ISourceMode = CurrentSourceMode.Sweep;
            this.CurrentSweepStart = start;
            this.CurrentSweepStop = stop;
            this.CurrentSweepStep = step;

            this.SetDataElements(DataElement.Voltage);

            this.ArmCount = 1;
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
                this.TriggerLayerInput = TriggerLayerInputs.Source;
                this.TriggerLayerDirection = TriggerDirection.SOURCE;
            }
            else
            {
                this.TriggerLayerInput = TriggerLayerInputs.Sense;
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
            this.AutoZero = ControlAutoZero.Once;
            this.IsOutputOn = true;
        }

        public void SetupFixedCurrentSweep(bool isMaster,
   double complianceVoltage_V, double measurementIntergrationPeriod,
   TriggerLine triggerInputLine, TriggerLine triggerOutputLine, int triggerCount, bool remoteSense, SelectTerminal terminal, double current)
        {
            this.Timeout_ms = 60 * 1000;
            this.ResetTriggerableSourceMeter();
            this.SourceMode = SourceModeTypes.Current;
            this.SenseMode = SenseModeTypes.Voltage;
            this.IsFourWireOn = remoteSense;
            this.Terminal = terminal;
            this.IsCurrentSourceAutoRangeOn = false;
            this.CurrentSourceRange_A = current;
            this.CurrentSetpoint_A = current;
            this.VoltageCompliance_V = complianceVoltage_V;
            this.IsVoltageSenseAutoRangeOn = false;
            this.VoltageSenseRange_V = complianceVoltage_V;
            this.VMeasurementIntegrationPeriod = measurementIntergrationPeriod;

            this._chassis.TryWrite("*CLS");
            this._chassis.TryWrite(":TRACE:CLE");
            this.AutoZero = ControlAutoZero.Off;

            this.IsSourceDelayAuto = true;
            this.TriggerDelay_s = 0;
            this.TriggerCount = triggerCount;

            this.ISourceMode = CurrentSourceMode.Fixed;


            this.SetDataElements(DataElement.Voltage);

            this.ArmCount = 1;
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
                this.TriggerLayerInput = TriggerLayerInputs.Source;
                this.TriggerLayerDirection = TriggerDirection.SOURCE;
            }
            else
            {
                this.TriggerLayerInput = TriggerLayerInputs.Sense;
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
            this.AutoZero = ControlAutoZero.Once;
            this.IsOutputOn = true;
        }

        public int GetCurrentSourcePointListCount()
        {
            string command = BuildCommand(":SOUR{0}:LIST:CURR:POIN?");
            string response = this._chassis.Query(command, defaultRespondingTime_ms);
            return int.Parse(response);
        }

        public int GetVoltageSourcePointListCount()
        {
            string command = BuildCommand(":SOUR{0}:LIST:VOLT:POIN?");
            string response = this._chassis.Query(command, defaultRespondingTime_ms);
            return int.Parse(response);
        }

        public void SetVoltageSourcePointList(double[] voltages_V)
        {
            this.SetSourcePointList(voltages_V, "VOLT");
        }

        public void SetCurrentSourcePointList(double[] currents_A)
        {
            this.SetSourcePointList(currents_A, "CURR");

        }

        private void SetSourcePointList(double[] values, string sourceName)
        {
            int loopCount = (values.Length - 1) / 100 + 1;

            for (int index = 0; index < loopCount; index++)
            {
                if (index != 0)
                {
                    Thread.Sleep(100);
                }
                string command = BuildCommand(":Source{0}:List:") + sourceName + (index == 0 ? " " : ":Append ")
                        + GetArrayString(values, index * 100, 100);
                this._chassis.TryWrite(command);
            }
        }

        private string GetArrayString(double[] values, int startIndex, int count)
        {
            StringBuilder builder = new StringBuilder();
            int endIndex = Math.Min(startIndex + count, values.Length);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (i != startIndex)
                {
                    builder.Append(",");
                }
                builder.Append((1.0 * values[i]).ToString("0.000000"));
                //builder.Append((this.Sign() * values[i]).ToString("0.000000"));
            }
            return builder.ToString();
        }

        private double CurrentSweepStart
        {
            get
            {
                double response = Convert.ToDouble(this._chassis.Query(BuildCommand(":SOUR{0}:CURR:START?"), defaultRespondingTime_ms));
                return response;
            }
            set
            {
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:CURR:START " + value.ToString()));
            }
        }
        private double CurrentSweepStop
        {
            get
            {
                double response = Convert.ToDouble(this._chassis.Query(BuildCommand(":SOUR{0}:CURR:STOP?"), defaultRespondingTime_ms));
                return response;
            }
            set
            {
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:CURR:STOP " + value.ToString()));
            }
        }
        private double CurrentSweepStep
        {
            get
            {
                double response = Convert.ToDouble(this._chassis.Query(BuildCommand(":SOUR{0}:CURR:STEP?"), defaultRespondingTime_ms));
                return response;
            }
            set
            {
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:CURR:STEP " + value.ToString()));
            }
        }
        private double VoltageSweepStart
        {
            get
            {
                double response = Convert.ToDouble(this._chassis.Query(BuildCommand(":SOUR{0}:VOLT:START?"), defaultRespondingTime_ms));
                return response;
            }
            set
            {
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:VOLT:START " + value.ToString()));
            }
        }
        private double VoltageSweepStop
        {
            get
            {
                double response = Convert.ToDouble(this._chassis.Query(BuildCommand(":SOUR{0}:VOLT:STOP?"), defaultRespondingTime_ms));
                return response;
            }
            set
            {
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:VOLT:STOP " + value.ToString()));
            }
        }
        private double VoltageSweepStep
        {
            get
            {
                double response = Convert.ToDouble(this._chassis.Query(BuildCommand(":SOUR{0}:VOLT:STEP?"), defaultRespondingTime_ms));
                return response;
            }
            set
            {
                this._chassis.TryWrite(BuildCommand(":SOUR{0}:VOLT:STEP " + value.ToString()));
            }
        }
        #endregion

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
           // throw new NotImplementedException();
        }
    }

}
