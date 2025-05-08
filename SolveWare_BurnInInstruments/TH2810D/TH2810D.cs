using System;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class TH2810D : InstrumentBase, IInstrumentBase, ITH2810D
    {
        const int Delay_ms = 50;
        public TH2810D(string name, string address, IInstrumentChassis chassis)
        : base(name, address, chassis)
        {
        }
        /// <summary>
        /// 设定TH2810D 的测试速度
        /// </summary>
        /// 
        TH2810D_SPEED _isSpeed = TH2810D_SPEED.MEDium;
        public TH2810D_SPEED SPEED
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isSpeed = value;
                    return;
                }
                string command = "SPEED ";
                string subCommand = "MEDium\r\n";
                switch (value)
                {
                    case TH2810D_SPEED.FAST:
                        subCommand = "FAST\r\n";
                        break;
                    case TH2810D_SPEED.MEDium:
                        subCommand = "MEDium\r\n";
                        break;
                    case TH2810D_SPEED.SLOW:
                        subCommand = "SLOW\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + SPEED.ToString());
                }

                command += subCommand;
                this._chassis.TryWrite(command);


            }
            get
            {
                if (this._isOnline == false)
                {
                    return _isSpeed;
                }

                string response = this._chassis.Query("SPEED?\r\n", Delay_ms);
                TH2810D_SPEED result = TH2810D_SPEED.MEDium;
                switch (response)
                {
                    case "FAST\r\n":
                        result = TH2810D_SPEED.FAST;
                        break;
                    case "MEDium\r\n":
                        result = TH2810D_SPEED.MEDium;
                        break;
                    case "SLOW\r\n":
                        result = TH2810D_SPEED.SLOW;
                        break;
                    default:
                        throw new NotSupportedException("Unknown results:" + response);
                }
                return result;
            }
        }
        /// <summary>
        /// 设定测试结果的显示方式
        /// </summary>
        TH2810D_DISPlay _isDisplay = TH2810D_DISPlay.DIRect;
        public TH2810D_DISPlay DISPlay
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isDisplay = value;
                    return;
                }
                string command = "DISPlay ";
                string subCommand = "DIRect\r\n";
                switch (value)
                {
                    case TH2810D_DISPlay.DIRect:
                        subCommand = "DIRect\r\n";
                        break;
                    case TH2810D_DISPlay.PERcent:
                        subCommand = "PERcent\r\n";
                        break;
                    case TH2810D_DISPlay.ABSolute:
                        subCommand = "ABSolute\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + DISPlay.ToString());
                }
                command += subCommand;
                this._chassis.TryWrite(command);
            }
            get
            {
                if (this._isOnline == false)
                {
                    return _isDisplay;
                }

                string response = this._chassis.Query("DISPlay? \r\n", Delay_ms);
                TH2810D_DISPlay result = TH2810D_DISPlay.DIRect;
                switch (response)
                {
                    case "DIRECT\r\n":
                        result = TH2810D_DISPlay.DIRect;
                        break;
                    case "PERCENT\r\n":
                        result = TH2810D_DISPlay.PERcent;
                        break;
                    case "ABSOLUTE\r\n":
                        result = TH2810D_DISPlay.ABSolute;
                        break;
                    default:
                        throw new NotSupportedException("Unknown results:" + response);
                }

                return result;
            }
        }
        /// <summary>
        /// 设定测试信号源的频率
        /// </summary>
        TH2810D_FREQ _isFreq = TH2810D_FREQ.Hz_120;
        public TH2810D_FREQ FREQuency
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isFreq = value;
                    return;
                }
                string command = "FREQuency ";
                string subCommand = "10K\r\n";
                switch (value)
                {
                    case TH2810D_FREQ.Hz_100:
                        subCommand = "100\r\n";
                        break;
                    case TH2810D_FREQ.Hz_120:
                        subCommand = "120\r\n";
                        break;
                    case TH2810D_FREQ.Hz_1K:
                        subCommand = "1K\r\n";
                        break;
                    case TH2810D_FREQ.Hz_10K:
                        subCommand = "10K\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + FREQuency.ToString());
                }

                command += subCommand;
                this._chassis.TryWrite(command);
            }
            get
            {
                if (this._isOnline == false)
                {
                    return _isFreq;
                }

                string response = this._chassis.Query("FREQuency? \r\n", Delay_ms);
                TH2810D_FREQ result = TH2810D_FREQ.Hz_120;
                switch (response)
                {
                    case "100\r\n":
                        result = TH2810D_FREQ.Hz_100;
                        break;
                    case "120\r\n":
                        result = TH2810D_FREQ.Hz_120;
                        break;
                    case "1K\r\n":
                        result = TH2810D_FREQ.Hz_1K;
                        break;
                    case "10K\r\n":
                        result = TH2810D_FREQ.Hz_10K;
                        break;
                    default:
                        throw new NotSupportedException("Unknown results:" + response);
                }
                return result;
            }
        }
        /// <summary>
        /// 设定主副被测参数的组合
        /// </summary>
        TH2810D_PARA _isPara = TH2810D_PARA.CD;
        public TH2810D_PARA PARAmeter
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isPara = value;
                    return;
                }
                string command = "PARAmeter ";
                string subCommand = "CD\r\n";
                switch (value)
                {
                    case TH2810D_PARA.CD:
                        subCommand = "CD\r\n";
                        break;
                    case TH2810D_PARA.RQ:
                        subCommand = "RQ\r\n";
                        break;
                    case TH2810D_PARA.ZQ:
                        subCommand = "ZQ\r\n";
                        break;
                    case TH2810D_PARA.LQ:
                        subCommand = "LQ\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + value);
                }

                command += subCommand;
                this._chassis.TryWrite(command);
            }
        }
        public string GetPARAmeter()
        {
            if (this._isOnline == false)
            {
                return _isPara.ToString();
            }
            string response = this._chassis.Query("PARAmeter?\r\n", Delay_ms);
            return response;
        }
        /// <summary>
        /// 设定测试信号源的输出电压
        /// </summary>
        TH2810D_LEV _isLev = TH2810D_LEV.V3;
        public TH2810D_LEV LEVel
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isLev = value;
                    return;
                }
                string command = "LEVel ";
                string subCommand = "0.1V\r\n";
                switch (value)
                {
                    case TH2810D_LEV.V1:
                        subCommand = "1.0V\r\n";
                        break;
                    case TH2810D_LEV.V2:
                        subCommand = "0.3V\r\n";
                        break;
                    case TH2810D_LEV.V3:
                        subCommand = "0.1V\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + value);
                }

                command += subCommand;
                this._chassis.TryWrite(command);
            }
        }
        public string GetLEVel()
        {
            if (this._isOnline == false)
            {
                return _isLev.ToString();
            }
            string response = this._chassis.Query("LEVel? \r\n", Delay_ms);
            return response;
        }
        /// <summary>
        /// 设定信号源的输出电阻
        /// </summary>
        TH2810D_SR _isSr = TH2810D_SR.R_100;
        public TH2810D_SR SRESistor
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isSr = value;
                    return;
                }
                string command = "SRESistor ";
                string subCommand = "100\r\n";
                switch (value)
                {
                    case TH2810D_SR.R_30:
                        subCommand = "30\r\n";
                        break;
                    case TH2810D_SR.R_100:
                        subCommand = "100\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + value);
                }

                command += subCommand;
                this._chassis.TryWrite(command);
            }
        }
        public int GetSRESistor()
        {
            if (this._isOnline == false)
            {
                return int.Parse(_isSr.ToString());
            }
            string response = this._chassis.Query("SRESistor? \r\n", Delay_ms);
            return int.Parse(response);
        }
        /// <summary>
        /// 触发一次测量或设定触发方式
        /// </summary>
        TH2810D_TRIG _isTrig = TH2810D_TRIG.IMMediate;
        public TH2810D_TRIG TRIGger
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isTrig = value;
                    return;
                }
                string command = "TRIGger ";
                string subCommand = "IMMediate\r\n";
                switch (value)
                {
                    case TH2810D_TRIG.INTernal:
                        subCommand = "INTernal\r\n";
                        break;
                    case TH2810D_TRIG.EXTernal:
                        subCommand = "EXTernal\r\n";
                        break;
                    case TH2810D_TRIG.IMMediate:
                        subCommand = "IMMediate\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + value);
                }

                command += subCommand;
                this._chassis.TryWrite(command);
            }
        }
        public string GetTRIGger()
        {
            if (this._isOnline == false)
            {
                return _isTrig.ToString();
            }
            string response = this._chassis.Query("TRIGger? \r\n", Delay_ms);
            return response;
        }
        /// <summary>
        /// 对不同的测试电压和信号源内阻下执行OPEN 或SHORt 清零操作
        /// </summary>
        TH2810D_CORR _isCorr;
        public TH2810D_CORR CORRection
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isCorr = value;
                    return;
                }
                string command = "CORRection ";
                string subCommand = "OPEN\r\n";
                switch (value)
                {
                    case TH2810D_CORR.OPEN:
                        break;
                    case TH2810D_CORR.OPEN_ALL:
                        subCommand = "OPEN_ALL\r\n";
                        break;
                    case TH2810D_CORR.SHORt:
                        subCommand = "SHORt\r\n";
                        break;
                    case TH2810D_CORR.SHORt_ALL:
                        subCommand = "SHORt_ALL\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + value);
                }

                command += subCommand;
                this._chassis.TryWrite(command);
            }
        }
        /// <summary>
        /// 比较功能
        /// </summary>
        bool _isCOMParator = false;
        public bool IsCOMParator
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isCOMParator = value;
                    return;
                }

                string command;
                if (value == true)
                {
                    command = "COMParator ON\r\n";
                }
                else
                {
                    command = "COMParator OFF\r\n";
                }

                this._chassis.TryWrite(command);

                Thread.Sleep(20);
                if (value != IsCOMParator)
                {
                    throw new NotSupportedException("COMParator didn't set properly");
                }
            }
            get
            {
                if (this._isOnline == false)
                {
                    return _isCOMParator;
                }

                string response = this._chassis.Query("COMParator? \r\n", Delay_ms);
                if (response.Contains("ON"))
                {
                    return true;
                }
                else if (response.Contains("OFF"))
                {
                    return false;
                }
                else
                {
                    throw new NotSupportedException("response to command ':COMParator?' is not ON or OFF");
                }
            }
        }
        ///// <summary>
        ///// 打开比较功能
        ///// </summary>
        //public void COMParator_ON()
        //{
        //    string command = "LIMIT:COMParator ON ";
        //    this._chassis.TryWrite(command);
        //}
        ///// <summary>
        ///// 关闭比较功能
        ///// </summary>
        //public void COMParator_OFF()
        //{
        //    string command = "LIMIT:COMParator OFF ";
        //    this._chassis.TryWrite(command);
        //}
        //public string GetCOMParator()
        //{
        //    string response = this._chassis.Query("LIMIT:COMParator? ", Delay_ms);
        //    return response;
        //}

        /// <summary>
        /// 设定被测件的等效电路方式
        /// </summary>
        TH2810D_EQU _isEqu = TH2810D_EQU.SERial;
        public TH2810D_EQU EQUivalent
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isEqu = value;
                    return;
                }
                string command = "EQUivalent ";
                string subCommand = "SERial\r\n";
                switch (value)
                {
                    case TH2810D_EQU.SERial:
                        break;
                    case TH2810D_EQU.PARallel:
                        subCommand = "PARallel\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + value);
                }

                command += subCommand;
                this._chassis.TryWrite(command);
            }
        }
        public string GetEQUivalent()
        {
            if (this._isOnline == false)
            {
                return _isEqu.ToString();
            }
            string response = this._chassis.Query("EQUivalent? \r\n", Delay_ms);
            return response;
        }
        /// <summary>
        /// 设定量程选择方式或设定当前测试量程
        /// </summary>
        TH2810D_RANG _isRang = TH2810D_RANG.AUTO;
        public TH2810D_RANG RANGe
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isRang = value;
                    return;
                }
                string command = "RANGe ";
                string subCommand = "AUTO\r\n";
                switch (value)
                {
                    case TH2810D_RANG.AUTO:
                        subCommand = "AUTO\r\n";
                        break;
                    case TH2810D_RANG.HOLD:
                        subCommand = "HOLD\r\n";
                        break;
                    case TH2810D_RANG.ZERO:
                        subCommand = "0\r\n";
                        break;
                    case TH2810D_RANG.ONE:
                        subCommand = "1\r\n";
                        break;
                    case TH2810D_RANG.TWO:
                        subCommand = "2\r\n";
                        break;
                    case TH2810D_RANG.THREE:
                        subCommand = "3\r\n";
                        break;
                    case TH2810D_RANG.FOUR:
                        subCommand = "4\r\n";
                        break;
                    case TH2810D_RANG.FIVE:
                        subCommand = "5\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + value);
                }
                command += subCommand;
                this._chassis.TryWrite(command);
            }
        }
        public string GetRANGe()
        {
            if (this._isOnline == false)
            {
                return _isRang.ToString();
            }
            string response = this._chassis.Query("RANGe? \r\n", Delay_ms);
            return response;
        }
        /// <summary>
        /// 设定蜂鸣器的讯响状态
        /// </summary>
        TH2810D_ALAR _isAlar = TH2810D_ALAR.OFF;
        public TH2810D_ALAR ALARm
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isAlar = value;
                    return;
                }
                string command = "ALARm ";
                string subCommand = "OFF\r\n";
                switch (value)
                {
                    case TH2810D_ALAR.OFF:
                        subCommand = "OFF\r\n";
                        break;
                    case TH2810D_ALAR.AUX:
                        subCommand = "AUX\r\n";
                        break;
                    case TH2810D_ALAR.P3:
                        subCommand = "P3\r\n";
                        break;
                    case TH2810D_ALAR.P2:
                        subCommand = "P2\r\n";
                        break;
                    case TH2810D_ALAR.P1:
                        subCommand = "P1\r\n";
                        break;
                    case TH2810D_ALAR.NG:
                        subCommand = "NG\r\n";
                        break;
                    default:
                        throw new NotSupportedException("Incorrect input: " + value);
                }

                command += subCommand;
                this._chassis.TryWrite(command);
            }
        }
        public string GetALARm()
        {
            if (this._isOnline == false)
            {
                return _isAlar.ToString();
            }
            string response = this._chassis.Query("ALARm? \r\n", Delay_ms);
            return response;
        }

        #region LIMIT 子系统命令
        /// <summary>
        /// 查询返回最近一次主副参数的测试结果
        /// </summary>
        /// <returns></returns>
        string _isFetch = "false";
        public string GetFETCh()
        {
            if (this._isOnline == false)
            {
                return _isFetch;
            }

            string response = this._chassis.Query("FETCH ?\r\n", Delay_ms);
            return response;
        }
        /// <summary>
        /// 设定比较功能副参数的上限和下限值
        /// </summary>
        /// <param name="lowlimit"></param>
        /// <param name="highlimit"></param>
        string _isSecondary = "false";
        public void SECondary(double lowlimit, double highlimit)
        {
            if (this._isOnline == false)
            {
                _isSecondary = lowlimit + "," + highlimit;
                return;
            }

            string command = $"LIMIT:SECondary {lowlimit},{highlimit}\r\n";
            this._chassis.TryWrite(command);
        }
        public string GetSECondary()
        {
            if (this._isOnline == false)
            {
                return _isSecondary;
            }

            string response = this._chassis.Query("LIMIT:SECondary? \r\n", Delay_ms);
            return response;
        }
        /// <summary>
        /// 设定比较功能各档的上下极限值
        /// </summary>
        /// <param name="lowlimit"></param>
        /// <param name="highlimit"></param>
        string _isBin = "false";
        public void BIN(int n, double lowlimit, double highlimit)
        {
            if (n >= 1 && n <= 3)
            {
                if (this._isOnline == false)
                {
                    _isBin = lowlimit + "," + highlimit;
                    return;
                }

                string command = $"LIMIT:BIN{n} {lowlimit},{highlimit}\r\n";
                this._chassis.TryWrite(command);
            }
            else
            {
                throw new NotSupportedException("Incorrect input: " + n);
            }
        }
        public string GetBIN(int n)
        {
            if (n >= 3 && n <= 1)
            {
                throw new NotSupportedException("Incorrect input: " + n);
            }
            else
            {
                if (this._isOnline == false)
                {
                    return _isBin;
                }

                string response = this._chassis.Query($"LIMIT:BIN{n}?\r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 设定标称值
        /// </summary>
        string _isNominal_C = "false";
        public string NOMinal_C
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isNominal_C = value;
                    return;
                }

                string command = $"LIMIT:NOMinal_C {value}\r\n";
                this._chassis.TryWrite(command);
            }
            get
            {
                if (this._isOnline == false)
                {
                    return _isNominal_C;
                }

                string response = this._chassis.Query("LIMIT:NOMinal_C? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 设定标称值
        /// </summary>
        string _isNominal_L = "false";
        public string NOMinal_L
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isNominal_L = value;
                    return;
                }

                string command = $"LIMIT:NOMinal_L {value}\r\n";
                this._chassis.TryWrite(command);
            }
            get
            {
                if (this._isOnline == false)
                {
                    return _isNominal_L;
                }

                string response = this._chassis.Query("LIMIT:NOMinal_L? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 设定标称值
        /// </summary>
        string _isNominal_Z = "false";
        public string NOMinal_Z
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isNominal_Z = value;
                    return;
                }
                string command = $"LIMIT:NOMinal_Z {value} \r\n";
                this._chassis.TryWrite(command);
            }
            get
            {
                if (this._isOnline == false)
                {
                    return _isNominal_Z;
                }

                string response = this._chassis.Query("LIMIT:NOMinal_Z? \r\n", Delay_ms);
                return response;
            }
        }
        /// <summary>
        /// 设定标称值
        /// </summary>
        string _isNominal_R = "false";
        public string NOMinal_R
        {
            set
            {
                if (this._isOnline == false)
                {
                    _isNominal_Z = value;
                    return;
                }

                string command = $"LIMIT:NOMinal_R {value} \r\n";
                this._chassis.TryWrite(command);
            }
            get
            {
                if (this._isOnline == false)
                {
                    return _isNominal_Z;
                }

                string response = this._chassis.Query("LIMIT:NOMinal_R? \r\n", Delay_ms);
                return response;
            }
        }
        #endregion
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            // throw new NotImplementedException();
        }
        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //  throw new NotImplementedException();
        }
    }
}
