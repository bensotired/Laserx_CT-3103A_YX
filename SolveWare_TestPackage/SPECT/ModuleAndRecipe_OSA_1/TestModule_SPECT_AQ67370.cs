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
    [SupportedCalculator("TestCalculator_SPECT")]





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

    [ConfigurableInstrument("ISourceMeter_Golight", "GoLight_1", "用于驱动器件")]
    [ConfigurableInstrument("OSA_AQ67370", "OSA_1", "用于产品读取光功率参数")]
    #endregion

    public class TestModule_SPECT_AQ67370 : TestModuleBase
    {

      

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

        OSA_AQ67370 OSA_1 { get { return (OSA_AQ67370)this.ModuleResource["OSA_1"]; } }

        #endregion

        TestRecipe_SPECT_AQ67370 TestRecipe { get; set; }


        RawData_SPECT RawData { get; set; }


        public TestModule_SPECT_AQ67370() : base() { }


        public override Type GetTestRecipeType() { return typeof(TestRecipe_SPECT_AQ67370); }


        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_SPECT_AQ67370>(testRecipe);
        }

        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_SPECT();
            return RawData;
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

                this.Log_Global($"开始测试!");
                if (token.IsCancellationRequested)
                {
                    this.Log_Global($"用户取消测试！");
                    token.ThrowIfCancellationRequested();
                }

                OSA_1.Reset();
                Thread.Sleep(20);


                OSA_1.CalibrationAutoZeroEnable = false;
                OSA_1.ResolutionBandwidth_nm = this.TestRecipe.OsaRbw_nm;
                OSA_1.CenterWavelength_nm = this.TestRecipe.CenterWavelength_nm;
                OSA_1.WavelengthSpan_nm = this.TestRecipe.WavelengthSpan_nm;
                OSA_1.TraceLength = this.TestRecipe.OsaTraceLength;
                OSA_1.Sensitivity = this.TestRecipe.Sensitivity;
                OSA_1.SmsrModeDiff = this.TestRecipe.SmsrModeDiff_dB;
                OSA_1.SMSRMask_nm = this.TestRecipe.SMSRMask_nm;

                {
                    if (token.IsCancellationRequested)
                    {
                        this.Log_Global($"用户取消测试！");
                        token.ThrowIfCancellationRequested();
                    }

                    //加电前等待
                    if (this.TestRecipe.BiasBeforeWait_ms > 0)
                    {
                        this.Log_Global($"加电前等待 {this.TestRecipe.BiasBeforeWait_ms}ms.");
                        Thread.Sleep(this.TestRecipe.BiasBeforeWait_ms);
                    }

                    //加电
                    SourceMeter_Master.SetupSMU_LD(this.TestRecipe.strBiasCurrentList_A, this.TestRecipe.complianceVoltage_V);

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
                    OsaTrace traceEnum = (OsaTrace)Enum.Parse(typeof(OsaTrace), this.TestRecipe.OsaTrace);

                    PowerWavelengthTrace trace = OSA_1.GetOpticalSpectrumTrace(true, traceEnum);

                    //RawData.Trace = trace;
                    //RawData.Smsr_dB = OSA_Master.GetSmsr_dB();
                    //RawData.Wavelength_nm = OSA_Master.ReadCenterWL_nm();
                    //RawData.PeakPower_dbm = OSA_Master.ReadPowerAtPeak_dbm();
                    //RawData.SpectrumWidth_20db_nm = OSA_Master.ReadSpectrumWidth_nm(20);
                    //RawData.SpectrumWidth_3db_nm = OSA_Master.ReadSpectrumWidth_nm(3);
                    RawData.LaserCurrent_A = this.TestRecipe.strBiasCurrentList_A;

                    for (int i = 0; i < trace.Count; i++)
                    {
                        RawData.Add(new RawDatumItem_SPECT()
                        {
                            Power = trace[i].Power_dBm,
                            Wavelength_nm = trace[i].Wavelength_nm,

                        });
                    }
                }

                OSA_1.Reset();

            }
            catch (Exception ex)
            {


            }
            finally
            {
                this.SourceMeter_Master.IsOutputEnable = false;

                Out_LeftPerAvoid.TurnOn(true);

                Out_LeftPerFront.TurnOn(true);

            }
        }
    }
}