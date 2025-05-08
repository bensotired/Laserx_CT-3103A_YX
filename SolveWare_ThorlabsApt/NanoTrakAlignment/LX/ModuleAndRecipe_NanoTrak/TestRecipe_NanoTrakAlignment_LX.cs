using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_NanoTrakAlignment_LX : TestRecipeBase
    {
        public TestRecipe_NanoTrakAlignment_LX()
        {
            this.DrivingCurrent_mA = 100.0;
           // this.Threshold_PD_Current_mA = 5;
            this.Power_Factor_K = 1;
            this.Power_Factor_B = 0;
            this.Three_Current_mA_Diff = 0.005;
            this.MoveCount = 10;
        }
        [DisplayName("器件驱动电流(mA)")]
        [PropEditable(true)]
        public double DrivingCurrent_mA { get; set; }
        //[DisplayName("耦合成功门限光电流I(mA)")]
        //[PropEditable(true)]
        //public double Threshold_PD_Current_mA { get; set; }
        [DisplayName("三次耦合成功与平均差值光电流I(mA)")]
        [PropEditable(true)]
        public double Three_Current_mA_Diff { get; set; }
        [DisplayName("耦合时，Y轴单向(向前向后次数一半)移动次数（每次0.001mm）")]
        [PropEditable(true)]
        public double MoveCount { get; set; }

        [DisplayName("耦合成功门限光功率K值(P = I * K + B)")]
        [PropEditable(true)]
        public double Power_Factor_K { get; set; }
        [DisplayName("耦合成功门限光功率B值(P = I * K + B)")]
        [PropEditable(true)]
        public double Power_Factor_B { get; set; }
    }
}