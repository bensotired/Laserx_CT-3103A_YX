using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    //[Recipe]
    public class TestRecipe_FarField : TestRecipeBase
    {
        public TestRecipe_FarField()
        {
            Zero_Position = "FarField零点";
            //this.ProductModel = Product.DFB;
            this.Compliance_V = 2.5;
            this.Current_Base = CurrentBase.Null;
            this.Current_mA = 10;
            this.CurrentSenseRange_mA = 10;
            //this.NPLC = 1;
            this.Trajspeed = 2;
            this.Analog_CH = 3;

            this.Start_Angle = -20.0;
            this.Stop_Angle = 20.0;

            this.MovingSmooth_Angle = 0.05;
        }

        [DisplayName("扫描臂逻辑零点位名称")]
        [PropEditable(true)]
        public string Zero_Position { get; set; }

        //[DisplayName("产品型号")]
        //[Description("ProductModel")]
        //[PropEditable(true)]
        //public Product ProductModel { get; set; }

        [DisplayName("模拟量通道号")]
        [Description("Analog_CH")]
        [PropEditable(true)]
        public int Analog_CH { get; set; }

        [DisplayName("插补速度(mm/s)")]
        [Description("Trajspeed")]
        [PropEditable(true)]
        public double Trajspeed { get; set; }

        [DisplayName("水平起始角(°)")]
        [Description("Arm_H_Start_Angle")]
        [PropEditable(true)]
        public double Start_Angle { get; set; }

        [DisplayName("水平停止角(°)")]
        [Description("Arm_H_Stop_Angle")]
        [PropEditable(true)]
        public double Stop_Angle { get; set; }

        //[DisplayName("NPLC")]
        //[Description("NPLC")]
        //[PropEditable(true)]
        //public double NPLC { get; set; }

        [DisplayName("加电基数")]
        [Description("CurrentBase")]
        [PropEditable(true)]
        public CurrentBase Current_Base { get; set; }

        [DisplayName("电流(mA)")]
        [Description("Current_mA")]
        [PropEditable(true)]
        public double Current_mA { get; set; }

        [DisplayName("PD电流测量范围(mA)")]
        [Description("CurrentSenseRange_mA")]
        [PropEditable(true)]
        public double CurrentSenseRange_mA { get; set; }

        [DisplayName("限制电压(V)")]
        [Description("Compliance_V")]
        [PropEditable(true)]
        public double Compliance_V { get; set; }

        [DisplayName("移动平滑角度窗口(DegC)")]
        [Description("MovingSmooth_Angle")]
        [PropEditable(true)]
        public double MovingSmooth_Angle { get; set; }

        //public enum Product
        //{
        //    DFB,
        //    VCSEL
        //}

        public enum CurrentBase
        {
            Null,
            Ith1,
            Ith2,
            Ith3,
            Pop
        }
    }
}