using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    public class TestRecipe_SPECT_ARIS800 : TestRecipeBase
    {
        public TestRecipe_SPECT_ARIS800()
        {
            FiberAlignment = false;

            SourceChannel = Keithley2602BChannel.CHA;
            strBiasCurrentList_mA = "30";
            complianceVoltage_V = 2.5;
            BiasBeforeWait_ms = 0;
            BiasAfterWait_ms = 0;


            BoxcarWidth_nm = 1577;
            ScansToAverage_nm = 20;

            IntegrationTime_ms = 1000;

        }

        [DisplayName("true：由FiberAlignment模块确定精确测试位置并运动至该位置" +
             "false：测试模块自动运动至预设测试位置")]
        [Description("SourceChannel")]
        [PropEditable(true)]
        public bool FiberAlignment { get; set; }



        [DisplayName("源表LD加电通道")]
        [Description("SourceChannel")]
        [PropEditable(true)]
        public Keithley2602BChannel SourceChannel { get; set; }

        [DisplayName("测试电流点集合(多个电流点以“,”相隔开)")]
        [Description("strBiasCurrentList_mA")]
        [PropEditable(true)]
        public string strBiasCurrentList_mA { get; set; }


        [DisplayName("加电前等待时间(ms)")]
        [Description("BiasBeforeWait_ms")]
        [PropEditable(true)]
        public int BiasBeforeWait_ms { get; set; }

        [DisplayName("加电后等待时间(ms)，默认至少500ms")]
        [Description("BiasAfterWait_ms")]
        [PropEditable(true)]
        public int BiasAfterWait_ms { get; set; }


        [DisplayName("加电过程中的保护电压(V)")]
        [Description("complianceVoltage_V")]
        [PropEditable(true)]
        public double complianceVoltage_V { get; set; }







        [DisplayName("芯片中心波长")]
        [Description("CenterWavelength_nm")]
        [PropEditable(true)]
        public int BoxcarWidth_nm { get; set; }


        [DisplayName("扫描范围")]
        [Description("WavelengthSpan_nm")]
        [PropEditable(true)]
        public int ScansToAverage_nm { get; set; }


        [DisplayName("扫描时间")]
        [Description("IntegrationTime_ms")]
        [PropEditable(true)]
        public int IntegrationTime_ms { get; set; }



    }
}