using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using LX_Utilities;
using SolveWare_IO;
using SolveWare_TestComponents;
using SolveWare_TestComponents.ResourceProvider;
using System.Linq;
using static SolveWare_TestPackage.LaserX_9078_Traj_Function;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
     (
        "TestCalculator_FF_PeakAngle",
        "TestCalculator_FF_AW_95Percent",
        "TestCalculator_FF_AW_e2",
        "TestCalculator_FF_AW_FWHM",
        "TestCalculator_FF_Centroid"
     )]
    [ConfigurableInstrument("Keithley_24xx", "Keithley_2401", "用于加电")]
    [ConfigurableInstrument("Keithley_6485", "Keithley_6485", "用于读数")]
    [ConfigurableInstrument("Relay8CHController", "Relay8CHController", "用于切换PD")]
    [StaticResource(ResourceItemType.AXIS, "SX", "FF垂直轴")]
    [StaticResource(ResourceItemType.AXIS, "SZ", "FF水平轴")]
    //[StaticResource(ResourceItemType.POS, "FF_H_Zero_Pos", "FF水平测试产品零点")]
    public partial class TestModule_FarField_H : TestModuleBase
    {
        public TestModule_FarField_H() : base()
        {
        }

        #region 以get属性获取测试模块运行所需资源

        private Relay8CHController RelayController
        { get { return (Relay8CHController)this.ModuleResource["Relay8CHController"]; } }

        private Keithley_24xx Keithley_2401
        { get { return (Keithley_24xx)this.ModuleResource["Keithley_2401"]; } }

        private Keithley_6485 Keithley_6485
        { get { return (Keithley_6485)this.ModuleResource["Keithley_6485"]; } }

        private MotorAxisBase SX
        { get { return (MotorAxisBase)this.ModuleResource["SX"]; } }

        private MotorAxisBase SZ
        { get { return (MotorAxisBase)this.ModuleResource["SZ"]; } }

        private AxesPosition Zero_Pos
        { get { return (AxesPosition)this.dynamicPositions.GetSingleItem(this.TestRecipe.Zero_Position); } }
        //private AxesPosition Start_Pos
        //{ get { return (AxesPosition)this.ModuleResource[ResourceItemType.POS][TestRecipe.Zero_Position]; } }

        #endregion 以get属性获取测试模块运行所需资源

        //20230303 浩彬增加, recipe获取坐标的代码
        private AxesPositionCollection dynamicPositions;
        public override bool SetupResources(DataBook<string, string> userDefineInstrumentConfig, DataBook<string, string> userDefineAxisConfig, DataBook<string, string> userDefinePositionConfig, ITestPluginResourceProvider resourceProvider)
        {
       
            try
            {
                base.SetupResources(userDefineInstrumentConfig, userDefineAxisConfig, userDefinePositionConfig, resourceProvider);

                dynamicPositions = resourceProvider.Local_AxesPosition_ResourceProvider.AxesPositionCollection as AxesPositionCollection;
                // var posNames = dynamicPositions.GetDataListByPropName<string>("Name");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private TestRecipe_FarField TestRecipe { get; set; }

        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_FarField);
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_FarField>(testRecipe);
        }

        private RawData_FarField RawData { get; set; }

        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_FarField(); return RawData;
        }

        //连续捕捉
        public override void Run(CancellationToken token)
        {
            try
            {
                Log_Global($"开始FF测试.");
                var currBase = 0.0;
                //if (TestRecipe.Current_Base == TestRecipe_FarField.CurrentBase.Null)
                //{
                //}
                //else
                //{
                //    SummaryDataCollection summaryData = SummaryData;
                //    var dataDict = summaryData.ToDictionary();
                //    foreach (var item in dataDict)
                //    {
                //        if (item.Key.Contains(TestRecipe.Current_Base.ToString()))
                //        {
                //            if (!double.TryParse(item.Value.ToString(), out currBase))
                //            {
                //                var err = "无法在SummaryData中找到加电基数!";
                //                throw new Exception(err);
                //            }
                //        }
                //    }
                //}

                int[] relayvalue = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int i = 0; i < 8; i++)
                {
                    if (relayvalue[i] == 1)
                    {
                        RelayController.OnChannel(i);
                    }
                    if (relayvalue[i] == 0)
                    {
                        RelayController.OffChannel(i);
                    }
                }

                if (Keithley_2401.IsOnline)
                {
                    Keithley_2401.Timeout_ms = 1000;
                    Keithley_2401.CurrentSetpoint_A = 0;
                    Keithley_2401.VoltageSetpoint_V = 0;
                    Keithley_2401.IsOutputOn = false;
                }

                Keithley_6485.ZeroCorrection();
                Keithley_6485.IsCurrentSenseAutoRangeOn = false;
                Keithley_6485.CurrentSenseRange_A = TestRecipe.CurrentSenseRange_mA / 1000;

                Keithley_2401.Terminal = SelectTerminal.Front;
                ////设置NPLC
                //Keithley_2401.IMeasurementIntegrationPeriod = TestRecipe.NPLC;
                //Keithley_2401.VMeasurementIntegrationPeriod = TestRecipe.NPLC;
                //Keithley_6485.IMeasurementIntegrationPeriod = TestRecipe.NPLC;

                //测量暗电流
                double DarkCurrent_mA = 0;
                {
                    List<double> Current_mA = new List<double>();
                    for (int i = 0; i < 10; i++)
                    {
                        Current_mA.Add(Keithley_6485.ReadCurrent_A() * 1000);
                    }
                    DarkCurrent_mA = Current_mA.Average();
                }

                //暗电流
                Log_Global($"暗电流={DarkCurrent_mA}mA");
                RawData.DarkCurrent = DarkCurrent_mA;


                //设置顺从电压(电压波动上限
                Keithley_2401.VoltageCompliance_V = TestRecipe.Compliance_V;
                //设置电流
                Keithley_2401.CurrentSetpoint_A = (TestRecipe.Current_mA + currBase) / 1000;
                Keithley_2401.IsOutputOn = true;

                if (token.IsCancellationRequested)
                {
                    Log_Global($"用户取消测试");
                    token.ThrowIfCancellationRequested();
                    throw new OperationCanceledException();
                }
                Range_At2500mV = Keithley_6485.CurrentSenseRange_A;



                RawData.Current_mA = TestRecipe.Current_mA;
                RawData.Axis = "H";

                MotionActionV2 Action_FF_Axis_H = new MotionActionV2();
                MotionActionV2 Action_FF_Axis_V = new MotionActionV2();
                Dictionary<MotorAxisBase, MotionActionV2> axisDict = new Dictionary<MotorAxisBase, MotionActionV2>();
                axisDict.Add(SZ, Action_FF_Axis_H);

                Action_FF_Axis_V.SingleAxisHome(this.SX);
                Action_FF_Axis_H.SingleAxisHome(this.SZ);

                var actAngle = 0.0;
                foreach (var item in Zero_Pos)
                {
                    if (item.Name == "SZ")
                    {
                        actAngle = item.Position;
                    }
                }
                actAngle += TestRecipe.Start_Angle;
                Action_FF_Axis_H.SingleAxisMotion(this.SZ, actAngle, SpeedLevel.Normal);

                double logAngle = TestRecipe.Start_Angle;
                RawDatumItem_FarField rawDatumItem = new RawDatumItem_FarField();

                //单轴插补
                var actStartAngle = Zero_Pos.ItemCollection.Find(item => item.Name == "SZ").Position;
                AxesPosition P1 = CloneHelper.Clone<AxesPosition>(Zero_Pos);
                AxesPosition P2 = CloneHelper.Clone<AxesPosition>(Zero_Pos);
                P1.ItemCollection.Find(item => item.Name == "SZ").Position += TestRecipe.Start_Angle;
                P2.ItemCollection.Find(item => item.Name == "SZ").Position += TestRecipe.Stop_Angle;

                //正反扫曲线
                TrajResultItem result_P1_P2;
                TrajResultItem result_P2_P1;
                Dictionary<PmTrajAxisType, MotorAxisBase> trajDict = new Dictionary<PmTrajAxisType, MotorAxisBase>(); //插补轴定义
                trajDict.Add(PmTrajAxisType.A_Dir, this.SZ);

                int rtn = Parallel_MoveLine(trajDict, P1, P2, TestRecipe.Trajspeed, out result_P1_P2, token, SpeedLevel.Normal);
                if (rtn != 0)
                {
                    var err = $"运动出错! FF流程单轴插补错误返回值为[{rtn}]";
                    throw new Exception(err);
                }

                rtn = Parallel_MoveLine(trajDict, P2, P1, TestRecipe.Trajspeed, out result_P2_P1, token, SpeedLevel.Normal);
                if (rtn != 0)
                {
                    var err = $"运动出错! FF流程单轴插补错误返回值为[{rtn}]";
                    throw new Exception(err);
                }

                //P1->P2
                List<double> pList_P1_P2 = new List<double>();
                List<double> thetaList_P1_P2 = new List<double>();
                pList_P1_P2 = result_P1_P2.Voltage_mV[TestRecipe.Analog_CH - 1];//运动卡模拟量通道
                foreach (var item in result_P1_P2.MotorPos_mm)
                {
                    if (item.Key.Name == "SZ")
                    {
                        thetaList_P1_P2 = item.Value;
                    }
                }
                //移动平均法平滑
                List<double> smoothList_P1_P2 = new List<double>();
                smoothList_P1_P2.AddRange(ArrayMath.CalculateMovingAverage(pList_P1_P2.ToArray(), 11));

                //P2->P1
                List<double> pList_P2_P1 = new List<double>();
                List<double> thetaList_P2_P1 = new List<double>();
                pList_P2_P1 = result_P2_P1.Voltage_mV[TestRecipe.Analog_CH - 1];//运动卡模拟量通道
                foreach (var item in result_P1_P2.MotorPos_mm)
                {
                    if (item.Key.Name == "SZ")
                    {
                        thetaList_P2_P1 = item.Value;
                    }
                }
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

                    smoothList_P1_P2_R.InsertRange(0,ZeroData);
                    smoothList_P1_P2_R.RemoveRange(PCnt, OffsetCount);

                    smoothList_P2_P1_L.RemoveRange(0, OffsetCount);
                    smoothList_P2_P1_L.AddRange(ZeroData);

                    smoothList_P2_P1_R.InsertRange(0,ZeroData);
                    smoothList_P2_P1_R.RemoveRange(PCnt, OffsetCount);

                    //计算2组差值
                    double[] ListDiff = new double[2];
                    for (int i = OffsetCount; i < PCnt- OffsetCount; i++)
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
                        for (; i < PCnt- OffsetCount; i++)
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

                //List<double> pList = new List<double>();
                //List<double> thetaList = new List<double>();
                //pList = result_P1_P2.Voltage_mV[TestRecipe.Analog_CH - 1];//运动卡模拟量通道
                foreach (var item in result_P1_P2.MotorPos_mm)
                {
                    if (item.Key.Name == "SZ")
                    {
                        thetaList = item.Value;
                    }
                }


                //计算出平滑点数量
                int SmoothCount = 0;
                if(this.TestRecipe.MovingSmooth_Angle!=0)
                {
                    double C_DegC = thetaList[thetaList.Count / 2];

                    for (int i = thetaList.Count / 2; i > 0; i--)
                    {
                        if (Math.Abs(C_DegC - thetaList[i]) >= this.TestRecipe.MovingSmooth_Angle)
                        {
                            SmoothCount = Math.Abs(thetaList.Count / 2 - i);
                            break;
                        }
                    }
                }

                ////移动平均法平滑
                double[] smoothList_2;
                if (SmoothCount==0)
                {
                    smoothList_2 = smoothList.ToArray();
                }
                else
                {
                    smoothList_2 = ArrayMath.CalculateMovingAverage(smoothList.ToArray(), SmoothCount);
                }

                //保存RawData
                for (int i = 0; i < smoothList_2.Length; i++)
                {
                    RawDatumItem_FarField rawdatumitem = new RawDatumItem_FarField();
                    rawdatumitem.Theta = Math.Round((thetaList[i] - actStartAngle), 5);
                    rawdatumitem.AnalogVoltage = smoothList_2[i];
                    rawdatumitem.PDCurrent = CalcPD_Current_mA(smoothList_2[i]) - DarkCurrent_mA; //消除暗电流
                    RawData.DataCollection.Add(rawdatumitem);
                }

                //回零
                Action_FF_Axis_H.SingleAxisMotion(this.SZ, 0, SpeedLevel.Normal);
                Action_FF_Axis_H.SingleAxisHome(this.SZ);
                //下电
                Keithley_2401.CurrentSetpoint_A = 0.0;
                Keithley_2401.IsOutputOn = false;

                Log_Global($"FF测试完成.");
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    throw ex;
                }
                else
                {
                    this._core.ReportException("远场发散角测试流程出现异常", ErrorCodes.Module_FF_Failed, ex);
                    throw ex;
                }
            }
            finally
            {
                Keithley_2401.IsOutputOn = false;
            }
        }

        private double Range_At2500mV;  //当前挡位

        private double CalcPD_Current_mA(double Voltage_mV, double Range)
        {
            //2500             -> Keithley_6485.CurrentSenseRange_A
            //pList[maxIndex]  -> 多少A

            double PD_Current_mA = (Voltage_mV * Range / 2500) * 1000;
            return PD_Current_mA;
        }

        private double CalcPD_Current_mA(double Voltage_mV)
        {
            return CalcPD_Current_mA(Voltage_mV, Range_At2500mV);
        }
    }
}