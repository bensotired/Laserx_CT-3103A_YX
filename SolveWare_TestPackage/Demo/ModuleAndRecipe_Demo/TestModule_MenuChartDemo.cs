using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TestPlugin_Demo;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
    (
        "TestCalculator_Demo"
    )]

    //[ConfigurableInstrument("MeerstetterTECController_1089", "TC_5", "用于提供产品内部测试温度")]
    //[ConfigurableInstrument("RelayController", "RC_44", "用于切换硬件测试线路")]

    //[StaticResource(ResourceItemType.AXIS, "导轨移动X轴", "导轨移动X轴")]
    //[StaticResource(ResourceItemType.POS, "Slot_1_Dut_1测试工位", "Slot_1_Dut_1测试工位")]
    //[StaticResource(ResourceItemType.POS, "Slot_1_Dut_2测试工位", "Slot_1_Dut_2测试工位")]
    //[StaticResource(ResourceItemType.POS, "Slot_2_Dut_1测试工位", "Slot_2_Dut_1测试工位")]
    //[StaticResource(ResourceItemType.POS, "Slot_2_Dut_2测试工位", "Slot_2_Dut_2测试工位")]

    //[StaticResource(ResourceItemType.IO, "Output_偏振片上升", "偏振片上")]
    //[StaticResource(ResourceItemType.IO, "Output_偏振片下降", "偏振片下")]
    //[StaticResource(ResourceItemType.IO, "Output_积分球前进", "积分球前")]
    //[StaticResource(ResourceItemType.IO, "Output_积分球后退", "积分球后")]



    public class TestModule_MenuChartDemo : TestModuleBase
    {


        TestRecipe_ChartDemo TestRecipe { get; set; }




        RawDataMenuCollection<RawData_MenuChartDemo> RawData { get; set; }



        public TestModule_MenuChartDemo() : base() { }


       

       

        public override Type GetTestRecipeType() { return typeof(TestRecipe_ChartDemo); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_ChartDemo>(testRecipe); }

        public override IRawDataBaseLite CreateRawData() { RawData =new RawDataMenuCollection<RawData_MenuChartDemo>(); return RawData; }


        public override void Run(CancellationToken token)
        {
            try
            {
                this.Log_Global($"开始测试!");
                if (token.IsCancellationRequested)
                {
                    this.Log_Global($"用户取消测试！");
                    token.ThrowIfCancellationRequested();
                }

                double[] array = new double[] { 100, 300, 600 };
                for (int i = 0; i < array.Length; i++)
                {
                    RawData_MenuChartDemo rawData_MenuChartDemo = new RawData_MenuChartDemo();
                    for (int j = TestRecipe.X_LimitLower; j < TestRecipe.X_LimitUpper; j++)
                    {
                        RawDatumItem_MenuChartDemo temp = new RawDatumItem_MenuChartDemo()
                        {
                            Value_X = j,
                            Value_Y1 = Math.Round(j*(i+1) * Math.PI * GetRandomValue(2, 5), 2),
                            //Value_Y2 = Math.Round((j*(i+1) + 3) * Math.PI * GetRandomValue(10, 15), 2),
                        };
                        rawData_MenuChartDemo.Add(temp);                        
                    }
                    rawData_MenuChartDemo.LDDrivingCurrent_mA = array[i];
                    RawData.Add(rawData_MenuChartDemo);
                }
            }
            catch (Exception ex)
            {
                this._core.ReportException("耦合模块运行错误", ErrorCodes.TestModuleRuntimeExceptionRaised, ex);
            }
        }


        int GetRandomValue(int lower, int upper)
        {
            Random random = new Random();

            return random.Next(lower, upper);
        }


    }
}