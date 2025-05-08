using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace SolveWare_BurnInInstruments //命名空间根据应用程序修改
{

    //采样电阻设定部分

    public static partial class LaserX_9078_Utilities
    {
        const int ResAdjusted_Ver = 0x105;    //支持电阻调节的FPGA硬件软件最低的版本

        //默认采样电阻不可调节
        static public bool[] ResIsAdjusted =new bool[1];

        /// <summary>
        /// 初始化SPI扩展接口（扩展接口上连接4个AD5293BRUZ-100）
        /// 通过复位芯片然后读取RDAC寄存器复位值的方式判断扩展板是否正常工作
        /// </summary>
        /// <param name="dev">设备编号</param>
        /// <returns></returns>
        static public int P9078_SPIExtInit(int dev)
        {
            int rc = 0, err = 0;

            //查询版本号
            uint[] devInfo = new uint[16];
            rc = LaserX_9078_Utilities.P9078_MotionGetDevInfo(dev, devInfo, 16);
            if (rc != 0)
            {
                string exMsg = $"控制卡ID[{dev}] 板卡信息读取失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                throw new Exception($"{exMsg}!");
            }
            else
            {
                string lblApiVer =
                    (devInfo[6] >> 24 & 0xFF).ToString() + "." +
                    (devInfo[6] >> 16 & 0xFF).ToString() + "." +
                    (devInfo[6] >> 8 & 0xFF).ToString() + "." +
                    (devInfo[6] >> 0 & 0xFF).ToString();
                string LblDriverVer =
                    (devInfo[5] >> 24 & 0xFF).ToString() + "." +
                    (devInfo[5] >> 16 & 0xFF).ToString() + "." +
                    (devInfo[5] >> 8 & 0xFF).ToString() + "." +
                    (devInfo[5] >> 0 & 0xFF).ToString();

                string LblLogicVer =
                    (devInfo[4] >> 8 & 0xFF).ToString() + "." +
                    (devInfo[4] >> 0 & 0xFF).ToString();

                string LblPeriodMin = (devInfo[12] * 0.000001).ToString() + "(ms)";
                string LblPeriodMax = (devInfo[13] * 0.000001).ToString() + "(ms)";
                string LblPeriodMean = (devInfo[14] * 0.000001).ToString() + "(ms)  " + devInfo[8].ToString() + " ns";
            }

            //当版本小于105的情况下, 不支持电阻设定
            if(devInfo[4]< ResAdjusted_Ver)
            {
                ResIsAdjusted[dev] = false;
                return (int)errcodevalue.Finish;
            }

            uint ver = 0;
            ushort[] TxCmd = new ushort[4] { 0, 0, 0, 0 };
            ushort[] RxCmd = new ushort[4] { 0, 0, 0, 0 };

            rc = P9078_MotionInd(dev, 0x810, ref ver);
            if (rc == 0)
            {
                ver = ver & 0xffff;
                if (ver < 0x0105)
                {
                    System.Diagnostics.Debug.Print(string.Format("PCIe-9078#{0} FPGA version: 0x{1:x}(expect: 0x0105)\n", dev, ver));
                    err += 1;
                    ResIsAdjusted[dev] = false;
                    return (int)errcodevalue.Err_ReadTimeout;
                }
            }
            else
            {
                System.Diagnostics.Debug.Print(string.Format("In SPIExitInit fails to check FPGA version\n"));
                err += 1;
                ResIsAdjusted[dev] = false;
                return rc;
            }

            //register 0x814 bit definition
            //bit[8] - SPI SS_N
            //bit[9] - SPI SCLK
            //bit[10] - SPI MOSI
            //bit[11] - SPI MISO(read only)

            //set SS_N high
            rc = P9078_MotionOutd(dev, 0x814, 0x1 << 8);
            DelayMicrosec(10);

            //bit[13:10] command code
            //0x0  NOP command. Do nothing.
            //0x1  Write contents of serial register data to RDAC
            //0x2  Read RDAC wiper setting from SDO output in the next frame.
            //0x4  Reset. Refresh RDAC with midscale code
            //0x6  Write contents of serial register data to control register
            //0x7 Read control register from SDO output in the next frame.

            //detect AD5293's presence by resetting AD5293 and then checking RDAC value
            //send RESET command
            TxCmd[0] = 0x1000;
            TxCmd[1] = 0x1000;
            TxCmd[2] = 0x1000;
            TxCmd[3] = 0x1000;
            rc = SPIExtWrite(dev, TxCmd, RxCmd);
            if (rc != 0)
            {
                System.Diagnostics.Debug.Print(string.Format("ERROR: SPIExtWrite[{0}] - reset to midscale fails({1})", dev, rc));
                err += 1;
            }

            //read RDAC
            //Console.WriteLine("Read RDAC");
            Array.Clear(RxCmd, 0, RxCmd.Length);

            TxCmd[0] = 0x800;
            TxCmd[1] = 0x800;
            TxCmd[2] = 0x800;
            TxCmd[3] = 0x800;
            rc = SPIExtWrite(dev, TxCmd, RxCmd);
            if (rc != 0)
            {
                System.Diagnostics.Debug.Print(string.Format("ERROR: SPIExtWrite[{0}] - prepare for reading RDAC fails({1})", dev, rc));
                err += 1;
            }

            Array.Clear(RxCmd, 0, RxCmd.Length);

            TxCmd[0] = 0x800;
            TxCmd[1] = 0x800;
            TxCmd[2] = 0x800;
            TxCmd[3] = 0x800;
            rc = SPIExtWrite(dev, TxCmd, RxCmd);
            if (rc != 0)
            {
                System.Diagnostics.Debug.Print(string.Format("ERROR: SPIExtWrite[{0}] - read RDAC fails({1})", dev, rc));
                err += 1;
            }
            else
            {
                //after reset, RDAC becomes 0x200
                if (RxCmd[0] != 0x200 ||
                    RxCmd[1] != 0x200 ||
                    RxCmd[2] != 0x200 ||
                    RxCmd[3] != 0x200)
                {
                    System.Diagnostics.Debug.Print(string.Format("ERROR: fails to check RDAC value after reset: {0:x4}, {1:x4}, {2:x4}, {3:x4}, ", RxCmd[0], RxCmd[1], RxCmd[2], RxCmd[3]));
                    err += 1;
                }
            }

            //disable RDAC register write protect           
            Array.Clear(RxCmd, 0, RxCmd.Length);

            TxCmd[0] = 0x1802;
            TxCmd[1] = 0x1802;
            TxCmd[2] = 0x1802;
            TxCmd[3] = 0x1802;
            rc = SPIExtWrite(dev, TxCmd, RxCmd);
            if (rc != 0)
            {
                System.Diagnostics.Debug.Print(string.Format("ERROR: SPIExtWrite[{0}] Disabling write protect fails({1})", dev, rc));
                err += 1;
            }
            if (err == 0)
            {
                ResIsAdjusted[dev] = true;
                return (int)errcodevalue.Finish;
            }
            else
            {
                ResIsAdjusted[dev] = false;
                return (int)errcodevalue.Err_ReadTimeout;
            }
        }



        /// <summary>
        /// 获得当前的最大电流范围
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="index"></param>
        /// <param name="CurrentRange_mA"></param>
        /// <returns></returns>
        public static double P9078_GetSenseCurrentRange_mA(int dev, int index)
        {

            double MinVoltage_mV = -9995.12;
            double MaxVoltage_mV = 9995.12;

            //得到当前的采样电阻
            double Res_Ohm = AnalogCard_ResList[dev][index];

            P9078_GetSenseRes(dev, index, ref Res_Ohm);

            //计算电流范围
            double CurrentRange_mA = MaxVoltage_mV / Res_Ohm;

            return CurrentRange_mA;

        }

        /// <summary>
        /// 设置当前的最大电流范围
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="index"></param>
        /// <param name="CurrentRange_mA"></param>
        /// <returns></returns>
        public static int P9078_SetSenseCurrentRange_mA(int dev, int index, double CurrentRange_mA)
        {

            double MinVoltage_mV = -9995.12;
            double MaxVoltage_mV = 9995.12;

            //计算出当前的采样电阻
            double Res_Ohm = MaxVoltage_mV / CurrentRange_mA;

            return P9078_SetSenseRes(dev, index, Res_Ohm);

        }


        /// <summary>
        /// 读取采样电阻值到内存
        /// </summary>
        /// <param name="dev">设备</param>
        /// <param name="index">端口</param>
        /// <param name="Res_Ohm">电阻值(欧姆)</param>
        /// <returns></returns>
        public static int P9078_GetSenseRes(int dev, int index, ref double Res_Ohm)
        {
            Res_Ohm = AnalogCard_ResList[dev][index];

            //不可调节电阻
            if (ResIsAdjusted[dev] == false)
            {                
                return (int)errcodevalue.Finish;
            }

            //一次就要读4个
            uint[] Res=new uint[MOT_MAX_AIO];

            int rc = 0, err = 0;
            ushort[] TxCmd = new ushort[4] { 0, 0, 0, 0 };
            ushort[] RxCmd = new ushort[4] { 0, 0, 0, 0 };

            if (dev < 0 || dev >= MOT_MAX_DEVICE)
            {
               // System.Diagnostics.Debug.Print(string.Format("ERROR: SenseResRead[{0}] invalid parameter({1})", dev, Res.Length));
                return (int)errcodevalue.Err_StartCondition;
            }
            if (index < 0 || index >= MOT_MAX_AIO)
            {
               // System.Diagnostics.Debug.Print(string.Format("ERROR: SenseResRead[{0}] invalid parameter({1})", dev, Res.Length));
                return (int)errcodevalue.Err_StartCondition;
            }
            //read RDAC
            //Console.WriteLine("Read RDAC");
            Array.Clear(RxCmd, 0, RxCmd.Length);

            TxCmd[0] = 0x800;
            TxCmd[1] = 0x800;
            TxCmd[2] = 0x800;
            TxCmd[3] = 0x800;
            rc = SPIExtWrite(dev, TxCmd, RxCmd);
            if (rc != 0)
            {
                System.Diagnostics.Debug.Print(string.Format("ERROR: SenseResRead[{0}] - prepare for reading RDAC fails({1})", dev, rc));
                err += 1;
            }

            Array.Clear(RxCmd, 0, RxCmd.Length);

            TxCmd[0] = 0x800;
            TxCmd[1] = 0x800;
            TxCmd[2] = 0x800;
            TxCmd[3] = 0x800;
            rc = SPIExtWrite(dev, TxCmd, RxCmd);
            if (rc != 0)
            {
                System.Diagnostics.Debug.Print(string.Format("ERROR: SenseResRead[{0}] - read RDAC fails({1})", dev, rc));
                err += 1;
            }
            else
            {
                //For AD5293
                //R_AW = (1024 - D)*R_AB/1024
                //R_BW = D*R_AB/1024

                //Current Sense Resistor Value(Ohm): R_AW + 4750
                //(1024 - D)*1E5/1024 + 4750
                for (int i = 0; i < RxCmd.Length; i++)
                {
                    if (RxCmd[i] > 1023)
                    {
                        System.Diagnostics.Debug.Print(string.Format("ERROR: SenseResRead[{0}] - invalid RDAC[{1}] value({2})", dev, i, RxCmd[i]));
                        err += 1;
                    }
                }
                int Length = Math.Min(Res.Length, RxCmd.Length);
                for (int i = 0; i < Length; i++)
                {
                    Res[i] = (1024 - (uint)RxCmd[i]) * 100000 / 1024 + 4750;
                    AnalogCard_ResList[dev][i] = Res[i];
                }

            }

            if (err == 0)
            {
                Res_Ohm = AnalogCard_ResList[dev][index];
                return (int)errcodevalue.Finish;
            }
            else
            {
                return (int)errcodevalue.Err_ReadTimeout;
            }
        }


        /// <summary>
        /// 20230905 设置采样电阻值
        /// </summary>
        /// <param name="dev">设备</param>
        /// <param name="index">端口</param>
        /// <param name="Res_Ohm">电阻值(欧姆)</param>
        /// <returns></returns>
        public static int P9078_SetSenseRes(int dev, int index, double Res_Ohm)
        {

            //不可调节电阻
            if (ResIsAdjusted[dev] == false)
            {                
                return (int)errcodevalue.Finish;
            }

            //转换为整数
            uint iRes_Ohm = (uint)Res_Ohm;

            //电阻超限控制
            if (iRes_Ohm < (4750 + 100000 / 1024))
            {
                iRes_Ohm = (4750 + 100000 / 1024);
            }
            if(iRes_Ohm > 104750)
            {
                iRes_Ohm = 104750;
            }

            //一次就要设定4个
            uint[] Res=new uint[MOT_MAX_AIO];

            uint res;
                int d, rc, err = 0;
            ushort[] TxCmd = new ushort[4] { 0, 0, 0, 0 };
            ushort[] RxCmd = new ushort[4] { 0, 0, 0, 0 };
            ushort[] TxValue = new ushort[4] { 0, 0, 0, 0 };

            //电阻转换为变量
            for(int i=0;i< MOT_MAX_AIO;i++)
            {
                //电阻
                if (index == i)
                    res = iRes_Ohm;
                else
                    res = (uint)AnalogCard_ResList[dev][i];

                //计算到TxCmd
                d = (int)(1024 - (res - 4750) * 1024.0 / 1E5);
                if (d > 1023) d = 1023;
                TxCmd[i] = (ushort)d;
            }

            //bit[13:10] command code
            //0x0  NOP command. Do nothing.
            //0x1  Write contents of serial register data to RDAC
            //0x2  Read RDAC wiper setting from SDO output in the next frame.
            //0x4  Reset. Refresh RDAC with midscale code
            //0x6  Write contents of serial register data to control register
            //0x7 Read control register from SDO output in the next frame.

            //mask unused bits and add command prefix
            for (int i = 0; i < TxCmd.Length; i++)
                TxCmd[i] = (ushort)(TxCmd[i] & 0x3ffu | 0x400u);

            rc = SPIExtWrite(dev, TxCmd, RxCmd);
            if (rc == 0)
            {
                AnalogCard_ResList[dev][index] = iRes_Ohm;
                Thread.Sleep(10);
                return (int)errcodevalue.Finish;
            }
            else
            {
                return (int)errcodevalue.Err_ReadTimeout;
                //MessageBox.Show("写入电阻值失败", "错误", MessageBoxButtons.OK);
            }



        }

        /// <summary>
        /// 20230905 设置所有采样电阻值
        /// </summary>
        /// <param name="dev">设备</param>
        /// <param name="index">端口</param>
        /// <returns></returns>
        public static int P9078_SetSenseAllRes(int dev)
        {

            //不可调节电阻
            if (ResIsAdjusted[dev] == false)
            {
                return (int)errcodevalue.Finish;
            }
            //一次就要设定4个
            uint[] Res = new uint[MOT_MAX_AIO];

            for (int i = 0; i < MOT_MAX_AIO; i++)
            {
                //转换为整数
                Res[i] = (uint)AnalogCard_ResList[dev][i];

                //电阻超限控制
                if (Res[i] < (4750 + 100000 / 1024))
                {
                    Res[i] = (4750 + 100000 / 1024);
                }
                if (Res[i] > 104750)
                {
                    Res[i] = 104750;
                }
            }
            
            uint res;
            int d, rc, err = 0;
            ushort[] TxCmd = new ushort[4] { 0, 0, 0, 0 };
            ushort[] RxCmd = new ushort[4] { 0, 0, 0, 0 };
            ushort[] TxValue = new ushort[4] { 0, 0, 0, 0 };

            //电阻转换为变量
            for (int i = 0; i < MOT_MAX_AIO; i++)
            {
                res = Res[i];
                //计算到TxCmd
                d = (int)(1024 - (res - 4750) * 1024.0 / 1E5);
                if (d > 1023) d = 1023;
                TxCmd[i] = (ushort)d;
            }

            //bit[13:10] command code
            //0x0  NOP command. Do nothing.
            //0x1  Write contents of serial register data to RDAC
            //0x2  Read RDAC wiper setting from SDO output in the next frame.
            //0x4  Reset. Refresh RDAC with midscale code
            //0x6  Write contents of serial register data to control register
            //0x7 Read control register from SDO output in the next frame.

            //mask unused bits and add command prefix
            for (int i = 0; i < TxCmd.Length; i++)
                TxCmd[i] = (ushort)(TxCmd[i] & 0x3ffu | 0x400u);

            rc = SPIExtWrite(dev, TxCmd, RxCmd);
            if (rc == 0)
            {
                return (int)errcodevalue.Finish;
            }
            else
            {
                return (int)errcodevalue.Err_ReadTimeout;
                //MessageBox.Show("写入电阻值失败", "错误", MessageBoxButtons.OK);
            }



        }


        //==========================

        /// <summary>
        /// precise delay in microsecond(μs)
        /// </summary>
        /// <param name="microsec"></param>
        static void DelayMicrosec(int microsec)
        {
            long end = System.Diagnostics.Stopwatch.GetTimestamp() + System.Diagnostics.Stopwatch.Frequency * microsec / 1000000;
            while (System.Diagnostics.Stopwatch.GetTimestamp() < end) ;
        }

        /// <summary>
        /// 写入配置数据（一次写操作必须写入4个数据），同时读取外设的输出数据
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="tx">写入到外设的数据，必须为4个，其中tx[0]对应通道1， tx[3]对应通道4</param>
        /// <param name="rx">从外设读取到的数据，必须为4个，其中rx[0]对应通道1， rx[3]对应通道4</param>
        /// <returns>0表示成功；非零表示失败</returns>
        static private int SPIExtWrite(int dev, [In] ushort[] tx, [Out] ushort[] rx)
        {
            int rc = 0;
            uint d = 0, temp = 0;
            uint dr = 0;
            if (tx.Length != 4)
            {
                System.Diagnostics.Debug.Print(string.Format("ERROR: In SPIExtWrite tx word length: {0}, expect 4\n", tx.Length));
                return -1;
            }
            if (rx.Length != 4)
            {
                System.Diagnostics.Debug.Print(string.Format("ERROR: In SPIExtWrite rx word length: {0}, expect 4\n", rx.Length));
                return -1;
            }

            rc = P9078_MotionInd(dev, 0x814, ref d);

            //set SPI SS_N low
            d &= ~(0x1u << 8);
            P9078_MotionOutd(dev, 0x814, d);
            DelayMicrosec(10);
            for (int byteIndex = 3; byteIndex >= 0; byteIndex--)
            {
                dr = 0;
                for (int bitIndex = 15; bitIndex >= 0; bitIndex--)
                {
                    dr = dr << 1;

                    //set MOSI
                    if ((tx[byteIndex] & (1u << bitIndex)) != 0)
                        d |= (0x1u << 10);
                    else
                        d &= ~(0x1u << 10);

                    //pulse SCLK
                    d |= (0x1u << 9);
                    P9078_MotionOutd(dev, 0x814, d);
                    DelayMicrosec(10);
                    d &= ~(0x1u << 9);
                    P9078_MotionOutd(dev, 0x814, d);
                    DelayMicrosec(10);

                    temp = 0;
                    P9078_MotionInd(dev, 0x814, ref temp);
                    if ((temp & (0x1u << 11)) != 0)
                        dr |= 0x1u;
                    else
                        dr &= ~(0x1u);
                }
                rx[byteIndex] = (ushort)dr;
            }

            //set SPI SS_N high
            d |= (0x1u << 8);
            P9078_MotionOutd(dev, 0x814, d);
            DelayMicrosec(10);
            return rc;
        }


    }
}