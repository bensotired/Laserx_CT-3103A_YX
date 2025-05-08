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
    //[ConfigurableInstrument("MeerstetterTECController_1089", "TEC", "用于控温")]

    [ConfigurableInstrument("MeerstetterTECController_1089", "TC_1", "用于获取载台当前温度")]
    public class TestModule_TempControl_1089_WaitStable : TestModuleBase
    {
        public TestModule_TempControl_1089_WaitStable() : base()
        {
        }

        //private MeerstetterTECController_1089 TEC_1089
        //{ get { return (MeerstetterTECController_1089)this.ModuleResource[ResourceItemType.Instrument_Meter]["TEC"]; } }
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
                this.Log_Global($"开始控温流程.");
                if (TestRecipe.EnableTEC)
                {
                    //控温
                    var ab=TEC_1089.CurrentObjectTemperature;
                    TEC_1089.TemperatureSetPoint_DegreeC = TestRecipe.Temperature;
                    TEC_1089.MinTimeInWindow_Sec = (int)TestRecipe.TempControl_StabilizingTime_sec;
                    TEC_1089.TemperatureDeviationDegreeC = TestRecipe.TempControl_Tolerance;
                    TEC_1089.IsOutputEnabled = true;

                    //TEC_1089.TemperatureSetPoint_DegreeC = tempC;
                    //TEC_1089.IsOutputEnabled = true;

                    Thread.Sleep(3000);

                    Log_Global("检查控温温度稳定...");

                    Stopwatch swTemp = Stopwatch.StartNew();
                    while (true)
                    {
                        if (token.IsCancellationRequested)
                        {
                            Log_Global("用户取消测试");
                            token.ThrowIfCancellationRequested();
                            throw new OperationCanceledException();
                        }
                        double CurrentObjectTemp = TEC_1089.CurrentObjectTemperature;

                        bool isstable = true;

                        if(TEC_1089.isStable!=2)
                        {
                            isstable = false; //温度没有稳定
                        }

                        //20190409 温度稳定功能下放,设定参数
                        if (isstable == true) //温度已经稳定
                        {
                            Log_Global("温控完成...");
                            break;
                        }

                        if (swTemp.ElapsedMilliseconds / 1000 > TestRecipe.TempControl_Timeout)
                        {
                            Log_Global("温控超时...");
                            TEC_1089.IsOutputEnabled = false;
                            throw new Exception("温控超时");
                        }
                    }
                    swTemp.Stop();
                    swTemp.Reset();
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
                    this.ReportException($"控温流程出现异常", ErrorCodes.Module_TempControl_Failed, ex);
                    throw ex;
                }
            }
        }
    }
}