using LX_BurnInSolution.Utilities;
using SolveWare_Analog;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_FF2")]

    #region 轴、位置、IO、仪器
    [StaticResource(ResourceItemType.AXIS, "右短摆臂旋转", "右短摆臂")]
    [StaticResource(ResourceItemType.AXIS, "右长摆臂旋转", "右长摆臂")]
    [StaticResource(ResourceItemType.AXIS, "右偏振片旋转", "右偏振片")]

    [StaticResource(ResourceItemType.POS, "右长摆臂旋转绝对零位", "右长摆臂零位")]
    [StaticResource(ResourceItemType.POS, "右短摆臂旋转绝对零位", "右短摆臂零位")]

    [StaticResource(ResourceItemType.IO, "Input_右PER前后移动气缸动作", "右PER前后动作")]
    [StaticResource(ResourceItemType.IO, "Input_右PER前后移动气缸复位", "右PER前后复位")]
    [StaticResource(ResourceItemType.IO, "Input_右PER避位气缸动作", "右PER避位气缸动作")]
    [StaticResource(ResourceItemType.IO, "Input_右PER避位气缸复位", "右PER避位气缸复位")]

    [StaticResource(ResourceItemType.IO, "Input_右PER上下移动气缸动作", "右PER上下动作")]
    [StaticResource(ResourceItemType.IO, "Input_右PER上下移动气缸复位", "右PER上下复位")]

    [StaticResource(ResourceItemType.IO, "Output_右PER前后移动气缸电磁阀", "右PER前后电磁阀")]
    [StaticResource(ResourceItemType.IO, "Output_右PER避位气缸电磁阀", "右PER避位电磁阀")]

    [StaticResource(ResourceItemType.IO, "Output_右PER上下移动气缸电磁阀_下降", "右PER上下_下降")]
    [StaticResource(ResourceItemType.IO, "Output_右PER上下移动气缸电磁阀_上升", "右PER上下_上升")]

    //[ConfigurableInstrument("ISourceMeter_Golight", "GoLight_2", "用于驱动器件")]
    #endregion

    public class TestModule_FF2 : TestModuleBase
    {

        public TestModule_FF2() : base() { }

        #region 以Get获取资源
        MotorAxisBase RightShort { get { return (MotorAxisBase)this.ModuleResource["右短摆臂旋转"]; } }
        MotorAxisBase RightLong { get { return (MotorAxisBase)this.ModuleResource["右长摆臂旋转"]; } }
        MotorAxisBase RightPer { get { return (MotorAxisBase)this.ModuleResource["右偏振片旋转"]; } }
        AxesPosition RightShortZero { get { return (AxesPosition)this.ModuleResource["右短摆臂旋转绝对零位"]; } }
        AxesPosition RightLongZero { get { return (AxesPosition)this.ModuleResource["右长摆臂旋转绝对零位"]; } }

        IOBase In_RightPerFrontAction { get { return (IOBase)this.ModuleResource["Input_右PER前后移动气缸动作"]; } }
        IOBase In_RightPerFrontRest { get { return (IOBase)this.ModuleResource["Input_右PER前后移动气缸复位"]; } }

        IOBase In_RightPerAvoidAction { get { return (IOBase)this.ModuleResource["Input_右PER避位气缸动作"]; } }
        IOBase In_RightPerAvoidRest { get { return (IOBase)this.ModuleResource["Input_右PER避位气缸复位"]; } }

        IOBase In_RightPerAction { get { return (IOBase)this.ModuleResource["Input_右PER上下移动气缸动作"]; } }
        IOBase In_RightPerRest { get { return (IOBase)this.ModuleResource["Input_右PER上下移动气缸复位"]; } }

        IOBase Out_RightPerFront { get { return (IOBase)this.ModuleResource["Output_右PER前后移动气缸电磁阀"]; } }
        IOBase Out_RightPerAvoid { get { return (IOBase)this.ModuleResource["Output_右PER避位气缸电磁阀"]; } }

        IOBase Out_RightPerDown { get { return (IOBase)this.ModuleResource["Output_右PER上下移动气缸电磁阀_下降"]; } }
        IOBase Out_RightPerUp { get { return (IOBase)this.ModuleResource["Output_右PER上下移动气缸电磁阀_上升"]; } }

        //ISourceMeter_Golight sourceMeter { get { return (ISourceMeter_Golight)this.ModuleResource["GoLight_2"]; } }



        #endregion

        TestRecipe_FF2 TestRecipe { get; set; }
        RawData_FF2 RawData { get; set; }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_FF2(); return RawData;
        }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_FF2);
        }
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_FF2>(testRecipe);
        }

        public override void Run(CancellationToken token)
        {
            try
            {
                FarFieldRawDataCollection farField_Short = null;
                FarFieldRawDataCollection farField_Long = null;

                //if (this.sourceMeter == null)
                //{
                //    this._core.Log_Global("仪器连接错误！！！");
                //    return;
                //}
                //if (!this.sourceMeter.IsOnline)
                //{
                //    this.Log_Global("仪表异常，取消测试！");
                //    sourceMeter.Timeout_ms = 1000;
                //    sourceMeter.CurrentSetpoint_A = 0;
                //    sourceMeter.VoltageSetpoint_V = 0;
                //    sourceMeter.VoltageSetpoint_PD_V = 0;
                //    sourceMeter.VoltageSetpoint_EA_V = 0;
                //    sourceMeter.IsOutputEnable = false;
                //    return;
                //}
                //sourceMeter.Timeout_ms = 60 * 1000;

                //this._core.Log_Global("开始加电");
                //this.sourceMeter.IsOutputEnable = true;
                //this.sourceMeter.CurrentSetpoint_A = Convert.ToSingle(this.TestRecipe.Current_A);

                if (this.TestRecipe.IsRightShort)
                {
                    if (!In_RightPerFrontRest.Interation.IsActive)
                    {
                        Out_RightPerFront.TurnOn(false);
                    }
                    if (!In_RightPerAvoidRest.Interation.IsActive)
                    {
                        Out_RightPerAvoid.TurnOn(false);
                    }
                    if (!In_RightPerRest.Interation.IsActive)
                    {
                        Out_RightPerDown.TurnOn(false);
                        Out_RightPerUp.TurnOn(true);
                    }
                    Analog_LaserX_9078.SetSenseCurrentRange_mA(RightShort as Motor_LaserX_9078, 0, this.TestRecipe.ShortMaxCurrent);
                    var Rightan = Analog_LaserX_9078.GetSenseCurrentRange_mA(RightShort as Motor_LaserX_9078, 0);


                    RightShort.HomeRun();
                    RightShort.WaitHomeDone(new CancellationTokenSource());//有问题

                    farField_Short = SingleAxisScan(RightShort, this.TestRecipe.ShortSpeed, 0, RightShortZero.ItemCollection[0].Position,
                        this.TestRecipe.ShortStartAngle, this.TestRecipe.ShortStopAngle, 0, 0, token);

                    //for (int i = 0; i < farField_Short.Point_Theta.Length; i++)
                    //{
                    //    RawData.Add(new RawDatumItem_FF2()
                    //    {
                    //        Short_Theta = farField_Short.Point_Theta[i],
                    //        Short_Point_PD_Reading = farField_Short.Point_PD_Reading[i],
                    //        Short_Point_PD_ReadingTo1 = farField_Short.Point_PD_ReadingTo1[i],
                    //    });
                    //}

                }
                if (this.TestRecipe.IsRightLong)
                {
                    if (!In_RightPerFrontRest.Interation.IsActive)
                    {
                        Out_RightPerFront.TurnOn(false);
                    }
                    if (!In_RightPerAvoidRest.Interation.IsActive)
                    {
                        Out_RightPerAvoid.TurnOn(false);
                    }
                    if (!In_RightPerRest.Interation.IsActive)
                    {
                        Out_RightPerDown.TurnOn(false);
                        Out_RightPerUp.TurnOn(true);
                    }
                    Analog_LaserX_9078.SetSenseCurrentRange_mA(RightShort as Motor_LaserX_9078, 1, this.TestRecipe.LongMaxCurrent);
                    var Rightan = Analog_LaserX_9078.GetSenseCurrentRange_mA(RightShort as Motor_LaserX_9078, 1);
                    RightLong.HomeRun();
                    RightLong.WaitHomeDone(new CancellationTokenSource());//有问题

                    farField_Long = SingleAxisScan(RightLong, this.TestRecipe.LongSpeed, 1, RightLongZero.ItemCollection[0].Position,
                        this.TestRecipe.LongStartAngle, this.TestRecipe.LongStopAngle, 0, 0, token);

                    //for (int i = 0; i < farField_Long.Point_Theta.Length; i++)
                    //{
                    //    RawData.Add(new RawDatumItem_FF2()
                    //    {
                    //        Long_Theta = farField.Point_Theta[i],
                    //        Long_Point_PD_Reading = farField.Point_PD_Reading[i],
                    //        Long_Point_PD_ReadingTo1 = farField.Point_PD_ReadingTo1[i],
                    //    });
                    //}

                }

                for (int i = 0; i < farField_Long.Point_Theta.Length; i++)
                {
                    RawDatumItem_FF2 rawDatum2 = new RawDatumItem_FF2();
                    if (this.TestRecipe.IsRightShort)
                    {
                        rawDatum2.Short_Theta = farField_Short.Point_Theta[i];
                        rawDatum2.Short_Point_PD_Reading = farField_Short.Point_PD_Reading[i];
                        rawDatum2.Short_Point_PD_ReadingTo1 = farField_Short.Point_PD_ReadingTo1[i];
                    }
                    if (this.TestRecipe.IsRightLong)
                    {
                        rawDatum2.Long_Theta = farField_Long.Point_Theta[i];
                        rawDatum2.Long_Point_PD_Reading = farField_Long.Point_PD_Reading[i];
                        rawDatum2.Long_Point_PD_ReadingTo1 = farField_Long.Point_PD_ReadingTo1[i];
                    }

                    RawData.Add(rawDatum2);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //this.sourceMeter.IsOutputEnable = false;
            }

        }

        protected FarFieldRawDataCollection SingleAxisScan(
            MotorAxisBase FFAxis, //轴
            double Traj_Speed,  //速度
            int AnalogChannel,  //模拟量通道
            double phyZeroAngle, //逻辑零角度
            double startAngle,  //开始
            double stopAngle,    //结束
            double StepAngle,   //步进
            double Delay_ms,    //单步延时
            CancellationToken token)
        {

            FarFieldRawDataCollection ffrawCol = new FarFieldRawDataCollection();
            ffrawCol.FFAxisName = FFAxis.Name;

            //计算出扫描角度
            double phyStartAngle = phyZeroAngle + startAngle;
            double phyStopAngle = phyZeroAngle + stopAngle;

            Motor_LaserX_9078 ffa = FFAxis as Motor_LaserX_9078;

            AxesPosition P1 = new AxesPosition();
            P1.AddSingleItem(new AxisPosition() { Name = ffa.Name, CardNo = ffa.CardNo.ToString(), AxisNo = ffa.AxisNo.ToString(), Position = phyStartAngle });

            AxesPosition P2 = new AxesPosition();
            P2.AddSingleItem(new AxisPosition() { Name = ffa.Name, CardNo = ffa.CardNo.ToString(), AxisNo = ffa.AxisNo.ToString(), Position = phyStopAngle });


            //运动到开始点
            FFAxis.MoveToV3(phyStartAngle, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
            FFAxis.WaitMotionDone();
            Thread.Sleep(50);

            //建立 X,Z 方向插补




            //    objTraj = new LaserX_9078_Traj_Function();

            //objTraj.

            //(FFAxis, null, null, null, null, null);

            try
            {


                Dictionary<LaserX_9078_Traj_Function.PmTrajAxisType, MotorAxisBase> axisDict = new Dictionary<LaserX_9078_Traj_Function.PmTrajAxisType, MotorAxisBase>();
                axisDict.Add(LaserX_9078_Traj_Function.PmTrajAxisType.X_Dir, FFAxis);

                //执行运动捕捉
                LaserX_9078_Traj_Function.TrajResultItem result_P1_P2;
                LaserX_9078_Traj_Function.Parallel_MoveLine(axisDict, P1, P2, Traj_Speed, out result_P1_P2, token);


                //执行运动捕捉
                LaserX_9078_Traj_Function.TrajResultItem result_P2_P1;
                LaserX_9078_Traj_Function.Parallel_MoveLine(axisDict, P2, P1, Traj_Speed, out result_P2_P1, token);


                //处理输出结果
                ////输出csv(测试用)
                //if (true)
                //{
                //    string path = "c:\\Test";
                //    if (!Directory.Exists(path))
                //    {
                //        Directory.CreateDirectory(path);
                //    }
                //    var strb = objTraj.PrintCSV(Traj_Result);
                //    var sw = new StreamWriter(path + $@"\3D线插补_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                //    sw.Write(strb.ToString());
                //    sw.Close();
                //    strb.Clear();
                //}

                //P1->P2
                List<double> pList_P1_P2 = result_P1_P2.Current_mA[AnalogChannel];//运动卡模拟量通道
                List<double> thetaList_P1_P2 = result_P1_P2.MotorPos_mm[ffa];
                //移动平均法平滑
                List<double> smoothList_P1_P2 = new List<double>();
                smoothList_P1_P2.AddRange(ArrayMath.CalculateMovingAverage(pList_P1_P2.ToArray(), 11));

                //P2->P1
                List<double> pList_P2_P1 = result_P2_P1.Current_mA[AnalogChannel];//运动卡模拟量通道
                List<double> thetaList_P2_P1 = result_P2_P1.MotorPos_mm[ffa];
                //移动平均法平滑
                List<double> smoothList_P2_P1 = new List<double>();
                smoothList_P2_P1.AddRange(ArrayMath.CalculateMovingAverage(pList_P2_P1.ToArray(), 11));

                //翻回来
                thetaList_P2_P1.Reverse();
                smoothList_P2_P1.Reverse();

                //截取相同数量
                int PCnt = Math.Min(smoothList_P1_P2.Count, smoothList_P2_P1.Count);
                thetaList_P1_P2.RemoveRange(PCnt, thetaList_P1_P2.Count - PCnt);
                thetaList_P2_P1.RemoveRange(PCnt, thetaList_P2_P1.Count - PCnt);
                smoothList_P1_P2.RemoveRange(PCnt, smoothList_P1_P2.Count - PCnt);
                smoothList_P2_P1.RemoveRange(PCnt, smoothList_P2_P1.Count - PCnt);

                List<double> thetaList = new List<double>();
                List<double> smoothList = new List<double>();
                {
                    //曲线平移偏移量

                    //阈值设定
                    double Th_P1_P2 = (smoothList_P1_P2.Max() - smoothList_P1_P2.Min()) / 2 + smoothList_P1_P2.Min();
                    double Th_P2_P1 = (smoothList_P2_P1.Max() - smoothList_P2_P1.Min()) / 2 + smoothList_P2_P1.Min();

                    //总偏移量
                    int OffsetCount = 0;
                    for (int i = 0; i < PCnt; i++)
                    {
                        double diff1 = 0;
                        if (smoothList_P1_P2[i] > Th_P1_P2) diff1 = 1;
                        double diff2 = 0;
                        if (smoothList_P2_P1[i] > Th_P2_P1) diff2 = 1;
                        if (diff1 != diff2) OffsetCount++;
                    }

                    //得到曲线应该的偏移量
                    OffsetCount /= 4;

                    var smoothList_P1_P2_L = CloneHelper.Clone<List<double>>(smoothList_P1_P2);
                    var smoothList_P1_P2_R = CloneHelper.Clone<List<double>>(smoothList_P1_P2);

                    var smoothList_P2_P1_L = CloneHelper.Clone<List<double>>(smoothList_P2_P1);
                    var smoothList_P2_P1_R = CloneHelper.Clone<List<double>>(smoothList_P2_P1);

                    double[] ZeroData = new double[OffsetCount];

                    //移动数据
                    smoothList_P1_P2_L.RemoveRange(0, OffsetCount);
                    smoothList_P1_P2_L.AddRange(ZeroData);

                    smoothList_P1_P2_R.InsertRange(0, ZeroData);
                    smoothList_P1_P2_R.RemoveRange(PCnt, OffsetCount);

                    smoothList_P2_P1_L.RemoveRange(0, OffsetCount);
                    smoothList_P2_P1_L.AddRange(ZeroData);

                    smoothList_P2_P1_R.InsertRange(0, ZeroData);
                    smoothList_P2_P1_R.RemoveRange(PCnt, OffsetCount);

                    //计算2组差值
                    double[] ListDiff = new double[2];
                    for (int i = OffsetCount; i < PCnt - OffsetCount; i++)
                    {
                        ListDiff[0] += Math.Abs(smoothList_P1_P2_L[i] - smoothList_P2_P1_R[i]);
                        ListDiff[1] += Math.Abs(smoothList_P1_P2_R[i] - smoothList_P2_P1_L[i]);
                    }

                    //找到2组中求和最小的组
                    smoothList = new List<double>();//进行数据合并
                    if (ListDiff[0] <= ListDiff[1])
                    {
                        smoothList_P1_P2 = smoothList_P1_P2_L;
                        smoothList_P2_P1 = smoothList_P2_P1_R;

                        int i = 0;
                        for (i = 0; i < OffsetCount; i++)
                        {
                            smoothList.Add(smoothList_P1_P2[i]);
                        }
                        for (; i < PCnt - OffsetCount; i++)
                        {
                            smoothList.Add((smoothList_P1_P2[i] + smoothList_P2_P1[i]) / 2);
                        }
                        for (; i < PCnt; i++)
                        {
                            smoothList.Add(smoothList_P2_P1[i]);
                        }
                    }
                    else
                    {
                        smoothList_P1_P2 = smoothList_P1_P2_R;
                        smoothList_P2_P1 = smoothList_P2_P1_L;

                        int i = 0;
                        for (i = 0; i < OffsetCount; i++)
                        {
                            smoothList.Add(smoothList_P2_P1[i]);
                        }
                        for (; i < PCnt - OffsetCount; i++)
                        {
                            smoothList.Add((smoothList_P1_P2[i] + smoothList_P2_P1[i]) / 2);
                        }
                        for (; i < PCnt; i++)
                        {
                            smoothList.Add(smoothList_P1_P2[i]);
                        }
                    }
                }

                foreach (var item in result_P1_P2.MotorPos_mm)
                {
                    if (item.Key.Name == FFAxis.Name)
                    {
                        thetaList = item.Value;
                    }
                }

                //计算出平滑点数量
                int SmoothCount = 0;
                //if (this.TestModuleRecipe.MovingSmooth_Angle != 0)
                //{
                //    double C_DegC = thetaList[thetaList.Count / 2];

                //    for (int i = thetaList.Count / 2; i > 0; i--)
                //    {
                //        if (Math.Abs(C_DegC - thetaList[i]) >= this.TestModuleRecipe.MovingSmooth_Angle)
                //        {
                //            SmoothCount = Math.Abs(thetaList.Count / 2 - i);
                //            break;
                //        }
                //    }
                //}

                ////移动平均法平滑
                double[] smoothList_2;
                if (SmoothCount == 0)
                {
                    smoothList_2 = smoothList.ToArray();
                }
                else
                {
                    smoothList_2 = ArrayMath.CalculateMovingAverage(smoothList.ToArray(), SmoothCount);
                }


                for (int i = 0; i < smoothList_2.Length; i++)
                {
                    try
                    {
                        FarFieldRawDataPoint ffraw = new FarFieldRawDataPoint();
                        ffraw.Theta = Math.Round((thetaList[i] - phyZeroAngle), 5); //产品的角度
                        ffraw.PD_Reading = Convert.ToDouble(smoothList_2[i]);
                        ffrawCol.Points.Add(ffraw);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return ffrawCol;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //结束插补
                GC.Collect();

                //回零
                FFAxis.MoveToV3(0, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                FFAxis.WaitMotionDone();

                FFAxis.HomeRun();


            }


            return ffrawCol;

        }



    }
}
