using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    //[Recipe]
    public class TestRecipe_IR_GS820 : TestRecipeBase
    {

        public TestRecipe_IR_GS820()
        {
            this.Voltage_V = -5;
            this.ComplianceCurrent_A=0.1;
            this.senseCurrent_uA = 0.1;
        }

        [DisplayName("驱动电压")]
        [Description("Voltage_V")]
        [PropEditable(true)]
        public double Voltage_V { get; set; }

        [DisplayName("保护电流")]
        [Description("ComplianceCurrent_A")]
        [PropEditable(true)]
        public double ComplianceCurrent_A { get; set; }

        [DisplayName("响应电流量程")]
        [Description("senseCurrent_uA")]
        [PropEditable(true)]
        public double senseCurrent_uA { get; set; }
    }
}