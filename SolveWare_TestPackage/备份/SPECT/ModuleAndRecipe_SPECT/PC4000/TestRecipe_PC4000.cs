using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents;
using System.ComponentModel;
using SolveWare_BurnInCommon;

namespace SolveWare_TestPackage
{
    //[Recipe]
    public class TestRecipe_PC4000 : TestRecipeBase
    {

        public TestRecipe_PC4000()
        {
            IntegrationTime_ms = 200;
            BoxcarWidth= 1;
            ScansToAverage = 1;
            BiasCurrent_mA = 75;
            ComplianceVoltage_V = 10;
        }
        [DisplayName("积分时间")]
        [Description("IntegrationTime_ms")]
        [PropEditable(true)]
        public double IntegrationTime_ms { get; set; }

        [DisplayName("平滑窗口，窗口平滑算法中半宽像素值，最小值为0")]
        [Description("BoxcarWidth")]
        [PropEditable(true)]
        public int BoxcarWidth { get; set; }

        [DisplayName("采集的光谱曲线进行多次平均的次数，最小值是1")]
        [Description("ScansToAverage")]
        [PropEditable(true)]
        public int ScansToAverage { get; set; }


        [DisplayName("测试电流点")]
        [Description("BiasCurrent_mA")]
        [PropEditable(true)]
        public double BiasCurrent_mA { get; set; }

        [DisplayName("加电过程中的保护电压(V)")]
        [Description("ComplianceVoltage_V")]
        [PropEditable(true)]
        public double ComplianceVoltage_V { get; set; }

        
       

    }

}