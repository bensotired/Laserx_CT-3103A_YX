using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    public class TestRecipe_Coupling_LD : TestRecipeBase
    {
        public TestRecipe_Coupling_LD()
        {
            UsedCurrentPos = false;
            Position = "Coupling_LD开始点";
            //ProductModel = Product.DFB;
            Analog_CH = 3;
            Current_mA = 3;
            Compliance_V = 5;
            Rough_Trajspeed = 2;
            Fine_Trajspeed = 2;
            Rough_Radius = 0.2;
            Rough_Involute_Interval = 0.005;
            Rough_Enable = true;

            Fine_Radius = 0.02;
            Fine_Involute_Interval = 0.001;

            CrossPoint_Interval = 0.0005;
            CrossPoint_Times = 2;
            CrossPoint_DelayBeforeMeasure_ms = 10;

            //Cross_Width = 0.1;
            //Cross_Times = 3;
            Layer_Range = 0.4;
            Layer_Step = 0.1;
            Power_Threshold = 1.0;
            CurrentSenseRange_mA = 0.01;
            SaveDebugFiles = false;
            ScanDirByLayerPower = 2;   //扫描几层才能认定方向

        }

        [DisplayName("是否从当前位置(忽略初始点位)开始耦合")]
        [PropEditable(true)]
        public bool UsedCurrentPos { get; set; }

        [DisplayName("点位名称")]
        [PropEditable(true)]
        public string Position { get; set; }


        //[DisplayName("产品型号")]
        //[Description("ProductModel")]
        //[PropEditable(true)]
        //public Product ProductModel { get; set; }

        [DisplayName("产品电流(mA)")]
        [Description("Current_mA")]
        [PropEditable(true)]
        public double Current_mA { get; set; }

        [DisplayName("保护电压(V)")]
        [Description("Compliance_V")]
        [PropEditable(true)]
        public double Compliance_V { get; set; }

        [DisplayName("粗扫插补速度(mm/s)")]
        [Description("Rough_Trajspeed")]
        [PropEditable(true)]
        public double Rough_Trajspeed { get; set; }

        [DisplayName("精扫插补速度(mm/s)")]
        [Description("Fine_Trajspeed")]
        [PropEditable(true)]
        public double Fine_Trajspeed { get; set; }

        [DisplayName("粗扫半径(mm)")]
        [Description("Rough_Radius")]
        [PropEditable(true)]
        public double Rough_Radius { get; set; }

        [DisplayName("粗扫线间隔(mm)")]
        [Description("Rough_Involute_Interval")]
        [PropEditable(true)]
        public double Rough_Involute_Interval { get; set; }

        [DisplayName("精扫半径(mm)")]
        [Description("Fine_Radius")]
        [PropEditable(true)]
        public double Fine_Radius { get; set; }

        [DisplayName("精扫线间隔(mm)(!应小于等于产品收光半径!)")]
        [Description("Fine_Involute_Interval")]
        [PropEditable(true)]
        public double Fine_Involute_Interval { get; set; }

        [DisplayName("粗扫是否执行)")]
        [Description("Rough_Enable")]
        [PropEditable(true)]
        public bool Rough_Enable { get; set; }


        [DisplayName("十字搜索点间隔(mm)")]
        [Description("CrossPoint_Interval")]
        [PropEditable(true)]
        public double CrossPoint_Interval { get; set; }

        [DisplayName("十字搜索点扫描轮数")]
        [Description("CrossPoint_Times")]
        [PropEditable(true)]
        public int CrossPoint_Times { get; set; }

        [DisplayName("十字搜索点读数前等待")]
        [Description("CrossPoint_DelayBeforeMeasure_ms")]
        [PropEditable(true)]
        public int CrossPoint_DelayBeforeMeasure_ms { get; set; }

        //[DisplayName("十字扫描宽度(mm)")]
        //[Description("Cross_Width")]
        //[PropEditable(true)]
        //public double Cross_Width { get; set; }

        //[DisplayName("十字扫描轮数")]
        //[Description("Cross_Width")]
        //[PropEditable(true)]
        //public int Cross_Times { get; set; }

        [DisplayName("搜索寻光范围(mm)")]
        [Description("Layer_Range")]
        [PropEditable(true)]
        public double Layer_Range { get; set; }

        [DisplayName("寻光层步长(mm)")]
        [Description("Layer_Step")]
        [PropEditable(true)]
        public double Layer_Step { get; set; }

        [DisplayName("寻光阈值")]
        [Description("Power_Threshold")]
        [PropEditable(true)]
        public double Power_Threshold { get; set; }

        [DisplayName("模拟量通道号")]
        [Description("Analog_CH")]
        [PropEditable(true)]
        public int Analog_CH { get; set; }

        [DisplayName("PD电流测量范围(mA)")]
        [Description("CurrentSenseRange_mA")]
        [PropEditable(true)]
        public double CurrentSenseRange_mA { get; set; }

        [DisplayName("是否保存耦合数据")]
        [Description("SaveDebugFiles")]
        [PropEditable(true)]
        public bool SaveDebugFiles { get; set; }

        [DisplayName("使用几个层数据确认搜索方向")]
        [Description("ScanDirByLayerPower")]
        [PropEditable(true)]
        public int ScanDirByLayerPower { get; set; }

        //public enum Product
        //{
        //    DFB,
        //    VCSEL
        //}
    }
}