using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_PeaPer : TestRecipeBase
    {
        public TestRecipe_PeaPer()
        {
            this.PeakSearchStart_Deg=0;
            this.PeakSearchEnd_Deg=90;
            this.NullSearchStart_Deg=91;
            this.NullSearchEnd_Deg=180;
            this.StepAngle_Deg = 0.2;
            this.Power_Factor_K = 1;
            this.Power_Factor_B = 0;
        }

        //[DisplayName("是否使用前置测试结果参数作为驱动电流")]
        //[Description("UseRefData_DrivingCurrent_mA")]
        //[PropEditable(true)]
        //public bool UseRefData_DrivingCurrent_mA { get; set; } = false;

        //[DisplayName("使用前置测试结果参数作为驱动电流,该参数名称")]
        //[Description("RefData_Name_DrivingCurrent_mA")]
        //[PropEditable(true)]
        //public CurrentBase RefData_Name_DrivingCurrent_mA { get; set; } = CurrentBase.Null;
        //[DisplayName("使用前置测试结果参数作为驱动电流时,电流的偏移量(区分正负值)")]
        //[Description("RefData_DrivingCurrent_Offset_mA")]
        //[PropEditable(true)]
        //public double RefData_DrivingCurrent_Offset_mA { get; set; } = 0.0;


        //[DisplayName("使用的默认驱动电流值")]
        //[Description("Default_DrivingCurrent_mA")]
        //[PropEditable(true)]
        //public double Default_DrivingCurrent_mA { get; set; }

        [DisplayName("峰顶扫描起始角度")] 
        [PropEditable(true)]       
        public double PeakSearchStart_Deg { get; set; }
        [DisplayName("峰顶扫描结束角度")]
        [PropEditable(true)]
        public double PeakSearchEnd_Deg { get; set; }
        [DisplayName("峰谷扫描起始角度")]
        [PropEditable(true)]
        public double NullSearchStart_Deg { get; set; }
        [DisplayName("峰谷扫描结束角度")]
        [PropEditable(true)]
        public double NullSearchEnd_Deg { get; set; }
        [DisplayName("扫描角度的步进")]
        [PropEditable(true)]
        public double StepAngle_Deg { get; set; }
        [DisplayName("器件驱动电流(mA)")]
        [PropEditable(true)]
        public double DrivingCurrent_mA { get; set; }
        [DisplayName("偏振态成功门限光功率K值(P = I * K + B)")]
        [PropEditable(true)]
        public double Power_Factor_K { get; set; }
        [DisplayName("偏振态成功门限光功率B值(P = I * K + B)")]
        [PropEditable(true)]
        public double Power_Factor_B { get; set; }
        //public enum CurrentBase
        //{
        //    Null,
        //    Ith1,
        //    Ith2,
        //    Ith3,
        //    // Pop
        //}
    }
}