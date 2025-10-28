using LX_BurnInSolution.Utilities;
using SolveWare_Analog;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static SolveWare_BurnInInstruments.LaserX_9078_Utilities;
using static SolveWare_TestPackage.LaserX_9078_Traj_Function;
using static SolveWare_TestPackage.TestModule_AA;

namespace SolveWare_TestPackage
{
 
    #region  轴、位置、IO、仪器
 
    [StaticResource(ResourceItemType.IO, "PD_3", "切换PD")]
    [StaticResource(ResourceItemType.AXIS, "LNX", "LNX")] // 耦合模块X轴
    [StaticResource(ResourceItemType.AXIS, "LNY", "LNY")] // 耦合模块Y轴
    [StaticResource(ResourceItemType.AXIS, "LNZ", "LNZ")] // 耦合模块Z轴
    [ConfigurableInstrument("PXISourceMeter_4143", "PD", "PD")]
    [ConfigurableInstrument("PXISourceMeter_4143", "SOA1", "SOA1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "SOA2", "SOA2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "LP", "LP")]
    [ConfigurableInstrument("PXISourceMeter_4143", "PH1", "PH1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "PH2", "PH2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MIRROR1", "MIRROR1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MIRROR2", "MIRROR2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "BIAS1", "BIAS1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "BIAS2", "BIAS2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MPD1", "MPD1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MPD2", "MPD2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "GAIN", "GAIN")]
    [ConfigurableInstrument("PXISourceMeter_6683H", "6683H", "6683H")]
    [ConfigurableInstrument("FWM8612", "FWM8612", "波长计")]
    [ConfigurableInstrument("ScpiOsa", "OSA", "OSA")]
    [ConfigurableInstrument("OSA_AQ67370", "OSA_6370", "OSA_6370")]
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]

    #endregion
    public class TestModule_AA_FineTuning : TestModuleBase
    {

        string ModuleName = "AA_FineTuning";
        public TestModule_AA_FineTuning() : base() { }

        #region 以Get获取资源
        IOBase SwitchPD { get { return (IOBase)this.ModuleResource["PD_3"]; } }
        private MotorAxisBase X2 { get { return (MotorAxisBase)this.ModuleResource["LNX"]; } }
        private MotorAxisBase Y2 { get { return (MotorAxisBase)this.ModuleResource["LNY"]; } }
        private MotorAxisBase Z2 { get { return (MotorAxisBase)this.ModuleResource["LNZ"]; } }

        PXISourceMeter_4143 PD { get { return (PXISourceMeter_4143)this.ModuleResource["PD"]; } }
        PXISourceMeter_4143 SOA1 { get { return (PXISourceMeter_4143)this.ModuleResource["SOA1"]; } }
        PXISourceMeter_4143 SOA2 { get { return (PXISourceMeter_4143)this.ModuleResource["SOA2"]; } }
        PXISourceMeter_4143 LP { get { return (PXISourceMeter_4143)this.ModuleResource["LP"]; } }
        PXISourceMeter_4143 PH1 { get { return (PXISourceMeter_4143)this.ModuleResource["PH1"]; } }
        PXISourceMeter_4143 PH2 { get { return (PXISourceMeter_4143)this.ModuleResource["PH2"]; } }
        PXISourceMeter_4143 MIRROR1 { get { return (PXISourceMeter_4143)this.ModuleResource["MIRROR1"]; } }
        PXISourceMeter_4143 MIRROR2 { get { return (PXISourceMeter_4143)this.ModuleResource["MIRROR2"]; } }
        PXISourceMeter_4143 BIAS1 { get { return (PXISourceMeter_4143)this.ModuleResource["BIAS1"]; } }
        PXISourceMeter_4143 BIAS2 { get { return (PXISourceMeter_4143)this.ModuleResource["BIAS2"]; } }
        PXISourceMeter_4143 MPD1 { get { return (PXISourceMeter_4143)this.ModuleResource["MPD1"]; } }
        PXISourceMeter_4143 MPD2 { get { return (PXISourceMeter_4143)this.ModuleResource["MPD2"]; } }
        PXISourceMeter_4143 GAIN { get { return (PXISourceMeter_4143)this.ModuleResource["GAIN"]; } }
        PXISourceMeter_6683H S_6683H { get { return (PXISourceMeter_6683H)this.ModuleResource["6683H"]; } }

        FWM8612 FWM8612 { get { return (FWM8612)this.ModuleResource["FWM8612"]; } }

        ScpiOsa OSA_86142B { get { return (ScpiOsa)this.ModuleResource["OSA"]; } }
        OSA_AQ67370 OSA_6370 { get { return (OSA_AQ67370)this.ModuleResource["OSA_6370"]; } }
        private OpticalSwitch OSwitch { get { return (OpticalSwitch)this.ModuleResource["OSwitch"]; } }

        #endregion
        IDeviceStreamDataBase _dutStreamData;
        TestRecipe_AA_FineTuning TestRecipe { get; set; }
   
        QWLT2_TestData qWLT2_TestDta { get; set; }
 
        private RawData_AA RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_AA_FineTuning);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_AA(); 
            return RawData;
       
        }
        public void Choose(string section, out PXISourceMeter_4143 pXISource)
        {
            var source = (Section)Enum.Parse(typeof(Section), section);
            switch (source)
            {
                case Section.PD:
                    pXISource = PD;
                    break;
                case Section.SOA1:
                    pXISource = SOA1;
                    break;
                case Section.SOA2:
                    pXISource = SOA2;
                    break;
                case Section.LP:
                    pXISource = LP;
                    break;
                case Section.PH1:
                    pXISource = PH1;
                    break;
                case Section.PH2:
                    pXISource = PH2;
                    break;
                case Section.MIRROR1:
                    pXISource = MIRROR1;
                    break;
                case Section.MIRROR2:
                    pXISource = MIRROR2;
                    break;
                case Section.BIAS1:
                    pXISource = BIAS1;
                    break;
                case Section.BIAS2:
                    pXISource = BIAS2;
                    break;
                case Section.MPD1:
                    pXISource = MPD1;
                    break;
                case Section.MPD2:
                    pXISource = MPD2;
                    break;
                case Section.GAIN:
                    pXISource = GAIN;
                    break;
                default:
                    pXISource = null;
                    break;
            }
        }
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_AA_FineTuning>(testRecipe);
        }

        public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        {
            try
            {
                _dutStreamData = dutStreamData;
                SerialNumber = dutStreamData.SerialNumber;
                if (dutStreamData.RawDataCollecetionCount < 2)
                {
                    return;
                }
                this.qWLT2_TestDta = QWLT2_TestData.GetQwlt2_Data(dutStreamData);
            }
            catch (Exception ex)
            {

            }
        }
        string MaskName { get; set; }
        string SerialNumber { get; set; }
 

        public override void Run(CancellationToken token)
        {
            try
            {
                int SerachDir = 1;

                if (TestRecipe.Creep_Step_um > 0)
                {
                    this.Log_Global($"重新快速找光");

                    Merged_PXIe_4143.Reset();

                    this.Log_Global("开始加电.");

                    OptialPath_Controller.SwitchTo(OSwitch, OptialPath.TapPD);

                    //var och = Convert.ToByte(this.TestRecipe.LIVOpticalSwitchChannel);
                    //if (OSwitch.SetCH(och) == false)
                    //{
                    //    string msg = "光开关通道切换失败！";
                    //    this.Log_Global(msg);
                    //    throw new Exception(msg);
                    //}

                    if (this.TestRecipe.Inherit)
                    {

                        GAIN.AssignmentMode_Current(qWLT2_TestDta.GAIN, 2.5);
                        LP.AssignmentMode_Current(qWLT2_TestDta.LP, 2.5);
                        MIRROR1.AssignmentMode_Current(qWLT2_TestDta.MIRROR1, 2.5);
                        MIRROR2.AssignmentMode_Current(qWLT2_TestDta.MIRROR2, 2.5);

                        SOA1.AssignmentMode_Current(qWLT2_TestDta.SOA1, 2.5);
                        SOA2.AssignmentMode_Current(qWLT2_TestDta.SOA2, 2.5);

                        PH1.AssignmentMode_Current(qWLT2_TestDta.PH1, 2.5);
                        PH2.AssignmentMode_Current(qWLT2_TestDta.PH2, 2.5);
                    }
                    else
                    {
                        //临时设定为定制  以下数据需要从qwlt2获取
                        GAIN.AssignmentMode_Current(120, 2.5);
                        LP.AssignmentMode_Current(1.25, 2.5);
                        MIRROR1.AssignmentMode_Current(9.3, 1.6);
                        MIRROR2.AssignmentMode_Current(10.7, 1.6);

                        //PH1.AssignmentMode_Current(6, 2.5);
                        //PH2.AssignmentMode_Current(0, 2.5);
                        SOA1.AssignmentMode_Current(50, 2.5);
                        SOA2.AssignmentMode_Current(40, 2.5);


                        MPD1.AssignmentMode_Voltage(-2, 20);
                        MPD2.AssignmentMode_Voltage(-2, 20);
                        BIAS1.AssignmentMode_Voltage(-3, 20);
                        BIAS2.AssignmentMode_Voltage(-3, 20);
                    }

                    //初始位
                    var actlnx = X2 as Motor_LaserX_9078;
                    var actlny = Y2 as Motor_LaserX_9078;
                    var actlnz = Z2 as Motor_LaserX_9078;
                    var ThreeAxisList = new List<Motor_LaserX_9078>() { actlnz, actlnx, actlny };

                    //初始位置
                    var t_Start_Pos = new AxesPosition()
                    {
                        ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlny.Name,
                                        CardNo =    actlny.CardNo.ToString(),
                                        AxisNo =    actlny.AxisNo.ToString(),
                                        Position =  actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlnz.Name,
                                        CardNo =    actlnz.CardNo.ToString(),
                                        AxisNo =    actlnz.AxisNo.ToString(),
                                        Position =  actlnz.Get_CurUnitPos()
                                    },
                                }
                    };

                    var P1 = new AxesPosition()
                    {
                        ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlny.Name,
                                        CardNo =    actlny.CardNo.ToString(),
                                        AxisNo =    actlny.AxisNo.ToString(),
                                        Position =  actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlnz.Name,
                                        CardNo =    actlnz.CardNo.ToString(),
                                        AxisNo =    actlnz.AxisNo.ToString(),
                                        Position =  actlnz.Get_CurUnitPos()
                                    },
                                }
                    };

                    foreach (var axis in ThreeAxisList)
                    {
                        axis.WaitMotionDone();
                    }


                    double size = TestRecipe.Fine_Radius;  //左右两边的空间

                    var ch = this.TestRecipe.Analog_CH - 1;


                    int id = 100;


                    double xSize = size;
                    double zSize = size;
                    double ySize = size;

                    double pd_Max = 0;

                    double current_PD_max = Analog_LaserX_9078.GetCurrent_mA(X2 as Motor_LaserX_9078, ch);
                    double Creep_step = TestRecipe.Creep_Step_um / 1000;//蠕动步进

                    //最大次数
                    int xserachcount = (int)(xSize / Creep_step);
                    int yserachcount = (int)(ySize / Creep_step);
                    int zserachcount = (int)(zSize / Creep_step);

                    int xyzserachcount = xserachcount;
                    int serachPointMax = 1;//需要2个点进行判断是否找到最大
                    int serachPointCount = 0;//当前搜索失败点

                    Motor_LaserX_9078 ln_axis = actlnx;
                    SerachDir = 1;
                    bool UpdatePosition;
                    int GetdataDelay_ms = (int)TestRecipe.CreepDelay_ms;

                    double axisspeed = TestRecipe.Fine_Trajspeed;

                    #region  先粗耦合一次

                    Circuit_Controller.TapPD_ConnectTo(SwitchPD, TapPD_Circuit.AlignmentSystem); //使用耦合通道
                    Thread.Sleep(100);

                    TrajResultItem result;
                    var UsedPlane = LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CW;

                    Dictionary<AxesPosition, double> retPoint = new Dictionary<AxesPosition, double>();
                    Dictionary<int, PointResult> maxList = new Dictionary<int, PointResult>();
                    string LogDataMsg = string.Empty;
                    string path_aa = Application.StartupPath + $@"\Data\AlignmentResult_FineTuning\{DateTime.Now:yyyyMMdd_HHmmss}";
                    if (!string.IsNullOrEmpty(SerialNumber))
                    {
                        path_aa = Application.StartupPath + $@"\Data\AlignmentResult_FineTuning\{SerialNumber}";
                    }
                    if (!Directory.Exists(path_aa))
                    {
                        Directory.CreateDirectory(path_aa);
                    }
                    StringBuilder strb = new StringBuilder();
                    StreamWriter sw_aa;
                    t_Start_Pos = new AxesPosition()
                    {
                        ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlny.Name,
                                        CardNo =    actlny.CardNo.ToString(),
                                        AxisNo =    actlny.AxisNo.ToString(),
                                        Position =  actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name =      actlnz.Name,
                                        CardNo =    actlnz.CardNo.ToString(),
                                        AxisNo =    actlnz.AxisNo.ToString(),
                                        Position =  actlnz.Get_CurUnitPos()
                                    },
                                }
                    };
                    var P1_AA = new AxesPosition()
                    {
                        ItemCollection =
                                {
                                    new AxisPosition()
                                    {
                                        Name = actlnx.Name,
                                        CardNo = actlnx.CardNo.ToString(),
                                        AxisNo = actlnx.AxisNo.ToString(),
                                        Position = actlnx.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name = actlny.Name,
                                        CardNo = actlny.CardNo.ToString(),
                                        AxisNo = actlny.AxisNo.ToString(),
                                        Position = actlny.Get_CurUnitPos()
                                    },
                                    new AxisPosition()
                                    {
                                        Name = actlnz.Name,
                                        CardNo = actlnz.CardNo.ToString(),
                                        AxisNo = actlnz.AxisNo.ToString(),
                                        Position = actlnz.Get_CurUnitPos()
                                    },
                                }
                    };


                    {
                        //阈值停止
                        TrajThresholdStop thresholdStop = new TrajThresholdStop()
                        {
                            En = true,
                            ThCurrent_mA = new Dictionary<int, double>(),
                            ThVoltage_mV = new Dictionary<int, double>()
                        };

                        //增加阈值
                        thresholdStop.ThCurrent_mA.Add(ch, TestRecipe.PowerThreshold_mA);

                        //设置挡位
                        Analog_LaserX_9078.SetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, ch, 0.1);

                        Log_Global($"开始AQWLT耦合");
                        result = Run_Involute(eRunSize_Table.Fine_Double, P1, UsedPlane, thresholdStop, token);

                        this.CheckCancellationRequested(token);

                        while (!DataAnalyze(t_Start_Pos, result, false, out retPoint))
                        {
                            this.CheckCancellationRequested(token);

                            LogDataMsg = path_aa + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_AAFT耦合超量程.csv";
                            this.WriteCSCVFile(LogDataMsg, out strb, out sw_aa, result);

                            result = Run_Involute(eRunSize_Table.Fine_Double, P1, UsedPlane, thresholdStop, token);

                            this.CheckCancellationRequested(token);
                        }
                        LogDataMsg = path_aa + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_AAFT耦合.csv";
                        this.WriteCSCVFile(LogDataMsg, out strb, out sw_aa, result);

                        this.JudgeThreshold_mW(retPoint);

                        P1_AA = this.FindP1(ThreeAxisList, maxList, 0, retPoint, false);

                        //运行到P1点
                        this.MoveToAxesPosition(ThreeAxisList, P1_AA, token);
                        Thread.Sleep(300);
                        this.CheckCancellationRequested(token);
                        Log_Global($"结束AAFT耦合");
                    }

                    #endregion



                    this.Log_Global($"恢复Gain电流,进行光电流耦合");

                    GAIN.AssignmentMode_Current(qWLT2_TestDta.GAIN, 2.5);

                    Circuit_Controller.TapPD_ConnectTo(SwitchPD ,TapPD_Circuit.SMU); //使用源表

                    double pdSenseCurrentRange_mA = 10;// Math.Round(pd_Max * 5, 6);

                    PD.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(0, pdSenseCurrentRange_mA);

                    Thread.Sleep(400);

                    pd_Max = PD.ReadCurrent_A() * 1000.0;
                    Log_Global($"源表读取当前光电流为[{pd_Max}]mA");

                    //最大轮
                    int maxStep = 1;
                    int SerachStep = 0;

                    //3方向搜索
                    for (int iSerach = 0; iSerach <= maxStep;)
                    {

                        switch (SerachStep)
                        {
                            case 0:  // X+
                                Log_Global($"开始蠕动扫描[{id}]");
                                if (iSerach == maxStep) //只离焦一次
                                {
                                    //远离焦点0.003mm
                                    double fd_um = this.TestRecipe.OutOfFocusDistance_um;
                                    if (fd_um < 0) fd_um = 0;
                                    if (fd_um > 1000) fd_um = 1000;
                                    Log_Global($"离焦[{fd_um}]um");
                                    actlny.MoveToV3(actlny.Get_CurUnitPos() - fd_um / 1000.0, axisspeed);  // 离焦
                                    actlny.WaitMotionDone();
                                    Thread.Sleep(GetdataDelay_ms);
                                }

                                ln_axis = actlnx;
                                break;

                            case 1:  // Z+
                                ln_axis = actlnz;
                                break;

                            case 2:  // Y+
                                ln_axis = actlny;
                                break;

                            case 3:

                                Log_Global($"AA_FT 蠕动扫描[{id}] 最大光电流为[{pd_Max}_mA]");

                                id++;
                                iSerach++;
                                SerachStep = 0;
                                continue;
                                break;

                        }

                        SerachStep++;

                        //最后2次搜索时候, 只判断一个点
                        if (iSerach >= maxStep)
                        {
                            serachPointMax = 1;

                            if (ln_axis == actlny)
                            {
                                continue;
                            }
                        }

                        #region 进行搜索

                        //+方向
                        SerachDir = 1;
                        serachPointCount = 1;
                        Log_Global($"AA_FT 蠕动扫描中[{ln_axis.Name}] 方向[{SerachDir}]");
                        pd_Max = CreepGetPDCurrent_mA(ln_axis, ch);
                        UpdatePosition = false;
                        for (int i = 0; i < xyzserachcount; i++)
                        {

                            ln_axis.MoveToV3(ln_axis.Get_CurUnitPos() + SerachDir * Creep_step, axisspeed);
                            ln_axis.WaitMotionDone();
                            Thread.Sleep(GetdataDelay_ms);

                            //得到当前电流                            
                            current_PD_max = CreepGetPDCurrent_mA(ln_axis, ch);

                            if (pd_Max > current_PD_max)
                            {
                                if (serachPointCount >= serachPointMax)
                                {
                                    ln_axis.MoveToV3(ln_axis.Get_CurUnitPos() + (-1) * serachPointMax * SerachDir * Creep_step, axisspeed);
                                    ln_axis.WaitMotionDone();
                                    Thread.Sleep(GetdataDelay_ms);
                                    break;
                                }
                                else
                                {
                                    serachPointCount++;
                                }


                            }
                            else
                            {
                                pd_Max = current_PD_max;
                                P1.ItemCollection.FirstOrDefault(axis => axis.Name == ln_axis.Name).Position = ln_axis.Get_CurUnitPos(); ;
                                UpdatePosition = true;
                                serachPointCount = 1;
                            }
                        }

                        if (UpdatePosition == false)
                        {
                            SerachDir = -1;
                            serachPointCount = 1;
                            Log_Global($"AA_FT 蠕动扫描中[{ln_axis.Name}] 方向[{SerachDir}]");
                            pd_Max = CreepGetPDCurrent_mA(ln_axis, ch);

                            for (int i = 0; i < xyzserachcount; i++)
                            {

                                ln_axis.MoveToV3(ln_axis.Get_CurUnitPos() + SerachDir * Creep_step, axisspeed);
                                ln_axis.WaitMotionDone();
                                Thread.Sleep(GetdataDelay_ms);

                                //得到当前电流
                                current_PD_max = CreepGetPDCurrent_mA(ln_axis, ch);

                                if (pd_Max > current_PD_max)
                                {
                                    if (serachPointCount >= serachPointMax)
                                    {
                                        ln_axis.MoveToV3(ln_axis.Get_CurUnitPos() + (-1) * serachPointMax * SerachDir * Creep_step, axisspeed);
                                        ln_axis.WaitMotionDone();
                                        Thread.Sleep(GetdataDelay_ms);
                                        break;
                                    }
                                    else
                                    {
                                        serachPointCount++;
                                    }

                                }
                                else
                                {
                                    pd_Max = current_PD_max;
                                    P1.ItemCollection.FirstOrDefault(axis => axis.Name == ln_axis.Name).Position = ln_axis.Get_CurUnitPos(); ;
                                    UpdatePosition = true;
                                    serachPointCount = 1;
                                }
                            }
                        }

                        #endregion


                    }

                    pd_Max = PD.ReadCurrent_A() * 1000.0;
                    Log_Global($"AAFT最终光电流为[{pd_Max}]mA ");
                    var orgX = X2.Get_CurUnitPos();
                    var orgY = Y2.Get_CurUnitPos();
                    var orgZ = Z2.Get_CurUnitPos();
                    this.Log_Global($"X_final = {orgX} Y_final = {orgY} Z_final = {orgZ} {Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
                this.Log_Global($"[{ex.Message}]-[{ex.StackTrace}]");
                throw new Exception($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {
                Merged_PXIe_4143.Reset();
                //Circuit_Controller.TapPD_ConnectTo(SwitchPD, TapPD_Circuit.AlignmentSystem);
                this.Log_Global($"结束测试!");
            }
        }

        /// <summary>
        /// 检查用户是否取消测试
        /// </summary>
        /// <param name="token"></param>
        private void CheckCancellationRequested(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                Log_Global("用户取消测试");
                token.ThrowIfCancellationRequested();
                throw new OperationCanceledException();
            }
        }

        private double CreepGetPDCurrent_mA(Motor_LaserX_9078 Axis, int index)
        {
            var val = PD.ReadCurrent_A();

            val *= 1000;
            Log_Global($"Debug 光电流为[{val}]mA");

            return val;

        }

        private double CreepGetCurrent_mA(Motor_LaserX_9078 Axis, int index)
        {
            double current_PD_max = 0;
            List<double> lstcurrent = new List<double>();
            var sense = Analog_LaserX_9078.GetSenseCurrentRange_mA(Axis, index); //当前挡位

            for (int i = 0; i < 10;)
            {
                //得到当前电流
                current_PD_max = Analog_LaserX_9078.GetCurrent_mA(Axis, index);



                if (current_PD_max > sense * 0.95)
                {
                    if (sense >= 1.8)
                    {
                        lstcurrent.Add(current_PD_max);
                        Log_Global($"已达最大电流档位");

                        break;
                    }

                    Analog_LaserX_9078.SetSenseCurrentRange_mA(Axis, index, sense * 2);
                    sense = Analog_LaserX_9078.GetSenseCurrentRange_mA(Axis, index); //当前挡位
                }
                else
                {
                    i++;
                    lstcurrent.Add(current_PD_max);
                    Thread.Sleep(5);
                }

            }

            return lstcurrent.Average();
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
                var ch = this.TestRecipe.Analog_CH - 1;
                List<double> pList = new List<double>();
                List<double> xList = new List<double>();
                List<double> yList = new List<double>();
                List<double> zList = new List<double>();

                foreach (var item in result.MotorPos_mm)
                {
                    if (item.Key.Name == "LNX")
                    {
                        xList = item.Value;
                    }
                    if (item.Key.Name == "LNY")
                    {
                        yList = item.Value;
                    }
                    if (item.Key.Name == "LNZ")
                    {
                        zList = item.Value;
                    }
                }

                //运动卡模拟量通道
                pList = result.Current_mA[ch];
                if (pList.Max() >= 2047)
                {
                    List<double> tpList = new List<double>();
                    List<double> txList = new List<double>();
                    List<double> tyList = new List<double>();
                    List<double> tzList = new List<double>();

                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i] >= 2047)
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

                    if (x_range <= this.TestRecipe.Fine_Radius / 2 && y_range <= this.TestRecipe.Fine_Radius / 2 && z_range <= this.TestRecipe.Fine_Radius / 2)
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
                        return this.TestRecipe.Fine_Radius;
                    }
                }

                return this.TestRecipe.Fine_Radius;
            }
            catch (Exception ex)
            {
                return this.TestRecipe.Fine_Radius;
            }
        }





        /// <summary>
        /// 写CSV文档
        /// </summary>
        /// <param name="LogDataMsg"></param>
        /// <param name="strb"></param>
        /// <param name="sw"></param>
        /// <param name="result"></param>
        private void WriteCSCVFile(string LogDataMsg, out StringBuilder strb, out StreamWriter sw, TrajResultItem result)
        {
            Log_Global($"原始数据:[{LogDataMsg}]");
            strb = PrintCSV(result);
            sw = new StreamWriter(LogDataMsg);
            sw.Write(strb.ToString());
            sw.Close(); strb.Clear();
        }
        public StringBuilder PrintCSV(TrajResultItem result)
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
        /// <summary>
        /// 找到最大点位置
        /// </summary>
        /// <param name="lnx"></param>
        /// <param name="threeAxisList"></param>
        /// <param name="P1"></param>
        /// <param name="zeroScan"></param>
        /// <param name="maxList"></param>
        /// <param name="ch"></param>
        /// <param name="id"></param>
        /// <param name="retPoint"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private AxesPosition FindP1(List<Motor_LaserX_9078> threeAxisList,
                            Dictionary<int, PointResult> maxList,
                            int id,
                            Dictionary<AxesPosition, double> retPoint,
                            bool addlist)
        {
            //判断是否无光
            //this.JudgeThreshold_mW(retPoint);
            return MaxListAdd(threeAxisList, maxList, id, retPoint, addlist);
        }
        /// <summary>
        /// 与门限值判断
        /// </summary>
        /// <param name="retPoint"></param>
        private void JudgeThreshold_mW(Dictionary<AxesPosition, double> retPoint)
        {
            if (retPoint.First().Value <= TestRecipe.PowerThreshold_mA)
            {
                //while(true)
                //{
                //    Thread.Sleep(100);
                //}
                Log_Global($"{this.Name} 扫描范围内无光]");
                string str = $"{this.Name} 扫描范围内无光...";
                throw new Exception(str);
            }
        }

        /// <summary>
        /// 插补运动
        /// </summary>
        /// <param name="eSize"></param>
        /// <param name="radius"></param>
        /// <param name="Position"></param>
        /// <param name="Plane"></param>
        /// <returns></returns>
        public TrajResultItem Run_Involute(eRunSize_Table eSize,
                                           AxesPosition Position,
                                           PmTrajSelectPlane Plane,
                                           TrajThresholdStop thresholdStop, //阈值停止
                                           CancellationToken token)
        {
            int rtn = 0;
            double RadiusSales = 1;       //比例
            double IntervalSales = 1;
            double Rough_R = 1;
            double Rough_Inv = 1;

            double Trajspeed = 1;

            //螺旋运动
            switch (eSize)
            {
                case eRunSize_Table.Fine_Double:   //精扫
                    {
                        RadiusSales = 1;
                        IntervalSales = 1;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Involute_Interval * IntervalSales;
                        Trajspeed = TestRecipe.Fine_Trajspeed;
                    }
                    break;
            }

            return Run_Involute_Parameter(Rough_Inv, Rough_R, Rough_Inv, Trajspeed, Position, Plane, thresholdStop, token);
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
        public TrajResultItem Run_Involute_Rough_ParameterR(eRunSize_Table eSize,
                                                            double Rough_R,
                                                            AxesPosition Position,
                                                            PmTrajSelectPlane Plane,
                                                            TrajThresholdStop thresholdStop, //阈值停止
                                                            CancellationToken token)
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
                        RadiusSales = 2;
                        IntervalSales = 2;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Radius / 10;
                        Trajspeed = TestRecipe.Fine_Trajspeed;
                    }
                    break;
            }
            return Run_Involute_Parameter(Rough_Inv / 2, Rough_R, Rough_Inv, Trajspeed, Position, Plane, thresholdStop, token);
        }
        /// <summary>
        /// 运行到指定点
        /// </summary>
        /// <param name="axisList"></param>
        /// <param name="targetPoint"></param>
        /// <param name="token"></param>
        public void MoveToAxesPosition(List<Motor_LaserX_9078> axisList, AxesPosition targetPoint, CancellationToken token)
        {
            foreach (var axis in axisList)
            {
                var pos = targetPoint.ItemCollection
                    .Where(kvp => kvp.AxisNo == axis.AxisNo.ToString())
                    .Select(kvp => kvp.Position)
                    .FirstOrDefault();
                axis.MoveToV3(pos, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Low);
            }
            foreach (var axis in axisList)
            {
                axis.WaitMotionDone();
            }
            Thread.Sleep(50);
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
        public TrajResultItem Run_Involute_Parameter(
            double Rough_R_Inside,
            double Rough_R,
            double Rough_Inv,
            double Trajspeed,
            AxesPosition Position,
            PmTrajSelectPlane Plane,
            TrajThresholdStop thresholdStop, //阈值停止
            CancellationToken token)
        {
            //插补轴定义
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict = new Dictionary<PmTrajAxisType, MotorAxisBase>
            {
                { PmTrajAxisType.X_Dir, X2 },
                { PmTrajAxisType.Y_Dir, Y2 },
                { PmTrajAxisType.Z_Dir, Z2 }
            };

            Thread.Sleep(100);

            TrajResultItem result = new TrajResultItem();
            int rtn = 0;
            rtn = Parallel_2DCycleInvolute(axisDict,
                                           Position,
                                           Rough_R_Inside,
                                           Rough_R,
                                           Rough_Inv,
                                           Plane,
                                           true,
                                           Trajspeed,
                                           out result,
                                           thresholdStop,
                                           token);

            if (rtn != 0)
            {
                //异常返回;
            }

            return result;
        }
        /// <summary>
        /// 存储RawData并返回峰值点位
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool DataAnalyze(AxesPosition t_Start_Pos, TrajResultItem result,
                                bool isRough,
                                out Dictionary<AxesPosition, double> AnalyzeResult)
        {
            const double Power_Threshold = 0.9;// 0.618;
            try
            {
                var ch = this.TestRecipe.Analog_CH - 1;

                List<double> pList = new List<double>();
                List<double> xList = new List<double>();
                List<double> yList = new List<double>();
                List<double> zList = new List<double>();

                List<double> tpList = new List<double>();
                List<double> txList = new List<double>();
                List<double> tyList = new List<double>();
                List<double> tzList = new List<double>();

                double tpSum = 0;   //求和
                double txSum = 0;   //加权求和
                double tySum = 0;
                double tzSum = 0;

                foreach (var item in result.MotorPos_mm)
                {
                    if (item.Key.Name == "LNX")
                    {
                        xList = item.Value;
                    }
                    if (item.Key.Name == "LNY")
                    {
                        yList = item.Value;
                    }
                    if (item.Key.Name == "LNZ")
                    {
                        zList = item.Value;
                    }
                }

                var actlnx = X2 as Motor_LaserX_9078;
                var actlny = Y2 as Motor_LaserX_9078;
                var actlnz = Z2 as Motor_LaserX_9078;
                var actList = new List<Motor_LaserX_9078>() { actlnx, actlny, actlnz };

                //运动卡模拟量通道, 取电流
                pList = result.Current_mA[ch];
                var sense = Analog_LaserX_9078.GetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, 0);
                if (pList.Max() >= sense * 0.95)
                {
                    tpList = new List<double>();
                    txList = new List<double>();
                    tyList = new List<double>();
                    tzList = new List<double>();

                    tpSum = 0;   //求和
                    txSum = 0;   //加权求和
                    tySum = 0;
                    tzSum = 0;

                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i] >= sense * 0.95)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);

                            tpSum += pList[i];
                            txSum += xList[i] * pList[i];
                            tySum += yList[i] * pList[i];
                            tzSum += zList[i] * pList[i];
                        }
                    }

                    double x_range = txList.Max() - txList.Min();
                    double y_range = tyList.Max() - tyList.Min();
                    double z_range = tzList.Max() - tzList.Min();

                    if (x_range <= this.TestRecipe.Fine_Radius * 1.5 && y_range <= this.TestRecipe.Fine_Radius * 1.5 && z_range <= this.TestRecipe.Fine_Radius * 1.5)
                    {
                        var tPmax = new AxesPosition();
                        foreach (var axisPos in t_Start_Pos)
                        {
                            var axis = actList.FirstOrDefault(item => item.AxisNo.ToString() == axisPos.AxisNo);
                            // 填入XYZ轴对应位置
                            tPmax.ItemCollection.Add(new AxisPosition()
                            {
                                Name = axis.Name,
                                CardNo = axis.CardNo.ToString(),
                                AxisNo = axis.AxisNo.ToString(),
                                Position = axis.Name == "LNX" ? txSum / tpSum :  //txList.Average() :
                                           axis.Name == "LNY" ? tySum / tpSum :  //tyList.Average() :
                                           axis.Name == "LNZ" ? tzSum / tpSum :  //tzList.Average() :
                                           axisPos.Position
                            });
                        }

                        Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                        //PD电流
                        tmaxPoint.Add(tPmax, sense);

                        AnalyzeResult = tmaxPoint;

                        //这里超过量程了, 需要跳挡
                        Analog_LaserX_9078.SetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, ch, sense * 2);

                        //这里到达最大量程
                        if (sense >= 1.8)
                        {
                            Log_Global($"已达最大电流档位，降低Gain电流10mA");

                            double tcurrent_mA = GAIN.ReadCurrent_A() * 1000.0;
                            GAIN.AssignmentMode_Current(tcurrent_mA - 10.0, 2.5);  //20240627 更换光开关后降低耦合电流
                        }

                        return true;
                    }
                    else
                    {
                        //20230224 面积中心做返回值
                        var pmax = pList.Max();
                        var pmin = pList.Min();

                        //使用黄金分割高度
                        var threshold_power = (pmax - pmin) * Power_Threshold + pmin;

                        tpList = new List<double>();
                        txList = new List<double>();
                        tyList = new List<double>();
                        tzList = new List<double>();

                        tpSum = 0;   //求和
                        txSum = 0;   //加权求和
                        tySum = 0;
                        tzSum = 0;

                        for (int i = 0; i < pList.Count; i++)
                        {
                            if (pList[i] >= threshold_power)
                            {
                                tpList.Add(pList[i]);
                                txList.Add(xList[i]);
                                tyList.Add(yList[i]);
                                tzList.Add(zList[i]);

                                tpSum += pList[i];
                                txSum += xList[i] * pList[i];
                                tySum += yList[i] * pList[i];
                                tzSum += zList[i] * pList[i];
                            }
                        }

                        var tPmax = new AxesPosition();
                        foreach (var axisPos in t_Start_Pos)
                        {
                            var axis = actList.FirstOrDefault(item => item.AxisNo.ToString() == axisPos.AxisNo);
                            // 填入XYZ轴对应位置
                            tPmax.ItemCollection.Add(new AxisPosition()
                            {
                                Name = axis.Name,
                                CardNo = axis.CardNo.ToString(),
                                AxisNo = axis.AxisNo.ToString(),
                                Position = axis.Name == "LNX" ? txSum / tpSum :  //txList.Average() :
                                           axis.Name == "LNY" ? tySum / tpSum :  //tyList.Average() :
                                           axis.Name == "LNZ" ? tzSum / tpSum :  //tzList.Average() :
                                           axisPos.Position
                            });
                        }

                        var maxIndex = GetMax(pList);

                        Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                        //PD电流
                        tmaxPoint.Add(tPmax, pList[maxIndex]);

                        AnalyzeResult = tmaxPoint;

                        //这里超过量程了, 需要跳挡
                        Analog_LaserX_9078.SetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, ch, sense * 2);


                        //这里到达最大量程
                        if (sense >= 1.8)
                        {
                            Log_Global($"已达最大电流档位，降低Gain电流10mA");

                            double tcurrent_mA = GAIN.ReadCurrent_A() * 1000.0;
                            GAIN.AssignmentMode_Current(tcurrent_mA - 10.0, 2.5);  //20240627 更换光开关后降低耦合电流
                        }

                        return false;
                    }
                }

                //20230224 面积中心做返回值
                {
                    var pmax = pList.Max();
                    var pmin = pList.Min();

                    //使用黄金分割高度
                    var threshold_power = (pmax - pmin) * Power_Threshold + pmin;

                    tpList = new List<double>();
                    txList = new List<double>();
                    tyList = new List<double>();
                    tzList = new List<double>();

                    tpSum = 0;   //求和
                    txSum = 0;   //加权求和
                    tySum = 0;
                    tzSum = 0;

                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i] >= threshold_power)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);

                            tpSum += pList[i];
                            txSum += xList[i] * pList[i];
                            tySum += yList[i] * pList[i];
                            tzSum += zList[i] * pList[i];
                        }
                    }

                    var tPmax = new AxesPosition();
                    foreach (var axisPos in t_Start_Pos)
                    {
                        var axis = actList.FirstOrDefault(item => item.AxisNo.ToString() == axisPos.AxisNo);
                        // 填入XYZ轴对应位置
                        tPmax.ItemCollection.Add(new AxisPosition()
                        {
                            Name = axis.Name,
                            CardNo = axis.CardNo.ToString(),
                            AxisNo = axis.AxisNo.ToString(),
                            Position = axis.Name == "LNX" ? txSum / tpSum :  //txList.Average() :
                                       axis.Name == "LNY" ? tySum / tpSum :  //tyList.Average() :
                                       axis.Name == "LNZ" ? tzSum / tpSum :  //tzList.Average() :
                                       axisPos.Position
                        });
                    }

                    var maxIndex = 0;
                    if (isRough)
                    {
                        maxIndex = GetMax_Rough(pList);
                    }
                    else
                    {
                        maxIndex = GetMax(pList);
                    }

                    Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                    //PD电流
                    tmaxPoint.Add(tPmax, pList[maxIndex]);

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
        public int GetMax_Rough(List<double> pList)
        {
            try
            {
                double[] countArr = new double[pList.Count];
                for (int i = 0; i < countArr.Length; i++)
                {
                    countArr[i] = i + 1;
                }
                double[] smoothArr = ArrayMath.CalculateSmoothedNthDerivate(countArr, pList.ToArray(), 1, 3, 7);
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
                var halfHeight = (pList.Max() - pList.Min()) * 0.75 + pList.Min();
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
                        finalDict.Add(item.Key, item.Value);
                    }
                }

                if (finalDict.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return finalDict.Aggregate((m, n) => m.Value > n.Value ? m : n).Key;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// MaxList添加
        /// </summary>
        /// <param name="maxList"></param>
        /// <param name="id"></param>
        /// <param name="retPoint"></param>
        /// <returns></returns>
        private AxesPosition MaxListAdd(List<Motor_LaserX_9078> threeAxisList, Dictionary<int, PointResult> maxList, int id, Dictionary<AxesPosition, double> retPoint, bool addlist)
        {
            AxesPosition P1 = new AxesPosition();

            var maxPoint = new PointResult();
            maxPoint.ID = id;
            maxPoint.Position = new AxesPosition() { ItemCollection = retPoint.First().Key.ItemCollection };
            maxPoint.Power = retPoint.First().Value; //K_PD_mW;

            //拷贝出来坐标位置
            foreach (Motor_LaserX_9078 axis in threeAxisList)
            {
                var pos = Math.Round(retPoint.First().Key.ItemCollection.FirstOrDefault(item => item.AxisNo == axis.AxisNo.ToString()).Position, 6);

                P1.ItemCollection.Add(
                new AxisPosition()
                {
                    Name = axis.Name,
                    CardNo = axis.CardNo.ToString(),
                    AxisNo = axis.AxisNo.ToString(),
                    Position = pos
                });
            }

            if (addlist)
            {
                if (maxList.ContainsKey(id))
                {
                    maxList[id] = maxPoint;
                }
                else
                {
                    maxList.Add(id, maxPoint);
                }
            }
            //P1.ItemCollection = maxPoint2.Position.ItemCollection;
            return P1;
        }

        /// <summary>
        /// MaxList添加
        /// </summary>
        /// <param name="maxList"></param>
        /// <param name="id"></param>
        /// <param name="retPoint"></param>
        /// <returns></returns>
        private AxesPosition MaxListAdd(List<Motor_LaserX_9078> threeAxisList, Dictionary<int, PointResult> maxList, int id, AxesPosition retPoint, double power, bool addlist)
        {
            AxesPosition P1 = new AxesPosition();

            var maxPoint = new PointResult();
            maxPoint.ID = id;
            maxPoint.Position = new AxesPosition() { ItemCollection = retPoint.ItemCollection };
            maxPoint.Power = power; //K_PD_mW;

            //拷贝出来坐标位置
            foreach (Motor_LaserX_9078 axis in threeAxisList)
            {
                var pos = Math.Round(retPoint.ItemCollection.FirstOrDefault(item => item.AxisNo == axis.AxisNo.ToString()).Position, 6);

                P1.ItemCollection.Add(
                new AxisPosition()
                {
                    Name = axis.Name,
                    CardNo = axis.CardNo.ToString(),
                    AxisNo = axis.AxisNo.ToString(),
                    Position = pos
                });
            }

            if (addlist)
            {
                if (maxList.ContainsKey(id))
                {
                    maxList[id] = maxPoint;
                }
                else
                {
                    maxList.Add(id, maxPoint);
                }
            }
            //P1.ItemCollection = maxPoint2.Position.ItemCollection;
            return P1;
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
                double[] smoothArr = ArrayMath.CalculateSmoothedNthDerivate(countArr, pList.ToArray(), 1, 3, 7);
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
                var halfHeight = (pList.Max() - pList.Min()) * 0.75 + pList.Min();
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

                if (finalDict.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return finalDict.Aggregate((m, n) => m.Value > n.Value ? m : n).Key;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //返回THz
        public static double WaveLengthToFrequency(double waveLength_nm)
        {
            double waveLength = waveLength_nm / 1e9;

            const double SpeedOfLight = 299792458.0; // 光速，单位是 m/s
            double frequency = SpeedOfLight / waveLength; // 单位是 1/m

            double frequency_THz = frequency / 1e12;
            return frequency_THz;
        }
    }
}