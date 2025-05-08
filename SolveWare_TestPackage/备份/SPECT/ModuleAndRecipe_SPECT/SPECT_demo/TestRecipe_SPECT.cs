using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents;
using System.ComponentModel;
using SolveWare_BurnInCommon;

namespace SolveWare_TestPackage
{
    //[Recipe]
    public class TestRecipe_SPECT : TestRecipeBase
    {

        public TestRecipe_SPECT()
        {
            CenterWavelength_nm = 1577;
            WavelengthSpan_nm = 20;
            OsaRbw_nm = 0.07;
            OsaTraceLength = 1001;
            SmsrModeDiff_dB = 2.5;
            SMSRMask_nm = 0.5;
            strBiasCurrentList_mA = "30";
            BiasBeforeWait_ms = 0;
            BiasAfterWait_ms = 0;
            complianceVoltage_V = 2.5;
            OsaTrace = "TRA";
            En_EAOutput = false;
            EAVoltage_V = 0;
            EACurrentClampI_mA = -150;
        }
        

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

        [DisplayName("测试电流点集合")]
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

        [DisplayName("光谱仪的曲线类型")]
        [Description("OsaTrace")]
        [PropEditable(true)]
        public string OsaTrace { get; set; }

        [DisplayName("是否启用EA输出")]
        [Description("En_EAOutput")]
        [PropEditable(true)]
        public bool En_EAOutput { get; set; }

        [DisplayName("EA设置的电压(V)")]
        [Description("EAVoltage_V")]
        [PropEditable(true)]
        public double EAVoltage_V { get; set; }

        [DisplayName("EA保护电流(mA)")]
        [Description("EACurrentClampI_mA")]
        [PropEditable(true)]
        public double EACurrentClampI_mA { get; set; }

    }

}