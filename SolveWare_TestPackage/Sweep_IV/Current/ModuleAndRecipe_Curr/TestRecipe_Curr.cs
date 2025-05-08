using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_Curr : TestRecipeBase
    {
        public TestRecipe_Curr()
        {
            this.Section = Section_Curr.GAIN;
            this.I_Start_mA = 0.0F;
            this.I_Stop_mA = 3.0F;
            this.I_Step_mA = 0.1F;

            this.complianceVoltage_V = 2F;

            this.ApertureTime_s = 9;

            //this.IsFourWireOn = true;
        }


        [DisplayName("扫描Section")]
        [Description("Section")]
        [PropEditable(true)]
        public Section_Curr Section { get; set; }

        [DisplayName("扫描起始电流(mA)")]
        [Description("I_Start_mA")]
        [PropEditable(true)]
        public float I_Start_mA { get; set; }

        [DisplayName("扫描结束电流(mA)")]
        [Description("I_Stop_mA")]
        [PropEditable(true)]
        public float I_Stop_mA { get; set; }

        [DisplayName("扫描步进电流(mA)")]
        [Description("I_Step_mA")]
        [PropEditable(true)]
        public float I_Step_mA { get; set; }

        [DisplayName("限制电压")]
        [Description("complianceVoltage_V")]
        [PropEditable(true)]
        public float complianceVoltage_V { get; set; }

        [DisplayName("ApertureTime_s")]
        [Description("ApertureTime_s")]
        [PropEditable(true)]
        public float ApertureTime_s { get; set; }

        //[DisplayName("IsFourWireOn")]
        //[Description("IsFourWireOn")]
        //[PropEditable(true)]
        //public bool IsFourWireOn { get; set; }
    

    }

}