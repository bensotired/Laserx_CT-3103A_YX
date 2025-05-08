using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    public class TestRecipe_AlignmentDemo : TestRecipeBase
    {
        public TestRecipe_AlignmentDemo()
        {
            this.Temp = 25;
            this.TempWait = 3;
            this.ChWait = 500;
            this.UpperTemp = 30;
            //this.DrivingCurrent_mA = "5,6,7,8,9";
            this.convert = true;
            this.StartCurrent_mA = 0.0;
            this.StopCurrent_mA = 100.0;
            this.StepCurrent_mA = 1.0;
            this.CompliaceVoltage_V = 2.5;
            this.PdBiasVoltage_V = 0;
            this.PdComplianceCurrent_mA = 20;
            this.K2400_NPLC = 0;

        }
        [DisplayName("控温℃")]
        [PropEditable(true)]
        public double Temp { get; set; }
        [DisplayName("控温等待时间_S")]
        [PropEditable(true)]
        public int TempWait { get; set; }
        [DisplayName("切换通道等待时间_ms")]
        [PropEditable(true)]
        public int ChWait { get; set; }
        [DisplayName("极限温度")]
        [PropEditable(true)]
        public double UpperTemp { get; set; }
        //[DisplayName("器件驱动电流(mA)——单步加电")]
        //[PropEditable(true)]
        //public string DrivingCurrent_mA { get; set; }

        [DisplayName("加电模式转换（true-平扫，false-单步)")]
        [PropEditable(true)]
        public bool convert { get; set; }
        [DisplayName("器件驱动起始电流(mA)——平扫加电")]
        [PropEditable(true)]
        public double StartCurrent_mA { get; set; }

        [DisplayName("器件驱动步进电流(mA)——平扫加电")]
        [PropEditable(true)]
        public double StepCurrent_mA { get; set; }

        [DisplayName("器件驱动结束电流(mA)——平扫加电")]
        [PropEditable(true)]
        public double StopCurrent_mA { get; set; }

        [DisplayName("LD偏置电压上限")]
        [PropEditable(true)]
        public double CompliaceVoltage_V { get; set; }

        [DisplayName("PD偏置电压目标值 ")]
        [PropEditable(true)]
        public double PdBiasVoltage_V { get; set; }

        [DisplayName("1通道PD光电流上限 ")]
        [PropEditable(true)]
        public double PdComplianceCurrent_mA { get; set; }
        [DisplayName("交流电一个周期是50HZ 1代表一个完整的1HZ  为两点间采集时间2ms 可配置1,0.1,0.01")]
        [Description("K2400_NPLC")]
        [PropEditable(true)]
        public double K2400_NPLC { get; set; }
    }
}