using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Threading;
using System.Windows.Forms;

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

    #region  轴、位置、IO、仪器
    [StaticResource(ResourceItemType.AXIS, "左短摆臂旋转", "左短摆臂")]
    [StaticResource(ResourceItemType.AXIS, "左长摆臂旋转", "左长摆臂")]

    [StaticResource(ResourceItemType.IO, "Input_左PER前后移动气缸动作", "左PER前后动作")]
    [StaticResource(ResourceItemType.IO, "Input_左PER前后移动气缸复位", "左PER前后复位")]
    [StaticResource(ResourceItemType.IO, "Input_左PER避位气缸动作", "左PER避位气缸动作")]
    [StaticResource(ResourceItemType.IO, "Input_左PER避位气缸复位", "左PER避位气缸复位")]

    [StaticResource(ResourceItemType.IO, "Input_左PER上下移动气缸动作", "左PER上下动作")]
    [StaticResource(ResourceItemType.IO, "Input_左PER上下移动气缸复位", "左PER上下复位")]

    [StaticResource(ResourceItemType.IO, "Output_左PER前后移动气缸电磁阀", "左PER前后电磁阀")]
    [StaticResource(ResourceItemType.IO, "Output_左PER避位气缸电磁阀", "左PER避位电磁阀")]

    [StaticResource(ResourceItemType.IO, "Output_左PER上下移动气缸电磁阀_下降", "左PER上下_下降")]
    [StaticResource(ResourceItemType.IO, "Output_左PER上下移动气缸电磁阀_上升", "左PER上下_上升")]
    [ConfigurableInstrument("ISourceMeter_Golight", "GoLight_1", "用于LIV扫描")]
    #endregion
    public class TestModule_LIV_Pluse_1 : TestModuleBase
    {
        public TestModule_LIV_Pluse_1() : base() { }

        #region 以Get获取资源
        MotorAxisBase LeftShort { get { return (MotorAxisBase)this.ModuleResource["左短摆臂旋转"]; } }
        MotorAxisBase LeftLong { get { return (MotorAxisBase)this.ModuleResource["左长摆臂旋转"]; } }
        MotorAxisBase LeftPer { get { return (MotorAxisBase)this.ModuleResource["左偏振片旋转"]; } }
        AxesPosition LeftShortZero { get { return (AxesPosition)this.ModuleResource["左短摆臂旋转绝对零位"]; } }
        AxesPosition LeftLongZero { get { return (AxesPosition)this.ModuleResource["左长摆臂旋转绝对零位"]; } }
        IOBase In_LeftPerFrontAction { get { return (IOBase)this.ModuleResource["Input_左PER前后移动气缸动作"]; } }
        IOBase In_LeftPerFrontRest { get { return (IOBase)this.ModuleResource["Input_左PER前后移动气缸复位"]; } }
        IOBase In_LeftPerAvoidAction { get { return (IOBase)this.ModuleResource["Input_左PER避位气缸动作"]; } }
        IOBase In_LeftPerAvoidRest { get { return (IOBase)this.ModuleResource["Input_左PER避位气缸复位"]; } }

        IOBase In_LeftPerAction { get { return (IOBase)this.ModuleResource["Input_左PER上下移动气缸动作"]; } }
        IOBase In_LeftPerRest { get { return (IOBase)this.ModuleResource["Input_左PER上下移动气缸复位"]; } }

        IOBase Out_LeftPerFront { get { return (IOBase)this.ModuleResource["Output_左PER前后移动气缸电磁阀"]; } }
        IOBase Out_LeftPerAvoid { get { return (IOBase)this.ModuleResource["Output_左PER避位气缸电磁阀"]; } }

        IOBase Out_LeftPerDown { get { return (IOBase)this.ModuleResource["Output_左PER上下移动气缸电磁阀_下降"]; } }
        IOBase Out_LeftPerUp { get { return (IOBase)this.ModuleResource["Output_左PER上下移动气缸电磁阀_上升"]; } }

        ISourceMeter_Golight SourceMeter_Master { get { return (ISourceMeter_Golight)this.ModuleResource["GoLight_1"]; } }



        #endregion

        TestRecipe_LIV_Pulse_1 TestRecipe { get; set; }
        RawData_LIV RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_LIV_Pulse_1);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_LIV();
            return RawData;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_LIV_Pulse_1>(testRecipe);
        }

        public override void Run(CancellationToken token)
        {
            try
            {
                if (!In_LeftPerRest.Interation.IsActive)
                {
                    Out_LeftPerDown.TurnOn(false);
                    Out_LeftPerUp.TurnOn(true);
                }
                Thread.Sleep(200);
                if (In_LeftPerAvoidRest.Interation.IsActive)
                {
                    Out_LeftPerAvoid.TurnOn(true);
                }
                if (In_LeftPerFrontRest.Interation.IsActive)
                {
                    Out_LeftPerFront.TurnOn(true);
                }
                int count = 0;
                while (true)
                {
                    count++;
                    if (In_LeftPerAvoidAction.Interation.IsActive)
                    {
                        break;
                    }
                    if (count > 5)
                    {
                        var mess = MessageBox.Show("左PER避位气缸感应异常，请人工确认", "人工确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (mess == DialogResult.OK)
                        {
                            break;
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                    Thread.Sleep(500);
                }
                count = 0;
                while (true)
                {
                    count++;
                    Thread.Sleep(500);
                    if (In_LeftPerFrontAction.Interation.IsActive)
                    {
                        break;
                    }
                    if (count > 5)
                    {
                        var mess = MessageBox.Show("左PER前后移动气缸感应异常，请人工确认", "继续", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (mess == DialogResult.OK)
                        {
                            break;
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                }

                this.Log_Global($"开始测试!");
                if (SourceMeter_Master == null)
                {
                    return;
                }
                if (!SourceMeter_Master.IsOnline)
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
                int sweepPoints = Convert.ToInt32((this.TestRecipe.I_Stop_A - this.TestRecipe.I_Start_A) / this.TestRecipe.I_Step_A) + 1;
                float[] ldCurrs_mA = new float[sweepPoints];
                float[] ldVolt_V = new float[sweepPoints];
                float[] pdCurrs_mA = new float[sweepPoints];
                float[] mpdCurrs_mA = new float[sweepPoints];
                float[] eaCurrs_mA = new float[sweepPoints];

                if (this.TestRecipe.Pulsewidth < 5000)
                {
                    this.TestRecipe.Pulsewidth = 5000;
                }
                if (this.TestRecipe.Pulseperiod < 10000)
                {
                    this.TestRecipe.Pulseperiod = 10000;
                }


                this.Log_Global($"Start LIV sweep ...{this.TestRecipe.I_Start_A}~{this.TestRecipe.I_Stop_A}A step {this.TestRecipe.I_Step_A}A ");
                SourceMeter_Master.Sweep_LD_PD_Pulse(Convert.ToSingle(this.TestRecipe.I_Start_A*1000),
                                    Convert.ToSingle(this.TestRecipe.I_Step_A*1000),
                                    Convert.ToSingle(this.TestRecipe.I_Stop_A*1000),
                                    Convert.ToSingle(this.TestRecipe.complianceVoltage_V),
                                    Convert.ToSingle(this.TestRecipe.PDBiasVoltage_V), //PD偏置电压, 默认0V
                                    Convert.ToSingle(this.TestRecipe.PDComplianceCurrent_mA),
                                    this.TestRecipe.Pulsewidth,
                                    this.TestRecipe.Pulseperiod,
                                    this.TestRecipe.K2400_NPLC);
                while (SourceMeter_Master.IsSweeping)
                {
                    Thread.Sleep(100);
                }
                this.Log_Global("Fetch LIV sweep data.");
                ldCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.LD_Drive_Current_mA);
                ldVolt_V = SourceMeter_Master.FetchLDSweepData(SweepDataType.LD_Drive_Voltage_V);
                pdCurrs_mA = SourceMeter_Master.FetchLDSweepData(SweepDataType.EA_Drive_Voltage_V);

                if (!SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Timeout_ms = 1000;
                    SourceMeter_Master.CurrentSetpoint_A = 0;
                    SourceMeter_Master.VoltageSetpoint_V = 0;
                    SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                    SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                    SourceMeter_Master.IsOutputEnable = false;
                }
                this.Log_Global("LIV sweep finished..");


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
            finally
            {
                this.SourceMeter_Master.IsOutputEnable = false;
                Out_LeftPerAvoid.TurnOn(false);

                Out_LeftPerFront.TurnOn(false);
            }
        }
    }
}
