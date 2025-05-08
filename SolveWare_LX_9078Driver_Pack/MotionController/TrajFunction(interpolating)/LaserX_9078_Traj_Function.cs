using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using LX_BurnInSolution.Utilities;
using System.Text;
using System.IO;

namespace SolveWare_TestPackage
{
    public partial class LaserX_9078_Traj_Function
    {
        protected static void Log_Global(string log)
        {
            //this._core?.Log_Global($"{this.Name} {log}");
            //this._core?.Log_Global($"[{this.Name} @ {this._exeInteration.Name}] {log}.");
        }

        //插补捕捉功能

        #region 直线插补等

        public enum PmTrajAxisType   //坐标系方向
        {
            X_Dir = 0,//X分量
            Y_Dir,//Y分量
            Z_Dir,//Z分量

            A_Dir,//A分量
            B_Dir,//B分量
            C_Dir,//C分量
        }

        public struct TrajResultItem
        {
            public List<int> Id;
            public List<int> DataIndex; //数据序列号
            public Dictionary<Motor_LaserX_9078, List<double>> MotorPos_mm;
            public Dictionary<int, List<short>> ADC;
            public Dictionary<int, List<double>> Voltage_mV;
            public Dictionary<int, List<double>> Current_mA;
        }

        //阈值停止方式
        public struct TrajThresholdStop
        {
            public bool En; // 是否启动阈值方式
            public Dictionary<int, double> ThCurrent_mA;    //通道 + 电流阈值
            public Dictionary<int, double> ThVoltage_mV;    //通道 + 电压阈值
        }

        public static int Parallel_MoveLine(
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict,
            AxesPosition StartPosition,     //起点
            AxesPosition EndPosition,       //终点
            double TrajSpeed,               //希望的插补速度
            out TrajResultItem result,      //返回结果
            CancellationToken token,
            SpeedLevel speedLevel = SpeedLevel.Normal
            )
        {
            try
            {
                result = new TrajResultItem();

                //希望的插补速度
                TrajSpeed = Math.Abs(TrajSpeed);
                if (TrajSpeed <= 0)
                {
                    //插补速度不可为0
                    return ErrorCodes.Unexecuted;
                }

                int CardNo;
                //停止插补组
                int rtn = TrajAxisGroup_Close(axisDict.First().Value);
                if (rtn != ErrorCodes.NoError)
                {
                    return rtn;
                }

                {
                    ////运动到开始点
                    //Dictionary<PmTrajAxisType, Motor_LaserX_9078> axis9078 = new Dictionary<PmTrajAxisType, Motor_LaserX_9078>();

                    //foreach (var item in axisDict)
                    //{
                    //    Motor_LaserX_9078 t = item.Value as Motor_LaserX_9078;

                    //    axis9078.Add(item.Key, t);
                    //}

                    //foreach (var a in axis9078)
                    //{
                    //    a.Value.MoveToV3(StartPosition.GetSingleItem(a.Value.Name).Position, SpeedType.Auto, speedLevel);
                    //}

                    //foreach (var a in axis9078)
                    //{
                    //    a.Value.WaitMotionDone();
                    //}
                }

                //建立插补组
                rtn = TrajAxisGroup_Init(axisDict, new AxesPosition[] { StartPosition, EndPosition }, out CardNo);
                if (rtn != ErrorCodes.NoError)
                {
                    return rtn;
                }

                //插补速度和加速度的建立
                double TrajMaxSpeed;
                double TrajMaxAcc;
                rtn = TrajAxisGroup_Speed(axisDict, new AxesPosition[] { StartPosition, EndPosition }, speedLevel, out TrajMaxSpeed, out TrajMaxAcc);
                if (rtn != ErrorCodes.NoError)
                {
                    return rtn;
                }

                //插补速度的控制
                if (TrajSpeed < TrajMaxSpeed) TrajMaxSpeed = TrajSpeed;

                //空间直线插补运动
                int err = 0;
                int id = 1;
                int rc = 0;
                int turn = 0;

                rtn = TrajAxisGroup_LinearMove(ref id, CardNo, axisDict, StartPosition, TrajMaxSpeed, TrajMaxAcc, false);  //运动, 但不捕捉数据
                if (rtn != ErrorCodes.NoError)
                {
                    //出现异常
                    TrajAxisGroup_Close(axisDict.First().Value);
                    return rtn;
                }

                rc = LaserX_9078_Utilities.P9078_TrajDelay(CardNo, 10.0 / 1000.0);       //不捕捉

                rtn = TrajAxisGroup_LinearMove(ref id, CardNo, axisDict, EndPosition, TrajMaxSpeed, TrajMaxAcc, true);  //运动捕捉数据
                if (rtn != ErrorCodes.NoError)
                {
                    //出现异常
                    TrajAxisGroup_Close(axisDict.First().Value);
                    return rtn;
                }

                rc = LaserX_9078_Utilities.P9078_TrajDelay(CardNo, 10.0 / 1000.0);       //不捕捉

                //最大执行步骤是2000, 如果超过2000, 就必须一边压数据, 一边取结果

                //阈值停止
                TrajThresholdStop thresholdStop = new TrajThresholdStop()
                {
                    En = false,
                    ThCurrent_mA = new Dictionary<int, double>(),
                    ThVoltage_mV = new Dictionary<int, double>()
                };

                //等待插补完成 并且获取数据
                rtn = Traj_Run(CardNo, axisDict, thresholdStop, token, out result);
                if (rtn != ErrorCodes.NoError)
                {
                    //出现异常
                    TrajAxisGroup_Close(axisDict.First().Value);
                    return rtn;
                }

                //停止插补组
                rtn = TrajAxisGroup_Close(axisDict.First().Value);
                if (rtn != ErrorCodes.NoError)
                {
                    return rtn;
                }

                return rtn;
            }
            catch (Exception ex)
            {
                //出现异常
                TrajAxisGroup_Close(axisDict.First().Value);
                throw ex;
            }
            finally
            {
                TrajAxisGroup_Close(axisDict.First().Value);
            }

        }

        //渐开线/渐近线
        //渐开线/渐近线
        public static int Parallel_2DCycleInvolute(
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict,
            AxesPosition CenterPosition,     //中心坐标
            double Radius_Inside,            //中间的旋转半径
            double Radius,                   //旋转半径
            double Interval,                 //螺旋间距
            LaserX_9078_Utilities.PmTrajSelectPlane RunPlane,   //在哪个面运行, 正反运行
            bool InSideToOutSide,                                   //true:渐开线   false:渐近线
            double TrajSpeed,               //希望的插补速度
            out TrajResultItem result,      //返回结果
            TrajThresholdStop thresholdStop,  //阈值停止
            CancellationToken token,
            SpeedLevel speedLevel = SpeedLevel.Normal
            )
        {
            try
            {
                result = new TrajResultItem();
                string exMsg = "";

                //希望的插补速度
                TrajSpeed = Math.Abs(TrajSpeed);
                if (TrajSpeed <= 0)
                {
                    //插补速度不可为0
                    return ErrorCodes.Unexecuted;
                }

                //至少要有1个轴
                if (axisDict.Count < 2)
                {
                    exMsg = $"至少要有2个轴";
                    Log_Global($"Parallel_2DCycleInvolute Error - {exMsg}");
                    return ErrorCodes.Unexecuted;
                }

                Radius_Inside=Math.Abs(Radius_Inside);
                Radius = Math.Abs(Radius);
                Interval = Math.Abs(Interval);

                if (Interval < 0.00001)
                {
                    exMsg = $"旋转间距设置过小";
                    Log_Global($"Parallel_2DCycleInvolute Error - {exMsg}");
                    return ErrorCodes.Unexecuted;
                }

                if(Radius_Inside< Interval)
                {
                    Radius_Inside = Interval;
                }

                if (Radius_Inside >= Radius- Interval)
                {
                    exMsg = $"初始半径设置过小";
                    Log_Global($"Parallel_2DCycleInvolute Error - {exMsg}");
                    return ErrorCodes.Unexecuted;
                }
                //运行平面的挂载轴错误
                switch (RunPlane)
                {
                    case LaserX_9078_Utilities.PmTrajSelectPlane.XY_CW:
                    case LaserX_9078_Utilities.PmTrajSelectPlane.XY_CCW:
                        if (axisDict.ContainsKey(PmTrajAxisType.X_Dir) == false || axisDict.ContainsKey(PmTrajAxisType.Y_Dir) == false)
                        {
                            exMsg = $"挂载轴参数不足";
                            Log_Global($"Parallel_2DCycleInvolute Error - {exMsg}");
                            return ErrorCodes.Unexecuted;
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CW:
                    case LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CCW:
                        if (axisDict.ContainsKey(PmTrajAxisType.X_Dir) == false || axisDict.ContainsKey(PmTrajAxisType.Z_Dir) == false)
                        {
                            exMsg = $"挂载轴参数不足";
                            Log_Global($"Parallel_2DCycleInvolute Error - {exMsg}");
                            return ErrorCodes.Unexecuted;
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.YZ_CW:
                    case LaserX_9078_Utilities.PmTrajSelectPlane.YZ_CCW:
                        if (axisDict.ContainsKey(PmTrajAxisType.Y_Dir) == false || axisDict.ContainsKey(PmTrajAxisType.Z_Dir) == false)
                        {
                            exMsg = $"挂载轴参数不足";
                            Log_Global($"Parallel_2DCycleInvolute Error - {exMsg}");
                            return ErrorCodes.Unexecuted;
                        }
                        break;
                }

                //希望的插补速度
                TrajSpeed = Math.Abs(TrajSpeed);
                if (TrajSpeed <= 0)
                {
                    //插补速度不可为0
                    return ErrorCodes.Unexecuted;
                }

                int CardNo;
                //停止插补组
                int rtn = TrajAxisGroup_Close(axisDict.First().Value);
                if (rtn != ErrorCodes.NoError)
                {
                    return rtn;
                }

                if (token.IsCancellationRequested)
                {
                    Log_Global("用户取消测试");
                    token.ThrowIfCancellationRequested();
                    throw new OperationCanceledException();
                }

                //AxesPosition P1 = CloneHelper.Clone<AxesPosition>(CenterPosition);
                //AxesPosition P2 = CloneHelper.Clone<AxesPosition>(CenterPosition);

                LaserX_9078_Utilities.MotPose target_Center = AxesPosition_To_MotPose(axisDict, CenterPosition);       //中心坐标
                LaserX_9078_Utilities.MotPose target_P1 = AxesPosition_To_MotPose(axisDict, CenterPosition);       //螺旋开始
                LaserX_9078_Utilities.MotPose target_P2 = AxesPosition_To_MotPose(axisDict, CenterPosition);      //螺旋结束

                LaserX_9078_Utilities.PmCartesian nv = new LaserX_9078_Utilities.PmCartesian();         //螺旋线法线
                LaserX_9078_Utilities.PmCartesian center = AxesPosition_To_PmCartesian(axisDict, CenterPosition);    //螺旋线中心

                //运行参数计算 按照渐开线计算
                switch (RunPlane)
                {
                    case LaserX_9078_Utilities.PmTrajSelectPlane.XY_CW:
                        {
                            target_P1.x = center.x - Radius_Inside;// Interval / 2;
                            target_P1.y = center.y;

                            target_P1.z = center.z;

                            target_P2.x = center.x - Radius;
                            target_P2.y = center.y;

                            target_P2.z = center.z;

                            nv.x = 0.0; nv.y = 0.0; nv.z = 1.0;    //plance XY, direction CW
                        }
                        break;


                    case LaserX_9078_Utilities.PmTrajSelectPlane.XY_CCW:
                        {
                            target_P1.x = center.x - Radius_Inside;//Interval / 2;
                            target_P1.y = center.y;

                            target_P2.x = center.x - Radius;
                            target_P2.y = center.y;

                            nv.x = 0.0; nv.y = 0.0; nv.z = -1.0;    //plance XY, direction CCW
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CW:
                        {
                            target_P1.x = center.x - Radius_Inside;//Interval / 2;
                            target_P1.z = center.z;

                            target_P2.x = center.x - Radius;
                            target_P2.z = center.z;

                            nv.x = 0.0; nv.y = 1.0; nv.z = 0.0;    //plance XZ, direction CW
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CCW:
                        {
                            target_P1.x = center.x - Radius_Inside; //Interval / 2;
                            target_P1.z = center.z;

                            target_P2.x = center.x - Radius;
                            target_P2.z = center.z;

                            nv.x = 0.0; nv.y = -1.0; nv.z = 0.0;    //plance XZ, direction CCW
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.YZ_CW:
                        {
                            target_P1.y = center.y - Radius_Inside; //Interval / 2;
                            target_P1.z = center.z;

                            target_P2.y = center.y - Radius;
                            target_P2.z = center.z;

                            nv.x = 1.0; nv.y = 0.0; nv.z = 0.0;    //plance YZ, direction CW
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.YZ_CCW:
                        {
                            target_P1.y = center.y - Radius_Inside; //Interval / 2;
                            target_P1.z = center.z;

                            target_P2.y = center.y - Radius;
                            target_P2.z = center.z;

                            nv.x = -1.0; nv.y = 0.0; nv.z = 0.0;    //plance YZ, direction CCW
                        }
                        break;

                    //水平线反向
                    case LaserX_9078_Utilities.PmTrajSelectPlane.XY_CW_LP:
                        {
                            target_P1.x = center.x + Radius_Inside; //Interval / 2;
                            target_P1.y = center.y;

                            target_P1.z = center.z;

                            target_P2.x = center.x + Radius;
                            target_P2.y = center.y;

                            target_P2.z = center.z;

                            nv.x = 0.0; nv.y = 0.0; nv.z = 1.0;    //plance XY, direction CW
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.XY_CCW_LP:
                        {
                            target_P1.x = center.x + Radius_Inside; //Interval / 2;
                            target_P1.y = center.y;

                            target_P2.x = center.x + Radius;
                            target_P2.y = center.y;

                            nv.x = 0.0; nv.y = 0.0; nv.z = -1.0;    //plance XY, direction CCW
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CW_LP:
                        {
                            target_P1.x = center.x + Radius_Inside; //Interval / 2;
                            target_P1.z = center.z;

                            target_P2.x = center.x + Radius;
                            target_P2.z = center.z;

                            nv.x = 0.0; nv.y = 1.0; nv.z = 0.0;    //plance XZ, direction CW
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CCW_LP:
                        {
                            target_P1.x = center.x + Radius_Inside; //Interval / 2;
                            target_P1.z = center.z;

                            target_P2.x = center.x + Radius;
                            target_P2.z = center.z;

                            nv.x = 0.0; nv.y = -1.0; nv.z = 0.0;    //plance XZ, direction CCW
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.YZ_CW_LP:
                        {
                            target_P1.y = center.y + Radius_Inside; //Interval / 2;
                            target_P1.z = center.z;

                            target_P2.y = center.y + Radius;
                            target_P2.z = center.z;

                            nv.x = 1.0; nv.y = 0.0; nv.z = 0.0;    //plance YZ, direction CW
                        }
                        break;

                    case LaserX_9078_Utilities.PmTrajSelectPlane.YZ_CCW_LP:
                        {
                            target_P1.y = center.y + Radius_Inside; //Interval / 2;
                            target_P1.z = center.z;

                            target_P2.y = center.y + Radius;
                            target_P2.z = center.z;

                            nv.x = -1.0; nv.y = 0.0; nv.z = 0.0;    //plance YZ, direction CCW
                        }
                        break;

                }

                //{
                //    //运动到开始点
                //    Dictionary<PmTrajAxisType, Motor_LaserX_9078> axis9078 = new Dictionary<PmTrajAxisType, Motor_LaserX_9078>();

                //    foreach (var item in axisDict)
                //    {
                //        Motor_LaserX_9078 t = item.Value as Motor_LaserX_9078;

                //        axis9078.Add(item.Key, t);
                //    }
                //    if (InSideToOutSide == true)
                //    {
                //        foreach (var a in axis9078)
                //        {
                //            a.Value.MoveToV3(target_P1.GetSingleItem(a.Value.Name).Position, SpeedType.Auto, speedLevel);
                //        }
                //    }
                //    else
                //    {
                //        foreach (var a in axis9078)
                //        {
                //            a.Value.MoveToV3(target_P2.GetSingleItem(a.Value.Name).Position, SpeedType.Auto, speedLevel);
                //        }
                //    }

                //    foreach (var a in axis9078)
                //    {
                //        a.Value.WaitMotionDone();
                //    }

                //}

                //建立插补组
                rtn = TrajAxisGroup_Init(axisDict, new AxesPosition[] { CenterPosition }, out CardNo);
                if (rtn != ErrorCodes.NoError)
                {
                    return rtn;
                }

                //插补速度和加速度的建立
                double TrajMaxSpeed;
                double TrajMaxAcc;
                rtn = TrajAxisGroup_Speed(axisDict, new AxesPosition[] { CenterPosition }, speedLevel, out TrajMaxSpeed, out TrajMaxAcc);
                if (rtn != ErrorCodes.NoError)
                {
                    return rtn;
                }

                //插补速度的控制
                if (TrajSpeed < TrajMaxSpeed) TrajMaxSpeed = TrajSpeed;

                //空间螺旋运动
                int err = 0;
                int id = 1;
                int rc = 0;
                int turn = (int)Math.Ceiling((Radius - Radius_Inside) / Interval);       //旋转多少周
                //int turn = (int)Math.Ceiling((Radius - Interval / 2) / Interval);       //旋转多少周

                if (InSideToOutSide == true)
                {
                    //渐开线
                    //首先运动到Center点
                    LaserX_9078_Utilities.P9078_TrajSetMotionId(CardNo, id++);    //为每条插补指令分配一个ID号
                    rc = LaserX_9078_Utilities.P9078_TrajLinearMoveEx(CardNo, ref target_Center, TrajMaxSpeed, TrajMaxAcc, 0);       //不捕捉
                    if (rc != 0) err++;

                    rc = LaserX_9078_Utilities.P9078_TrajDelay(CardNo, 10.0 / 1000.0);       //等待
                    if (rc != 0) err++;

                    //直线P1点
                    LaserX_9078_Utilities.P9078_TrajSetMotionId(CardNo, id++);    //为每条插补指令分配一个ID号
                    rc = LaserX_9078_Utilities.P9078_TrajLinearMoveEx(CardNo, ref target_P1, TrajMaxSpeed, TrajMaxAcc, 1);       //捕捉
                    if (rc != 0) err++;

                    //渐开线插补（注意：起点或终点的半径不能为0，否则函数返回错误）
                    LaserX_9078_Utilities.P9078_TrajSetMotionId(CardNo, id++);    //为每条插补指令分配一个ID号
                    rc = LaserX_9078_Utilities.P9078_TrajCircularMoveEx(CardNo, ref target_P2, ref center, ref nv, turn, TrajMaxSpeed, TrajMaxAcc, 1); //捕捉
                    if (rc != 0) err++;
                }
                else
                {
                    //渐进线

                    //首先运动到P2点
                    LaserX_9078_Utilities.P9078_TrajSetMotionId(CardNo, id++);    //为每条插补指令分配一个ID号
                    rc = LaserX_9078_Utilities.P9078_TrajLinearMoveEx(CardNo, ref target_P2, TrajMaxSpeed, TrajMaxAcc, 0);       //不捕捉
                    if (rc != 0) err++;

                    rc = LaserX_9078_Utilities.P9078_TrajDelay(CardNo, 10.0 / 1000.0);       //等待
                    if (rc != 0) err++;

                    //渐进线插补（注意：起点或终点的半径不能为0，否则函数返回错误）
                    LaserX_9078_Utilities.P9078_TrajSetMotionId(CardNo, id++);    //为每条插补指令分配一个ID号
                    rc = LaserX_9078_Utilities.P9078_TrajCircularMoveEx(CardNo, ref target_P1, ref center, ref nv, turn, TrajMaxSpeed, TrajMaxAcc, 1); //捕捉
                    if (rc != 0) err++;

                    //直线Center点
                    LaserX_9078_Utilities.P9078_TrajSetMotionId(CardNo, id++);    //为每条插补指令分配一个ID号
                    rc = LaserX_9078_Utilities.P9078_TrajLinearMoveEx(CardNo, ref target_Center, TrajMaxSpeed, TrajMaxAcc, 1);       //捕捉
                    if (rc != 0) err++;
                }

                if (err > 0)
                {
                    //运动规划增加项出现异常
                    TrajAxisGroup_Close(axisDict.First().Value);
                    return rtn;
                }
                //最大执行步骤是2000, 如果超过2000, 就必须一边压数据, 一边取结果


                if (token.IsCancellationRequested)
                {
                    Log_Global("用户取消测试");
                    token.ThrowIfCancellationRequested();
                    throw new OperationCanceledException();
                }

                //阈值停止
                //TrajThresholdStop thresholdStop = new TrajThresholdStop()
                //{
                //    En = false,
                //    ThCurrent_mA = new Dictionary<int, double>(),
                //    ThVoltage_mV = new Dictionary<int, double>()
                //};

                //等待插补完成 并且获取数据
                rtn = Traj_Run(CardNo, axisDict, thresholdStop, token, out result);
                if (rtn != ErrorCodes.NoError)
                {
                    //出现异常
                    TrajAxisGroup_Close(axisDict.First().Value);
                    return rtn;
                }

                //停止插补组
                rtn = TrajAxisGroup_Close(axisDict.First().Value);
                if (rtn != ErrorCodes.NoError)
                {
                    return rtn;
                }

                return rtn;
            }
            catch (Exception ex)
            {
                //停止插补组
                TrajAxisGroup_Close(axisDict.First().Value);
                throw ex;
            }
            finally
            {
                TrajAxisGroup_Close(axisDict.First().Value);
            }
        }
        #region 插补支持函数

        //插补轴组的建立
        private static int TrajAxisGroup_Init(
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict,
            AxesPosition[] Position,    //位置坐标
            out int TrajCardNo      //返回卡号
            )
        {
            int errorCode = ErrorCodes.NoError;

            string exMsg;

            TrajCardNo = -1;

            //至少要有1个轴
            if (axisDict.Count < 1)
            {
                exMsg = $"至少要有1个轴";
                return ErrorCodes.Unexecuted;
            }

            //建立新结构
            Dictionary<PmTrajAxisType, Motor_LaserX_9078> axis9078 = new Dictionary<PmTrajAxisType, Motor_LaserX_9078>();

            foreach (var item in axisDict)
            {
                if (item.Value.GetType() != typeof(Motor_LaserX_9078))
                {
                    exMsg = $"插补捕捉功能, 仅支持LaserX_9078类型的轴";
                    return ErrorCodes.Unexecuted;
                }

                Motor_LaserX_9078 t = item.Value as Motor_LaserX_9078;

                axis9078.Add(item.Key, t);
            }

            int CardNo = axis9078.First().Value.CardNo;

            foreach (var item in axis9078)
            {
                if (item.Value.CardNo != CardNo)
                {
                    exMsg = $"不能跨板卡进行插补操作";
                    return ErrorCodes.Unexecuted;
                }
            }

            if (LaserX_9078_Utilities.CardIDList.Contains(CardNo) == false)
            {
                exMsg = $"不存在控制卡";
                return ErrorCodes.Unexecuted;
            }

            //检查数据点坐标
            foreach (var a in axis9078)
            {
                foreach (var p in Position)
                {
                    int exist = p.ItemCollection.Where(Item => int.Parse(Item.CardNo) == a.Value.CardNo && int.Parse(Item.AxisNo) == a.Value.AxisNo).Count();
                    if (exist <= 0)
                    {
                        exMsg = $"数据点中不存在插补轴参数";
                        return ErrorCodes.Unexecuted;
                    }
                }
            }

            //使能插补规划器
            int rc = LaserX_9078_Utilities.P9078_TrajEnable(CardNo);
            if (rc != 0)
            {
                exMsg = $"执行错误 P9078_TrajEnable [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                return ErrorCodes.Unexecuted;
            }
            //将控制轴从插补规划器解除绑定
            rc = LaserX_9078_Utilities.P9078_TrajCombineAxes(CardNo, 0x0);
            if (rc != 0)
            {
                exMsg = $"执行错误 P9078_TrajCombineAxes [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                return ErrorCodes.Unexecuted;
            }

            Thread.Sleep(2); //需要1ms执行

            //如果当前已经有插补操作
            //读取插补器上各个分量的坐标,没有用到的分量保持原值不变
            rc = LaserX_9078_Utilities.P9078_MotionUpdate(CardNo);
            if (rc == 0)
            {
                //LaserX_9078_Utilities.MOT_TRAJ_STAT trajStat = new LaserX_9078_Utilities.MOT_TRAJ_STAT();
                LaserX_9078_Utilities.MOT_TRAJ_STAT trajStat = default(LaserX_9078_Utilities.MOT_TRAJ_STAT);
                LaserX_9078_Utilities.P9078_MotionGetTrajStatus(CardNo, ref trajStat, Marshal.SizeOf(trajStat));
                if (trajStat.inpos == 0)
                {
                    exMsg = $"执行错误 当前板卡已经有插补操作在执行";
                    return ErrorCodes.Unexecuted;
                }
            }

            //连接到本地方向分量
            int[] AxisEx = new int[LaserX_9078_Utilities.MOT_MAX_AXIS];
            for (int i = 0; i < LaserX_9078_Utilities.MOT_MAX_AXIS; i++)
            {
                AxisEx[i] = -1; //空置所有轴
            }

            foreach (var item in axis9078)
            {
                AxisEx[(int)item.Key] = item.Value.AxisNo; //将轴绑定方向分量
            }
            //绑定成功后自由模式控制轴的命令位置被赋值到插补规划器对应分量的命令位置
            rc = LaserX_9078_Utilities.P9078_TrajCombineAxesEx(CardNo, AxisEx, 6);
            if (rc != 0)
            {
                exMsg = $"执行错误 P9078_TrajCombineAxesEx [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                return ErrorCodes.Unexecuted;
            }

            Thread.Sleep(2); //需要1ms执行

            //使能数据采集，第一个参数为X轴的轴号   1:表示进行数据采集
            rc = LaserX_9078_Utilities.P9078_AxisSetParameter(CardNo, axis9078.First().Value.AxisNo, (int)LaserX_9078_Utilities.PN_NUMBER.PN_EnableCaptureFifo, 1);
            if (rc != 0)
            {
                exMsg = $"执行错误 使能数据采集 [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                return ErrorCodes.Unexecuted;
            }

            //插补卡号
            TrajCardNo = CardNo;
            return errorCode;
        }

        //插补轴速度的建立
        private static int TrajAxisGroup_Speed(
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict,
            AxesPosition[] Position,   //位置坐标
            SpeedLevel speedLevel,         //速度
            out double TrajMaxSpeed,         //这些插补轴能够运行的最大速度
            out double TrajMaxAcc         //这些插补轴能够运行的最大加速度
            )
        {
            int errorCode = ErrorCodes.NoError;

            string exMsg;

            TrajMaxSpeed = 0;
            TrajMaxAcc = 0;
            //至少要有1个轴
            if (axisDict.Count < 1)
            {
                exMsg = $"至少要有1个轴";
                return ErrorCodes.Unexecuted;
            }

            //建立新结构
            Dictionary<PmTrajAxisType, Motor_LaserX_9078> axis9078 = new Dictionary<PmTrajAxisType, Motor_LaserX_9078>();

            foreach (var item in axisDict)
            {
                if (item.Value.GetType() != typeof(Motor_LaserX_9078))
                {
                    exMsg = $"插补捕捉功能, 仅支持LaserX_9078类型的轴";
                    return ErrorCodes.Unexecuted;
                }

                Motor_LaserX_9078 t = item.Value as Motor_LaserX_9078;

                axis9078.Add(item.Key, t);
            }

            int CardNo = axis9078.First().Value.CardNo;

            foreach (var item in axis9078)
            {
                if (item.Value.CardNo != CardNo)
                {
                    exMsg = $"不能跨板卡进行插补操作";
                    return ErrorCodes.Unexecuted;
                }
            }

            if (LaserX_9078_Utilities.CardIDList.Contains(CardNo) == false)
            {
                exMsg = $"不存在控制卡";
                return ErrorCodes.Unexecuted;
            }

            //得到最小速度作为插补速度
            double? MinSpeed = null;
            double? MinAcc = null;

            foreach (var a in axis9078)
            {
                foreach (var p1 in Position)
                {
                    var pos = p1.ItemCollection.Where(Item => int.Parse(Item.CardNo) == a.Value.CardNo && int.Parse(Item.AxisNo) == a.Value.AxisNo).ToArray();
                    foreach (var p2 in pos)
                    {
                        double? speed = null;
                        double? acc = null;

                        switch (speedLevel)
                        {
                            case SpeedLevel.High:
                            case SpeedLevel.Normal:
                                {
                                    speed = a.Value.Speed.Auto_Max_Velocity;
                                    acc = a.Value.Speed.Auto_Acceleration;
                                }
                                break;

                            case SpeedLevel.Low:
                                {
                                    speed = a.Value.Speed.Auto_Low_Max_Velocity;
                                    acc = a.Value.Speed.Auto_Acceleration;
                                }
                                break;
                        }

                        if (speed != null)
                        {
                            if (MinSpeed == null) MinSpeed = speed;
                            else if (speed < MinSpeed) MinSpeed = speed;
                        }

                        if (acc != null)
                        {
                            if (MinAcc == null) MinAcc = acc;
                            else if (acc < MinAcc) MinAcc = acc;
                        }
                    }
                }
            }

            if (MinSpeed == null)
            {
                exMsg = $"所有点位均未配置速度";
                return ErrorCodes.Unexecuted;
            }
            if (MinAcc == null)
            {
                exMsg = $"所有点位均未配置加速度";
                return ErrorCodes.Unexecuted;
            }

            //最大插补速度是这些轴里的最小速度
            TrajMaxSpeed = (double)MinSpeed;
            TrajMaxAcc = (double)MinAcc;

            return errorCode;
        }

        //插补组的关闭
        private static int TrajAxisGroup_Close(MotorAxisBase Axis)
        {
            int errorCode = ErrorCodes.NoError;
            int rc = 0;

            string exMsg = "";

            if (Axis.GetType() != typeof(Motor_LaserX_9078))
            {
                exMsg = $"插补捕捉功能, 仅支持LaserX_9078类型的轴";
                return ErrorCodes.Unexecuted;
            }

            int CardNo = (Axis as Motor_LaserX_9078).CardNo;
            int AxisNo = (Axis as Motor_LaserX_9078).AxisNo;

            if (LaserX_9078_Utilities.CardIDList.Contains(CardNo) == false)
            {
                exMsg = $"不存在控制卡";
                return ErrorCodes.Unexecuted;
            }

            //停止数据采集，第一个参数为X轴的轴号    0:表示停止数据采集
            rc = LaserX_9078_Utilities.P9078_AxisSetParameter(CardNo, AxisNo, (int)LaserX_9078_Utilities.PN_NUMBER.PN_EnableCaptureFifo, 0);
            if (rc != 0)
            {
                exMsg = $"执行错误 停止数据采集 [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                return ErrorCodes.Unexecuted;
            }
            Thread.Sleep(5);

            //将控制轴从插补规划器解除绑定
            rc = LaserX_9078_Utilities.P9078_TrajCombineAxes(CardNo, 0x0);
            if (rc != 0)
            {
                int i;
                for (i = 0; i < 500; i++)
                {
                    Thread.Sleep(5);
                    rc = LaserX_9078_Utilities.P9078_TrajCombineAxes(CardNo, 0x0);
                    if (rc == 0)
                    {
                        break;
                    }
                }
                if (rc != 0)
                {
                    exMsg = $"执行错误 解除绑定 [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                    return ErrorCodes.Unexecuted;
                }

            }

            Thread.Sleep(5); //需要1ms执行
            Log_Global("插补组关闭");
            return errorCode;
        }

        //根据AxesPosition坐标点生成插补系坐标
        private static LaserX_9078_Utilities.MotPose AxesPosition_To_MotPose
            (
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict,
            AxesPosition Position     //目标
            )
        {
            LaserX_9078_Utilities.MotPose p1 = new LaserX_9078_Utilities.MotPose();

            //建立新结构
            Dictionary<PmTrajAxisType, Motor_LaserX_9078> axis9078 = new Dictionary<PmTrajAxisType, Motor_LaserX_9078>();

            foreach (var item in axisDict)
            {
                Motor_LaserX_9078 t = item.Value as Motor_LaserX_9078;
                axis9078.Add(item.Key, t);
            }

            int CardNo = axis9078.First().Value.CardNo;

            //检查数据点坐标
            foreach (var a in axis9078)
            {
                int AxisNo = a.Value.AxisNo;
                var p = Position.ItemCollection.Where(Item => int.Parse(Item.CardNo) == CardNo && int.Parse(Item.AxisNo) == AxisNo).First();
                p1.SetAxis((int)a.Key, p.Position);
            }

            return p1;
        }

        private static LaserX_9078_Utilities.PmCartesian AxesPosition_To_PmCartesian
            (
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict,
            AxesPosition Position     //目标
            )
        {
            LaserX_9078_Utilities.PmCartesian p1 = new LaserX_9078_Utilities.PmCartesian();

            //建立新结构
            Dictionary<PmTrajAxisType, Motor_LaserX_9078> axis9078 = new Dictionary<PmTrajAxisType, Motor_LaserX_9078>();

            foreach (var item in axisDict)
            {
                Motor_LaserX_9078 t = item.Value as Motor_LaserX_9078;
                axis9078.Add(item.Key, t);
            }

            int CardNo = axis9078.First().Value.CardNo;

            //检查数据点坐标
            foreach (var a in axis9078)
            {
                int AxisNo = a.Value.AxisNo;
                var p = Position.ItemCollection.Where(Item => int.Parse(Item.CardNo) == CardNo && int.Parse(Item.AxisNo) == AxisNo).First();
                p1.SetAxis((int)a.Key, p.Position);
            }

            return p1;
        }

        private static int TrajAxisGroup_LinearMove(
            ref int id,
            int CardNo,
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict,
            AxesPosition Position,     //目标
            double TrajMaxSpeed,    //速度
            double TrajMaxAcc,      //加速度
            bool Caption)  //运动, 但不捕捉数据
        {
            int errorCode = ErrorCodes.NoError;
            int rc = 0;

            string exMsg = "";

            //得到插补坐标
            var p1 = AxesPosition_To_MotPose(axisDict, Position);

            int cap = 0;
            if (Caption) cap = 1;

            //运动到P1点
            LaserX_9078_Utilities.P9078_TrajSetMotionId(CardNo, id++);    //为每条插补指令分配一个ID号
            rc = LaserX_9078_Utilities.P9078_TrajLinearMoveEx(CardNo, ref p1, TrajMaxSpeed, TrajMaxAcc, cap);       //不捕捉
            if (rc != 0)
            {
                exMsg = $"执行错误 P9078_TrajLinearMoveEx [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                return ErrorCodes.Unexecuted;
            }

            return errorCode;
        }

        /// <summary>
        /// 等待插补规划器运动到位
        /// </summary>
        /// <param name="dev">控制卡卡号</param>
        /// <returns>零表示到位；非零表示出错，如限位有效、伺服告警导致插补规划器或控制轴被禁止</returns>
        private static int WaitForTrajInpos(int dev, CancellationToken token)
        {
            int rc = 0;
            //LaserX_9078_Utilities.MOT_TRAJ_STAT trajStat = new LaserX_9078_Utilities.MOT_TRAJ_STAT();
            LaserX_9078_Utilities.MOT_TRAJ_STAT trajStat = default(LaserX_9078_Utilities.MOT_TRAJ_STAT);
            for (; ; )
            {
                rc = LaserX_9078_Utilities.P9078_MotionUpdate(dev);
                if (rc == 0)
                {
                    LaserX_9078_Utilities.P9078_MotionGetTrajStatus(dev, ref trajStat, Marshal.SizeOf(trajStat));
                    if (trajStat.inpos != 0)
                        return 0;
                    if (trajStat.enabled == 0)
                        return -1;
                }
                System.Threading.Thread.Sleep(1);   //release CPU resource


                if (token.IsCancellationRequested)
                {
                    //中断
                    LaserX_9078_Utilities.P9078_TrajAbort(dev);

                    Thread.Sleep(5);

                    Log_Global("用户取消测试");
                    token.ThrowIfCancellationRequested();
                    throw new OperationCanceledException();
                }
            }
            return -2;
        }

        private static int Traj_Run(
               int CardNo,
               Dictionary<PmTrajAxisType, MotorAxisBase> axisDict, //插补轴  分量与轴的对应关系
               TrajThresholdStop thresholdStop,  //阈值停止
               CancellationToken token,
               out TrajResultItem Traj_CaptionResult)
        {
            int errorCode = ErrorCodes.NoError;
            int rc = 0;

            string exMsg = "";

            //建立返回的数据结构
            Traj_CaptionResult = new TrajResultItem();
            Traj_CaptionResult.Id = new List<int>();    //ID
            Traj_CaptionResult.DataIndex = new List<int>();    //序列号
            Traj_CaptionResult.MotorPos_mm = new Dictionary<Motor_LaserX_9078, List<double>>(); //电机位置
            Traj_CaptionResult.ADC = new Dictionary<int, List<short>>();    //模拟量
            Traj_CaptionResult.Voltage_mV = new Dictionary<int, List<double>>(); //电压
            Traj_CaptionResult.Current_mA = new Dictionary<int, List<double>>();

            //建立新结构
            Dictionary<PmTrajAxisType, Motor_LaserX_9078> axis9078 = new Dictionary<PmTrajAxisType, Motor_LaserX_9078>();

            foreach (var item in axisDict)
            {
                if (item.Value.GetType() != typeof(Motor_LaserX_9078))
                {
                    exMsg = $"插补捕捉功能, 仅支持LaserX_9078类型的轴";
                    Log_Global($"Traj_Run Error - {exMsg}");
                    return ErrorCodes.Unexecuted;
                }

                Motor_LaserX_9078 t = item.Value as Motor_LaserX_9078;

                axis9078.Add(item.Key, t);
            }

            //建立返回的数据结构
            foreach (var item in axis9078)
            {
                Traj_CaptionResult.MotorPos_mm.Add(item.Value, new List<double>());
            }
            for (int i = 0; i < LaserX_9078_Utilities.MOT_MAX_AIO; i++)
            {
                Traj_CaptionResult.ADC.Add(i, new List<short>());
                Traj_CaptionResult.Voltage_mV.Add(i, new List<double>());
                Traj_CaptionResult.Current_mA.Add(i, new List<double>());
            }

            //LaserX_9078_Utilities.MOT_TRAJ_STAT trajStat = new LaserX_9078_Utilities.MOT_TRAJ_STAT();
            LaserX_9078_Utilities.MOT_TRAJ_STAT trajStat = default(LaserX_9078_Utilities.MOT_TRAJ_STAT);
            uint[] fifoStatus = new uint[8];

            //等待运动完成
            //rc = WaitForTrajInpos(CardNo, token);
            //if (rc != 0)
            //{
            //    exMsg = $"执行错误 WaitForTrajInpos [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
            //    Log_Global($"Traj_Run Error - {exMsg}");
            //    return ErrorCodes.Unexecuted;
            //}

            LaserX_9078_Utilities.FIFO_ITEM[] fifoItems = new LaserX_9078_Utilities.FIFO_ITEM[1024];

            //等待运动完成并读取数据
            for (; ; )
            {
                //更新状态
                rc = LaserX_9078_Utilities.P9078_MotionUpdate(CardNo);
                if (rc == 0)
                {
                    LaserX_9078_Utilities.P9078_MotionGetTrajStatus(CardNo, ref trajStat, Marshal.SizeOf(trajStat));
                    if (trajStat.inpos != 0)
                    {
                        Thread.Sleep(10);  //等待最后的数据充满缓冲区
                    }
                    if (trajStat.enabled == 0)
                    {
                        exMsg = $"插补中执行错误 WaitForTrajInpos [trajStat.enabled = 0]";
                        Log_Global($"Traj_Run Error - {exMsg}");
                        return ErrorCodes.Unexecuted;
                    }
                    System.Threading.Thread.Sleep(1);   //release CPU resource

                    #region 接收并分析缓冲区的数据

                    //fifoStatus[0]: fifoItemSize
                    //fifoStatus[1]: fifoCountForRead
                    //fifoStatus[2]: fifoFull
                    //fifoStatus[3]: fifoEnabled
                    //fifoStatus[4]: fifoReturnCount of last P9076_MotionReadFifo
                    //fifoStatus[5]: fifoBufferSize
                    rc = LaserX_9078_Utilities.P9078_MotionGetFifoStatus(CardNo, fifoStatus, fifoStatus.Length);
                    if (rc != 0)
                        throw new Exception(string.Format("MotionGetFifoStatus[{0}] fails({1})", CardNo, rc));

                    if (fifoStatus[2] != 0)
                    {
                        exMsg = "ERROR: capture fifo full, data might be corrupted!";
                        Log_Global($"Traj_Run Error - {exMsg}");
                    }

                    //Log(string.Format("Number of data items: {0}, item size: {1} bytes, buffer size: {2} bytes, capture enabled: {3}\n",
                    //    fifoStatus[1], fifoStatus[0], fifoStatus[5], fifoStatus[3]));

                    if (fifoStatus[1] > 0)  //缓冲区里面有数据
                    {
                        //LaserX_9078_Utilities.FIFO_ITEM[] fifoItems = new LaserX_9078_Utilities.FIFO_ITEM[1024];
                        int actCount = 0;
                        int readCount = 0;
                        rc = 0;

                        Stopwatch sw = new Stopwatch();
                        sw.Start();

                        for (; ; )
                        {
                            actCount = 0;
                            rc = LaserX_9078_Utilities.P9078_MotionReadFifo(CardNo, fifoItems, fifoItems.Length, ref actCount);
                            if (actCount > fifoItems.Length)
                            {
                                exMsg = $"ERROR: dev[{CardNo}] actcount{actCount}) larger than read buffer length({fifoItems.Length})";
                                Log_Global($"Traj_Run Error - {exMsg}");
                            }

                            //foreach (var item in objAxis)
                            //{
                            //    pos += string.Format("{0}\t", item.Name);
                            //}
                            //pos += "\r\n";

                            for (int n = 0; n < actCount; n++)
                            {
                                //pos += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\r\n",
                                //    fifoItems[n].id,
                                //    fifoItems[n].pos0, fifoItems[n].pos1, fifoItems[n].pos2, fifoItems[n].pos3, fifoItems[n].pos4, fifoItems[n].pos5,
                                //    fifoItems[n].ain0, fifoItems[n].ain2
                                //    );

                                var _pos = fifoItems[n].GetPos();
                                var _adc = fifoItems[n].GetAin();
                                var _voltage_mv = fifoItems[n].GetVoltage_mV();
                                var _current_ma = fifoItems[n].GetCurrent_mA(CardNo);
                                var _id = fifoItems[n].id;
                                var _index = fifoItems[n].rsv; //数据编号

                                //id加入
                                Traj_CaptionResult.Id.Add(_id);
                                Traj_CaptionResult.DataIndex.Add(_index);

                                //位置加入
                                foreach (var item in axis9078)
                                {
                                    int index = (int)item.Key;
                                    Traj_CaptionResult.MotorPos_mm[item.Value].Add(Math.Round(_pos[item.Value.AxisNo], 9));
                                }

                                //模拟量加入
                                for (int i = 0; i < LaserX_9078_Utilities.MOT_MAX_AIO; i++)
                                {
                                    Traj_CaptionResult.ADC[i].Add(_adc[i]);
                                    Traj_CaptionResult.Voltage_mV[i].Add(Math.Round(_voltage_mv[i], 3));
                                    Traj_CaptionResult.Current_mA[i].Add(Math.Round(_current_ma[i], 9));
                                }


                            }
                            readCount += actCount;
                            //Log(string.Format("dev[{0}] MotionReadFifo actcnt: {1}, read count: {2}\n", dev, actCount, readCount));

                            //20241128 捕捉中判断
                            if(thresholdStop.En)
                            {
                                bool finded = false;
                                
                                if (thresholdStop.ThVoltage_mV!=null)
                                {
                                    foreach (var Curritem in thresholdStop.ThVoltage_mV)
                                    {
                                        if (Traj_CaptionResult.Voltage_mV[Curritem.Key].Max() > Curritem.Value)
                                        {
                                            finded = true;
                                            break;
                                        }
                                    }
                                }

                                if (thresholdStop.ThCurrent_mA!=null)
                                {
                                    foreach (var Curritem in thresholdStop.ThCurrent_mA)
                                    {
                                        if (Traj_CaptionResult.Current_mA[Curritem.Key].Max() > Curritem.Value)
                                        {
                                            finded = true;
                                            break;
                                        }
                                    }
                                }

                                if(finded) //找到了阈值
                                {
                                    //直接停止插补退出
                                    Log_Global($"Traj_Run 发现目标阈值快速完成!");
                                    //中断
                                    LaserX_9078_Utilities.P9078_TrajAbort(CardNo);

                                    return errorCode;


                                }
                            }




                            //20240718 增加读取数据超时记录
                            if (sw.ElapsedMilliseconds > 10 * 1000)
                            {
                                exMsg = $"取回数据超时:当前需要取回数据[{readCount}] < 需要取回数据[{fifoStatus[1]}]";
                                Log_Global($"Traj_Run Error - {exMsg}");

                                string LogDataMsg = Path.Combine(System.Windows.Forms.Application.StartupPath + $@"\取回数据超时信息_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                                WriteCSCVFile(LogDataMsg, Traj_CaptionResult);

                                break;
                            }

                            if (token.IsCancellationRequested)
                            {
                                //中断
                                LaserX_9078_Utilities.P9078_TrajAbort(CardNo);

                                Thread.Sleep(5);

                                Log_Global("用户取消测试");
                                token.ThrowIfCancellationRequested();
                                throw new OperationCanceledException();
                            }

                            //dump fifo status
                            //{
                            //    int r0, r1;
                            //    uint[] buff = new uint[8];
                            //    r0 = PCI9076.P9076_MotionUpdate();
                            //    r1 = PCI9076.P9076_MotionGetFifoStatus(0, buff, buff.Length);
                            //    if (r0 == 0 && r1 == 0)
                            //    {
                            //        Log(string.Format("number of data items: {0}, item size: {1} bytes, capture buffer size: {2} bytes, enabled: {3}",
                            //        buff[1], buff[0], buff[5], buff[3]));
                            //    }
                            //}
                            Thread.Sleep(5);

                            if(trajStat.inpos != 0)   //如果耦合完成后的最后一波数据, 必须全部收了
                            {
                                if(((uint)readCount >= fifoStatus[1]))
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        //while (
                        //while (rc == 0 && ((uint)readCount < fifoStatus[1]));
                    }


                    #endregion
                    if (trajStat.inpos != 0)
                    {                        
                        break;   //捕捉完成, 数据取回完成
                    }
                }

                if (token.IsCancellationRequested)
                {
                    //中断
                    LaserX_9078_Utilities.P9078_TrajAbort(CardNo);

                    Thread.Sleep(5);

                    Log_Global("用户取消测试");
                    token.ThrowIfCancellationRequested();
                    throw new OperationCanceledException();
                }
            }

            //数据已经全部取回来了
            return errorCode;
        }



        private static int Traj_Run_备份(
               int CardNo,
               Dictionary<PmTrajAxisType, MotorAxisBase> axisDict, //插补轴  分量与轴的对应关系
               CancellationToken token,
               out TrajResultItem Traj_CaptionResult)
        {
            int errorCode = ErrorCodes.NoError;
            int rc = 0;

            string exMsg = "";

            //建立返回的数据结构
            Traj_CaptionResult = new TrajResultItem();
            Traj_CaptionResult.Id = new List<int>();    //ID
            Traj_CaptionResult.DataIndex = new List<int>();    //序列号
            Traj_CaptionResult.MotorPos_mm = new Dictionary<Motor_LaserX_9078, List<double>>(); //电机位置
            Traj_CaptionResult.ADC = new Dictionary<int, List<short>>();    //模拟量
            Traj_CaptionResult.Voltage_mV = new Dictionary<int, List<double>>(); //电压
            Traj_CaptionResult.Current_mA = new Dictionary<int, List<double>>();

            //建立新结构
            Dictionary<PmTrajAxisType, Motor_LaserX_9078> axis9078 = new Dictionary<PmTrajAxisType, Motor_LaserX_9078>();

            foreach (var item in axisDict)
            {
                if (item.Value.GetType() != typeof(Motor_LaserX_9078))
                {
                    exMsg = $"插补捕捉功能, 仅支持LaserX_9078类型的轴";
                    Log_Global($"Traj_Run Error - {exMsg}");
                    return ErrorCodes.Unexecuted;
                }

                Motor_LaserX_9078 t = item.Value as Motor_LaserX_9078;

                axis9078.Add(item.Key, t);
            }

            //建立返回的数据结构
            foreach (var item in axis9078)
            {
                Traj_CaptionResult.MotorPos_mm.Add(item.Value, new List<double>());
            }
            for (int i = 0; i < LaserX_9078_Utilities.MOT_MAX_AIO; i++)
            {
                Traj_CaptionResult.ADC.Add(i, new List<short>());
                Traj_CaptionResult.Voltage_mV.Add(i, new List<double>());
                Traj_CaptionResult.Current_mA.Add(i, new List<double>());
            }

            //LaserX_9078_Utilities.MOT_TRAJ_STAT trajStat = new LaserX_9078_Utilities.MOT_TRAJ_STAT();
            LaserX_9078_Utilities.MOT_TRAJ_STAT trajStat = default(LaserX_9078_Utilities.MOT_TRAJ_STAT);
            uint[] fifoStatus = new uint[8];

            //等待运动完成
            rc = WaitForTrajInpos(CardNo, token);
            if (rc != 0)
            {
                exMsg = $"执行错误 WaitForTrajInpos [{LaserX_9078_Utilities.P9078_ErrIDInfo(rc)}]";
                Log_Global($"Traj_Run Error - {exMsg}");
                return ErrorCodes.Unexecuted;
            }

            string pos = "";

            //while (true)
            {
                rc = LaserX_9078_Utilities.P9078_MotionUpdate(CardNo);
                if (rc == 0)
                {
                    LaserX_9078_Utilities.P9078_MotionGetTrajStatus(CardNo, ref trajStat, Marshal.SizeOf(trajStat));
                    //Log(string.Format("dev[{0}] current command position: {1:f1}, {2:f1}\n", dev, trajStat.cmdPos.x, trajStat.cmdPos.y));

                    //fifoStatus[0]: fifoItemSize
                    //fifoStatus[1]: fifoCountForRead
                    //fifoStatus[2]: fifoFull
                    //fifoStatus[3]: fifoEnabled
                    //fifoStatus[4]: fifoReturnCount of last P9076_MotionReadFifo
                    //fifoStatus[5]: fifoBufferSize
                    rc = LaserX_9078_Utilities.P9078_MotionGetFifoStatus(CardNo, fifoStatus, fifoStatus.Length);
                    if (rc != 0)
                        throw new Exception(string.Format("MotionGetFifoStatus[{0}] fails({1})", CardNo, rc));

                    if (fifoStatus[2] != 0)
                    {
                        exMsg = "ERROR: capture fifo full, data might be corrupted!";
                        Log_Global($"Traj_Run Error - {exMsg}");
                    }

                    //Log(string.Format("Number of data items: {0}, item size: {1} bytes, buffer size: {2} bytes, capture enabled: {3}\n",
                    //    fifoStatus[1], fifoStatus[0], fifoStatus[5], fifoStatus[3]));

                    if (fifoStatus[1] > 0)
                    {
                        LaserX_9078_Utilities.FIFO_ITEM[] fifoItems = new LaserX_9078_Utilities.FIFO_ITEM[1024];
                        int actCount = 0;
                        int readCount = 0;
                        rc = 0;

                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        do
                        {
                            actCount = 0;
                            rc = LaserX_9078_Utilities.P9078_MotionReadFifo(CardNo, fifoItems, fifoItems.Length, ref actCount);
                            if (actCount > fifoItems.Length)
                            {
                                exMsg = $"ERROR: dev[{CardNo}] actcount{actCount}) larger than read buffer length({fifoItems.Length})";
                                Log_Global($"Traj_Run Error - {exMsg}");
                            }

                            //foreach (var item in objAxis)
                            //{
                            //    pos += string.Format("{0}\t", item.Name);
                            //}
                            //pos += "\r\n";

                            for (int n = 0; n < actCount; n++)
                            {
                                //pos += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\r\n",
                                //    fifoItems[n].id,
                                //    fifoItems[n].pos0, fifoItems[n].pos1, fifoItems[n].pos2, fifoItems[n].pos3, fifoItems[n].pos4, fifoItems[n].pos5,
                                //    fifoItems[n].ain0, fifoItems[n].ain2
                                //    );

                                var _pos = fifoItems[n].GetPos();
                                var _adc = fifoItems[n].GetAin();
                                var _voltage_mv = fifoItems[n].GetVoltage_mV();
                                var _current_ma = fifoItems[n].GetCurrent_mA(CardNo);
                                var _id = fifoItems[n].id;
                                var _index = fifoItems[n].rsv; //数据编号

                                //id加入
                                Traj_CaptionResult.Id.Add(_id);
                                Traj_CaptionResult.DataIndex.Add(_index);

                                //位置加入
                                foreach (var item in axis9078)
                                {
                                    int index = (int)item.Key;
                                    Traj_CaptionResult.MotorPos_mm[item.Value].Add(Math.Round(_pos[item.Value.AxisNo], 9));
                                }

                                //模拟量加入
                                for (int i = 0; i < LaserX_9078_Utilities.MOT_MAX_AIO; i++)
                                {
                                    Traj_CaptionResult.ADC[i].Add(_adc[i]);
                                    Traj_CaptionResult.Voltage_mV[i].Add(Math.Round(_voltage_mv[i], 3));
                                    Traj_CaptionResult.Current_mA[i].Add(Math.Round(_current_ma[i], 9));
                                }
                            }
                            readCount += actCount;
                            //Log(string.Format("dev[{0}] MotionReadFifo actcnt: {1}, read count: {2}\n", dev, actCount, readCount));

                            //20240718 增加读取数据超时记录
                            if (sw.ElapsedMilliseconds > 10 * 1000)
                            {
                                exMsg = $"取回数据超时:当前取回数据[{readCount}] < 需要取回数据[{fifoStatus[1]}]";
                                Log_Global($"Traj_Run Error - {exMsg}");

                                string LogDataMsg = Path.Combine(System.Windows.Forms.Application.StartupPath + $@"\取回数据超时信息_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                                WriteCSCVFile(LogDataMsg, Traj_CaptionResult);

                                break;
                            }

                            if (token.IsCancellationRequested)
                            {
                                //中断
                                LaserX_9078_Utilities.P9078_TrajAbort(CardNo);

                                Thread.Sleep(5);

                                Log_Global("用户取消测试");
                                token.ThrowIfCancellationRequested();
                                throw new OperationCanceledException();
                            }

                            //dump fifo status
                            //{
                            //    int r0, r1;
                            //    uint[] buff = new uint[8];
                            //    r0 = PCI9076.P9076_MotionUpdate();
                            //    r1 = PCI9076.P9076_MotionGetFifoStatus(0, buff, buff.Length);
                            //    if (r0 == 0 && r1 == 0)
                            //    {
                            //        Log(string.Format("number of data items: {0}, item size: {1} bytes, capture buffer size: {2} bytes, enabled: {3}",
                            //        buff[1], buff[0], buff[5], buff[3]));
                            //    }
                            //}
                            Thread.Sleep(5);
                        }
                        while (rc == 0 && ((uint)readCount < fifoStatus[1]));
                    }
                }
            }

            //数据已经全部取回来了
            return errorCode;
        }
        #endregion 插补支持函数

        /// <summary>
        /// 写CSV文档
        /// </summary>
        /// <param name="LogDataMsg"></param>
        /// <param name="strb"></param>
        /// <param name="sw"></param>
        /// <param name="result"></param>
        private static void WriteCSCVFile(string LogDataMsg, TrajResultItem result)
        {
            //Log_Global($"原始数据:[{LogDataMsg}]");
            StringBuilder strb = PrintCSV(result);
            StreamWriter sw = new StreamWriter(LogDataMsg);
            sw.Write(strb.ToString());
            sw.Close(); strb.Clear();
        }

        private static StringBuilder PrintCSV(TrajResultItem result)
        {
            StringBuilder sb = new StringBuilder();
            //try
            //{
            string str = "";
            {
                str = $"Id,";
                foreach (var item in result.MotorPos_mm)
                {
                    str += $"{item.Key.Name},";
                }
                foreach (var item in result.Voltage_mV)
                {
                    str += $"Ch{item.Key},";
                }
                foreach (var item in result.Current_mA)
                {
                    str += $"Ch{item.Key}_mA,";
                }
                sb.AppendLine(str);
            }

            int count = result.Id.Count;
            for (int j = 0; j < count; j++)
            {
                str = $"{result.Id[j]}_{result.DataIndex[j]},";
                foreach (var item in result.MotorPos_mm)
                {
                    str += $"{item.Value[j]},";
                }
                foreach (var item in result.Voltage_mV)
                {
                    str += $"{item.Value[j]},";
                }
                foreach (var item in result.Current_mA)
                {
                    str += $"{item.Value[j]},";
                }
                sb.AppendLine(str);
            }

            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}


            return sb;
        }



        #endregion 直线插补等
    }
}