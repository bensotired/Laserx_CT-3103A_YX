using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
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



    public class TestModule_ChartDemo : TestModuleBase
    {
        //MeerstetterTECController_1089 TC_1 { get { return (MeerstetterTECController_1089)this.ModuleResource["TC_5"]; } }

        //RelayController RC_44 { get { return (RelayController)this.ModuleResource["RC_44"]; } }

        //MotorAxisBase X_Axis { get { return (MotorAxisBase)this.ModuleResource["导轨移动X轴"]; } }

        //AxesPosition X_Position
        //{
        //    get
        //    {
        //        var slotName = this.TestRecipe.Slot;
        //        var DutName = this.TestRecipe.Dut;
        //        string positionName = "Slot_1_Dut_1测试工位";
        //        if (slotName == SlotType.Slot_1 && DutName == DutType.Dut_1)
        //        {
        //            positionName = "Slot_1_Dut_1测试工位";
        //        }
        //        else if (slotName == SlotType.Slot_1 && DutName == DutType.Dut_2)
        //        {
        //            positionName = "Slot_1_Dut_2测试工位";
        //        }
        //        else if (slotName == SlotType.Slot_2 && DutName == DutType.Dut_1)
        //        {
        //            positionName = "Slot_2_Dut_1测试工位";
        //        }
        //        else if (slotName == SlotType.Slot_2 && DutName == DutType.Dut_2)
        //        {
        //            positionName = "Slot_2_Dut_2测试工位";
        //        }
        //        return (AxesPosition)this.ModuleResource[positionName];
        //    }
        //}

        //IOBase Output_Polaroid_Up { get { return (IOBase)this.ModuleResource["Output_偏振片上升"]; } }
        //IOBase Output_Polaroid_Down { get { return (IOBase)this.ModuleResource["Output_偏振片下降"]; } }

        //IOBase Output_PD_Front { get { return (IOBase)this.ModuleResource["Output_积分球前进"]; } }
        //IOBase Output_PD_Back { get { return (IOBase)this.ModuleResource["Output_积分球后退"]; } }


        TestRecipe_ChartDemo TestRecipe { get; set; }


        RawData_ChartDemo RawData { get; set; }



        public TestModule_ChartDemo() : base() { }


        

        


        public override Type GetTestRecipeType() { return typeof(TestRecipe_ChartDemo); }
        public override void Localization(ITestRecipe testRecipe) { TestRecipe = ConvertObjectTo<TestRecipe_ChartDemo>(testRecipe); }

        public override IRawDataBaseLite CreateRawData() { RawData = new RawData_ChartDemo(); return RawData; }


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

                for (int i = TestRecipe.X_LimitLower; i < TestRecipe.X_LimitUpper; i++)
                {
                    //RawDatumItem_ChartDemo temp = new RawDatumItem_ChartDemo()
                    //{
                    //    Value_X = i,
                    //    Value_Y1= GetRandomValue(TestRecipe.Y1_LimitLower, TestRecipe.Y1_LimitUpper),
                    //    Value_Y11 = GetRandomValue(TestRecipe.Y11_LimitLower, TestRecipe.Y11_LimitUpper),
                    //    Value_Y12 = GetRandomValue(TestRecipe.Y12_LimitLower, TestRecipe.Y12_LimitUpper),
                    //    Value_Y2 = GetRandomValue(TestRecipe.Y2_LimitLower, TestRecipe.Y2_LimitUpper),
                    //    Value_Y21 = GetRandomValue(TestRecipe.Y21_LimitLower, TestRecipe.Y21_LimitUpper),
                    //    Value_Y22 = GetRandomValue(TestRecipe.Y22_LimitLower, TestRecipe.Y22_LimitUpper),
                    //};

                    RawDatumItem_ChartDemo temp = new RawDatumItem_ChartDemo()
                    {
                        Value_X = i,
                        Value_Y1 = Math.Round(i * Math.PI * GetRandomValue(2, 5), 2),
                        Value_Y11 = Math.Round((i + 1) * Math.PI* GetRandomValue(1,10), 2),
                        Value_Y12 = Math.Round((i + 2) * Math.PI * GetRandomValue(-5, -1), 2),
                        Value_Y2 = Math.Round((i + 3) * Math.PI*GetRandomValue(10, 15), 2),
                        Value_Y21 = Math.Round((i + 4) * Math.PI * GetRandomValue(10, 20), 2),
                        Value_Y22 = Math.Round((i + 5) * Math.PI * GetRandomValue(15, 30), 2),
                    };


                    this.RawData.Add(temp);
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