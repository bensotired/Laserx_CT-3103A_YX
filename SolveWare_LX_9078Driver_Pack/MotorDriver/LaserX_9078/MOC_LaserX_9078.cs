using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace SolveWare_Motion
{
    public class MOC_LaserX_9078 : MotionControllerBase, IMotionController
    {
        private string DefaultStationConfigFilePath = @"SystemConfig"; //应该在这个类里面TestStationConfig,但是我不会, 所以新加参数

        public MOC_LaserX_9078(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {
            _AxesCollection = new List<MotorAxisBase>();
        }

        public override void LoadSpecificConfig(object configObj)
        {
            if (!_chassis.IsOnline) return;

            int rc = 0;
            string exMsg = "";
            AppPluginConfigItem config = configObj as AppPluginConfigItem;
            //var arg = config.PluginArguments;

            //[0,\card1.ini]
            //[1,\bcd.ini]
            //板卡文件参数整理
            var pathTable = ReadINITable(config.PluginArguments);//板卡参数导入文件在这里

            foreach (var item in pathTable)
            {
                if (Array.IndexOf(LaserX_9078_Utilities.CardIDList, item.Key) >= 0)
                {//存在
                    string path = Path.Combine(Path.GetFullPath(DefaultStationConfigFilePath), item.Value);
                    if (File.Exists(path))
                    {
                        int carditem = item.Key;
                        //板卡的参数文件导入
                        rc = LaserX_9078_Utilities.P9078_MotionLoadCfg(carditem, path, 0);
                        if (rc != 0)
                        {
                            exMsg = $"控制卡ID[{carditem}] PN_AxisHandleErrorMode设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                            //throw new Exception($"{exMsg}!");
                        }
                    }
                    uint[] devInfo = new uint[16];
                    rc = LaserX_9078_Utilities.P9078_MotionGetDevInfo(item.Key, devInfo, 16);
                    if (rc == 0)
                    {
                        string ApiVer = ((devInfo[6] >> 24) & 0xFF).ToString() + "." + ((devInfo[6] >> 16) & 0xFF).ToString() + "." +
                                  ((devInfo[6] >> 8) & 0xFF).ToString() + "." + ((devInfo[6] >> 0) & 0xFF).ToString() +
                                  "(0x" + devInfo[6].ToString("x8") + ")";

                        string DriverVer = ((devInfo[5] >> 24) & 0xFF).ToString() + "." + ((devInfo[5] >> 16) & 0xFF).ToString() + "." +
                                              ((devInfo[5] >> 8) & 0xFF).ToString() + "." + ((devInfo[5] >> 0) & 0xFF).ToString() +
                                                "(0x" + devInfo[5].ToString("x8") + ")";
                        string LogicVer = ((devInfo[4] >> 8) & 0xFF).ToString() + "." + (devInfo[4] & 0xFF).ToString() + "(0x" + devInfo[4].ToString("x4") + ")";

                        string PeriodMin = (devInfo[12] * 0.000001).ToString("f3") + " ms";
                        string PeriodMax = (devInfo[13] * 0.000001).ToString("f3") + " ms";
                        string PeriodMean = (devInfo[14] * 0.000001).ToString("f3") + " ms";
                    }
                }
            }

            //板卡级固定参数设置
            foreach (var carditem in LaserX_9078_Utilities.CardIDList)
            {
                //碰到极限传感器后的停止方案  。
                //设置控制器上 6 个控制轴的错误处理方式：立即停止并禁止运动规划器，
                //减速停止（不禁止运动规划器）。
                //注意，当错误处理方式设置为减速停止，就不能再使用控制轴或插补规划
                //器的 enabled 等于零来判断是否出现异常，而应该使用控制轴或插补规划
                //器状态里面的 error 不等于零来判断是否出现异常。
                //控制器默认方式为立即停止并禁止运动规划器。
                //  0 表示立即停止并禁止规划器
                //  1 表示减速停止
                rc = LaserX_9078_Utilities.P9078_AxisSetParameter(carditem, 0, (int)LaserX_9078_Utilities.PN_NUMBER.PN_AxisHandleErrorMode, 1);
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{carditem}] PN_AxisHandleErrorMode设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    //throw new Exception($"{exMsg}!");
                    break;
                }

                //0:表示伺服与控制轴运动规划器同步（联动）
                //1:表示伺服与控制轴运动规划器相互独立
                rc = LaserX_9078_Utilities.P9078_AxisSetParameter(carditem, 0, (int)LaserX_9078_Utilities.PN_NUMBER.PN_AxisExtEnableMode, 0); //联动控制
                if (rc != 0)
                {
                    exMsg = $"控制卡ID[{carditem}] PN_AxisExtEnableMode设置失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                //停止数据采集，第一个参数为X轴的轴号    0:表示停止数据采集
                rc = LaserX_9078_Utilities.P9078_AxisSetParameter(carditem, 0, (int)LaserX_9078_Utilities.PN_NUMBER.PN_EnableCaptureFifo, 0);
                if (rc != 0)
                {
                    exMsg = $"执行错误 停止数据采集 [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                //将控制轴从插补规划器解除绑定
                LaserX_9078_Utilities.P9078_TrajCombineAxes(carditem, 0x0);

                //自动初始化电阻设定功能
                LaserX_9078_Utilities.P9078_SPIExtInit(carditem);
                if (rc != 0)
                {
                    exMsg = $"执行错误 设定采样电阻 [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }

                LaserX_9078_Utilities.P9078_SetSenseAllRes(carditem);
                if (rc != 0)
                {
                    exMsg = $"执行错误 设定采样电阻 [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    throw new Exception($"{exMsg}!");
                }
            }
        }

        public Dictionary<int, string> ReadINITable(string para)
        {
            Dictionary<int, string> p = new Dictionary<int, string>();

            string pattern = @"(?<=\[)(.*?),(.*?)(?=\])";
            try
            {
                //进行正则表达式分解
                Regex r = new Regex(pattern);

                //获得组号码的清单
                int[] gnums = r.GetGroupNumbers();

                //首次匹配
                Match m = r.Match(para);

                while (m.Success)
                {
                    // string N1 = m.Groups[0].ToString();
                    int CardIndex = 0;
                    if (int.TryParse(m.Groups[1].ToString(), out CardIndex) == false)
                    {
                        return p;
                    }
                    string path = m.Groups[2].ToString();

                    p.Add(CardIndex, path);

                    //下一个匹配
                    m = m.NextMatch();
                }
            }
            catch
            {
                return p;
            }

            return p;
        }

        public override MotorAxisBase CreateAxisInstance(MotorSetting setting)
        {
            var axis = new Motor_LaserX_9078(setting);
            if (axis.Interation.IsSimulation == false)
            {
                if (this.IsOnline)//并且轴是在线的状态
                {
                    axis.Init();
                }
            }
            return axis;
        }

        //public Dictionary<int, double[]> Get_AnalogInputVoltage_V()
        //{
        //    int rc = 0;
        //    string exMsg = "";

        //    Dictionary<int, double[]> Voltage_V = new Dictionary<int, double[]>();

        //    foreach(var carditem in LaserX_9078_Utilities.CardIDList)
        //    {
        //        double[] vol = new double[LaserX_9078_Utilities.MOT_MAX_AIO];
        //        for(int i=0;i<LaserX_9078_Utilities.MOT_MAX_AIO;i++)
        //        {
        //            double t = 0;
        //            rc = LaserX_9078_Utilities.P9078_MotionGetAin(carditem, i,ref t );
        //            if (rc != 0)
        //            {
        //                exMsg = $"控制卡ID[{carditem}] 模拟量读取失败[{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
        //                //throw new Exception($"{exMsg}!");
        //                break;
        //            }
        //            vol[i] = t;
        //        }

        //        Voltage_V.Add(carditem, vol);
        //    }

        //    return Voltage_V;
        //}
    }
}