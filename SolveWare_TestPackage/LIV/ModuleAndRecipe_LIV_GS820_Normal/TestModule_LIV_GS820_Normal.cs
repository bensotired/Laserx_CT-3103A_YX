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
     (
         "TestCalculator_LIV_VF",
         "TestCalculator_LIV_Iop",
         "TestCalculator_LIV_Vop",
         "TestCalculator_LIV_Ith1",
         "TestCalculator_LIV_Ith2",
         "TestCalculator_LIV_Ith3",
         "TestCalculator_LIV_Kink_Current",
         "TestCalculator_LIV_Kink_Percentage",
         "TestCalculator_LIV_Kink_Power",
         "TestCalculator_LIV_Pout",
         "TestCalculator_LIV_Rs",
         "TestCalculator_LIV_Rs_2Point",
         "TestCalculator_LIV_SE_mWpermA",
         "TestCalculator_LIV_SE_mWpermW"
     )]
    [ConfigurableInstrument("ISourceMeter_GS820", "SourceMeter_GS820", "用于LIV扫描")]
    public class TestModule_LIV_GS820_Normal : TestModuleBase
    {

        public TestModule_LIV_GS820_Normal() : base()
        {
        }

        ISourceMeter_GS820 SourceMeter_GS820 { get { return (ISourceMeter_GS820)this.ModuleResource["SourceMeter_GS820"]; } }

        TestRecipe_LIV_GS820_Normal TestRecipe { get; set; }
        RawData_LIV RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_LIV_GS820_Normal);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_LIV();
            return RawData;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_LIV_GS820_Normal>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                //根据1100A
                this.Log_Global($"开始测试!");
                //这里还没有实例化  需要制作方法得到smu的实例

                if (SourceMeter_GS820 == null)
                {
                    return;
                }
                //this.SourceMeter_GS820.Reset();//下面方法中有

                double startValue = this.TestRecipe.I_Start_mA / 1000;
                double stopValue = this.TestRecipe.I_Stop_mA / 1000;
                double stepValue = this.TestRecipe.I_Step_mA / 1000;
                double complianceVoltage_V = this.TestRecipe.complianceVoltage_V;
                double PDBiasVoltage_V = this.TestRecipe.PDBiasVoltage_V;
                double PDComplianceCurrent_A = this.TestRecipe.PDComplianceCurrent_mA / 1000;
                double Period_s = this.TestRecipe.Period_ms / 1000;
                double SourceDelay_s = this.TestRecipe.SourceDelay_ms / 1000;
                double SenseDelay_s = this.TestRecipe.SenseDelay_ms / 1000;
                double GS820_NPLC_s = this.TestRecipe.GS820_NPLC_ms / 1000;
                bool PulsedMode = false;
                double DutyRatio = double.NaN;

                this.Log_Global($"Start TestModule_LIV_GS820_Normal sweep ...{this.TestRecipe.I_Start_mA}~{this.TestRecipe.I_Stop_mA}mA " +
                                      $"step  {this.TestRecipe.I_Step_mA}mA");//.SweepDualChannelsSYNC
                var cRaw = SourceMeter_GS820.SweepDualChannelsSYNC
                                      (startValue,//配置参数的开始电流 0.001
                                       stopValue,//配置参数的结束电流   1.2
                                       stepValue,//配置参数的步进电流    0.001
                                       complianceVoltage_V,//配置参数的限压 2.5
                                       PDBiasVoltage_V,
                                       PDComplianceCurrent_A,
                                       Period_s,//粗扫定时器周期秒    默认值0.005
                                       SourceDelay_s, //粗扫源延迟秒      默认值0.001
                                       SenseDelay_s,//粗扫测量延迟秒    默认值0.001
                                       GS820_NPLC_s / 0.02,//粗略扫描NPLC  默认是0.02秒一个扫描周期   默认值0.001
                                       PulsedMode,
                                       DutyRatio);//粗扫占空比  50/100*0.005
                this.Log_Global("TestModule_LIV_GS820_Normal sweep finished..");
                //PD校准系数 应该从主配置文件中导入的*************
                this.SourceMeter_GS820.Reset();


                var sour1curr = cRaw[0].Split(',');
                var sour1volt = cRaw[1].Split(',');
                var sour2curr = cRaw[2].Split(',');
                var sour2volt = cRaw[3].Split(',');

                double SphereResponsivity = 1;
                double PowerOffset_mW = 0;

                if (sour1curr.Length == sour1volt.Length && sour1curr.Length == sour2curr.Length && sour1curr.Length == sour2volt.Length)
                {
                    var len = sour1curr.Length;
                    for (int i = 0; i < len; i++)
                    {
                        RawData.Add(new RawDatumItem_LIV()
                        {
                            Current_mA = Convert.ToDouble(sour1curr[i]) * 1000,
                            Voltage_V = Convert.ToDouble(sour1volt[i]),
                            PDCurrent_mA = Convert.ToDouble(sour2curr[i]) * 1000,
                            PDVoltage_V = Convert.ToDouble(sour2volt[i]),
                            Power_mW = (Convert.ToDouble(sour2curr[i]) * 1000 * SphereResponsivity + PowerOffset_mW)
                        });
                    }
                }


            }
            catch (Exception ex)
            {
            }
        }


    }
}