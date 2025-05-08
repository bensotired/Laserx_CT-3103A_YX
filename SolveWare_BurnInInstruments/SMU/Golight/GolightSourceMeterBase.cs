using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SolveWare_BurnInInstruments
{

    public class GolightSourceMeterBase : InstrumentBase, ISourceMeter_Golight
    {
 
        protected virtual int DATA_HEAD_LEN { get; } = 1;
        protected virtual int DATA_PACK_CALC_LEN { get; } = 2;
        protected virtual int CMD_MAJOR_LEN { get; } = 4;
        protected virtual int CMD_MINOR_LEN { get; } = 4;
        protected virtual int CMD_SUM_VERIFY_LEN { get; } = 1;
        protected virtual int Delay_ms { get; } = 300;
        int _debugProp = 0;
        public virtual int debugProp { set { _debugProp = value; }  }  
        public GolightSourceMeterBase(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {


        }
        public override object HandleChassisOnline()
        {
            SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH1, GolightSource_PD_TEST_CHANNEL_RANGE.Range1);
            SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH2, GolightSource_PD_TEST_CHANNEL_RANGE.Range1);
            return true;
        }

        /// <summary>
        /// build full command with 'ADD8' verification
        /// </summary>
        /// <param name="cmdMajor"></param>
        /// <param name="cmdMinor"></param>
        /// <param name="cmdData"></param>
        /// <returns></returns>
        protected virtual byte[] BuildCommand(byte[] cmdMajor, byte[] cmdMinor, byte[] cmdData)
        {
            List<byte> cmd = new List<byte>();
            //数据头
            cmd.Add(0xAA);
            //数据长度-临时占位
            cmd.Add(0x00);
            cmd.Add(0x00);
            //主命令
            cmd.AddRange(cmdMajor);
            //从命令
            cmd.AddRange(cmdMinor);
            //命令包含数据
            if (cmdData != null)
            {
                cmd.AddRange(cmdData);
            }
            //校验和-临时占位
            cmd.Add(0x00);
            //计算数据长度-替换占位
            var lenBytes = Converter.IntToBytes(cmd.Count - 3);
            cmd[1] = lenBytes[1];
            cmd[2] = lenBytes[0];
            //计算校验和-替换占位
            int sum = 0;
            for (int i = 0; i < cmd.Count - 1; i++)
            {
                sum += cmd[i];
            }
            cmd[cmd.Count - 1] = (byte)sum;
            return cmd.ToArray();
        }
        protected virtual byte[] GetCommonReturnData(byte[] retBytes)
        {
            int dataLen = retBytes.Length -
                            DATA_HEAD_LEN -
                            DATA_PACK_CALC_LEN -
                            CMD_MAJOR_LEN -
                            CMD_MINOR_LEN -
                            CMD_SUM_VERIFY_LEN;

            byte[] destBytes = new byte[dataLen];

            Array.Copy(retBytes, (DATA_HEAD_LEN + DATA_PACK_CALC_LEN + CMD_MAJOR_LEN + CMD_MINOR_LEN), destBytes, 0, dataLen);
            return destBytes;
        }

        public virtual string InstrumentIDN
        {
            get
            {
                string idn = "GolightSourceMeter";
                if (this.IsOnline == false)
                {
                    return idn;
                }
                byte[] cmdMajor = { 0x52, 0x44, 0x44, 0x49 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x4E };
                byte[] cmdData = null;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);
                idn = ASCIIEncoding.Default.GetString(dataBytes);
                return idn;
            }
        }
        public virtual string InstrumentSerialNumber
        {
            get
            {
                string sn = "Unknown SN";
                if (this.IsOnline == false)
                {
                    return sn;
                }
                byte[] cmdMajor = { 0x52, 0x44, 0x44, 0x49 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x53 };
                byte[] cmdData = null;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);
                sn = ASCIIEncoding.Default.GetString(dataBytes);
                return sn;
            }
        }
        public virtual string InstrumentVersion
        {
            get
            {
                string sn = "Unknown Version";
                if (this.IsOnline == false)
                {
                    return sn;
                }
                byte[] cmdMajor = { 0x52, 0x44, 0x44, 0x49 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x56 };
                byte[] cmdData = null;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);
                sn = ASCIIEncoding.Default.GetString(dataBytes);
                return sn;
            }
        }
        public virtual string InstrumentIP
        {
            get
            {
                string ip = "Unknown IP";
                if (this.IsOnline == false)
                {
                    return ip;
                }
                byte[] cmdMajor = { 0x52, 0x44, 0x44, 0x49 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x49 };
                byte[] cmdData = null;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);
                ip = string.Format("{0}.{1}.{2}.{3}", dataBytes[0], dataBytes[1], dataBytes[2], dataBytes[3]);
                return ip;
            }
            set
            {
                if (this.IsOnline == false) return;
                //写入IP地址 
                //上位机发送： AA 0D 00 57 52 44 49 00 00 00 49【IP地址】【校验和】
                //下位机返回： AA 09 00 57 52 44 49 00 00 00 49【校验和】 IP地址 长度4字节，
                //分别存储的8位无符号整数分别对应设备IPv4地址的4个字段，
                //如 0A 00 00 0B 对应IP地 址 10.0.0.11 下位机将把 IP地址 写入内置的Flash中，重新上电后自动载入
                string setIp = value;
                string[] ipArr = setIp.Split('.');
                List<byte> tempBytes = new List<byte>();
                for (int i = 0; i < ipArr.Length; i++)
                {
                    tempBytes.Add(Convert.ToByte(ipArr[i]));
                }
                byte[] cmdMajor = { 0x57, 0x52, 0x44, 0x49 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x49 };
                byte[] cmdData = tempBytes.ToArray();
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
            }
        }
        public virtual int InstrumentNetworkPort
        {
            get
            {
                int port = -1;
                if (this.IsOnline == false)
                {
                    return port;
                }
                byte[] cmdMajor = { 0x52, 0x44, 0x44, 0x49 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x50 };
                byte[] cmdData = null;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);
                byte[] rbytes = new byte[2];
                rbytes[0] = dataBytes[1];
                rbytes[1] = dataBytes[0];
                port = (int)Converter.BytesToInt(rbytes, 0, 2);
                return port;
            }
            set
            {
                if (this.IsOnline == false) return;
                int setPort = value;
                var bytes = Converter.IntToBytes(setPort);
                byte[] rbytes = new byte[2];
                rbytes[0] = bytes[1];
                rbytes[1] = bytes[0];
                byte[] cmdMajor = { 0x57, 0x52, 0x44, 0x49 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x50 };
                byte[] cmdData = rbytes;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
            }
        }
        public virtual int PivAvailableTestChannelCount
        {
            get
            {
                int count = -1;
                if (this.IsOnline == false)
                {
                    return count;
                }
                byte[] cmdMajor = { 0x52, 0x44, 0x43, 0x43 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x54 };
                byte[] cmdData = null;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);
                count = (int)Converter.BytesToInt(dataBytes, 0, 1);
                return count;
            }
        }
        public virtual int PivTestChannelPDCount
        {
            get
            {
                int count = -1;
                if (this.IsOnline == false)
                {
                    return count;
                }
                byte[] cmdMajor = { 0x52, 0x44, 0x43, 0x43 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x50 };
                byte[] cmdData = null;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);
                count = (int)Converter.BytesToInt(dataBytes, 0, 1);
                return count;
            }
        }



        #region LD functions
        float _currentSetPoint_A = 0f;
        /// <summary>
        /// Set LD current in 'A'
        /// </summary>
        public virtual float CurrentSetpoint_A
        {
            get
            {

                return this._currentSetPoint_A;
            }
            set
            {
                //上位机发送： AA 0D 00 53 54 50 56 00 00 00 49【LD驱动电流】【校验和】
                //下位机返回： AA 09 00 53 54 50 56 00 00 00 49【校验和】 
                //LD驱动电流 长度4字节，
                //以Intel Little-Endian方式存储的32位单精度浮点数对应驱动电流mA值 
                //注意：==激光器驱动电流均为非负值== 
                this._currentSetPoint_A = value;
                if (this.IsOnline == false) return;
                float curr_mA = value * 1000.0f;
                var bytes = BitConverter.GetBytes(curr_mA);
                //Array.Reverse(bytes);
                byte[] cmdMajor = { 0x53, 0x54, 0x50, 0x56 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x49 };
                byte[] cmdData = bytes;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
            }
        }
        private bool _sOutputEnalbe = false;
        public virtual bool IsOutputEnable
        {
            get
            {
                if (IsOnline == false)
                {
                    return _sOutputEnalbe;
                }
                byte[] cmdMajor = { 0x52, 0x44, 0x50, 0x56 };
                byte[] cmdMinor = { 0x00, 0x00, 0x01, 0x53 };
                byte[] cmdData = null;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);
                if (dataBytes[0] == 0x00)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                if (IsOnline == false)
                {
                    _sOutputEnalbe = value;
                    return;
                }

                byte[] cmdMajor = { 0x53, 0x54, 0x50, 0x56 };
                byte[] cmdMinor = { 0x00, 0x00, 0x01, 0x53 };
                byte[] cmdData = value == true ? new byte[] { 0x01 } : new byte[] { 0x00 };
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);

            }

        }
        /// <summary>
        /// Read LD current in 'A'
        /// </summary>
        /// <returns></returns>
        public virtual float ReadCurrent_A()
        {
            float curr_A = -999.0f;
            if (this.IsOnline == false)
            {
                return curr_A;
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x50, 0x56 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x49 };
            byte[] cmdData = null;
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            curr_A = BitConverter.ToSingle(dataBytes, 0) / 1000.0f;
            return curr_A;
        }
        /// <summary>
        /// Read LD voltage in 'V'
        /// </summary>
        /// <returns></returns>
        public virtual double ReadVoltage_V()
        {
            //读取激光器偏置电压测量值 
            //上位机发送： AA 09 00 52 44 50 56 00 00 00 56【校验和】
            //下位机返回： AA 0D 00 52 44 50 56 00 00 00 56【LD偏置电压】【校验和】
            //LD偏置电压 长度4字节，
            //以Intel Little-Endian方式存储的32位单精度浮点数对应激光器偏置电压V值 
            float volt_V = -999.0f;
            if (this.IsOnline == false)
            {
                return volt_V;
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x50, 0x56 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x56 };
            byte[] cmdData = null;
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            volt_V = BitConverter.ToSingle(dataBytes, 0);
            return volt_V;
        }
        #endregion
        #region EA functions
        float _voltageSetPoint_EA_V = 0f;
        /// <summary>
        /// Set EA voltage in 'V'
        /// </summary>
        public virtual float VoltageSetpoint_EA_V
        {
            get
            {

                return this._voltageSetPoint_EA_V;
            }
            set
            {
                //设置EA偏置电压目标值 
                //上位机发送： AA 0D 00 53 54 50 56 00 00 00 45【EA偏置电压】【校验和】
                //下位机返回： AA 09 00 53 54 50 56 00 00 00 45【校验和】
                //EA偏置电压 长度4字节，
                //以Intel Little-Endian方式存储的32位单精度浮点数对应EA偏置电压V值
                //注意：==EA偏置电压均为非正值==
                this._voltageSetPoint_EA_V = value;
                if (this.IsOnline == false) return;
                float volt_ea_V = value;
                var bytes = BitConverter.GetBytes(volt_ea_V);
                //Array.Reverse(bytes);
                byte[] cmdMajor = { 0x53, 0x54, 0x50, 0x56 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x45 };
                byte[] cmdData = bytes;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
            }
        }
        /// <summary>
        /// Read EA current in 'A'
        /// </summary>
        /// <returns></returns>
        public virtual float ReadCurrent_EA_A()
        {
            //读取EA偏置电流测量值 
            //上位机发送： AA 09 00 52 44 50 56 00 00 00 65【校验和】
            //下位机返回： AA 0D 00 52 44 50 56 00 00 00 65【EA偏置电流】【校验和】
            //EA偏置电流 长度4字节，
            //以Intel Little-Endian方式存储的32位单精度浮点数对应EA偏置电流mA值 
            float curr_A = -999.0f;
            if (this.IsOnline == false)
            {
                return curr_A;
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x50, 0x56 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x65 };
            byte[] cmdData = null;
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            curr_A = BitConverter.ToSingle(dataBytes, 0) / 1000.0f;
            return curr_A;
        }
        /// <summary>
        /// Read EA voltage in 'V'
        /// </summary>
        /// <returns></returns>
        public virtual float ReadVoltage_EA_V()
        {
            //上位机发送： AA 09 00 52 44 50 56 00 00 00 45【校验和】 
            //下位机返回： AA 0D 00 52 44 50 56 00 00 00 45【EA偏置电压】【校验和】 
            //EA偏置电压 长度4字节，
            //以Intel Little-Endian方式存储的32位单精度浮点数对应EA偏置电压V值 
            float volt_V = -999.0f;
            if (this.IsOnline == false)
            {
                return volt_V;
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x50, 0x56 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x45 };
            byte[] cmdData = null;
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            volt_V = BitConverter.ToSingle(dataBytes, 0);
            return volt_V;
        }
        #endregion
        #region PD functions
        float _voltageSetPoint_PD_V = 0f;
        /// <summary>
        /// Set EA voltage in 'V'
        /// </summary>
        public virtual float VoltageSetpoint_PD_V
        {
            get
            {

                return this._voltageSetPoint_PD_V;
            }
            set
            {
                //设置PD偏置电压目标值 
                //上位机发送： AA 0D 00 53 54 50 56 00 00 00 70【PD偏置电压】【校验和】
                //下位机返回： AA 09 00 53 54 50 56 00 00 00 70【校验和】
                //PD偏置电压 长度4字节，
                //以Intel Little-Endian方式存储的32位单精度浮点数对应PD偏置电压V值 
                //注意：==PD偏置电压均为非正值==
                this._voltageSetPoint_PD_V = value;
                if (this.IsOnline == false) return;
                float volt_pd_V = value;
                var bytes = BitConverter.GetBytes(volt_pd_V);
                //Array.Reverse(bytes);
                byte[] cmdMajor = { 0x53, 0x54, 0x50, 0x56 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x70 };
                byte[] cmdData = bytes;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
            }
        }

        public virtual float ReadVoltage_PD_V()
        {
            //读取PD偏置电压测量值 
            //上位机发送： AA 09 00 52 44 50 56 00 00 00 70【校验和】 
            //下位机返回： AA 0D 00 52 44 50 56 00 00 00 70【PD偏置电压】【校验和】
            //PD偏置电压 长度4字节，
            //以Intel Little-Endian方式存储的32位单精度浮点数对应PD偏置电压V值 
            float volt_V = -999.0f;
            if (this.IsOnline == false)
            {
                return volt_V;
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x50, 0x56 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x70 };
            byte[] cmdData = null;
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            volt_V = BitConverter.ToSingle(dataBytes, 0);
            return volt_V;
        }

        public virtual GolightSource_PD_TEST_CHANNEL_RANGE GetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL pdTestChannel)
        {
            //读取PD光电流检测档位 
            //上位机发送：AA 09 00 52 44 4D 56 00 00【PD序号】47【校验和】
            //下位机返回：AA 0A 00 52 44 4D 56 00 00【PD序号】47【档位序号】【校验和】
            //PD序号 长度1字节，存储的8位无符号整数，取值与PD光电流检测通道对应关系如下：
            //0x01 ：
            //第1个PD光电流测量通道
            //0x02 ：
            //第2个PD光电流测量通道
            //档位序号 长度1字节，存储的8位无符号整数，取值与档位对应关系如下：
            //0x01 ：
            //档位1
            //0x02 ：
            //档位2 
            if (this.IsOnline == false)
            {
                return GolightSource_PD_TEST_CHANNEL_RANGE.Range1;
            }
            byte chByte = (byte)pdTestChannel;
            byte[] cmdMajor = { 0x52, 0x44, 0x4d, 0x56 };
            byte[] cmdMinor = { 0x00, 0x00, chByte, 0x47 };
            byte[] cmdData = null;
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            return (GolightSource_PD_TEST_CHANNEL_RANGE)dataBytes[0];
        }
        public virtual void SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL pdTestChannel, GolightSource_PD_TEST_CHANNEL_RANGE channelRange)
        {
            //设置PD光电流检测档位 
            //上位机发送： AA 0A 00 53 54 4D 56 00 00【PD序号】47【档位序号】【校验和】
            //下位机返回： AA 09 00 53 54 4D 56 00 00【PD序号】47【校验和】 
            //PD序号 长度1字节，
            //存储的8位无符号整数，取值与PD光电流检测通道对应关系如下：
            //0x01 ：第1个PD光电流测量通道
            //0x02 ：第2个PD光电流测量通道
            //档位序号 长度1字节，存储的8位无符号整数，取值与档位对应关系如下：
            //0x01 ：档位1 
            //0x02 ：档位2
            if (this.IsOnline == false)
            {
                return;
            }
            byte chByte = (byte)pdTestChannel;
            byte[] cmdMajor = { 0x53, 0x54, 0x4d, 0x56 };
            byte[] cmdMinor = { 0x00, 0x00, chByte, 0x47 };
            byte[] cmdData = { (byte)channelRange };
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
        }
        public virtual float ReadCurrent_PD_A(GolightSource_PD_TEST_CHANNEL pdTestChannel)
        {
            //读取PD光电流测量值 
            //上位机发送： AA 09 00 52 44 50 56 00 00【PD序号】50【校验和】
            //下位机返回： AA 0D 00 52 44 50 56 00 00【PD序号】50【PD光电流】【校验和】 
            //PD序号 长度1字节，存储的8位无符号整数，取值与PD光电流检测通道对应关系如下：
            //0x01 ：第1个PD光电流测量通道
            //0x02 ：第2个PD光电流测量通道 
            //PD光电流 长度4字节，存储的32位单精度浮点数对应目标PD的光电流mA值 
            float curr_A = -999.0f;
            if (this.IsOnline == false)
            {
                return curr_A;
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x50, 0x56 };
            byte[] cmdMinor = { 0x00, 0x00, (byte)pdTestChannel, 0x50 };
            byte[] cmdData = null;
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            curr_A = BitConverter.ToSingle(dataBytes, 0) / 1000.0f;
            return curr_A;
        }
        #endregion
        #region Sweep functions
        //新增LIV脉冲模式  Irwin
        public virtual void Sweep_LD_PD_Pulse(float startCurrent_mA,
                            float stepCurrent_mA,
                            float stopCurrent_mA,
                            float compliaceVoltage_V,
                            float pdBiasVoltage_V,
                            float pdComplianceCurrent_mA,
                            int Pulsewidth,//脉冲宽度
                            int Pulseperiod,//脉冲周期
                            double K2400_NPLC)
        {
            if (this.IsOnline == false) return;
            this.IsOutputEnable = true;
            this.VoltageSetpoint_PD_V = pdBiasVoltage_V;
            this.CurrentSetpoint_A = startCurrent_mA / 1000.0f;
            if (pdComplianceCurrent_mA <= 0.08)
            {
                this.SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH1, GolightSource_PD_TEST_CHANNEL_RANGE.Range2);
            }
            else
            {
                this.SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH1, GolightSource_PD_TEST_CHANNEL_RANGE.Range1);
            }
            //启动激光器脉冲PIV测试 
            //上位机发送：
            //AA 31 00 53 54 4D 54 00 00 00 50【LD驱动电流起始值】【LD驱动电流扫描步长】【LD驱动电流上限】【LD偏置电压上限】
            //【第1通道PD光电流上限】【第2通道PD光电流上限】【第3通道PD光电流上限】【第4通道PD光电流上限】【脉冲宽度】【脉冲周期】【校验和】

            byte[] cmdMajor = { 0x53, 0x54, 0x4d, 0x54 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x50 };
            byte[] cmdData = null;
            byte[] tempBytes = null;
            List<byte> tempCmdData = new List<byte>();
            //LD驱动电流起始值 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流起始 mA值，例如0.000 
            tempBytes = BitConverter.GetBytes(startCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //驱动电流扫描步长 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流扫描步 长mA值，例如0.450
            tempBytes = BitConverter.GetBytes(stepCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //LD驱动电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流上限mA 值，例如450.000 
            tempBytes = BitConverter.GetBytes(stopCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //LD偏置电压上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应对应激光器偏置电压上 限V值，例如2.500
            tempBytes = BitConverter.GetBytes(compliaceVoltage_V);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第1通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第1通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第2通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第2通道PD光电流 上限mA值，例如20.000 
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);

            //第3通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第3通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第4通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第4通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);

            //脉冲宽度 长度4字节,存储的32位无符号整数最小值5000，单位us,表示脉冲宽度
            tempBytes = BitConverter.GetBytes(Pulsewidth);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //脉冲周期 长度4字节,存储的32位无符号整数最小值10000，单位us,表示脉冲周期
            tempBytes = BitConverter.GetBytes(Pulseperiod);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);

            cmdData = tempCmdData.ToArray();
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);

            double delaytime_ms = 0;
            delaytime_ms = (stopCurrent_mA - startCurrent_mA) / stepCurrent_mA * 60 * 0.07;
            Thread.Sleep((int)delaytime_ms);
        }
        public virtual void Sweep_LD_PD(float startCurrent_mA,
                            float stepCurrent_mA,
                            float stopCurrent_mA,
                            float compliaceVoltage_V,
                            float pdBiasVoltage_V,
                            float pdComplianceCurrent_mA,
                            double K2400_NPLC)
        {
            if (this.IsOnline == false) return;
            this.IsOutputEnable = true;
            this.VoltageSetpoint_PD_V = pdBiasVoltage_V;
            this.CurrentSetpoint_A = startCurrent_mA / 1000.0f;
            if (pdComplianceCurrent_mA <= 0.08)
            {
                this.SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH1, GolightSource_PD_TEST_CHANNEL_RANGE.Range2);
            }
            else
            {
                this.SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH1, GolightSource_PD_TEST_CHANNEL_RANGE.Range1);
            }
            //启动激光器PIV测试 
            //上位机发送： AA 21 00 53 54 4D 54 00 00 00 4C【LD驱动电流起始值】【LD驱动电流扫描步长】【LD驱动电流上 限】【LD偏置电压上限】【第1通道PD光电流上限】【第2通道PD光电流上限】【校验和】

            byte[] cmdMajor = { 0x53, 0x54, 0x4d, 0x54 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x4c };
            byte[] cmdData = null;
            byte[] tempBytes = null;
            List<byte> tempCmdData = new List<byte>();
            //LD驱动电流起始值 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流起始 mA值，例如0.000 
            tempBytes = BitConverter.GetBytes(startCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //驱动电流扫描步长 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流扫描步 长mA值，例如0.450
            tempBytes = BitConverter.GetBytes(stepCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //LD驱动电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流上限mA 值，例如450.000 
            tempBytes = BitConverter.GetBytes(stopCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //LD偏置电压上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应对应激光器偏置电压上 限V值，例如2.500
            tempBytes = BitConverter.GetBytes(compliaceVoltage_V);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第1通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第1通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第2通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第2通道PD光电流 上限mA值，例如20.000 
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);

            cmdData = tempCmdData.ToArray();
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);

            double delaytime_ms = 0;
            delaytime_ms = (stopCurrent_mA - startCurrent_mA) / stepCurrent_mA * 60 * 0.07;
            Thread.Sleep((int)delaytime_ms);
        }
        public virtual void Sweep_LD_MPD_PD(float startCurrent_mA,
                    float stepCurrent_mA,
                    float stopCurrent_mA,
                    float compliaceVoltage_V,
                    float mpdBiasVoltage_V,
                    float mpdComplianceCurrent_mA,
                    float pdBiasVoltage_V,
                    float pdComplianceCurrent_mA,
                    double K2400_NPLC)
        {
            if (this.IsOnline == false) return;
            this.IsOutputEnable = true;
            this.VoltageSetpoint_PD_V = mpdBiasVoltage_V;
            this.CurrentSetpoint_A = startCurrent_mA / 1000.0f;
            if (mpdComplianceCurrent_mA <= 0.08)
            {
                this.SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH2, GolightSource_PD_TEST_CHANNEL_RANGE.Range2);
            }
            else
            {
                this.SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH2, GolightSource_PD_TEST_CHANNEL_RANGE.Range1);
            }
            //启动激光器PIV测试 
            //上位机发送： AA 21 00 53 54 4D 54 00 00 00 4C【LD驱动电流起始值】【LD驱动电流扫描步长】【LD驱动电流上 限】【LD偏置电压上限】【第1通道PD光电流上限】【第2通道PD光电流上限】【校验和】

            byte[] cmdMajor = { 0x53, 0x54, 0x4d, 0x54 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x4c };
            byte[] cmdData = null;
            byte[] tempBytes = null;
            List<byte> tempCmdData = new List<byte>();
            //LD驱动电流起始值 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流起始 mA值，例如0.000 
            tempBytes = BitConverter.GetBytes(startCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //驱动电流扫描步长 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流扫描步 长mA值，例如0.450
            tempBytes = BitConverter.GetBytes(stepCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //LD驱动电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流上限mA 值，例如450.000 
            tempBytes = BitConverter.GetBytes(stopCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //LD偏置电压上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应对应激光器偏置电压上 限V值，例如2.500
            tempBytes = BitConverter.GetBytes(compliaceVoltage_V);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第1通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第1通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第2通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第2通道PD光电流 上限mA值，例如20.000 
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);

            cmdData = tempCmdData.ToArray();
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);

            double delaytime_ms = 0;

            delaytime_ms = (stopCurrent_mA - startCurrent_mA) / stepCurrent_mA * 60 * 0.07;

            Thread.Sleep((int)delaytime_ms);
        }
        public virtual void Sweep_LD_EA_PD(float startCurrent_mA,
            float stepCurrent_mA,
            float stopCurrent_mA,
            float compliaceVoltage_V,
            float eaVoltage_V,
            float eaComplianceCurrent_mA,
            float pdBiasVoltage_V,
            float pdComplianceCurrent_mA,
            double K2400_NPLC)
        {
            if (this.IsOnline == false) return;
            this.IsOutputEnable = true;
            this.VoltageSetpoint_PD_V = pdBiasVoltage_V;
            this.CurrentSetpoint_A = startCurrent_mA / 1000.0f;
            this.VoltageSetpoint_EA_V = eaVoltage_V;
            if (pdComplianceCurrent_mA <= 0.08)
            {
                this.SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH1, GolightSource_PD_TEST_CHANNEL_RANGE.Range2);
            }
            else
            {
                this.SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL.CH1, GolightSource_PD_TEST_CHANNEL_RANGE.Range1);
            }
            //启动激光器PIV测试 
            //上位机发送： AA 21 00 53 54 4D 54 00 00 00 4C【LD驱动电流起始值】【LD驱动电流扫描步长】【LD驱动电流上 限】【LD偏置电压上限】【第1通道PD光电流上限】【第2通道PD光电流上限】【校验和】

            byte[] cmdMajor = { 0x53, 0x54, 0x4d, 0x54 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x4c };
            byte[] cmdData = null;
            byte[] tempBytes = null;
            List<byte> tempCmdData = new List<byte>();
            //LD驱动电流起始值 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流起始 mA值，例如0.000 
            tempBytes = BitConverter.GetBytes(startCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //驱动电流扫描步长 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流扫描步 长mA值，例如0.450
            tempBytes = BitConverter.GetBytes(stepCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //LD驱动电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流上限mA 值，例如450.000 
            tempBytes = BitConverter.GetBytes(stopCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //LD偏置电压上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应对应激光器偏置电压上 限V值，例如2.500
            tempBytes = BitConverter.GetBytes(compliaceVoltage_V);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第1通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第1通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第2通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第2通道PD光电流 上限mA值，例如20.000 
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);

            cmdData = tempCmdData.ToArray();
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);

            double delaytime_ms = 0;
            delaytime_ms = (stopCurrent_mA - startCurrent_mA) / stepCurrent_mA * 60 * 0.07;
            Thread.Sleep((int)delaytime_ms);
        }

        public virtual float[] FetchLDSweepData(SweepDataType dataType)
        {
            //读取激光器PIV测试结果 
            //上位机发送： AA 09 00 52 44 54 52 00 00【ADC序号】4C【校验和】 
            if (this.IsOnline == false) return null;
            if (dataType == SweepDataType.EA_Drive_Current_mA ||
                dataType == SweepDataType.EA_Drive_Voltage_V)
            {
                throw new Exception("Nonsupport data type!");
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x54, 0x52 };
            byte[] cmdMinor = { 0x00, 0x00, (byte)dataType, 0x4c };
            byte[] cmdData = null;

            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            byte[] singleDataBytes = new byte[4];
            List<float> dataList = new List<float>();
            for (int i = 0; i < dataBytes.Length; i += 4)
            {
                Array.Copy(dataBytes, i, singleDataBytes, 0, 4);
                var tempVal = BitConverter.ToSingle(singleDataBytes, 0);
                dataList.Add(tempVal);
            }
            return dataList.ToArray();
        }
        public virtual void SweepEA(float ldCurrent_mA,
                            float ldComplianceVoltage_V,
                            float startVoltage_V,
                            float stepVoltage_V,
                            float stopVoltage_V,
                            float eaComplianceCurrent_mA,
                            float pdBiasVoltage_V,
                            float pd_complianceCurrent_mA,
                            double K2400_NPLC)
        {
            if (this.IsOnline == false) return;
            this.IsOutputEnable = true;
            this.CurrentSetpoint_A = ldCurrent_mA / 1000.0f;
            this.VoltageSetpoint_EA_V = startVoltage_V;
            //启动EA PIV测试 
            //上位机发送： AA 21 00 53 54 4D 54 00 00 00 45【EA偏置电压起始值】【EA偏置电压扫描步长】【EA偏置电压下 限】【EA偏置电流下限】【第1通道PD光电流下限】【第2通道PD光电流下限】【校验和】
            byte[] cmdMajor = { 0x53, 0x54, 0x4d, 0x54 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x45 };
            byte[] cmdData = null;
            byte[] tempBytes = null;
            List<byte> tempCmdData = new List<byte>();
            //EA偏置电压起始值 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应EA偏置电压起始V值， 例如0.000
            tempBytes = BitConverter.GetBytes(startVoltage_V);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //EA偏置电压扫描步长 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应EA偏置电压扫描步 长V值，例如-0.010 
            tempBytes = BitConverter.GetBytes(stepVoltage_V);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //EA偏置电压下限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应EA偏置电压下限V值，例 如-5.000 
            tempBytes = BitConverter.GetBytes(stopVoltage_V);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //EA偏置电流下限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应对应EA偏置电流下限mA 值，例如-100.000 
            eaComplianceCurrent_mA = -Math.Abs(eaComplianceCurrent_mA);
            tempBytes = BitConverter.GetBytes(eaComplianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第1通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第1通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pd_complianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //第2通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第2通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pd_complianceCurrent_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);

            cmdData = tempCmdData.ToArray();
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);

            double delaytime_ms = 0;

            delaytime_ms = (stopVoltage_V - startVoltage_V) / stepVoltage_V * 60 * 0.07;

            Thread.Sleep((int)delaytime_ms);
        }

        /// <summary>
        /// 读取EA PIV测试结果
        ///上位机发送： AA 09 00 52 44 54 52 00 00【ADC序号】45【校验和】
        ///下位机返回： AA【包长度】52 44 54 52 00 00【ADC序号】45【测试结果序列】【校验和】
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public virtual float[] FetchEASweepData(SweepDataType dataType)
        {
            if (this.IsOnline == false) return null;
            if (dataType == SweepDataType.LD_Drive_Current_mA ||
                dataType == SweepDataType.LD_Drive_Voltage_V)
            {
                throw new Exception("Nonsupport data type!");
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x54, 0x52 };
            byte[] cmdMinor = { 0x00, 0x00, (byte)dataType, 0x45 };
            //读PD值用0x4C,其他的用0x45?
            if (dataType == SweepDataType.PD_Ch1_Current_mA || 
                dataType == SweepDataType.PD_Ch2_Current_mA ||
                dataType == SweepDataType.PD_Ch3_Current_mA ||
                dataType == SweepDataType.PD_Ch4_Current_mA)
            {
                cmdMinor = new byte[] { 0x00, 0x00, (byte)dataType, 0x4c };
            }

            byte[] cmdData = null;

            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            byte[] singleDataBytes = new byte[4];
            List<float> dataList = new List<float>();
            for (int i = 0; i < dataBytes.Length; i += 4)
            {
                Array.Copy(dataBytes, i, singleDataBytes, 0, 4);
                var tempVal = BitConverter.ToSingle(singleDataBytes, 0);
                dataList.Add(tempVal);
            }
            return dataList.ToArray();
        }


        public virtual void StopSweeping()
        {
            //中止PIV测试 
            //上位机发送： AA 09 00 53 54 4D 54 00 00 00 58【校验和】 
            //下位机返回： AA 09 00 53 54 4D 54 00 00 00 58【校验和】
            //在PIV测试过程中接收到该命令，下位机将立即中止当前PIV测试过程 
            if (this.IsOnline == false)
            {
                return;
            }
            byte[] cmdMajor = { 0x53, 0x54, 0x4d, 0x54 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x58 };
            byte[] cmdData = null;
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
        }
        public virtual bool CheckIsSweeping(ref int count)
        {
            if (this.IsOnline == false)
            {
                return false;
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x54, 0x43 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x00 };
            byte[] cmdData = null;
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            if (dataBytes[0] == 0x01)
            {
                count = dataBytes[1];
                return true;
            }
            else if (dataBytes[0] == 0x00)
            {
                count = dataBytes[1];
                return false;
            }
            else
            {
                return false;
            }
        }

        public virtual void SetupSMU_EA(double voltageSetpoint_V, double complianceCurrent_mA)
        {
            this.IsOutputEnable = true;
            this.VoltageSetpoint_EA_V = (float)voltageSetpoint_V;
        }

        public virtual void SetupSMU_PD(double voltageSetpoint_V, double complianceCurrent_mA)
        {
            this.VoltageSetpoint_PD_V = (float)voltageSetpoint_V;
        }

        public virtual void SetupSMU_LD(double currentSetpoint_mA, double complianceVoltage_V)
        {
            this.IsOutputEnable = true;
            this.CurrentSetpoint_A = (float)currentSetpoint_mA / 1000.0f;
        }

        public virtual void SetupSMU_MPD(double voltageSetpoint_V, double complianceCurrent_mA)
        {
            this.VoltageSetpoint_PD_V = (float)voltageSetpoint_V;
        }

        public virtual void SweepLD(float startCurrent_mA, float stepCurrent_mA, float ldCurrentUpperLimit_mA, float ldVoltageUpperLimit_V, bool enableEAOutput, float eaVoltage_V, float pdTestCh1_complianceCurrent_mA, float pdTestCh2_complianceCurrent_mA, float pdTestCh3_complianceCurrent_mA, float pdTestCh4_complianceCurrent_mA,
                            double K2400_NPLC)
        {
            throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            // throw new NotImplementedException();
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }

        public virtual bool IsSweeping
        {
            get
            {
                //读取PIV测试进度 
                //上位机发送： AA 09 00 52 44 54 43 00 00 00 00【校验和】
                //下位机返回： AA 0C 00 52 44 54 43 00 00 00 00【测试进行标志】【测试已完成次数】【校验和】
                //测试进行标志 长度1字节，存储的8位无符号整数，
                //0x01 表示测试进行中，
                //0x00 表示测试完成或中止 
                //测试已完成次数 长度2字节，以Intel Little-Endian方式存储的16位无符号整数，对应当前PIV测试已完成的次 数
                //上位机应在 测试进行标志 为0x00后再读取PIV测试结果 

                if (this.IsOnline == false)
                {
                    return false;
                }
                byte[] cmdMajor = { 0x52, 0x44, 0x54, 0x43 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x00 };
                byte[] cmdData = null;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);
                if (dataBytes[0] == 0x01)
                {
                    return true;
                }
                else if (dataBytes[0] == 0x00)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion


        float _voltageSetpoint_V;
        public virtual float VoltageSetpoint_V
        {
            get
            {
                return this._voltageSetpoint_V;
            }
            set
            {
                this._voltageSetpoint_V = value;
                if (this.IsOnline == false) return;
                float volt_V = value;
                var bytes = BitConverter.GetBytes(volt_V);
                Array.Reverse(bytes);
                byte[] cmdMajor = { 0x53, 0x54, 0x50, 0x56 };
                byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x49 };
                byte[] cmdData = bytes;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
            }
        }
    }
}