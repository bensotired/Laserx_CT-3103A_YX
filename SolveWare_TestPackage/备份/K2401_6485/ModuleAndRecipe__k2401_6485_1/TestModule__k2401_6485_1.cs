using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace SolveWare_TestPackage.K2401_6485.ModuleAndRecipe__k2401_6485_1
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
    [ConfigurableInstrument("ISourceMeter_Keithley", "K2401_Master", "K2401源表")]
    [ConfigurableInstrument("ISourceMeter_Keithley", "K6485_Master", "K6485源表")]
    public class TestModule_k2401_6485_1 : TestModuleBase
    {
        public TestModule_k2401_6485_1() : base() { }

        ISourceMeter_Keithley K2401 { get { return (ISourceMeter_Keithley)this.ModuleResource["K2401_Master"]; } }
        ISourceMeter_Keithley K6485 { get { return (ISourceMeter_Keithley)this.ModuleResource["K6485_Master"]; } }
        TestRecipe_k2401_6485_1 TestRecipe { get; set; }
        RawData_LIV RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_k2401_6485_1);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_LIV();
            return RawData;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_k2401_6485_1>(testRecipe);
        }

        public override void Run(CancellationToken token)
        {
            try
            {
                //K2401.TurnOnline(true);
                //K6485.TurnOnline(true);
                K2401.IsOutputOn = false;
                K2401.Reset(); Thread.Sleep(100);
                //K2401.Trigger();

                K2401.SourceMode = SourceModeTypes.Current;//输出的类型

                K2401.SenseMode = SenseModeTypes.AllOff;
                K2401.SenseMode = SenseModeTypes.Voltage;//回读的类型
                K2401.Terminal = SelectTerminal.Rear;//接线在背面
                K2401.IsCurrentSenseAutoRangeOn = true; //全部设置成自动范围
                K2401.IsCurrentSourceAutoRangeOn = true;
                K2401.IsVoltageSenseAutoRangeOn = true;
                K2401.IsVoltageSourceAutoRangeOn = true;
                K2401.IsFourWireOn = true;  //4线方式测量  电流+ -    两个电压
                K2401.VoltageCompliance_V = (this.TestRecipe.I_A+0.5)/1000;//限压   设置电流的时候需要限压
                K2401.CurrentSetpoint_A = this.TestRecipe.I_A;//设置使用的电流 （单位是A）

                Thread.Sleep(this.TestRecipe.Delayafter_ms);
                K2401.IsOutputOn = true;


                K6485.ZeroCorrection();
                K6485.IsCurrentSenseAutoRangeOn = this.TestRecipe.IsCurrentSenseAutoRangeOn;
                K6485.CurrentSenseRange_A = this.TestRecipe.CurrentSenseRange_A;

                //double currentK2401 = K2401.ReadCurrent_A();
                //double currentK6485_mA = K6485.ReadCurrent6485_A();

                ArrayList arrayListvolts = new ArrayList();
                ArrayList arrayListpdCurrs = new ArrayList();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stopwatch.ElapsedMilliseconds < (this.TestRecipe.Samplingduration_m * 60 * 1000))
                {
                    double volts = K2401.ReadVoltage_V();
                    double currentK6485_mA = K6485.ReadCurrent6485_A();

                    arrayListvolts.Add(volts);


                    arrayListpdCurrs.Add(currentK6485_mA*1000);

                    Thread.Sleep(this.TestRecipe.Samplinginterval_s * 1000);
                    if (!this.TestRecipe.IsPass)
                    {
                        break;
                    }
                }
                stopwatch.Reset();
                K2401.IsOutputOn = false;
                //PD校准系数 应该从主配置文件中导入的*************

                //每个电流点对应的 电压和光功率
                for (int i = 0; i < arrayListvolts.Count; i++)
                {
                    RawData.Add(new RawDatumItem_LIV()
                    {
                        Current_mA = this.TestRecipe.I_A * 1000.0,
                        Voltage_V = Convert.ToDouble(arrayListvolts[i]),
                        Power_mW = Convert.ToDouble(arrayListpdCurrs[i])
                    });
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
