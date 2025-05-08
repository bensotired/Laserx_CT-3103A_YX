using LX_BurnInSolution.Charts;
using LX_BurnInSolution.Utilities;
using Newtonsoft.Json.Linq;
using SolveWare_Analog;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static SolveWare_BurnInInstruments.LaserX_9078_Utilities;
using static SolveWare_TestPackage.LaserX_9078_Traj_Function;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_AA")]

    #region  轴、位置、IO、仪器
    [StaticResource(ResourceItemType.IO, "PD_3", "切换PD")]
    [StaticResource(ResourceItemType.AXIS, "LNX", "LNX")] // 耦合模块X轴
    [StaticResource(ResourceItemType.AXIS, "LNY", "LNY")] // 耦合模块Y轴
    [StaticResource(ResourceItemType.AXIS, "LNZ", "LNZ")] // 耦合模块Z轴
    [StaticResource(ResourceItemType.AXIS, "LY", "LY")]
    [StaticResource(ResourceItemType.POS, "LN_Focuser", "Focuser")]
    [StaticResource(ResourceItemType.POS, "LN_Focuser_Right", "LN_Focuser_Right")]
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
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]

    #endregion
    public class TestModule_AA : TestModuleBase
    {
        string ModuleName = "AA";

        public TestModule_AA() : base()
        {
        }

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

        #region 以Get获取资源
        MotorAxisBase LY { get { return (MotorAxisBase)this.ModuleResource["LY"]; } }
        private MotorAxisBase X2 { get { return (MotorAxisBase)this.ModuleResource["LNX"]; } }
        private MotorAxisBase Y2 { get { return (MotorAxisBase)this.ModuleResource["LNY"]; } }
        private MotorAxisBase Z2 { get { return (MotorAxisBase)this.ModuleResource["LNZ"]; } }
        AxesPosition LN_Focuser { get { return (AxesPosition)this.ModuleResource["LN_Focuser"]; } }
        AxesPosition LN_Focuser_Right { get { return (AxesPosition)this.ModuleResource["LN_Focuser_Right"]; } }
        IOBase SwitchPD { get { return (IOBase)this.ModuleResource["PD_3"]; } }
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
        private OpticalSwitch OSwitch { get { return (OpticalSwitch)this.ModuleResource["OSwitch"]; } }

        #endregion 以Get获取资源

        private TestRecipe_AA TestRecipe { get; set; }
        private RawData_AA RawData { get; set; }
        QWLT2_TestDta qWLT2_TestDta { get; set; }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_AA(); return RawData;
        }

        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_AA);
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_AA>(testRecipe);
        }
        IDeviceStreamDataBase _dutStreamData;
        string SerialNumber { get; set; }
        public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        {
            _dutStreamData = dutStreamData;
            SerialNumber = dutStreamData.SerialNumber;
            if (dutStreamData.RawDataCollecetionCount < 2)
            {
                return;
            }
            qWLT2_TestDta = new QWLT2_TestDta();
            //var dataMenu = dutStreamData.RawDataCollection[2];
            foreach (var dataMenu in dutStreamData.RawDataCollection)
            {
                if (dataMenu is IRawDataMenuCollection)
                {

                    var rawd = dataMenu as IRawDataMenuCollection;
                    var type = rawd.GetType();
                    if (type.Name == "RawDataMenu_QWLT2")
                    {
                        var props = rawd.GetType().GetProperties();
                        var broEleProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(props);
                        foreach (var bp in broEleProps)
                        {
                            if (bp.Name == "MIRROR1_mid_slope_val")
                            {
                                qWLT2_TestDta.MIRROR1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "MIRROR2_mid_slope_val")
                            {
                                qWLT2_TestDta.MIRROR2 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "LP")
                            {
                                qWLT2_TestDta.LP = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "PH_Max_Sec_1")
                            {
                                qWLT2_TestDta.PH1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "PH_Max_Sec_2")
                            {
                                qWLT2_TestDta.PH2 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "mPd1_V")
                            {
                                qWLT2_TestDta.MPD1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "mPd2_V")
                            {
                                qWLT2_TestDta.MPD2 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "Bais1_V")
                            {
                                qWLT2_TestDta.BIAS1 = (double)bp.GetValue(rawd);
                            }
                            if (bp.Name == "Bais2_V")
                            {
                                qWLT2_TestDta.BIAS2 = (double)bp.GetValue(rawd);
                            }


                        }
                        //qWLT2_TestDta.MIRROR1 = (double)broEleProps[2].GetValue(rawd);
                        //qWLT2_TestDta.MIRROR2 = (double)broEleProps[3].GetValue(rawd);
                        //qWLT2_TestDta.LP = (double)broEleProps[4].GetValue(rawd);
                        //qWLT2_TestDta.PH1 = (double)broEleProps[7].GetValue(rawd);
                        //qWLT2_TestDta.PH2 = (double)broEleProps[8].GetValue(rawd);
                    }
                }
            }

        }

        //public override bool SetupResources(DataBook<string, string> userDefineInstrumentConfig, DataBook<string, string> userDefineAxisConfig, DataBook<string, string> userDefinePositionConfig, ITestPluginResourceProvider resourceProvider)
        //{
        //    bool isbaseok = base.SetupResources(userDefineInstrumentConfig, userDefineAxisConfig, userDefinePositionConfig, resourceProvider);

        //    bool is_dual_ok = false;
        //    bool needDualResources = false;
        //    var staticResrouces = PropHelper.GetAttributeValueCollection<StaticResourceAttribute>(this.GetType());
        //    foreach (var staItem in staticResrouces)
        //    {
        //        if (staItem.ResourceType == ResourceItemType.AXIS_2 ||
        //            staItem.ResourceType == ResourceItemType.IO_2 ||
        //            staItem.ResourceType == ResourceItemType.POS_2)
        //        {
        //            needDualResources = true;
        //            break;
        //        }
        //    }
        //    if (needDualResources)
        //    {
        //        var resourceProvider_dual = resourceProvider as ITestPluginResourceProvider_Dual;
        //        foreach (var staItem in staticResrouces)
        //        {
        //            if (staItem.ResourceType != ResourceItemType.AXIS_2 ||
        //                staItem.ResourceType != ResourceItemType.IO_2 ||
        //                staItem.ResourceType != ResourceItemType.POS_2)
        //            {
        //                continue;
        //            }
        //            object staticResObj = null;
        //            switch (staItem.ResourceType)
        //            {
        //                case ResourceItemType.AXIS_2:
        //                    {
        //                        staticResObj = resourceProvider_dual.Local_Axis_ResourceProvider_2.GetAxis_Object(staItem.ResourceName);
        //                    }
        //                    break;

        //                case ResourceItemType.IO_2:
        //                    {
        //                        staticResObj = resourceProvider_dual.Local_IO_ResourceProvider_2.GetIO_Object(staItem.ResourceName);
        //                    }
        //                    break;

        //                case ResourceItemType.POS_2:
        //                    {
        //                        staticResObj = resourceProvider_dual.Local_AxesPosition_ResourceProvider_2.GetAxesPosition_Object(staItem.ResourceName);
        //                    }
        //                    break;
        //            }
        //            if (staticResObj == null)
        //            {
        //                this.Log_Global($"未找到测试模块[{this.Name}]所需资源[{staItem.ResourceType} - {staItem.ResourceName}]!");
        //                return false;
        //            }
        //            ModuleResource.Add(staItem.ResourceName, staticResObj);
        //        }
        //        is_dual_ok = true;
        //        return isbaseok && is_dual_ok;
        //    }
        //    else
        //    {
        //        return isbaseok;
        //    }
        //}

        public override void Run(CancellationToken token)
        {
            try
            {
                string path = Application.StartupPath + $@"\Data\AA\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    path = Application.StartupPath + $@"\Data\{SerialNumber}\AA";
                }

                Log_Global($"开始AA测试.");

                #region Init
                var LYPosition = LY.Get_CurUnitPos();
                if (LYPosition > 100)
                {
                    if (LN_Focuser.ItemCollection.Count == 3)//左载台耦合初始位
                    {
                        X2.MoveToV3(LN_Focuser.GetSingleItem(X2.Name).Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                        X2.WaitMotionDone();
                        Z2.MoveToV3(LN_Focuser.GetSingleItem(Z2.Name).Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                        Z2.WaitMotionDone();
                        Y2.MoveToV3(LN_Focuser.GetSingleItem(Y2.Name).Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                        Y2.WaitMotionDone();
                    }
                }
                else
                {
                    if (LN_Focuser_Right.ItemCollection.Count == 3)//右载台耦合初始位
                    {
                        X2.MoveToV3(LN_Focuser_Right.GetSingleItem(X2.Name).Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                        X2.WaitMotionDone();
                        Z2.MoveToV3(LN_Focuser_Right.GetSingleItem(Z2.Name).Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                        Z2.WaitMotionDone();
                        Y2.MoveToV3(LN_Focuser_Right.GetSingleItem(Y2.Name).Position, SolveWare_Motion.SpeedType.Auto, SolveWare_Motion.SpeedLevel.Normal);
                        Y2.WaitMotionDone();
                    }
                }

                SwitchPD.TurnOn(false);
                Merged_PXIe_4143.Reset();

                //OSwitch切换:
                {
                    var och = Convert.ToByte(this.TestRecipe.OpticalSwitchChannel);
                    if (OSwitch.SetCH(och) == false)
                    {
                        string msg = "光开关通道切换失败！";
                        this.Log_Global(msg);
                        throw new Exception(msg);
                    }
                }

                var UsedPlane = LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CW;
                string LogDataMsg = string.Empty;
                string Alignmentpath = Application.StartupPath + $@"\Data\AlignmentResult\{DateTime.Now:yyyyMMdd_HHmmss}";
                if (!string.IsNullOrEmpty(SerialNumber))
                {
                    Alignmentpath = Application.StartupPath + $@"\Data\AlignmentResult\{SerialNumber}";
                }
                if (!Directory.Exists(Alignmentpath))
                {
                    Directory.CreateDirectory(Alignmentpath);
                }
                StringBuilder strb = new StringBuilder();
                StreamWriter sw;

                //初始位
                var actlnx = X2 as Motor_LaserX_9078;
                var actlny = Y2 as Motor_LaserX_9078;
                var actlnz = Z2 as Motor_LaserX_9078;
                var ThreeAxisList = new List<Motor_LaserX_9078>() { actlnz, actlnx, actlny };

                //初始位置
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
                            Position =  actlny.Get_CurUnitPos(),// - TestRecipe.Layer_Step
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
                            Position =  actlny.Get_CurUnitPos(),// - TestRecipe.Layer_Step
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

                var LastP = new AxesPosition()
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
                            Position =  actlny.Get_CurUnitPos(),// - TestRecipe.Layer_Step
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

                var t1 = new AxesPosition()
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
                            Position =  actlny.Get_CurUnitPos(),// - TestRecipe.Layer_Step
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

                var t2 = new AxesPosition()
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
                            Position =  actlny.Get_CurUnitPos(),// - TestRecipe.Layer_Step
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

                var StartPos = new AxesPosition()
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
                            Position =  actlny.Get_CurUnitPos(),// - TestRecipe.Layer_Step
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
                this.MoveToAxesPosition(ThreeAxisList, P1, token);

                bool zeroScan = true;
                bool firstFineScan = false;

                //单向最大步数
                //var maxStep = 0;
                //if (TestRecipe.Layer_Range != 0)
                //{
                //    maxStep = Convert.ToInt32(TestRecipe.Layer_Range / TestRecipe.Layer_Step);
                //}
                //var CrossScanCount = this.TestRecipe.CrossScanCount;
                Dictionary<int, PointResult> maxList = new Dictionary<int, PointResult>();

                TrajResultItem result;
                var ch = this.TestRecipe.Analog_CH - 1;
                double PDCurr_mA;

                #endregion Init

                this.Log_Global("开始加电.");

                if(qWLT2_TestDta==null)
                {
                    qWLT2_TestDta = new QWLT2_TestDta();
                }

                //上电  上电条件从LIV移植过来
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
                    //MPD1.AssignmentMode_Voltage(qWLT2_TestDta.MPD1, 20);
                    //MPD2.AssignmentMode_Voltage(qWLT2_TestDta.MPD2, 20);
                    //BIAS1.AssignmentMode_Voltage(qWLT2_TestDta.BIAS1, 20);
                    //BIAS2.AssignmentMode_Voltage(qWLT2_TestDta.BIAS2, 20);

                    Thread.Sleep(200);
                    this.Log_Global("初始加电状态:");
                    PXISourceMeter_4143[] SourceMeterCheckDataList = new PXISourceMeter_4143[]
                    {
                        GAIN,
                        LP,
                        MIRROR1,
                        MIRROR2,

                        SOA1,
                        SOA2,

                        PH1,
                        PH2,
                        //MPD1,
                        //MPD2,
                        //BIAS1,
                        //BIAS2,
                    };


                    string name = "";
                    double curr = 0;
                    double volt = 0;

                    foreach(var item in SourceMeterCheckDataList)
                    {
                        name = item.Name.ToString();
                        curr = item.ReadCurrent_A() * 1000.0;
                        volt = item.ReadVoltage_V();
                        this.Log_Global($"{name}:Curr[{curr}mA] Volt[{volt}V]");
                    }



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



                Thread.Sleep(200);

                #region AA流程


                {

                    //此处为画10字扫描方法
                    Log_Global($"start Cross Scan");

                    #region 第0层

                    this.CheckCancellationRequested(token);

                    var t=Analog_LaserX_9078.SetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, ch, Math.Max(this.TestRecipe.InitialCurrentSense_mA, TestRecipe.PowerThreshold_mA));
                    if(t!= (int)errcodevalue.Finish)
                    {
                        Log_Global($"调档位失败");
                    }

                    //主动刷新一下
                    LaserX_9078_Utilities.P9078_MotionUpdate((X2 as Motor_LaserX_9078).CardNo);  //20241120

                    int id = 0;
                    Dictionary<AxesPosition, double> retPoint = new Dictionary<AxesPosition, double>();

                    TrajResultItem result_Big = new TrajResultItem();
                    TrajResultItem result_Small = new TrajResultItem();


                    //使能粗扫
                    // if (maxStep != 0 || (maxStep == 0 && this.TestRecipe.Rough_Enable))
                    {
                        //粗扫变更
                        PmTrajSelectPlane[] lst_UsedPlane = new PmTrajSelectPlane[]
                        {
                            LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CW,
                            LaserX_9078_Utilities.PmTrajSelectPlane.XZ_CW_LP
                        };


                        //阈值停止
                        TrajThresholdStop thresholdStop = new TrajThresholdStop()
                        {
                            En = true,
                            ThCurrent_mA = new Dictionary<int, double>(),
                            ThVoltage_mV = new Dictionary<int, double>()
                        };

                        //增加阈值
                        thresholdStop.ThCurrent_mA.Add(ch, TestRecipe.PowerThreshold_mA);

                        //20241122 优化旋转逻辑
                        var Big_UsedPlane = lst_UsedPlane[0];
                        //优化跳跃逻辑
                        int PlaneJump = 0;

                        for (int i = 0; ; i++)
                        {
                            this.CheckCancellationRequested(token);

                            Log_Global($"开始第{i+1}次粗耦合[{id}]");

                            result_Big = Run_Involute(eRunSize_Table.Rough, P1, Big_UsedPlane, thresholdStop, token);


                            this.CheckCancellationRequested(token);

                            if (!DataAnalyze(result_Big, true, out retPoint))
                            {
                                this.CheckCancellationRequested(token);

                                LogDataMsg = Alignmentpath + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_Plane[{PlaneJump}]粗耦合超量程.csv";
                                this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_Big);

                                var sense = Analog_LaserX_9078.GetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, ch); //当前挡位
                                Analog_LaserX_9078.SetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, ch, sense * 2);

                                //P1.ItemCollection = retPoint.First().Key.ItemCollection;

                                ////寻找最小的半径
                                //double MinR = DataAnalyze_MinR(result_Big);

                                //result_Big = Run_Involute_Rough_ParameterR(eRunSize_Table.Rough, MinR, P1, Big_UsedPlane, thresholdStop, token);

                                this.CheckCancellationRequested(token);
                            }
                            else
                            {
                                LogDataMsg = Alignmentpath + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_Plane[{PlaneJump}]粗耦合.csv";
                                this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_Big);
                            }


                            if (this.CheckThreshold_mW(retPoint) == true)
                                break;

                            //给3次机会
                            if (i>=20)
                            {
                                Log_Global($"{this.Name} 扫描范围内无光");
                                string str = $"{this.Name} 扫描范围内无光...";
                                throw new Exception(str);
                            }
                            else
                            {
                                if (PlaneJump == 0) PlaneJump = 1;  //初始推进

                                //while (true)
                                //{ Thread.Sleep(100); }
                                Log_Global($"{this.Name} 扫描范围内无光， 推进到[{PlaneJump}]步进平面");
                                //推进一个平面
                                P1.ItemCollection.FirstOrDefault(axis => axis.Name == "LNY").Position = 
                                    StartPos.ItemCollection.FirstOrDefault(axis => axis.Name == "LNY").Position +  PlaneJump * TestRecipe.Layer_Step;

                                //Jump逻辑
                                {
                                    if (PlaneJump>0)
                                    {
                                        PlaneJump *= -1;
                                    }
                                    else
                                    {
                                        PlaneJump = Math.Abs(PlaneJump) + 1;
                                    }
                                }

                                //变更搜索模式
                                if (Big_UsedPlane == lst_UsedPlane[0]) Big_UsedPlane = lst_UsedPlane[1];
                                else Big_UsedPlane = lst_UsedPlane[0];

                            }

                            Thread.Sleep(1000);
                        }

                        P1 = this.FindP1(ThreeAxisList, maxList, id, retPoint, false);

                        this.CheckCancellationRequested(token);

                        ////double精扫
                        //id++;
                        //Log_Global($"开始DoubleSize粗耦合[{id}]");
                        //result_Small = Run_Involute(eRunSize_Table.Fine_Double_line, P1, UsedPlane, thresholdStop, token);

                        //this.CheckCancellationRequested(token);

                        //while (!DataAnalyze(result_Small, false, out retPoint))
                        //{
                        //    this.CheckCancellationRequested(token);

                        //    LogDataMsg = Alignmentpath + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_DoubleSize精耦合超量程.csv";
                        //    this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_Small);

                        //    result_Small = Run_Involute(eRunSize_Table.Fine_Double_line, P1, UsedPlane, thresholdStop, token);

                        //    this.CheckCancellationRequested(token);
                        //}
                        //LogDataMsg = Alignmentpath + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_DoubleSize精耦合.csv";
                        //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_Small);

                        ////this.JudgeThreshold_mW(retPoint);
                        ////如果新值更大就替换maxPoint键值
                        //P1 = MaxListAdd(ThreeAxisList, maxList, id, retPoint, false);
                    }

                    //精扫
                    id++;
                    Log_Global($"开始精耦合[{id}]");

                    this.Updatet1t2Pos(ThreeAxisList, P1, t1, t2);

                    this.Updatet1t2Pos(ThreeAxisList, P1, StartPos, t2);

                    double size = GetSerachSize(eRunSize_Table.Fine);  //左右两边的空间

                    StartPos.ItemCollection.FirstOrDefault(item => item.Name == "LNX").Position -= size * 1;
                    StartPos.ItemCollection.FirstOrDefault(item => item.Name == "LNZ").Position -= size * 1;
                    //StartPos[MonIC.GetAxis(Axis.X2)] -= 0.3;
                    //StartPos[MonIC.GetAxis(Axis.Z2)] -= 0.3;

                    //此处需要运行到外部初始位置
                    for (int i = 0; i < this.TestRecipe.CrossScanCount; i++)
                    {
                        P1 = HorizontalLine(Alignmentpath, i, id, size*3,
                                          ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine,
                                          out sw, out strb, out LogDataMsg, out result, out retPoint, token);

                        P1 = VerticalLine(Alignmentpath, i, id, size*3,
                                          ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine,
                                          out sw, out strb, out LogDataMsg, out result, out retPoint, token);
                    }
                    //增加点
                    P1 = MaxListAdd(ThreeAxisList, maxList, id, retPoint, true);
                    firstFineScan = true;

                    #endregion 第0层


                    //上一次的位置
                    double LastLNX = P1.ItemCollection.FirstOrDefault(item => item.Name == "LNX").Position;
                    double LastLNY = P1.ItemCollection.FirstOrDefault(item => item.Name == "LNY").Position;
                    double LastLNZ = P1.ItemCollection.FirstOrDefault(item => item.Name == "LNZ").Position;

                    //本次的位置
                    double tLNX = LastLNX;
                    double tLNY = LastLNY;
                    double tLNZ = LastLNZ;

                    LastP.ItemCollection.FirstOrDefault(item => item.Name == "LNX").Position = LastLNX;
                    LastP.ItemCollection.FirstOrDefault(item => item.Name == "LNY").Position = LastLNY;
                    LastP.ItemCollection.FirstOrDefault(item => item.Name == "LNZ").Position = LastLNZ;

                    //三维扫
                    Log_Global($"开始三维搜索[{id+1}]");

                    double pd_Max = 0;

                    //最大层
                    int maxStep = 40;

                    //持续记录最后的2组数据
                    TrajResultItem result_H = new TrajResultItem();
                    TrajResultItem result_V = new TrajResultItem();
                    for (int iSerach = 0; iSerach <= maxStep; iSerach++)
                    {

                        this.CheckCancellationRequested(token);

                        if (iSerach == maxStep)
                        {
                            throw new OperationCanceledException($"搜索[{iSerach}]次仍未找到最大光位置]");
                        }

                        id++;

                        //
                        double LastPX = LastP.ItemCollection.FirstOrDefault(item => item.Name == "LNX").Position;
                        double LastPY = LastP.ItemCollection.FirstOrDefault(item => item.Name == "LNY").Position;
                        double LastPZ = LastP.ItemCollection.FirstOrDefault(item => item.Name == "LNZ").Position;

                        double distance = Math.Sqrt(Math.Pow(LastPX - tLNX, 2) + Math.Pow(LastPY - tLNY, 2) + Math.Pow(LastPZ - tLNZ, 2));
                        this.Log_Global($"耦合点空间距离[{distance}]");


                        if (true)//distance < 0.004)
                        {
                            Thread.Sleep(100);
                            //距离很近扫Y
                            P1 = DepthLine(out LogDataMsg, Alignmentpath, out strb, ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine_Half, out sw, out result, id, out retPoint, TestRecipe.Layer_Step * 4, iSerach, token);
                        }
                        else
                        {
                            //距离远了扫3D
                            P1 = Depth_3DLine(out LogDataMsg, Alignmentpath, out strb, ThreeAxisList, P1,LastP, t1, t2, StartPos, eRunSize_Table.Fine_Half, out sw, out result, id, out retPoint, TestRecipe.Layer_Step * 4, iSerach, token);

                        }

                        //Y运动完后， 一定要等一下， 等待机械稳定
                        //Thread.Sleep(300); 

                        LastP.ItemCollection.FirstOrDefault(item => item.Name == "LNX").Position = LastLNX;
                        LastP.ItemCollection.FirstOrDefault(item => item.Name == "LNY").Position = LastLNY;
                        LastP.ItemCollection.FirstOrDefault(item => item.Name == "LNZ").Position = LastLNZ;


                        //后续扫空间直线

                        for (int j = 0; j < this.TestRecipe.CrossScanCount; j++)
                        {
                            Thread.Sleep(100);
                            P1 = HorizontalLine(Alignmentpath, iSerach, id, size,
                                              ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine_Half,
                                              out sw, out strb, out LogDataMsg, out result_H, out retPoint, token);
                            Thread.Sleep(100);
                            P1 = VerticalLine(Alignmentpath, iSerach, id, size,
                                              ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine_Half,
                                              out sw, out strb, out LogDataMsg, out result_V, out retPoint, token);

                        }

                        //增加点
                        P1 = MaxListAdd(ThreeAxisList, maxList, id, retPoint, true);

                        //判定搜索位置
                        tLNX = P1.ItemCollection.FirstOrDefault(item => item.Name == "LNX").Position;
                        tLNY = P1.ItemCollection.FirstOrDefault(item => item.Name == "LNY").Position;
                        tLNZ = P1.ItemCollection.FirstOrDefault(item => item.Name == "LNZ").Position;

                        distance = Math.Sqrt(Math.Pow(LastLNX - tLNX, 2) + Math.Pow(LastLNY - tLNY, 2) + Math.Pow(LastLNZ - tLNZ, 2));

                        //至少迭代5次
                        if (iSerach >=2 && distance < 0.004) //4um以内
                        {
                            break;
                        }

                        LastLNX = tLNX;
                        LastLNY = tLNY;
                        LastLNZ = tLNZ;



                    }

                    {
                        //20241121绘制耦合图 result_Big / result_Small  result_H  result_V

                        using (Coupling_Trajectory ctr = new Coupling_Trajectory())
                        {
                            str2DCouplingData d1 = new str2DCouplingData()
                            {
                                Name = "粗耦合",
                                X_Pos_mm = result_Big.MotorPos_mm[X2 as Motor_LaserX_9078].ToArray(),
                                Y_Pos_mm = result_Big.MotorPos_mm[Z2 as Motor_LaserX_9078].ToArray(),
                                Power = result_Big.Current_mA[ch].ToArray(),

                            };

                            //str2DCouplingData d2 = new str2DCouplingData()
                            //{
                            //    Name = "Double耦合",
                            //    X_Pos_mm = result_Small.MotorPos_mm[X2 as Motor_LaserX_9078].ToArray(),
                            //    Y_Pos_mm = result_Small.MotorPos_mm[Z2 as Motor_LaserX_9078].ToArray(),
                            //    Power = result_Small.Current_mA[ch].ToArray(),
                            //};

                            str2DCouplingData d3 = new str2DCouplingData()
                            {
                                Name = "H方向",
                                X_Pos_mm = result_H.MotorPos_mm[X2 as Motor_LaserX_9078].ToArray(),
                                Y_Pos_mm = result_H.MotorPos_mm[Z2 as Motor_LaserX_9078].ToArray(),
                                Power = result_H.Current_mA[ch].ToArray(),
                            };

                            str2DCouplingData d4 = new str2DCouplingData()
                            {
                                Name = "V方向",
                                X_Pos_mm = result_V.MotorPos_mm[X2 as Motor_LaserX_9078].ToArray(),
                                Y_Pos_mm = result_V.MotorPos_mm[Z2 as Motor_LaserX_9078].ToArray(),
                                Power = result_V.Current_mA[ch].ToArray(),
                            };

                            //==========================================


                            List<str2DCouplingData> tData = new List<str2DCouplingData>();
                            tData.Add(d1);

                            ctr.SetData("Big", tData.ToArray());

                            string imagePath = Path.Combine(path, $@"..\AA_0_{SerialNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                            imagePath = Path.GetFullPath(imagePath);

                            ctr.SavePictrue(imagePath);

                            //==========================================


                            tData = new List<str2DCouplingData>();
                            tData.Add(d3);
                            tData.Add(d4);

                            ctr.SetData("Cross", tData.ToArray());

                            imagePath = Path.Combine(path, $@"..\AA_1_{SerialNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                            imagePath = Path.GetFullPath(imagePath);

                            ctr.SavePictrue(imagePath);

                            //==========================================


                            //tData = new List<str2DCouplingData>();
                            //tData.Add(d2);
                            //tData.Add(d3);
                            //tData.Add(d4);

                            //ctr.SetData("Small and Cross", tData.ToArray());

                            //imagePath = Path.Combine(path, $@"..\AA_2_{SerialNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                            //imagePath = Path.GetFullPath(imagePath);

                            //ctr.SavePictrue(imagePath);

                        }


                    }

                    {
                        //运动到目标位置
                        var MaxPower_id = maxList.OrderByDescending(item => item.Value.Power).First().Value.ID;
                        P1.ItemCollection = maxList[MaxPower_id].Position.ItemCollection;

                        //运行到P1点
                        this.MoveToAxesPosition(ThreeAxisList, P1, token);
                        Thread.Sleep(300);

                        //得到当前电流
                        pd_Max = Analog_LaserX_9078.GetCurrent_mA(actlny as Motor_LaserX_9078, ch);

                        Log_Global($"精耦合到的最大光电流（模拟量值）为[{pd_Max}_mA]");
                    }

                    //用2612进行耦合电流
                    {
                        this.Log_Global($"恢复Gain电流,进行光电流耦合");
                        SwitchPD.TurnOn(true);
                        GAIN.AssignmentMode_Current(qWLT2_TestDta.GAIN, 2.5);

                        #region PD回读

                        double pdSenseCurrentRange_mA = 10;// Math.Round(pd_Max * 5, 6);

                        PD.SetupAndEnableSourceOutput_SinglePoint_Voltage_V(0, pdSenseCurrentRange_mA);

                        Thread.Sleep(400);

                        pd_Max = PD.ReadCurrent_A() * 1000.0;

                        //K2602.SetIsOutputOn(Keithley2602BChannel.CHB, false);
                        #endregion PD回读


                        Log_Global($"源表读取当前光电流为[{pd_Max}]mA");


                    }


                    //蠕动扫描
                    id = 100;

                    //20240814 在层范围内点动搜索
                    if (TestRecipe.Creep_Step_um > 0)
                    {
                        double xSize = size;
                        double zSize = size;
                        double ySize = TestRecipe.Layer_Step * 2;


                        double current_PD_max = pd_Max;
                        double Creep_step = TestRecipe.Creep_Step_um / 1000;//蠕动步进

                        //最大次数
                        int xserachcount = (int)(xSize / Creep_step);
                        int yserachcount = (int)(ySize / Creep_step);
                        int zserachcount = (int)(zSize / Creep_step);

                        int xyzserachcount = xserachcount;


                        Motor_LaserX_9078 ln_axis = actlnx;
                        int SerachDir = 1;
                        bool UpdatePosition;
                        int GetdataDelay_ms = (int)TestRecipe.CreepDelay_ms;

                        double axisspeed = TestRecipe.Rough_Trajspeed * 0.5;




                        int serachPointMax = 1;//需要2个点进行判断是否找到最大
                        int serachPointCount = 0;//当前搜索失败点
                        maxStep = 3; //用多次找回

                        int SerachStep = 0;

                        //3方向搜索
                        for (int iSerach = 0; iSerach <= maxStep;)
                        {

                            switch (SerachStep)
                            {
                                case 0:  // X+
                                    Log_Global($"开始蠕动扫描[{id}]");
                                    if (iSerach == maxStep - 1) //只离焦一次
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
                                    xyzserachcount = xserachcount;
                                    break;

                                case 1:  // Z+
                                    ln_axis = actlnz;
                                    xyzserachcount = zserachcount;
                                    break;

                                case 2:  // Y+
                                    ln_axis = actlny;
                                    xyzserachcount = yserachcount;
                                    break;

                                case 3:
                                    //加入数组
                                    MaxListAdd(ThreeAxisList, maxList, id, P1, pd_Max, true);

                                    Log_Global($"蠕动扫描[{id}] 最大光电流为[{pd_Max}_mA]");

                                    id++;
                                    iSerach++;
                                    SerachStep = 0;
                                    continue;
                                    break;

                            }

                            SerachStep++;

                            //最后2次搜索时候, 只判断一个点
                            if (iSerach >= maxStep - 1)
                            {
                                serachPointMax = 1;

                                if(ln_axis == actlny)
                                {
                                    continue;
                                }
                            }


                            #region 进行搜索

                            //+方向
                            SerachDir = 1;
                            serachPointCount = 1;
                            Log_Global($"DEBUG 蠕动扫描中[{ln_axis.Name}] 方向[{SerachDir}]");
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
                                Log_Global($"DEBUG 蠕动扫描中[{ln_axis.Name}] 方向[{SerachDir}]");
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




                        //Log_Global($"蠕动最大位置[{id}] 当前光电流为[{pd_Max}_mA]");

                        //需要退后离开最大位置
                        //actlny.MoveToV3(actlny.Get_CurUnitPos() - 0.001, axisspeed);
                        //actlny.WaitMotionDone();

                    }


                   
                    this.Log_Global("耦合完成加电状态:");
                    PXISourceMeter_4143[] SourceMeterCheckDataList = new PXISourceMeter_4143[]
                    {
                        GAIN,
                        LP,
                        MIRROR1,
                        MIRROR2,

                        SOA1,
                        SOA2,

                        PH1,
                        PH2,
                        //MPD1,
                        //MPD2,
                        //BIAS1,
                        //BIAS2,
                    };


                    string name = "";
                    double curr = 0;
                    double volt = 0;

                    foreach (var item in SourceMeterCheckDataList)
                    {
                        name = item.Name.ToString();
                        curr = item.ReadCurrent_A() * 1000.0;
                        volt = item.ReadVoltage_V();
                        this.Log_Global($"{name}:Curr[{curr}mA] Volt[{volt}V]");
                    }

                    pd_Max = CreepGetPDCurrent_mA(actlnx, ch);

                    Log_Global($"耦合到的最大光电流（模拟量值）为[{pd_Max}_mA]");



#if false
                    if (maxList.Count > 1)
                    {
                        var lastMaxPoint = maxList.OrderByDescending(item => item.Value.Power).First();
                        if (maxList.Max(n => n.Value.Power) != lastMaxPoint.Value.Power)
                        {
                            string str = $"{this.Name} 记录最大值点与储存最大值点不符.";
                            throw new Exception(str);
                        }
                        else
                        {
                        }

                        P1.ItemCollection = lastMaxPoint.Value.Position.ItemCollection;

                        //三维扫
                        for (int i = 0; i < 2; i++)
                        {

                            //扫Y
                            Thread.Sleep(100);
                            P1 = DepthLine(out LogDataMsg, path, out strb, ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine_Half, out sw, out result, id, out retPoint, TestRecipe.Layer_Step*2, i, token);

                            for (int j = 0; j < CrossScanCount; j++)
                            {
                                Thread.Sleep(100);
                                P1 = HorizontalLine(out LogDataMsg, path, out strb, ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine_Half, out sw, out result, id, out retPoint, size, i, token);

                                Thread.Sleep(100);
                                P1 = VerticalLine(path, i, id, size,
                                                  ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine_Half,
                                                  out sw, out strb, out LogDataMsg, out result, out retPoint, token);

                            }
                        }

                        MaxListAdd(maxList, 100, retPoint, true);
                        this.MoveToAxesPosition(ThreeAxisList, P1, token);

                        while(true)
                        {
                            Thread.Sleep(100);
                            double curr_mA2 = Analog_LaserX_9078.GetCurrent_mA(X2 as Motor_LaserX_9078, 0);
                        }
#region 渐开线最大位置耦合
                        //RawData.LastScanSlot = lastMaxPoint.Key;
                        Log_Global($"开始渐开线最大值耦合");
                        var finalresult = Run_Involute(eRunSize_Table.Fine_Involute_line, P1, UsedPlane, token);

                        this.CheckCancellationRequested(token);
                        Dictionary<AxesPosition, double> finalPoint = new Dictionary<AxesPosition, double>();

                        DataAnalyze(finalresult, false, out finalPoint);

                        LogDataMsg = path + $@"\渐开线最大值耦合_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                        this.WriteCSCVFile(LogDataMsg, out strb, out sw, finalresult);

                        P1.ItemCollection = finalPoint.OrderByDescending(item => item.Value).First().Key.ItemCollection;
                        Pd_Max = Math.Round(finalPoint.Max((kvp => kvp.Value)), 6);
                        //P1 = finalPoint.OrderByDescending(item => item.Value).First().Key.ToDictionary(item => item.Key, item => item.Value);
                        //double Pd_Max = Math.Round(lastMaxPoint.Value.Power, 6);
                        RawData.Pmax_mA = Pd_Max;
                        Log_Global($"耦合到的最大光电流（模拟量值）为[{Pd_Max}_mA]");
                        this.CheckCancellationRequested(token);

#endregion 渐开线最大位置耦合


                        double Pd_tMax = 0;
                        //平面扫
                        for (int i = 0; i < 2; i++)
                        {

                            Thread.Sleep(100);
                            P1 = HorizontalLine(out LogDataMsg, path, out strb, ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine_Half, out sw, out result, id, out retPoint, size, i, token);

                            DataAnalyze(result, false, out finalPoint);

                            LogDataMsg = path + $@"\渐开线后H精扫_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                            this.WriteCSCVFile(LogDataMsg, out strb, out sw, result);

                            Pd_tMax = Math.Round(finalPoint.Max((kvp => kvp.Value)), 6);

                            P1.ItemCollection = finalPoint.OrderByDescending(item => item.Value).First().Key.ItemCollection;

                            if (Pd_tMax >= Pd_Max) break;

                            Thread.Sleep(100);
                            P1 = VerticalLine(path, i, id, size,
                                              ThreeAxisList, P1, t1, t2, StartPos, eRunSize_Table.Fine_Half,
                                              out sw, out strb, out LogDataMsg, out result, out retPoint, token);


                            DataAnalyze(result, false, out finalPoint);

                            LogDataMsg = path + $@"\渐开线后V精扫_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                            this.WriteCSCVFile(LogDataMsg, out strb, out sw, result);

                            Pd_tMax = Math.Round(finalPoint.Max((kvp => kvp.Value)), 6);

                            P1.ItemCollection = finalPoint.OrderByDescending(item => item.Value).First().Key.ItemCollection;

                            if (Pd_tMax >= Pd_Max) break;

                        }

                        //运行到P1点
                        this.MoveToAxesPosition(ThreeAxisList, P1, token);

                        //得到当前电流
                        double curr_mA = Analog_LaserX_9078.GetCurrent_mA(X2 as Motor_LaserX_9078, 0);



                    }
                    else
                    {
                        Log_Global($"仅有一个平面, 跳过渐开线最大值耦合步骤");
                    }
#endif

                }

#region  耦合面数据存储到文件
                string strmaxlist = "";
                {
                    string pos = "";
                    foreach (var item2 in maxList.First().Value.Position)
                    {
                        pos += $"{item2.Name},";
                    }
                    strmaxlist += $"id,{pos}PDCurrent_mA\r\n";
                }
                foreach (var item in maxList)
                {
                    string pos = "";
                    foreach (var item2 in item.Value.Position)
                    {
                        pos += $"{item2.Position},";
                    }
                    strmaxlist += $"{item.Key},{pos}{item.Value.Power}\r\n";
                }

                LogDataMsg = Alignmentpath + $@"\耦合面电流_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                sw = new StreamWriter(LogDataMsg);
                sw.Write(strmaxlist);
                sw.Close();
#endregion

                //运行到P1点
                //this.MoveToAxesPosition(ThreeAxisList, P1, token);
                List<double> Max_TempPara = new List<double>();
                this.PosP1log(ThreeAxisList, P1, out Max_TempPara);
                //写数据进去
                if (Max_TempPara.Count == 3) //暂时想到这样固定的写
                {
                    RawData.Z_Pos_Pmax_mm = Math.Round(Max_TempPara[0], 5);
                    RawData.X_Pos_Pmax_mm = Math.Round(Max_TempPara[1], 5);
                    RawData.Y_Pos_Pmax_mm = Math.Round(Max_TempPara[2], 5);
                }

#endregion AA流程
                //this.Log_Global("耦合结果:");
                //this.Log_Global(strmaxlist);
                Log_Global($"AA测试完成.");
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    throw ex;
                }
                else if (ex is OutOfMemoryException)
                {
                    var currentProcess = Process.GetCurrentProcess();
                    var msg = $"Out of Memory Exception! \r\n" +
                              $"Private Memory Size: {currentProcess.PrivateMemorySize64} \r\n" +
                              $"Virtual Memory Size: {currentProcess.VirtualMemorySize64} \r\n" +
                              $"Working Set: {currentProcess.WorkingSet64} \r\n";
                    this._core.ReportException(msg, ErrorCodes.Module_FF_Failed, ex);
                    throw ex;
                }
                else
                {
                    this._core.ReportException("AA测试流程出现异常", ErrorCodes.Module_FF_Failed, ex);
                    throw ex;
                }
            }
            finally
            {
                Merged_PXIe_4143.Reset();
            }
        }

        #region Tools

        private double CreepGetPDCurrent_mA(Motor_LaserX_9078 Axis, int index)
        {
            List<Double> curr = new List<double>();
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(50);
                var val = PD.ReadCurrent_A();
                val *= 1000;
                curr.Add(val);
            }


            Log_Global($"Debug 光电流为[{curr.Average()}]mA");

            return curr.Average();

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

        private double pmax = 0.0;

        //private AxesPosition t_Start_Pos;
        private AxesPosition t_Start_Pos;

        public enum eRunSize_Table
        {
            Rough,      //粗扫
            Rough_Half,

            Fine_Double, //精扫
            Fine,
            Fine_Half,

            Fine_Double_line, //十字精扫,用于提升速度
            Fine_Involute_line,  //用于十字最后的渐开线扫描
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
            this.JudgeThreshold_mW(retPoint);
            return MaxListAdd(threeAxisList, maxList, id, retPoint, addlist);
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
        /// 最后的P1点写log
        /// </summary>
        /// <param name="axisList"></param>
        /// <param name="targetPoint"></param>
        /// <param name="Max_TempPara"></param>
        public void PosP1log(List<Motor_LaserX_9078> axisList, AxesPosition targetPoint, out List<double> Max_TempPara)
        {
            Max_TempPara = new List<double>();
            string AxisAndPos = string.Empty;
            foreach (var axis in axisList)
            {
                var pos = Math.Round(targetPoint.ItemCollection.Where(kvp => kvp.AxisNo == axis.AxisNo.ToString()).Select(kvp => kvp.Position).FirstOrDefault(), 6);
                AxisAndPos += $"[{axis.Name}:{pos}mm]";
                Max_TempPara.Add(pos);
            }
            Log_Global($"产品耦合后的P1点位置为_{AxisAndPos}");
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

        private AxesPosition VerticalLine(
            string path,
            int i,
            int id,
            double size,
            List<Motor_LaserX_9078> ThreeAxisList,
            AxesPosition P1,
            AxesPosition t1,
            AxesPosition t2,
            AxesPosition StartPos,
            eRunSize_Table eSize,
            out StreamWriter sw,
            out StringBuilder strb,
            out string LogDataMsg,
            out TrajResultItem result,
            out Dictionary<AxesPosition, double> retPoint,
            CancellationToken token)
        {
            //运动到初始点
            this.MoveToAxesPosition(ThreeAxisList, StartPos, token);
            //======================垂直划线=================
            this.Updatet1t2Pos(ThreeAxisList, P1, t1, t2);

            var actlnz = Z2 as Motor_LaserX_9078;
            t1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnz.AxisNo.ToString()).Position -= size;
            t2.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnz.AxisNo.ToString()).Position += size;

            //result = Run_Line(eRunSize_Table.Fine, t1, t2, token);

            Thread.Sleep(50);
            TrajResultItem result_1to2 = Run_Line(eSize, t1, t2, token);
            Thread.Sleep(50);
            TrajResultItem result_2to1 = Run_Line(eSize, t2, t1, token);

            result = MixResult(result_1to2, result_2to1, actlnz, 0);

            this.CheckCancellationRequested(token);

            while (!DataAnalyze(result, false, out retPoint))
            {
                this.CheckCancellationRequested(token);
                LogDataMsg = path + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_精耦合超量程_{i}_Z.csv";
                this.WriteCSCVFile(LogDataMsg, out strb, out sw, result);
                //LogDataMsg = path + $@"\精耦合超量程_{id}_Z1to2_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_1to2);
                //LogDataMsg = path + $@"\精耦合超量程_{id}_Z2to1_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_2to1);

                //运动到初始点
                this.MoveToAxesPosition(ThreeAxisList, StartPos, token);
                //result = Run_Line(eRunSize_Table.Fine, t1, t2, token);
                Thread.Sleep(50);
                result_1to2 = Run_Line(eSize, t1, t2, token);
                Thread.Sleep(50);
                result_2to1 = Run_Line(eSize, t2, t1, token);

                result = MixResult(result_1to2, result_2to1, actlnz, 0);
                this.CheckCancellationRequested(token);
            }
            LogDataMsg = path + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_精耦合_{i}_Z.csv";
            this.WriteCSCVFile(LogDataMsg, out strb, out sw, result);

            //LogDataMsg = path + $@"\精耦合_{id}_Z1to2_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_1to2);
            //LogDataMsg = path + $@"\精耦合_{id}_Z2to1_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_2to1);
            //this.JudgeThreshold_mW(retPoint);
            P1 = retPoint.First().Key;  //更新最佳位置
            return P1;
        }

        private AxesPosition HorizontalLine(
            string path,
            int i,
            int id,
            double size,
            List<Motor_LaserX_9078> ThreeAxisList,
            AxesPosition P1,
            AxesPosition t1,
            AxesPosition t2,
            AxesPosition StartPos,
            eRunSize_Table eSize,
            out StreamWriter sw,
            out StringBuilder strb,
            out string LogDataMsg,
            out TrajResultItem result,
            out Dictionary<AxesPosition, double> retPoint,
            CancellationToken token)
        {
            //运动到初始点
            this.MoveToAxesPosition(ThreeAxisList, StartPos, token);
            //======================水平划线=================
            this.Updatet1t2Pos(ThreeAxisList, P1, t1, t2);

            var actlnx = X2 as Motor_LaserX_9078;
            t1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnx.AxisNo.ToString()).Position -= size;
            t2.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnx.AxisNo.ToString()).Position += size;

            //result = Run_Line(eRunSize_Table.Fine, t1, t2, token);

            Thread.Sleep(50);
            TrajResultItem result_1to2 = Run_Line(eSize, t1, t2, token);
            Thread.Sleep(50);
            TrajResultItem result_2to1 = Run_Line(eSize, t2, t1, token);

            result = MixResult(result_1to2, result_2to1, actlnx, 0);

            this.CheckCancellationRequested(token);

            while (!DataAnalyze(result, false, out retPoint))
            {
                this.CheckCancellationRequested(token);
                LogDataMsg = path + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_精耦合超量程_{i}_X1to2.csv";
                this.WriteCSCVFile(LogDataMsg, out strb, out sw, result);

                //LogDataMsg = path + $@"\精耦合超量程_{id}_X1to2_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_1to2);
                //LogDataMsg = path + $@"\精耦合超量程_{id}_X2to1_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_2to1);

                //运动到初始点
                this.MoveToAxesPosition(ThreeAxisList, StartPos, token);

                //result = Run_Line(eRunSize_Table.Fine, t1, t2, token);
                Thread.Sleep(50);
                result_1to2 = Run_Line(eSize, t1, t2, token);
                Thread.Sleep(50);
                result_2to1 = Run_Line(eSize, t2, t1, token);

                result = MixResult(result_1to2, result_2to1, actlnx, 0);
                this.CheckCancellationRequested(token);
            }

            LogDataMsg = path + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_精耦合_{i}_X.csv";
            this.WriteCSCVFile(LogDataMsg, out strb, out sw, result);

            //LogDataMsg = path + $@"\精耦合_{id}_X1to2_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_1to2);
            //LogDataMsg = path + $@"\精耦合_{id}_X2to1_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_2to1);
            this.CheckCancellationRequested(token);


            P1 = retPoint.First().Key;  //更新最佳位置
            return P1;
        }

        private TrajResultItem MixResult(TrajResultItem result_P1_P2, TrajResultItem result_P2_P1, Motor_LaserX_9078 ffa, int AnalogChannel)
        {
            //P1->P2
            List<double> pList_P1_P2 = result_P1_P2.Current_mA[AnalogChannel];//运动卡模拟量通道
            List<double> thetaList_P1_P2 = new List<double>();
            thetaList_P1_P2.AddRange(result_P1_P2.MotorPos_mm[ffa]);

            //移动平均法平滑
            List<double> smoothList_P1_P2 = new List<double>();
            smoothList_P1_P2.AddRange(ArrayMath.CalculateMovingAverage(pList_P1_P2.ToArray(), 11));

            //P2->P1
            List<double> pList_P2_P1 = result_P2_P1.Current_mA[AnalogChannel];//运动卡模拟量通道
            List<double> thetaList_P2_P1 = new List<double>();
            thetaList_P2_P1.AddRange(result_P2_P1.MotorPos_mm[ffa]);
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

                //使用黄金分割高度
                double Th_P1_P2 = (smoothList_P1_P2.Max() - smoothList_P1_P2.Min()) * 0.818 + smoothList_P1_P2.Min();
                double Th_P2_P1 = (smoothList_P2_P1.Max() - smoothList_P2_P1.Min()) * 0.818 + smoothList_P2_P1.Min();

                //计算质量中心
                double[] lstdiff1 = new double[PCnt];
                double[] lstdiff2 = new double[PCnt];

                double p1Sum = 0;   //求和
                double p2Sum = 0;   //求和
                double tx1Sum = 0;   //加权求和
                double tx2Sum = 0;   //加权求和

                for (int i = 0; i < PCnt; i++)
                {
                    if (smoothList_P1_P2[i] > Th_P1_P2)
                    {
                        p1Sum += smoothList_P1_P2[i];
                        tx1Sum += i * smoothList_P1_P2[i];
                    }

                    if (smoothList_P2_P1[i] > Th_P2_P1)
                    {
                        p2Sum += smoothList_P2_P1[i];
                        tx2Sum += i * smoothList_P2_P1[i];
                    }
                }

                //总偏移量
                double P1Centerpos = tx1Sum / p1Sum;
                double P2Centerpos = tx2Sum / p2Sum;
                int OffsetCount = (int)(Math.Abs(P1Centerpos - P2Centerpos) / 2);

                //OffsetCount 为0的问题
                if (OffsetCount <= 0) OffsetCount = 1;

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
                //double[] ListDiff = new double[2];
                //for (int i = OffsetCount; i < PCnt - OffsetCount; i++)
                //{
                //    ListDiff[0] += Math.Abs(smoothList_P1_P2_L[i] - smoothList_P2_P1_R[i]);
                //    ListDiff[1] += Math.Abs(smoothList_P1_P2_R[i] - smoothList_P2_P1_L[i]);
                //}

                //找到2组中求和最小的组
                smoothList = new List<double>();//进行数据合并
                if (P1Centerpos >= P2Centerpos)  //ListDiff[0] <= ListDiff[1])
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

            //foreach (var item in result_P1_P2.MotorPos_mm)
            //{
            //    if (item.Key.Name == ffa.Name)
            //    {
            //        thetaList = item.Value;
            //    }
            //}

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
                    result_P1_P2.Current_mA[AnalogChannel][i] = smoothList_2[i];
                    //result_P1_P2.MotorPos_mm[ffa][i] = thetaList[i];

                    //FarFieldRawDataPoint ffraw = new FarFieldRawDataPoint();
                    //ffraw.Theta = Math.Round((thetaList[i] - phyZeroAngle), 5); //产品的角度
                    //ffraw.PD_Reading = Convert.ToDouble(smoothList_2[i]);
                    //ffrawCol.Points.Add(ffraw);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result_P1_P2;
        }


        private AxesPosition DepthLine(out string LogDataMsg, string path, out StringBuilder strb,
            List<Motor_LaserX_9078> ThreeAxisList, AxesPosition P1, AxesPosition t1,
            AxesPosition t2, AxesPosition StartPos, eRunSize_Table eSize,  out StreamWriter sw, out TrajResultItem result,
            int id, out Dictionary<AxesPosition, double> retPoint, double size, int i, CancellationToken token)
        {
            //运动到初始点
            this.MoveToAxesPosition(ThreeAxisList, StartPos, token);
            //======================Y划线=================
            this.Updatet1t2Pos(ThreeAxisList, P1, t1, t2);

            var actlny = Y2 as Motor_LaserX_9078;
            t1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlny.AxisNo.ToString()).Position -= size;
            t2.ItemCollection.FirstOrDefault(item => item.AxisNo == actlny.AxisNo.ToString()).Position += size;

            //result = Run_Line(eRunSize_Table.Fine, t1, t2, token);

            Thread.Sleep(50);
            TrajResultItem result_1to2 = Run_Line(eSize, t1, t2, token);
            Thread.Sleep(50);
            TrajResultItem result_2to1 = Run_Line(eSize, t2, t1, token);

            result = MixResult(result_1to2, result_2to1, actlny, 0);
            this.CheckCancellationRequested(token);

            while (!DataAnalyze(result, false, out retPoint))
            //while (!DataAnalyze_line(result_1to2, result_2to1, false, out retPoint))
            {
                this.CheckCancellationRequested(token);
                LogDataMsg = path + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_精耦合超量程_Y方向_{i}.csv";
                this.WriteCSCVFile(LogDataMsg, out strb, out sw, result);
                //LogDataMsg = path + $@"\精耦合超量程_{id}_Y1to2_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_1to2);
                //LogDataMsg = path + $@"\精耦合超量程_{id}_Y2to1_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_2to1);

                //运动到初始点
                this.MoveToAxesPosition(ThreeAxisList, StartPos, token);

                //result = Run_Line(eRunSize_Table.Fine, t1, t2, token);
                Thread.Sleep(50);
                result_1to2 = Run_Line(eSize, t1, t2, token);
                Thread.Sleep(50);
                result_2to1 = Run_Line(eSize, t2, t1, token);

                result = MixResult(result_1to2, result_2to1, actlny, 0);
                this.CheckCancellationRequested(token);
            }
            LogDataMsg = path + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_精耦合_{i}_Y方向.csv";
            this.WriteCSCVFile(LogDataMsg, out strb, out sw, result);
            //LogDataMsg = path + $@"\精耦合_{id}_Y1to2_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_1to2);
            //LogDataMsg = path + $@"\精耦合_{id}_Y2to1_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_2to1);
            this.CheckCancellationRequested(token);

            //运动到初始点
            this.MoveToAxesPosition(ThreeAxisList, StartPos, token);

            P1 = retPoint.First().Key;  //更新最佳位置
            return P1;
        }

        //空间直线搜索
        private AxesPosition Depth_3DLine(out string LogDataMsg, string path, out StringBuilder strb,
            List<Motor_LaserX_9078> ThreeAxisList, AxesPosition P1, AxesPosition LastP1, AxesPosition t1,
            AxesPosition t2, AxesPosition StartPos, eRunSize_Table eSize, out StreamWriter sw, out TrajResultItem result,
            int id, out Dictionary<AxesPosition, double> retPoint, double size, int i, CancellationToken token)
        {
            //运动到初始点
            this.MoveToAxesPosition(ThreeAxisList, StartPos, token);
            //======================3D划线=================
            this.Updatet1t2Pos(ThreeAxisList, P1, t1, t2);

            var actlnx = X2 as Motor_LaserX_9078;
            var actlny = Y2 as Motor_LaserX_9078;
            var actlnz = Z2 as Motor_LaserX_9078;

            //3D线
            {
                //P1 X,Y,Z
                double p1x = P1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnx.AxisNo.ToString()).Position;
                double p1y = P1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlny.AxisNo.ToString()).Position;
                double p1z = P1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnz.AxisNo.ToString()).Position;

                double p2x = LastP1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnx.AxisNo.ToString()).Position;
                double p2y = LastP1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlny.AxisNo.ToString()).Position;
                double p2z = LastP1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnz.AxisNo.ToString()).Position;


                //  x - x1      y  - y1      z  - z1
                //---------- = ---------- = ----------
                // x2 - x1      y2 - y1      z2 - z1


                double t1y = t1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlny.AxisNo.ToString()).Position - size;
                double t2y = t1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlny.AxisNo.ToString()).Position + size;

                double t1x = (p2x - p1x) * (t1y - p1y) / (p2y - p1y) + p1x;
                double t2x = (p2x - p1x) * (t2y - p1y) / (p2y - p1y) + p1x;

                double t1z = (p2z - p1z) * (t1y - p1y) / (p2y - p1y) + p1z;
                double t2z = (p2z - p1z) * (t2y - p1y) / (p2y - p1y) + p1z;

                //更新位置
                t1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnx.AxisNo.ToString()).Position = t1x;
                t1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlny.AxisNo.ToString()).Position = t1y;
                t1.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnz.AxisNo.ToString()).Position = t1z;

                t2.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnx.AxisNo.ToString()).Position = t2x;
                t2.ItemCollection.FirstOrDefault(item => item.AxisNo == actlny.AxisNo.ToString()).Position = t2y;
                t2.ItemCollection.FirstOrDefault(item => item.AxisNo == actlnz.AxisNo.ToString()).Position = t2z;

            }




            //result = Run_Line(eRunSize_Table.Fine, t1, t2, token);

            Thread.Sleep(50);
            TrajResultItem result_1to2 = Run_Line(eSize, t1, t2, token);
            Thread.Sleep(50);
            TrajResultItem result_2to1 = Run_Line(eSize, t2, t1, token);

            result = MixResult(result_1to2, result_2to1, actlny, 0);
            this.CheckCancellationRequested(token);

            while (!DataAnalyze(result, false, out retPoint))
            //while (!DataAnalyze_line(result_1to2, result_2to1, false, out retPoint))
            {
                this.CheckCancellationRequested(token);
                LogDataMsg = path + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_精耦合超量程_3D方向_{i}.csv";
                this.WriteCSCVFile(LogDataMsg, out strb, out sw, result);
                //LogDataMsg = path + $@"\精耦合超量程_{id}_Y1to2_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_1to2);
                //LogDataMsg = path + $@"\精耦合超量程_{id}_Y2to1_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_2to1);

                //运动到初始点
                this.MoveToAxesPosition(ThreeAxisList, StartPos, token);

                //result = Run_Line(eRunSize_Table.Fine, t1, t2, token);
                Thread.Sleep(50);
                result_1to2 = Run_Line(eSize, t1, t2, token);
                Thread.Sleep(50);
                result_2to1 = Run_Line(eSize, t2, t1, token);

                result = MixResult(result_1to2, result_2to1, actlny, 0);
                this.CheckCancellationRequested(token);
            }
            LogDataMsg = path + $@"\{ModuleName}_{id}_{DateTime.Now:yyyyMMdd_HHmmss}_精耦合_{i}_3D方向.csv";
            this.WriteCSCVFile(LogDataMsg, out strb, out sw, result);
            //LogDataMsg = path + $@"\精耦合_{id}_Y1to2_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_1to2);
            //LogDataMsg = path + $@"\精耦合_{id}_Y2to1_{i}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            //this.WriteCSCVFile(LogDataMsg, out strb, out sw, result_2to1);
            this.CheckCancellationRequested(token);

            //运动到初始点
            this.MoveToAxesPosition(ThreeAxisList, StartPos, token);


            P1 = retPoint.First().Key;  //更新最佳位置
            return P1;
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

        //门限检查不抛出异常
        private bool CheckThreshold_mW(Dictionary<AxesPosition, double> retPoint)
        {
            if (retPoint.First().Value <= TestRecipe.PowerThreshold_mA)
            {
                //while(true)
                //{
                //    Thread.Sleep(100);
                //}
                Log_Global($"{this.Name} 扫描范围内无光]");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 存储RawData并返回峰值点位
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool DataAnalyze(TrajResultItem result,
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

                    if (x_range <= this.TestRecipe.Rough_Radius / 2 && y_range <= this.TestRecipe.Rough_Radius / 2 && z_range <= this.TestRecipe.Rough_Radius / 2)
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
                        return this.TestRecipe.Rough_Radius;
                    }
                }

                return this.TestRecipe.Rough_Radius;
            }
            catch (Exception ex)
            {
                return this.TestRecipe.Rough_Radius;
            }
        }

        /// <summary>
        /// 更新t1和t2位置
        /// </summary>
        /// <param name="threeAxisList"></param>
        /// <param name="P1"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        private void Updatet1t2Pos(List<Motor_LaserX_9078> threeAxisList,
                                    AxesPosition P1,
                                    AxesPosition t1,
                                    AxesPosition t2)
        {
            foreach (Motor_LaserX_9078 axis in threeAxisList)
            {
                var pos = Math.Round(P1.ItemCollection.FirstOrDefault(item => item.AxisNo == axis.AxisNo.ToString()).Position, 6);
                t1.ItemCollection.FirstOrDefault(item => item.AxisNo == axis.AxisNo.ToString()).Position = pos;
                t2.ItemCollection.FirstOrDefault(item => item.AxisNo == axis.AxisNo.ToString()).Position = pos;
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
        public TrajResultItem Run_Line(eRunSize_Table eSize, AxesPosition P1, AxesPosition P2, CancellationToken token)
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
                        RadiusSales = 2;
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

            return Run_Line_Parameter(Trajspeed, P1, P2, token);
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
        public TrajResultItem Run_Line_Parameter(double Trajspeed, AxesPosition P1, AxesPosition P2, CancellationToken token)
        {
            Dictionary<PmTrajAxisType, MotorAxisBase> axisDict = new Dictionary<PmTrajAxisType, MotorAxisBase>
            {
                { PmTrajAxisType.X_Dir, X2 },
                { PmTrajAxisType.Y_Dir, Y2 },
                { PmTrajAxisType.Z_Dir, Z2 }
            }; //插补轴定义

            //获取当前需求轴位置, 作为返回点
            var lnx = X2;
            var lny = Y2;
            var lnz = Z2;

            var actlnx = X2 as Motor_LaserX_9078;
            var actlny = Y2 as Motor_LaserX_9078;
            var actlnz = Z2 as Motor_LaserX_9078;

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

            //开始点组
            List<AxesPosition> _P1 = new List<AxesPosition>();

            //结束点组
            List<AxesPosition> _P2 = new List<AxesPosition>();

            _P1.Add(P1);
            _P2.Add(P2);

            //Thread.Sleep(100);

            TrajResultItem result = new TrajResultItem();
            int rtn = 0;
            Thread.Sleep(10);
            rtn = Parallel_MoveLine(axisDict, P1, P2, Trajspeed, out result, token);

            if (rtn != 0)
            {
                //异常返回;
            }

            return result;
        }

        public double GetSerachSize(eRunSize_Table eSize)
        {
            double RadiusSales = 1;       //比例
            double IntervalSales = 1;
            double Rough_R = 1;
            double Rough_Inv = 1;

            double Trajspeed = 1;

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
                        RadiusSales = 2;
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

            return Rough_R;
        }

        /// <summary>
        /// 存储RawData并返回峰值点位,针对画10字
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool DataAnalyze_line(TrajResultItem result1, TrajResultItem result2, bool isRough, out Dictionary<AxesPosition, double> AnalyzeResult)
        {
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

                foreach (var item in result1.MotorPos_mm)
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
                foreach (var item in result2.MotorPos_mm)
                {
                    if (item.Key.Name == "LNX")
                    {
                        xList.AddRange(item.Value);
                    }
                    if (item.Key.Name == "LNY")
                    {
                        yList.AddRange(item.Value);
                    }
                    if (item.Key.Name == "LNZ")
                    {
                        zList.AddRange(item.Value);
                    }
                }

                //运动卡模拟量通道, 取电流
                pList.AddRange(result1.Current_mA[ch]);
                pList.AddRange(result2.Current_mA[ch]);

                var pmax = pList.Max();
                var sense = Analog_LaserX_9078.GetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, 0); //当前挡位

                var actlnx = X2 as Motor_LaserX_9078;
                var actlny = Y2 as Motor_LaserX_9078;
                var actlnz = Z2 as Motor_LaserX_9078;
                var actList = new List<Motor_LaserX_9078>() { actlnx, actlny, actlnz };

                if (pList.Max() >= 2) // TODO 待修改饱和值
                {
                    tpList = new List<double>();
                    txList = new List<double>();
                    tyList = new List<double>();
                    tzList = new List<double>();

                    for (int i = 0; i < pList.Count; i++)
                    {
                        if (pList[i] >= 2)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);
                        }
                    }
                    var maxIndex = 0;
                    //if (tpList.Count()<3)
                    {
                        if (isRough)
                        {
                            maxIndex = GetMax_Rough(pList);
                        }
                        else
                        {
                            maxIndex = GetMax(pList);
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
                            Position = axis.Name == "LNX" ? txList.Average() :
                                       axis.Name == "LNY" ? tyList.Average() :
                                       axis.Name == "LNZ" ? tzList.Average() :
                                       axisPos.Position
                        });
                    }

                    Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                    //PD电流
                    tmaxPoint.Add(tPmax, pList[maxIndex]);
                    AnalyzeResult = tmaxPoint;
                    if (pList.Max() >= 2) { Log_Global("PD Range 饱和"); }
                    return true;
                }
                else if (pList.Max() >= sense * 0.95)//大于当前当前量程的最大值，则去跳档
                {
                    tpList = new List<double>();
                    txList = new List<double>();
                    tyList = new List<double>();
                    tzList = new List<double>();

                    for (int i = 0; i < pList.Count; i++)
                    {
                        //if (pList[i] >= sense * 0.95)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);
                        }
                    }
                    var maxIndex = GetMax(tpList);

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
                            Position = axis.Name == "LNX" ? txList[maxIndex] :
                                       axis.Name == "LNY" ? tyList[maxIndex] :
                                       axis.Name == "LNZ" ? tzList[maxIndex] :
                                       axisPos.Position
                        });
                    }

                    Dictionary<AxesPosition, double> tmaxPoint = new Dictionary<AxesPosition, double>();
                    //PD电流
                    tmaxPoint.Add(tPmax, sense);

                    AnalyzeResult = tmaxPoint;

                    //这里超过量程了, 需要跳挡
                    Analog_LaserX_9078.SetSenseCurrentRange_mA(X2 as Motor_LaserX_9078, ch, sense * 2);

                    return false;
                }
                else //既没超过最大值，也没超过当前量程最大值，则正常找最大值出去
                {
                    //var pmax = pList.Max();
                    //var pmin = pList.Min();

                    ////使用黄金分割高度
                    //var threshold_power = (pmax - pmin) * 0.618 + pmin;

                    tpList = new List<double>();
                    txList = new List<double>();
                    tyList = new List<double>();
                    tzList = new List<double>();

                    for (int i = 0; i < pList.Count; i++)
                    {
                        //if (pList[i] >= threshold_power)
                        {
                            tpList.Add(pList[i]);
                            txList.Add(xList[i]);
                            tyList.Add(yList[i]);
                            tzList.Add(zList[i]);
                        }
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
                            Position = axis.Name == "LNX" ? txList[maxIndex] :
                                       axis.Name == "LNY" ? tyList[maxIndex] :
                                       axis.Name == "LNZ" ? tzList[maxIndex] :
                                       axisPos.Position
                        });
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

        public int GetMax_Rough(List<double> pList)
        {
            try
            {
                if(pList.Count<2)
                {
                    return 0;
                }

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
            double Rough_R_Inside = 1;
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
                        Rough_R_Inside = Rough_Inv;
                        Trajspeed = TestRecipe.Rough_Trajspeed;
                    }
                    break;

                case eRunSize_Table.Rough_Half:
                    {
                        RadiusSales = 0.5;
                        IntervalSales = 0.5;
                        Rough_R = TestRecipe.Rough_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Rough_Involute_Interval * IntervalSales;
                        Rough_R_Inside = Rough_Inv;
                        Trajspeed = TestRecipe.Rough_Trajspeed * RadiusSales;
                    }
                    break;

                case eRunSize_Table.Fine_Double:   //精扫
                    {
                        RadiusSales = 2;
                        IntervalSales = 2;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales * 3;
                        Rough_Inv = TestRecipe.Fine_Involute_Interval * IntervalSales;
                        Rough_R_Inside = Rough_Inv;
                        Trajspeed = TestRecipe.Fine_Trajspeed * RadiusSales;
                    }
                    break;

                //case eRunSize_Table.Fine_Double_line:   //Double粗扫
                //    {
                //        //RadiusSales = 2;
                //        //IntervalSales = 2;
                //        //Rough_R = TestRecipe.DoubleSizeline_Radius;
                //        //Rough_Inv = TestRecipe.DoubleSizeline_Interval;
                //        //Rough_R_Inside = TestRecipe.DoubleSizeline_Radius_Inside;    // 排除中间的
                //        //Trajspeed = TestRecipe.DoubleSizeline_Trajspeed; //相对提高一下速度
                //    }
                //    break;

                //case eRunSize_Table.Fine_Involute_line:   //用于10字最后的渐开线扫描
                //    {
                //        RadiusSales = 1;
                //        IntervalSales = 1;
                //        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                //        Rough_Inv = TestRecipe.Fine_Involute_Interval * IntervalSales;
                //        Rough_R_Inside = Rough_Inv;
                //        Trajspeed = TestRecipe.DoubleSizeline_Trajspeed / 2; //相对提高一下速度
                //    }
                //    break;

                case eRunSize_Table.Fine:   //精扫
                    {
                        RadiusSales = 1;
                        IntervalSales = 1;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Involute_Interval * IntervalSales;
                        Rough_R_Inside = Rough_Inv;
                        Trajspeed = TestRecipe.Fine_Trajspeed * RadiusSales;
                    }
                    break;

                case eRunSize_Table.Fine_Half:
                    {
                        RadiusSales = 0.5;
                        IntervalSales = 0.5;
                        Rough_R = TestRecipe.Fine_Radius * RadiusSales;
                        Rough_Inv = TestRecipe.Fine_Involute_Interval * IntervalSales;
                        Rough_R_Inside = Rough_Inv;
                        Trajspeed = TestRecipe.Fine_Trajspeed * RadiusSales;
                    }
                    break;
                default:
                    return new TrajResultItem();
            }

            return Run_Involute_Parameter(Rough_R_Inside, Rough_R, Rough_Inv, Trajspeed, Position, Plane,thresholdStop, token);
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

            return Run_Involute_Parameter(Rough_Inv/2, Rough_R, Rough_Inv, Trajspeed, Position, Plane, thresholdStop, token);
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

#endregion Tools
    }
}