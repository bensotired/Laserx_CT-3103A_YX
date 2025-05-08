using System;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_BurnInInstruments
{

    public class GolightSourceMeter_PIV450_002 : GolightSourceMeterBase, ISourceMeter_Golight
    {

        public GolightSourceMeter_PIV450_002(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {

        }
        #region Sweep functions
        public override void Sweep_LD_PD(float startCurrent_mA,
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
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            tempCmdData.AddRange(tempBytes);
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            tempCmdData.AddRange(tempBytes);

            cmdData = tempCmdData.ToArray();
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
            double delaytime_ms = 0;

            delaytime_ms = (stopCurrent_mA - startCurrent_mA) / stepCurrent_mA * 60 * 0.08;

            Thread.Sleep((int)delaytime_ms);
        }
        public override void Sweep_LD_MPD_PD(float startCurrent_mA,
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

            tempCmdData.AddRange(tempBytes);
            //驱动电流扫描步长 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流扫描步 长mA值，例如0.450
            tempBytes = BitConverter.GetBytes(stepCurrent_mA);

            tempCmdData.AddRange(tempBytes);
            //LD驱动电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应激光器驱动电流上限mA 值，例如450.000 
            tempBytes = BitConverter.GetBytes(stopCurrent_mA);

            tempCmdData.AddRange(tempBytes);
            //LD偏置电压上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应对应激光器偏置电压上 限V值，例如2.500
            tempBytes = BitConverter.GetBytes(compliaceVoltage_V);

            tempCmdData.AddRange(tempBytes);
            //第1通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第1通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            tempCmdData.AddRange(tempBytes);
            //第2通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第2通道PD光电流 上限mA值，例如20.000 
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            tempCmdData.AddRange(tempBytes);
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            tempCmdData.AddRange(tempBytes);
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            tempCmdData.AddRange(tempBytes);

            cmdData = tempCmdData.ToArray();
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
    
            double delaytime_ms = 0;
            delaytime_ms = (stopCurrent_mA - startCurrent_mA) / stepCurrent_mA * 60 * 0.08;
            Thread.Sleep((int)delaytime_ms);
        }
        public override void Sweep_LD_EA_PD(float startCurrent_mA,
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

            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            tempCmdData.AddRange(tempBytes);
            tempBytes = BitConverter.GetBytes(pdComplianceCurrent_mA);
            tempCmdData.AddRange(tempBytes);

            cmdData = tempCmdData.ToArray();
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);
 
            double delaytime_ms = 0;

            delaytime_ms = (stopCurrent_mA - startCurrent_mA) / stepCurrent_mA * 60 * 0.08;

            Thread.Sleep((int)delaytime_ms);
        }
     
        public override void SweepEA(float ldCurrent_mA,
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

            //第3通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第1通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pd_complianceCurrent_mA);
            tempCmdData.AddRange(tempBytes);
            //第4通道PD光电流上限 长度4字节，以Intel Little-Endian方式存储的32位单精度浮点数对应第2通道PD光电流 上限mA值，例如20.000
            tempBytes = BitConverter.GetBytes(pd_complianceCurrent_mA);
            tempCmdData.AddRange(tempBytes);


            cmdData = tempCmdData.ToArray();
            var cmd = this.BuildCommand(cmdMajor, cmdMinor, cmdData);
            var ret = this._chassis.Query(cmd, Delay_ms);

            //下位机返回： AA 09 00 53 54 4D 54 00 00 00 45【校验和】
            //下位机以 EA偏置电压起始值 为起点，以 EA偏置电压扫描步长 为递增步长，依次设置 EA偏置电压 ，
            //然后测量 EA 偏置电压 、 EA偏置电流 、 第1通道PD光电流 和 第2通道PD光电流 ，
            //直至上述任一测量值达到或超过预设允许范 围 测试完成后EA偏置电流归零
            //若在PIV测试过程中接收到该命令，下位机将放弃当前PIV测试过程，
            //按照上述参数启动EA PIV测试 
            //注意：==( EA偏置电压下限 - EA偏置电压起始值 ) / EA偏置电压扫描步长 对应的预设扫描步数应不大于1001== 

            //20201023 上位机的延迟时间
            //（stop - start)/ step * average_count * 80us，对于0~100mA范围，0.1mA步进，平均６０次，大约是４.８ｓ

            //       A(2PD), 基础时间是70us
            //       B(4PD),基础时间是80us

            double delaytime_ms = 0;

            delaytime_ms = (stopVoltage_V - startVoltage_V) / stepVoltage_V * 60 * 0.08;

            Thread.Sleep((int)delaytime_ms);
        }
        #endregion
        public override float[] FetchLDSweepData(SweepDataType dataType)
        {
            //读取激光器PIV测试结果 
            //上位机发送： AA 09 00 52 44 54 52 00 00【ADC序号】4C【校验和】 
            if (this.IsOnline == false) return null;
            //if (dataType == SweepDataType.EA_Drive_Current_mA ||
            //    dataType == SweepDataType.EA_Drive_Voltage_V)
            //{
            //    throw new Exception("Nonsupport data type!");
            //}
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
    }
}


