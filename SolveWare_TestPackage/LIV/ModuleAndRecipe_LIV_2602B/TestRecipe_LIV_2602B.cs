using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_LIV_2602B : TestRecipeBase
    {

        public TestRecipe_LIV_2602B()
        {
            this.I_Start_mA = 0.0;
            this.I_Stop_mA = 100.0;
            this.I_Step_mA = 1.0;
            this.complianceVoltage_V = 2.5;
            this.PDBiasVoltage_V = 0;
            this.PDComplianceCurrent_mA = 20;
            this.Period_ms = 5;
            this.pulseWidth_ms= 4;
            this.SourceDelay_ms = 0.1;
            this.SenseDelay_ms = 0.1;
            this.NPLC_ms = 1;
            this.PulsedMode = false;
            //this.DutyRatio = double.NaN;
            this.Power_Factor_K = 1;
            this.Power_Factor_B = 0;
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



        [DisplayName("限制电压")]
        [Description("complianceVoltage_V")]
        [PropEditable(true)]
        public double complianceVoltage_V { get; set; }


        [DisplayName("PD偏置电压")]
        [Description("PDBiasVoltage_V")]
        [PropEditable(true)]
        public double PDBiasVoltage_V { get; set; }

        [DisplayName("PD限制电流&量程_可配置0.01,0.1,1,10,100")]
        [Description("PDComplianceCurrent_mA")]
        [PropEditable(true)]
        public double PDComplianceCurrent_mA { get; set; }

        [DisplayName("粗扫定时器周期 毫秒")]
        [Description("Period_ms")]
        [PropEditable(true)]
        public double Period_ms { get; set; }        

        [DisplayName("粗扫脉冲宽度 毫秒(Pulse下有效)")]
        [Description("pulseWidth_ms")]
        [PropEditable(true)]
        public double pulseWidth_ms { get; set; }

        [DisplayName("扫描源延迟 毫秒")]
        [Description("SourceDelay_ms")]
        [PropEditable(true)]
        public double SourceDelay_ms { get; set; }



        [DisplayName("扫描源测量延迟 毫秒")]
        [Description("SenseDelay_ms")]
        [PropEditable(true)]
        public double SenseDelay_ms { get; set; }


        [DisplayName("交流电一个周期是50HZ 1代表一个完整的1HZ,为两点间采集时间2ms,可配置1,10,100")]
        [Description("2602B_NPLC_ms")]
        [PropEditable(true)]
        public double NPLC_ms { get; set; }


        [DisplayName("是否启用脉冲扫描模式")]
        [Description("PulsedMode")]
        [PropEditable(true)]
        public bool PulsedMode { get; set; }

        [DisplayName("单个电流点的脉冲个数(Pulse下有效)")]
        [Description("PulseCount")]
        [PropEditable(true)]
        public int PulseCount { get; set; } = 3;

        [DisplayName("设置PCE在第n电流前输出值为0)")]
        [Description("PCESort")]
        [PropEditable(true)]
        public int PCESort { get; set; } = 1; 
        //[DisplayName("扫描占空比")]
        //[Description("DutyRatio")]
        //[PropEditable(true)]
        //public double DutyRatio { get; set; }

        [DisplayName("LIV光功率K值(P = I * K + B)")]
        [PropEditable(true)]
        public double Power_Factor_K { get; set; }
        [DisplayName("LIV光功率B值(P = I * K + B)")]
        [PropEditable(true)]
        public double Power_Factor_B { get; set; }

    }
}