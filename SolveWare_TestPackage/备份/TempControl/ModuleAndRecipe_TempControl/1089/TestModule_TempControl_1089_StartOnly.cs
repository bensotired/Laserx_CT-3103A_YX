using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SolveWare_TestPackage
{
    [ConfigurableInstrument("MeerstetterTECController_1089", "TEC", "用于控温")]
    public class TestModule_TempControl_1089_StartOnly : TestModuleBase
    {
        public TestModule_TempControl_1089_StartOnly() : base()
        {
        }

        MeerstetterTECController_1089 TEC_1089 { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_1"]; } }

        private TestRecipe_TempControl_1089 TestRecipe { get; set; }

        private RawData_TempControl RawData { get; set; }

        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_TempControl(); return RawData;
        }

        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_TempControl_1089);
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_TempControl_1089>(testRecipe);
        }

        public override void Run(CancellationToken token)
        {
            try
            {
                this.Log_Global($"开始开启控温(仅启动)流程.");
                if (TestRecipe.EnableTEC)
                {
                    //控温
                    TEC_1089.TemperatureSetPoint_DegreeC = TestRecipe.Temperature;
                    TEC_1089.MinTimeInWindow_Sec = (int)TestRecipe.TempControl_StabilizingTime_sec;
                    TEC_1089.TemperatureDeviationDegreeC = TestRecipe.TempControl_Tolerance;
                    TEC_1089.IsOutputEnabled = true;

                    Log_Global("开启控温(仅启动)完成...");
                }
                else
                {
                    TEC_1089.IsOutputEnabled = false;
                }
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    throw ex;
                }
                else
                {
                    this.ReportException($"开启控温(仅启动)流程出现异常", ErrorCodes.Module_TempControl_Failed, ex);
                    throw ex;
                }
            }
        }
    }
}