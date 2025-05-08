using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments
{
    public class FWM8612 : InstrumentBase
    {
        private const int Delay_ms = 50;
        private const string ErrStr = "Instrument not connected";

        public FWM8612(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {
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
                    _instrumentIdn = this._chassis.Query("*IDN?\r\n", Delay_ms);
                }

                return _instrumentIdn;
            }
        }
        public override void Initialize()
        {

        }
      
        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="channel">通道1,2</param>
        /// <param name="time">时间</param>
        public void SetExposureTime(Channel channel, int time)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            int ch = 1;
            if (channel == Channel.ch_2)
            {
                ch = 2;
            }
            try
            {
                this._chassis.TryWrite($":CONF:EXP{ch} {time}\r\n");
            }
            catch (Exception ex)
            {
            }

        }
        /// <summary>
        /// 获取曝光值
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public int GetExposureTime(Channel channel)
        {
            if (this.IsOnline == false)
            {
                return 0;
            }
            int ch = 1;
            if (channel == Channel.ch_2)
            {
                ch = 2;
            }
            try
            {
                string val = this._chassis.Query($":CONF:EXP{ch}?\r\n", Delay_ms);
                return int.Parse(val);
            }
            catch (Exception ex)
            {
                return -1;
            }

        }
        /// <summary>
        /// 自动曝光 自适应曝光（开、关）
        /// </summary>
        /// <param name="auto"></param>
        public void SetAutomaticExposure(Auto auto)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                switch (auto)
                {
                    case Auto.Ones:
                        {
                            this._chassis.TryWrite($":CONF:EXP:AUTO\r\n");
                        }
                        break;
                    case Auto.On:
                        {
                            this._chassis.TryWrite($":CONF:EXP:ADAP 1\r\n");
                        }
                        break;
                    case Auto.Off:
                        {
                            this._chassis.TryWrite($":CONF:EXP:ADAP 0\r\n");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="gain"></param>
        public void SetGain(Channel channel, Gain_FWM gain)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                int ch = 1;
                if (channel == Channel.ch_2)
                {
                    ch = 2;
                }
                string hl = "High";
                if (gain == Gain_FWM.Low)
                {
                    hl = "Low";
                }
                this._chassis.TryWrite($":CONF:GAIN{ch} {hl}\r\n");
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取增益
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public Gain_FWM GetGain(Channel channel)
        {
            if (this.IsOnline == false)
            {
                return Gain_FWM.Low;
            }
            try
            {
                int ch = 1;
                if (channel == Channel.ch_2)
                {
                    ch = 2;
                }
                string val = this._chassis.Query($":CONF:GAIN{ch}?\r\n", Delay_ms);
                return (Gain_FWM)Enum.Parse(typeof(Gain_FWM), val);

            }
            catch (Exception ex)
            {
                return Gain_FWM.Low;
            }
        }
        /// <summary>
        /// 设置滤波数
        /// </summary>
        /// <param name="number"></param>
        public void SetFilterNumber(int number)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                if (number <= 1)
                {
                    number = 1;
                }
                if (number >= 128)
                {
                    number = 128;
                }
                this._chassis.TryWrite($":CONF:FILT:COUN {number}\r\n");
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取滤波数
        /// </summary>
        /// <returns></returns>
        public int GetFilt()
        {
            if (this.IsOnline == false)
            {
                return 0;
            }
            try
            {
                string val = this._chassis.Query($":CONF:FILT:COUN?\r\n", Delay_ms);
                return int.Parse(val);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        /// <summary>
        /// 设置波长单位
        /// </summary>
        /// <param name="unit"></param>
        public void SetWavelengthUnit(WaveUnit unit)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                string un = "NM";
                switch (unit)
                {
                    case WaveUnit.NM:
                        break;
                    case WaveUnit.THZ:
                        un = "THZ";
                        break;
                    case WaveUnit.ICM:
                        un = "ICM";
                        break;
                }
                this._chassis.TryWrite($":CONFigure:WAVelength:UNIT {un}\r\n");
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取波长单位
        /// </summary>
        /// <returns></returns>
        public WaveUnit GetWaveUnit()
        {
            if (this.IsOnline == false)
            {
                return WaveUnit.NM;
            }
            try
            {
                string val = this._chassis.Query($":CONFigure:WAVelength:UNIT?\r\n", Delay_ms);
                return (WaveUnit)Enum.Parse(typeof(WaveUnit), val);
            }
            catch (Exception ex)
            {
                return WaveUnit.NM;
            }
        }
        /// <summary>
        /// 设置功率单位
        /// </summary>
        /// <param name="unit"></param>
        public void SetPowerUnit(PowUnit unit)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                string un = "DBM";
                switch (unit)
                {
                    case PowUnit.DBM:
                        break;
                    case PowUnit.MW:
                        un = "MW";
                        break;
                }
                this._chassis.TryWrite($":CONF:POW:UNIT {un}\r\n");
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取功率单位
        /// </summary>
        /// <returns></returns>
        public PowUnit GetPowUnit()
        {
            if (this.IsOnline == false)
            {
                return PowUnit.DBM;
            }
            try
            {
                string val = this._chassis.Query($":CONF:POW:UNIT?\r\n", Delay_ms);
                return (PowUnit)Enum.Parse(typeof(PowUnit), val);
            }
            catch (Exception ex)
            {
                return PowUnit.DBM;
            }
        }
        /// <summary>
        /// 设置宽带、窄带模式
        /// </summary>
        /// <param name="band"></param>
        public void SetBand(Band band)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                string mode = "Narrow";
                switch (band)
                {
                    case Band.Broad:
                        mode = "Broad";
                        break;
                    case Band.Narrow:
                        break;
                }
                this._chassis.TryWrite($":CONF:BAND {mode}\r\n");
            }
            catch (Exception ex)
            {
            }
        }

        public Band GetBand()
        {
            if (this.IsOnline == false)
            {
                return Band.Narrow;
            }
            try
            {
                string val = this._chassis.Query($":CONF:BAND?\r\n", Delay_ms);
                return (Band)Enum.Parse(typeof(Band), val);
            }
            catch (Exception ex)
            {
                return Band.Narrow;
            }
        }
        /// <summary>
        /// 设置触发频率
        /// </summary>
        /// <param name="fer"></param>
        public void SetFREQuency(int fer)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                if (fer <= 1)
                {
                    fer = 1;
                }
                if (fer >= 1000)
                {
                    fer = 1000;
                }
                this._chassis.TryWrite($":CONF:INT:FREQ {fer}\r\n");
            }
            catch (Exception ex)
            {
            }
        }

        public int GetFERQuency()
        {
            if (this.IsOnline == false)
            {
                return 0;
            }
            try
            {
                string val = this._chassis.Query($":CONF:INT:FREQ?\r\n", Delay_ms);
                return int.Parse(val);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        /// <summary>
        /// 获取波长
        /// </summary>
        /// <returns></returns>
        public double GetWavelenth()
        {
            if (this.IsOnline == false)
            {
                return 0;
            }
            try
            {
                int delay_ms_fast = 5;
                string val = this._chassis.Query($":MEASure:WL?\r\n", delay_ms_fast);
                if (val == "Error -102\n")
                {
                    return -1;
                }
                return double.Parse(val);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        /// <summary>
        /// 获取功率
        /// </summary>
        /// <returns></returns>
        public double GetPower()
        {
            if (this.IsOnline == false)
            {
                return 0;
            }
            try
            {
                int delay_ms_fast = 5; 
                string val = this._chassis.Query($":MEAS:POW?\r\n", delay_ms_fast);
                return double.Parse(val);
            }
            catch (Exception ex)
            {
                return -30;
            }
        }
        /// <summary>
        /// 获取光波数
        /// </summary>
        /// <returns></returns>
        public double GetWNumber()
        {
            if (this.IsOnline == false)
            {
                return 0;
            }
            try
            {
                string val = this._chassis.Query($":MEASure:SCALar:POWer:WNUMber?\r\n", Delay_ms);
                return double.Parse(val);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        /// <summary>
        /// 获取波长和功率
        /// </summary>
        /// <returns></returns>
        public double[] GetWPower()
        {
            if (this.IsOnline == false)
            {
                return new double[] { 0, 0 };
            }
            try
            {
                string val = this._chassis.Query($":MEAS:WPOW?\r\n", Delay_ms);
                val = val.Trim(';');
                var result = val.Split(',');
                return new double[] { double.Parse(result[0]), double.Parse(result[1]) };
            }
            catch (Exception ex)
            {
                return new double[] { -1, -1 };
            }
        }
        /// <summary>
        /// 获取外部触发返回的功率和波长
        /// </summary>
        /// <returns></returns>
        public DataBook<double, double> EXTernalStart()
        {
            if (this.IsOnline == false)
            {
                var val = new DataBook<double, double>();
                val.Add(0, 0);
                return val;
            }
            try
            {
                _chassis.DefaultBufferSize = 1024*10;
                string val = this._chassis.Query($"READ:WL:EXTernalStart\r\n", 500);
                var res = val.Split(';');
                var result = new DataBook<double, double>();
                for (int i = 0; i < res.Length - 1; i++)
                {
                    var str = res[i].Split(',');
                    result.Add(double.Parse(str[0]), double.Parse(str[1]));
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"FWM8612  error:{ex.Message}!");
            }
        }

        public void EXTernalStart2()
        {
            if (this.IsOnline == false)
            {
                return ;
            }
            this._chassis.TryWrite($"READ:WL:EXTernalStart\r\n");
            this._chassis.DefaultBufferSize = 1024 * 100;

            Thread.Sleep(500); //等待执行完成

        }
        public bool FethEXTernalData(string SerialNumber, int count, out WavelengthAndPower result)
        {
            result = new WavelengthAndPower();
            if (this.IsOnline == false)
            {
                return true;
            }

            var respon_org = (this._chassis as EthernetChassis).ReadStringLoop(count);
            var respon_org2 = respon_org.Replace("\r", "").Replace("\n", "");
            var respon = respon_org2.Trim(';');
            var wavestr = respon.Split(';');

            try
            {
                //新固件有返回错误的问题, 绕过
                for (int i = 0; i < wavestr.Length; i++)
                {
                    double wl = 0;
                    double pw = 0;

                    var str = wavestr[i].Split(',');
                    if (str.Length >= 2)
                    {
                        if (double.TryParse(str[1], out wl) == false) wl = 0;
                        if (double.TryParse(str[0], out pw) == false) pw = 0;

                        result.Wavelength.Add(wl);
                        result.Power.Add(pw);
                    }
                    else
                    {
                        string msg = "返回数据格式错误异常";

                        //20250225 保存错误数据


                    }
                    //result.Wavelength.Add(double.Parse(str[1]));
                    //result.Power.Add(double.Parse(str[0]));
                }
            }
            catch (Exception ex)
            {
                string path = Application.StartupPath + $@"\Data\FWM8612_ResponseErr\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\FWM8612_ResponseErr";
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //20250228 增加数据存储功能
                DateTime dt = DateTime.Now;

                string defaultFileName_org = string.Concat(@"FWM8612_FormatErrOrg_", BaseDataConverter.ConvertDateTimeTo_FILE_string(dt), ".txt");
                var finalFileName_org = $@"{path}\{defaultFileName_org}";
                var t_respon = respon_org;

                using (StreamWriter sw = new StreamWriter(finalFileName_org, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(t_respon);
                }

                string defaultFileName_org2 = string.Concat(@"FWM8612_FormatErr_", BaseDataConverter.ConvertDateTimeTo_FILE_string(dt), ".txt");
                var finalFileName_org2 = $@"{path}\{defaultFileName_org2}";

                t_respon = respon_org2.Replace(";", "\r\n");

                using (StreamWriter sw = new StreamWriter(finalFileName_org2, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(t_respon);
                }

                throw ex;
            }


            if(result.Wavelength.Count!=count)
            {
                string path = Application.StartupPath + $@"\Data\FWM8612_ResponseErr\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\FWM8612_ResponseErr";
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //20250228 增加数据存储功能
                DateTime dt = DateTime.Now;

                string defaultFileName_org = string.Concat(@"FWM8612_CountErrOrg_", BaseDataConverter.ConvertDateTimeTo_FILE_string(dt), ".txt");
                var finalFileName_org = $@"{path}\{defaultFileName_org}";
                var t_respon = respon_org;

                using (StreamWriter sw = new StreamWriter(finalFileName_org, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(t_respon);
                }

                string defaultFileName_org2 = string.Concat(@"FWM8612_CountErr_", BaseDataConverter.ConvertDateTimeTo_FILE_string(dt), ".txt");
                var finalFileName_org2 = $@"{path}\{defaultFileName_org2}";

                t_respon = respon_org2.Replace(";", "\r\n");

                using (StreamWriter sw = new StreamWriter(finalFileName_org2, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine($"Target:{count}");
                    sw.WriteLine($"Recive:{result.Wavelength.Count}");
                    sw.WriteLine("========================");

                    sw.WriteLine(t_respon);
                }

                //throw new Exception($"波长计获取结果数量[{wavestr.Length}]与需求目标数量[{count}]不等");
                return false;
            }
            else
            {
                //20250314  保存正常的通讯数据
                string path = Application.StartupPath + $@"\Data\FWM8612_ResponseErr\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\FWM8612_ResponseErr";
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //20250228 增加数据存储功能
                DateTime dt = DateTime.Now;

                string defaultFileName_org = string.Concat(@"FWM8612_CountNormalOrg_", BaseDataConverter.ConvertDateTimeTo_FILE_string(dt), ".txt");
                var finalFileName_org = $@"{path}\{defaultFileName_org}";
                var t_respon = respon_org;

                using (StreamWriter sw = new StreamWriter(finalFileName_org, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(t_respon);
                }

                string defaultFileName_org2 = string.Concat(@"FWM8612_CountNormal_", BaseDataConverter.ConvertDateTimeTo_FILE_string(dt), ".txt");
                var finalFileName_org2 = $@"{path}\{defaultFileName_org2}";

                t_respon = respon_org2.Replace(";", "\r\n");

                using (StreamWriter sw = new StreamWriter(finalFileName_org2, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine($"Target:{count}");
                    sw.WriteLine($"Recive:{result.Wavelength.Count}");
                    sw.WriteLine("========================");

                    sw.WriteLine(t_respon);
                }
            }

            return true;
        }


        /// <summary>
        /// 关闭外部触发
        /// </summary>
        public void EXTernalStop()
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                this._chassis.TryWrite($"READ:WL:EXTernalStop\r\n");
                Thread.Sleep(500); //等待执行完成
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取内部触发返回的功率和波长
        /// </summary>
        /// <returns></returns>
        public DataBook<double, double> INTernalStart()
        {
            if (this.IsOnline == false)
            {
                var val = new DataBook<double, double>();
                val.Add(0, 0);
                return val;
            }
            try
            {
                _chassis.DefaultBufferSize = 1024*10;
                _chassis.Timeout_ms = 60 * 1000;
                string val = this._chassis.QueryWithLongResponTime($":READ:WL:INTernalStart\r\n", 500,5 *1000);
                var res = val.Split(';');
                var result = new DataBook<double, double>();
                for (int i = 0; i < res.Length - 1; i++)
                {
                    var str = res[i].Split(',');
                    result.Add(double.Parse(str[0]), double.Parse(str[1]));
                }
                return result;
            }
            catch (Exception ex)
            {
                _chassis.Timeout_ms =500;
                var val = new DataBook<double, double>();
                val.Add(-1, -1);
                return val;
            }
        }
        /// <summary>
        /// 关闭内部触发
        /// </summary>
        public void INTernalStop()
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                this._chassis.TryWrite($":READ:WL:INTernalStop\r\n");
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 设置触发源
        /// </summary>
        /// <param name="source"></param>
        public void SetTriggerSource(Source source)
        {
            if (this.IsOnline == false)
            {
                return;
            }
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    var sour = "SINGle";
                    switch (source)
                    {
                        case Source.EXTernal:
                            sour = "EXTernal";
                            break;
                        case Source.INTernal:
                            sour = "INTernal";
                            break;
                        case Source.SINGle:
                            break;
                    }
                    this._chassis.TryWrite($":TRIG:SOUR {sour}\r\n");
                    Thread.Sleep(200);  //等待执行完成

                    //确认已经切换成功
                    if (GetTriggerSource() == source)
                    {
                        Thread.Sleep(200);  //等待执行完成
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(200);  //等待一下, 再执行一次
                }
            }
        }
        public Source GetTriggerSource()
        {
            if (this.IsOnline == false)
            {
                return Source.SINGle;
            }
            try
            {
                string val = this._chassis.Query($":TRIG:SOUR?\r\n", Delay_ms);
                return (Source)Enum.Parse(typeof(Source), val);
            }
            catch (Exception ex)
            {
                return Source.SINGle;
            }
        }

        public void RST()
        {
            if (this.IsOnline == false)
            {
                return;
            }
            try
            {
                this._chassis.TryWrite($"*RST?\r\n");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Thread.Sleep(200);//等待运行完成
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
    public class WavelengthAndPower
    {
        public WavelengthAndPower()
        {
            Wavelength = new List<double>();
            Power = new List<double>();
        }

        public List<double> Wavelength { get; set; }
        public List<double> Power { get; set; }
    }
    public enum Channel
    {
        ch_1,
        ch_2,
    }
    public enum Auto
    {
        Ones,
        On,
        Off,
    }
    public enum Gain_FWM
    {
        High,
        Low,
    }
    public enum WaveUnit
    {
        NM,
        THZ,
        ICM,
    }
    public enum PowUnit
    {
        DBM,
        MW,
    }
    public enum Band
    {
        Broad,//宽
        Narrow,//窄
    }
    public enum Source
    {
        EXTernal,//外
        INTernal,//内
        SINGle,//单
    }
}
