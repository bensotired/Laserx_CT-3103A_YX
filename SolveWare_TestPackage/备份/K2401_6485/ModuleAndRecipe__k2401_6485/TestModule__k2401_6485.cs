using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    [ConfigurableInstrument("ISourceMeter_Keithley", "K2401_Master", "K2401源表")]
    [ConfigurableInstrument("ISourceMeter_Keithley", "K6485_Master", "K6485源表")]
    public class TestModule_k2401_6485 : TestModuleBase
    {
        public TestModule_k2401_6485() : base() { }

        ISourceMeter_Keithley K2401 { get { return (ISourceMeter_Keithley)this.ModuleResource["K2401_Master"]; } }
        ISourceMeter_Keithley K6485 { get { return (ISourceMeter_Keithley)this.ModuleResource["K6485_Master"]; } }
        TestRecipe_k2401_6485 TestRecipe { get; set; }
        RawData_LIV RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_k2401_6485);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_LIV();
            return RawData;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_k2401_6485>(testRecipe);
        }

        public override void Run(CancellationToken token)
        {
            try
            {
                //K2401.TurnOnline(true);
                //K6485.TurnOnline(true);
                K2401.Reset();
                //K6485.Reset();
                //K6485.Initialize();
                K6485.ZeroCorrection();

                double ldcomplianceVoltage_V = this.TestRecipe.ldcomplianceVoltage_V;//保护电压V
                double nplc = this.TestRecipe.NPLC;
                double start = this.TestRecipe.I_Start_A;
                double stop = this.TestRecipe.I_Stop_A;
                double step = this.TestRecipe.I_Step_A;

                double[] currs = LX_BurnInSolution.Utilities.ArrayMath.CalculateArray(start, stop, step);

                K6485.IsCurrentSenseAutoRangeOn = this.TestRecipe.IsCurrentSenseAutoRangeOn;
                K6485.CurrentSenseRange_A = this.TestRecipe.CurrentSenseRange_A;
                TriggerLine triggerInputLine = (TriggerLine)Enum.Parse(typeof(TriggerLine), this.TestRecipe.triggerInputLine.ToString().Trim());
                TriggerLine triggerOutputLine = (TriggerLine)Enum.Parse(typeof(TriggerLine), this.TestRecipe.triggerOutputLine.ToString().Trim());

                K2401.SetupCurrentStairSweep(true, ldcomplianceVoltage_V, nplc, triggerInputLine, triggerOutputLine, currs.Length, false, SelectTerminal.Rear, start, stop, step);
                K6485.SetupTrigger(false, triggerOutputLine, triggerInputLine, currs.Length, nplc);
                //Thread.Sleep(2000);
                K6485.Trigger();
                K2401.Trigger();
                //Thread.Sleep(2000);
                //K2401.WaitForComplete();

               
                double[] volts = K2401.FetchRealData();
                double[] pdCurrs = K6485.FetchRealData();
                K2401.IsOutputOn = false;
                //PD校准系数 应该从主配置文件中导入的*************

                //每个电流点对应的 电压和光功率
                for (int i = 0; i < currs.Length; i++)
                {
                    RawData.Add(new RawDatumItem_LIV()
                    {
                        Current_mA = currs[i] * 1000.0,
                        Voltage_V = volts[i],
                        Power_mW = pdCurrs[i]
                    });
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}