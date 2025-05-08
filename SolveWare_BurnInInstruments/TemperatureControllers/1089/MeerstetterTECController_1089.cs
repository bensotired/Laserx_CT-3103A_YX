using LX_BurnInSolution.Utilities;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Xml.Linq;


namespace SolveWare_BurnInInstruments
{
    public class MeerstetterTECController_1089 : InstrumentBase, IInstrumentBase// ITemperatureController 
    {
        private UInt16 SequenceNumber = 0;
        private int Instance = 1;
        const int DELAY_MS = 1000;
        const int LONG_DELAY_MS = 1500;
        public MeerstetterTECController_1089(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {
            this.CurrentLimit_A = 0;
            this.TemperatureUnit = TemperatureUnit.Celsius;
        }
        double _temperatureSetPoint_DegreeC = 25.0;
        public double TemperatureSetPoint_DegreeC
        {
            set
            {
                _temperatureSetPoint_DegreeC = value;

                if (this._isOnline == false)
                {
                    return;
                }
                lock (this._chassis)
                {
                    this.TemperatureUnit = TemperatureUnit.Celsius;
                    string command = BuildCommand("VS", (int)MeerstetterTECCommands.SetTemperatureSetPoint, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));

                    string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.GetTemperatureSetPoint, "");
                        var retfm = this._chassis.Query(command, LONG_DELAY_MS);
                   
                        Thread.Sleep(200);
                        string response = this.GetResponseFrame(retfm);
                        _temperatureSetPoint_DegreeC = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));

                    }
                }
                else
                {
                   
                }
                return _temperatureSetPoint_DegreeC;
            }
        }
        //public double CurrentTemperature   //输出一个给全国人民用 
        //{
        //    get
        //    {
        //        if (this._isOnline == true)
        //        {
        //            return _currentObjectTemperature;
        //        }
        //        else return 0;
        //    }

        //}
        private double CurrentLimit_A { get; set; }

        public  bool StabilizeTemperature(double temperatureToSet_DegreeC, double minStableTime_s,
            double temperatureTolerance_DegreeC, double timeOut_s, int minStableCount, CancellationToken token)
        {
            if(this.IsOnline == false)
            {
                return false;
            }
            double currentLimit_A = Math.Round(this.CurrentProtectionLimit_A, 1);
            if (currentLimit_A < this.CurrentLimit_A)
            {
                currentLimit_A = this.CurrentLimit_A;
            }
            else
            {
                this.CurrentLimit_A = currentLimit_A;
            }
 
            bool ok = false;
 
            this.SourceFunction = MeerstetterTECSourceFunction.TemperatureControllerMode;
  
            this.TemperatureSetPoint_DegreeC = temperatureToSet_DegreeC;
            this.IsOutputEnabled = true;
 
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            double stableStartTime = double.NaN;
            double limit_A = this.CurrentProtectionLimit_A;

            while (true)
            {
                Thread.Sleep(250);
                int status = this.ReadMeasurementEventStatus();


                if (status == 3)
                {
 
                    break;
                }

                bool isEnable = this.IsOutputEnabled;
                if (!isEnable)
                {
                    break;
                }
 
                if (isEnable && (status != 3) && (isStable == 2))
                {
                    if (double.IsNaN(stableStartTime))
                    {
                        stableStartTime = stopWatch.Elapsed.TotalSeconds;
                    }
                }

                if (!double.IsNaN(stableStartTime))
                {
                    if (stopWatch.Elapsed.TotalSeconds - stableStartTime > minStableTime_s)
                    {
                        ok = true;
                        break;
                    }
                }

                if (stopWatch.Elapsed.TotalSeconds > timeOut_s)
                {
                    break;
                }

                if (token.IsCancellationRequested)
                {

                    break;

                }

                if ((this.IsPowerCheckWhenStablizing) && (this.MaxPower_W > 0))
                {
                    double power_w = this.ReadPower_W();
                    if (power_w >= this.MaxPower_W)
                    {
                    }
                }
            }
            stopWatch.Stop();
            return ok;
        }

  

        private string _instrumentIdn=string.Empty;
        /// <summary>
        /// Gets IDN of the instrument.
        /// </summary>
        public string InstrumentIDN
        {
            get
            {
                if (this._isOnline == false)
                {
                    return _instrumentIdn;
                }
                if (string.IsNullOrEmpty(_instrumentIdn))
                {
                    lock (this._chassis)
                    {
                        _instrumentIdn = this._chassis.Query(BuildCommand("?IF", "", ""), DELAY_MS).Substring(7);
                    }
                }

                return _instrumentIdn;
            }
        }
        /// <summary>
        /// Gets or sets whether the TEC controller output is enabled.
        /// </summary>
        bool _isOutputEnabled = false;
        public bool IsOutputEnabled
        {
            get
            {
                if (this._isOnline == false)
                {
                    return _isOutputEnabled;
                }
                string response = string.Empty;
                lock (this._chassis)
                {
                    string command = BuildCommand("?VR", (int)MeerstetterTECCommands.OutputEnabled, "");
                    response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                }
                int status = Convert.ToInt32(response.Substring(7, 8), 16);
                if (status == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                _isOutputEnabled = value;
                if (this._isOnline == false)
                {
                    return;
                }
                lock (this._chassis)
                {

                    if (_isOutputEnabled == true)
                    {
                        string temp = this.GetResponseFrame(this._chassis.Query(BuildCommand("VS", (int)MeerstetterTECCommands.OutputEnabled, "00000001"), DELAY_MS));
                    }
                    else if (_isOutputEnabled == false)
                    {
                        string temp = this.GetResponseFrame(this._chassis.Query(BuildCommand("VS", (int)MeerstetterTECCommands.OutputEnabled, "00000000"), DELAY_MS));
                    }
                }
            }
        }
        double _currentObjectTemperature = 25.0;
        public double CurrentObjectTemperature
        {
            get
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.GetTemperature, "");
                        string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));

                        if (string.IsNullOrEmpty(response)==true)
                        {
                            this._chassis.Log_Global($"[{this.Name}-{this._chassis.Resource}] GetResponseFrame(string response) exception");
                        }
                        else
                        {
                            _currentObjectTemperature =Math.Round( ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8)),3);
                        }
                    }
                }
                return _currentObjectTemperature;
            }
        }

        public double ReadSinkTemperature_DegreeC()
        {
            if (this._isOnline == false)
            {
                return 25.0;
            }
            string response = string.Empty;
            lock (this._chassis)
            {
                string command = BuildCommand("?VR", (int)MeerstetterTECCommands.GetSinkTemperature, "");
                response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
            }
            return ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
 
        }

        public double ReadPower_W()
        {
            if (this._isOnline == false)
            {
                return 0.0;
            }
            double current_A=0.0;
            double voltage_V=0.0;
            lock (this._chassis)
            {
                string command = BuildCommand("?VR", (int)MeerstetterTECCommands.GetTECCurrent, "");
                string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                current_A = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));

                command = BuildCommand("?VR", (int)MeerstetterTECCommands.GetTECVoltage, "");
                response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                voltage_V = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
            }

            return current_A * voltage_V;
        }
        public int ReadMeasurementEventStatus()
        {
            if (this._isOnline == false)
            {
                return 0;
            }
            string response = string.Empty;
            lock (this._chassis)
            {
                string command = BuildCommand("?VR", (int)MeerstetterTECCommands.GetDeviceStatus, "");
                response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
            }

            //20190404 发现数据处理的BUG
            return (int)(Convert.ToUInt32(response.Substring(7, 8), 16));
        }
        #region Meerstetter only
 

        public bool IsPowerCheckWhenStablizing { get; set; }

        public double MaxPower_W { get; set; }
        MeerstetterTECSourceFunction _sourceFunction;
        public MeerstetterTECSourceFunction SourceFunction
        {
            get
            {
                if (this._isOnline == false)
                {
                    return _sourceFunction;
                }
                string response = string.Empty;
                lock (this._chassis)
                {
                    string command = BuildCommand("?VR", (int)MeerstetterTECCommands.Mode, "");
                    response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                }
                
                switch (Convert.ToUInt32(response.Substring(7, 8), 16))
                {
                    case 0:
                        _sourceFunction = MeerstetterTECSourceFunction.StaticMode;
                        break;
                    case 1:
                        _sourceFunction = MeerstetterTECSourceFunction.LiveMode;
                        break;
                    case 2:
                    default:
                        _sourceFunction = MeerstetterTECSourceFunction.TemperatureControllerMode;
                        break;
                }
                return _sourceFunction;
            }
            set
            {
                _sourceFunction = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.Mode, ((int)value).ToString("X8"));
                        string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                        if (_sourceFunction == MeerstetterTECSourceFunction.LiveMode)
                        {
                            command = BuildCommand("VS", (int)MeerstetterTECCommands.TemperatureSourceMode, "00000001");
                            response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                        }
                    }
                }
                else
                {

                }
             
            }
        }


        public TemperatureUnit TemperatureUnit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status is stable.
        /// </summary>
        public int isStable
        {
            get
            {
                if (this._isOnline == true)
                {
                    string response = string.Empty;
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.GetStableStatus, "");
                        response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                    }
                    return Convert.ToInt32(response.Substring(7, 8), 16);
                }
                return 0;
            }
        }
        double _currentProtectionLimit_A = 0.0;
        /// <summary>
        /// Gets or sets current protection limit in A.
        /// </summary>
        public double CurrentProtectionLimit_A
        {
            set
            {
                _currentProtectionLimit_A = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.CurrentProtectionLimit, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    string response = string.Empty;
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.CurrentProtectionLimit, "");
                        response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                    }
                    _currentProtectionLimit_A= ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                }
                return _currentProtectionLimit_A;
            }
        }
        double _voltageProtectionLimit_V = 0.0;
        /// <summary>
        /// Gets or sets voltage protection limit in V.
        /// </summary>
        public double VoltageProtectionLimit_V
        {
            set
            {
                _voltageProtectionLimit_V = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.VoltageProtectionLimit, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    string response = string.Empty;
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.VoltageProtectionLimit, "");
                        response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                        _voltageProtectionLimit_V = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                    }
                }
                return _voltageProtectionLimit_V;
            }
        }
        #region PID setting
        double _temperatureGain=0.0;
        /// <summary>
        /// Gets or sets proportional constant value of PID.
        /// </summary>
        public double TemperatureGain
        {
            set
            {
                _temperatureGain = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.PIDGain, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.PIDGain, "");
                        string response = this.GetResponseFrame(this._chassis.Query(command,DELAY_MS));
                        _temperatureGain = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                    }
                }
                return _temperatureGain;
            }
        }
        double _temperatureDerivative = 0.0;
        /// <summary>
        /// Gets or sets derivative constant value of PID.
        /// </summary>
        public double TemperatureDerivative 
        {
            set
            {
                _temperatureDerivative = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.PIDDerivative, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    string command = BuildCommand("?VR", (int)MeerstetterTECCommands.PIDDerivative, "");
                    string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                    _temperatureDerivative = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                }
                return _temperatureDerivative;
            }
        }
        double _temperatureIntegral = 0.0;
        /// <summary>
        /// Gets or sets integral constant value of PID.
        /// </summary>
        public double TemperatureIntegral
        {
            set
            {
                _temperatureIntegral = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.PIDIntegral, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                      this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.PIDIntegral, "");
                        string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                        _temperatureIntegral = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                    }
                }
                return _temperatureIntegral;
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets whether four wire is on.
        /// </summary>
        public bool IsFourWireOn
        {
            get;
            set;
        }


        public string BuildCommand(string Command, int ID, string Data)
        {
            byte[] uc = Encoding.UTF8.GetBytes(Command + ID.ToString("X4") + this.Instance.ToString("X2") + Data);
            return BuildCommand(uc);
        }
        public string BuildCommand(string Command, string Instance, string Data)
        {
            byte[] uc = Encoding.UTF8.GetBytes(Command + Instance + Data);
            return BuildCommand(uc);
        }
        public string BuildCommand(string Command, string Data)
        {
            return BuildCommand(Command, this.Instance.ToString("X2"), Data);
        }
        public string BuildCommand(byte[] ucData)
        {
            int newAddr = Convert.ToInt32(this.Address);
            char[] ucHex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            int Length = ucData.Length;
            char[] buffer = new char[12 + Length];
            ushort usCRC = 0;

            buffer[0] = (char)0x23;
            buffer[1] = ucHex[newAddr / 16];
            buffer[2] = ucHex[newAddr % 16];
            this.SequenceNumber++;
            if(this.SequenceNumber>60000) this.SequenceNumber=1;  //20190410 防止数据溢出
            buffer[3] = ucHex[this.SequenceNumber / 4096];
            buffer[4] = ucHex[(this.SequenceNumber / 256) % 16];
            buffer[5] = ucHex[(this.SequenceNumber % 256) / 16];
            buffer[6] = ucHex[(this.SequenceNumber % 256) % 16];
            for (int i = 0; i < Length; i++)
            {
                buffer[7 + i] = (char)ucData[i];
            }
            usCRC = 0;
            for (int i = 0; i < 7 + ucData.Length; i++)
            {
                usCRC = CRC16Algorithm(usCRC, (byte)buffer[i]);
            }
            buffer[7 + Length] = (ucHex[usCRC / 4096]);
            buffer[8 + Length] = (ucHex[(usCRC / 256) % 16]);
            buffer[9 + Length] = (ucHex[(usCRC % 256) / 16]);
            buffer[10 + Length] = (ucHex[(usCRC % 256) % 16]);
            buffer[11 + Length] = (char)0x0D;
            string command = new string(buffer);
            return new string(buffer);
        }

        ushort CRC16Algorithm(ushort CRC, byte Ch)
        {
            uint genPoly = 0x1021; //CCITT CRC-16 Polynominal
            uint uiCharShifted = ((uint)Ch & 0x00FF) << 8;
            CRC = (ushort)(CRC ^ uiCharShifted);
            for (int i = 0; i < 8; i++)
            {
                if ((CRC & 0x8000) > 0)
                {
                    CRC = (ushort)((CRC << 1) ^ genPoly);
                }
                else
                {
                    CRC = (ushort)(CRC << 1);
                }
            }
            CRC &= 0xFFFF;
            return CRC;
        }
        public string GetResponseFrame(string response)
        {
            string[] responseList = response.Split('\r');
            string frame = null;
            try
            {
                foreach (string item in responseList)
                {
                    if (item.Substring(3, 4) == this.SequenceNumber.ToString("X4"))
                    {
                        frame = item + '\r';
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                frame = null;
            }

            return frame;
        }
        DeviceStatus _deviceStatus = DeviceStatus.Ready;
        public DeviceStatus DeviceStatus
        {
            get
            {
                if (this._isOnline == true)
                {
                    string response = string.Empty;
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.DeviceStatus, "");

                        var retfm = this._chassis.Query(command, LONG_DELAY_MS);

                        //response = GetResponseFrame(retfm.Substring(7, 2));
                        response = retfm.Substring(7, 8);

                        _deviceStatus = (DeviceStatus)Convert.ToInt32(response, 16);
                    }
                }
                return _deviceStatus;
            }
        }

        public void Reset()
        {
            if (this._isOnline == true)
            {
                lock (this._chassis)
                {
                    var cmd = BuildCommand("RS", "", "");
                    this._chassis.Query(cmd, DELAY_MS);
                }
            }
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
         //   throw new NotImplementedException();
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
         //   throw new NotImplementedException();
        }

        //20190409温度保持函数之温度范围
        public double TemperatureDeviationDegreeC
        {
            set
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.TemperatureDeviation, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
        }

        //20190409温度保持函数之保持时间
        public int MinTimeInWindow_Sec
        {
            set
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.MinTimeInWindow, ConvertMath.ConvertFloatToIEEE754ReservedString((int)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
        }
        double _thermistor_LT_T = 0.0;
        /// <summary>
        ///4024 Upper Point:   Temperature
        ///4025 Upper Point:    Resistance
        ///4022 Middle Point:   Temperature
        ///4023 Middle Point:   Resistance 
        ///4020 Lower Point:    Temperature
        ///4021 Lower Point:    Resistance
        /// </summary>
        public double Thermistor_LT_T
        {
            set
            {
                _thermistor_LT_T = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.Obj_Lower_Point_Temperature, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.Obj_Lower_Point_Temperature, "");
                        string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                        _thermistor_LT_T = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                    }
                }
                return _thermistor_LT_T;
            }
        }
        double _thermistor_LT_R = 0.0;
        public double Thermistor_LT_R
        {
            set
            {
                _thermistor_LT_R = value; 
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.Obj_Lower_Point_Resistance, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.Obj_Lower_Point_Resistance, "");
                        string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                        _thermistor_LT_R = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                    }
                }return _thermistor_LT_R;
            }
        }
        double _thermistor_MT_T = 0.0;
        public double Thermistor_MT_T
        {
            set
            {
                _thermistor_MT_T = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.Obj_Middle_Point_Temperature, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.Obj_Middle_Point_Temperature, "");
                        string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                        _thermistor_MT_T = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                    }
                }
                return _thermistor_MT_T;
            }
        }
        double _thermistor_MT_R = 0.0;
        public double Thermistor_MT_R
        {
            set
            {
                _thermistor_MT_R = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.Obj_Middle_Point_Resistance, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.Obj_Middle_Point_Resistance, "");
                        string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                        _thermistor_MT_R = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                    }
                }return _thermistor_MT_R;
            }
        }
        double _thermistor_HT_T = 0.0;
        public double Thermistor_HT_T
        {
            set
            {
                _thermistor_HT_T = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.Obj_Upper_Point_Temperature, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.Obj_Upper_Point_Temperature, "");
                        string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                        _thermistor_HT_T = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                    }
                }
                return _thermistor_HT_T;
            }
        }
        double _thermistor_HT_R = 0.0;
        public double Thermistor_HT_R
        {
            set
            {
                _thermistor_HT_R = value;
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        this.TemperatureUnit = TemperatureUnit.Celsius;
                        string command = BuildCommand("VS", (int)MeerstetterTECCommands.Obj_Upper_Point_Resistance, ConvertMath.ConvertFloatToIEEE754ReservedString((float)value));
                        this._chassis.Query(command, DELAY_MS);
                    }
                }
            }
            get
            {
                if (this._isOnline == true)
                {
                    lock (this._chassis)
                    {
                        string command = BuildCommand("?VR", (int)MeerstetterTECCommands.Obj_Upper_Point_Resistance, "");
                        string response = this.GetResponseFrame(this._chassis.Query(command, DELAY_MS));
                        _thermistor_HT_R = ConvertMath.ConvertIEEE754ReservedStringToFloat(response.Substring(7, 8));
                    }
                }
                return _thermistor_HT_R;
            }
        }

        #endregion Meerstetter only

    }
}