using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class GolightSourceMeter_PIV200_002 : GolightSourceMeterBase, ISourceMeter_Golight
    {

        public GolightSourceMeter_PIV200_002(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
           
        }
        bool _sOutputEnalbe;
        public override bool IsOutputEnable
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
                byte[] cmdMinor = { 0x00, 0x00, 0x03, 0x53 };
                byte[] cmdData = value == true ? new byte[] { 0x01 } : new byte[] { 0x00 };
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
                var dataBytes = this.GetCommonReturnData(ret);

            }

        }

        float _voltageSetPoint_V;
        public override float VoltageSetpoint_V
        {
            get
            {

                return this._voltageSetPoint_V;
            }
            set
            {
                //设置APD偏置电压目标值 
                //上位机发送： AA 0D 00 53 54 50 56 00 00【驱动序号】56【电压目标值】【校验和】
                //下位机返回： AA 09 00 53 54 50 56 00 00【驱动序号】56【校验和】
                //PD偏置电压 长度4字节，
                //以Intel Little-Endian方式存储的32位单精度浮点数对应PD偏置电压V值 
                //注意：==PD偏置电压均为非正值==
                this._voltageSetPoint_V = value;
                if (this.IsOnline == false) return;
                float volt_pd_V = value;
                var bytes = BitConverter.GetBytes(volt_pd_V);
                //Array.Reverse(bytes);
                byte[] cmdMajor = { 0x53, 0x54, 0x50, 0x56 };
                byte[] cmdMinor = { 0x00, 0x00, 0x03, 0x56 };
                byte[] cmdData = bytes;
                var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
                var ret = this._chassis.Query(cmd, Delay_ms);
            }
        }

        public override float ReadCurrent_A()
        {
            //上位机发送：AA 09 00 52 44 50 56 00 00【驱动序号】49【校验和】
            //下位机返回：AA 0D 00 52 44 50 56 00 00【驱动序号】49【电流测量值】【校验和】
            float curr_A = -999.0f;
            if (this.IsOnline == false)
            {
                return curr_A;
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x50, 0x56 };
            byte[] cmdMinor = { 0x00, 0x00, 0x83, 0x49 };
            byte[] cmdData = null;
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            var dataBytes = this.GetCommonReturnData(ret);
            curr_A = BitConverter.ToSingle(dataBytes, 0) / 1000.0f;
            return curr_A;
        }

        public void SweepVbr(int stepDelay_us, int averagingTimes, float voltageStart_V, float voltageStep_V, float voltageStop_V, float current_mA)
        {
            ///上位机发送：AA 21 00 53 54 4D 54 00 00 00 41【驱动步进延时】
            //【累加平均次数】【APD 偏置电压起始值】【APD 偏置电压扫描步长】
            //【APD 偏置电压上限】【APD 偏置电流上限】【校验和】
            //下位机返回：AA 09 00 53 54 4D 54 00 00 00 41【校验和】
            
            
            //下位机将立即启动APD IV 测试，直至APD 偏置电压或APD 偏置电流
            //测量值达到设定上限并返回常规模式
            if (this.IsOnline == false) return;
            this.IsOutputEnable = true;

            byte[] cmdMajor = { 0x53, 0x54, 0x4d, 0x54 };
            byte[] cmdMinor = { 0x00, 0x00, 0x00, 0x41 };
            byte[] cmdData = null;
            byte[] tempBytes = null;
            List<byte> tempCmdData = new List<byte>();
            //【驱动步进延时】长度4 字节，以Intel Little-Endian 方式存储的32
            //位无符号整数，对应设置APD 偏置步进电压后延时等待的时间，单位uS
            tempBytes = BitConverter.GetBytes(stepDelay_us);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //【累加平均次数】长度4 字节，以Intel Little-Endian 方式存储的32
            //位无符号整数，对应每次步进测量的累加平均次数
            tempBytes = BitConverter.GetBytes(averagingTimes);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //【APD 偏置电压起始值】长度4 字节，以Intel Little-Endian 方式存储
            //的32 位单精度浮点数，对应APD 偏置电压起始V 值，例如0.000
            tempBytes = BitConverter.GetBytes(voltageStart_V);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //【APD 偏置电压扫描步长】长度4 字节，以Intel Little-Endian 方式存
            //储的32 位单精度浮点数，对应APD 偏置电压扫描步长V 值，例如0.1
            tempBytes = BitConverter.GetBytes(voltageStep_V);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //【APD 偏置电压上限】长度4 字节，以Intel Little-Endian 方式存储的
            //32 位单精度浮点数，对应APD 偏置电压上限V 值，例如50.000
            tempBytes = BitConverter.GetBytes(voltageStop_V);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);
            //【APD 偏置电流上限】长度4 字节，以Intel Little-Endian 方式存储的
            //32 位单精度浮点数，对应对应APD 偏置电流上限mA 值，例如0.1
            tempBytes = BitConverter.GetBytes(current_mA);
            //Array.Reverse(tempBytes);
            tempCmdData.AddRange(tempBytes);

            cmdData = tempCmdData.ToArray();
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            double delaytime_ms = 0;

            delaytime_ms =Math.Abs( (voltageStop_V - voltageStart_V) / voltageStep_V )* stepDelay_us * averagingTimes * 2/ 1000.0;

            Thread.Sleep((int)delaytime_ms);

        }
        public float[] FetchAPDSweepData(SweepDataType dataType)
        {
            //上位机发送：AA 09 00 52 44 4D 54 00 00【驱动序号】52【校验
            //            和】
            //下位机返回：AA【包长度】52 44 4D 54 00 00【驱动序号】52【测试
            //结果序列】【校验和】
            //【驱动序号】长度1 字节，存储的8 位无符号整数中
            //0x01：激光器偏置电流，单位mA，仅对激光器PIV 测试有效
            //0x81：激光器偏置电压，单位V，仅对激光器PIV 测试有效
            //0x05：PD1 光电流，单位ｍＡ，仅对激光器PIV 测试有效
            //0x15：PD2 光电流，单位ｍＡ，仅对激光器PIV 测试有效
            //0x03：APD 偏置电压，单位V，仅对APD IV 测试有效
            //0x83：APD 偏置电流，单位mA，仅对APD IV 测试有效
            //【测试结果序列】长度为4×测试已完成次数字节，每4 字节以Intel
            //Little - Endian 方式存储的32 位单精度浮点数对应一个测试结果值
            if (this.IsOnline == false) return null;
            if (dataType != SweepDataType.APD_Voltage_V &&
                dataType != SweepDataType.APD_Current_mA)
            {
                throw new Exception("Nonsupport data type!");
            }
            byte[] cmdMajor = { 0x52, 0x44, 0x4D, 0x54 };
            byte[] cmdMinor = { 0x00, 0x00, (byte)dataType, 0x52 };

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


    }
}
