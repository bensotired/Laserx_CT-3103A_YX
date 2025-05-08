using LX_BurnInSolution.Utilities;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestPlugin_Demo;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
    (
        "TestCalculator_LIV_PowerCalibration",
         "TestCalculator_LIV_VF",
         "TestCalculator_LIV_Iop",
         "TestCalculator_LIV_Pop",
         "TestCalculator_LIV_Ith1",
         "TestCalculator_LIV_Ith2",
         "TestCalculator_LIV_Ith3",
         "TestCalculator_LIV_Kink_Current",
         "TestCalculator_LIV_Kink_Percentage",
         "TestCalculator_LIV_Kink_Power",
         "TestCalculator_LIV_PowerMax",
         "TestCalculator_LIV_Rs",
         "TestCalculator_LIV_Rs_2Point",
         "TestCalculator_LIV_SE_mWpermA",
         "TestCalculator_LIV_SE_mWpermW"

    )]


    [ConfigurableInstrument("Keithley2601B_PULSE", "K2602B", "用于LIV扫描_信号发生")]
    [ConfigurableInstrument("LaserX_SMU_3133", "LX_3133", "用于LIV扫描_数据采集")]
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


    //2602的通道CHA加电，CHB采集MPD，3133采集PD

    public class TestModule_LIV : TestModuleBase
    {
        Keithley2601B_PULSE K2602 { get { return (Keithley2601B_PULSE)this.ModuleResource["K2602B"]; } }
        LaserX_SMU_3133 LX_3133A { get { return (LaserX_SMU_3133)this.ModuleResource["LX_3133"]; } }
        MeerstetterTECController_1089 TC_1 { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_5"]; } }
        //RelayController RC_44 { get { return (RelayController)this.ModuleResource["RC_44"]; } }

        MotorAxisBase X_Axis { get { return (MotorAxisBase)this.ModuleResource["导轨移动X轴"]; } }

        MotorAxisBase R_Axis { get { return (MotorAxisBase)this.ModuleResource["积分球旋转轴"]; } }



        AxesPosition R_Position { get { return (AxesPosition)this.ModuleResource["积分球零点位"]; } }



        IOBase Output_Polaroid_Up { get { return (IOBase)this.ModuleResource["Output_偏振片上升"]; } }
        IOBase Output_Polaroid_Down { get { return (IOBase)this.ModuleResource["Output_偏振片下降"]; } }

        IOBase Output_PD_Front { get { return (IOBase)this.ModuleResource["Output_积分球前进"]; } }
        IOBase Output_PD_Back { get { return (IOBase)this.ModuleResource["Output_积分球后退"]; } }


        IOBase Input_Polaroid_Up { get { return (IOBase)this.ModuleResource["Input_偏振片上"]; } }
        IOBase Input_Polaroid_Down { get { return (IOBase)this.ModuleResource["Input_偏振片下"]; } }


        TestRecipe_LIV TestRecipe { get; set; }

        RawData_LIV RawData { get; set; }


        public TestModule_LIV() : base() { }


        public override Type GetTestRecipeType() { return typeof(TestRecipe_LIV); }


        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_LIV>(testRecipe);
        }


        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_LIV();
            return RawData;
        }

        public override void Run(CancellationToken token)
        {
            try
            {
                if (K2602 == null || LX_3133A == null || TC_1 == null)
                {
                    this.Log_Global($"仪器未准备!测试不会开始！");
                    return;
                }

                if (!K2602.IsOnline)
                {
                    this.Log_Global($"仪器未启用!测试不会开始！");
                    return;
                }
                if (!LX_3133A.IsOnline)
                {
                    this.Log_Global($"仪器未启用!测试不会开始！");
                    return;
                }

                K2602.Reset();
                LX_3133A.Reset();

                //RawData.SlotName = this.TestRecipe.Slot.ToString();
                //RawData.DutName = this.TestRecipe.Dut.ToString();
                //RawData.DutChannel = this.TestRecipe.DutCh.ToString();

                ////先切换继电器板
                //MPD_Command[] mPD_Commands = new MPD_Command[] { RelayCmds.MPD_Cmd };
                //TECTH_Command[] tECTH_Commands = new TECTH_Command[] { RelayCmds.TEC_Cmd };
                //LD_Command[] lD_Commands = new LD_Command[] { RelayCmds[this.TestRecipe.DutCh] };

                //RC_44.Dut_MultiHandover(mPD_Commands, tECTH_Commands, lD_Commands);
                //Thread.Sleep(200);



                this.Log_Global($"开始测试!");
                if (token.IsCancellationRequested)
                {
                    this.Log_Global($"用户取消测试！");
                    token.ThrowIfCancellationRequested();
                }

                //RawData.Temperature = TC_1.CurrentObjectTemperature;

                K2602.Reset();
                LX_3133A.Reset();
                Thread.Sleep(20);

                //PD电流采集范围调节
                LX_3133A.PDAndMPDRangeAdjust(this.TestRecipe.PDCurrentRange, this.TestRecipe.PDCurrentRange);
                Thread.Sleep(20);

                //【3】LD加电以及DAQ采集
                double startCurrent_A = this.TestRecipe.I_Start_mA / 1000.0;
                double stopCurrent_A = this.TestRecipe.I_Stop_mA / 1000.0;
                double stepCurrent_A = this.TestRecipe.I_Step_mA / 1000.0;

                double period_s = Math.Round(this.TestRecipe.Period_ms / 1000.0, 6); //0.01;        //源触发时间间隔
                double pulseWidth_s = Math.Round(period_s * this.TestRecipe.DutyRatio / 100.0, 6);       //源信号释放的时间长短               
                double measDelay_s = Math.Round(this.TestRecipe.SenseDelay_ms / 1000.0, 6);     //表采集延时时间(此处为2602脉冲延时，有脉冲，DAQ才开始采集)
                var pulsedMode = TestRecipe.PulsedMode;
                double IntegratingPeriod_s;
                if (pulsedMode)
                {
                    IntegratingPeriod_s = pulseWidth_s - measDelay_s;
                    if (IntegratingPeriod_s <= 0)
                    {
                        IntegratingPeriod_s = pulseWidth_s * 0.1;
                    }
                }
                else
                {
                    IntegratingPeriod_s = period_s * 0.8;  //与k2602保持一致
                    if (IntegratingPeriod_s <= 0)
                    {
                        IntegratingPeriod_s = period_s * 0.1;
                    }
                }

                double nplc = 1;
                if (pulsedMode)
                {
                    nplc = Math.Round((pulseWidth_s * 0.8) / 0.02, 3);
                }
                else
                {
                    nplc = Math.Round((period_s * 0.8) / 0.02, 3);
                }

                //0.0001
                double MPDSenseCurrentRange_mA = Math.Round(this.TestRecipe.MPDSenseCurrentRange_mA / 1000.0, 6);

                int sweepPoints = Convert.ToInt32((stopCurrent_A - startCurrent_A) / stepCurrent_A) + 1;

                Action daqAct = new Action(() =>
                {
                    LX_3133A.LIV_Acq_PD(IntegratingPeriod_s, sweepPoints);

                });
                daqAct.BeginInvoke(null, null);
                Thread.Sleep(1000);



                //var data_k2601 = K2602.SweepDualChannelsUsingTimer(startCurrent_A, stopCurrent_A, this.TestRecipe.ComplianceVoltage_V,0,0.002,0, measDelay_s
                //    , nplc, sweepPoints, pulsedMode, IntegratingPeriod_s, period_s);

                //var data_k2601 = K2602.Sweep_LDandMPD(startCurrent_A, stopCurrent_A, sweepPoints, pulsedMode, IntegratingPeriod_s,
                //    period_s, measDelay_s, this.TestRecipe.ComplianceVoltage_V, InternalParam_DAQ.KeithleyTriggerOutChannel, 0.0001);


                //加电 
                //var data_k2601 = K2602.SweepDualChannelsUsingTimer(startCurrent_A, stopCurrent_A, this.TestRecipe.ComplianceVoltage_V, 
                //    this.TestRecipe.MPDSourceVoltage_V, MPDSenseCurrentRange_mA, 0, measDelay_s, nplc, sweepPoints,
                //    pulsedMode, IntegratingPeriod_s, period_s, InternalParam_DAQ.KeithleyTriggerOutChannel);

                Thread.Sleep(500);

                //[4]关闭仪器
                K2602.Reset();
                LX_3133A.Reset();

                if (token.IsCancellationRequested)
                {
                    this.Log_Global($"用户取消测试！");
                    token.ThrowIfCancellationRequested();
                }


                //[5]获取数据
                var dataDaq = LX_3133A.DataBook;

                double startCurrent_mA = this.TestRecipe.I_Start_mA;
                double stopCurrent_mA = this.TestRecipe.I_Stop_mA;
                double stepCurrent_mA = this.TestRecipe.I_Step_mA;


                //源表加电电流
                double[] ldCur_Dut = new double[sweepPoints];

                //源表回读
                double[] ldCur_Dut1 = new double[sweepPoints];
                double[] ldVol_Dut1 = new double[sweepPoints];
                double[] mpdVol_Dut1 = new double[sweepPoints];


                //daq回读
                double[] pdVol_Dut1 = new double[sweepPoints];


                //2602脉冲输出在延迟后触发，因此不用对DAQ数据进行剔除处理
                //int measureDelayPoints = Convert.ToInt32(measDelay_s * sampleClockRate);

                //var STR = DateTime.Now.ToString("yyyyMMddHHmm");
                //System.IO.StreamWriter SW = new System.IO.StreamWriter($"pd{STR}.csv");

                for (int j = 0; j < sweepPoints; j++)
                {
                    //ldCur_Dut[j] = startCurrent_mA + stepCurrent_mA * j;

                    //ldCur_Dut1[j] = Convert.ToDouble(data_k2601[0].Split(',')[j]);
                    //ldVol_Dut1[j] = Convert.ToDouble(data_k2601[1].Split(',')[j]);
                    //mpdVol_Dut1[j] = Convert.ToDouble(data_k2601[2].Split(',')[j]);

                    //pdVol_Dut1[j] = dataDaq[j][0].Average();
                }
                //SW.Close();


                //平滑滤波

                //var LDCur = ArrayMath.CalculateMovingAverage(ldCur_Dut1, 20);
                //var LDVol = ArrayMath.CalculateMovingAverage(ldVol_Dut1, 20);

                //var PDVol = ArrayMath.CalculateMovingAverage(pdVol_Dut1, 20);
                //var MPDVol = ArrayMath.CalculateMovingAverage(mpdVol_Dut1, 20);


                var LDCur = ldCur_Dut1;
                var LDVol = ldVol_Dut1;

                var PDVol = pdVol_Dut1;
                var MPDVol = mpdVol_Dut1;


                //补偿系数从SummaryData中添加，
                //此处是DAQ转换系数，目的只是为了将电压转电流，电流转功率
                //如果存在偏差，则从SummaryData中添加

                for (int k = 0; k < sweepPoints; k++)
                {
                    var pdCur = Math.Abs(PDVol[k]) * this.TestRecipe.PDFactor_K_1st + this.TestRecipe.PDFactor_B_1st;
                    var pdPower = pdCur * this.TestRecipe.PDFactor_K_2ed + this.TestRecipe.PDFactor_B_2ed;

                    var mpdCur = Math.Abs(MPDVol[k]);

                    //var mpdPower = mpdCur * this.TestRecipe.MPDFactor_K + this.TestRecipe.MPDFactor_B;

                    RawData.Add(new RawDatumItem_LIV()
                    {
                        Current_mA = ldCur_Dut[k],

                        ActualCurrent_mA = LDCur[k] * 1000,
                        Voltage_V = LDVol[k],

                        PDCurrent_mA = pdCur * 1000,
                        Power_mW = pdPower * 1000,

                        MPDCurrent_mA = mpdCur * 1000,
                        //MPDPower_mW = mpdPower * 1000
                    });
                }

                Output_PD_Front.TurnOn(false);
                Output_PD_Back.TurnOn(true);

            }
            catch (Exception ex)
            {


            }
        }
    }
}
