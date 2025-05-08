using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jdsu.XEngine.Plugin.Instrument.Osas;

namespace Jdsu.XEngine.Plugin.Instrument
{
    public class Ag8614xOsa : ScpiOsa
    {
        public Ag8614xOsa(string name, string slot, string subSlot, IVisaChassis chassis)
            : base(name, slot, subSlot, chassis)
        {

        }

        public override void CheckIDN()
        {
            if (!this.Chassis.IsOnline) return;

            string idn = this.InstrumentIDN.ToString();

            if (!idn.Contains("AGILENT") && !idn.Contains("HP") && !idn.Contains("HEWLETT-PACKARD"))
            {
                throw new XEnginePluginInstrumentException(string.Format("Wrong IDN ({0}) doesn't contain 'Agilent' and 'HP' and 'HEWLETT-PACKARD'.",
                        idn), this.Chassis.ResourceName, this.Name);
            }

            if (!idn.Contains("8614"))
            {
                throw new XEnginePluginInstrumentException(string.Format("Wrong IDN ({0}) doesn't contain '8614'.",
                       idn), this.Chassis.ResourceName, this.Name);
            }
        }

        public override PowerWavelengthTrace GetOpticalSpectrumTrace(bool triggerNewSweep)
        {
            if (this.Chassis.IsOnline == false)
            {
                return null;
            }

            if (triggerNewSweep)
            {
                this.TriggerSweep();
            }

            PowerWavelengthTrace trace = new PowerWavelengthTrace();
            this.Chassis.Write(":FORMAT:DATA REAL,+32");
            string command = "TRACE:DATA:Y? TRA";
            this.Chassis.Write(command);
            double[] yArray = this.ReadTraceArray(4);
            double xStart_nm = this.StartWavelength_nm;
            double xStop_nm = this.StopWavelength_nm;
            int size = yArray.Length;
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

        private double[] ReadTraceArray(int bytesPerNumber)
        {
            string s = System.Text.Encoding.ASCII.GetString(this.Chassis.ReadByteArray(2)); // "4#"
            int byteLen = Convert.ToInt32(s.Substring(1, 1));                            // ByteLen = 4
            s = System.Text.Encoding.ASCII.GetString(this.Chassis.ReadByteArray(byteLen)); // s = "4004" for 1001 double samples
            int totalNumBytes = Convert.ToInt32(s);
            dynamic bytes = new List<byte>();
            while (!(bytes.Count == totalNumBytes)) 
            { 
                bytes.AddRange(this.Chassis.ReadByteArray(Math.Min(1000000, totalNumBytes - bytes.Count)));
            }
            this.Chassis.ReadString();
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

        public override bool IsTraceLengthAuto
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

      

        public override double ReadWavelengthAtPeak_nm()
        {
            throw new NotImplementedException();
        }



        public override OsaSmsrResults GetSmsrResults()
        {
            if (this.Chassis.IsOnline == false) return default(OsaSmsrResults);
            throw new NotImplementedException();
        }

        public bool IsSensitivityAuto
        {
            get
            {
                if (this.Chassis.IsOnline == false)
                {
                    return false;
                }
                string command = ":sens:pow:dc:range:low:auto?";
                int response = this.Chassis.QueryInteger(command);

                if (response == 1)
                {
                    return true;
                }
                else if (response == 0)
                {
                    return false;
                }

                throw new XEnginePluginInstrumentException("Unknow response: " + response,
                            this.Chassis.ResourceName,
                            this.Name);
                
            }
        }
    }
}
