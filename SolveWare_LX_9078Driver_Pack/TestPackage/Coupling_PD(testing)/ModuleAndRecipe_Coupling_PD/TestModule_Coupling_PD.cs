using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static SolveWare_TestPackage.LaserX_9078_Traj_Function;

namespace SolveWare_TestPackage
{
    [ConfigurableInstrument("Keithley_24xx", "Keithley_2401", "用于耦合加电")]
    [ConfigurableInstrument("Keithley_6485", "Keithley_6485", "用于耦合读数")]
    [ConfigurableInstrument("Relay8CHController", "Relay8CHController", "用于切换电路")]
    [ConfigurableInstrument("GolightMultiLaserLightSource", "MultiLaserLightSource", "用于PDArray耦合打光")]
    [StaticResource(ResourceItemType.AXIS, "TPSX", "耦合X轴")]
    [StaticResource(ResourceItemType.AXIS, "TPSY", "耦合Y轴")]
    [StaticResource(ResourceItemType.AXIS, "TPSZ", "耦合Z轴")]
    public partial class TestModule_Coupling_PD : TestModuleBase
    {
        public class PointResult
        {
            public PointResult()
            {
                ID = 0;
                Position = null;
                Power = 0.0;
            }

            public int ID { get; set; }
            public AxesPosition Position { get; set; }
            public double Power { get; set; }
        }

        #region 以get属性获取测试模块运行所需资源

        private Keithley_24xx Keithley_2401
        { get { return (Keithley_24xx)this.ModuleResource["Keithley_2401"]; } }

        private Keithley_6485 Keithley_6485
        { get { return (Keithley_6485)this.ModuleResource["Keithley_6485"]; } }

        //private ILightSource_Golight LightSource
        //{ get { return (ILightSource_Golight)this.ModuleResource["MultiLaserLightSource"]; } }

        private Relay8CHController RelayController
        { get { return (Relay8CHController)this.ModuleResource["Relay8CHController"]; } }

        private MotorAxisBase TPSX
        { get { return (MotorAxisBase)this.ModuleResource["TPSX"]; } }

        private MotorAxisBase TPSY
        { get { return (MotorAxisBase)this.ModuleResource["TPSY"]; } }

        private MotorAxisBase TPSZ
        { get { return (MotorAxisBase)this.ModuleResource["TPSZ"]; } }

        //开始点
        private AxesPosition t_Start_Pos;

        //参考位置开始点
        private AxesPosition Start_Pos
        { get { return (AxesPosition)this.dynamicPositions.GetSingleItem(this.TestRecipe.Position); } }

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

        #endregion 以get属性获取测试模块运行所需资源

        private TestRecipe_Coupling_PD TestRecipe { get; set; }

        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_Coupling_PD);
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_Coupling_PD>(testRecipe);
        }

        private RawData_Coupling_PD RawData { get; set; }

        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_Coupling_PD(); return RawData;
        }

        public override void Run(CancellationToken token)
        {
            int channel = TestRecipe.LightSource_Channel;
            //耦合面
            var UsedPlane = LaserX_9078_Utilities.PmTrajSelectPlane.XY_CW;
            Log_Global($"开始耦合流程.");

            string path = Path.Combine(Path.GetFullPath(Application.StartupPath));
            //    this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CSV_DATA_PATH),
            //    "单项调试数据",
            //    "耦合Debug数据");


            //string LSourcename = LightSource.Name + "_" + TestRecipe.LightSource_Model;

            //double[] k;
            //if (SolveWare_TestPlugin.SourceLight_CalibrationParameter.GetCalibrationParamsK(LSourcename, TestRecipe.Wavelength, out k) == false)
            //{
            //    var err = $"SourceLight校准数据中不存在[名称={LSourcename}][波长={TestRecipe.Wavelength}]的校准系数, 使用默认系数.";
            //    Log_Global(err);
            //}
            //else
            //{

            //    var err = $"SourceLight校准数据[名称={LSourcename}][波长={TestRecipe.Wavelength}]的校准系数,";
            //    for (int i = k.Length - 1; i >= 0; i--)
            //    {
            //        err += $"K({i})={k[i]} ";
            //    }
            //    Log_Global(err);

            //}

            //转换后功率
            double TestPower=0;
           // SolveWare_TestPlugin.SourceLight_CalibrationParameter.CalcCalibrationValue(LSourcename, TestRecipe.Wavelength, TestRecipe.LightPower_mW, out TestPower);

            #region 初始化

            //切换继电器
            int[] relayvalue = new int[] { 1, 0, 0, 0, 1, 0, 0, 0 };
            for (int i = 0; i < 8; i++)
            {
                if (relayvalue[i] == 0)
                {
                    RelayController.OffChannel(i);
                }
                else
                {
                    RelayController.OnChannel(i);
                }
            }

            MotionActionV2 Action_Axis_X = new MotionActionV2();
            MotionActionV2 Action_Axis_Y = new MotionActionV2();
            MotionActionV2 Action_Axis_Z = new MotionActionV2();

            Dictionary<MotorAxisBase, MotionActionV2> axisDict = new Dictionary<MotorAxisBase, MotionActionV2>();
            axisDict.Add(this.TPSX, Action_Axis_X);
            axisDict.Add(this.TPSY, Action_Axis_Y);
            axisDict.Add(this.TPSZ, Action_Axis_Z);

            Keithley_2401.Reset();
            Thread.Sleep(500);
            Keithley_6485.ZeroCorrection();
            Thread.Sleep(500);
            Keithley_6485.IsCurrentSenseAutoRangeOn = false;
            Keithley_6485.CurrentSenseRange_A = TestRecipe.CurrentSenseRange_mA / 1000;

            //控电
            if (Keithley_2401.IsOnline)
            {
                Keithley_2401.Timeout_ms = 1000;
                Keithley_2401.CurrentSetpoint_A = 0;
                Keithley_2401.VoltageSetpoint_V = 0;
                Keithley_2401.IsOutputOn = false;
            }
            Keithley_2401.Terminal = SelectTerminal.Rear;
            Keithley_2401.SourceMode = SourceModeTypes.Voltage;
            Keithley_2401.IsVoltageSourceAutoRangeOn = true;
            Keithley_2401.SenseMode = SenseModeTypes.Current;
            Keithley_2401.IsCurrentSenseAutoRangeOn = false;

            Keithley_2401.CurrentCompliance_A = TestRecipe.Compliance_mA / 1000;
            Keithley_2401.CurrentSenseRange_A = TestRecipe.Compliance_mA / 1000;
            Keithley_2401.VoltageSetpoint_V = TestRecipe.Voltage_V;
            Keithley_2401.IsOutputOn = true;

            //LightSource.SetChEnable(channel, true);
            //LightSource.SetChOPower(channel, (float)TestPower);

            Thread.Sleep(500);

            #endregion 初始化

            bool DebugFile = false;
            if (TestRecipe.SaveDebugFiles)
            {
                DebugFile = true;
            }

            //初始位置
            t_Start_Pos = CloneHelper.Clone<AxesPosition>(Start_Pos);
            if (this.TestRecipe.UsedCurrentPos)
            {
                //初始位置
                t_Start_Pos.ItemCollection.Find(item => item.Name == "TPSX").Position = this.TPSX.Get_CurUnitPos();
                t_Start_Pos.ItemCollection.Find(item => item.Name == "TPSY").Position = this.TPSY.Get_CurUnitPos();
                t_Start_Pos.ItemCollection.Find(item => item.Name == "TPSZ").Position = this.TPSZ.Get_CurUnitPos();
            }

            AxesPosition P1 = CloneHelper.Clone<AxesPosition>(t_Start_Pos);

            MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, P1, SequenceOrder.Normal);

            StringBuilder strb = new StringBuilder();
            bool zeroScan = true;
            bool firstFineScan = false;
            bool needReverse = false;

            //单向最大步数
            var maxStep = Convert.ToInt32(TestRecipe.Layer_Range / TestRecipe.Layer_Step);

            Dictionary<int, PointResult> maxList = new Dictionary<int, PointResult>();// List<PointResult>();
            StreamWriter sw;

            //20230210  此处扫描层的Y方向有严重错误 已修复
            TrajResultItem result;

            double K_Curr_mA;
            try
            {
                #region 第0层

                if (zeroScan)
                {
                    if (token.IsCancellationRequested)
                    {
                        Log_Global("用户取消测试");
                        token.ThrowIfCancellationRequested();
                        throw new OperationCanceledException();
                    }

                    var id = 0;
                    Dictionary<AxesPosition, double> retPoint = new Dictionary<AxesPosition, double>();

                    //使能粗扫
                    if (maxStep != 0 || (maxStep == 0 && this.TestRecipe.Rough_Enable))
                    {
                        //粗扫

                        Log_Global($"开始粗耦合[{id}]");
                        result = Run_Involute(eRunSize_Table.Rough, P1, UsedPlane, token);

                        if (token.IsCancellationRequested)
                        {
                            Log_Global("用户取消测试");
                            token.ThrowIfCancellationRequested();
                            throw new OperationCanceledException();
                        }


                        while (!DataAnalyze(result, false, out retPoint))
                        {
                            if (token.IsCancellationRequested)
                            {
                                Log_Global("用户取消测试");
                                token.ThrowIfCancellationRequested();
                                throw new OperationCanceledException();
                            }

                            if (DebugFile)
                            {
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                strb = PrintCSV(result);
                                sw = new StreamWriter(path + $@"\粗耦合超量程_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                                sw.Write(strb.ToString());
                                sw.Close(); strb.Clear();
                            }

                            Keithley_6485.CurrentSenseRange_A *= 10;

                            P1 = CloneHelper.Clone(retPoint.First().Key);

                            //寻找最小的半径
                            double MinR = DataAnalyze_MinR(result);

                            result = Run_Involute_Rough_ParameterR(eRunSize_Table.Rough, MinR, P1, UsedPlane, token);

                            if (token.IsCancellationRequested)
                            {
                                Log_Global("用户取消测试");
                                token.ThrowIfCancellationRequested();
                                throw new OperationCanceledException();
                            }
                        }

                        //输出csv(测试用)
                        if (DebugFile)
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            strb = PrintCSV(result);
                            sw = new StreamWriter(path + $@"\粗耦合_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                            sw.Write(strb.ToString());
                            sw.Close(); strb.Clear();
                        }

                        {
                            //此时运动到P1点, 读取6485电流
                            MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, CloneHelper.Clone(retPoint.First().Key), SequenceOrder.Normal);
                            Thread.Sleep(100);

                            do
                            {
                                K_Curr_mA = Keithley_6485.ReadCurrent_A() * 1000;
                                if (K_Curr_mA > 3000)
                                {
                                    Keithley_6485.CurrentSenseRange_A *= 10;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            while (true);

                            //判断是否无光
                            if (K_Curr_mA <= TestRecipe.Power_Threshold)
                            {
                                Log_Global($"{this.Name} 扫描范围内无光]");
                                string str = $"{this.Name} 扫描范围内无光...";
                                zeroScan = false;
                                throw new Exception(str);
                            }


                            var maxPoint = new PointResult();
                            maxPoint.ID = id;
                            maxPoint.Position = CloneHelper.Clone(retPoint.First().Key);
                            maxPoint.Power = K_Curr_mA;
                            maxList.Add(id, maxPoint);

                            P1 = CloneHelper.Clone(maxPoint.Position);
                        }


                        if (token.IsCancellationRequested)
                        {
                            Log_Global("用户取消测试");
                            token.ThrowIfCancellationRequested();
                            throw new OperationCanceledException();
                        }

                        //double精扫
                        Log_Global($"开始DoubleSize粗耦合[{id}]");
                        result = Run_Involute(eRunSize_Table.Fine_Double, P1, UsedPlane, token);

                        if (token.IsCancellationRequested)
                        {
                            Log_Global("用户取消测试");
                            token.ThrowIfCancellationRequested();
                            throw new OperationCanceledException();
                        }


                        while (!DataAnalyze(result, false, out retPoint))
                        {
                            if (token.IsCancellationRequested)
                            {
                                Log_Global("用户取消测试");
                                token.ThrowIfCancellationRequested();
                                throw new OperationCanceledException();
                            }

                            if (DebugFile)
                            {
                                strb = PrintCSV(result);
                                sw = new StreamWriter(path + $@"\DoubleSize精耦合超量程_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                                sw.Write(strb.ToString());
                                sw.Close(); strb.Clear();
                            }

                            Keithley_6485.CurrentSenseRange_A *= 10;
                            result = Run_Involute(eRunSize_Table.Fine_Double, P1, UsedPlane, token);

                            if (token.IsCancellationRequested)
                            {
                                Log_Global("用户取消测试");
                                token.ThrowIfCancellationRequested();
                                throw new OperationCanceledException();
                            }
                        }

                        //输出csv(测试用)
                        if (DebugFile)
                        {
                            strb = PrintCSV(result);
                            sw = new StreamWriter(path + $@"\DoubleSize精耦合_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                            sw.Write(strb.ToString());
                            sw.Close(); strb.Clear();
                        }

                        if (retPoint.First().Value <= TestRecipe.Power_Threshold)
                        {
                            string str = $"{this.Name} 扫描范围内无光...";
                            zeroScan = false;
                            throw new Exception(str);
                        }

                        {
                            //如果新值更大就替换maxPoint键值
                            var maxPoint = new PointResult();
                            maxPoint.ID = id;
                            maxPoint.Position = CloneHelper.Clone(retPoint.First().Key);
                            maxPoint.Power = retPoint.First().Value;

                            P1 = CloneHelper.Clone(maxPoint.Position);
                        }
                    }

                    //精扫
                    Log_Global($"开始精耦合[{id}]");
                    result = Run_Involute(eRunSize_Table.Fine, P1, UsedPlane, token);

                    if (token.IsCancellationRequested)
                    {
                        Log_Global("用户取消测试");
                        token.ThrowIfCancellationRequested();
                        throw new OperationCanceledException();
                    }

                    while (!DataAnalyze(result, false, out retPoint))
                    {
                        if (token.IsCancellationRequested)
                        {
                            Log_Global("用户取消测试");
                            token.ThrowIfCancellationRequested();
                            throw new OperationCanceledException();
                        }

                        if (DebugFile)
                        {
                            strb = PrintCSV(result);
                            sw = new StreamWriter(path + $@"\精耦合超量程_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                            sw.Write(strb.ToString());
                            sw.Close(); strb.Clear();
                        }

                        Keithley_6485.CurrentSenseRange_A *= 10;
                        result = Run_Involute(eRunSize_Table.Fine, P1, UsedPlane, token);

                        if (token.IsCancellationRequested)
                        {
                            Log_Global("用户取消测试");
                            token.ThrowIfCancellationRequested();
                            throw new OperationCanceledException();
                        }
                    }

                    //输出csv(测试用)
                    if (DebugFile)
                    {
                        strb = PrintCSV(result);
                        sw = new StreamWriter(path + $@"\精耦合_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                        sw.Write(strb.ToString());
                        sw.Close(); strb.Clear();
                    }

                    if (retPoint.First().Value <= TestRecipe.Power_Threshold)
                    {
                        string str = $"{this.Name} 扫描范围内无光...";
                        zeroScan = false;
                        throw new Exception(str);
                    }

                    //此时运动到P1点, 读取6485电流
                    MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, CloneHelper.Clone(retPoint.First().Key), SequenceOrder.Normal);
                    Thread.Sleep(100);
                    do
                    {
                        K_Curr_mA = Keithley_6485.ReadCurrent_A() * 1000;
                        if (K_Curr_mA > 3000)
                        {
                            Keithley_6485.CurrentSenseRange_A *= 10;
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (true);

                    //如果新值更大就替换maxPoint键值
                    //double maxList_Max = 0;
                    //if (maxList.Count > 0)
                    //{
                    //    maxList_Max = maxList.Max(item => item.Value.Power);
                    //}

                    {
                        var maxPoint = new PointResult();
                        maxPoint.ID = id;
                        maxPoint.Position = CloneHelper.Clone(retPoint.First().Key);
                        maxPoint.Power = K_Curr_mA;// retPoint.First().Value;

                        if (maxList.ContainsKey(id))
                        {
                            if (maxList[id].Power <= maxPoint.Power)
                            {
                                maxList[id] = maxPoint;
                            }
                        }
                        else
                        {
                            maxList.Add(id, maxPoint);
                        }
                        P1 = CloneHelper.Clone(maxPoint.Position);
                    }
                    //if (maxList_Max <= retPoint.First().Value)
                    //{
                    //    var maxPoint = new PointResult();
                    //    maxPoint.ID = id;
                    //    maxPoint.Position = CloneHelper.Clone(retPoint.First().Key);
                    //    maxPoint.Power = retPoint.First().Value;

                    //    maxList.Add(id, maxPoint);

                    //    P1 = CloneHelper.Clone(maxPoint.Position);
                    //}
                    //else
                    //{
                    //}

                    zeroScan = false;
                    firstFineScan = true;
                }
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}

                #endregion 第0层

                if (maxStep >= 1)
                {
                    #region 第-1层(向上为正方向)

                    try
                    {
                        if (firstFineScan)
                        {
                            if (token.IsCancellationRequested)
                            {
                                Log_Global("用户取消测试");
                                token.ThrowIfCancellationRequested();
                                throw new OperationCanceledException();
                            }

                            var id = -1;
                            P1.ItemCollection.Find(item => item.Name == "TPSZ").Position -= (id - 0) * TestRecipe.Layer_Step;

                            Log_Global($"开始精耦合[{id}]");

                            result = Run_Involute(eRunSize_Table.Fine, P1, UsedPlane, token);

                            if (token.IsCancellationRequested)
                            {
                                Log_Global("用户取消测试");
                                token.ThrowIfCancellationRequested();
                                throw new OperationCanceledException();
                            }

                            Dictionary<AxesPosition, double> retPoint = new Dictionary<AxesPosition, double>();
                            while (!DataAnalyze(result, false, out retPoint))
                            {
                                if (token.IsCancellationRequested)
                                {
                                    Log_Global("用户取消测试");
                                    token.ThrowIfCancellationRequested();
                                    throw new OperationCanceledException();
                                }
                                if (DebugFile)
                                {
                                    strb = PrintCSV(result);
                                    sw = new StreamWriter(path + $@"\精耦合超量程_{id}_{DateTime.Now:yyyymmdd_hhmmss}.csv");
                                    sw.Write(strb.ToString());
                                    sw.Close(); strb.Clear();
                                }

                                Keithley_6485.CurrentSenseRange_A *= 10;
                                result = Run_Involute(eRunSize_Table.Fine, P1, UsedPlane, token);

                                if (token.IsCancellationRequested)
                                {
                                    Log_Global("用户取消测试");
                                    token.ThrowIfCancellationRequested();
                                    throw new OperationCanceledException();
                                }
                            }

                            //输出csv(测试用)
                            if (DebugFile)
                            {
                                strb = PrintCSV(result);
                                sw = new StreamWriter(path + $@"\精耦合_{id}_{DateTime.Now:yyyymmdd_hhmmss}.csv");
                                sw.Write(strb.ToString());
                                sw.Close(); strb.Clear();
                            }

                            //此时运动到P1点, 读取6485电流
                            MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, CloneHelper.Clone(retPoint.First().Key), SequenceOrder.Normal);
                            Thread.Sleep(100);
                            do
                            {
                                K_Curr_mA = Keithley_6485.ReadCurrent_A() * 1000;
                                if (K_Curr_mA > 3000)
                                {
                                    Keithley_6485.CurrentSenseRange_A *= 10;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            while (true);

                            var point = new PointResult();
                            point.ID = id;
                            point.Position = CloneHelper.Clone(retPoint.First().Key);
                            point.Power = K_Curr_mA;// retPoint.First().Value;

                            if (maxList.ContainsKey(id))
                            {
                                if (maxList[id].Power <= point.Power)
                                {
                                    maxList[id] = point;
                                }
                            }
                            else
                            {
                                maxList.Add(id, point);
                            }

                            P1 = CloneHelper.Clone(point.Position);


                            //double maxList_Max = 0;
                            //if (maxList.Count > 0)
                            //{
                            //    maxList_Max = maxList.Max(item => item.Value.Power);
                            //}

                            //if (maxList_Max > point.Power || point.Power <= TestRecipe.Power_Threshold)
                            //{
                            //    maxList.Add(point);
                            //    needReverse = true;
                            //    P1 = CloneHelper.Clone(maxList.Last()Position);
                            //}
                            //if (maxList.Max(item => item.Power) <= point.Power)
                            //{
                            //    maxList.Add(point);
                            //    P1 = CloneHelper.Clone(point.Position);
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    #endregion 第1层(向上为正方向)

                    #region 第i层

                    //搜索层的方向判断
                    int ScanDir = TestRecipe.ScanDirByLayerPower;
                    if (ScanDir < 2) ScanDir = 2;
                    if (ScanDir > 20) ScanDir = 20;
                    try
                    {
                        // if (needReverse)
                        {
                            P1 = CloneHelper.Clone(maxList[0].Position);
                            int i = 1;
                            for (; -maxStep <= i && i <= maxStep;)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    Log_Global("用户取消测试");
                                    token.ThrowIfCancellationRequested();
                                    throw new OperationCanceledException();
                                }

                                var id = i;
                                P1.ItemCollection.Find(item => item.Name == "TPSZ").Position = maxList[0].Position.GetSingleItem("TPSZ").Position - id * TestRecipe.Layer_Step;

                                //if (id == 1)
                                //{
                                //    P1 = CloneHelper.Clone(maxList[0].Position);
                                //    //P1 = CloneHelper.Clone(maxList.Find(p => p.ID == 0).Position);
                                //    P1.ItemCollection.Find(item => item.Name == "TPSZ").Position -= TestRecipe.Layer_Step;
                                //}
                                //else
                                //{
                                //    P1.ItemCollection.Find(item => item.Name == "TPSZ").Position -= TestRecipe.Layer_Step;
                                //}

                                Log_Global($"开始精耦合[{id}]");

                                result = Run_Involute(eRunSize_Table.Fine, P1, UsedPlane, token);

                                if (token.IsCancellationRequested)
                                {
                                    Log_Global("用户取消测试");
                                    token.ThrowIfCancellationRequested();
                                    throw new OperationCanceledException();
                                }

                                Dictionary<AxesPosition, double> retPoint = new Dictionary<AxesPosition, double>();
                                while (!DataAnalyze(result, false, out retPoint))
                                {
                                    if (token.IsCancellationRequested)
                                    {
                                        Log_Global("用户取消测试");
                                        token.ThrowIfCancellationRequested();
                                        throw new OperationCanceledException();
                                    }
                                    //输出csv(测试用)
                                    if (DebugFile)
                                    {
                                        strb = PrintCSV(result);
                                        sw = new StreamWriter(path + $@"\精耦合超量程_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                                        sw.Write(strb.ToString());
                                        sw.Close(); strb.Clear();
                                    }
                                    Keithley_6485.CurrentSenseRange_A *= 10;
                                    result = Run_Involute(eRunSize_Table.Fine, P1, UsedPlane, token);

                                    if (token.IsCancellationRequested)
                                    {
                                        Log_Global("用户取消测试");
                                        token.ThrowIfCancellationRequested();
                                        throw new OperationCanceledException();
                                    }
                                }

                                //输出csv(测试用)
                                if (DebugFile)
                                {
                                    strb = PrintCSV(result);
                                    sw = new StreamWriter(path + $@"\精耦合_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                                    sw.Write(strb.ToString());
                                    sw.Close(); strb.Clear();
                                }

                                //此时运动到P1点, 读取6485电流
                                MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, CloneHelper.Clone(retPoint.First().Key), SequenceOrder.Normal);
                                Thread.Sleep(100);
                                do
                                {
                                    K_Curr_mA = Keithley_6485.ReadCurrent_A() * 1000;
                                    if (K_Curr_mA > 3000)
                                    {
                                        Keithley_6485.CurrentSenseRange_A *= 10;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                while (true);

                                var point = new PointResult();
                                point.ID = id;
                                point.Position = CloneHelper.Clone(retPoint.First().Key);
                                point.Power = K_Curr_mA;// retPoint.First().Value;

                                if (maxList.ContainsKey(id))
                                {
                                    if (maxList[id].Power <= point.Power)
                                    {
                                        maxList[id] = point;
                                    }
                                }
                                else
                                {
                                    maxList.Add(id, point);
                                }

                                P1 = CloneHelper.Clone(point.Position);

                                //如果最大功率点不在最远的2个位置, 就认为是最大点
                                {
                                    var MaxPower_id = maxList.OrderByDescending(item => item.Value.Power).First().Value.ID;
                                    //最大的功率
                                    int maxid = maxList.Max(item => item.Value.ID);
                                    int minid = maxList.Min(item => item.Value.ID);

                                    if (Math.Abs(MaxPower_id - minid) <= ScanDir)
                                    {
                                        //向小的方向运动
                                        P1 = CloneHelper.Clone(maxList[minid].Position);
                                        i = minid - 1;
                                    }
                                    else if (Math.Abs(MaxPower_id - maxid) <= ScanDir)
                                    {
                                        //向大的方向运动
                                        P1 = CloneHelper.Clone(maxList[maxid].Position);
                                        i = maxid + 1;
                                    }
                                    else
                                    {
                                        //这就是最大点
                                        break;
                                    }
                                }


                                //if (maxList.Max(item => item.Power) > point.Power || point.Power <= TestRecipe.Power_Threshold)
                                //{
                                //    P1 = CloneHelper.Clone(maxList.Last().Position);
                                //    break;
                                //}
                                //if (maxList.Max(item => item.Power) <= point.Power)
                                //{
                                //    maxList.Add(point);
                                //    P1 = CloneHelper.Clone(point.Position);
                                //}
                            }
                        }
                        //  else
                        //{
                        //    P1 = CloneHelper.Clone(maxList[0].Position);

                        //    for (int i = -1; i >= -maxStep; i--)
                        //    {
                        //        if (token.IsCancellationRequested)
                        //        {
                        //            Log_Global("用户取消测试");
                        //            token.ThrowIfCancellationRequested();
                        //            throw new OperationCanceledException();
                        //        }

                        //        var id = i;
                        //        P1.ItemCollection.Find(item => item.Name == "TPSZ").Position = maxList[0].Position.GetSingleItem("TPSZ").Position - id * TestRecipe.Layer_Step;

                        //        //P1.ItemCollection.Find(item => item.Name == "TPSZ").Position += TestRecipe.Layer_Step;

                        //        result = Run_Involute(eRunSize_Table.Fine, P1, UsedPlane);
                        //        Dictionary<AxesPosition, double> retPoint = new Dictionary<AxesPosition, double>();
                        //        while (!DataAnalyze(result, false, out retPoint))
                        //        {
                        //            if (token.IsCancellationRequested)
                        //            {
                        //                Log_Global("用户取消测试");
                        //                token.ThrowIfCancellationRequested();
                        //                throw new OperationCanceledException();
                        //            }
                        //            //输出csv(测试用)
                        //            if (DebugFile)
                        //            {
                        //                strb = PrintCSV(result);
                        //                sw = new StreamWriter(path + $@"\精耦合超量程_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                        //                sw.Write(strb.ToString());
                        //                sw.Close(); strb.Clear();
                        //            }
                        //            Keithley_6485.CurrentSenseRange_A *= 10;
                        //            result = Run_Involute(eRunSize_Table.Fine, P1, UsedPlane);
                        //        }

                        //        //输出csv(测试用)
                        //        if (DebugFile)
                        //        {
                        //            strb = PrintCSV(result);
                        //            sw = new StreamWriter(path + $@"\精耦合_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                        //            sw.Write(strb.ToString());
                        //            sw.Close(); strb.Clear();
                        //        }
                        //        var point = new PointResult();
                        //        point.ID = id;
                        //        point.Position = CloneHelper.Clone(retPoint.First().Key);
                        //        point.Power = retPoint.First().Value;

                        //        if (maxList.ContainsKey(id))
                        //        {
                        //            if (maxList[id].Power <= point.Power)
                        //            {
                        //                maxList[id] = point;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            maxList.Add(id, point);
                        //        }

                        //        P1 = CloneHelper.Clone(point.Position);

                        //        //if (maxList.Max(item => item.Power) > point.Power || point.Power <= TestRecipe.Power_Threshold)
                        //        //{
                        //        //    P1 = CloneHelper.Clone(maxList.Last().Position);
                        //        //    break;
                        //        //}
                        //        //if (maxList.Max(item => item.Power) <= point.Power)
                        //        //{
                        //        //    maxList.Add(point);
                        //        //    P1 = CloneHelper.Clone(point.Position);
                        //        //}
                        //    }
                        //}


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    #endregion 第i层

                }

                //try
                //{
                #region 输出数据 坐标与功率的关系
                if (DebugFile)
                {
                    strb = new StringBuilder();
                    {
                        string str = "";
                        str += $"Id,";
                        foreach (var item in maxList.First().Value.Position)
                        {
                            str += $"{item.Name},";
                        }
                        str += "Power_mA";
                        strb.AppendLine(str);
                    }
                    foreach (var lst in maxList)
                    {
                        string str = "";
                        str += $"{lst.Value.ID},";
                        foreach (var item in lst.Value.Position)
                        {
                            str += $"{item.Position},";
                        }
                        str += $"{lst.Value.Power}";
                        strb.AppendLine(str);
                    }
                    sw = new StreamWriter(path + $@"\坐标与功率的关系_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                    sw.Write(strb.ToString());
                    sw.Close(); strb.Clear();
                }
                #endregion

                #region 渐开线最大位置耦合
                if (maxList.Count > 1)
                {
                    var lastMaxPoint = maxList.OrderByDescending(item => item.Value.Power).First();
                    if (maxList.Max(n => n.Value.Power) != lastMaxPoint.Value.Power)
                    {
                        string str = $"{this.Name} 记录最大值点与储存最大值点不符.";
                        zeroScan = false;
                        throw new Exception(str);
                    }
                    else
                    {
                    }

                    P1 = CloneHelper.Clone(lastMaxPoint.Value.Position);

                    Log_Global($"开始渐开线最大值耦合");
                    var finalresult = Run_Involute(eRunSize_Table.Fine, P1, UsedPlane, token);

                    if (token.IsCancellationRequested)
                    {
                        Log_Global("用户取消测试");
                        token.ThrowIfCancellationRequested();
                        throw new OperationCanceledException();
                    }
                    Dictionary<AxesPosition, double> finalPoint = new Dictionary<AxesPosition, double>();
                    if (TestRecipe.CrossPoint_Times == 0)
                    {
                        DataAnalyze(finalresult, true, out finalPoint);
                    }
                    else
                    {
                        DataAnalyze(finalresult, false, out finalPoint);
                    }

                    //输出csv(测试用)
                    if (DebugFile)
                    {
                        strb = PrintCSV(finalresult);
                        StreamWriter sw0 = new StreamWriter(path + $@"\渐开线最大值耦合_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                        sw0.Write(strb.ToString());
                        sw0.Close(); strb.Clear();
                    }

                    P1 = CloneHelper.Clone(finalPoint.First().Key);

                    if (token.IsCancellationRequested)
                    {
                        Log_Global("用户取消测试");
                        token.ThrowIfCancellationRequested();
                        throw new OperationCanceledException();
                    }


                    RawData.Position_X_mm = Math.Round(P1.ItemCollection.Find(item => item.Name == "TPSX").Position, 5);
                    RawData.Position_Y_mm = Math.Round(P1.ItemCollection.Find(item => item.Name == "TPSY").Position, 5);
                    RawData.Position_Z_mm = Math.Round(P1.ItemCollection.Find(item => item.Name == "TPSZ").Position, 5);
                    RawData.PDCurrent_mA = lastMaxPoint.Value.Power;


                }
                else
                {
                    Log_Global($"仅有一个平面, 跳过开始渐开线最大值耦合步骤");
                }
                #endregion

                //运行到P1点
                MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, P1, SequenceOrder.Normal);


                //#region 十字扫描 并运动到位

                //var crossResult = Run_Cross(eRunSize_Table.Fine_Half, P1, path);
                //if (crossResult != null)
                //{
                //    P1 = CloneHelper.Clone(crossResult.First().Key);
                //}

                //if (token.IsCancellationRequested)
                //{
                //    Log_Global("用户取消测试");
                //    token.ThrowIfCancellationRequested();
                //    throw new OperationCanceledException();
                //}
                //#endregion 十字扫描
                #region 单点搜寻 并运动到位
                if (this.TestRecipe.CrossPoint_Times > 0)
                {
                    Log_Global($"开始单点搜寻耦合");

                    //范围 步进
                    var PointSearchResult = Run_PointSearch(this.TestRecipe.Fine_Radius, this.TestRecipe.CrossPoint_Interval, P1, true, path, token);

                    if (token.IsCancellationRequested)
                    {
                        Log_Global("用户取消测试");
                        token.ThrowIfCancellationRequested();
                        throw new OperationCanceledException();
                    }
                    if (PointSearchResult != null)
                    {
                        RawData.Position_X_mm = Math.Round(PointSearchResult.First().Key.ItemCollection.Find(item => item.Name == "TPSX").Position, 5);
                        RawData.Position_Y_mm = Math.Round(PointSearchResult.First().Key.ItemCollection.Find(item => item.Name == "TPSY").Position, 5);
                        RawData.Position_Z_mm = Math.Round(PointSearchResult.First().Key.ItemCollection.Find(item => item.Name == "TPSZ").Position, 5);
                        RawData.PDCurrent_mA = PointSearchResult.First().Value;

                        if (DebugFile)
                        {
                            strb = PrintCSV(PointSearchResult);
                            StreamWriter sw0 = new StreamWriter(path + $@"\单点搜寻耦合_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                            sw0.Write(strb.ToString());
                            sw0.Close(); strb.Clear();
                        }

                        P1 = CloneHelper.Clone(PointSearchResult.First().Key);
                    }


                }

                if (token.IsCancellationRequested)
                {
                    Log_Global("用户取消测试");
                    token.ThrowIfCancellationRequested();
                    throw new OperationCanceledException();
                }

                #endregion 单点搜寻完成


                Log_Global($"耦合完成.");
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}

            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    throw ex;
                }
                else
                {
                    this.ReportException($"耦合流程出现异常", ErrorCodes.Module_Coupling_Failed, ex);
                    throw ex;
                }
            }
            finally
            {
                Keithley_2401.IsOutputOn = false;
                //LightSource.SetChOPower(channel, 0);
                //LightSource.SetChEnable(channel, false);
            }
        }

        public StringBuilder PrintCSV(TrajResultItem result)
        {
            StringBuilder sb = new StringBuilder();
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
                foreach (var item in result.Voltage_mV)
                {
                    str += $"Ch{item.Key}_mA,";
                }
                sb.AppendLine(str);
            }

            int count = result.Id.Count;
            for (int j = 0; j < count; j++)
            {
                str = $"{result.Id[j]},";
                foreach (var item in result.MotorPos_mm)
                {
                    str += $"{item.Value[j]},";
                }
                foreach (var item in result.Voltage_mV)
                {
                    str += $"{item.Value[j]},";
                }
                foreach (var item in result.Voltage_mV)
                {
                    str += $"{CalcPD_Current_mA(item.Value[j])},";
                }
                sb.AppendLine(str);
            }

            return sb;
        }
        public StringBuilder PrintCSV(Dictionary<AxesPosition, double> result)
        {
            StringBuilder sb = new StringBuilder();
            string str = "";
            {
                str = $"Id,";
                foreach (var item in result.First().Key.ItemCollection)
                {
                    str += $"{item.Name},";
                }
                for(int i=0;i<4;i++)
                {
                    str += $"ChX,";
                }
                for (int i = 0; i < 4; i++)
                {
                    str += $"ChX_mA,";
                }
                sb.AppendLine(str);
            }

            int count = result.Count;

            foreach(var item in result)
            {
                str = $"X,";
                foreach (var item2 in item.Key.ItemCollection)
                {
                    str += $"{item2.Position},";
                }
                for (int i = 0; i < 4; i++)
                {
                    str += $"0,";
                }
                for (int i = 0; i < 4; i++)
                {
                    str += $"{item.Value},";
                }
                sb.AppendLine(str);
            }
            return sb;
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

        /// <summary>
        /// 在中心不变的情况下, 寻找一个能包住超限值的半径 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public double DataAnalyze_MinR(TrajResultItem result)
        {
            try
            {
                List<double> pList = new List<double>();
                List<double> xList = new List<double>();
                List<double> yList = new List<double>();
                List<double> zList = new List<double>();

                foreach (var item in result.MotorPos_mm)
                {
                    if (item.Key.Name == "TPSX")
                    {
                        xList = item.Value;
                    }
                    if (item.Key.Name == "TPSY")
                    {
                        yList = item.Value;
                    }
                    if (item.Key.Name == "TPSZ")
                    {
                        zList = item.Value;
                    }
                }

                //运动卡模拟量通道
                pList = result.Voltage_mV[TestRecipe.Analog_CH - 1];
                if (pList.Max() >= 2600)
                {
                    List<double> tpList = new List<double>();
                    List<double> txList = new List<double>();
                    List<double> tyList = new List<double>();
                    List<double> tzList = new List<double>();

                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i] >= 2600)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);
                        }
                    }

                    double x_range = txList.Max() - txList.Min();
                    double y_range = tyList.Max() - tyList.Min();
                    double z_range = tzList.Max() - tzList.Min();

                    if (x_range <= TestRecipe.Rough_Radius / 2 && y_range <= TestRecipe.Rough_Radius / 2 && z_range <= TestRecipe.Rough_Radius / 2)
                    {

                        double x_center = txList[0];
                        double y_center = tyList[0];
                        double z_center = tzList[0];

                        double x_Maxr = Math.Max(Math.Abs(txList.Max() - x_center), Math.Abs(txList.Min() - x_center));
                        double y_Maxr = Math.Max(Math.Abs(tyList.Max() - y_center), Math.Abs(tyList.Min() - y_center));
                        double z_Maxr = Math.Max(Math.Abs(tzList.Max() - z_center), Math.Abs(tzList.Min() - z_center));

                        double x_Minr = Math.Min(Math.Abs(txList.Max() - x_center), Math.Abs(txList.Min() - x_center));
                        double y_Minr = Math.Min(Math.Abs(tyList.Max() - y_center), Math.Abs(tyList.Min() - y_center));
                        double z_Minr = Math.Min(Math.Abs(tzList.Max() - z_center), Math.Abs(tzList.Min() - z_center));

                        //计算出三维空间半径
                        double Maxr = Math.Sqrt(Math.Pow(x_Maxr, 2) + Math.Pow(y_Maxr, 2) + Math.Pow(z_Maxr, 2));
                        double Minr = Math.Sqrt(Math.Pow(x_Minr, 2) + Math.Pow(y_Minr, 2) + Math.Pow(z_Minr, 2));

                        //最大三维半径 最大半径加一倍的光斑范围
                        return Maxr + (Maxr - Minr);
                    }
                    else
                    {
                        return TestRecipe.Rough_Radius;
                    }
                }

                return TestRecipe.Rough_Radius;

            }
            catch (Exception ex)
            {
                return TestRecipe.Rough_Radius;
            }
        }


        /// <summary>
        /// 存储RawData并返回峰值点位
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool DataAnalyze(TrajResultItem result, bool SaveRawData, out Dictionary<AxesPosition, double> AnalyzeResult)
        {
            try
            {
                List<double> pList = new List<double>();
                List<double> xList = new List<double>();
                List<double> yList = new List<double>();
                List<double> zList = new List<double>();

                List<double> tpList = new List<double>();
                List<double> txList = new List<double>();
                List<double> tyList = new List<double>();
                List<double> tzList = new List<double>();


                //挡位
                Range_At2500mV = Keithley_6485.CurrentSenseRange_A;

                foreach (var item in result.MotorPos_mm)
                {
                    if (item.Key.Name == "TPSX")
                    {
                        xList = item.Value;
                    }
                    if (item.Key.Name == "TPSY")
                    {
                        yList = item.Value;
                    }
                    if (item.Key.Name == "TPSZ")
                    {
                        zList = item.Value;
                    }
                }

                //运动卡模拟量通道
                pList = result.Voltage_mV[TestRecipe.Analog_CH - 1];
                if (pList.Max() >= 2600)
                {
                    tpList = new List<double>();
                    txList = new List<double>();
                    tyList = new List<double>();
                    tzList = new List<double>();

                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i] >= 2600)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);
                        }
                    }

                    double x_range = txList.Max() - txList.Min();
                    double y_range = tyList.Max() - tyList.Min();
                    double z_range = tzList.Max() - tzList.Min();

                    if (x_range <= TestRecipe.Fine_Radius * 1.5 && y_range <= TestRecipe.Fine_Radius * 1.5 && z_range <= TestRecipe.Fine_Radius * 1.5)
                    {
                        var tPmax = CloneHelper.Clone<AxesPosition>(t_Start_Pos);
                        tPmax.ItemCollection.Find(item => item.Name == "TPSX").Position = txList.Average();
                        tPmax.ItemCollection.Find(item => item.Name == "TPSY").Position = tyList.Average();
                        tPmax.ItemCollection.Find(item => item.Name == "TPSZ").Position = tzList.Average();

                        Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                        //PD电流
                        tmaxPoint.Add(tPmax, CalcPD_Current_mA(2500));

                        AnalyzeResult = tmaxPoint;

                        Keithley_6485.CurrentSenseRange_A *= 10;    //这里超过量程了, 也需要跳挡

                        return true;
                    }
                    else
                    {
                        //20230224 面积中心做返回值
                        var pmax = pList.Max();
                        var pmin = pList.Min();

                        //使用黄金分割高度
                        var threshold_power = (pmax - pmin) * 0.618 + pmin;

                        tpList = new List<double>();
                        txList = new List<double>();
                        tyList = new List<double>();
                        tzList = new List<double>();

                        for (int i = 0; i < pList.Count; i++)
                        {
                            if (pList[i] >= threshold_power)
                            {
                                tpList.Add(pList[i]);
                                txList.Add(xList[i]);
                                tyList.Add(yList[i]);
                                tzList.Add(zList[i]);
                            }
                        }

                        var tPmax = CloneHelper.Clone<AxesPosition>(t_Start_Pos);
                        tPmax.ItemCollection.Find(item => item.Name == "TPSX").Position = txList.Average();
                        tPmax.ItemCollection.Find(item => item.Name == "TPSY").Position = tyList.Average();
                        tPmax.ItemCollection.Find(item => item.Name == "TPSZ").Position = tzList.Average();

                        var maxIndex = GetMax(pList);

                        Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                        //PD电流
                        tmaxPoint.Add(tPmax, CalcPD_Current_mA(pList[maxIndex]));

                        AnalyzeResult = tmaxPoint;

                        return false;
                    }

                }

                //保存RawData
                if (SaveRawData)
                {
                    for (int i = 0; i < pList.Count; i++)
                    {
                        RawDatumItem_Coupling_PD rawdatumitem = new RawDatumItem_Coupling_PD();
                        rawdatumitem.Pos_X = Math.Round(xList[i], 5);
                        rawdatumitem.Pos_Y = Math.Round(yList[i], 5);
                        rawdatumitem.Pos_Z = Math.Round(zList[i], 5);
                        rawdatumitem.Power = Math.Round(pList[i], 5);
                        rawdatumitem.PDCurrent_mA = CalcPD_Current_mA(pList[i]);
                        RawData.DataCollection.Add(rawdatumitem);
                    }
                }

                //20230224 面积中心做返回值
                {
                    var pmax = pList.Max();
                    var pmin = pList.Min();

                    //使用黄金分割高度
                    var threshold_power = (pmax - pmin) * 0.618 + pmin;

                    tpList = new List<double>();
                    txList = new List<double>();
                    tyList = new List<double>();
                    tzList = new List<double>();

                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i] >= threshold_power)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);
                        }
                    }

                    var tPmax = CloneHelper.Clone<AxesPosition>(t_Start_Pos);
                    tPmax.ItemCollection.Find(item => item.Name == "TPSX").Position = txList.Average();
                    tPmax.ItemCollection.Find(item => item.Name == "TPSY").Position = tyList.Average();
                    tPmax.ItemCollection.Find(item => item.Name == "TPSZ").Position = tzList.Average();

                    var maxIndex = GetMax(pList);

                    Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                    //PD电流
                    tmaxPoint.Add(tPmax, CalcPD_Current_mA(pList[maxIndex]));

                    AnalyzeResult = tmaxPoint;

                    return true;
                }
            }
            catch (Exception ex)
            {
                AnalyzeResult = null;
                return false;
            }
        }

        /// <summary>
        /// 求峰值index
        /// </summary>
        /// <param name="pList"></param>
        /// <returns></returns>
        public int GetMax(List<double> pList)
        {
            try
            {
                double[] countArr = new double[pList.Count];
                for (int i = 0; i < countArr.Length; i++)
                {
                    countArr[i] = i + 1;
                }
                double[] smoothArr = ArrayMath.CalculateSmoothedNthDerivate(countArr, pList.ToArray(), 1, 3, 11);
                Dictionary<int, double> maxDict = new Dictionary<int, double>();
                var top = pList.Select((value, index) => new { value, index })
                                    .OrderByDescending(item => item.value)
                                    .ThenByDescending(item => item.index)
                                    .Take(3)
                                    .ToArray();
                foreach (var item in top)
                {
                    maxDict.Add(item.index, item.value);
                }
                var halfHeight = (pList.Max() - pList.Min()) * 0.75;
                Dictionary<int, double> finalDict = new Dictionary<int, double>();
                foreach (var item in maxDict)
                {
                    if (item.Key == 0)
                    {
                        if (pList[item.Key] - pList[item.Key + 1] >= halfHeight)
                        {
                        }
                        else
                        {
                            finalDict.Add(item.Key, item.Value);
                        }
                    }
                    else if (item.Key == pList.Count - 1)
                    {
                        if (pList[item.Key] - pList[item.Key - 1] >= halfHeight)
                        {
                        }
                        else
                        {
                            finalDict.Add(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        if (pList[item.Key] - pList[item.Key - 1] >= halfHeight ||
                        pList[item.Key] - pList[item.Key + 1] >= halfHeight)
                        {
                        }
                        else
                        {
                            finalDict.Add(item.Key, item.Value);
                        }
                    }
                }

                return finalDict.Aggregate((m, n) => m.Value > n.Value ? m : n).Key;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public enum eRunSize_Table
        {
            Rough,      //粗扫
            Rough_Half,

            Fine_Double, //精扫
            Fine,
            Fine_Half,
        }

        /// <summary>
        /// 插补运动
        /// </summary>
        /// <param name="isFine"></param>
        /// <param name="radius"></param>
        /// <param name="Position"></param>
        /// <param name="Plane"></param>
        /// <returns></returns>
        public TrajResultItem Run_Involute(eRunSize_Table eSize, AxesPosition Position, LaserX_9078_Utilities.PmTrajSelectPlane Plane, CancellationToken token)
        {
            //Dictionary<PmTrajAxisType, MotorAxisBase> axisDict = new Dictionary<PmTrajAxisType, MotorAxisBase>(); //插补轴定义
            //axisDict.Add(PmTrajAxisType.X_Dir, this.TPSX);
            //axisDict.Add(PmTrajAxisType.Y_Dir, this.TPSY);
            //axisDict.Add(PmTrajAxisType.Z_Dir, this.TPSZ);

            //AxesPosition Center = CloneHelper.Clone<AxesPosition>(Position);
            //TrajResultItem result = new TrajResultItem();

            int rtn = 0;
            double RadiusSales = 1;       //比例
            double IntervalSales = 1;
            double Rough_R = 1;
            double Rough_Inv = 1;

            double Trajspeed = 1;

            //螺旋运动
            switch (eSize)
            {
                case eRunSize_Table.Rough:  //粗扫
                    {
                        RadiusSales = 1;
                        IntervalSales = 1;
                        Rough_R = TestRecipe.Rough_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Rough_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Rough_Trajspeed;
                    }
                    break;

                case eRunSize_Table.Rough_Half:
                    {
                        RadiusSales = 0.5;
                        IntervalSales = 0.5;
                        Rough_R = TestRecipe.Rough_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Rough_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Rough_Trajspeed * RadiusSales;
                    }
                    break;

                case eRunSize_Table.Fine_Double:   //精扫
                    {
                        RadiusSales = 1.5;
                        IntervalSales = 2;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Fine_Trajspeed;
                    }
                    break;

                case eRunSize_Table.Fine:   //精扫
                    {
                        RadiusSales = 1;
                        IntervalSales = 1;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Fine_Trajspeed * RadiusSales;
                    }
                    break;

                case eRunSize_Table.Fine_Half:
                    {
                        RadiusSales = 0.5;
                        IntervalSales = 0.5;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Fine_Trajspeed * RadiusSales;
                    }
                    break;
            }

            return Run_Involute_Parameter(Rough_R, Rough_Inv, Trajspeed, Position, Plane, token);

            if (token.IsCancellationRequested)
            {
                Log_Global("用户取消测试");
                token.ThrowIfCancellationRequested();
                throw new OperationCanceledException();
            }

            //rtn = Parallel_2DCycleInvolute(
            //    axisDict,
            //    Center,
            //    Rough_R,
            //    Rough_Inv,
            //    Plane,
            //    true,      //true:渐开线    false：渐近线
            //    Trajspeed, out result, SpeedLevel.Normal);

            //if (rtn != 0)
            //{
            //    //异常返回;
            //}

            //return result;
        }
        /// <summary>
        /// 插补运动
        /// </summary>
        /// <param name="Rough_R">半径</param>
        /// <param name="Rough_Inv">间距</param>
        /// <param name="Trajspeed">速度</param>
        /// <param name="Position"></param>
        /// <param name="Plane"></param>
        /// <returns></returns>
        public TrajResultItem Run_Involute_Rough_ParameterR(eRunSize_Table eSize, double Rough_R, AxesPosition Position, LaserX_9078_Utilities.PmTrajSelectPlane Plane, CancellationToken token)
        {
            int rtn = 0;
            double RadiusSales = 1;       //比例
            double IntervalSales = 1;
            double Rough_Inv = 1;

            double Trajspeed = 1;

            //螺旋运动
            switch (eSize)
            {
                case eRunSize_Table.Rough:  //粗扫
                    {
                        RadiusSales = 1;
                        IntervalSales = 1;
                        Rough_Inv = TestRecipe.Rough_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Rough_Trajspeed;
                    }
                    break;

                case eRunSize_Table.Rough_Half:
                    {
                        RadiusSales = 0.5;
                        IntervalSales = 0.5;
                        Rough_R = TestRecipe.Rough_Radius * RadiusSales;
                        Trajspeed = TestRecipe.Rough_Trajspeed * RadiusSales;
                    }
                    break;
                default:
                    TrajResultItem result = new TrajResultItem();
                    return result;
            }

            return Run_Involute_Parameter(Rough_R, Rough_Inv, Trajspeed, Position, Plane, token);

            if (token.IsCancellationRequested)
            {
                Log_Global("用户取消测试");
                token.ThrowIfCancellationRequested();
                throw new OperationCanceledException();
            }
        }

        /// <summary>
        /// 插补运动
        /// </summary>
        /// <param name="Rough_R">半径</param>
        /// <param name="Rough_Inv">间距</param>
        /// <param name="Trajspeed">速度</param>
        /// <param name="Position"></param>
        /// <param name="Plane"></param>
        /// <returns></returns>
        public TrajResultItem Run_Involute_Parameter(double Rough_R, double Rough_Inv, double Trajspeed, AxesPosition Position, LaserX_9078_Utilities.PmTrajSelectPlane Plane, CancellationToken token)
        {
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict = new Dictionary<PmTrajAxisType, MotorAxisBase>(); //插补轴定义
            axisDict.Add(PmTrajAxisType.X_Dir, this.TPSX);
            axisDict.Add(PmTrajAxisType.Y_Dir, this.TPSY);
            axisDict.Add(PmTrajAxisType.Z_Dir, this.TPSZ);

            AxesPosition Center = CloneHelper.Clone<AxesPosition>(Position);
            TrajResultItem result = new TrajResultItem();

            int rtn = 0;

            rtn = Parallel_2DCycleInvolute(
                axisDict,
                Center,
                Rough_R,
                Rough_Inv,
                Plane,
                true,      //true:渐开线    false：渐近线
                Trajspeed, out result,
                token,
                SpeedLevel.Normal);

            if (rtn != 0)
            {
                //异常返回;
            }

            return result;
        }


        public Dictionary<AxesPosition, double> Run_Cross(eRunSize_Table eSize, AxesPosition Position, string path)
        {
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict = new Dictionary<PmTrajAxisType, MotorAxisBase>(); //插补轴定义
            axisDict.Add(PmTrajAxisType.X_Dir, this.TPSX);
            axisDict.Add(PmTrajAxisType.Y_Dir, this.TPSY);
            axisDict.Add(PmTrajAxisType.Z_Dir, this.TPSZ);

            AxesPosition Center = CloneHelper.Clone<AxesPosition>(Position);
            int rtn = 0;
            TrajResultItem result = new TrajResultItem();
            var max = new Dictionary<AxesPosition, double>();

            double RadiusSales = 1;       //比例
            double IntervalSales = 1;
            double Rough_R = 1;
            double Rough_Inv = 1;

            double Trajspeed = 1;

            //螺旋运动
            switch (eSize)
            {
                case eRunSize_Table.Rough:  //粗扫
                    {
                        RadiusSales = 1;
                        IntervalSales = 1;
                        Rough_R = TestRecipe.Rough_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Rough_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Rough_Trajspeed;
                    }
                    break;

                case eRunSize_Table.Rough_Half:
                    {
                        RadiusSales = 0.5;
                        IntervalSales = 0.5;
                        Rough_R = TestRecipe.Rough_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Rough_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Rough_Trajspeed * RadiusSales;
                    }
                    break;

                case eRunSize_Table.Fine:   //精扫
                    {
                        RadiusSales = 1;
                        IntervalSales = 1;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Fine_Trajspeed * RadiusSales;
                    }
                    break;

                case eRunSize_Table.Fine_Half:
                    {
                        RadiusSales = 0.5;
                        IntervalSales = 0.5;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Fine_Trajspeed * RadiusSales;
                    }
                    break;
            }

            if (false) // TestRecipe.Cross_Times != 0)
            {
                //for (int i = 0; i < TestRecipe.Cross_Times; i++)
                //{
                //    var xstart = new AxesPosition();
                //    var xend = new AxesPosition();
                //    var ystart = new AxesPosition();
                //    var yend = new AxesPosition();
                //    var strb = new StringBuilder();

                //    xstart = CloneHelper.Clone<AxesPosition>(Center);
                //    xend = CloneHelper.Clone<AxesPosition>(Center);
                //    xstart.ItemCollection.Find(item => item.Name == "TPSX").Position -= TestRecipe.Cross_Width;
                //    xend.ItemCollection.Find(item => item.Name == "TPSX").Position += TestRecipe.Cross_Width;

                //    rtn = Parallel_MoveLine(axisDict, xstart, xend, TestRecipe.Fine_Trajspeed, out result, SpeedLevel.Normal);
                //    DataAnalyze(result, false, out max);
                //    Center = CloneHelper.Clone<AxesPosition>(max.Last().Key);

                //    //输出csv(测试用)
                //    if (TestRecipe.SaveDebugFiles)
                //    {
                //        strb = PrintCSV(result);
                //        var sw0 = new StreamWriter(path + $@"\十字线扫X{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                //        sw0.Write(strb.ToString());
                //        sw0.Close(); strb.Clear();
                //    }

                //    ystart = CloneHelper.Clone<AxesPosition>(Center);
                //    yend = CloneHelper.Clone<AxesPosition>(Center);
                //    ystart.ItemCollection.Find(item => item.Name == "TPSY").Position -= TestRecipe.Cross_Width;
                //    yend.ItemCollection.Find(item => item.Name == "TPSY").Position += TestRecipe.Cross_Width;

                //    rtn = Parallel_MoveLine(axisDict, ystart, yend, TestRecipe.Fine_Trajspeed, out result, SpeedLevel.Normal);
                //    if (i == TestRecipe.Cross_Times - 1)
                //    {
                //        DataAnalyze(result, true, out max);
                //    }
                //    else
                //    {
                //        DataAnalyze(result, false, out max);
                //    }
                //    Center = CloneHelper.Clone<AxesPosition>(max.Last().Key);

                //    //输出csv(测试用)
                //    if (TestRecipe.SaveDebugFiles)
                //    {
                //        strb = PrintCSV(result);
                //        var sw0 = new StreamWriter(path + $@"\十字线扫Y{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                //        sw0.Write(strb.ToString());
                //        sw0.Close(); strb.Clear();
                //    }
                //}
            }
            else
            {
                max = null;
            }

            return max;
        }


        //单点搜索
        public Dictionary<AxesPosition, double> Run_PointSearch(double Area, double Step, AxesPosition Position, bool SaveRawData, string path, CancellationToken token)
        {
            if (Area == 0 || Step == 0 || Step >= Area)
            {
                return null;
            }

            if (Area / Step < 2)
            {
                return null;
            }

            int CPTimes = this.TestRecipe.CrossPoint_Times;
            if (CPTimes <= 0)
            {
                return null;
            }
            if (CPTimes > 100) CPTimes = 100;


            MotionActionV2 Action_Axis_X = new MotionActionV2();
            MotionActionV2 Action_Axis_Y = new MotionActionV2();
            MotionActionV2 Action_Axis_Z = new MotionActionV2();

            Dictionary<MotorAxisBase, MotionActionV2> axisDict = new Dictionary<MotorAxisBase, MotionActionV2>();
            axisDict.Add(this.TPSX, Action_Axis_X);
            axisDict.Add(this.TPSY, Action_Axis_Y);
            axisDict.Add(this.TPSZ, Action_Axis_Z);


            //初始参考点
            string X_AxisName = "TPSX";
            string Y_AxisName = "TPSY";

            //搜索的中心位置
            double XCenterPos = Position.ItemCollection.Find(item => item.Name == X_AxisName).Position;
            double YCenterPos = Position.ItemCollection.Find(item => item.Name == Y_AxisName).Position;

            //重复运动的初始位置
            double Xminpos = Position.ItemCollection.Find(item => item.Name == X_AxisName).Position - Area / 2;
            double Yminpos = Position.ItemCollection.Find(item => item.Name == Y_AxisName).Position - Area / 2;

            AxesPosition Start_Position = CloneHelper.Clone<AxesPosition>(Position);
            Start_Position.GetSingleItem(X_AxisName).Position = Xminpos;
            Start_Position.GetSingleItem(Y_AxisName).Position = Yminpos;

            //基于中心是0,0的情况下的步进数量
            int ScanCount = (int)Math.Ceiling(Area / Step / 2);


            //运行中的位置
            //运行中的位置
            AxesPosition Target_Position = CloneHelper.Clone<AxesPosition>(Position);

            double TargetPos_X = 0;
            double TargetPos_Y = 0;

            var TargetSearch = new Dictionary<AxesPosition, double>();

            int delay_ms = this.TestRecipe.CrossPoint_DelayBeforeMeasure_ms;

            if (delay_ms <= 5)
                delay_ms = 5;
            if (delay_ms > 10 * 1000) delay_ms = 10 * 1000;

            //建立搜索序列
            for (int CPTimes_i = 0; CPTimes_i < CPTimes; CPTimes_i++)
            {
                //首先运动到开始点
                MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, Start_Position, SequenceOrder.Normal);

                #region X方向运动
                Target_Position.GetSingleItem(Y_AxisName).Position = YCenterPos + TargetPos_Y * Step;

                List<PointF> X_Target_Data = new List<PointF>();
                List<double> Target_Data = new List<double>();
                for (int X_Target_i = -ScanCount; X_Target_i < ScanCount; X_Target_i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Log_Global("用户取消测试");
                        token.ThrowIfCancellationRequested();
                        throw new OperationCanceledException();
                    }

                    //运动到Y方向的初始点
                    Target_Position.GetSingleItem(X_AxisName).Position = XCenterPos + X_Target_i * Step;

                    //运行到目标点
                    MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, Target_Position, SequenceOrder.Normal);

                    Thread.Sleep(delay_ms);

                    double K_Curr_mA;
                    //捕捉数据
                    do
                    {
                        K_Curr_mA = Keithley_6485.ReadCurrent_A() * 1000;
                        if (K_Curr_mA > 3000)
                        {
                            Keithley_6485.CurrentSenseRange_A *= 10;
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (true);

                    TargetSearch.Add(CloneHelper.Clone<AxesPosition>(Target_Position), K_Curr_mA);
                    X_Target_Data.Add(new PointF(X_Target_i, (float)K_Curr_mA));
                    Target_Data.Add(K_Curr_mA);
                }
                {//计算最大值

                    double Th = (Target_Data.Max() - Target_Data.Min()) / 2 + Target_Data.Min();

                    List<int> pos = new List<int>();
                    foreach (var item in X_Target_Data)
                    {
                        if (item.Y > Th)
                            pos.Add((int)item.X);
                    }

                    TargetPos_X = pos.Average();
                }

                #endregion

                //首先运动到开始点
                MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, Start_Position, SequenceOrder.Normal);


                #region Y方向运动
                Target_Position.GetSingleItem(X_AxisName).Position = XCenterPos + TargetPos_X * Step;

                List<PointF> Y_Target_Data = new List<PointF>();
                Target_Data = new List<double>();
                for (int Y_Target_i = -ScanCount; Y_Target_i < ScanCount; Y_Target_i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Log_Global("用户取消测试");
                        token.ThrowIfCancellationRequested();
                        throw new OperationCanceledException();
                    }

                    //运动到Y方向的初始点
                    Target_Position.GetSingleItem(Y_AxisName).Position = YCenterPos + Y_Target_i * Step;

                    //运行到目标点
                    MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, Target_Position, SequenceOrder.Normal);

                    Thread.Sleep(delay_ms);

                    double K_Curr_mA;
                    //捕捉数据
                    do
                    {
                        K_Curr_mA = Keithley_6485.ReadCurrent_A() * 1000;
                        if (K_Curr_mA > 3000)
                        {
                            Keithley_6485.CurrentSenseRange_A *= 10;
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (true);

                    TargetSearch.Add(CloneHelper.Clone<AxesPosition>(Target_Position), K_Curr_mA);
                    Y_Target_Data.Add(new PointF(Y_Target_i, (float)K_Curr_mA));
                    Target_Data.Add(K_Curr_mA);
                }
                {//计算最大值

                    double Th = (Target_Data.Max() - Target_Data.Min()) / 2 + Target_Data.Min();

                    List<int> pos = new List<int>();
                    foreach (var item in Y_Target_Data)
                    {
                        if (item.Y > Th)
                            pos.Add((int)item.X);
                    }

                    TargetPos_Y = pos.Average();
                }

                #endregion


            }

            //保存RawData
            if (SaveRawData)
            {
                foreach (var item in TargetSearch)
                {
                    RawDatumItem_Coupling_PD rawdatumitem = new RawDatumItem_Coupling_PD();
                    rawdatumitem.Pos_X = Math.Round(item.Key.GetSingleItem(this.TPSX.Name).Position, 6);
                    rawdatumitem.Pos_Y = Math.Round(item.Key.GetSingleItem(this.TPSY.Name).Position, 6);
                    rawdatumitem.Pos_Z = Math.Round(item.Key.GetSingleItem(this.TPSZ.Name).Position, 6);
                    rawdatumitem.Power = Math.Round(item.Value, 6);
                    rawdatumitem.PDCurrent_mA = Math.Round(item.Value, 6);
                    RawData.DataCollection.Add(rawdatumitem);
                }
            }

            //首先运动到开始点
            MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, Start_Position, SequenceOrder.Normal);

            Target_Position.GetSingleItem(X_AxisName).Position = XCenterPos + TargetPos_X * Step;
            Target_Position.GetSingleItem(Y_AxisName).Position = YCenterPos + TargetPos_Y * Step;

            //运行到目标点
            MultipleAxisAction.Sequence_MoveToAxesPosition(axisDict, Target_Position, SequenceOrder.Normal);

            //重新按序列运动到此位置
            {
                double K_Curr_mA;
                //捕捉数据
                do
                {
                    K_Curr_mA = Keithley_6485.ReadCurrent_A() * 1000;
                    if (K_Curr_mA > 3000)
                    {
                        Keithley_6485.CurrentSenseRange_A *= 10;
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);

                TargetSearch.Add(CloneHelper.Clone<AxesPosition>(Target_Position), K_Curr_mA);

                Log_Global($"搜索到最大电流={K_Curr_mA}mA");

                return TargetSearch;
            }

            return null;
        }
    }
}