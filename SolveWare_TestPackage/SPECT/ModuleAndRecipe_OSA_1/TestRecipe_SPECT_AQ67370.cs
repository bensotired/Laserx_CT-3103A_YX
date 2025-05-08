using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_SPECT_AQ67370 : TestRecipeBase
    {
        public TestRecipe_SPECT_AQ67370()
        {
            //FiberAlignment = false;

            //SourceChannel = Keithley2602BChannel.CHA;
            strBiasCurrentList_A = 1;
            BiasBeforeWait_ms = 0;
            BiasAfterWait_ms = 0;
            complianceVoltage_V = 2.5;


            CenterWavelength_nm = 1577;
            WavelengthSpan_nm = 20;
            OsaRbw_nm = 0.07;
            OsaTraceLength = 1001;
            SmsrModeDiff_dB = 2.5;
            SMSRMask_nm = 0.5;
            OsaTrace = "TRA";
            Sensitivity = YokogawaAQ6370SensitivityModes.Mid;
        }

        //[DisplayName("true：由FiberAlignment模块确定精确测试位置并运动至该位置" +
        //     "false：测试模块自动运动至预设测试位置")]
        //[Description("SourceChannel")]
        //[PropEditable(true)]
        //public bool FiberAlignment { get; set; }



        //[DisplayName("源表LD加电通道")]
        //[Description("SourceChannel")]
        //[PropEditable(true)]
        //public Keithley2602BChannel SourceChannel { get; set; }

        [DisplayName("驱动电流（A）")]
        [Description("strBiasCurrentList_A")]
        [PropEditable(true)]
        public double strBiasCurrentList_A { get; set; }


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
        public double CenterWavelength_nm { get; set; }


        [DisplayName("扫描范围")]
        [Description("WavelengthSpan_nm")]
        [PropEditable(true)]
        public double WavelengthSpan_nm { get; set; }


        [DisplayName("分辨率")]
        [Description("OsaRbw_nm")]
        [PropEditable(true)]
        public double OsaRbw_nm { get; set; }


        [DisplayName("采集点的数目")]
        [Description("OsaTraceLength")]
        [PropEditable(true)]
        public int OsaTraceLength { get; set; }

        [DisplayName("主峰和次峰最小相隔的功率差")]
        [Description("SmsrModeDiff_dB")]
        [PropEditable(true)]
        public double SmsrModeDiff_dB { get; set; }


        [DisplayName("主峰和次峰最小相隔的距离")]
        [Description("SMSRMask_nm")]
        [PropEditable(true)]
        public double SMSRMask_nm { get; set; }







        [DisplayName("光谱仪的曲线类型")]
        [Description("OsaTrace")]
        [PropEditable(true)]
        public string OsaTrace { get; set; }



        [DisplayName("SensitivityModes")]
        [Description("Sensitivity")]
        [PropEditable(true)]
        public YokogawaAQ6370SensitivityModes Sensitivity { get; set; }

    }
}