using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NationalInstruments.VisaNS;
using System.Diagnostics;

namespace SolveWare_BurnInInstruments
{
    /// <summary>
    /// A driver for Yokogawa AQ67370x control using SCPI commands.
    /// </summary>
    public class OSA_AG86142B : InstrumentBase, IInstrumentBase, IOSA
    {
        const int Delay_ms = 60 * 1000;
        public OSA_AG86142B(string name, string address, IInstrumentChassis chassis)
         : base(name, address, chassis)
        {


        }
        public override void RefreshDataOnceCycle(CancellationToken token)
        {
           // throw new NotImplementedException();
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
           // throw new NotImplementedException();
        }


        public PowerWavelengthTrace GetOpticalSpectrumTrace(bool triggerNewSweep, OsaTrace traceType)
        {
            //const int timeout_ms = 10 * 1000;
            //const int bufferSize = 131072;
            const int timeout_ms = 30 * 1000;
            const int bufferSize = 10 * 131072;

            _chassis.Timeout_ms = timeout_ms;

            if (triggerNewSweep)
            {
                Thread.Sleep(200);
                TriggerSweep();
            }
            Thread.Sleep(10);


            int bufferSizeAtStart = this._chassis.DefaultBufferSize;
            this._chassis.DefaultBufferSize = bufferSize;
            Thread.Sleep(10);
            PowerWavelengthTrace traceData = new PowerWavelengthTrace();
            string[] wavelengthStringArray = getWavelengthTraceData(traceType).Split(',');
            Thread.Sleep(10);
            string[] powerStringArray = getAmplitudeTraceData(traceType).Split(',');
            if (wavelengthStringArray.Length != powerStringArray.Length)
            {
                throw new NotSupportedException("The numbers of Wavelength and Power are not same.");
            }

            for (int i = 0; i < wavelengthStringArray.Length; i++)
            {
                PowerWavelength item = new PowerWavelength();
                item.Power_dBm = Math.Round(double.Parse(powerStringArray[i]), 3);
                item.Wavelength_nm = Math.Round(double.Parse(wavelengthStringArray[i]) * 1e9, 3);
                traceData.Add(item);
            }
            Thread.Sleep(1);
            this._chassis.DefaultBufferSize = bufferSizeAtStart;

            traceData.StartWavelength_nm = traceData.GetWavelengthes().Min();
            traceData.StopWavelength_nm = traceData.GetWavelengthes().Max();
            return traceData;
        }

        public string getWavelengthTraceData(OsaTrace trace)
        {
            string command = ":TRAC:X? " + trace.ToString();

            return this._chassis.Query(command, Delay_ms);

        }

        public string getAmplitudeTraceData(OsaTrace trace)
        {
            string command = ":TRAC:Y? " + trace.ToString();
            return this._chassis.Query(command, Delay_ms);

        }




        public bool IsTraceLengthAuto
        {
            get
            {
                string command = ":SENSe:SWEep:POINts:AUTO?";
                string data = this._chassis.Query(command, Delay_ms).Trim();
                if (data.StartsWith("1"))
                {
                    return true;
                }
                else if (data.StartsWith("0"))
                {
                    return false;
                }

                throw new NotSupportedException(string.Format("Wrong return data '{0}'.", data));
            }
            set
            {
                string command = ":SENSe:SWEep:POINts:AUTO " + (value ? "1" : "0");
                this._chassis.TryWrite(command);
            }
        }



        public PowerUnit PowerUnit
        {
            set
            {
                string command = "DISP:WIND:TRAC:Y1:SCAL:UNIT ";
                string subCommand = "0";
                switch (value)
                {
                    case PowerUnit.dBm:
                        subCommand = "0";
                        break;
                    case PowerUnit.W:
                        subCommand = "1";
                        break;
                    case PowerUnit.dBmPerNm:
                        subCommand = "2";
                        break;
                    case PowerUnit.WPerNm:
                        subCommand = "3";
                        break;
                    default:
                        throw new NotSupportedException("Wrong power unit: " + PowerUnit.ToString());
                }
                command += subCommand;
                this._chassis.TryWrite(command);
            }
            get
            {
                string response = this._chassis.Query("DISP:WIND:TRAC:Y1:SCAL:UNIT?", Delay_ms);

                PowerUnit powerUnits = PowerUnit.mW;
                switch (response)
                {
                    case "0":
                        powerUnits = PowerUnit.dBm;
                        break;
                    case "1":
                        powerUnits = PowerUnit.W;
                        break;
                    case "2":
                        powerUnits = PowerUnit.dBmPerNm;
                        break;
                    case "3":
                        powerUnits = PowerUnit.WPerNm;
                        break;
                    default:
                        throw new NotSupportedException("The " + this.GetType().Name + " with the name has Power units set to:" + powerUnits + " this is unknown");
                }

                return powerUnits;

            }
        }

        public double ReadWavelengthAtPeak_nm()
        {
            string command = ":CALC:MARK:MAX;:CALC:MARK:X? 0";
            string response = this._chassis.Query(command, Delay_ms);
            double peakWavelength_nm = Convert.ToDouble(response) * 1e9;
            return peakWavelength_nm;
        }
        public double ReadPowerAtPeak_dbm()
        {
            string command = ":CALC:MARK:MAX;:CALC:MARK:Y? 0";
            string response = this._chassis.Query(command, Delay_ms);
            double peakPower_dbm = Convert.ToDouble(response);
            return peakPower_dbm;
        }
        public double ReadSpectrumWidth_nm(float db)
        {

            this._chassis.TryWrite(":calc:category swth");
            this._chassis.TryWrite(":CALCULATE:PARAMETER:SWTHRESH:K 1.0");  //20190708 测试结果的频谱宽度的倍数,这里需要固定为1
            this._chassis.TryWrite(":CALCULATE:PARAMETER:SWTHRESH:TH " + db.ToString("0.00"));  //20190703 修改程序BUG 增加新输入参数
            this._chassis.TryWrite(":calc");
            string response = this._chassis.Query(":calc:data?", Delay_ms);

            double spectrumWidth_nm = Convert.ToDouble(response.Split(',')[1]) * 1e9;
            return spectrumWidth_nm;
        }


        public double ResolutionBandwidth_nm
        {
            set
            {
                //20190709 这个指令有错误,修改正确
                string command = ":SENS:BAND:RES " + (value / 1E9).ToString() + "M";
                //string command = ":SENS:BAND:RES " + value.ToString() + "nm";
                this._chassis.TryWrite(command);
            }

            get
            {
                string response = this._chassis.Query(":SENS:BAND:RES?", Delay_ms);

                double resolutionBandwidthValue = Convert.ToDouble(response) * 1e9;
                return resolutionBandwidthValue;
            }

        }


        public YokogawaAQ6370SensitivityModes Sensitivity
        {
            get
            {
                string command = ":SENS:SENS?";
                string response = this._chassis.Query(command, Delay_ms);

                YokogawaAQ6370SensitivityModes sensitivity = (YokogawaAQ6370SensitivityModes)Enum.Parse(typeof(YokogawaAQ6370SensitivityModes), response);
                return sensitivity;
            }
            set
            {
                string command = ":SENS:SENS " + (byte)value;
                this._chassis.TryWrite(command);
            }
        }
        public double GetSmsr_dB()
        {
            this._chassis.TryWrite(":calc:category smsr");
            this._chassis.TryWrite(":CALCULATE:PARAMETER:SMSR SMSR1");
            this._chassis.TryWrite(":calc");
            string response = this._chassis.Query(":calc:data?", Delay_ms);
            return Convert.ToDouble(response.Split(',')[5]);
        }
        public double GetPeakWavelength_nm() 
        {
            this._chassis.TryWrite(":calc:category smsr");
            this._chassis.TryWrite(":CALCULATE:PARAMETER:SMSR SMSR1");
            //this._chassis.TryWrite(string.Format(":CALCULATE:PARAMETER:SMSR:MASK {0:00}nm", SMSRMask_nm));
            this._chassis.TryWrite(":calc");
            string response = this._chassis.Query(":calc:data?", Delay_ms);
            string[] dataList = response.Split(',');
           // return Convert.ToDouble(response[0]) * 1e9;
            return Convert.ToDouble(dataList[0]) * 1e9;
            //别家,备用
            //lock (this.Chassis)
            //{
            //    this.Chassis.Write(":calc:category SMSR");
            //    this.Chassis.Write(":CALCULATE:PARAMETER:SMSR MODE SMSR1");
            //    this.Chassis.Write(string.Format(":CALCULATE:PARAMETER:SMSR:MASK {0:00}nm", SMSRMask_nm));
            //    this.Chassis.Write(":calc");
            //    string response = this.Chassis.Query(":calc:data?");
            //    string[] dataList = response.Split(',');


            //    DFBLDData.PeakWavelength_nm = Convert.ToDouble(dataList[0]) * 1e9;
            //    DFBLDData.PeakPower_dBm = Convert.ToDouble(dataList[1]);
            //    DFBLDData.SecondPeakWavelength_nm = Convert.ToDouble(dataList[2]) * 1e9;
            //    DFBLDData.SecondPeakPower_dBm = Convert.ToDouble(dataList[3]);
            //    //var deltaWL = Convert.ToDouble(dataList[4]) * 1e9;
            //    DFBLDData.SMSR_dB = Convert.ToDouble(dataList[5]);
            //}
        }
        public string GetRawSmsr_dB()
        {
            this._chassis.TryWrite(":calc:category smsr");
            this._chassis.TryWrite(":CALCULATE:PARAMETER:SMSR SMSR1");
            this._chassis.TryWrite(":calc");
            return this._chassis.Query(":calc:data?", Delay_ms);

        }

        /// <summary>
        /// Triggers sweep and gets SMSR values etc.
        /// </summary>
        /// <returns></returns>
        public OsaSmsrResults GetSmsrResults()
        {
            //Find Peak
            int errorCount = 0;
            OsaSmsrResults smsrPoint = new OsaSmsrResults();
            string[] data = null;
            string smsr = null;
            string delta = null;
            while (true)
            {
                string command = ":CALC:PAR:DFBL SMSR,SMOD,SMSR1;:CALC:DATA?";
                string response;
                try
                {
                    ////TriggerSweep();//don't trigger sweep.
                    response = this._chassis.Query(command, Delay_ms);
                }
                catch
                {
                    Thread.Sleep(1000);
                    TriggerSweep();
                    Thread.Sleep(20);
                    response = this._chassis.Query(command, 800);

                }
                data = response.Split(new char[] { ',' });

                try
                {
                    smsr = data[4];
                    delta = data[3];
                }
                catch (Exception ex)
                {
                    if (errorCount >= 3)
                    {
                        throw new NotSupportedException("Response error: " + response, ex);
                    }
                    errorCount++;
                    Thread.Sleep(1000);
                    continue;
                }
                break;
            }


            smsrPoint.Smsr_dB = Math.Round(double.Parse(smsr), 4);
            smsrPoint.SideModeFrequencyOffset_nm = Math.Round(double.Parse(delta) * 1e9, 4);
            return smsrPoint;

        }

        /// <summary>
        /// Checks IDN of the instrument.
        /// </summary>
        public void CheckIDN()
        {
            string idn = this.InstrumentIDN;
            string myId = "YOKOGAWA,AQ6370";
            if (!idn.StartsWith(myId))
            {
                throw new NotSupportedException(string.Format("The IDN '{0}' from instrument doesn't contains '{1}'.",
                        idn, myId));
            }
        }


        #region Yokogawa only
        /// <summary>
        /// Gets or sets sweep mode.
        /// </summary>
        public YokogawaAQ6370SweepModes SweepMode
        {
            set
            {

                string command = ":INIT:SMOD " + (int)value;
                try
                {

                    this._chassis.TryWrite(command);

                    Thread.Sleep(200);
                    this._chassis.TryWrite(":INIT");
                    Thread.Sleep(500);
                    //this._chassis.Query("*OPC?", Delay_ms);
                    this._chassis.Query("*OPC?", 2000);
                }
                catch
                {
                    try
                    {
                        this._chassis.TryWrite(command);
                        Thread.Sleep(1000);
                        this._chassis.TryWrite(":INIT");
                        Thread.Sleep(1000);
                        this._chassis.Query("*OPC?", 2000);
                    }
                    catch (Exception ex)
                    {
                        throw new NotSupportedException(ex.Message);
                    }
                }
            }
            get
            {
                string command = ":INIT:SMOD?";
                string response = this._chassis.Query(command, Delay_ms);

                YokogawaAQ6370SweepModes sweepMode = (YokogawaAQ6370SweepModes)Enum.Parse(typeof(YokogawaAQ6370SweepModes), response);
                return sweepMode;
            }
        }

        /// <summary>
        /// Gets or sets whether resolution bandwidth corrrention is on.
        /// </summary>
        public bool IsResolutionBandwidthCorrectionOn
        {
            get
            {
                string response = this._chassis.Query(":SENS:SETT:CORR?", Delay_ms).Trim();

                if (response == "1")
                {
                    return true;
                }
                else if (response == "0")
                {
                    return false;
                }
                else
                {
                    throw new NotSupportedException("response to command '::SENS:SETT:CORR?' is not 0 or 1");
                }
            }
            set
            {
                string command;
                if (value == true)
                {
                    command = ":SENS:SETT:CORR 1";
                }
                else
                {
                    command = ":SENS:SETT:CORR 0";
                }
                this._chassis.TryWrite(command);
                Thread.Sleep(20);
                if (value != IsResolutionBandwidthCorrectionOn)
                {
                    throw new NotSupportedException("ResolutionBandwidthCorrectionIsOn didn't set properly");
                }
            }
        }

        /// <summary>
        /// Gets or sets the mask range during
        ///level measurement applied to noise level
        ///measurements made by the WDM analysis
        ///function.
        /// </summary>
        public double NoiseFittingMaskArea_nm
        {
            get
            {
                string command = ":CALC:PAR:WDM:MAR?";
                string response = this._chassis.Query(command, Delay_ms);

                double noiseFittingMaskArea_nm = Convert.ToDouble(response) * 1e9;
                noiseFittingMaskArea_nm = Math.Round(noiseFittingMaskArea_nm, 4);
                return noiseFittingMaskArea_nm;
            }
            set
            {
                string command = ":CALC:PAR:WDM:MAR " + value * 1e-9;
                this._chassis.TryWrite(command);
            }
        }

        /// <summary>
        /// Gets or sets the measuring range applied to
        ///noise level measurements made by the WDM
        ///analysis function.
        /// </summary>
        public double NoiseFittingArea_nm
        {
            get
            {
                string command = ":CALC:PAR:WDM:NAR?";
                string response = this._chassis.Query(command, Delay_ms);

                double noiseFittingArea_nm = Convert.ToDouble(response) * 1e9;
                noiseFittingArea_nm = Math.Round(noiseFittingArea_nm, 4);
                return noiseFittingArea_nm;
            }
            set
            {
                string command = ":CALC:PAR:WDM:NAR " + value * 1e-9;
                this._chassis.TryWrite(command);
            }
        }
        /// <summary>
        /// Sets/queries the noise bandwidth for the WDM 
        /// analysis function.
        /// </summary>
        public double NoiseBandwidth_nm
        {
            get
            {
                string command = ":CALCULATE:PARAMETER:WDM:NBW?";
                string response = this._chassis.Query(command, Delay_ms);
                double value_nm = Convert.ToDouble(response) * 1e9;
                value_nm = Math.Round(value_nm, 4);
                return value_nm;
            }
            set
            {
                string command = ":CALCULATE:PARAMETER:WDM:NBW " + value * 1e-9;
                this._chassis.TryWrite(command);
            }
        }
        /// <summary>
        /// Sets/queries the channel mask threshold level
        /// for the WDM analysis function.
        /// <para></para>
        /// To turn off the channel mask function, set the
        /// threshold level to –999.
        /// </summary>
        public double MaskThresholdLevel_dB
        {
            get
            {
                string command = ":CALCULATE:PARAMETER:WDM:DMASK?";
                string response = this._chassis.Query(command, Delay_ms);
                double value_nm = Convert.ToDouble(response);
                return value_nm;
            }
            set
            {
                string command = ":CALCULATE:PARAMETER:WDM:DMASK " + value;
                this._chassis.TryWrite(command);
            }
        }
        /// <summary>
        /// Sets/queries the peak bottom difference
        ///    of channel detection for the WDM analysis
        ///    function.
        /// </summary>
        public double ModeDifference_dB
        {
            get
            {
                string command = ":CALCULATE:PARAMETER:WDM:MDIFF?";
                string response = this._chassis.Query(command, Delay_ms);
                double value_nm = Convert.ToDouble(response);
                return value_nm;
            }
            set
            {
                string command = ":CALCULATE:PARAMETER:WDM:MDIFF " + value.ToString() + "DB";
                this._chassis.TryWrite(command);
            }
        }

        /// <summary>
        /// Gets or sets the measurement algorithm
        ///applied to noise level measurements made by
        ///the WDM analysis function.
        /// </summary>
        public YokogawaAQ6370NoiseAlgorithms NoiseAlgorithm
        {
            get
            {
                string command = ":CALC:PAR:WDM:NALG?";
                string response = this._chassis.Query(command, Delay_ms);
                YokogawaAQ6370NoiseAlgorithms noiseAlgorithms = (YokogawaAQ6370NoiseAlgorithms)Enum.Parse(typeof(YokogawaAQ6370NoiseAlgorithms), response);
                return noiseAlgorithms;
            }
            set
            {
                string command = ":CALC:PAR:WDM:NALG " + value.ToString();
                this._chassis.TryWrite(command);
            }
        }

        /// <summary>
        /// Gets or sets the fitting function during
        ///level measurement applied to noise level
        ///measurements made by the WDM analysis
        ///function.
        /// </summary>
        public YokogawaAQ6370NoiseFittingAlgorithms NoiseFittingAlgorithms
        {
            get
            {
                string command = ":CALC:PAR:WDM:FALG?";
                string response = this._chassis.Query(command, Delay_ms);
                YokogawaAQ6370NoiseFittingAlgorithms noiseFittingAlgorithms = (YokogawaAQ6370NoiseFittingAlgorithms)Enum.Parse(typeof(YokogawaAQ6370NoiseFittingAlgorithms), response);
                return noiseFittingAlgorithms;
            }
            set
            {
                string command = ":CALC:PAR:WDM:FALG " + (int)value;
                this._chassis.TryWrite(command);
            }
        }

        /// <summary>
        /// Gets or sets whether to enable the auto offset
        ///function of the level.
        /// </summary>
        public bool CalibrationAutoZeroEnable
        {
            get
            {
                string command = ":CAL:ZERO?";
                string response = this._chassis.Query(command, Delay_ms);
                if (int.Parse(response) == 0)
                {
                    return false;
                }
                return true;

            }
            set
            {
                string command = ":CAL:ZERO ";
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


        /// <summary>
        /// Read OSNR value in dB but don't trigger sweep.
        /// </summary>
        /// <returns></returns>
        public double GetOsnr_dB()
        {
            double osnr = double.NaN;
            string command = ":CALCulate:CATegory WDM;:CALCulate;:CALCulate:DATA:CSNR?";

            string response = this._chassis.Query(command, Delay_ms);

            bool result = double.TryParse(response, out osnr);
            if (!result)
            {
                throw new NotSupportedException("Bad return from OSNR reading: " + response);
            }
            return osnr;
        }

        #endregion

        #region internal types
        //private enum OsaTrace { TRA, TRB, TRC, TRD, TRE, TRF, TRG }

        #endregion
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
                this._chassis.TryWrite(":SENS:POW:RANG:AUTO " + (value ? "1" : "0"));

            }
            get
            {
                string response = this._chassis.Query(":SENS:POW:RANG:AUTO?", Delay_ms);
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
                    throw new NotSupportedException("response 0 or 1 expected");
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
                this._chassis.TryWrite(":SENS:SWE:TIME:AUTO " + (value ? "1" : "0"));

            }
            get
            {
                string response = this._chassis.Query(":SENS:SWE:TIME:AUTO?", Delay_ms).Trim(); ;

                bool sweepTimeIsAuto = false;
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
                    throw new NotSupportedException("response to command ':SENS:SWE:TIME:AUTO?' is not 0 or 1");
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

                this._chassis.TryWrite(":CALC:MARK:PEXC " + value);
            }
            get
            {

                string response = this._chassis.Query(":CALC:MARK:PEXC?", Delay_ms).Trim(); ;
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
                this._chassis.TryWrite(":DISP:TRAC:Y:RLEV " + value.ToString() + "DBM");
            }
            get
            {
                this.PowerUnit = PowerUnit.dBm;

                string response = this._chassis.Query(":DISP:TRAC:Y:RLEV?", Delay_ms);
                double referenceLevelValue = Convert.ToDouble(response);
                return referenceLevelValue;
            }
        }

        public double Sensitivity_dBm
        {
            get
            {

                string command = ":SENS:POW:DC:RANGE:LOW?";
                string response = this._chassis.Query(command, Delay_ms);
                return Convert.ToDouble(response);
            }
            set
            {
                this._chassis.TryWrite(":SENS:POW:DC:RANGE:LOW " + value.ToString() + "dBm");
            }
        }


        public void TriggerSweep()
        {
            //this.SweepMode = YokogawaAQ6370SweepModes.Single;
            //this._chassis.TryWrite(":INIT:IMM");
            //Thread.Sleep(100);
            //this._chassis.Query("*OPC?", 2000);

            
            //this.Chassis.Write(":INIT:IMM");
            //this._chassis.Query(":INIT:IMM", Delay_ms);
            this._chassis.TryWrite(":INIT:IMM");
            Thread.Sleep(100);
            //Socket 需要循环查询等待
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string rtn = "";
            while (true)
            {
                if (sw.ElapsedMilliseconds > 5000)
                {
                    break;
                }

                try
                {
                    //rtn = this.Chassis.Query("*OPC?");
                    // rtn = this._chassis.Query("*OPC?", Delay_ms);
                    //rtn = this._chassis.Query("*OPC?", 10000);
                    rtn = this._chassis.Query("*OPC?", Delay_ms);
                    break;

                }
                catch (Exception)
                {
                }


            }

            sw.Stop();
            sw.Reset();
            sw.Start();

            while (true)
            {
                if (rtn.Contains("1"))
                {
                    break;
                }
                else
                {
                    Thread.Sleep(5);
                    try
                    {
                       // rtn = this.Chassis.ReadString();
                       // rtn = this._chassis.Query("*OPC?", Delay_ms);
                        //rtn = this._chassis.Query("*OPC?", 10000);
                        rtn = this._chassis.Query("*OPC?", Delay_ms);
                    }
                    catch (Exception)
                    {

                        //throw;
                    }

                }

                if (sw.ElapsedMilliseconds > 15000)
                {
                    break;
                }
            }
        }


        /// <summary>
        /// Gets or sets start wavelength in nm.
        /// </summary>
        public double StartWavelength_nm
        {
            get
            {
                string command = ":SENS:WAV:STAR?";
                string response = this._chassis.Query(command, Delay_ms);
                double startWavelength_nm = Convert.ToDouble(response) * 1e9;
                return startWavelength_nm;
            }
            set
            {

                string command = ":SENS:WAV:STAR " + value.ToString() + "nm";
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
                string command = ":SENS:WAV:STOP?";
                string response = this._chassis.Query(command, Delay_ms);
                double stopWavelength_nm = Convert.ToDouble(response) * 1e9;
                return stopWavelength_nm;
            }
            set
            {
                string command = ":SENS:WAV:STOP " + value.ToString() + "nm";
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
                string command = ":SENS:SWE:POIN " + value;
                this._chassis.TryWrite(command);
            }
            get
            {
                string command = ":SENS:SWE:POIN?";
                string response = this._chassis.Query(command, Delay_ms);
                int retVal = Convert.ToInt32(response);
                return retVal;
            }
        }
        public String TraceLength_string
        {
            set
            {
                int _value;
                string command = string.Empty;
                bool _Isok = int.TryParse(value, out _value);
                if (_Isok)
                {
                    command = ":SENS:SWE:POIN " + _value;
                }
                else
                {
                    command = ":SENSe:SWEep:POINts:AUTO " + "1";
                }
                //string command = ":SENS:SWE:POIN " + value;
                this._chassis.TryWrite(command);
            }
            get
            {
                //string command = ":SENS:SWE:POIN?";
                //this.Chassis.Write(command);
                //String retVal = this.Chassis.ReadString().Trim();
                //return retVal;
                string command = ":SENS:SWE:POIN?";
                string response = this._chassis.Query(command, Delay_ms);
                return response;
            }
        }


        /// <summary>
        /// Gets or sets center wavelength in nm.
        /// </summary>
        public double CenterWavelength_nm
        {
            set
            {
                this._chassis.TryWrite(":SENS:WAV:CENT " + value.ToString() + "nm");
            }

            get
            {
                string command = ":SENS:WAV:CENT?";
                string response = this._chassis.Query(command, Delay_ms);
                return Convert.ToDouble(response) * 1e9;
            }
        }

        /// <summary>
        /// 设定SMSR的次峰阈值标准.
        /// </summary>
        public double SmsrModeDiff
        {
            set
            {
                this._chassis.TryWrite(":CALCULATE:PARAMETER:COMMON:MDIFF " + value.ToString() + "DB");
            }

            get
            {
                string command = ":CALCULATE:PARAMETER:COMMON:MDIFF?";
                string response = this._chassis.Query(command, Delay_ms);
                return Convert.ToDouble(response);
            }
        }

        /// <summary>
        /// Gets or sets wavelength span in nm.
        /// </summary>
        public double WavelengthSpan_nm
        {
            set
            {
                this._chassis.TryWrite(":SENS:WAV:SPAN " + value.ToString() + "nm");
            }
            get
            {

                string command = ":SENS:WAV:SPAN?";
                string response = this._chassis.Query(command, Delay_ms);
                double wavelengthSpanValue = Convert.ToDouble(response) * 1e9;
                return wavelengthSpanValue;
            }
        }

        public void Reset()
        {
            this._chassis.TryWrite("*RST");
        }

    
        public bool IsSweepContinuous
        {
            get
            {
                string command = ":INIT:CONT?";

                int response = Convert.ToInt16(this._chassis.Query(command, Delay_ms));
                switch (response)
                {
                    case 1:
                        return true;
                    case 0:
                        return false;
                    default:
                        throw new NotSupportedException("Unknown response: " + response);
                }
            }
            set
            {
                string command = ":INIT:CONT ";
                if (value)
                {
                    command += "1";
                    this.SweepMode = YokogawaAQ6370SweepModes.Repeat;
                }
                else
                {
                    command += "0";
                    this.SweepMode = YokogawaAQ6370SweepModes.Single;
                }
                this._chassis.TryWrite(command);

            }
        }
        public double SMSRMask_nm
        {
            get
            {
                string command = ":CALCULATE:PARAMETER:SMSR MASK?";
                string response = this._chassis.Query(command, Delay_ms);
                double value_nm = Convert.ToDouble(response);
                return value_nm;
            }
            set
            {
                string command = ":CALCULATE:PARAMETER:SMSR MASK " + value.ToString() + "nm";
                this._chassis.TryWrite(command);
            }
        }
        public double ReadCenterWL_nm()
        {
            this._chassis.TryWrite(":calc:category swth");
            this._chassis.TryWrite(":calc");
            string response = this._chassis.Query(":calc:data?", Delay_ms);
            double centerWL_nm = Convert.ToDouble(response.Split(',')[0]) * 1e9;
            return centerWL_nm;
        }

        public void FixTrace(OsaTrace trace, bool isFix)
        {
            this._chassis.TryWrite($":TRACe:ATTRibute:{trace} {Convert.ToInt32(isFix)}");
        }
        public void DisplayTrace(OsaTrace trace, bool isOn)
        {
            this._chassis.TryWrite($":TRACe:STATe:{trace} {Convert.ToInt32(isOn)}");
        }
        //public string CalculateGainNF(
        //    int AALGo = 0,
        //    int FALGo = 0,
        //    double FARea = 1,
        //    double IOFFset = 0,
        //    double IRANge = 10,
        //    double MARea = 0.4,
        //    double MDIFF = 3.0,
        //    double OOFFset = 0,
        //    int PDISplay = 1,
        //    double TH = 20,
        //    int RBWidth = 1,
        //    int SNOISE = 1,
        //    int SPOWer = 0)
        public string CalculateGainNF(
                      int AALGo ,
                      int FALGo ,
                      double FARea ,
                      double IOFFset ,
                      double IRANge ,
                      double MARea ,
                      double MDIFF ,
                      double OOFFset,
                      int PDISplay ,
                      double TH ,
                      int RBWidth ,
                      int SNOISE ,
                      int SPOWer )
        {
            this._chassis.TryWrite(":CALCulate:CATegory NF");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:AALGo {AALGo}");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:FALGo {FALGo}");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:FARea {FARea}NM");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:IOFFset {IOFFset}");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:IRANge {IRANge}");//Sets or queries the integration frequency range for when the EDFA - NF analysis feature calculates the signal optical power.
            this._chassis.TryWrite($":CALCulate:PARameter:NF:MARea {MARea}NM");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:MDIFF {MDIFF}DB");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:OOFFset {OOFFset}");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:PDISplay {PDISplay}");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:TH {TH}DB");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:RBWidth {RBWidth}");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:SNOISE {SNOISE}");
            this._chassis.TryWrite($":CALCulate:PARameter:NF:SPOWer {SPOWer}");
            this._chassis.TryWrite(":CALCulate");
            string resp = this._chassis.Query(":CALCulate:DATA?", Delay_ms);
            return resp;
        }
    }
}