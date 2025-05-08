using System;
using System.Collections.Generic;

namespace SolveWare_BurnInInstruments //命名空间根据应用程序修改
{
    public static partial class LaserX_9078_Utilities
    {
        //当前设备存在的卡
        public static int[] CardIDList { get; set; } = new int[0];

        //当前设备模拟量电阻
        //板卡ID, 通道ID, 电阻值
        public static Dictionary<int, Dictionary<int, double>> AnalogCard_ResList { get; set; } = new Dictionary<int, Dictionary<int, double>>();
        
        public enum PmTrajSelectPlane   //插补用渐近线
        {
            XY_CW,
            XY_CCW,

            XZ_CW,
            XZ_CCW,

            YZ_CW,
            YZ_CCW,

            //横线反向
            XY_CW_LP,
            XY_CCW_LP,

            XZ_CW_LP,
            XZ_CCW_LP,

            YZ_CW_LP,
            YZ_CCW_LP,


        }

        //public enum PmTrajAxisType   //坐标系方向
        //{
        //    X_Dir,
        //    Y_Dir,
        //    Z_Dir,
        //}

        public static int P9078_MotionGetDoutEx(int dev, out UInt32 value)
        {
            //更新数据
            P9078_MotionUpdate(dev);

            //0X838寄存器[15:0] - DO[15:0], zero for ON, non-zero for OFF
            uint d = 0;
            int rc = 0;
            rc = P9078_MotionInd(dev, 0x838, ref d);

            value = d;
            return rc;
        }

        public static int P9078_MotionGetDinEx(int dev, out UInt32 value)
        {
            //更新数据
            P9078_MotionUpdate(dev);

            //0X834寄存器[15:0] - DI[15:0], 0表示导通, 1表示截止
            uint d = 0;
            int rc = 0;
            rc = P9078_MotionInd(dev, 0x834, ref d);

            value = d;
            return rc;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Adc">(-2047, 2047)代表(-9995.12,9995.12)mV</param>
        /// <returns></returns>
        public static double P9078_AdcToVoltage_mV(int Adc)
        {
            int MinAdc = -2048;
            int MaxAdc = 2047;
            double MinVoltage_mV = -9995.12;
            double MaxVoltage_mV = 9995.12;

            //计算出电压
            double Voltage_mV = (MaxVoltage_mV - MinVoltage_mV) / (MaxAdc - MinAdc) * (Adc - MinAdc) + MinVoltage_mV;

            return Voltage_mV;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="Adc">(-2047, 2047)代表(-9995.12,9995.12)mV</param>
        /// <returns></returns>
        public static double P9078_AdcToCurrent_mA(int Adc, double Res)
        {
            //计算出电压
            double Voltage_mV = P9078_AdcToVoltage_mV(Adc);

            //计算出电流
            double Current_mA = Voltage_mV / Res;

            return Current_mA;
        }

        public enum errcodevalue
        {
            Finish=0,
            Err_NoConnect=-1,
            Err_Driver=-2,
            Err_StartCondition=-3,
            Err_ReadTimeout=-4,
            Err_invalid=-5,
            Err_IsOpened=-6,
            Err_NoResources=-7
        }

        public static string P9078_ErrIDInfo(int errcode)
        {
            switch (errcode)
            {
                case 0:
                    return "指令发送并执行成功";

                case -1:
                    return "无法建立与实时任务的通讯,没有安装驱动程序";

                case -2:
                    return "发送指令超时,驱动程序和函数库版本不匹配";

                case -3:
                    return "无法执行指令,执行指令的前置条件不满足";

                case -4:
                    return "读取状态超时或数据不完整,驱动程序和函数库版本不匹配";

                case -5:
                    return "无效参数,指令参数无效";

                case -6:
                    return "设备已经打开,设备已经被应用程序打开";

                case -7:
                    return "分配系统资源失败,操作系统资源严重不足";

                default:
                    return $"异常返回代码[{errcode}], 未定义错误";
            }
        }
    }
}