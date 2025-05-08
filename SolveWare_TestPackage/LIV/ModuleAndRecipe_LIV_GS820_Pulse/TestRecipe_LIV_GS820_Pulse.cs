using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    //[Recipe]
    public class TestRecipe_LIV_GS820_Pulse : TestRecipeBase
    {

        public TestRecipe_LIV_GS820_Pulse()
        {
            this.I_Start_mA = 0.0;
            this.I_Stop_mA = 100.0;
            this.I_Step_mA = 1.0;
            this.MasterSourceComplianceVoltage_V = 2.5;
            this.MasterSourceMeasureRangeVoltage_V = 2.5;
            this.PDBiasVoltage_V = 1;
            this.PDComplianceCurrent_mA = 20;
            this.Period_ms = 5;
            this.SourceDelay_ms = 0.1;
            this.SenseDelay_ms = 0;
            this.GS820_NPLC_ms = 1;
            this.PulsedMode = true;
            this.DutyRatio = 50;
        }

        [DisplayName("扫描起始电流(mA)")]
        [Description("I_Start_mA")]
        [PropEditable(true)]
        public double I_Start_mA { get; set; }

        [DisplayName("扫描结束电流(mA)")]
        [Description("I_Stop_mA")]
        [PropEditable(true)]
        public double I_Stop_mA { get; set; }

        [DisplayName("扫描步进电流(mA)")]
        [Description("I_Step_mA")]
        [PropEditable(true)]
        public double I_Step_mA { get; set; }



        [DisplayName("通道1限制电压")]
        [Description("MasterSourceComplianceVoltage_V")]
        [PropEditable(true)]
        public double MasterSourceComplianceVoltage_V { get; set; }


        [DisplayName("通道1电压测量量程")]
        [Description("MasterSourceMeasureRangeVoltage_V")]
        [PropEditable(true)]
        public double MasterSourceMeasureRangeVoltage_V { get; set; }

        [DisplayName("PD偏置电压")]
        [Description("PDBiasVoltage_V")]
        [PropEditable(true)]
        public double PDBiasVoltage_V { get; set; }

        [DisplayName("通道2电流测量量程")]
        [Description("MasterSourceMeasureRangeVoltage_V")]
        [PropEditable(true)]
        public double SlaveSourceMeasureRangeCurrent_mA { get; set; }
        [DisplayName("PD限制电流")]
        [Description("PDComplianceCurrent_mA")]
        [PropEditable(true)]
        public double PDComplianceCurrent_mA { get; set; }

        [DisplayName("粗扫定时器周期 毫秒")]
        [Description("Period_ms")]
        [PropEditable(true)]
        public double Period_ms { get; set; }


        [DisplayName("扫描源延迟 毫秒")]
        [Description("SourceDelay_ms")]
        [PropEditable(true)]
        public double SourceDelay_ms { get; set; }



        [DisplayName("扫描源测量延迟 毫秒")]
        [Description("SenseDelay_ms")]
        [PropEditable(true)]
        public double SenseDelay_ms { get; set; }


        [DisplayName("交流电一个周期是50HZ 1代表一个完整的1HZ  为两点间采集时间2ms 可配置1,10,100")]
        [Description("GS820_NPLC_ms")]
        [PropEditable(true)]
        public double GS820_NPLC_ms { get; set; }


        [DisplayName("是否启用脉冲扫描模式")]
        [Description("PulsedMode")]
        [PropEditable(true)]
        public bool PulsedMode { get; set; }


        [DisplayName("扫描占空比")]
        [Description("DutyRatio")]
        [PropEditable(true)]
        public double DutyRatio { get; set; }

    }
}