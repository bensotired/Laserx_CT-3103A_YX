using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_LIV_SOA : TestRecipeBase
    {
        public TestRecipe_LIV_SOA()
        {
            this.Inherit = true;

            this.I_Start_A = 0.0F;
            this.I_Stop_A = 120F;
            this.I_Step_A = 1F;

            this.complianceVoltage_V = 2.5f;

            this.SOAVoltage_V = -1;
            this.SOA_ComplianceCurrent_mA = 10;
            this.ApertureTime_s = 0.001f;

            //OpticalSwitchChannel = 2;
        }
        [DisplayName("从QWLT2获取数值")]
        [Description("Inherit")]
        [PropEditable(true)]
        public bool Inherit { get; set; }

        [DisplayName("扫描起始电流(A)")]
        [Description("I_Start_A")]
        [PropEditable(true)]
        public float I_Start_A { get; set; }
 
        [DisplayName("扫描结束电流(A)")]
        [Description("I_Stop_A")]
        [PropEditable(true)]
        public float I_Stop_A { get; set; }
 
        [DisplayName("扫描步进电流(A)")]
        [Description("I_Step_A")]
        [PropEditable(true)]
        public float I_Step_A { get; set; }

        [DisplayName("限制电压")]
        [Description("complianceVoltage_V")]
        [PropEditable(true)]
        public float complianceVoltage_V { get; set; }

        [DisplayName("SOA电压")]
        [Description("SOAVoltage_V")]
        [PropEditable(true)]
        public float SOAVoltage_V { get; set; }

        [DisplayName("SOA限制电流")]
        [Description("SOA_BiasVoltage_V")]
        [PropEditable(true)]
        public float SOA_ComplianceCurrent_mA { get; set; }

        [DisplayName("ApertureTime_s")]
        [Description("ApertureTime_s")]
        [PropEditable(true)]
        public float ApertureTime_s { get; set; }

        //[DisplayName("OSwitch光开关通道")]
        //[Description("OpticalSwitchChannel")]
        //[PropEditable(true)]
        //public int OpticalSwitchChannel { get; set; }

    }
}