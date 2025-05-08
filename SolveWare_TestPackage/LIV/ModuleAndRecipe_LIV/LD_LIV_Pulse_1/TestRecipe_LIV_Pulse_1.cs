using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_LIV_Pulse_1 : TestRecipeBase
    {
        public TestRecipe_LIV_Pulse_1()
        {

        }
        [DisplayName("脉冲宽度(us) 最小值5000")]
        [Description("Pulsewidth")]
        [PropEditable(true)]
        public int Pulsewidth { get; set; }
        [DisplayName("脉冲长度(us) 最小值10000")]
        [Description("Pulseperiod")]
        [PropEditable(true)]
        public int Pulseperiod { get; set; }


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

        [DisplayName("限制电压")]
        [Description("complianceVoltage_V")]
        [PropEditable(true)]
        public double complianceVoltage_V { get; set; }

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
    }
}
