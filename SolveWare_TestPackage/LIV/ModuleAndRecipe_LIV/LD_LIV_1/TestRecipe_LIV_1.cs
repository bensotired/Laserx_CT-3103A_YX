using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_LIV_1 : TestRecipeBase
    {
        public TestRecipe_LIV_1()
        {
            this.I_Start_A = 0.0;
            this.I_Stop_A = 3.0;
            this.I_Step_A = 0.1;

            //this.EnableEAOutput = false;
            //this.EAVoltage_V = 0;

            this.complianceVoltage_V = 2.5;

            //this.EAComplianceCurrent_mA = 150;

            this.PDBiasVoltage_V = 0;
            this.PDComplianceCurrent_mA = 20;

            this.K2400_NPLC = 0;
            //this.ReadMPD = false;
            //this.MPDBiasVoltage = 0;
            //this.MPDComplianceCurrent_mA = 1;
        }
 
        [DisplayName("扫描起始电流(A)")]
        [Description("I_Start_A")]
        [PropEditable(true)]
        public double I_Start_A { get; set; }
 
        [DisplayName("扫描结束电流(A)")]
        [Description("I_Stop_A")]
        [PropEditable(true)]
        public double I_Stop_A { get; set; }
 
        [DisplayName("扫描步进电流(A)")]
        [Description("I_Step_A")]
        [PropEditable(true)]
        public double I_Step_A { get; set; }


        //[DisplayName("是否启用EA输出")]
        //[Description("EnableEAOutput")]
        //[PropEditable(true)]
        //public bool EnableEAOutput { get; set; }


        //[DisplayName("EA电压")]
        //[Description("EAVoltage_V")]
        //[PropEditable(true)]
        //public double EAVoltage_V { get; set; }

        [DisplayName("限制电压")]
        [Description("complianceVoltage_V")]
        [PropEditable(true)]
        public double complianceVoltage_V { get; set; }

        //[DisplayName("EA限制电流")]
        //[Description("EAComplianceCurrent_mA")]
        //[PropEditable(true)]
        //public double EAComplianceCurrent_mA { get; set; }

        [DisplayName("PD偏置电压")]
        [Description("PDBiasVoltage_V")]
        [PropEditable(true)]
        public double PDBiasVoltage_V { get; set; }

        [DisplayName("PD限制电流")]
        [Description("PDBiasVoltage_V")]
        [PropEditable(true)]
        public double PDComplianceCurrent_mA { get; set; }

        [DisplayName("交流电一个周期是50HZ 1代表一个完整的1HZ  为两点间采集时间2ms 可配置1,0.1,0.01")]
        [Description("K2400_NPLC")]
        [PropEditable(true)]
        public double K2400_NPLC { get; set; }


        //[DisplayName("是否读取背光电流")]
        //[Description("ReadMPD")]
        //[PropEditable(true)]
        //public bool ReadMPD { get; set; }

        //[DisplayName("MPDBias电压")]
        //[Description("MPDBiasVoltage")]
        //[PropEditable(true)]
        //public double MPDBiasVoltage { get; set; }

        //[DisplayName("MPD限定电流")]
        //[Description("MPDComplianceCurrent_mA")]
        //[PropEditable(true)]
        //public double MPDComplianceCurrent_mA { get; set; }        

    }
}