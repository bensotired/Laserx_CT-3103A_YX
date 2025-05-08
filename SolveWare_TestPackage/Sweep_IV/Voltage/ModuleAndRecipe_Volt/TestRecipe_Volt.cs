using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_Volt : TestRecipeBase
    {
        public TestRecipe_Volt()
        {
            this.Section = Section_Volt.GAIN;
            this.Start_V = -5F;
            this.Stop_V = 1.15F;
            this.Step_V = 0.1F;

            this.complianceCurrent_mA = 3.15F;

            this.CurrentAutoRange = true;

            //this.IsFourWireOn = true;
        }


        [DisplayName("扫描Section")]
        [Description("Section")]
        [PropEditable(true)]
        public Section_Volt Section { get; set; }

        [DisplayName("扫描起始电流(V)")]
        [Description("I_Start_mA")]
        [PropEditable(true)]
        public float Start_V { get; set; }

        [DisplayName("扫描结束电流(V)")]
        [Description("I_Stop_mA")]
        [PropEditable(true)]
        public float Stop_V { get; set; }

        [DisplayName("扫描步进电流(V)")]
        [Description("I_Step_mA")]
        [PropEditable(true)]
        public float Step_V { get; set; }

        [DisplayName("限制电流")]
        [Description("complianceCurrent_mA")]
        [PropEditable(true)]
        public float complianceCurrent_mA { get; set; }

        [DisplayName("CurrentAutoRange")]
        [Description("CurrentAutoRange")]
        [PropEditable(true)]
        public bool CurrentAutoRange { get; set; }

        //[DisplayName("IsFourWireOn")]
        //[Description("IsFourWireOn")]
        //[PropEditable(true)]
        //public bool IsFourWireOn { get; set; }
    

    }
}