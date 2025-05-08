using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    /// <summary>
    /// A base OSA instrument driver using SCPI commands.
    /// </summary>
    public class ScpiOsa : InstrumentBase, IInstrumentBase
    {
        const int Delay_ms = 60 * 1000;
        /// <summary>
        /// Initializes an instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="slot"></param>
        /// <param name="subSlot"></param>
        /// <param name="chassis"></param>
        public ScpiOsa(string name, string address, IInstrumentChassis chassis)
         : base(name, address, chassis)
        {
            //this.VisaChassis = chassis;
        }

        private string _instrumentIdn;
        /// <summary>
        /// Gets IDN of the instrument.
        /// </summary>
        public string InstrumentIDN
        {
            get
            {

                if (_instrumentIdn == null)
                {
                    _instrumentIdn = this._chassis.Query("*IDN?", Delay_ms);
                }

                return _instrumentIdn;
            }
        }
        /// <summary>
        /// Gets or sets whether power range is auto.
        /// </summary>
        public bool IsPowerRangeAuto
        {
            set
            {
                this._chassis.TryWrite("SENS:POW:RANG:AUTO " + (value ? "1" : "0"));

            }
            get
            {
                if (this._chassis.IsOnline == false) return false;
                string response = this._chassis.Query("SENS:POW:RANG:AUTO?", Delay_ms);
                response = response.Trim();
                bool powerRangeIsAuto;
                if (response.StartsWith("1"))
                {
                    powerRangeIsAuto = true;
                }
                else if (response.StartsWith("0"))
                {
                    powerRangeIsAuto = false;
                }
                else
                {
                    throw new Exception();
                }
                return powerRangeIsAuto;
            }
        }

        /// <summary>
        /// Gets or sets whether sweep time is auto.
        /// </summary>
        public bool IsSweepTimeAuto
        {
            set
            {
                this._chassis.TryWrite("SENS:SWE:TIME:AUTO " + (value ? "1" : "0"));

            }
            get
            {
                if (this._chassis.IsOnline == false) return false;
                string response = this._chassis.Query("SENS:SWE:TIME:AUTO?", Delay_ms);
                bool sweepTimeIsAuto;
                if (response.StartsWith("1"))
                {
                    sweepTimeIsAuto = true;
                }
                else if (response.StartsWith("0"))
                {
                    sweepTimeIsAuto = false;
                }
                else
                {
                    throw new Exception();
                }
                return sweepTimeIsAuto;
            }
        }

        /// <summary>
        /// Gets or sets peak excursion in dB.
        /// </summary>
        public double PeakExcursion_dB
        {
            set
            {

                this._chassis.TryWrite("CALC:MARK:PEXC " + value);

            }
            get
            {
                if (this._chassis.IsOnline == false) return 0.0;
                string command = "CALC:MARK:PEXC?";
                string response = this._chassis.Query(command, Delay_ms);
                return Convert.ToDouble(response);
            }
        }

        /// <summary>
        /// Gets or sets reference level in dBm.
        /// </summary>
        public double ReferenceLevel_dBm
        {
            set
            {
                this._chassis.TryWrite("DISP:TRAC:Y:RLEV " + value.ToString() + "DBM");
            }
            get
            {
                if (this._chassis.IsOnline == false) return 0.0;
                this.PowerUnit = PowerUnit.dBm;
                string command = "DISP:TRAC:Y:RLEV?";
                string response = this._chassis.Query(command, Delay_ms);
                double referenceLevelValue = Convert.ToDouble(response);
                return referenceLevelValue;
            }
        }
        /// <summary>
        /// Gets or sets resoltuion bandwidth in nm.
        /// </summary>
        public double ResolutionBandwidth_nm
        {
            set
            {
                //sens:bwid:res 0.1nm
                this._chassis.TryWrite("SENS:BWID:RES " + value.ToString() + "nm");

            }
            get
            {
                if (this._chassis.IsOnline == false) return 0.0;
                string command = "SENS:BWID:RES?";
                string response = this._chassis.Query(command, Delay_ms);
                double resolutionBandwidthValue = Convert.ToDouble(response) * 1e9;
                return resolutionBandwidthValue;
            }
        }
        //public YokogawaAQ6370SensitivityModes Sensitivity
        //{
        //    get
        //    {
        //        string command = "SENS:SENS?";
        //        string response = this._chassis.Query(command, Delay_ms);

        //        YokogawaAQ6370SensitivityModes sensitivity = (YokogawaAQ6370SensitivityModes)Enum.Parse(typeof(YokogawaAQ6370SensitivityModes), response);
        //        return sensitivity;
        //    }
        //    set
        //    {
        //        string command = "SENS:SENS " + (byte)value;
        //        this._chassis.TryWrite(command);
        //    }
        //}
        public string Sensitivity_dBm
        {
            set
            {
                if (this._chassis.IsOnline == false) return;

                if (value.ToUpper().Contains("AUTO"))
                {
                    this._chassis.TryWrite("SENS:POW:DC:RANGE:LOW:AUTO 1");
                }
                else
                {
                    this._chassis.TryWrite("SENS:POW:DC:RANGE:LOW:AUTO 0");
                    this._chassis.TryWrite("SENS:POW:DC:RANGE:LOW " + value.ToString() + "dBm");
                }
            }
            get
            {
                if (this._chassis.IsOnline == false) return "AUTO";

                string command = "SENS:POW:DC:RANGE:LOW:AUTO?";
                string response = this._chassis.Query(command, Delay_ms);

                int auto = 0;
                int.TryParse(response, out auto);
                if(auto==1)
                {
                    return "AUTO";
                }
                else
                {
                    command = "SENS:POW:DC:RANGE:LOW?";
                    response = this._chassis.Query(command, Delay_ms);
                    return (Convert.ToDouble(response) * 1e9).ToString();

                }


            }
        }

        public PowerUnit PowerUnit
        {
            private get
            {
                throw new NotImplementedException();
            }
            set
            {
                string powerUnit = "DBM";
                switch (value)
                {
                    case PowerUnit.W:
                        powerUnit = "W";
                        break;
                    case PowerUnit.dBm:
                        break;
                    default:
                        return;
                }
                string command = "UNIT:POW " + powerUnit;
                this._chassis.TryWrite(command);
            }
        }

        public void TriggerSweep()
        {
            if (this._chassis.IsOnline == false) return;
            this._chassis.TryWrite("INIT:IMM");
            Thread.Sleep(100);
            this._chassis.Query("*OPC?", Delay_ms);
        }

        public double GetSmsr_dB()
        {
            this._chassis.TryWrite("CALC:MARK1:MAX");

            double peakdB = double.Parse(this._chassis.Query("CALC:MARK1:Y?", Delay_ms));
            this._chassis.TryWrite("CALC:MARK1:MAX:NEXT");
            double nextPeakdB = double.Parse(this._chassis.Query("CALC:MARK1:Y?", Delay_ms));
            double retVal = peakdB - nextPeakdB;
            return retVal;
        }
        /// <summary>
        /// Gets or sets start wavelength in nm.
        /// </summary>
        public double StartWavelength_nm
        {
            get
            {
                if (this._chassis.IsOnline == false) return 0.0;
                string command = "SENS:WAV:STAR?";
                string resoponse = this._chassis.Query(command, Delay_ms);
                double startWavelength_nm = Convert.ToDouble(resoponse) * 1e9;
                return startWavelength_nm;
            }
            set
            {

                string command = "SENS:WAV:STAR " + value.ToString() + "nm";
                this._chassis.TryWrite(command);
            }
        }
        /// <summary>
        /// Gets or sets stop wavelength in nm.
        /// </summary>
        public double StopWavelength_nm
        {
            get
            {
                if (this._chassis.IsOnline == false) return 0.0;
                string command = "SENS:WAV:STOP?";
                string resoponse = this._chassis.Query(command, Delay_ms);
                double stopWavelength_nm = Convert.ToDouble(resoponse) * 1e9;
                return stopWavelength_nm;
            }
            set
            {
                string command = "SENS:WAV:STOP " + value.ToString() + "nm";
                this._chassis.TryWrite(command);
            }
        }
        /// <summary>
        /// Gets or sets trace length.
        /// </summary>
        public int TraceLength
        {
            set
            {
                string command = "SENS:SWE:POIN " + value;
                this._chassis.TryWrite(command);
            }
            get
            {
                if (this._chassis.IsOnline == false) return 0;
                string command = "SENS:SWE:POIN?";
                int retVal = Convert.ToInt16(this._chassis.Query(command, Delay_ms).Trim());
                return retVal;
            }
        }
        /// <summary>
        /// Gets or sets center wavelength in nm.
        /// </summary>
        public double CenterWavelength_nm
        {
            set
            {
                this._chassis.TryWrite("SENS:WAV:CENT " + value.ToString() + "nm");
            }

            get
            {
                if (this._chassis.IsOnline == false) return 0.0;
                string resoponse = this._chassis.Query("SENS:WAV:CENT?", Delay_ms);
                return Convert.ToDouble(resoponse) * 1e9;
            }
        }
        /// <summary>
        /// Gets or sets wavelength span in nm.
        /// </summary>
        public double WavelengthSpan_nm
        {
            set
            {
                this._chassis.TryWrite("SENS:WAV:SPAN " + value.ToString() + "nm");
            }
            get
            {
                if (this._chassis.IsOnline == false) return 0.0;
                string resoponse = this._chassis.Query("SENS:WAV:SPAN?", Delay_ms);
                double wavelengthSpanValue = Convert.ToDouble(resoponse) * 1e9;
                return wavelengthSpanValue;
            }
        }

        public void Reset()
        {
            this._chassis.TryWrite("*RST");
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public bool IsSweepContinuous
        {
            get
            {
                string command = "INIT:CONT?";
                string response = this._chassis.Query(command, Delay_ms);
                switch (response)
                {
                    case "1":
                        return true;
                    case "0":
                        return false;
                    default:
                        return false;
                }
            }
            set
            {
                string command = ":INIT:CONT ";
                if (value)
                {
                    command += "1";
                }
                else
                {
                    command += "0";
                }
                this._chassis.TryWrite(command);
            }
        }

        public PowerWavelengthTrace GetOpticalSpectrumTrace(bool triggerNewSweep)
        {
            if (this._chassis.IsOnline == false)
            {
                return null;
            }

            if (triggerNewSweep)
            {
                this.TriggerSweep();
            }

            _chassis.DefaultBufferSize = 1024*128;
            PowerWavelengthTrace trace = new PowerWavelengthTrace();
            //this._chassis.TryWrite(":FORMAT:DATA REAL,+32");
            string command = "TRACE:DATA:Y? TRA\n";
            string response = this._chassis.Query(command, Delay_ms);
            var RES = response.Split(',');
            List<double> yArray = new List<double>();

            for (int i = 0; i < RES.Length; i++)
            {
                if (RES[i].Contains("E"))
                {
                    yArray.Add(Convert.ToDouble(Convert.ToDecimal(Decimal.Parse(RES[i], System.Globalization.NumberStyles.Float))));
                }
            }
            trace.StartWavelength_nm = this.StartWavelength_nm;
            trace.StopWavelength_nm = this.StopWavelength_nm;
            double xStart_nm = this.StartWavelength_nm;
            double xStop_nm = this.StopWavelength_nm;
            int size = yArray.Count;
            double step = (xStop_nm - xStart_nm) / (size - 1);

            for (int i = 0; i < size; i++)
            {
                PowerWavelength pw = new PowerWavelength()
                {
                    Power_dBm = yArray[i],
                    Wavelength_nm = xStart_nm + step * i
                };
                trace.Add(pw);
            }

            return trace;
        }

        public double GetSMSR()
        {
            if (this._chassis.IsOnline == false)
            {
                return double.NaN;
            }
            this._chassis.TryWrite(":CALC:MARK1:MAX");
            Thread.Sleep(100);
            string command = ":CALC:MARK1:Y?";
            string Peakamp = this._chassis.Query(command, Delay_ms);

            this._chassis.TryWrite(":CALC:MARK1:MAX:NEXT");
            string NextPeakamp = this._chassis.Query(command, Delay_ms);
            double Peak = double.Parse(Peakamp);
            double NextPeak = double.Parse(NextPeakamp);


            return Peak - NextPeak;
        }

        public double ReadWavelengthAtPeak_nm()
        {
            string command = "CALC:MARK:MAX;:CALC:MARK:X? 0";
            string response = this._chassis.Query(command, Delay_ms);
            double peakWavelength_nm = Convert.ToDouble(response) * 1e9;
            return peakWavelength_nm;
        }
        public double ReadPowerAtPeak_dbm()
        {
            string command = "CALC:MARK:MAX;:CALC:MARK:Y? 0";
            string response = this._chassis.Query(command, Delay_ms);
            double peakPower_dbm = Convert.ToDouble(response);
            return peakPower_dbm;
        }
        private double[] ReadTraceArray(int bytesPerNumber)
        {
            string s = System.Text.Encoding.ASCII.GetString(((NiVisaChassis)this._chassis).ReadByteArray(2)); // "4#"
            int byteLen = Convert.ToInt32(s.Substring(1, 1));                            // ByteLen = 4
            s = System.Text.Encoding.ASCII.GetString(((NiVisaChassis)this._chassis).ReadByteArray(byteLen)); // s = "4004" for 1001 double samples
            int totalNumBytes = Convert.ToInt32(s);
            dynamic bytes = new List<byte>();
            while (!(bytes.Count == totalNumBytes))
            {
                bytes.AddRange(((NiVisaChassis)this._chassis).ReadByteArray(Math.Min(1000000, totalNumBytes - bytes.Count)));
            }
            ((NiVisaChassis)this._chassis).ReadString();
            byte[] buffer = bytes.ToArray();

            byte[] numByte = new byte[bytesPerNumber];
            int ii = 0;
            int numberOfSamples = buffer.Length / bytesPerNumber;
            double[] v = new double[numberOfSamples];
            for (ii = 0; ii <= numberOfSamples - 1; ii++)
            {
                for (int jj = 0; jj <= bytesPerNumber - 1; jj++)
                {
                    numByte[jj] = buffer[bytesPerNumber * ii + bytesPerNumber - jj - 1];
                }
                if (bytesPerNumber == 4)
                {
                    v[ii] = BitConverter.ToSingle(numByte, 0);
                }
                else
                {
                    v[ii] = BitConverter.ToDouble(numByte, 0);
                }
            }
            return v;
        }
    }
}
