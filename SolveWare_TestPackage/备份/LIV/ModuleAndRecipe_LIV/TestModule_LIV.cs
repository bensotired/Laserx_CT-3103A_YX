using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Threading;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
     ("TestCalculator_LIV_PowerCalibration",
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
    [ConfigurableInstrument("ISourceMeter_Golight", "SourceMeter_Master", "用于LIV扫描")]
    //[ConfigurableInstrument("ISourceMeter_Golight", "SourceMeter_Slaver", "用于LIV扫描2")]
    //[StaticResource (ResourceItemType.AXIS, "SWRY_进料Y","Y轴")]
    public class TestModule_LIV : TestModuleBase
    {

        public TestModule_LIV() : base()
        {
        }
 
        ISourceMeter_Golight SourceMeter_Master { get { return (ISourceMeter_Golight)this.ModuleResource["SourceMeter_Master"]; } }

        TestRecipe_LIV TestRecipe { get; set; }
        RawData_LIV RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_LIV);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_LIV();
            return RawData;
        }
        
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_LIV>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                //根据1100A
                this.Log_Global($"开始测试!");
                //这里还没有实例化  需要制作方法得到smu的实例

                if (SourceMeter_Master == null)
                {
                    return;
                }

                //smu.Reset();  每次使用的时候必须要reset 否则有可能导致带电状态接触芯片  导致芯片电死
                if (SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Timeout_ms = 1000;
                    SourceMeter_Master.CurrentSetpoint_A = 0;
                    SourceMeter_Master.VoltageSetpoint_V = 0;
                    SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                    SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                    SourceMeter_Master.IsOutputEnable = false;
                }
                SourceMeter_Master.Timeout_ms = 60 * 1000;

                //建立数组表
                int sweepPoints = Convert.ToInt32((this.TestRecipe.I_Stop_mA - this.TestRecipe.I_Start_mA) / this.TestRecipe.I_Step_mA) + 1;
                float[] ldCurrs_mA = new float[sweepPoints];
                float[] ldVolt_V = new float[sweepPoints];
                float[] pdCurrs_mA = new float[sweepPoints];
                float[] mpdCurrs_mA = new float[sweepPoints];
                float[] eaCurrs_mA = new float[sweepPoints];

                if (TestRecipe.EnableEAOutput)
                {
                    this.Log_Global($"Start LIV sweep ...{this.TestRecipe.I_Start_mA}~{this.TestRecipe.I_Stop_mA}mA " +
                                                 $"step  {this.TestRecipe.I_Step_mA}mA, " +
                                          $"EA Voltage = {this.TestRecipe.EAVoltage_V}V");
                    SourceMeter_Master.Sweep_LD_EA_PD(Convert.ToSingle(this.TestRecipe.I_Start_mA),
                                       Convert.ToSingle(this.TestRecipe.I_Step_mA),
                                       Convert.ToSingle(this.TestRecipe.I_Stop_mA),
                                       Convert.ToSingle(this.TestRecipe.complianceVoltage_V),
                                       Convert.ToSingle(this.TestRecipe.EAVoltage_V),
                                       Convert.ToSingle(this.TestRecipe.EAComplianceCurrent_mA),
                                       Convert.ToSingle(this.TestRecipe.PDBiasVoltage_V), //PD偏置电压, 默认0V
                                       Convert.ToSingle(this.TestRecipe.PDComplianceCurrent_mA),
                                       this.TestRecipe.K2400_NPLC);
                    while (SourceMeter_Master.IsSweeping)
                    {
                        Thread.Sleep(100);
                    }
                    this.Log_Global("Fetch LIV sweep data.");
                    ldCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.LD_Drive_Current_mA);
                    ldVolt_V = SourceMeter_Master.FetchLDSweepData(SweepDataType.LD_Drive_Voltage_V);
                    pdCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.PD_Ch1_Current_mA);

                }
                else if (this.TestRecipe.ReadMPD)
                {
                    this.Log_Global($"Start LIV sweep ...{this.TestRecipe.I_Start_mA}~{this.TestRecipe.I_Stop_mA}mA step {this.TestRecipe.I_Step_mA}mA, MPD Voltage = {this.TestRecipe.MPDBiasVoltage}V");
                    SourceMeter_Master.Sweep_LD_MPD_PD(Convert.ToSingle(this.TestRecipe.I_Start_mA),
                                        Convert.ToSingle(this.TestRecipe.I_Step_mA),
                                        Convert.ToSingle(this.TestRecipe.I_Stop_mA),
                                        Convert.ToSingle(this.TestRecipe.complianceVoltage_V),
                                        Convert.ToSingle(this.TestRecipe.MPDBiasVoltage),
                                        Convert.ToSingle(this.TestRecipe.MPDComplianceCurrent_mA),
                                        Convert.ToSingle(this.TestRecipe.PDBiasVoltage_V), //PD偏置电压, 默认0V
                                        Convert.ToSingle(this.TestRecipe.PDComplianceCurrent_mA),
                                        this.TestRecipe.K2400_NPLC);
                    while (SourceMeter_Master.IsSweeping)
                    {
                        Thread.Sleep(100);
                    }
                    this.Log_Global("Fetch LIV sweep data.");
                    ldCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.LD_Drive_Current_mA);
                    ldVolt_V = SourceMeter_Master.FetchLDSweepData(SweepDataType.LD_Drive_Voltage_V);
                    pdCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.PD_Ch1_Current_mA);
                    mpdCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.PD_Ch2_Current_mA);
                }
                else
                {
                    this.Log_Global($"Start LIV sweep ...{this.TestRecipe.I_Start_mA}~{this.TestRecipe.I_Stop_mA}mA step {this.TestRecipe.I_Step_mA}mA ");
                    SourceMeter_Master.Sweep_LD_PD(Convert.ToSingle(this.TestRecipe.I_Start_mA),
                                        Convert.ToSingle(this.TestRecipe.I_Step_mA),
                                        Convert.ToSingle(this.TestRecipe.I_Stop_mA),
                                        Convert.ToSingle(this.TestRecipe.complianceVoltage_V),
                                        Convert.ToSingle(this.TestRecipe.PDBiasVoltage_V), //PD偏置电压, 默认0V
                                        Convert.ToSingle(this.TestRecipe.PDComplianceCurrent_mA),
                                        this.TestRecipe.K2400_NPLC);
                    while (SourceMeter_Master.IsSweeping)
                    {
                        Thread.Sleep(100);
                    }
                    this.Log_Global("Fetch LIV sweep data.");
                    ldCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.LD_Drive_Current_mA);
                    ldVolt_V = SourceMeter_Master.FetchLDSweepData(SweepDataType.LD_Drive_Voltage_V);
                    pdCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.PD_Ch1_Current_mA);
                }
                if (SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Timeout_ms = 1000;
                    SourceMeter_Master.CurrentSetpoint_A = 0;
                    SourceMeter_Master.VoltageSetpoint_V = 0;
                    SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                    SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                    SourceMeter_Master.IsOutputEnable = false;
                }
                this.Log_Global("LIV sweep finished..");
                //PD校准系数 应该从主配置文件中导入的*************
                double SphereResponsivity = 1;
                double PowerOffset_mW = 0;
                for (int i = 0; i < ldCurrs_mA.Length; i++)
                {
                    RawData.Add(new RawDatumItem_LIV()
                    {
                        Current_mA = ldCurrs_mA[i],
                        Voltage_V = ldVolt_V[i],
                        Power_mW = (pdCurrs_mA[i] * SphereResponsivity + PowerOffset_mW)
                    });
                }
            }
            catch (Exception ex)
            {
            }
        }

        
    }
}