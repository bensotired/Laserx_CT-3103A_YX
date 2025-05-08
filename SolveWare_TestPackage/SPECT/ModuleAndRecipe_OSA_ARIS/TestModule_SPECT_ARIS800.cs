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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestPlugin_Demo;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
    (
         //"TestCalculator_SPECT_PowerMax",
         //"TestCalculator_SPECT_Wavelength"
         "TestCalculator_Demo"
    )]


    [ConfigurableInstrument("Keithley2601B_PULSE", "K2602B", "用于产品LD加电")]
    [ConfigurableInstrument("OSA_Aris800", "OSA_A800", "用于产品读取光功率参数")]
    [ConfigurableInstrument("MeerstetterTECController_1089", "TC_5", "用于提供产品内部测试温度")]
    [ConfigurableInstrument("RelayController", "RC_44", "用于切换硬件测试线路")]


    [StaticResource(ResourceItemType.AXIS, "导轨移动X轴", "导轨移动X轴")]
    [StaticResource(ResourceItemType.AXIS, "积分球旋转轴", "积分球旋转轴")]

    [StaticResource(ResourceItemType.POS, "Slot_1_Dut_1测试工位", "Slot_1_Dut_1测试工位")]
    [StaticResource(ResourceItemType.POS, "Slot_1_Dut_2测试工位", "Slot_1_Dut_2测试工位")]
    [StaticResource(ResourceItemType.POS, "Slot_2_Dut_1测试工位", "Slot_2_Dut_1测试工位")]
    [StaticResource(ResourceItemType.POS, "Slot_2_Dut_2测试工位", "Slot_2_Dut_2测试工位")]
    [StaticResource(ResourceItemType.POS, "积分球零点位", "积分球预设旋转方向测试工位")]

    [StaticResource(ResourceItemType.IO, "Output_偏振片上升", "偏振片上")]
    [StaticResource(ResourceItemType.IO, "Output_偏振片下降", "偏振片下")]
    [StaticResource(ResourceItemType.IO, "Output_积分球前进", "积分球前")]
    [StaticResource(ResourceItemType.IO, "Output_积分球后退", "积分球后")]

    [StaticResource(ResourceItemType.IO, "Input_偏振片上", "偏振片上")]
    [StaticResource(ResourceItemType.IO, "Input_偏振片下", "偏振片下")]

    public class TestModule_SPECT_ARIS800 : TestModuleBase
    {

        Keithley2601B_PULSE K2602 { get { return (Keithley2601B_PULSE)this.ModuleResource["K2602B"]; } }

        OSA_Aris800 OSA_A800 { get { return (OSA_Aris800)this.ModuleResource["OSA_A800"]; } }

        MeerstetterTECController_1089 TC_1 { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_5"]; } }

        RelayController RC_44 { get { return (RelayController)this.ModuleResource["RC_44"]; } }


        MotorAxisBase X_Axis { get { return (MotorAxisBase)this.ModuleResource["导轨移动X轴"]; } }

        MotorAxisBase R_Axis { get { return (MotorAxisBase)this.ModuleResource["积分球旋转轴"]; } }


        AxesPosition X_Position
        {
            get
            {
                var slotName = this.TestRecipe.Slot;
                var DutName = this.TestRecipe.Dut;
                string positionName = "Slot_1_Dut_1测试工位";
                if (slotName == SlotType.Slot_1 && DutName == DutType.Dut_1)
                {
                    positionName = "Slot_1_Dut_1测试工位";
                }
                else if (slotName == SlotType.Slot_1 && DutName == DutType.Dut_2)
                {
                    positionName = "Slot_1_Dut_2测试工位";
                }
                else if (slotName == SlotType.Slot_2 && DutName == DutType.Dut_1)
                {
                    positionName = "Slot_2_Dut_1测试工位";
                }
                else if (slotName == SlotType.Slot_2 && DutName == DutType.Dut_2)
                {
                    positionName = "Slot_2_Dut_2测试工位";
                }
                return (AxesPosition)this.ModuleResource[positionName];
            }
        }

        AxesPosition R_Position { get { return (AxesPosition)this.ModuleResource["积分球零点位"]; } }



        IOBase Output_Polaroid_Up { get { return (IOBase)this.ModuleResource["Output_偏振片上升"]; } }
        IOBase Output_Polaroid_Down { get { return (IOBase)this.ModuleResource["Output_偏振片下降"]; } }

        IOBase Output_PD_Front { get { return (IOBase)this.ModuleResource["Output_积分球前进"]; } }
        IOBase Output_PD_Back { get { return (IOBase)this.ModuleResource["Output_积分球后退"]; } }


        IOBase Input_Polaroid_Up { get { return (IOBase)this.ModuleResource["Input_偏振片上"]; } }
        IOBase Input_Polaroid_Down { get { return (IOBase)this.ModuleResource["Input_偏振片下"]; } }



        TestRecipe_SPECT_ARIS800 TestRecipe { get; set; }

        //DutRelayCommandCollection ExModuleConfig { get; set; }

        RelayCommandItem RelayCmds { get; set; }


        //光谱仪只有一个，但产品为多通道LD
        //RawDataMenuCollection<RawData_SPECT> RawData { get; set; }

        RawData_SPECT RawData { get; set; }


        public TestModule_SPECT_ARIS800() : base() { }


        public override Type GetTestRecipeType() { return typeof(TestRecipe_SPECT_ARIS800); }


        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_SPECT_ARIS800>(testRecipe);
        }

        public override IRawDataBaseLite CreateRawData()
        {
            //RawData = new RawDataMenuCollection<RawData_SPECT>();
            RawData = new RawData_SPECT();
            return RawData;
        }

        public override bool LoadExternalModuleConfig()
        {
            try
            {
                var filePath = this._core.StationConfig.TestPlugins.Find(x => x.PluginType == "TestPluginWorker_CT70250A").PluginConfigFile;
                var _AuxiliaryConfig = AuxiliaryTestPluginConfig.Load(filePath);


                RelayCmds = _AuxiliaryConfig.RC_44RelayCommands[this.TestRecipe.Slot][this.TestRecipe.Dut].DutRelayCmds;

                //转换系数待定
            }
            catch (Exception ex)
            {
                throw new Exception($"Load External Module Config Error!{ex.Message}");
            }
            return true;
        }

        public override bool LoadExternalModuleConfig(IDeviceStreamDataBase configObj)
        {
            try
            {
                var device = configObj as DeviceStreamData_CT70250A;

                TestRecipe.Slot = device.AutoRunningTestParams.Slot;
                TestRecipe.Dut = device.AutoRunningTestParams.Dut;

                RelayCmds = device.AutoRunningTestParams.RC_44RelayCommands[TestRecipe.Slot][TestRecipe.Dut].DutRelayCmds;

            }
            catch (Exception ex)
            {
                throw new Exception($"Load External Module Config Error!{ex.Message}");
            }
            return true;
        }



        public override void Run(CancellationToken token)
        {
            try
            {
                if (K2602 == null || OSA_A800 == null || TC_1 == null)
                {
                    this.Log_Global($"仪器未准备!测试不会开始！");
                    return;
                }

                if (!K2602.IsOnline || !OSA_A800.IsOnline)
                {
                    this.Log_Global($"仪器未启用!测试不会开始！");
                    return;
                }

                RawData.SlotName = this.TestRecipe.Slot.ToString();
                RawData.DutName = this.TestRecipe.Dut.ToString();
                RawData.DutChannel = this.TestRecipe.DutCh.ToString();



                MPD_Command[] mPD_Commands = new MPD_Command[] { RelayCmds.MPD_Cmd };
                TECTH_Command[] tECTH_Commands = new TECTH_Command[] { RelayCmds.TEC_Cmd };
                LD_Command[] lD_Commands = new LD_Command[] { RelayCmds[this.TestRecipe.DutCh] };

                RC_44.Dut_MultiHandover(mPD_Commands, tECTH_Commands, lD_Commands);
                Thread.Sleep(200);

                //回读温度，判断是否存在产品
                if (double.IsNaN(TC_1.CurrentObjectTemperature))
                {
                    this.Log_Global($"未检测到产品!测试不会开始！");
                    return;
                }
                var status = TC_1.DeviceStatus;
                if (status == DeviceStatus.Error)
                {
                    this.Log_Global($"温控器[{TC_1.Name}]状态异常！");
                    return;
                }

                //偏振片上
                //积分球后退
                Output_Polaroid_Down.TurnOn(false);
                Output_Polaroid_Up.TurnOn(true);

                Output_PD_Front.TurnOn(false);
                Output_PD_Back.TurnOn(true);


                //积分球移动至测试位置
                if (this.TestRecipe.FiberAlignment == false)
                {
                    //待验证
                    var axisPosition_x = this.X_Position.ItemCollection.Find(x => x.Name == X_Axis.Name);
                    if (axisPosition_x == null)
                    {
                        var msg = $"轴[{this.X_Axis.Name}]运行错误！\r\n" +
                            $"资源加载的点位[{this.X_Position.Name}]中未包含测试运行所需要的轴位[{this.X_Axis.Name}]\r\n" +
                            $"请检查点位[{this.X_Position.Name}]!";
                        this.Log_Global(msg);
                        return;
                    }
                    var xPosi_Zero = axisPosition_x.Position;

                    X_Axis.MoveToV3(xPosi_Zero, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                    int err = this.X_Axis.WaitMotionDone();
                    if (err == ErrorCodes.MotorMoveTimeOutError)
                    {
                        this.Log_Global($"轴[{this.X_Axis.Name}]运动超时！");
                        return;
                    }
                    Thread.Sleep(20);


                    var axisPosition_R = this.R_Position.ItemCollection.Find(x => x.Name == R_Axis.Name);
                    if (axisPosition_R == null)
                    {
                        var msg = $"轴[{this.R_Axis.Name}]运行错误！\r\n" +
                            $"资源加载的点位[{this.R_Position.Name}]中未包含测试运行所需要的轴位[{this.R_Axis.Name}]\r\n" +
                            $"请检查点位[{this.R_Position.Name}]!";
                        this.Log_Global(msg);
                        return;
                    }

                    var degPosi_Zero = axisPosition_R.Position;
                    R_Axis.MoveToV3(degPosi_Zero, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                    err = this.R_Axis.WaitMotionDone();
                    if (err == ErrorCodes.MotorMoveTimeOutError)
                    {
                        this.Log_Global($"轴[{this.R_Axis.Name}]运动超时！");
                        return;
                    }
                    Thread.Sleep(20);
                }

                //检查偏振片是否在上方
                if (!Input_Polaroid_Down.Interation.IsActive && Input_Polaroid_Up.Interation.IsActive)
                {
                    //积分球向前运动
                    Output_PD_Back.TurnOn(false);
                    Thread.Sleep(20);
                    Output_PD_Front.TurnOn(true);
                }
                else
                {
                    this.Log_Global($"积分球前向运动失败，测试不会进行！请检查到偏振片是否处于安全位置！");
                    return;
                }

                this.Log_Global($"开始测试!");
                if (token.IsCancellationRequested)
                {
                    this.Log_Global($"用户取消测试！");
                    token.ThrowIfCancellationRequested();
                }

                RawData.Temperature = TC_1.CurrentObjectTemperature;


                K2602.Reset();
                Thread.Sleep(20);


                OSA_A800.SetParameters(this.TestRecipe.IntegrationTime_ms, this.TestRecipe.BoxcarWidth_nm, this.TestRecipe.ScansToAverage_nm);

                List<string> BiasCurrentList = this.TestRecipe.strBiasCurrentList_mA.Split(',').ToList();
                for (int CurrentIndex = 0; CurrentIndex < BiasCurrentList.Count; CurrentIndex++)
                {
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                    }


                    double BiasCurrent_mA = 0;
                    double.TryParse(BiasCurrentList[CurrentIndex], out BiasCurrent_mA);

                    //加电前等待
                    if (this.TestRecipe.BiasBeforeWait_ms > 0)
                    {
                        this.Log_Global($"加电前等待 {this.TestRecipe.BiasBeforeWait_ms}ms.");
                        Thread.Sleep(this.TestRecipe.BiasBeforeWait_ms);
                    }


                    var current_A = BiasCurrent_mA / 1000.0;


                    K2602.SetCurrentOutput(this.TestRecipe.SourceChannel, current_A, this.TestRecipe.complianceVoltage_V);

                    //加电后等待至少500ms
                    if (this.TestRecipe.BiasAfterWait_ms > 0)
                    {
                        int waittime = 500;
                        waittime = Math.Max(waittime, this.TestRecipe.BiasAfterWait_ms);
                        this.Log_Global($"加电后等待 {waittime}ms.");
                        Thread.Sleep(waittime);
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }

                    var opticalData = OSA_A800.GetOpticalData();
                    var waveLength = OSA_A800.GetWaveLength();


                    RawData.LaserCurrent_mA = BiasCurrent_mA;

                    for (int i = 0; i < waveLength.Length; i++)
                    {
                        RawData.Add(new RawDatumItem_SPECT()
                        {
                            Power = opticalData[i],
                            Wavelength_nm = waveLength[i]
                        });
                    }
                }

                K2602.Reset();


                Output_PD_Front.TurnOn(false);
                Output_PD_Back.TurnOn(true);
            }
            catch (Exception ex)
            {


            }
        }
    }
}